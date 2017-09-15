using System;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class ApplicationStarted : IDomainEvent
    {
        public string Description => "Application Started";
    }
}
