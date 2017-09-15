using DancingOctopus.Composition;
using DancingOctopus.Domain;
using DancingOctopus.ViewModels;

namespace DancingOctopus.Desktop
{
    public class ViewModelLocator
    {
        private Configurator config = new Configurator();
        private ConnectionDetails connectionDetails => new ConnectionDetails(config.GetServer(), config.GetApiKey());
        private IContainer container;

        public ViewModelLocator()
        {
            container = new StructureMapContainer(connectionDetails);
        }

        public CurrentContentViewModel CurrentContent
            => container.GetInstance<CurrentContentViewModel>();

        public ServerStatusViewModel ServerStatus
            => container.GetInstance<ServerStatusViewModel>();

        public AuthenticationViewModel Authentication
            => container.GetInstance<AuthenticationViewModel>();

        public MainViewModel Main
            => container.GetInstance<MainViewModel>();

        public TenantSelectionViewModel TenantSelection
            => container.GetInstance<TenantSelectionViewModel>();

        public StagedTenantsViewModel StagedTenants
            => container.GetInstance<StagedTenantsViewModel>();

        public DeploymentQueueViewModel DeploymentQueue
            => container.GetInstance<DeploymentQueueViewModel>();

        public CompletedTenantDeploymentsViewModel CompletedTenants
            => container.GetInstance<CompletedTenantDeploymentsViewModel>();

        public ApplicationLogViewModel Logs
            => container.GetInstance<ApplicationLogViewModel>();
    }
}