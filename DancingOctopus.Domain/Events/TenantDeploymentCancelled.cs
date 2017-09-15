using System;
using DancingOctopus.Domain;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class TenantDeploymentCancelled : IDomainEvent
    {
        public Tenant Tenant { get; private set; }

        public string Description => $"{Tenant.Name} deployment cancelled";

        public TenantDeploymentCancelled(Tenant tenant)
        {
            this.Tenant = tenant;
        }

    }
}
