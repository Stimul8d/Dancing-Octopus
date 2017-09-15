using System.Collections.Generic;

namespace DancingOctopus.Domain.Services
{
    public interface IGetEnvironments
    {
        IEnumerable<DeploymentEnvironment> GetAll();
    }
}
