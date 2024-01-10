using RetryFramework.Error;
using RetryFramework.Events;
using RetryFramework.SDL2;

namespace RetryFramework;

public static partial class Texture
{
    public static Logger ErrorLog { get; internal set; } = new();

    internal static void error_push()
    {
        ErrorLog += SDL_image.IMG_GetError();
    }

    public abstract class RetryTexture : IDisposable ,Prepare, Release
    {
        //public
        public virtual void Prepare()
        {
            if(!size_update()) error_push();
        }
        public virtual void Release()
        {
            if (IsLoad) SDL.SDL_DestroyTexture(ptr);
        }
        public bool IsLoad => ptr != IntPtr.Zero;
        public int Width => size.w;
        public int Height => size.h;
        public void Dispose() => Release();
        //internal
        internal byte opacity
        {
            get
            {
                if (ptr == IntPtr.Zero) return 0;
                SDL.SDL_GetTextureAlphaMod(ptr, out byte result);
                return result;
            }
            set
            {
                if (ptr == IntPtr.Zero) return;
                if (SDL.SDL_SetTextureAlphaMod(ptr, value) is not 0) error_push();
            }
        }
        internal bool size_update()
        {
            if (SDL.SDL_QueryTexture(ptr, out _, out _, out size.w, out size.h) is not 0) return false;
            return true;
        }
        internal abstract IntPtr ptr { get; }
        internal SDL.SDL_Rect size = new();

    }
}
