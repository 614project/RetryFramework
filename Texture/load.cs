namespace RetryFramework;

public static partial class Texture
{
    public static FromFile Load(string path) => new (path);
    public static FromBinary Load(byte[] bytes) => new (bytes);
}
