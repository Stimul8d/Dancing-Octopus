using System.Collections.Generic;
using System.Diagnostics;

namespace DancingOctopus.Domain
{
    [DebuggerDisplay("{Id}:{Name}")]
    public class Tenant
    {
        public string Name { get; }
        public string ImagePath { get; }
        public string Id { get; }

        public List<Tag> Tags { get; }
        public IEnumerable<KeyValuePair<string, string>> SomeProperties { get; }

        public Tenant(string id,string name, string imagePath, IEnumerable<Tag> tags, IEnumerable<KeyValuePair<string, string>> someProps)
        {
            this.Id = id;
            this.Name = name;
            this.ImagePath = imagePath;
            this.Tags = new List<Tag>(tags);
            this.SomeProperties = someProps;
        }
    }
}
