using DancingOctopus.Data.InMemory;
using DancingOctopus.Data.OctoClient;
using DancingOctopus.Domain;
using DancingOctopus.Domain.Events;
using DancingOctopus.Domain.Services;
using DancingOctopus.Infrastructure.DomainEvents;
using DancingOctopus.ViewModels;
using StructureMap;
using System.Collections.Generic;

namespace DancingOctopus.Composition
{
    public class StructureMapContainer : IContainer
    {
        private Container container;

        public StructureMapContainer(ConnectionDetails connectionDetails)
        {
            this.container = new Container(c =>
            {
                c.ForSingletonOf<ApplicationLogViewModel>().Use<ApplicationLogViewModel>();
                c.For<IHandle<IDomainEvent>>().Use(ctx => ctx.GetInstance<ApplicationLogViewModel>());

                c.For<ConnectionDetails>().Use(ctx => connectionDetails);

                c.ForSingletonOf<IGetServerStatus>().Use<GetServerStatus>();

                c.For<IGetTenants>().Use<GetTenants>();
                c.For<ICheckConnections>().Use<CheckInMemoryConnection>();
                c.For<IGetEnvironments>().Use<GetEnvironments>();
                c.For<IGetTenantProjects>().Use<GetTenantProjects>();
                c.For<IGetReleases>().Use<GetReleases>();

                c.For<IHandle<TenantDeploymentQueued>>().Use<Promoter>();

                c.ForSingletonOf<CurrentContentViewModel>().Use<CurrentContentViewModel>();
                c.For<IHandle<ApplicationStarted>>().Use(ctx => ctx.GetInstance<CurrentContentViewModel>());
                c.For<IHandle<ServerConnected>>().Use(ctx => ctx.GetInstance<CurrentContentViewModel>());
                c.For<IHandle<DeploymentRequiresConfirmation>>().Use(ctx => ctx.GetInstance<CurrentContentViewModel>());
                c.For<IHandle<DeploymentConfirmationDenied>>().Use(ctx => ctx.GetInstance<CurrentContentViewModel>());
                c.For<IHandle<DeploymentConfirmed>>().Use(ctx => ctx.GetInstance<CurrentContentViewModel>());

                c.ForSingletonOf<ServerStatusViewModel>().Use<ServerStatusViewModel>();
                c.For<IHandle<ServerConnected>>().Use(ctx => ctx.GetInstance<ServerStatusViewModel>());

                c.ForSingletonOf<TenantSelectionViewModel>().Use<TenantSelectionViewModel>();
                c.For<IHandle<ServerConnected>>().Use(ctx => ctx.GetInstance<TenantSelectionViewModel>());
                c.For<IHandle<TenantStaged>>().Use(ctx => ctx.GetInstance<TenantSelectionViewModel>());
                c.For<IHandle<TenantUnstaged>>().Use(ctx => ctx.GetInstance<TenantSelectionViewModel>());
                c.For<IHandle<TenantDeploymentCancelled>>().Use(ctx => ctx.GetInstance<TenantSelectionViewModel>());
                c.For<IHandle<TenantDeploymentResultAcknowledged>>().Use(ctx => ctx.GetInstance<TenantSelectionViewModel>());

                c.ForSingletonOf<StagedTenantsViewModel>().Use<StagedTenantsViewModel>();
                c.For<IHandle<TenantStaged>>().Use(ctx => ctx.GetInstance<StagedTenantsViewModel>());
                c.For<IHandle<TenantUnstaged>>().Use(ctx => ctx.GetInstance<StagedTenantsViewModel>());
                c.For<IHandle<TenantDeploymentQueued>>().Use(ctx => ctx.GetInstance<StagedTenantsViewModel>());
                c.For<IHandle<ReleaseMarkedFirst>>().Use(ctx => ctx.GetInstance<StagedTenantsViewModel>());
                c.For<IHandle<ReleaseMarkedLast>>().Use(ctx => ctx.GetInstance<StagedTenantsViewModel>());

                c.ForSingletonOf<DeploymentQueueViewModel>().Use<DeploymentQueueViewModel>();
                c.For<IHandle<TenantDeploymentQueued>>().Use(ctx => ctx.GetInstance<DeploymentQueueViewModel>());
                c.For<IHandle<TenantDeploymentCancelled>>().Use(ctx => ctx.GetInstance<DeploymentQueueViewModel>());
                c.For<IHandle<TenantDeploymentCompleted>>().Use(ctx => ctx.GetInstance<DeploymentQueueViewModel>());
                c.For<IHandle<ProjectDeploymentStarted>>().Use(ctx => ctx.GetInstance<DeploymentQueueViewModel>());
                c.For<IHandle<ProjectDeploymentCompleted>>().Use(ctx => ctx.GetInstance<DeploymentQueueViewModel>());
                c.For<IHandle<ProjectDeploymentUpdated>>().Use(ctx => ctx.GetInstance<DeploymentQueueViewModel>());

                c.ForSingletonOf<CompletedTenantDeploymentsViewModel>().Use<CompletedTenantDeploymentsViewModel>();
                c.For<IHandle<TenantDeploymentCompleted>>().Use(ctx => ctx.GetInstance<CompletedTenantDeploymentsViewModel>());

            });

            DomainEvents.Container = container;
        }
        public IEnumerable<T> GetAllInstances<T>()
        {
            return container.GetAllInstances<T>();
        }
        public T GetInstance<T>()
        {
            return container.GetInstance<T>();
        }
    }
}
