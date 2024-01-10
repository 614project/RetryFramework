using static RetryFramework.Texture;
namespace RetryFramework.Objects;

public class Image : Drawable
{
    /// <summary>
    /// 이미지를 렌더링하는 객체입니다.
    /// </summary>
    /// <param name="texture">텍스쳐. null 일때는 Hide가 항상 true입니다.</param>
    /// <param name="ready">객체 생성 마지막에 실행될 함수. (없으면 null)</param>
    public Image(RetryTexture? texture,Action<Image>? ready = null)
    {
        _texture = texture;
        if (ready is not null) ready(this);
    }
    public Image(Action<Image>? ready = null)
    {
        if (ready is not null) ready(this);
    }
    public Image(string filepath,Action<Image>? ready = null)
    {
        _texture = new FromPath(filepath);
        if (ready is not null) ready(this);
    }
    public override bool Hide { get => (base.Hide | _texture is null); set => base.Hide = value; }
    public override void Prepare()
    {
        if (_texture is not null)
        {
            _texture.Prepare();
        }
        base.Prepare();
    }
    public override void Release()
    {
        if (_texture is not null) _texture.Release();
        base.Release();
    }
    public virtual RetryTexture? Texture
    {
        get => _texture; set
        {
            /*
             * 기존 텍스쳐가 null 이면? -> 변경.
             * 기존 텍스쳐가 null이 아니면? -> 사용중이면 할당 해제후 변경.
             */
            if (_texture is not null && _texture.IsLoad)
            {
                _texture.Release();
            }
            _texture = value;
            if(value is not null) value.Prepare();
        }
    }
    //internal
    internal override double actual_width => _texture is null ? 0 : (_texture.Width * Scale.X);
    internal override double actual_height => _texture is null ? 0 : (_texture.Height * Scale.Y);
    //private
    RetryTexture? _texture = null;
}
