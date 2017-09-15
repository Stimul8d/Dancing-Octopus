using DancingOctopus.Domain;
using DancingOctopus.Domain.Events;
using DancingOctopus.Domain.Services;
using DancingOctopus.Infrastructure.DomainEvents;
using Octopus.Client;
using Octopus.Client.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DancingOctopus.Data.OctoClient
{
    public class Promoter : IHandle<TenantDeploymentQueued>
    {
        List<Deployment> deploymentsToDo = new List<Deployment>();
        List<Deployment> completedDeployments = new List<Deployment>();

        private ConnectionDetails connectionDetails;
        private OctopusRepository repo;
        private IGetServerStatus serverStatus;

        public Promoter(ConnectionDetails conn, IGetServerStatus serverStatus)
        {
            this.connectionDetails = conn;
            this.serverStatus = serverStatus;
            this.repo = new OctopusRepository(new OctopusServerEndpoint(conn.Server, conn.ApiKey));
        }

        public void Handle(TenantDeploymentQueued args)
        {
            Task.Run(() =>
            {
                if (args.FirstRelease != null
                    && args.ProjectsToDeploy.Any(p => p.Id == args.FirstRelease.Project.Id))
                    Deploy(args.Tenant, args.FirstRelease, args.DestinationEnvironment); ;
            })
            .ContinueWith(t =>
            {
                Parallel.ForEach(args.Releases, (r) =>
                {
                    if (args.ProjectsToDeploy.Any(p => r.Project.Id == p.Id)
                        && AllPrereqsAndStartedSucceeded(args.Tenant, args.FirstRelease))
                        Deploy(args.Tenant, r, args.DestinationEnvironment);
                });
            })
            .ContinueWith(t =>
            {
                if (args.LastRelease != null
                    && args.ProjectsToDeploy.Any(p => args.LastRelease.Project.Id == p.Id)
                    && AllPrereqsAndStartedSucceeded(args.Tenant, args.FirstRelease))
                    Deploy(args.Tenant, args.LastRelease, args.DestinationEnvironment);
            })
            .ContinueWith(t =>
            {
                var allDeployments = completedDeployments.Concat(deploymentsToDo);

                DomainEvents.Raise(new TenantDeploymentCompleted(args.Tenant, allDeployments,
                                        AllPrereqsAndStartedSucceeded(args.Tenant, args.FirstRelease)));

                deploymentsToDo.RemoveAll(r => r.Complete);
            });
        }

        private bool AllPrereqsAndStartedSucceeded(Tenant tenant, Release firstRelease)
        {
            if (!completedDeployments.Any()) return true;

            if (firstRelease != null && completedDeployments.Any(d => firstRelease.Project.Id == d.Release.Project.Id))
            {
                var firstDeployment = completedDeployments.Single(d => firstRelease.Id == d.Release.Id);

                if (firstDeployment.Status != DeploymentStatus.Successful) return false;
            }

            return completedDeployments.All(d => d.Status == DeploymentStatus.Successful
                  || d.Status == DeploymentStatus.NotStarted);
        }

        public void Deploy(Tenant tenant, Release release, DeploymentEnvironment targetEnv)
        {
            var deployment = new Deployment(tenant, release);
            deploymentsToDo.Add(deployment);

            try
            {
                var job = repo.Deployments.Create(new DeploymentResource
                {
                    TenantId = tenant.Id,
                    EnvironmentId = targetEnv.Id,
                    ReleaseId = release.Id,
                });

                deployment.Status = DeploymentStatus.InProgress;
                DomainEvents.Raise(new ProjectDeploymentStarted(deployment, release.Project, tenant.Id, job.TaskId));

                //var task = repo.Tasks.Get(job.TaskId);
                var task = serverStatus.GetTaskStatus(job.TaskId);
                while (!task.IsCompleted)
                {
                    Task.Delay(5000).Wait();
                    task = serverStatus.GetTaskStatus(job.TaskId);
                    deployment.Duration = task.Duration;
                    deployment.Url = $"{connectionDetails.Server}/app#/tasks/{task.Id}";
                    DomainEvents.Raise(new ProjectDeploymentUpdated(deployment));
                }
                deployment.Complete = true;
                deployment.Status = task.Status;
            }
            catch
            {
                deployment.Status = DeploymentStatus.Failed;
                deployment.Complete = true;
            }
            finally
            {
                DomainEvents.Raise(new ProjectDeploymentCompleted(deployment));
                deploymentsToDo.Remove(deployment);
                completedDeployments.Add(deployment);
            }
        }
    }
}
