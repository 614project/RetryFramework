using RetryFramework.SDL2;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace RetryFramework;

public static class Framework
{
    //public
    /// <summary>
    /// 프레임워크를 초기화합니다. 그리고 창을 미리 생성합니다.
    /// </summary>
    /// <returns>실패시 오류 메시지를 반환합니다.</returns>
    public static string? Init(string Title, int Width,int Height,int? X=null,int? Y=null,Window.CreateOption? WindowOption = null)
    {
        //미리 렌더 옵션 설정
        SDL.SDL_RendererFlags render_flag = SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED;
        if ((WindowOption ??= new()).VSync)
        {
            render_flag |= SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC;
        }
        //SDL_GetError로 오류 메시지를 받아야되는 함수들.
        if (
            //SDL 초기화
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_EVENTS) != 0 ||
            SDL_image.IMG_Init(SDL_image.IMG_InitFlags.Recommend) == 0 ||
            SDL_mixer.Mix_Init(SDL_mixer.MIX_InitFlags.Recommend) == 0 ||
            SDL_ttf.TTF_Init() == -1 ||
            //창 생성
            (_window = SDL.SDL_CreateWindow(Title , X ?? SDL.SDL_WINDOWPOS_CENTERED , Y ?? SDL.SDL_WINDOWPOS_CENTERED , Width , Height , WindowOption._flags)) == IntPtr.Zero ||
            // 렌더러 생성
            (_renderer = SDL.SDL_CreateRenderer(_window, -1, render_flag)) == IntPtr.Zero
        )
        {
            return SDL.SDL_GetError();
        }
        //그 외 초기화.
        _stopwatch = new();
        Monitor.Update(); //무시해도 좋을 오류.
        return null;
    }
    /// <summary>
    /// 프레임워크를 실행합니다.
    /// </summary>
    /// <exception cref="RetryException">프레임워크가 초기화가 되어있지 않을경우 발생하는 오류입니다.</exception>
    public static void Run()
    {
        _start_();
        for (IsRunning = true ; IsRunning ;) {
            _event_polling_();
            _update_();
        }
        _final_();
    }
    /// <summary>
    /// 프레임워크를 중단합니다.
    /// </summary>
    public static void Stop()
    {
        IsRunning = false;
    }

    //private function
    static void _event_polling_()
    {
        while (SDL.SDL_PollEvent(out var _event) != 0)
        {
            switch (_event.type)
            {
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    EventHandler.KeyDown((Input.Keycode)_event.key.keysym.sym);
                    break;
                case SDL.SDL_EventType.SDL_KEYUP:
                    EventHandler.KeyUp((Input.Keycode)_event.key.keysym.sym);
                    break;
                case SDL.SDL_EventType.SDL_DROPFILE:
                    EventHandler.DropFile(SDL.UTF8_ToManaged(_event.drop.file , true));
                    break;
                case SDL.SDL_EventType.SDL_WINDOWEVENT:
                    _process_window_event_(_event.window);
                    break;

            }
        }
    }
    static void _process_window_event_(in SDL.SDL_WindowEvent _event)
    {
        switch (_event.windowEvent)
        {
            case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
                EventHandler.WindowQuit();
                break;
        }
    } 
    static void _start_()
    {
        if (!IsInitialized)
        {
            throw new RetryException("Framework is not initialized. Please call 'Framework.Init' function before run.");
        }
        _stopwatch.Start();
        EventHandler.Prepare();
    }
    static void _final_()
    {
        EventHandler.Release();
        _stopwatch.Stop();
        SDL.SDL_DestroyRenderer(_renderer);
        _renderer = IntPtr.Zero;
        SDL.SDL_DestroyWindow(_window);
        _window = IntPtr.Zero;
        SDL_ttf.TTF_Quit();
        SDL_image.IMG_Quit();
        SDL_mixer.Mix_Quit();
        SDL.SDL_Quit();
    }
    static void _update_()
    {
        //아직 렌더링 할 차례가 아니라면
        long now = _stopwatch.ElapsedTicks;
        if (now < _next_render_time_)
        {
            //1밀리초 이상의 여유가 있다면, 대기
            if (_next_render_time_- now > 1000)SDL.SDL_Delay(1);
            return;
        }
        //지금 렌더링 해야되면
        _next_render_time_ += Display._delta_time;
        EventHandler.Update(now - _last_render_time_); //먼저 업데이트
        _last_render_time_ = now;
        //그다음 화면에 드로잉
        _rendering_();
        //끝
    }
    static void _rendering_()
    {
        EventHandler._draw();
        SDL.SDL_RenderPresent(_renderer);
        SDL.SDL_SetRenderDrawColor(_renderer , Window.BackgroundColor.Red , Window.BackgroundColor.Green , Window.BackgroundColor.Blue , Window.BackgroundColor.Alpha);
        SDL.SDL_RenderClear(_renderer);
    }

    //property
    /// <summary>
    /// 프레임워크가 이벤트 별로 어떤 작업을 수행할지에 대한 함수가 모여있습니다. 상속을 통해 자유롭게 수정할수 있습니다. (대신 프레임워크가 이상하게 작동할수 있습니다.)
    /// </summary>
    public static FrameworkEventHandler EventHandler { get; set; } = new();
    /// <summary>
    /// 프레임워크가 실행중인지에 대한 여부입니다.
    /// </summary>
    public static bool IsRunning { get; private set; } = false;
    /// <summary>
    /// 프레임워크가 초기화 된 상태인지에 대한 여부입니다.
    /// </summary>
    public static bool IsInitialized => _window != IntPtr.Zero;
    /// <summary>
    /// 프레임워크가 작동한 시간입니다. 단위는 초 입니다.
    /// </summary>
    public static double RunningTime => _stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;

    //internal
    internal static Stopwatch _stopwatch = null!;
    internal static IntPtr _window = IntPtr.Zero;
    internal static IntPtr _renderer = IntPtr.Zero;

    //private
    static long _next_render_time_ = 0;
    static long _last_render_time_ = 0;
}
