using System;
using DancingOctopus.Domain;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class TenantStaged : IDomainEvent
    {
        public Tenant Tenant { get; private set; }

        public string Description => $"{Tenant.Name} staged.";

        public TenantStaged(Tenant tenant)
        {
            this.Tenant = tenant;
        }
    }
}
