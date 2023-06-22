using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Neurocita.Reactive.Utilities
{
    public class PointToPointSubject<T> : ISubject<T>, IDisposable
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
        private readonly ConcurrentQueue<Tuple<IObserver<T>, ICancelable>> observerDisposables = new ConcurrentQueue<Tuple<IObserver<T>, ICancelable>>();
        private Task consumerWorker = Task.CompletedTask;

        public void OnCompleted()
        {
            lock (consumerWorker)
            {
                consumerWorker = consumerWorker
                                    .ContinueWith(ConsumerWork)
                                    .ContinueWith(task => Task.FromCanceled(CancellationToken.None))
                                    .ContinueWith(ConsumerWork);
            }
        }

        public void OnError(Exception error)
        {
            lock (consumerWorker)
            {
                consumerWorker = consumerWorker
                                    .ContinueWith(ConsumerWork)
                                    .ContinueWith(task => Task.FromException(error))
                                    .ContinueWith(ConsumerWork);
            }
        }

        public void OnNext(T value)
        {
            if (disposables.IsDisposed)
                return;

            queue.Enqueue(value);
            lock (consumerWorker)
            {
                consumerWorker = consumerWorker
                                    .ContinueWith(ConsumerWork);
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (disposables.IsDisposed)
                return disposables as IDisposable;

            Tuple<IObserver<T>, ICancelable> observerDisposable = new Tuple<IObserver<T>, ICancelable>(observer, new BooleanDisposable());
            observerDisposables.Enqueue(observerDisposable);
            disposables.Add(observerDisposable.Item2);

            lock (consumerWorker)
            {
                consumerWorker = consumerWorker
                                    .ContinueWith(ConsumerWork);
            }

            return observerDisposable.Item2 as IDisposable;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposables.IsDisposed)
                return;

            if (disposing)
            {
                consumerWorker.Wait();
                disposables.Dispose();
                Tuple<IObserver<T>, ICancelable> observer = default(Tuple<IObserver<T>, ICancelable>);
                while (observerDisposables.TryDequeue(out observer));     // Empty observers
            }
        }

        private void ConsumerWork(Task task)
        {
            if (disposables.IsDisposed)
                return;

            Tuple<IObserver<T>, ICancelable> observerDisposable = default(Tuple<IObserver<T>, ICancelable>);
            while (task.IsCanceled && observerDisposables.TryDequeue(out observerDisposable))
            {
                if (observerDisposable.Item2.IsDisposed)
                    continue;
                
                observerDisposable.Item1.OnCompleted();
                break;
            }

            while (task.IsFaulted && observerDisposables.TryDequeue(out observerDisposable))
            {
                if (observerDisposable.Item2.IsDisposed)
                    continue;
                
                observerDisposable.Item1.OnError(task.Exception);
                break;
            }

            if (!task.IsCanceled && !task.IsFaulted)
            {
                T value = default(T);
                while (!observerDisposables.IsEmpty && !queue.IsEmpty)
                {
                    if (observerDisposables.TryDequeue(out observerDisposable))
                    {
                        if (observerDisposable.Item2.IsDisposed)
                            continue;
                            
                        observerDisposables.Enqueue(observerDisposable);
                        if (queue.TryDequeue(out value))
                            observerDisposable.Item1.OnNext(value);
                    }
                }
            }
            else
                Dispose();
        }
    }
}