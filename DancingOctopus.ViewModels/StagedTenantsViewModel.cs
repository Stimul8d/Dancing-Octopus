using DancingOctopus.Infrastructure.DomainEvents;
using GalaSoft.MvvmLight;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using DancingOctopus.Domain;
using DancingOctopus.Domain.Events;
using DancingOctopus.Domain.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class StagedTenantsViewModel : ViewModelBase,
        IHandle<TenantStaged>, IHandle<TenantUnstaged>, IHandle<TenantDeploymentQueued>,
        IHandle<ReleaseMarkedFirst>, IHandle<ReleaseMarkedLast>
    {
        private readonly IGetEnvironments getEnvironments;
        private readonly IGetTenantProjects getProjects;
        private IGetReleases getReleases;

        public ObservableCollection<StagedTenantViewModel> StagedTenants { get; private set; }

        public ObservableCollection<Tenant> AllTenants { get; }

        public Tenant SourceTenant { get; set; }

        public ObservableCollection<DeploymentEnvironment> Environments { get; private set; }
        public DeploymentEnvironment SourceEnvironment { get; set; }
        public ObservableCollection<ReleaseViewModel> SourceReleases { get; set; }
        public DeploymentEnvironment DestinationEnvironment { get; set; }

        public BindingCommand ClearTenants { get; }
        public BindingCommand RequestConfirmation { get; }
        public BindingCommand CancelConfirmation { get; }
        public BindingCommand DeployTenants { get; }

        public TaskFactory UiFactory { get; private set; }

        public StagedTenantsViewModel(IGetEnvironments getEnvironments, IGetReleases getReleases,
            IGetTenantProjects getProjects, IGetTenants getTenants)
        {
            this.UiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            this.getProjects = getProjects;
            this.getEnvironments = getEnvironments;
            this.getReleases = getReleases;

            this.AllTenants = new ObservableCollection<Tenant>(getTenants.GetAll());

            //Console.WriteLine("Name, Skin, Url, Listen Port, Database Name, Tenant App Pool User, Cache Prefix Client Name");
            //foreach (var t in AllTenants)
            //{
            //    foreach (var p in t.SomeProperties)
            //    {
            //        Console.Write(p.Value + ", ");
            //    }
            //    Console.WriteLine();
            //}

            this.SourceTenant = AllTenants.First();
            this.SourceReleases = new ObservableCollection<ReleaseViewModel>();

            IEnumerable<DeploymentEnvironment> env = null;
            Task.Run(() =>
            {
                env = getEnvironments.GetAll().ToList();
            })
            .ContinueWith(t =>
            {
                UiFactory.StartNew(() =>
                {
                    this.Environments = new ObservableCollection<DeploymentEnvironment>(env);
                    var proUAT = this.Environments.FirstOrDefault(e => e.Name.Contains("TicketmasterPro-UAT"));
                    this.SourceEnvironment = proUAT;
                    this.DestinationEnvironment = proUAT;
                });
            });

            StagedTenants = new ObservableCollection<StagedTenantViewModel>();

            this.ClearTenants = new BindingCommand(() =>
            {
                foreach (var t in StagedTenants.ToList()) DomainEvents.Raise(new TenantUnstaged(t.Tenant));
            },
            () => StagedTenants.Count > 0);

            this.RequestConfirmation = new BindingCommand(
                () =>
                {
                    var releases = getReleases.Get(SourceTenant, SourceEnvironment).Select(r => new ReleaseViewModel(r));

                    SourceReleases.Clear();
                    foreach (var release in releases) SourceReleases.Add(release);

                    DomainEvents.Raise(new DeploymentRequiresConfirmation());
                },
                () => StagedTenants.Count > 0);

            this.DeployTenants = new BindingCommand(() =>
            {
                DomainEvents.Raise(new DeploymentConfirmed());
                var firstRelease = SourceReleases.SingleOrDefault(r => r.IsFirst)?.Release;
                var lastRelease = SourceReleases.SingleOrDefault(r => r.IsLast)?.Release;
                var middleReleases = SourceReleases.Where(r => r.IsMiddle).Select(r => r?.Release);

                //remove the unattached projects.
                var attachedProjects = new List<Project>();
                if (firstRelease != null) attachedProjects.Add(firstRelease.Project);
                foreach (var rel in middleReleases) attachedProjects.Add(rel.Project);
                if (lastRelease != null) attachedProjects.Add(lastRelease.Project);

                foreach (var t in StagedTenants.ToList())
                {
                    // i only want to deploy projects that are in the releases list and in the tenant
                    var projectsToDeploy = from p1 in attachedProjects
                            join p2 in t.Projects on p1.Id equals p2.Id
                            select p1;

                    DomainEvents.Raise(new TenantDeploymentQueued(t.Tenant, projectsToDeploy,
                        firstRelease, middleReleases, lastRelease,
                        SourceTenant, SourceEnvironment, DestinationEnvironment));
                }
            }, () => StagedTenants.Count > 0);

            this.CancelConfirmation = new BindingCommand(() =>
            {
                DomainEvents.Raise(new DeploymentConfirmationDenied());
            });
        }

        public void Handle(TenantStaged args)
        {
            IEnumerable<Project> tenantProjects = null;
            StagedTenants.Add(new StagedTenantViewModel(args.Tenant));
            Task.Run(() =>
            {
                tenantProjects = getProjects.GetProjects(args.Tenant);
            })
            .ContinueWith(task =>
            {
                UiFactory.StartNew(() =>
                {
                    var vm = StagedTenants.First(t => t.Tenant.Id == args.Tenant.Id);
                    vm.UpdateProjects(tenantProjects);
                });
            });
        }

        public void Handle(TenantUnstaged args)
        {
            Unstage(args.Tenant);
        }

        public void Handle(TenantDeploymentQueued args)
        {
            Unstage(args.Tenant);
        }

        public void Handle(ReleaseMarkedFirst args)
        {
            var release = SourceReleases.Single(r => args.Release.Id == r.Release.Id);
            release.IsMiddle = false;
            SourceReleases.Remove(release);
            foreach (var rel in SourceReleases) rel.IsFirst = false;
            SourceReleases.Insert(0, release);
        }

        public void Handle(ReleaseMarkedLast args)
        {
            var release = SourceReleases.Single(r => args.Release.Id == r.Release.Id);
            release.IsMiddle = false;
            SourceReleases.Remove(release);
            foreach (var rel in SourceReleases) rel.IsLast = false;
            SourceReleases.Add(release);
        }

        private void Unstage(Tenant tenant)
        {
            var tenantToRemove = StagedTenants.First(t => t.Name == tenant.Name);
            StagedTenants.Remove(tenantToRemove);
        }
    }
}
