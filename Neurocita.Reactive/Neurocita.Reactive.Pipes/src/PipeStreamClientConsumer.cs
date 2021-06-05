using System;
using System.IO.Pipes;
using System.Reactive.Subjects;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Neurocita.Reactive.Pipes
{
    internal class PipeStreamClientConsumer<T> : IDisposableObservable<T>
    {
        private readonly NamedPipeClientStream pipeStream;
        private readonly Subject<T> subject = new Subject<T>();
        private readonly IFormatter formatter = new BinaryFormatter();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly Task task;

        internal PipeStreamClientConsumer(string serverName, string pipeName, IFormatter formatter = null)
        {
            if (formatter != null)
                    this.formatter = formatter;

            pipeStream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            pipeStream.Connect();
            
            task = Task.Factory.StartNew(() =>
            {
                using (pipeStream)
                {
                    try
                    {
                        while (!cancellationTokenSource.IsCancellationRequested && pipeStream.IsConnected)
                        {
                            try
                            {
                                T value = (T)this.formatter.Deserialize(pipeStream);
                                if (value != null)
                                    subject.OnNext(value);
                            }
                            catch (SerializationException)
                            {
                                break;
                            }
                        }
                    }
                    //catch
                    //{
                    // ToDo: Exception handling
                    //}
                    finally
                    {
                        subject.OnCompleted();
                        pipeStream.Close();
                    }
                }
            });
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            task.Wait();
            pipeStream.Dispose();
            subject.Dispose();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return subject.Subscribe(observer);
        }
    }
}
