using System;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class DeploymentConfirmed : IDomainEvent
    {
        public string Description => "Deployment Confirmed";
    }
}
