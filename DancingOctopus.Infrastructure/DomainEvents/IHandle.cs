namespace DancingOctopus.Infrastructure.DomainEvents
{
    public interface IHandle<T> where T : IDomainEvent
    {
        void Handle(T args);
    }
}
