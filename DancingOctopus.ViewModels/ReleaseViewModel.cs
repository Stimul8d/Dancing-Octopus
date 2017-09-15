using DancingOctopus.Domain;
using DancingOctopus.Domain.Events;
using DancingOctopus.Infrastructure.DomainEvents;
using PropertyChanged;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class ReleaseViewModel
    {
        public Release Release { get; }

        public string Name { get; private set; }

        public bool IsFirst { get; set; }
        public bool IsMiddle { get; set; }
        public bool IsLast { get; set; }

        public BindingCommand MakeFirst { get; }
        public BindingCommand MakeLast { get; }

        public ReleaseViewModel(Release release)
        {
            this.Release = release;
            this.Name = $"{release.Project.Name} : {release.Name}";
            this.IsMiddle = true;
            this.MakeFirst = new BindingCommand(() => { DomainEvents.Raise(new ReleaseMarkedFirst(release)); });
            this.MakeLast = new BindingCommand(() => { DomainEvents.Raise(new ReleaseMarkedLast(release)); });
        }
    }
}
