using DancingOctopus.Infrastructure.DomainEvents;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingOctopus.ViewModels
{
    [ImplementPropertyChanged]
    public class ApplicationLogViewModel : IHandle<IDomainEvent>
    {
        public ObservableCollection<string> Logs { get; private set; }
        TaskFactory UiFactory;
        public ApplicationLogViewModel()
        {
            Logs = new ObservableCollection<string>();
            UiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Handle(IDomainEvent args) => UiFactory.StartNew(() => Logs.Insert(0, $"{DateTime.Now.ToShortTimeString()} {args.Description}"));
    }
}
