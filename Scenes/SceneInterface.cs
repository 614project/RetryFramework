using RetryFramework.Events;

namespace RetryFramework.Scenes;

public interface SceneInterface : Prepare, Release, Update
{
    public SceneEventHandler EventHandler { get; }
}
