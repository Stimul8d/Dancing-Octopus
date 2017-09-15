using System;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Domain.Events
{
    public class ReleaseMarkedFirst : IDomainEvent
    {
        public Release Release { get; }

        public string Description => $"{Release.Name} marked first";

        public ReleaseMarkedFirst(Release release)
        {
            this.Release = release;
        }
    }

}
