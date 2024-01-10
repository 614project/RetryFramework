﻿using RetryFramework.SDL2;

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
           _timeofone = 10000000 / frequency;
        }
    }
    int frequency = 30; //주사율
    long _endtime;
    long _timeofone; //프레임 간격별 시간
    long one_ms_to_tick = 10000; //1밀리초를 100나노 기준으로 변환한 값.

    internal void Reset(long start)
    {
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
    internal void Drew()
    {
        _endtime += _timeofone;
    }
    internal void Waitable(long now) //휴식
    {
        if (_endtime <= now + one_ms_to_tick) return;
        SDL.SDL_Delay(1);
    }
}
