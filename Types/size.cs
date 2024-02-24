namespace RetryFramework;

public class Types
{
    /// <summary>
    /// 확대/축소 수준(배율)을 정하는데 사용되는 클래스입니다.
    /// </summary>
    public class Scale
    {
        public double X, Y;
        public const double Top = 1;
        public const double Bottom = 0;
        public const double Left = 1;
        public const double Right = 0;
        public const double Center = 0.5;
        public Scale(double x = 1, double y = 1)
        {
            this.X = x;
            this.Y = y;
        }
        /// <summary>
        /// Scale 클래스를 (double,double) 형식의 값으로 변환합니다.
        /// </summary>
        /// <param name="scale"></param>
        public static implicit operator (double, double)(Scale scale) => (scale.X,scale.Y);
        /// <summary>
        /// (double,double) 형식의 값을 Scale 클래스로 변환합니다.
        /// </summary>
        /// <param name="xy">(x값,y값)</param>
        public static implicit operator Scale((double,double) xy) => new(xy.Item1, xy.Item2);
        public static implicit operator Scale(double v) => new(v, v);
    }
    public class Point // 위치
    { 
        public int X; public int Y;
        public Point(int x=0,int y=0)
        {
            this.X = x;
            this.Y = y;
        }
        public static implicit operator Point((int, int) pos) => new(pos.Item1, pos.Item2);
        public static implicit operator Point(PinnedPoint pinpoint) => new(pinpoint.X, pinpoint.Y);
    }
    public class Size //크기
    {
        public int Width, Height;
        public Size(int Width = 0, int Height = 0)
        {
            this.Width = Width;
            this.Height = Height;
        }
        public static implicit operator (int,int)(Size size) => (size.Width,size.Height);
        public static implicit operator Size((int, int) size) => new(size.Item1, size.Item2);
        public static implicit operator Size(int size) => new(size, size);
    }
    public class DrawableRange
    {
        internal SDL2.SDL.SDL_FRect rect;
        public DrawableRange()
        {
            rect = new();
        }
    }
    // X, Y 값이 더이상 바뀌면 안되는 위치 변수
    public record PinnedPoint(int X,int Y)
    {
        public static implicit operator PinnedPoint((int, int) pos) => new(pos.Item1, pos.Item2);
        public static implicit operator PinnedPoint(Point point) => new(point.X,point.Y);
    }
}