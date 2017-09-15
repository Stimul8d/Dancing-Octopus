using System;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class ProjectDeploymentUpdated : IDomainEvent
    {
        public Deployment Deployment { get; }

        public string Description => $"{Deployment.Release.Id} updated.";

        public ProjectDeploymentUpdated(Deployment deployment)
        {
            this.Deployment = deployment;
        }
    }
}
