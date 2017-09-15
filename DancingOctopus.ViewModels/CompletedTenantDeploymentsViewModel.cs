using DancingOctopus.Domain;
using DancingOctopus.Domain.Events;
using DancingOctopus.Infrastructure.DomainEvents;
using GalaSoft.MvvmLight;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class CompletedTenantDeploymentsViewModel : ViewModelBase,
        IHandle<TenantDeploymentCompleted>
    {
        public TaskFactory UiFactory { get; private set; }
        public ObservableCollection<CompletedTenantDeploymentViewModel> Tenants { get; private set; }
        public DeploymentStatus Status { get; private set; }

        public BindingCommand ClearAll { get; }

        public CompletedTenantDeploymentsViewModel()
        {
            UiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            Tenants = new ObservableCollection<CompletedTenantDeploymentViewModel>();
            ClearAll = new BindingCommand(() =>
            {
                foreach (var tenantVm in Tenants.ToList())
                {
                    var vm = Tenants.First(v => v.Tenant.Id == tenantVm.Tenant.Id);
                    DomainEvents.Raise(new TenantDeploymentResultAcknowledged(vm.Tenant));
                    Tenants.Remove(vm);
                }
            });
        }

        public void Handle(TenantDeploymentCompleted args)
        {
            UiFactory.StartNew(() =>
            {
                Tenants.Add(new CompletedTenantDeploymentViewModel(args.Tenant, args.Deployments));
            });
        }
    }
}
