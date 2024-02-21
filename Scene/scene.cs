using RetryFramework.Events;
using RetryFramework.Interface;
using RetryFramework.Objects;
using RetryFramework.SDL2;

namespace RetryFramework.Scene;

public class Scene : RetryScene, QuickAddObject, SceneRequiredEvents
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

    /// <summary>
    /// 객체를 추가합니다.
    /// </summary>
    /// <typeparam name="T">추가할 객체의 타입</typeparam>
    /// <param name="ready">준비 함수</param>
    /// <param name="z">인덱스</param>
    /// <returns>성공시 true, 실패시 false</returns>
    public virtual bool AddMember<T>(Action<T>? ready = null, int? z = null) where T : RetryObject
    {
        RetryObject? obj = Convenience.CreateObject(ready);
        if (obj is null) return false;
        return AddMember(obj, z);
    }
    public virtual bool AddMember(RetryObject obj,int? z = null)
    {
        if (z is int zz) return this.Member.Insert(obj, zz);
        this.Member.AddLast(obj);
        return true;
    }
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
    public virtual void Resize()
    {
        Member.Foreach(obj => obj.Actions.Resize?.Invoke(obj));
    }
    public virtual void WindowMove()
    {
        Member.Foreach(obj => obj.Actions.WindowMove?.Invoke(obj));
    }
    public virtual void WindowClose()
    {
        Member.Foreach(obj => obj.Actions.WindowClose?.Invoke(obj));
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