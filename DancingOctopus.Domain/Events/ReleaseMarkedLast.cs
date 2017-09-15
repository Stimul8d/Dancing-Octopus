using System;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class ReleaseMarkedLast : IDomainEvent
    {
        public Release Release { get; }

        public string Description => $"{Release.Name} marked last.";

        public ReleaseMarkedLast(Release release)
        {
            this.Release = release;
        }
    }
}
