using NSubstitute;

namespace Test.DynDnsProxy;

public class TestsFor<T> where T : class
{
    protected class Context
    {
        public readonly Dictionary<Type, Func<object>> MockMap = new();
        public readonly Dictionary<Type, List<Action<object>>> MockConfigurations = new();
        public T? Subject { get; set; }
    }
    private Context _context;

    [SetUp]
    public void SetUpAutoInject()
    {
        _context = new Context();
    }

    protected T Subject => _context.Subject ??= CreateSubject();

    private T CreateSubject()
    {
        var constructorInfo = typeof(T).GetConstructors().First();
        var parameters = constructorInfo.GetParameters()
            .Select(x => SubstituteFor(x.ParameterType))
            .ToArray();
        return (T)constructorInfo.Invoke(parameters);
    }

    protected TFor SubstituteFor<TFor>() where TFor : class
        => (TFor)Configure(typeof(TFor), _context.MockMap.TryGetValue(typeof(TFor), out var value)
            ? value()
            : Substitute.For<TFor>())!;

    private object SubstituteFor(Type forThis)
        => Configure(forThis, _context.MockMap.TryGetValue(forThis, out var value) ? value() : Substitute.For([forThis], []))!;

    private object? Configure(Type type, object? obj)
    {
        if (obj != null && _context.MockConfigurations.TryGetValue(type, out var configurations))
            foreach (var configuration in configurations)
                configuration(obj);
        return obj;
    }

    protected MockConfigurator Set => new(_context);

    protected class MockConfigurator(Context context)
    {
        public MockConfigurationBuilder<TFor> SubstituteFor<TFor>() where TFor : class
            => new(context);
    }

    protected class MockConfigurationBuilder<TFor>(Context context) where TFor : class
    {
        public MockObjectConfigurator<TFor> ToNull() => To(() => null!);
        public MockObjectConfigurator<TFor> To(TFor concrete) => To(() => concrete);

        public MockObjectConfigurator<TFor> To(Func<TFor> concrete)
        {
            context.MockMap[typeof(TFor)] = concrete;
            return new MockObjectConfigurator<TFor>(context);
        }

        public void Configure(Action<TFor> func)
            => new MockObjectConfigurator<TFor>(context).Configure(func);
    }

    protected class MockObjectConfigurator<TFor>(Context context)
        where TFor : class
    {
        public void Configure(Action<TFor> configure)
        {
            if (!context.MockConfigurations.ContainsKey(typeof(TFor)))
                context.MockConfigurations[typeof(TFor)] = [];
            context.MockConfigurations[typeof(TFor)].Add(x => configure((x as TFor)!));
        }
    }
}
