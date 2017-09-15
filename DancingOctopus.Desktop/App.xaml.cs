using DancingOctopus.Domain.Events;
using DancingOctopus.Infrastructure.DomainEvents;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DancingOctopus.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Current.DispatcherUnhandledException += (o, e) => { DomainEvents.Raise(new ApplicationStarted()); };
            AppDomain.CurrentDomain.UnhandledException += (o, e) => { DomainEvents.Raise(new ApplicationStarted()); };
        }
    }
}
