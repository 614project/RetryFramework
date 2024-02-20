using RetryFramework.SDL2;
using static RetryFramework.Font;

namespace RetryFramework.Objects;

public class Text : Drawable
{
    // static
    public static Color DefaultColor = Color.Black;
    public static string DefaultContent = "";
    // non-static
    /// <summary>
    /// 문자열을 출력하는 객체를 생성합니다.
    /// </summary>
    /// <param name="ready">준비</param>
    /// <param name="path">글꼴파일 경로. null 일경우 Font.DefaultPath를 가져옵니다.</param>
    public Text(Action<Text>? ready = null, string? path = null)
    {
        ready?.Invoke(this);
        if (this.Font is null) Font = new FromFile(path);
    }
    public RetryFont? Font = null;
    public Color TextColor = DefaultColor.Copy;
    public Color? BackgroundColor = null;
    public override bool Hide { get => base.Hide || Font is null; set => base.Hide = value; }
    public string Content { get => _content; set {
            _content = value;
            if (_texture is not null && _texture.IsLoad) _texture.Release();
            _refresh = true;
        }
    }
    public override void Update()
    {
        base.Update();
        _render_update();
    }
    public override void Prepare()
    {
        base.Prepare();
        if (Font is not null && !Font.IsLoad) Font.Prepare();
    }
    internal override void rendering(SDL.SDL_Rect rect, Renderer.RenderOption opt)
    {
        if (_texture is null) return;
        _texture.opacity = (byte)(Opacity * opt.opacity * 255);
        SDL.SDL_RenderCopy(Renderer.ptr, _texture.ptr, ref _texture.size, ref rect);
    }
    internal override double actual_width => _texture.Width * Scale.X;
    internal override double actual_height => _texture.Height * Scale.Y;

    private void _render_update()
    {
        if (Font is null || !_refresh) return;
        _refresh = false;
        _texture = Font.Rendering(_content, TextColor, BackgroundColor);
    }
    private bool _refresh = true;
    private string _content = DefaultContent;
    private Texture.RetryTexture? _texture;
}
