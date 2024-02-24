using RetryFramework.Interface;

namespace RetryFramework.Objects;

public class Group: RetryObject, QuickAddObject
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
    public virtual bool AddMember(RetryObject obj, int? z = null)
    {
        if (z is int zz) return this.Member.Insert(obj, zz);
        this.Member.AddLast(obj);
        return true;
    }
}
