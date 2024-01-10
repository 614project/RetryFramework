using static RetryFramework.Types;
namespace RetryFramework.Objects;

public class Rectangle : Drawable
{
    public Rectangle(Action<Rectangle>? me = null) { if (me is not null) me(this); }
    public Size Size { get; set; } = new();
    public Color FillColor { get; set;} = Color.White;

    internal override double actual_width => this.Size.Width * this.Scale.X;
    internal override double actual_height => this.Size.Height * this.Scale.Y;
}
