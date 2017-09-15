using DancingOctopus.Domain;
using GalaSoft.MvvmLight;
using PropertyChanged;
using System.Diagnostics;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class ProjectViewModel : ViewModelBase
    {
        public Project Project { get; }
        public string Name { get; private set; }
        public DeploymentStatus Status { get; set; }
        public string Duration { get; set; }
        public string Url { get; set; }

        public BindingCommand OpenTask { get; }

        public ProjectViewModel(Project project)
        {
            this.Project = project;
            this.Name = project.Name;
            this.OpenTask = new BindingCommand(() =>
            {
                if (!string.IsNullOrWhiteSpace(Url))
                    Process.Start(Url);
            });
        }
    }
}
