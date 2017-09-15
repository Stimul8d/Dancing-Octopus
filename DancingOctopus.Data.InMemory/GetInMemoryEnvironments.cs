using System.Collections.Generic;
using DancingOctopus.Domain;
using DancingOctopus.Domain.Services;
using Bogus;
using System.Linq;

namespace DancingOctopus.Data.InMemory
{
    public class GetInMemoryEnvironments : IGetEnvironments
    {
        private Faker faker = new Faker();

        public IEnumerable<DeploymentEnvironment> GetAll() =>
            Enumerable.Range(5, faker.Random.Number(5, 10))
            .Select(x => new DeploymentEnvironment(faker.UniqueIndex.ToString(), faker.Commerce.Department()));
    }
}
