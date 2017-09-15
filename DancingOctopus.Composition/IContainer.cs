using System.Collections.Generic;

namespace DancingOctopus.Composition
{
    public interface IContainer
    {
        IEnumerable<T> GetAllInstances<T>();
        T GetInstance<T>();
    }
}
