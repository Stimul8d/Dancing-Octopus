using DancingOctopus.Domain.Services;
using Octopus.Client;
using System.Linq;
using DancingOctopus.Domain;
using System.Collections.Generic;

namespace DancingOctopus.Data.OctoClient
{
    public class GetTenants : IGetTenants
    {
        private OctopusRepository repo;
        private readonly ConnectionDetails connectionDetails;

        public GetTenants(ConnectionDetails conn)
        {
            this.connectionDetails = conn;
            this.repo = new OctopusRepository(new OctopusServerEndpoint(conn.Server, conn.ApiKey));
        }

        public IEnumerable<Tenant> GetAll()
        {
            var tags = repo.TagSets.GetAll().SelectMany(t => t.Tags)
                .Select(t => new
                {
                    Set = t.CanonicalTagName.Split('/')[0],
                    Name = t.CanonicalTagName.Split('/')[1],
                    Color = t.Color
                });

            return repo.Tenants.GetAll().Select(t =>
            {
                List<KeyValuePair<string, string>> tenantVars = new List<KeyValuePair<string, string>>();
                var variables = repo.Tenants.GetVariables(t);
                var projects = variables.ProjectVariables.Where(p => p.Value.ProjectName.Contains("SSETI - Venuemaster")
                                                               || p.Value.ProjectName.Contains("Pro UK")).Select(x => x.Value);
                tenantVars.Add(new KeyValuePair<string, string>("Name", t.Name));
                //
                // THIS POPULATES SOME PROPERTIES IF YOU CAN BE BOTHERED
                //
                //if (projects.Any())
                //{
                //    try
                //    {
                //        var skin = variables.LibraryVariables.ToList().FirstOrDefault().Value.Variables.ToList()[0].Value.Value;
                //        tenantVars.Add(new KeyValuePair<string, string>("Skin", skin));

                //        var url = variables.LibraryVariables.ToList().Last().Value.Variables.Last().Value.Value;
                //        tenantVars.Add(new KeyValuePair<string, string>("Url", url));

                //    }
                //    catch
                //    {
                //        //whatever 
                //    }

                //    var envs = repo.Environments.GetAll();
                //    foreach (var p in projects)
                //    {
                //        List<string> vars = new List<string>();
                //        var env = p.Variables.SingleOrDefault(v => v.Key == "Environments-24");
                //        if (env.Value != null) vars.AddRange(env.Value.Select(x => x.Value.Value));
                //        var templates = p.Templates.Select(x => x.Label);
                //        var combined = templates.Zip(vars, (a, b) => new KeyValuePair<string, string>(a, b));
                //        var interests = new string[] { "Tenant App Pool User", "Cache Prefix Client Name", "Listen Port", "Database Name" };
                //        var interesting = combined.Where(x => interests.Contains(x.Key));
                //        tenantVars.AddRange(interesting);
                //    }
                //}

                return new Tenant(t.Id, t.Name,
                    connectionDetails.Server + t.Links["Logo"] + "?apiKey=" + connectionDetails.ApiKey,
                    t.TenantTags.Select(tag =>
                    {
                        var setToMatch = tag.Split('/')[0];
                        var tagToMatch = tag.Split('/')[1];

                        var tagset = tags.First(x => x.Set == setToMatch && x.Name == tagToMatch);
                        return new Tag(tagset.Name, tagset.Color);
                    }), tenantVars);
            });
        }
    }
}
