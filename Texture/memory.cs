namespace RetryFramework;

public partial class Texture
{
    public class FromMemory : RetryTexture
    {
        public FromMemory(IntPtr address)
        {
            _resource = address;
            Prepare();
        }
        internal override nint ptr => _resource;
        IntPtr _resource;
    }
}
