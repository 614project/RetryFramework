using RetryFramework.SDL2;

namespace RetryFramework;

internal class FPSmanager
{
    public FPSmanager()
    {
        Display.Update();
        if (Display.Displays.Length != 0) FrameRate = 0;
    }

    /// <summary>
    /// 초당 프레임률을 설정합니다. 0을 넣을경우 디스플레이 기본 주사율로 설정합니다. (실패시 30 프레임)
    /// </summary>
    public ushort FrameRate
    {
        get => (ushort)frequency; set
        {
            if (value is 0)
            {
                if ((frequency = Display.RefreshRate) < 1)
                {
                    frequency = 30; //최소 주사율
                }
            }
            else frequency = value;
           _timeofone = one_ms_to_tick * 1000 / frequency;
        }
    }
    int frequency = 30; //주사율
    long _prev_time = 0;
    long _endtime;
    long _timeofone = 0; //프레임 간격별 시간
    const long one_ms_to_tick = 10000; //1밀리초를 100나노 기준으로 변환한 값.

    internal void reset(long start)
    {
        //프레임을 설정한 적이 없을경우 기본 주사율로 설정
        if (_timeofone is 0) FrameRate = 0;
        _endtime = start;
    }
    internal bool IsDrawNow(long time) {
        bool answer = _endtime <= time;
        // 1프레임 이상 시간초과 발생시 보정
        if (time >= _endtime + _timeofone)
        {
            _endtime = time; //현재 시간으로 보정
        }
        return answer;
    }
    /// <summary>
    /// 프레임을 카운팅 합니다.
    /// </summary>
    /// <returns>이전 프레임과의 시간차 (ms 단위)</returns>
    internal double drew(long now_time)
    {
        long delta = now_time - _prev_time;
        _prev_time = now_time;
        _endtime += _timeofone;
        return delta * 0.0001;
    }
    internal void waitable(long now) //휴식
    {
        if (_endtime <= now + one_ms_to_tick) return;
        SDL.SDL_Delay(1);
    }
}
