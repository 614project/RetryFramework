namespace RetryFramework.Scenes;

public abstract class SceneEventHandler 
{
    public SceneEventHandler(SceneInterface Scene)
    {
        _target = Scene;
    }
    internal virtual SceneInterface _target { get; set; }
    internal abstract void _draw();
}
