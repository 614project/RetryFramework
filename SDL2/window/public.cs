using RetryFramework.Error;
using RetryFramework.Events;
using RetryFramework.Scene;
using RetryFramework.SDL2; 
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

    public void Run()
    {
        IsRunning = true;
        _prepare();
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

    public bool WasInit => _winptr != IntPtr.Zero;
    public Color BackgroundColor = new();
    public int Width => _size.w;
    public int Height => _size.h;
    public SceneList Scenes { get; internal set; } = new();
    public ushort FrameRate { get => _fps_mamager.FrameRate; set => _fps_mamager.FrameRate = value; }
    public WindowEventHandler EventHandler { get; set; } = new();
    public bool IsRunning { get; private set; }
    public double RunningTime => _stopwatch.ElapsedTicks * 0.0000001;
    public double DeltaTime { get; private set; } = 0;
    public Logger ErrorLog { get; private set; } = new();
}