using RetryFramework.SDL2;

namespace RetryFramework.Objects;

public class Text : Drawable
{
    // static
    public static Color DefaultColor = Color.Black;
    // non-static
    /// <summary>
    /// 문자열을 출력하는 객체를 생성합니다.
    /// </summary>
    /// <param name="ready">준비</param>
    /// <param name="path">글꼴파일 경로. null 일경우 Font.DefaultPath를 가져옵니다.</param>
    public Text(Action<Text>? ready = null, string? path = null) : this(new Font.Font(path), ready) { }
    public Text(Font.Font font,Action<Text>? ready = null)
    {
        ready?.Invoke(this);
        Font = font;
    }
    public Font.Font? Font = null;
    public Color TextColor = DefaultColor.Copy;
    public Color? BackgroundColor = null;
    public override bool Hide { get => base.Hide || Font is null; set => base.Hide = value; }
    public string Content { get => _content; set {
            _content = value;
            if (_texture is not null && _texture.IsLoad) _texture.Release();
            _texture = Font?.Rendering(_content, TextColor, BackgroundColor);
    }}

    internal override void rendering(SDL.SDL_Rect rect, Renderer.RenderOption opt)
    {
        if (_texture is null) return;
        _texture.opacity = (byte)(Opacity * opt.opacity * 255);
        SDL.SDL_RenderCopy(Renderer.ptr, _texture.ptr, ref _texture.size, ref rect);
    }
    internal override double actual_width => throw new NotImplementedException();
    internal override double actual_height => throw new NotImplementedException();

    private string _content;
    private Texture.RetryTexture? _texture;
}
