namespace Neurocita.Reactive.Configuration
{
    public delegate TComponent ComponentFactory<TComponent>();

    public interface IComponentConfiguration<TComponent>
    {
        ComponentFactory<TComponent> Factory { get; }
    }
}
