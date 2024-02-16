using RetryFramework.SDL2;
namespace RetryFramework.Objects;

public class Rectangle : Drawable
{
    // static
    public static Color DefaultColor = Color.White;
    public static Types.Size DefaultSize = (100, 100);

    // non-static
    public Rectangle(Action<Rectangle>? ready = null) { ready?.Invoke(this); }
    public Types.Size Size { get; set; } = DefaultSize;
    public Color FillColor { get; set;} = Color.White.Copy;
    //internal
    internal override void rendering(SDL.SDL_Rect rect, Renderer.RenderOption opt)
    {
        SDL.SDL_SetRenderDrawColor(Renderer.ptr,
            FillColor.Red,
            FillColor.Green,
            FillColor.Blue,
            (byte)(FillColor.Alpha * opt.opacity)
        );
        SDL.SDL_RenderFillRect(Renderer.ptr, ref rect);
    }
    internal override double actual_width => this.Size.Width * this.Scale.X;
    internal override double actual_height => this.Size.Height * this.Scale.Y;
}
