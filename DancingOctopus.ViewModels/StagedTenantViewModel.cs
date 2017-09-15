using DancingOctopus.Domain;
using DancingOctopus.Domain.Events;
using DancingOctopus.Infrastructure.DomainEvents;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DancingOctopus.ViewModels
{
    public class StagedTenantViewModel : TenantViewModel
    {
        public BindingCommand UnstageForDeployment { get; }
        public ObservableCollection<Project> Projects { get; }

        public StagedTenantViewModel(Tenant tenant) : base(tenant)
        {
            this.Projects = new ObservableCollection<Project>();
            UnstageForDeployment = new BindingCommand(() => DomainEvents.Raise(new TenantUnstaged(tenant)));
        }

        public void UpdateProjects(IEnumerable<Project> projects)
        {
            this.Projects.Clear();
            foreach (var project in projects) Projects.Add(project);
        }
    }
}
