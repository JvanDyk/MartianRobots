using MartianRobots.Models;
using System.Collections.Concurrent;
namespace MartianRobots.Infrastructure.Queues;

public class RobotQueue
{
    private readonly BlockingCollection<Robot> _queue = new();
    private readonly TaskCompletionSource<bool> _completionSource = new();

    public void Enqueue(Robot robot) => _queue.Add(robot);

    public Robot Dequeue() => _queue.Take();

    public bool TryDequeue(out Robot robot) => _queue.TryTake(out robot);

    public bool IsEmpty => _queue.Count == 0;

    public bool IsCompleted => _queue.IsAddingCompleted;

    public IEnumerable<Robot> SequenceOfRobots => _queue.GetConsumingEnumerable();

    public int Count => _queue.Count;

    public IReadOnlyList<Robot> GetSnapshot()
    {
        return _queue.ToArray();
    }

    public void CompleteAdding()
    {
        _queue.CompleteAdding();
        if (_queue.Count == 0)
            _completionSource.SetResult(true);
    }

    public void WaitForCompletion()
    {
        _queue.GetConsumingEnumerable().ToList();
        _completionSource.Task.Wait();
    }
}
