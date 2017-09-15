using DancingOctopus.Domain.Events;
using DancingOctopus.Infrastructure.DomainEvents;
using MahApps.Metro.Controls;

namespace DancingOctopus.Desktop
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DomainEvents.Raise(new ApplicationStarted());
        }
    }
}
