using System.Collections.Generic;

namespace DancingOctopus.Domain.Services
{
    public interface IGetTenantProjects
    {
        IEnumerable<Project> GetProjects(Tenant tenant);
    }
}
