namespace DancingOctopus.Domain
{
    public class Tag
    {
        public string Name { get; }
        public string Color { get; }

        public Tag(string name, string color)
        {
            this.Name = name;
            this.Color = color;
        }
    }
}
