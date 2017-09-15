using GalaSoft.MvvmLight;
using PropertyChanged;
using DancingOctopus.Infrastructure.DomainEvents;
using DancingOctopus.Domain.Events;
using System;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class CurrentContentViewModel : ViewModelBase,
        IHandle<ApplicationStarted>,
        IHandle<ServerConnected>, IHandle<DeploymentRequiresConfirmation>,
        IHandle<DeploymentConfirmationDenied>, IHandle<DeploymentConfirmed>
    {
        public AuthenticationViewModel AuthenticationViewModel { get; }
        public MainViewModel MainViewModel { get; }
        public StagedTenantsViewModel ConfirmDeploymentViewModel { get; }
        public ViewModelBase CurrentContent { get; private set; }

        public CurrentContentViewModel(AuthenticationViewModel authVm, MainViewModel mainVm, StagedTenantsViewModel confirmVm)
        {
            this.AuthenticationViewModel = authVm;
            this.MainViewModel = mainVm;
            this.ConfirmDeploymentViewModel = confirmVm;
        }

        public void Handle(ApplicationStarted args)
        {
            CurrentContent = AuthenticationViewModel;
        }

        public void Handle(ServerConnected args)
        {
            CurrentContent = MainViewModel;
        }

        public void Handle(DeploymentRequiresConfirmation args)
        {
            CurrentContent = ConfirmDeploymentViewModel;
        }

        public void Handle(DeploymentConfirmationDenied args)
        {
            CurrentContent = MainViewModel;
        }

        public void Handle(DeploymentConfirmed args)
        {
            CurrentContent = MainViewModel;
        }
    }
}