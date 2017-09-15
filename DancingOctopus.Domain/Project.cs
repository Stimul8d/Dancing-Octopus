namespace DancingOctopus.Domain
{
    public class Project
    {
        public string Id { get; }
        public string Name { get; }

        public Project(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
