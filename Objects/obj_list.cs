using RetryFramework.Events;

namespace RetryFramework.Objects;

public class ObjectList : Prepare, Release, Update
{
    SortedList<int, RetryObject> _objs = new();
    public void AddLast(RetryObject obj)
    {
        if (_objs.Count is 0) _objs.Add(obj.Z = 0, obj);
        else _objs.Add(obj.Z = _objs.Max().Key + 1, obj);
        add_parent(obj);
    }
    public void AddLast(IEnumerable<RetryObject> list)
    {
        foreach(var l in list) AddLast(l);
    }
    public void AddFirst(RetryObject obj)
    {
        if (_objs.Count is 0) _objs.Add(obj.Z = 0, obj);
        else _objs.Add(obj.Z = _objs.Min().Key - 1, obj);
        add_parent(obj );
    }
    public void AddFirst(IEnumerable<RetryObject> list)
    {
        foreach (var l in list) AddFirst(l);
    }
    public bool Insert(RetryObject obj, int z)
    {
        if (_objs.ContainsKey(z)) return false;
        _objs.Add(obj.Z = z, obj);
        return true;
    }
    public bool Swap(RetryObject obj1, RetryObject obj2) => Swap(obj1.Z, obj2.Z);
    public bool Swap(int z1, int z2)
    {
        if (!_objs.ContainsKey(z1) || !_objs.ContainsKey(z2)) return false;
        (_objs[z2], _objs[z1]) = (_objs[z1], _objs[z2]);
        (_objs[z1].Z, _objs[z2].Z) = (z1, z2);
        return true;
    }
    public bool Insert(int z, RetryObject obj) => Insert(obj, z);
    public bool Remove(int z) {
        if(_objs.ContainsKey(z))
        {
            remove_parent(_objs[z]);
            _objs.Remove(z);
            return true;
        }
        return false;
    }
    public bool Remove(RetryObject obj) => _objs.Remove(obj.Z);
    public void Clear()
    {
        foreach (var obj in _objs.Values) remove_parent(obj);
        _objs.Clear();
    }
    public IList<RetryObject> List => _objs.Values;
    public IList<int> IndexZ => _objs.Keys;
    public void Foreach(Action<RetryObject> action)
    {
        foreach(var obj in  _objs.Values)
        {
            action(obj);
        }
    }
    public virtual void Prepare()
    {
        foreach (var obj in _objs) obj.Value.Prepare();
    }
    public virtual void Release()
    {
        foreach (var obj in _objs) obj.Value.Release();
    }
    public virtual void Update()
    {
        foreach(var obj in _objs)
        {
            if (obj.Value.RejectUpdate) continue;
            obj.Value.Update();
        }
    }

    //internal
    internal void add_parent(RetryObject obj)
    {
        obj.parent = this.parent;
    }
    internal void remove_parent(RetryObject obj)
    {
        obj.parent = null;
    }
    internal Group parent = null!;
}
