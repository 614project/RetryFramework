using RetryFramework.Objects;
using RetryFramework.SDL2;

namespace RetryFramework.Scene;

public class Scene : RetryScene
{
    public Scene(Action<Scene>? ready = null)
    {
        group_of_scene = new();
        Actions = new();
        if (ready is not null) ready(this);
    }
    public Scene(ActionForScene<Scene> action, Action<Scene>? ready = null)
    {
        Actions = action;
        if (ready is not null) ready(this);
    }
    public ObjectList Member => group_of_scene.Member;
    public ActionForScene<Scene> Actions;

    public override void Prepare()
    {
        if (Actions.Prepare is not null) Actions.Prepare(this);
        Member.Prepare();
    }
    public override void Release()
    {
        if (Actions.Release is not null) Actions.Release(this);
        Member.Release();
    }
    public override void Update()
    {
        if (Actions.Update is not null) Actions.Update(this);
        Member.Update();
    }
    internal Group group_of_scene;
    internal override void Draw(ref SDL.SDL_Rect win_size)
    {
        SDL.SDL_RenderSetViewport(Renderer.ptr, ref win_size);
        Renderer.RenderRetryObject(group_of_scene, new(width: win_size.w, height: win_size.h));
    }
}

public class ActionForScene<T> where T : RetryScene
{
    public Action<T>? Prepare;
    public Action<T>? Release;
    public Action<T>? Update;

    public ActionForScene(Action<T>? prepare = null, Action<T>? release = null, Action<T>? update= null) {
        this.Prepare = prepare;
        this.Release = release;
        this.Update = update;
    }
}