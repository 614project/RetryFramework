namespace RetryFramework.Events;

public class Handler
{
    public virtual void Resize(Window win)
    {
        foreach(var sc in win.Scenes)
        {
            if (sc is Scene.Scene scene) scene.Resize();
        }
    }
    public virtual void WindowMove(Window win)
    {
        foreach (var sc in win.Scenes)
        {
            if (sc is Scene.Scene scene) scene.WindowMove();
        }
    }
    public virtual void WindowClose(Window win)
    {
        foreach (var sc in win.Scenes)
        {
            if (sc is Scene.Scene scene) scene.WindowClose();
        }
        if (win.StopWhenClose) win.Stop();
    }
}
