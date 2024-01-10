using RetryFramework.SDL2;

namespace RetryFramework;

public class Display
{
    public static Information[] Displays { get; internal set; } = [];
    public static int Current { get; internal set; } = 0;
    public static int RefreshRate => Displays[Current].RefreshRate;

    public static bool Update()
    {
        bool not_error = true;
        int count = SDL.SDL_GetNumVideoDisplays();
        Displays = new Information[count];
        for(int i=0;i<count;i++)
        {
            //정보 구하기
            DPI? dpi = SDL.SDL_GetDisplayDPI(i,out float ddpi,out float hdpi,out float vdpi) is 0 ? new(ddpi,hdpi,vdpi) : null;
            //Types.Range? usable_size = SDL.SDL_GetDisplayUsableBounds(i, out var ret) is 0 ? new(ret) : null;
            SDL.SDL_GetDisplayBounds(i, out var ret);
            string name = SDL.SDL_GetDisplayName(i);
            Orientation orientation = (Orientation)SDL.SDL_GetDisplayOrientation(i);
            if (SDL.SDL_GetDisplayMode(i, 0, out SDL.SDL_DisplayMode mode) is not 0)
            {
                not_error = false;  
            }
            //배열에 저장
            Displays[i] = new(
                name,
                ret.w,
                ret.h,
                mode.refresh_rate,
                orientation,
                dpi
            );
        }
        return not_error;
    } 
    /// <summary>
    /// 디스플레이에 대한 정보
    /// </summary>
    /// <param name="Name">이름</param>
    /// <param name="Width">너비</param>
    /// <param name="Height">높이</param>
    /// <param name="RefreshRate">주사율</param>
    // <param name="UsableSize">실제 사용 가능한 범위 (윈도우에선 작업표시줄을 제외했을때 범위)</param>
    /// <param name="Orientation">디스플레이 방향</param>
    /// <param name="DPI">DPI</param>
    public record Information(string Name, int Width, int Height,int RefreshRate ,Orientation Orientation, DPI? DPI);
    /// <summary>
    /// DPI 정보
    /// </summary>
    /// <param name="Diagonal">대각선</param>
    /// <param name="Horizontal">수평</param>
    /// <param name="Vertical">수직</param>
    public record DPI(float Diagonal,float Horizontal,float Vertical);
    public enum Orientation : byte
    {
        Unknown = SDL.SDL_DisplayOrientation.SDL_ORIENTATION_UNKNOWN,
        Landscape = SDL.SDL_DisplayOrientation.SDL_ORIENTATION_LANDSCAPE,
        FilppedLandscape = SDL.SDL_DisplayOrientation.SDL_ORIENTATION_LANDSCAPE_FLIPPED,
        Portrait = SDL.SDL_DisplayOrientation.SDL_ORIENTATION_PORTRAIT,
        FilppedPortrait = SDL.SDL_DisplayOrientation.SDL_ORIENTATION_PORTRAIT_FLIPPED
    }
}

