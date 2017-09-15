using DancingOctopus.Domain;
using DancingOctopus.Infrastructure.DomainEvents;
using System.Collections.Generic;
using System;

namespace DancingOctopus.Domain.Events
{
    public class TenantDeploymentQueued : IDomainEvent
    {
        public Tenant Tenant { get; }
        public IEnumerable<Project> ProjectsToDeploy { get; }
        public Tenant SourceTenant { get; }
        public DeploymentEnvironment SourceEnvironment { get; }
        public DeploymentEnvironment DestinationEnvironment { get; }
        public Release FirstRelease { get; }
        public IEnumerable<Release> Releases { get; }
        public Release LastRelease { get; }

        public string Description => $"{Tenant.Name} deployment queued.";

        public TenantDeploymentQueued(Tenant tenant, IEnumerable<Project> projects, Release firstRelease, IEnumerable<Release> releases, Release lastRelease, Tenant sourceTenant, DeploymentEnvironment source, DeploymentEnvironment dest)
        {
            this.Tenant = tenant;
            this.ProjectsToDeploy = projects;
            this.FirstRelease = firstRelease;
            this.Releases = releases;
            this.LastRelease = lastRelease;
            this.SourceTenant = sourceTenant;
            this.SourceEnvironment = source;
            this.DestinationEnvironment = dest;
        }
    }
}
