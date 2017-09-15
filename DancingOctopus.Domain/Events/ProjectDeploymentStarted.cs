using System;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class ProjectDeploymentStarted : IDomainEvent
    {
        public Project Project { get; }
        public string TaskId { get; }
        public string TenantId { get; }

        public string Description => $"{Project.Name} started.";

        public Deployment Deployment { get; }

        public ProjectDeploymentStarted(Deployment deployment, Project project, string tenantId, string taskId)
        {
            this.Deployment = deployment;
            this.TenantId = tenantId;
            this.Project = project;
            this.TaskId = taskId;
        }
    }
}
