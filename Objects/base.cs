using RetryFramework.Events;
using static RetryFramework.Types;
namespace RetryFramework.Objects;

public abstract class RetryObject: Prepare, Release, Update
{
    //public
    public RetryObject(Action<RetryObject>? Ready = null)
    {
        if (Ready is not null) Ready(this);
        Actions = new();
    }
    public RetryObject(ActionForObject<RetryObject> Act, Action<RetryObject>? Ready = null) : this(Ready)
    {
        Actions = Act;
    }
    public Point Position = new();
    public Scale Origin = new(0.5,0.5);
    public ActionForObject<RetryObject> Actions;
    public int Z { get; internal set; }
    public virtual void Release() { if (Actions.Release is not null) Actions.Release(this); }
    public virtual void Prepare() { if (Actions.Prepare is not null) Actions.Prepare(this); }
    public virtual void Update() { if (Actions.Update is not null) Actions.Update(this); }
    public virtual double Opacity { get; set; } = 1;
    public virtual Scale Scale { get; set; } = new();
    public virtual bool Hide { get; set; } = false;
    public bool RejectEvent = false;
    public bool RejectUpdate = false;
    //protected
    internal Group? parent = null;
}

public class ActionForObject<T> where T : RetryObject
{
    public Action<T>? Prepare = null;
    public Action<T>? Release = null;
    public Action<T>? Update = null;
    public ActionForObject(Action<T>? prepare=null, Action<T>? release = null, Action<T>? update = null)
    {
        Prepare = prepare;
        Release = release;
        Update = update;
    }
}

public static class Convenience
{
    public static RetryObject CreateObject<T>(Action<T>? ready = null) where T : RetryObject
    {
        Type type = typeof(T);
        RetryObject obj = default!;
        if (typeof(Circle) == type) obj = new Circle((Action<Circle>)ready!);
        if (typeof(Rectangle) == type) obj = new Rectangle((Action<Rectangle>)ready!);
        if (typeof(Image) == type) obj = new Image((Action<Image>)ready!);
        if (typeof(Group) == type) obj = new Group((Action<Group>)ready!);
        return obj;
    }
}