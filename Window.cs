using RetryFramework.SDL2;
using RetryFramework.Unit;

namespace RetryFramework;

public static class Window
{
    //internal
    internal static SDL.SDL_Rect _size = new();
    internal static bool _exist_flag(SDL.SDL_WindowFlags f) => ((SDL.SDL_WindowFlags)SDL.SDL_GetWindowFlags(Framework._window)).HasFlag(f);
    //public
    /// <summary>
    /// 창의 배경색을 설정합니다.
    /// </summary>
    public static Color BackgroundColor = Color.White;
    /// <summary>
    /// 현재 창의 위치를 가져옵니다.
    /// </summary>
    public static (int X,int Y) Position { 
        get {
            SDL.SDL_GetWindowPosition(Framework._window,out int  X,out int Y);
            return (X, Y);
        }
    }
    /// <summary>
    /// 창의 수평 위치
    /// </summary>
    public static int X { get; internal set; }
    /// <summary>
    /// 창의 수직 위치
    /// </summary>
    public static int Y { get; internal set; }
    /// <summary>
    /// 창의 너비
    /// </summary>
    public static int Width => _size.w;
    /// <summary>
    /// 창의 높이
    /// </summary>
    public static int Height => _size.h;
    /// <summary>
    /// 다른 창에 가려져도 이 창에 초첨이 맞춰집니다. 잘 작동하진 않습니다.
    /// 창을 맨 위로 올리고 초첨을 맞출려면 Raise() 를 사용하세요.
    /// </summary>
    /// <returns></returns>
    public static bool InputFocus() => SDL.SDL_SetWindowInputFocus(Framework._window) == 0;
    /// <summary>
    /// 전체화면 여부
    /// </summary>
    public static bool Fullscreen {
        get => ((SDL.SDL_WindowFlags)SDL.SDL_GetWindowFlags(Framework._window)).HasFlag(SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN);
        set {
            SDL.SDL_SetWindowFullscreen(Framework._window , value ? 4097u : 0u);
        }
    }
    /// <summary>
    /// 창 제목
    /// </summary>
    public static string Title {
        get => SDL.SDL_GetWindowTitle(Framework._window);
        set => SDL.SDL_SetWindowTitle(Framework._window , value);
    }
    /// <summary>
    /// 창을 맨 앞으로 올리고 사용자가 조작할 대상을 이 창으로 설정합니다.
    /// </summary>
    public static void Raise() => SDL.SDL_RaiseWindow(Framework._window);
    /// <summary>
    /// 최소화 또는 최대화 된 창의 크기와 위치를 원래대로 돌려놓습니다.
    /// </summary>
    public static void Restore() => SDL.SDL_RestoreWindow(Framework._window);
    /// <summary>
    /// 창의 투명도를 설정합니다. 0.0f이 완전히 투명이며 1.0f이 완전 불투명입니다.
    /// 투명도를 지원하지 않는 운영체제에서 get은 1.0f을 반환합니다.
    /// </summary>
    public static float Opacity {
        get { SDL.SDL_GetWindowOpacity(Framework._window , out var op); return op; }
        set => SDL.SDL_SetWindowOpacity(Framework._window , value);
    }
    /// <summary>
    /// 창 표시여부
    /// </summary>
    public static bool Show { set { if (value) SDL.SDL_ShowWindow(Framework._window); else SDL.SDL_HideWindow(Framework._window); } }
    /// <summary>
    /// 창의 아이콘을 설정합니다.
    /// </summary>
    /// <param name="filename">파일명</param>
    /// <exception cref="JyunrcaeaFrameworkException">파일을 불러올수 없을때</exception>
    public static void Icon(string filename)
    {
        IntPtr surface = SDL_image.IMG_Load(filename);
        if (surface == IntPtr.Zero)
            throw new RetryException($"파일을 불러올수 없습니다. (SDL image Error: {SDL_image.IMG_GetError()})");
        SDL.SDL_SetWindowIcon(Framework._window , surface);
        SDL.SDL_FreeSurface(surface);
    }
    /// <summary>
    /// 창의 크기를 조절합니다.
    /// </summary>
    /// <param name="width">너비</param>
    /// <param name="height">높이</param>
    public static void Resize(int width , int height)
    {
        SDL.SDL_SetWindowSize(Framework._window , width , height);
        Window._size.w = width;
        Window._size.h = height;
        SDL.SDL_Event e = new() {
            type = SDL.SDL_EventType.SDL_WINDOWEVENT ,
            window = new() {
                type = SDL.SDL_EventType.SDL_WINDOWEVENT ,
                windowEvent = SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED ,
                data1 = width ,
                data2 = height ,
                windowID = SDL.SDL_GetWindowID(Framework._window) ,
                timestamp = SDL.SDL_GetTicks()
            }
        };
        SDL.SDL_PushEvent(ref e);
        e.window.windowEvent = SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED;
        SDL.SDL_PushEvent(ref e);
    }
    /// <summary>
    /// 창의 위치를 이동합니다.
    /// </summary>
    /// <param name="x">수평 위치</param>
    /// <param name="y">수직 위치</param>
    public static void Move(int? x = null , int? y = null)
    {
        SDL.SDL_SetWindowPosition(Framework._window , x ?? SDL.SDL_WINDOWPOS_CENTERED , y ?? SDL.SDL_WINDOWPOS_CENTERED);
    }
    /// <summary>
    /// 최대 창 크기를 지정합니다.
    /// </summary>
    /// <param name="Width">너비</param>
    /// <param name="Height">높이</param>
    public static (int Width, int Height) MaximizeSize {
        get { SDL.SDL_GetWindowMaximumSize(Framework._window , out int w , out int h); return (w, h); }
        set => SDL.SDL_SetWindowMaximumSize(Framework._window , value.Width , value.Height);
    }
    /// <summary>
    /// 최소 창 크기를 지정합니다.
    /// </summary>
    /// <param name="Width">너비</param>
    /// <param name="Height">높이</param>
    public static (int Width, int Height) MinimizeSize {
        get { SDL.SDL_GetWindowMinimumSize(Framework._window , out int w , out int h); return (w, h); }
        set => SDL.SDL_SetWindowMinimumSize(Framework._window , value.Width , value.Height);
    }
    /// <summary>
    /// 창을 최대화 합니다.
    /// </summary>
    public static void Maximize() => SDL.SDL_MaximizeWindow(Framework._window);
    /// <summary>
    /// 창을 최소화 합니다.
    /// </summary>
    public static void Minimize() => SDL.SDL_MinimizeWindow(Framework._window);
    /// <summary>
    /// 창을 항상 맨위에 올릴지에 대한 여부를 지정합니다.
    /// </summary>
    public static bool AlwaysOnTop {
        set => SDL.SDL_SetWindowAlwaysOnTop(Framework._window , value ? SDL.SDL_bool.SDL_TRUE : SDL.SDL_bool.SDL_FALSE);
        get => _exist_flag(SDL.SDL_WindowFlags.SDL_WINDOW_ALWAYS_ON_TOP);
    }
    /// <summary>
    /// 창을 닫을때 프레임워크를 중지할지에 대한 여부입니다.
    /// </summary>
    public static bool FrameworkStopWhenClose { get; set; } = true;
    /// <summary>
    /// Alt + F4 키로 창을 종료하는걸 막을지에 대한 여부입니다.
    /// </summary>
    public static bool BlockAltF4 {
        set => SDL.SDL_SetHint(SDL.SDL_HINT_WINDOWS_NO_CLOSE_ON_ALT_F4 , value ? "1" : "0");
        get => SDL.SDL_GetHint(SDL.SDL_HINT_WINDOWS_NO_CLOSE_ON_ALT_F4) == "1";
    }
    /// <summary>
    /// 창 크기를 조절할수 있게 할지에 대한 여부입니다.
    /// </summary>
    public static bool Resizable {
        set => SDL.SDL_SetWindowResizable(Framework._window , value ? SDL.SDL_bool.SDL_TRUE : SDL.SDL_bool.SDL_FALSE);
        get => _exist_flag(SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
    }
    //other
    /// <summary>
    /// 창 생성 옵션입니다.
    /// </summary>
    public class CreateOption
    {
        internal SDL.SDL_WindowFlags _flags { get; private set; } = SDL.SDL_WindowFlags.SDL_WINDOW_HIDDEN;
        internal bool VSync { get; private set; }
        public CreateOption(bool Resizable = true , bool Borderless = false , bool AlwaysOnTop = false , bool Show = false , bool Maximized = false , bool Minimized = false , bool SkipTaskbar = false , bool OpenGL = false , bool Vulkan = false , bool Metal = false , bool Fullscreen = false,bool VSync = false)
        {
            if (Resizable)
                _flags |= SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE;
            if (Borderless)
                _flags |= SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS;
            if (AlwaysOnTop)
                _flags |= SDL.SDL_WindowFlags.SDL_WINDOW_ALWAYS_ON_TOP;
            if (Show)
                _flags |= SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN;
            if (Maximized)
                _flags |= SDL.SDL_WindowFlags.SDL_WINDOW_MAXIMIZED;
            if (Minimized)
                _flags |= SDL.SDL_WindowFlags.SDL_WINDOW_MINIMIZED;
            if (SkipTaskbar)
                _flags |= SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR;
            if (OpenGL)
                _flags |= SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL;
            if (Vulkan)
                _flags |= SDL.SDL_WindowFlags.SDL_WINDOW_VULKAN;
            if (Metal)
                _flags |= SDL.SDL_WindowFlags.SDL_WINDOW_METAL;
            if (Fullscreen)
                _flags |= SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;
            this.VSync = VSync;
        }
    }
}
