using DancingOctopus.Domain.Services;
using System;

namespace DancingOctopus.Data.InMemory
{
    public class CheckInMemoryConnection : ICheckConnections
    {
        public bool CanConnect(Uri server, string apiKey) => true;
    }
}
