using static RetryFramework.Types;
namespace RetryFramework.Objects;

public class Circle : Drawable
{
    // static
    public static Color DefaultColor => Color.White;
    public static short DefaultRadius => 50;

    // non-static
    public Circle(Action<Circle>? me = null) { me?.Invoke(this); }
    public short Radius { get; set; } = DefaultRadius;
    public Color FillColor { get; set; } = DefaultColor;

    internal override double actual_width => (this.Radius << 1) * this.Scale.X;
    internal override double actual_height => (this.Radius << 1) * this.Scale.Y;
}
