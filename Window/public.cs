using RetryFramework.Error;
using RetryFramework.Events;
using RetryFramework.Scene;
using RetryFramework.SDL2;
using System;
namespace RetryFramework;

public partial class Window : IDisposable
{
    /// <summary>
    /// 창을 생성합니다.
    /// </summary>
    /// <param name="width">너비</param>
    /// <param name="height">높이</param>
    /// <param name="title">제목</param>
    /// <param name="x">가로 위치 (null 일경우 중앙)</param>
    /// <param name="y">세로 위치 (null 일경우 중앙)</param>
    public Window(int width,int height, string title,int? x = null,int? y = null)
    {
        _size.w = width; _size.h = height;
        if (!_init()) return;
        if (!_create(title,x,y,SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE)) return;
        if (!_create_renderer()) return;
    }

    public void Run(bool CallResize = true)
    {
        IsRunning = true;
        _prepare();
        if (CallResize) EventHandler.Resize(this);
        _running_loop();
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void Dispose()
    {
        SDL.SDL_DestroyRenderer(_renderptr);
        SDL.SDL_DestroyWindow(_winptr);
        SDL.SDL_Quit();
    }

    public Handler EventHandler { get; set; } = new();
    public bool WasInit => _winptr != IntPtr.Zero;
    public Color BackgroundColor = new();
    public int Width => _size.w;
    public int Height => _size.h;

    public (int X, int Y) Position { get; private set; } = (0, 0);
    /// <summary>
    /// 창 닫기를 눌렀을때 종료할지에 대한 여부입니다. (기본값은 true입니다.)
    /// </summary>
    public bool StopWhenClose { get; set; } = true;
    public SceneList Scenes { get; internal set; } = new();
    public ushort FrameRate { get => _fps_mamager.FrameRate; set => _fps_mamager.FrameRate = value; }
    public bool IsRunning { get; private set; }

    public double RunningTime => _stopwatch.ElapsedTicks * 0.0000001;
    public double DeltaTime { get; private set; } = 0;
    public Logger ErrorLog { get; private set; } = new();
    public InitOption DefaultInitValue { get; set; } = new();
}

/// <summary>
/// 세부적인 초기화 기본값입니다.
/// </summary>
/// <param name="audio">오디오 초기화 옵션</param>
public record InitOption(AudioOption Audio)
{
    public InitOption() : this(new AudioOption()) { }
}

/// <summary>
/// 오디오 옵션입니다.
/// </summary>
/// <param name="Frequency">주파수</param>
/// <param name="Format">오디오 형식</param>
/// <param name="Channels">채널 수</param>
/// <param name="BufferSize">버퍼 크기</param>
public record AudioOption(int Frequency, ushort Format, int Channels,int BufferSize)
{
    public AudioOption() : this(SDL_mixer.MIX_DEFAULT_FREQUENCY, SDL_mixer.MIX_DEFAULT_FORMAT, SDL_mixer.MIX_DEFAULT_CHANNELS,1024) { }
}