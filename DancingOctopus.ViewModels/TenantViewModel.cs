using DancingOctopus.Domain;
using DancingOctopus.Domain.Events;
using DancingOctopus.Infrastructure.DomainEvents;
using PropertyChanged;
using System.Collections.Generic;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class TenantViewModel
    {
        public Tenant Tenant { get; private set; }

        public string ImagePath { get { return Tenant.ImagePath; } }
        public string Name { get { return Tenant.Name; } }
        public IEnumerable<Tag> Tags { get { return Tenant.Tags; } }

        public BindingCommand StageForDeployment { get; }

        public TenantViewModel(Tenant tenant)
        {
            this.Tenant = tenant;
            this.StageForDeployment = new BindingCommand(() =>
            {
                DomainEvents.Raise(new TenantStaged(tenant));
            });
        }
    }
}
