using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Linq;
using System.IO;

namespace Neurocita.Reactive
{
    internal class SinkEndpoint : ISinkEndpoint
    {
        private readonly CompositeDisposable subscribers = new CompositeDisposable();
        private readonly ITransportMessageSink transportMessageSink;
        private readonly ISerializer serializer;

        public SinkEndpoint(ITransportMessageSink transportMessageSink, ISerializer serializer)
        {
            Util.CheckNullArgument(transportMessageSink);
            Util.CheckNullArgument(serializer);

            this.transportMessageSink = transportMessageSink;
            this.serializer = serializer;
        }

        public IDisposable From<T>(IObservable<T> observable) where T : IDataContract
        {
            return From(observable, new List<IPipelineTask<IPipelineContext>>());
        }

        public IDisposable From<T>(IObservable<T> observable, IEnumerable<IPipelineTask<IPipelineContext>> pipelineTasks) where T : IDataContract
        {
            Util.CheckNullArgument(observable);
            Util.CheckNullArgument(pipelineTasks);

            if (subscribers.IsDisposed)
            {
                IDisposable disposable = Disposable.Empty;
                disposable.Dispose();
                return disposable;
            }
            else
            {
                IObservable<IMessage<Stream>> messages =
                                                observable
                                                    .ToPipelineContext()
                                                    .Tasks(pipelineTasks.Where(task => task is IPipelineObjectTask<T>) as IEnumerable<IPipelineObjectTask<T>>)
                                                    .Serialize(serializer)
                                                    .Tasks(pipelineTasks.Where(task => task is IPipelineTransportTask) as IEnumerable<IPipelineTransportTask>)
                                                    .ToTransportMessage();
                // Pack the subscriber in a composite disposable which auto removes from registry on dispose
                CompositeDisposable subscriber = new CompositeDisposable(transportMessageSink.Observe(messages));
                subscriber.Add(Disposable.Create(() => subscribers.Remove(subscriber)));
                subscribers.Add(subscriber);
                return subscriber;
            }
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (subscribers.IsDisposed)
                return;

            if (disposing)
                subscribers.Dispose();
        }
    }
}
