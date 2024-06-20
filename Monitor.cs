using RetryFramework.SDL2;

namespace RetryFramework;

/// <summary>
/// 모니터와 관련된 정보가 담겨있습니다. (프레임워크를 초기화하지 않으면 작동하지 않습니다 .'Framework.Init'를 호출해주세요.)
/// </summary>
public static class Monitor
{
    //모니터 정보
    /// <summary>
    /// 사용 가능한 디스플레이(모니터)의 개수 입니다.
    /// </summary>
    public static int AvailableCount { get; private set; }
    /// <summary>
    /// 디스플레이의 정보들을 담은 리스트입니다. 
    /// </summary>
    public static List<Information> List = new();
    /// <summary>
    /// 디스플레이 정보를 새롭게 갱신합니다. (프레임워크가 초기화 된 상태에서만 유효합니다.)
    /// </summary>
    /// <returns>성공시 null, 실패시 SDL2 오류 메시지가 반환됩니다.</returns>
    public static string? Update()
    {
        //사용 가능한 모니터 개수 가져오기
        int count = SDL.SDL_GetNumVideoDisplays();
        if (count < 0)
        {
            return SDL.SDL_GetError();
        }
        AvailableCount = count;
        //추가하기.
        List.Clear();
        for (int i = 0 ; i < count ; i++)
        {
            List.Add(new(i));
        }
        return null;
    }

    /// <summary>
    /// 모니터에 대한 정보입니다.
    /// </summary>
    public class Information
    {
        /// <summary>
        /// 픽셀 너비
        /// </summary>
        public int Width { get; init; }
        /// <summary>
        /// 픽셀 높이
        /// </summary>
        public int Height { get; init; }
        /// <summary>
        /// 주사율
        /// </summary>
        public int FrameRate { get; init; }
        /// <summary>
        /// 픽셀당 비트수
        /// </summary>
        public uint BitsPerPixel { get; init; }
        public string PixelFormat { get; init; }
        public IntPtr DriverData { get; init; }

        public Information(int Index)
        {
            if (SDL.SDL_GetDesktopDisplayMode(Index , out var mode) != 0)
                throw new RetryException();
            this.Width = mode.w;
            this.Height = mode.h;
            this.FrameRate = mode.refresh_rate;
            this.BitsPerPixel = SDL.SDL_BITSPERPIXEL(mode.format);
            this.PixelFormat = SDL.SDL_GetPixelFormatName(mode.format);
            this.DriverData = mode.driverdata;
        }

        public override string ToString()
        {
            return $"Size: {Width}×{Height}, Frame rate: {FrameRate}, Pixel format: {PixelFormat}.";
        }
    }
}
