namespace DancingOctopus.Domain
{
    public class Release
    {
        public Tenant SourceTenant { get; }
        public Project Project { get; private set; }
        public DeploymentEnvironment Environment { get; }
        public string Id { get; }
        public string Name { get; }

        public Release(Tenant sourceTenant, Project project, DeploymentEnvironment environment, string id, string name)
        {
            this.SourceTenant = sourceTenant;
            this.Project = project;
            this.Environment = environment;
            this.Id = id;
            this.Name = name;
        }
    }
}
