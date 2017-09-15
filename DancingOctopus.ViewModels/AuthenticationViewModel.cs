using DancingOctopus.Domain;
using DancingOctopus.Domain.Events;
using DancingOctopus.Domain.Services;
using DancingOctopus.Infrastructure.DomainEvents;
using GalaSoft.MvvmLight;
using PropertyChanged;
using System;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class AuthenticationViewModel : ViewModelBase
    {
        public string Server { get; set; }
        public string ApiKey { get; set; }
        public BindingCommand Authenticate { get; }

        public AuthenticationViewModel(ICheckConnections connectionCheck,ConnectionDetails conn)
        {
            ApiKey = conn.ApiKey;
            Server = conn.Server;
            Authenticate = new BindingCommand(
                () =>
                {
                    if (connectionCheck.CanConnect(new Uri(Server), ApiKey))
                    {
                        conn.Server = Server;
                        conn.ApiKey = ApiKey;
                        DomainEvents.Raise(new ServerConnected(new Uri(Server), ApiKey));
                    }
                },
                () =>
                {
                    return !(string.IsNullOrWhiteSpace(ApiKey)
                            || string.IsNullOrWhiteSpace(Server)
                            || !Uri.TryCreate(Server, UriKind.Absolute, out Uri uri));
                });
        }
    }
}
