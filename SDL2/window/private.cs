using RetryFramework.SDL2;
using System.Diagnostics;

namespace RetryFramework;

public partial class Window
{
    IntPtr _winptr = IntPtr.Zero, _renderptr = IntPtr.Zero;
    SDL.SDL_Rect _size = default;
    Stopwatch _stopwatch = default;
    FPSmanager _fps_mamager = new();
    //창 생성
    bool _create(string name,int? x,int? y,SDL.SDL_WindowFlags flag)
    {
        if( (_winptr = SDL.SDL_CreateWindow(name,x ?? SDL.SDL_WINDOWPOS_CENTERED,y ?? SDL.SDL_WINDOWPOS_CENTERED, _size.w,_size.h,flag)) == IntPtr.Zero)
        {
            _error_push();
            return false;
        }
        return true;
    }
   //랜더러 생성
    bool _create_renderer()
    {
        if( (_renderptr = SDL.SDL_CreateRenderer(_winptr,-1,SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED)) == IntPtr.Zero)
        {
            _error_push();
            return false;
        }
        Renderer.ptr = _renderptr;
        // 이방성 필터링 (Direct3D)
        if (SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "2") is SDL.SDL_bool.SDL_FALSE)
        {
            // 실패한 경우: 선형 필터링 (OpenGL & Direct3D)
            SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1");
        }
        return true;
    }
    //SDL 초기화 (로그 포함)
    bool _init()
    {
        //필수 초기화
        if (!_init_without_log()) { _error_push(); return false; }
        //사소한 초기화
        _display_init();
        _stopwatch = new();
        return true;
    }
    //SDL 초기화 (로그 없음)
    bool _init_without_log()
    {
        if (
            SDL.SDL_Init(
                SDL.SDL_INIT_VIDEO | 
                SDL.SDL_INIT_AUDIO | 
                SDL.SDL_INIT_EVENTS
            ) is 0 &&
            SDL_image.IMG_Init(
                SDL_image.IMG_InitFlags.IMG_INIT_JPG |
                SDL_image.IMG_InitFlags.IMG_INIT_PNG |
                SDL_image.IMG_InitFlags.IMG_INIT_TIF |
                SDL_image.IMG_InitFlags.IMG_INIT_WEBP
            ) is not 0 &&
            SDL_mixer.Mix_Init(
                SDL_mixer.MIX_InitFlags.MIX_INIT_MP3 |
                SDL_mixer.MIX_InitFlags.MIX_INIT_FLAC |
                SDL_mixer.MIX_InitFlags.MIX_INIT_OGG |
                SDL_mixer.MIX_InitFlags.MIX_INIT_MID
            ) is not 0 &&
            SDL_ttf.TTF_Init() is 0
        ) return true;
        return false;
    }
    //준비
    void _prepare()
    {
        foreach (var sc in Scenes) sc.Prepare();
    }
    //루프
    void _running_loop()
    {
        _stopwatch.Reset();
        _stopwatch.Start();
        _fps_mamager.Reset(_stopwatch.ElapsedTicks);

        while(IsRunning)
        {
            if (_fps_mamager.IsDrawNow(_stopwatch.ElapsedTicks))
            {
                DeltaTime = _fps_mamager.Drew(_stopwatch.ElapsedTicks);
                _update();
                _draw();
            }
            _polling_update();
        }
    }
    void _event_process(SDL.SDL_Event e)
    {
        switch (e.type)
        {
            case SDL.SDL_EventType.SDL_WINDOWEVENT:
                _window_event_process(e.window);
                break;
        }
    }
    void _window_event_process(SDL.SDL_WindowEvent e)
    {
        switch (e.windowEvent)
        {
            case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED:
                (this._size.w, this._size.h) = (e.data1, e.data2);
                EventHandler.Resize(this);
                break;
            case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_MOVED:
                this.Position = (e.data1, e.data2);
                EventHandler.WindowMove(this);
                break;
            case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
                EventHandler.WindowClose(this);
                break;
        }
    }
    void _polling_update()
    {
        if (SDL.SDL_PollEvent(out SDL.SDL_Event e) is 0) _fps_mamager.Waitable(_stopwatch.ElapsedTicks);
        else _event_process(e);
    }
    void _update()
    {
        foreach (var sc in this.Scenes) sc.Update();
    }
    void _draw()
    {
        foreach(var sc in this.Scenes)
        {
            sc.Draw(ref this._size);
        }
        SDL.SDL_RenderPresent(_renderptr);
        SDL.SDL_SetRenderDrawColor(_renderptr,
            BackgroundColor.Red,
            BackgroundColor.Green,
            BackgroundColor.Blue,
            BackgroundColor.Alpha);
        SDL.SDL_RenderClear(_renderptr);
        //잦은 렌더링으로 이벤트가 너무 많이 쌓이는걸 방지.
        _clear_hold_off_events();
    }
    void _clear_hold_off_events() //보류된 이벤트 모두 처리
    {
        // 삭제예정
        if (bypass_rendering_stopped_when_resizing) return;
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) is not 0) _event_process(e);
    }
    //오류 추가
    void _error_push() => ErrorLog += SDL.SDL_GetError();
}