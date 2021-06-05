using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Neurocita.Reactive.Pipes
{
    internal class PipeStreamServerProducer<T> : IDisposable
    {
        private readonly PipeSecurity pipeSecurity = new PipeSecurity();
        private readonly IFormatter formatter = new BinaryFormatter();
        private readonly IObservable<T> observable;
        private readonly string pipeName;
        private readonly List<Task> workers = new List<Task>();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly Task watcher;

        internal PipeStreamServerProducer(IObservable<T> observable, string pipeName, IFormatter formatter = null)
        {
            pipeSecurity.AddAccessRule(
                new PipeAccessRule(WindowsIdentity.GetCurrent().User, PipeAccessRights.FullControl, AccessControlType.Allow)
            );
            pipeSecurity.AddAccessRule(
                new PipeAccessRule(
                    new SecurityIdentifier(WellKnownSidType.WorldSid, null), PipeAccessRights.ReadWrite,
                    AccessControlType.Allow
                    )
                );

            this.observable = observable;
            this.pipeName = pipeName;
            if (formatter != null)
                this.formatter = formatter;

            watcher = Task.Factory.StartNew(() =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    if (workers.Count == 0)
                        Task.Delay(500);
                    else
                    {
                        Task completed = Task.WhenAny(workers);
                        lock (workers)
                        {
                            workers.Remove(completed);
                        }
                    }
                }
            });

            Task task = Task.CompletedTask;
            task.ContinueWith(DoWork, cancellationTokenSource.Token, TaskContinuationOptions.LongRunning | TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            watcher.Wait(TimeSpan.FromSeconds(10));
            Task.WhenAll(workers).Wait(TimeSpan.FromSeconds(10));
            lock (workers)
            {
                workers.Clear();
            }
        }

        private void DoWork(Task task)
        {
#if NETFRAMEWORK
            NamedPipeServerStream serverPipeStream = new NamedPipeServerStream(pipeName, PipeDirection.InOut,
                -1, PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous, 4096, 4096, pipeSecurity);
#else
            NamedPipeServerStream serverPipeStream = new NamedPipeServerStream(pipeName, PipeDirection.InOut,
                -1, PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous, 4096, 4096);
#endif
            // ToDo: Implement scheduling in a new thread
            try
            {
                serverPipeStream.WaitForConnectionAsync(cancellationTokenSource.Token).Wait(cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            if (cancellationTokenSource.IsCancellationRequested || !serverPipeStream.IsConnected)
                return;

            lock (workers)
            {
                workers.Add(Task.Factory.StartNew(state =>
                {
                    PipeStream pipeStream = state as PipeStream;
                    using (pipeStream)
                    {
                        IDisposable observer = null;
                        observer = observable.Subscribe(
                            value =>
                            {
                                try
                                {
                                    if (!cancellationTokenSource.IsCancellationRequested && pipeStream.IsConnected)
                                    {
                                        formatter.Serialize(pipeStream, value);
                                        pipeStream.Flush();
                                    }
                                }
                                catch (IOException exception)       // ToDo: Exception handling ???
                                {
                                    if (!pipeStream.IsConnected)
                                        observer.Dispose();
                                    else
                                        throw exception;
                                }
                            },
                            exception => pipeStream.Close(),        // ToDo: Exception handling
                            () => pipeStream.Close());

                        while (!cancellationTokenSource.IsCancellationRequested && pipeStream.IsConnected)
                        {
                            Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
                        }

                        observer.Dispose();
                    }
                }, serverPipeStream, cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default));
            }

            task.ContinueWith(DoWork, cancellationTokenSource.Token, TaskContinuationOptions.LongRunning | TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }
    }
}
