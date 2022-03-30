using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MatcherChief.Server.Queues;

public class AsyncConcurrentQueue<T>
{
    private readonly SemaphoreSlim _semaphore;
    private readonly ConcurrentQueue<T> _queue;

    public AsyncConcurrentQueue()
    {
        _semaphore = new SemaphoreSlim(0);
        _queue = new ConcurrentQueue<T>();
    }

    public int Count { get { return _queue.Count; } }

    public void Enqueue(T item)
    {
        _queue.Enqueue(item);
        _semaphore.Release();
    }

    public void EnqueueRange(IEnumerable<T> source)
    {
        var n = 0;
        foreach (var item in source)
        {
            _queue.Enqueue(item);
            n++;
        }

        if (n > 0)
        {
            _semaphore.Release(n);
        }
    }

    public async Task<T> DequeueAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        while (true)
        {
            await _semaphore.WaitAsync(cancellationToken);

            T item;
            if (_queue.TryDequeue(out item))
            {
                return item;
            }
        }
    }
}