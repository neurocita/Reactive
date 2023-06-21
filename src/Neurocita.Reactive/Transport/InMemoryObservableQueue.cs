using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Neurocita.Reactive.Transport
{
    internal class InMemoryObservableQueue<T> : ConcurrentQueue<T>
    {
        private readonly IProducerConsumerCollection<IObserver<T>> consumers = new ConcurrentBag<IObserver<T>>();
        private Task dequeueWorker = Task.CompletedTask;
        private IEnumerator<IObserver<T>> consumerEnumerator = null;

        public InMemoryObservableQueue()
        {
            consumerEnumerator = consumers.GetEnumerator();
        }

        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            lock (dequeueWorker)
            {
                dequeueWorker = dequeueWorker.ContinueWith(DequeueWork);
            }
        }

        public bool TryAddConsumer(IObserver<T> consumer)
        {
            bool success = consumers.TryAdd(consumer);
            if (success)
            {
                lock (consumerEnumerator)
                {
                    consumerEnumerator = consumers.GetEnumerator();
                }
                lock (dequeueWorker)
                {
                    dequeueWorker = dequeueWorker.ContinueWith(DequeueWork);
                }
            }
            return success;
        }

        public bool TryRemoveConsumer(IObserver<T> consumer)
        {
            bool success = consumers.TryTake(out consumer);
            if (success)
            {
                lock (consumerEnumerator)
                {
                    consumerEnumerator = consumers.GetEnumerator();
                }
                lock (dequeueWorker)
                {
                    dequeueWorker = dequeueWorker.ContinueWith(DequeueWork);
                }
            }
            return success;
        }

        private void DequeueWork(Task task)
        {
            while (consumers.Count != 0 && this.Count != 0)
            {
                if (consumerEnumerator.MoveNext())
                {
                    T value = default(T);
                    if (this.TryDequeue(out value))
                        consumerEnumerator.Current.OnNext(value);
                }
                else
                    consumerEnumerator.Reset();
            }
        }
    }
}