using RetryFramework.SDL2;

namespace RetryFramework;

public partial class Texture
{
    public class FromFile : RetryTexture
    {
        public FromFile(string filepath)
        {
            _path = filepath;
        }
        public string FilePath
        {
            get => _path;
            set
            {
                if (this.IsLoad)
                {
                    Release();
                    _path = value;
                    Prepare();
                }
                else _path = value;
            }
        }
        public override void Prepare()
        {
            _resource = SDL_image.IMG_LoadTexture(Renderer.ptr,_path);
            if (_resource == IntPtr.Zero)
            {
                error_push();
                return;
            }
            base.Prepare();
        }
        internal override nint ptr => _resource;
        string _path;
        IntPtr _resource;
    }
}