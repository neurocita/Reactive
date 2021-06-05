using System;
using System.IO.Pipes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Neurocita.Reactive.Pipes
{
    internal class PipeStreamClientProducer<T> : IDisposable
    {
        private readonly NamedPipeClientStream pipeStream;
        private readonly IDisposable subscriber;
        private readonly IFormatter formatter = new BinaryFormatter();

        internal PipeStreamClientProducer(IObservable<T> observable, string serverName, string pipeName, IFormatter formatter = null)
        {
            if (formatter != null)
                this.formatter = formatter;

            pipeStream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            pipeStream.Connect();

            subscriber = observable.Subscribe(
                value =>
                {
                    this.formatter.Serialize(pipeStream, value);
                    pipeStream.Flush();
                },
                exception => pipeStream.Close(),        // ToDo: Excepion handling
                () => pipeStream.Close());
        }

        public void Dispose()
        {
            subscriber.Dispose();
            pipeStream.Close();
            pipeStream.Dispose();
        }
    }
}
