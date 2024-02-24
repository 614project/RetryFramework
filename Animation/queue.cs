namespace RetryFramework;

public partial class Animation
{
    public static readonly AnimationQueue Queue = new();
}

public class AnimationQueue
{
    /// <summary>
    /// 애니메이션을 대기열에 추가합니다.
    /// </summary>
    /// <param name="info">애니메이션 정보</param>
    /// <returns>성공적으로 추가할경우 true, 이미 존재하여 추가되지 않을경우 false를 반환합니다.</returns>
    public bool Add(Animation.Information info)
    {
       return _queue.Add(info);
    }
    /// <summary>
    /// 대기열에 있는 모든 애니메이션 정보를 갱신합니다.
    /// (애니메이션이 많을수록 이 함수는 비싼 작업이 됩니다. 과도하게 사용하지 마세요.)
    /// </summary>
    public void Update()
    {
        Queue<Animation.Information> removable = new();
        foreach (var info in _queue)
        {
            if (Window.RunningTime < info.StartTime) continue;
            if (info.update()) removable.Enqueue(info);
        }
        while(removable.Count > 0) _queue.Remove(removable.Dequeue());
    }
    /// <summary>
    /// 애니메이션이 존재하는지, 존재한다면 애니메이션이 끝날 시간이 되었을시 삭제합니다.
    /// </summary>
    /// <param name="info">애니메이션 정보</param>
    /// <returns>존재한다면 true, 아니면 false</returns>
    public bool Exist(Animation.Information info)
    {
        //존재하지도 않으면
        if (!_queue.Contains(info)) return false;
        //시작도 안했으면
        if (Window.RunningTime < info.StartTime) return true;
        //애니메이션이 아직 안끝났으면.
        if ((Window.RunningTime - info.StartTime) < info.AnimateTime)
        {
            return true;
        }
        info.done();
        return false;
    }

    private HashSet<Animation.Information> _queue = new();
}