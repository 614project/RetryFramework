namespace RetryFramework.Error;

public class Logger
{
    Queue<string> queue = new();
    public string? Pop() => queue.Count is 0 ? null : queue.Dequeue();
    internal void Push(string push) => queue.Enqueue(push);

    public static implicit operator string?(Logger logger) => logger.Pop();
    public static Logger operator +(Logger logger,string message)
    {
        logger.Push(message);
        return logger;
    }
}