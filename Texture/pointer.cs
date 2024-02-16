namespace RetryFramework;

public partial class Texture
{
    public class FromPointer : RetryTexture
    {
        public FromPointer(IntPtr pointer)
        {
            _pointer = pointer;
        }

        internal override nint ptr => _pointer;

        private IntPtr _pointer;
    }
}
