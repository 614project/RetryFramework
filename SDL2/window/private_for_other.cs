﻿using RetryFramework.SDL2;
using System.Runtime.InteropServices.ObjectiveC;

namespace RetryFramework;

public partial class Window
{
    private void _display_init()
    {
        if ((Display.Current = now_display_index) < 0)
        {
            Display.Current = 0;
            _error_push();
        }
        if (!Display.Update()) _error_push();
    }
    [Obsolete("this function will be removed when SDL 2.30 release.")]
    private void _bypass_block_main_thread_when_window_resizing()
    {
        //창 조절중에는 렌더링이 중단되는 현상 우회 (SDL 2.30 릴리즈시 제거 예정)
        SDL.SDL_SetEventFilter((_, eventPtr) =>
        {
            var e = (SDL.SDL_Event)System.Runtime.InteropServices.Marshal.PtrToStructure(eventPtr, typeof(SDL.SDL_Event))!;
            if (e.key.repeat != 0) return 0;
            if (e.type != SDL.SDL_EventType.SDL_WINDOWEVENT) return 1;
            switch (e.window.windowEvent)
            {
                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED:
                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_MOVED:
                    _draw();
                    return 1;
                default:
                    return 1;
            }
        }, IntPtr.Zero);
    }
}