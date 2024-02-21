using RetryFramework.Error;
using RetryFramework.SDL2;
namespace RetryFramework;

/// <summary>
/// 긴 소리를 재생하기에 적합한 음악 객체입니다.
/// </summary>
public class Music : Playable
{
    //static
    public static Logger ErrorLog { get; internal set; } = new();
    public static Music? Load(string path) => FromFile(path);
    public static Music? Load(byte[] binary) => FromBinary(binary);
    public static Music? FromFile(string path) => new(path);
    public static Music? FromBinary(byte[] binary) => new(binary);
    /// <summary>
    /// 음악이 현재 재생중인지에 대한 여부. (일시정지도 재생중으로 취급합니다.)
    /// </summary>
    public static bool IsNowPlaying => SDL_mixer.Mix_PlayingMusic() is 1;
    /// <summary>
    /// 음악 재생을 중단합니다.
    /// </summary>
    public static void Stop() => SDL_mixer.Mix_HaltMusic();
    /// <summary>
    /// 음악을 일시중지 합니다.
    /// </summary>
    public static void Pause() => SDL_mixer.Mix_PauseMusic();
    /// <summary>
    /// 일시중지된 음악을 재개합니다. (일시중지된 음악이 없다면 아무일도 일어나지 않습니다.)
    /// </summary>
    public static void Resume() => SDL_mixer.Mix_ResumeMusic();
    internal static void error_push() => ErrorLog.Push(SDL_mixer.Mix_GetError());
    //public
    public Music(string path)
    {
        _load(path);
    }
    public Music(byte[] binary)
    {
        _load(binary);
    }
    public virtual bool Play(int repeat = 0)
    {
        if (SDL_mixer.Mix_PlayMusic(_ptr, repeat) is -1)
        {
            error_push();
            return false;
        }
        return true;
    }
    public override void Release()
    {
        SDL_mixer.Mix_FreeMusic(_ptr);
        _ptr = IntPtr.Zero;
    }
    // private
    bool _load(string path)
    {
        if ((_ptr = SDL_mixer.Mix_LoadMUS(path)) == IntPtr.Zero)
        {
            error_push();
            return false;
        }
        return true;
    }
    bool _load(byte[] _bytes)
    {
        unsafe
        {
            fixed (byte* ptr = _bytes)
            {
                IntPtr stream = SDL.SDL_RWFromConstMem((nint)ptr, _bytes.Length);
                if (stream == IntPtr.Zero)
                {
                    error_push();
                    return false;
                }
                _ptr = SDL_mixer.Mix_LoadMUS_RW(stream, 614);
                if (_ptr == IntPtr.Zero)
                {
                    error_push();
                    return false;
                }
            }
        }
        _bytes = null!;
        return true;
    }
}
