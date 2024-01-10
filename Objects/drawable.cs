using static RetryFramework.Types;
namespace RetryFramework.Objects;

public abstract class Drawable : RetryObject
{
    public Drawable(Action<Drawable>? me = null) { if (me is not null) me(this); }
    public Scale RenderPosition { get; set; } = new(0.5, 0.5);
    internal abstract double actual_width { get; } //실제 렌더 너비 (그룹의 영향 안받을때)
    internal abstract double actual_height { get; } //실제 렌더 높이 (그룹의 영향 안받을때)

}
