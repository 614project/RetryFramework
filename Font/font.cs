using RetryFramework.Events;
using RetryFramework.SDL2;
namespace RetryFramework.Font;

public class Font : Prepare, Release, IDisposable
{
    // static
    public static string DefaultPath = "font.ttf";
    public static int DefaultSize = 32;
    public static Error.Logger ErrorLog { get; private set; } = new();
    internal static void ErrorPush() => ErrorLog.Push(SDL_ttf.TTF_GetError());
    // non-static
    public Font(string? path = null)
    {
        _path = path ?? DefaultPath;
    }
    public virtual string Path { get => _path; set {
            _path = value;
            if (IsLoad)
            {
                Release();
                Prepare();
            }
    }}
    public virtual int Size { get => _size; set {
            _size = value;
            if (IsLoad && SDL_ttf.TTF_SetFontSize(_ptr, _size) is -1) ErrorPush();
    }}
    public virtual void Prepare()
    {
        if ((_ptr = SDL_ttf.TTF_OpenFont(_path, _size)) == IntPtr.Zero) ErrorPush();
    }
    public virtual void Release()
    {
        SDL_ttf.TTF_CloseFont(_ptr);
    }
    public virtual void Dispose() => Release();
    public bool IsLoad => _ptr != IntPtr.Zero;

    public Texture.FromMemory? Rendering(string Content,Color TextColor, Color? BackgroundColor,uint WarpLength = 0 ,bool Blended = true)
    {
        if (!IsLoad) return null;
        if (Content.Length is 0) Content = " ";
        IntPtr temp;
        if (BackgroundColor is null)
        {
            if (Blended) temp = SDL_ttf.TTF_RenderUNICODE_Blended_Wrapped(pointer, Content, TextColor.sdl_color, WarpLength);
            else temp = SDL_ttf.TTF_RenderUNICODE_Solid_Wrapped(pointer, Content, TextColor.sdl_color, WarpLength);
        }
        else temp = SDL_ttf.TTF_RenderUNICODE_Shaded_Wrapped(pointer, Content, TextColor.sdl_color, BackgroundColor.sdl_color ,WarpLength);

        if (temp == IntPtr.Zero)
        {
            ErrorPush();
            return null;
        }

        var texture_pointer = SDL.SDL_CreateTextureFromSurface(Renderer.ptr, temp);
        if (texture_pointer == IntPtr.Zero)
        {
            ErrorPush();
            return null;
        }

        return new Texture.FromMemory(texture_pointer);
    }
    internal IntPtr pointer => _ptr;

    private string _path;
    private int _size;
    private IntPtr _ptr;
}
