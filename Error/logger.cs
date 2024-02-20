namespace RetryFramework.Error;

public class Logger
{
    Queue<(string,DateTime)> queue = new();
    public (string message, DateTime time)? Pop() => queue.Count is 0 ? null : queue.Dequeue();
    public bool Pop(out string error,out DateTime time)
    {
        error = string.Empty;
        time = default;
        if (queue.Count is 0) return false;
        (error,time) = queue.Dequeue();
        return true;
    }
    internal void Push(string push) => queue.Enqueue((push,DateTime.Now));

    public static implicit operator string?(Logger logger) => logger.Pop()?.message;
    public static implicit operator (string message, DateTime time)?(Logger logger) => logger.Pop();
    public static Logger operator +(Logger logger,string message)
    {
        logger.Push(message);
        return logger;
    }
}