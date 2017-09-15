using System;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class DeploymentRequiresConfirmation : IDomainEvent
    {
        public string Description => "Requesting Deployment";
    }
}
