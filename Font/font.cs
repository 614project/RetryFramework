using RetryFramework.Events;
using RetryFramework.SDL2;
namespace RetryFramework;

public partial class Font 
{
    // static
    public static string DefaultPath = "font.ttf";
    public static int DefaultSize = 32;
    public static Error.Logger ErrorLog { get; private set; } = new();
    internal static void error_push() => ErrorLog.Push(SDL_ttf.TTF_GetError());

    public abstract class RetryFont : Prepare, Release, IDisposable
    {
        public virtual int Size
        {
            get => _size; set
            {
                _size = value;
                if (IsLoad && SDL_ttf.TTF_SetFontSize(_ptr, _size) is -1) error_push();
            }
        }
        public abstract void Prepare();
        public virtual void Release()
        {
            SDL_ttf.TTF_CloseFont(_ptr);
            _ptr = IntPtr.Zero;
        }
        public virtual void Dispose() => Release();
        public bool IsLoad => _ptr != IntPtr.Zero;

        public Texture.FromMemory? Rendering(string Content, Color TextColor, Color? BackgroundColor, uint WarpLength = 0, bool Blended = true)
        {
            if (!IsLoad) return null;
            if (Content.Length is 0) Content = " ";
            IntPtr temp;
            if (BackgroundColor is null)
            {
                if (Blended) temp = SDL_ttf.TTF_RenderUNICODE_Blended_Wrapped(pointer, Content, TextColor.sdl_color, WarpLength);
                else temp = SDL_ttf.TTF_RenderUNICODE_Solid_Wrapped(pointer, Content, TextColor.sdl_color, WarpLength);
            }
            else temp = SDL_ttf.TTF_RenderUNICODE_Shaded_Wrapped(pointer, Content, TextColor.sdl_color, BackgroundColor.sdl_color, WarpLength);

            if (temp == IntPtr.Zero)
            {
                error_push();
                return null;
            }

            var texture_pointer = SDL.SDL_CreateTextureFromSurface(Renderer.ptr, temp);
            SDL.SDL_FreeSurface(temp);
            if (texture_pointer == IntPtr.Zero)
            {
                error_push();
                return null;
            }

            return new Texture.FromMemory(texture_pointer);
        }
        internal IntPtr pointer => _ptr;

        private protected int _size;
        private protected IntPtr _ptr = IntPtr.Zero;
    }
}
