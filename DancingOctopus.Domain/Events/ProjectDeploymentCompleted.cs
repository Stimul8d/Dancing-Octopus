using System;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class ProjectDeploymentCompleted : IDomainEvent
    {
        public Deployment Deployment { get; }

        public string Description => $"{Deployment.Status} {Deployment.Tenant} - {Deployment.Release.Project.Name} completed.";

        public ProjectDeploymentCompleted(Deployment deployment)
        {
            this.Deployment = deployment;

        }
    }
}
