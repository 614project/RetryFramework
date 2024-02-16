using static RetryFramework.Types;
namespace RetryFramework.Objects;

public class Rectangle : Drawable
{
    // static
    public static Color DefaultColor => Color.White;
    public static Size DefaultSize => (100, 100);

    // non-static
    public Rectangle(Action<Rectangle>? me = null) { if (me is not null) me(this); }
    public Size Size { get; set; } = DefaultSize;
    public Color FillColor { get; set;} = Color.White;

    internal override double actual_width => this.Size.Width * this.Scale.X;
    internal override double actual_height => this.Size.Height * this.Scale.Y;
}
