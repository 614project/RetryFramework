using static RetryFramework.Types;
namespace RetryFramework.Objects;

public class Group: RetryObject
{
    public Group(Action<Group>? Ready = null)
    {
        Member.parent = this;
        if (Ready is not null) Ready(this);
    }
    public ObjectList Member { get; } = new();
    public override void Prepare()
    {
        base.Prepare();
        Member.Prepare();
    }
    public override void Release()
    {
        base.Release();
        Member.Release();
    }
    public override void Update()
    {
        base.Update();
        Member.Update();
    }
}
