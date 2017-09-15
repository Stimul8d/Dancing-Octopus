using DancingOctopus.Infrastructure.DomainEvents;
using GalaSoft.MvvmLight;
using PropertyChanged;
using System.Collections.ObjectModel;
using DancingOctopus.Domain;
using System.Linq;
using System.Collections.Generic;
using DancingOctopus.Domain.Services;
using DancingOctopus.Domain.Events;
using System.Threading.Tasks;
using System;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class TenantSelectionViewModel : ViewModelBase,
        IHandle<ServerConnected>, IHandle<TenantStaged>, IHandle<TenantUnstaged>,
        IHandle<TenantDeploymentCancelled>, IHandle<TenantDeploymentResultAcknowledged>
    {
        private readonly IGetTenants getTenants;
        private HashSet<Tenant> allTenants;

        public BindingCommand StageSelected { get; }

        public ObservableCollection<TenantViewModel> InScopeTenants { get; private set; }

        public string TenantCountTitle { get; private set; }

        private string include;

        public string Include
        {
            get { return include; }
            set
            {
                include = value;
                FilterTenants();
                SetTitle();
            }
        }

        private string exclude;

        public string Exclude
        {
            get { return exclude; }
            set
            {
                exclude = value;
                FilterTenants();
                SetTitle();
            }
        }

        public TaskFactory UiFactory { get; private set; }

        public TenantSelectionViewModel(IGetTenants getTenants)
        {
            this.UiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            this.getTenants = getTenants;
            this.TenantCountTitle = "Tenants (0)";
            this.StageSelected = new BindingCommand(
               () =>
               {
                   foreach (var t in InScopeTenants.ToList()) DomainEvents.Raise(new TenantStaged(t.Tenant));
                   ResetFilters();
               });
        }


        public void Handle(ServerConnected args)
        {
            allTenants = new HashSet<Tenant>(getTenants.GetAll().ToList());
            InScopeTenants = new ObservableCollection<TenantViewModel>(
                allTenants.Select(x => new TenantViewModel(x)));
            SetTitle();
            SortTenants();
        }

        public void Handle(TenantStaged args)
        {
            var tenantToRemove = InScopeTenants.First(t => t.Name == args.Tenant.Name);
            InScopeTenants.Remove(tenantToRemove);
            allTenants.Remove(tenantToRemove.Tenant);
            SortTenants();
            SetTitle();
        }


        public void Handle(TenantUnstaged args)
        {
            AddTenant(args.Tenant);
        }

        public void Handle(TenantDeploymentCancelled args)
        {
            AddTenant(args.Tenant);
        }

        public void Handle(TenantDeploymentResultAcknowledged args)
        {
            AddTenant(args.Tenant);
        }

        private void AddTenant(Tenant tenant)
        {
            InScopeTenants.Add(new TenantViewModel(tenant));
            allTenants.Add(tenant);
            SetTitle();
            SortTenants();
        }

        private void FilterTenants()
        {
            IEnumerable<Tenant> filtered = allTenants;
            IEnumerable<Tenant> included = allTenants;
            IEnumerable<Tenant> excluded = Enumerable.Empty<Tenant>();

            Task.Run(() =>
            {
                if (!string.IsNullOrWhiteSpace(include))
                {
                    var includeWords = include.Split(',').Select(s => s.Trim()).ToList();
                    included = allTenants.Where(t => includeWords.All(w =>
                                        t.Tags.Any(tag => tag.Name.ToUpper().Contains(w.ToUpper()))
                                                       || t.Name.ToUpper().Contains(w.ToUpper())));
                    filtered = included;
                }

                if (!string.IsNullOrWhiteSpace(exclude))
                {
                    var excludeWords = exclude.Trim().Split(',').Select(s => s.Trim())
                        .Where(s=> !string.IsNullOrWhiteSpace(s)).ToList();

                    excluded = allTenants.Where(t => excludeWords.Any(w =>
                                        t.Tags.Any(tag => tag.Name.ToUpper().Contains(w.ToUpper()))
                                                       || t.Name.ToUpper().Contains(w.ToUpper())));
                    filtered = included.Except(excluded);
                }

                filtered = filtered.OrderBy(t => t.Name);
            })
            .ContinueWith((t) =>
            {
                UiFactory.StartNew(() =>
                {
                    InScopeTenants.Clear();
                    foreach (var tenant in filtered) InScopeTenants.Add(new TenantViewModel(tenant));
                    SetTitle();
                });
            });
        }

        private void SortTenants()
        {
            var sorted = InScopeTenants.OrderBy(t => t.Name).ToList();
            InScopeTenants.Clear();
            foreach (var tenant in sorted) InScopeTenants.Add(tenant);
        }

        private void ResetFilters()
        {
            Include = "";
            Exclude = "";
        }

        private void SetTitle()
        {
            TenantCountTitle = $"Tenants ({InScopeTenants.Count()})";
        }
    }
}
