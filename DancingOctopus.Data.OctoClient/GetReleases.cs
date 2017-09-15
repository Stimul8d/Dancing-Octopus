using DancingOctopus.Domain.Services;
using System;
using DancingOctopus.Domain;
using Octopus.Client;
using System.Linq;
using System.Collections.Generic;

namespace DancingOctopus.Data.OctoClient
{
    public class GetReleases : IGetReleases
    {
        private IOctopusRepository repo;
        private IGetTenantProjects getProjects;

        public GetReleases(IGetTenantProjects getProjects, ConnectionDetails conn)
        {
            this.repo = new OctopusRepository(new OctopusServerEndpoint(conn.Server, conn.ApiKey));
            this.getProjects = getProjects;
        }

        public IEnumerable<Release> Get(Tenant tenant, DeploymentEnvironment environment)
        {
            if (tenant == null || environment == null) return Enumerable.Empty<Release>();
            var projects = getProjects.GetProjects(tenant);
            var projectIds = projects.Select(p => p.Id);
            var dash = repo.Dashboards.GetDynamicDashboard(projectIds.ToArray(), new[] { environment.Id });
            return dash.Items.Where(i => i.TenantId == tenant.Id)
                        .Select(i => new Release(tenant, projects.Single(p => p.Id == i.ProjectId),
                        environment, i.ReleaseId, i.ReleaseVersion));
        }
    }
}
