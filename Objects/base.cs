using RetryFramework.Events;
using static RetryFramework.Types;
namespace RetryFramework.Objects;

public abstract class RetryObject: Prepare, Release, Update, Resize
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
    public virtual ActionForObject<RetryObject> Actions { get; set; }
    public int Z { get; internal set; }
    public virtual void Release() => Actions.Release?.Invoke(this);
    public virtual void Prepare() => Actions.Prepare?.Invoke(this);
    public virtual void Update() => Actions.Update?.Invoke(this);
    public virtual void Resize() => Actions.Resize?.Invoke(this);
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
    public Action<T>? Resize = null;
    public Action<T>? WindowMove = null;
    public Action<T>? WindowClose = null;
    public ActionForObject(Action<T>? prepare=null, Action<T>? release = null, Action<T>? update = null,Action<T>? resize = null)
    {
        Prepare = prepare;
        Release = release;
        Update = update;
        Resize = resize;
    }
}

public static class Convenience
{
    public static RetryObject? CreateObject<T>(Action<T>? ready = null) where T : RetryObject
    {
        Type type = typeof(T);
        RetryObject? obj = null;
        if (typeof(Circle) == type) obj = new Circle((Action<Circle>)ready!);
        else if (typeof(Rectangle) == type) obj = new Rectangle((Action<Rectangle>)ready!);
        else if (typeof(Image) == type) obj = new Image((Action<Image>)ready!);
        else if (typeof(Group) == type) obj = new Group((Action<Group>)ready!);
        else if (typeof(Text) == type) obj = new Text((Action<Text>)ready!);
        return obj;
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