using DancingOctopus.Domain.Events;
using DancingOctopus.Infrastructure.DomainEvents;
using GalaSoft.MvvmLight;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Threading.Tasks;
using DancingOctopus.Domain;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class DeploymentQueueViewModel : ViewModelBase,
        IHandle<TenantDeploymentQueued>, IHandle<TenantDeploymentCancelled>,
        IHandle<TenantDeploymentCompleted>, IHandle<ProjectDeploymentStarted>,
        IHandle<ProjectDeploymentCompleted>, IHandle<ProjectDeploymentUpdated>
    {
        public TaskFactory UiFactory { get; private set; }
        public ObservableCollection<DeployingTenantViewModel> QueuedTenants { get; private set; }

        public BindingCommand CancelAll { get; }

        public DeploymentQueueViewModel()
        {
            UiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext()); ;
            QueuedTenants = new ObservableCollection<DeployingTenantViewModel>();

            CancelAll = new BindingCommand(() =>
            {
                foreach (var t in QueuedTenants.ToList())
                    DomainEvents.Raise(new TenantDeploymentCancelled(t.Tenant));
            });
        }

        public void Handle(TenantDeploymentQueued args)
        {
            QueuedTenants.Add(new DeployingTenantViewModel(args.Tenant, args.ProjectsToDeploy));
        }

        public void Handle(TenantDeploymentCancelled args)
        {
            var tenantToRemove = QueuedTenants.First(t => t.Name == args.Tenant.Name);
            QueuedTenants.Remove(tenantToRemove);
        }

        public void Handle(TenantDeploymentCompleted args)
        {
            var tenantToRemove = QueuedTenants.First(t => t.Tenant.Id == args.Tenant.Id);
            UiFactory.StartNew(() => { QueuedTenants.Remove(tenantToRemove); });
        }

        public void Handle(ProjectDeploymentStarted args)
        {
            UpdateProject(args.Deployment);
        }

        public void Handle(ProjectDeploymentCompleted args)
        {
            UpdateProject(args.Deployment);
        }

        private void UpdateProject(Deployment deployment)
        {
            var tenant = this.QueuedTenants.Single(t => t.Tenant.Id == deployment.Tenant.Id);
            var project = tenant.Projects.Single(p => p.Project.Id == deployment.Release.Project.Id);

            project.Status = deployment.Status;
            project.Duration = deployment.Duration;
            project.Url = deployment.Url;
        }

        public void Handle(ProjectDeploymentUpdated args)
        {
            UpdateProject(args.Deployment);
        }
    }
}
