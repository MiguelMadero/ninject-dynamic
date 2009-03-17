namespace Ninject.Tests.Fakes
{
    public interface IGeneric<T> { }
    public class GenericService<T> : IGeneric<T> { }
    public class GenericService2<T> : IGeneric<T> { }
    public interface IGenericWithConstraints<T> where T : class { }
    public class GenericServiceWithConstraints<T> : IGenericWithConstraints<T> where T : class { }
}