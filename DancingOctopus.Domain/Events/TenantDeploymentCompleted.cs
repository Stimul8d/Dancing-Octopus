using DancingOctopus.Infrastructure.DomainEvents;
using System.Collections.Generic;
using System;

namespace DancingOctopus.Domain.Events
{
    public class TenantDeploymentCompleted : IDomainEvent
    {
        public Tenant Tenant { get; }
        public bool Successful { get; }
        public IEnumerable<Deployment> Deployments { get; }

        public string Description => $"{Tenant.Name} deployment completed.";

        public TenantDeploymentCompleted(Tenant tenant, IEnumerable<Deployment> deployments, bool successful)
        {
            this.Tenant = tenant;
            this.Deployments = deployments;
            this.Successful = successful;
        }
    }
}
