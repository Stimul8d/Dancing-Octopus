namespace DancingOctopus.Domain
{
    public class DeploymentEnvironment
    {
        public string Name { get; }
        public string Id { get; }

        public DeploymentEnvironment(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
