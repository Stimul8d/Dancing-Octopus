using System;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class TenantDeploymentResultAcknowledged : IDomainEvent
    {
        public Tenant Tenant { get; }

        public string Description => $"{Tenant.Name} deployment acknowledged.";

        public TenantDeploymentResultAcknowledged(Tenant tenant) => this.Tenant = tenant;
    }
}
