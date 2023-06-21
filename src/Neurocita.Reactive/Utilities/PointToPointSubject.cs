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
        private readonly ConcurrentQueue<Tuple<IObserver<T>, ICancelable>> observers = new ConcurrentQueue<Tuple<IObserver<T>, ICancelable>>();
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
            observers.Enqueue(observerDisposable);
            disposables.Add(observerDisposable.Item2);
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
                disposables.Dispose();
                Tuple<IObserver<T>, ICancelable> observer = default(Tuple<IObserver<T>, ICancelable>);
                while (observers.TryDequeue(out observer));     // Empty observers
            }
        }

        private void ConsumerWork(Task task)
        {
            if (disposables.IsDisposed)
                return;

            Tuple<IObserver<T>, ICancelable> observer = default(Tuple<IObserver<T>, ICancelable>);
            while (task.IsCanceled && observers.TryDequeue(out observer))
            {
                if (observer.Item2.IsDisposed)
                    continue;
                
                observer.Item1.OnCompleted();
                break;
            }

            while (task.IsFaulted && observers.TryDequeue(out observer))
            {
                if (observer.Item2.IsDisposed)
                    continue;
                
                observer.Item1.OnError(task.Exception);
                break;
            }

            if (!task.IsCanceled && !task.IsFaulted)
            {
                T value = default(T);
                while (!observers.IsEmpty && !queue.IsEmpty)
                {
                    if (observers.TryDequeue(out observer))
                    {
                        if (observer.Item2.IsDisposed)
                            continue;
                            
                        observers.Enqueue(observer);
                        if (queue.TryDequeue(out value))
                            observer.Item1.OnNext(value);
                    }
                }
            }
            else
                Dispose();
        }
    }
}