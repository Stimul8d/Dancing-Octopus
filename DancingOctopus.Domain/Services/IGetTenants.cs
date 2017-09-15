using System.Collections.Generic;

namespace DancingOctopus.Domain.Services
{
    public interface IGetTenants
    {
        IEnumerable<Tenant> GetAll();
    }
}
