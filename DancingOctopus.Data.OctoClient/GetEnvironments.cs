using DancingOctopus.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DancingOctopus.Domain;
using Octopus.Client;
using DancingOctopus.Domain.Events;
using DancingOctopus.Infrastructure.DomainEvents;

namespace DancingOctopus.Data.OctoClient
{
    public class GetEnvironments : IGetEnvironments, IHandle<ServerConnected>
    {
        private IOctopusRepository repo;

        public GetEnvironments(ConnectionDetails conn)
        {
            this.repo = new OctopusRepository(new OctopusServerEndpoint(conn.Server, conn.ApiKey));
        }

        public IEnumerable<DeploymentEnvironment> GetAll()
        {
            return repo.Environments.GetAll()
                .Select(e => new DeploymentEnvironment(e.Id, e.Name));
        }

        public void Handle(ServerConnected args)
        {
            var endPoint = new OctopusServerEndpoint(args.Server.AbsolutePath, args.ApiKey);
            repo = new OctopusRepository(endPoint);
        }
    }
}
