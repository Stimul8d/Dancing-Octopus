using System;
using DancingOctopus.Domain;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class TenantUnstaged : IDomainEvent
    {
        public Tenant Tenant { get; }

        public string Description => $"{Tenant.Name} Unstaged";

        public TenantUnstaged(Tenant tenant)
        {
            this.Tenant = tenant;
        }

    }
}
