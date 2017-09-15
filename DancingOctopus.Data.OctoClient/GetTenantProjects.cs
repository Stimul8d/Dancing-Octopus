using DancingOctopus.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using DancingOctopus.Domain;
using Octopus.Client;

namespace DancingOctopus.Data.OctoClient
{
    public class GetTenantProjects : IGetTenantProjects
    {
        private OctopusRepository repo;

        public GetTenantProjects(ConnectionDetails conn)
        {
            this.repo = new OctopusRepository(new OctopusServerEndpoint(conn.Server, conn.ApiKey));
        }

        public IEnumerable<Project> GetProjects(Tenant tenant)
        {
            var octoTenant = repo.Tenants.Get(tenant.Id);
            var vars = repo.Tenants.GetVariables(octoTenant);
            if(vars!=null) return vars .ProjectVariables.Select(pv => new Project(pv.Value.ProjectId, pv.Value.ProjectName));
            return Enumerable.Empty<Project>();
        }
    }
}
