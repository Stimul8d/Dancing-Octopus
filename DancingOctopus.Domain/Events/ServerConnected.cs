using DancingOctopus.Infrastructure.DomainEvents;
using System;

namespace DancingOctopus.Domain.Events
{
    public class ServerConnected : IDomainEvent
    {
        public Uri Server { get; private set; }
        public string ApiKey { get; private set; }

        public string Description => "Server connected.";

        public ServerConnected(Uri server, string apiKey)
        {
            this.Server = server;
            this.ApiKey = apiKey;
        }

    }
}
