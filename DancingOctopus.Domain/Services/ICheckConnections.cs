using System;

namespace DancingOctopus.Domain.Services
{
    public interface ICheckConnections
    {
        bool CanConnect(Uri server, string apiKey);
    }
}
