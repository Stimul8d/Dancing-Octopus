using DancingOctopus.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DancingOctopus.ViewModels
{
    public class CompletedTenantDeploymentViewModel : TenantViewModel
    {
        public ObservableCollection<ProjectViewModel> Projects { get; }
        public DeploymentStatus Status { get; }

        public CompletedTenantDeploymentViewModel(Tenant tenant, IEnumerable<Deployment> deployments) : base(tenant)
        {
            this.Projects = new ObservableCollection<ProjectViewModel>(
                deployments.Select(d => new ProjectViewModel(d.Release.Project) { Status = d.Status, Url=d.Url}));

            if (Projects.All(p => p.Status == DeploymentStatus.NotStarted))
            {
                Status = DeploymentStatus.Failed;
                return;
            }

            this.Status = Projects.All(p => p.Status == DeploymentStatus.NotStarted
                    || p.Status == DeploymentStatus.Successful) ? DeploymentStatus.Successful : DeploymentStatus.Failed;
        }
    }
}
