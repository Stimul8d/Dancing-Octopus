using StructureMap;

namespace DancingOctopus.Infrastructure.DomainEvents
{
    public static class DomainEvents
    {
        public static IContainer Container { get; set; } //as before

        //Raises the given domain event
        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (Container == null) return;

            foreach (var handler in Container.GetAllInstances<IHandle<IDomainEvent>>()) handler.Handle(args);

            foreach (var handler in Container.GetAllInstances<IHandle<T>>())handler.Handle(args);
        }
    }
}
