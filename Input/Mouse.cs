using RetryFramework.SDL2;

namespace RetryFramework.Input;

/// <summary>
/// 마우스와 관련된 클래스입니다.
/// </summary>
public static class Mouse
{
    /// <summary>
    /// 창 포커스를 얻기위해 마우스를 클릭했을때 생긴 입력 이벤트를 차단할지에 대한 여부입니다.
    /// </summary>
    public static bool BlockEventAtToFocus {
        get => SDL.SDL_GetHint(SDL.SDL_HINT_MOUSE_FOCUS_CLICKTHROUGH) == "1";

        set => SDL.SDL_SetHint(SDL.SDL_HINT_MOUSE_FOCUS_CLICKTHROUGH , value ? "1" : "0");
    }
    /// <summary>
    /// 마우스 커서의 위치입니다.
    /// </summary>
   public static (int X,int Y) Position {
        get {
            SDL.SDL_GetMouseState(out int x , out int y);
            return (x,y);
        }
    }
    /// <summary>
    /// 마우스를 창 밖으로 나오지 못하게 가둘지에 대한 여부입니다.
    /// </summary>
    public static bool Grab {
        set {
            SDL.SDL_SetWindowMouseGrab(Framework._window , value ? SDL.SDL_bool.SDL_TRUE : SDL.SDL_bool.SDL_FALSE);
        }
        get => SDL.SDL_GetWindowMouseGrab(Framework._window) == SDL.SDL_bool.SDL_TRUE;
    }
    /// <summary>
    /// 커서를 숨길지에 대한 여부입니다.
    /// </summary>
    public static bool HideCursor {
        set {
            SDL.SDL_ShowCursor(value ? 0 : 1);
        }
    }
    /// <summary>
    /// 커서가 숨겨져 있을때 창 테두리를 조작할수 있게 할지에 대한 여부입니다. (창 조절, 이동 등)
    /// </summary>
    public static bool BlockWindowFrame {
        set {
            SDL.SDL_SetHint(SDL.SDL_HINT_WINDOW_FRAME_USABLE_WHILE_CURSOR_HIDDEN , value ? "0" : "1");
        }
        get => SDL.SDL_GetHint(SDL.SDL_HINT_WINDOW_FRAME_USABLE_WHILE_CURSOR_HIDDEN) == "0";
    }
    public static void SetCursor(CursorType t)
    {
        SDL.SDL_SetCursor(SDL.SDL_CreateSystemCursor((SDL.SDL_SystemCursor)t));
    }
    public enum CursorType
    {
        Arrow,    // Arrow
        Ibeam,    // I-beam
        Wait,     // Wait
        Crosshair,    // Crosshair
        WaitArrow,    // Small wait cursor (or Wait if not available)
        SIZENWSE, // Double arrow pointing northwest and southeast
        SIZENESW, // Double arrow pointing northeast and southwest
        HorizonSizing,   // Double arrow pointing west and east
        VericalSizing,   // Double arrow pointing north and south
        Move,  // Four pointed arrow pointing north, south, east, and west
        NO,       // Slashed circle or crossbones
        HAND,     // Hand
        SYSTEM_CURSORS
    }
}
/// <summary>
/// 마우스 버튼 목록
/// </summary>
public enum MouseKey : byte
{
    /// <summary>
    /// 왼쪽
    /// </summary>
    Left = 1,
    /// <summary>
    /// 중간 (마우스 휠)
    /// </summary>
    Middle = 2,
    /// <summary>
    /// 오른쪽
    /// </summary>
    Right = 3
}