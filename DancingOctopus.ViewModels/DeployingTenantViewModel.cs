using System;
using DancingOctopus.Domain;
using DancingOctopus.Domain.Events;
using DancingOctopus.Infrastructure.DomainEvents;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using Humanizer;
using System.Windows.Threading;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class DeployingTenantViewModel : TenantViewModel
    {
        private Stopwatch stopwatch = Stopwatch.StartNew();

        public ObservableCollection<ProjectViewModel> Projects { get; private set; }
        public BindingCommand CancelDeployment { get; }
        public string Duration { get; private set; }
        public DeployingTenantViewModel(Tenant t, IEnumerable<Project> projects) : base(t)
        {
            this.Projects = new ObservableCollection<ProjectViewModel>(
                    projects.Select(p => new ProjectViewModel(p)));

            this.CancelDeployment = new BindingCommand(() =>
            {
                DomainEvents.Raise(new TenantDeploymentCancelled(Tenant));
            });

            var tmr = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            tmr.Tick += (o, e) => Duration = stopwatch.Elapsed.Humanize();
            tmr.Start();
        }
    }
}
