using RetryFramework.SDL2;

namespace RetryFramework;

public partial class Texture
{
    /// <summary>
    /// 바이너리에서 이미지를 로드합니다.
    /// </summary>
    public class FromBinary : RetryTexture
    {
        public FromBinary(byte[]? binary = null)
        {
           Binary= binary;
        }
        /// <summary>
        /// 바이너리. (설정하지 않았거나 성공적으로 불러왔을경우 null을 반환합니다.)
        /// </summary>
        public byte[]? Binary
        {
            get => _bytes;
            set
            {
                if (IsLoad)
                {
                    Release();
                    _bytes = value;
                    Prepare();
                }
                else _bytes = value;
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
                    _resource = SDL_image.IMG_LoadTexture_RW(Renderer.ptr,stream, 614);
                    if (_resource == IntPtr.Zero)
                    {
                        error_push();
                        return;
                    }
                }
            }
            _bytes = null!;
            base.Prepare();
        }
        //internal
        internal override nint ptr => _resource;
        //private
        IntPtr _resource;
        byte[]? _bytes;
    }
}
