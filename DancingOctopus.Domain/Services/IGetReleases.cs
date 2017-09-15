using System.Collections.Generic;

namespace DancingOctopus.Domain.Services
{
    public interface IGetReleases
    {
        IEnumerable<Release> Get(Tenant tenant, DeploymentEnvironment environment);
    }
}
