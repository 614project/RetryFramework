using RetryFramework.SDL2;

namespace RetryFramework;

public partial class Font
{
    public class FromBinary : RetryFont
    {
        public FromBinary(byte[]? binary = null)
        {
            _bytes = binary;
        }
        public byte[]? Binary
        {
            get => _bytes;
            set
            {
                if ((_bytes = value) is null)
                {
                    if (IsLoad) Release();
                    return;
                }
                if (IsLoad)
                {
                    Release();
                    Prepare();
                }
            }
        }
        public override void Prepare()
        {
            if (_bytes is null) return;
            unsafe
            {
                fixed (byte* ptr = _bytes)
                {
                    IntPtr stream = SDL.SDL_RWFromConstMem((nint)ptr, _bytes.Length);
                    if (stream == IntPtr.Zero)
                    {
                        error_push();
                        return;
                    }
                    _ptr = SDL_ttf.TTF_OpenFontRW(stream, 614, this.Size);
                    if (_ptr == IntPtr.Zero)
                    {
                        error_push();
                        return;
                    }
                }
            }
            _bytes = null!;
        }
        byte[]? _bytes = null;
    }
}
