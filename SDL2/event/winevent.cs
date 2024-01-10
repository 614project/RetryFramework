using static RetryFramework.Types;
namespace RetryFramework.Events;

public class WindowEventHandler
{
    public Action<Window>? Prepare = null;
    public Action<Window>? Release = null;
    public Action<Window,Size>? Resize = null;
    public Action<Window,Point>? Move = null;
    public Action<Window>? Close = x => x.Stop();
}