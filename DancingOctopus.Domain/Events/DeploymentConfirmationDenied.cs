using System;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class DeploymentConfirmationDenied : IDomainEvent
    {
        public string Description => "Deployment Denied";
    }
}
