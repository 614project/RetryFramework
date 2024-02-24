using RetryFramework.SDL2;

namespace RetryFramework;

public partial class Font
{
    public class FromFile : RetryFont
    {
        public FromFile(string path)
        {
            _path = path;
        }
        public virtual string Path
        {
            get => _path; set
            {
                _path = value;
                if (IsLoad)
                {
                    Release();
                    Prepare();
                }
            }
        }
        public override void Prepare()
        {
            if ((_ptr = SDL_ttf.TTF_OpenFont(_path, _size)) == IntPtr.Zero) error_push();
        }
        private string _path;
    }
}
