using RetryFramework.Events;
namespace RetryFramework.Objects;

public class ObjectList : Prepare, Release, Update
{
    SortedList<int, RetryObject> _objs = new();
    /// <summary>
    /// 객체를 리스트 마지막에 추가합니다.
    /// </summary>
    /// <param name="obj">객체</param>
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
    /// <summary>
    /// 객체를 리스트 앞에 추가합니다.
    /// </summary>
    /// <param name="obj">객체</param>
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
    /// <summary>
    /// 객체를 원하는 위치에 삽입합니다.
    /// </summary>
    /// <param name="obj">객체</param>
    /// <param name="z">위치</param>
    /// <returns>이미 그 위치에 객체가 존재할경우 false를 반환하며 실패합니다. 성공시 true</returns>
    public bool Insert(RetryObject obj, int z)
    {
        if (_objs.ContainsKey(z)) return false;
        _objs.Add(obj.Z = z, obj);
        add_parent(obj);
        return true;
    }
    public bool Swap(RetryObject obj1, RetryObject obj2) => Swap(obj1.Z, obj2.Z);
    /// <summary>
    /// 현재 리스트에 있는 두 객체의 위치를 서로 바꿉니다.
    /// </summary>
    /// <param name="obj1">객체1</param>
    /// <param name="obj2">객체2</param>
    /// <returns>한 객체라도 리스트에 없을경우 false를 반환하며 실패합니다. 성공시 true</returns>
    public bool Swap(int z1, int z2)
    {
        if (!_objs.ContainsKey(z1) || !_objs.ContainsKey(z2)) return false;
        (_objs[z2], _objs[z1]) = (_objs[z1], _objs[z2]);
        (_objs[z1].Z, _objs[z2].Z) = (z1, z2);
        return true;
    }
    public bool Insert(int z, RetryObject obj) => Insert(obj, z);
    /// <summary>
    /// 원하는 객체를 현재 리스트에서 제외합니다.
    /// </summary>
    /// <param name="z">객체의 위치</param>
    /// <returns>객체가 존재하지 않으면 false를 반환하며 실패합니다. 성공시 true</returns>
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
    /// <summary>
    /// 현재 리스트에 있는 객체를 다른 객체로 교체합니다.
    /// </summary>
    /// <param name="z">객체의 위치</param>
    /// <param name="obj">교체될 새 객체</param>
    /// <returns>교체할 객체가 리스트에 존재하지 않으면 false를 반환하며 실패합니다. 성공시 true</returns>
    public bool Replace(int z, RetryObject obj)
    {
        if (!_objs.ContainsKey(z)) return false;
        Remove(z);
        Insert(z, obj);
        return true;
    }
    public bool Replace(RetryObject obj, int z) => Replace(z, obj);
    public bool Replace(RetryObject replaced, RetryObject substitute) => Replace(replaced.Z, substitute);
    public void Clear()
    {
        foreach (var obj in _objs.Values) remove_parent(obj);
        _objs.Clear();
    }
    public IList<RetryObject> List => _objs.Values;
    public IList<int> IndexZ => _objs.Keys;
    /// <summary>
    /// 모든 객체에 대해 정해진 함수를 실행합니다. 객체는 위치값 순으로 함수 인자에 넘겨 실행됩니다.
    /// </summary>
    /// <param name="action">함수</param>
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

    public RetryObject? this[int z]
    {
        get
        {
            if (_objs.ContainsKey(z)) return _objs[z];
            return null;
        }
        set
        {
            if (value is null)
            {
                Remove(z);
                return;
            }
            if (Insert(z,value))
            {
                Replace(z,value);
            }
        }
    }
}
