namespace RetryFramework;

partial class Animation
{
    public static class Effects
    {
        public static double Default(double x) => x;
        public static double EaseInSine(double x) => 1d - Math.Cos((x * Math.PI) * 0.5d);
        public static double EaseOutSine(double x) => Math.Sin((x * Math.PI) * 0.5d);
        public static double EaseInOutSine(double x) => -(Math.Cos(Math.PI * x) - 1d) * 0.5d;
        public static double EaseInQuad(double x) => x * x;
        public static double EaseOutQuad(double x) => 1 - ((x = 1d - x) * x);
        public static double EaseInOutQuad(double x) => x < 0.5d ? 2d * x * x : 1d - Math.Pow(-2d * x + 2d, 2d) * 0.5d;
    }
}
