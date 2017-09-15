using DancingOctopus.Domain.Events;
using DancingOctopus.Domain.Services;
using DancingOctopus.Infrastructure.DomainEvents;
using GalaSoft.MvvmLight;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.Windows.Threading;
using DancingOctopus.Domain;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class ServerStatusViewModel : ViewModelBase, IHandle<ServerConnected>
    {
        private IGetServerStatus serverStatus;
        private ServerStatus status;

        public string Server { get; private set; }
        public string NumberOfRunningTasks { get; private set; }

        public BindingCommand GoToTasks { get; }

        public ServerStatusViewModel(IGetServerStatus serverStatus)
        {
            this.serverStatus = serverStatus;
            var timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 5) };
            timer.Tick += (o,s) => { Update(); };
            timer.Start();

            GoToTasks = new BindingCommand(() => Process.Start($"{status.Server}/app#/tasks"));
        }

        public void Handle(ServerConnected args) => Update();

        private void Update()
        {
            status = serverStatus.GetStatus();
            this.Server = $"Connected to {status.Server}";
            this.NumberOfRunningTasks = $"{status.NumberOfRunningTasks} Running Tasks";
        }
    }
}
