using RetryFramework.Error;
using RetryFramework.SDL2;

namespace RetryFramework;

public class ImageOnMemory : IDisposable
{
    public static Logger ErrorLog { get; } = new();
    static void push_error() => ErrorLog.Push(SDL_image.IMG_GetError());

    SDL.SDL_Surface _surface;
    IntPtr _surface_ptr;

    public ImageOnMemory(IntPtr address)
    {
        _init(address);
    }

    private bool _init(IntPtr address)
    {
        _surface_ptr = address;
        if (_surface_ptr == IntPtr.Zero)
        {
            push_error();
            return false;
        }
        _surface = System.Runtime.InteropServices.Marshal.PtrToStructure<SDL.SDL_Surface>(_surface_ptr);
        _format = System.Runtime.InteropServices.Marshal.PtrToStructure<SDL.SDL_PixelFormat>(_surface.format);
        _bpp = _format.BytesPerPixel;
        unsafe
        {
            switch (_bpp)
            {
                case 1:
                    pp = Process.One;
                    break;
                case 2:
                    pp = Process.Two;
                    break;
                case 3:
                    pp = Process.Three;
                    break;
                case 4:
                    pp = Process.Four;
                    break;
                default:
                    pp = Process.Default;
                    break;
            }
        }
        return true;
    }

    private void _init(ImageOnMemory me)
    {
        this._surface = me._surface;
        this._surface_ptr = me._surface_ptr;
        this.pp = me.pp;
        this._format = me._format;
        this._bpp = me._bpp;
    }

    public ImageOnMemory(string path) : this(SDL_image.IMG_Load(path))
    {

    }

    public int Width => _surface.w; public int Height => _surface.h;

    Process.PixelProcesser pp = null!;

    SDL.SDL_PixelFormat _format;
    int _bpp;

    public unsafe UInt32 GetPixel(int x, int y)
    {
        return pp((byte*)_surface.pixels + y * _surface.pitch + x * _bpp);
    }

    public void GetRGBA(int x, int y, out byte r, out byte g, out byte b, out byte a)
    {
        uint p = GetPixel(x, y);
        SDL.SDL_GetRGBA(p, _surface.format, out r, out g, out b, out a);
    }

    public ImageOnMemory? GetResizedImage(int width, int height)
    {
        IntPtr result = SDL.SDL_CreateRGBSurfaceWithFormat(0, width, height, 32, SDL.SDL_PIXELFORMAT_ARGB8888);
        SDL.SDL_Rect origin = new() { x = 0, y = 0, w = _surface.w, h = _surface.h };
        SDL.SDL_Rect targetsize = new() { x = 0, y = 0, w = width, h = height };
        SDL.SDL_LowerBlitScaled(_surface_ptr, ref origin, result, ref targetsize);
        if (result == IntPtr.Zero)
        {
            push_error();
            return null;
        }
        return new(result);
    }

    public bool Resize(int width, int height)
    {
        var result = GetResizedImage(width, height);
        if (result is null) return false;
        this.Dispose();
        this._init(result);
        return true;
    }

    public Texture.RetryTexture? GetTexture()
    {
        if (_surface_ptr == IntPtr.Zero)
        {
            return null;
        }
        var texture = SDL.SDL_CreateTextureFromSurface(Renderer.ptr, this._surface_ptr);
        if (texture == IntPtr.Zero) return null;
        return new Texture.FromMemory(texture);
    }

    public void Dispose()
    {
        if (_surface_ptr == IntPtr.Zero) return;
        SDL.SDL_FreeSurface(_surface_ptr);
        _surface_ptr = IntPtr.Zero;
    }

    private static unsafe class Process
    {
        internal delegate UInt32 PixelProcesser(byte* p);

        internal static UInt32 One(byte* p)
        {
            return *p;
        }

        internal static UInt32 Two(byte* p)
        {
            return *(UInt16*)p;
        }

        internal static UInt32 Three(byte* p)
        {
            return (UInt32)(p[0] | p[1] << 8 | p[2] << 16);
        }

        internal static UInt32 Four(byte* p)
        {
            return *(UInt32*)p;
        }

        internal unsafe static UInt32 Default(byte* p)
        {
            ErrorLog.Push("can't read pixel.");
            return 614;
        }
    }
}
