using System.Collections.Generic;
using System.Linq;
using System;
using DancingOctopus.Domain;
using Bogus;
using DancingOctopus.Domain.Services;

namespace DancingOctopus.Data.InMemory
{
    public class GetInMemoryTenants : IGetTenants
    {
        private string[] colors = new[] { "#983230", "#E8634F", "#A77B22", "#ECAD3F", "#227647", "#36A766", "#52818C", "#77B7C5", "#203A88", "#3156B3", "#752BA5", "#9786A7", "#6e6e6e", "#9d9d9d" };

        private Faker faker = new Faker();
        private string ImagePath = Environment.CurrentDirectory + @"/Assets/Club.png";

        private IEnumerable<Tag> Tags() =>
            Enumerable.Range(2, faker.Random.Number(2, 6)).Select(x => new Tag(faker.Random.Word(), faker.PickRandom(colors)));

        public IEnumerable<Tenant> GetAll() => Enumerable.Range(5, faker.Random.Number(5, 10))
                .Select(x => new Tenant(x.ToString(), faker.Company.CompanyName(),
                    $"{faker.Image.Sports()}?cb={faker.Random.Int()}",
                    Tags(), null)).OrderBy(x => x.Name);
    }
}
