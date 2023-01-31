namespace Neurocita.Reactive.Pipeline
{
    public interface IPipelineInterceptor<TPayload>
    {
        IMessage<TPayload> Intercept(IMessage<TPayload> payload);
    }
}
