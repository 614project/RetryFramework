namespace RetryFramework;

public class Color
{
    public byte Red, Green, Blue, Alpha;
    public Color(byte red=255, byte green=255, byte blue=255, byte alpha=255)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }
    public Color((double,double,double,double) color) : this(
        (byte)(color.Item1*255),
        (byte)(color.Item2* 255),
        (byte)(color.Item3* 255),
        (byte)(color.Item4* 255)
    ) { }
    //흑백 계열
    public static Color White => new(255, 255, 255, 255);
    public static Color Black => new(0, 0, 0, 0);
    public static Color Gray => new(127, 127, 127);
    public static Color Silver => new(192, 192, 192);
    public static Color DarkGray => new(63, 63, 63);
    //보라 계열
    public static Color Purple => new(128, 0, 128);
    public static Color Violet => new(127, 0, 255);
    public static Color Lilac => new(200, 162, 200);
    public static Color Lavender => new(230, 230, 255);
    //남보라 계열
    public static Color Periwinkle => new(128, 128, 255);
    public static Color LightPeriwinkle => new(204, 204, 255);
    //남색 계열
    public static Color Indigo => new(75, 0, 130);
    public static Color Navy => new(0, 0, 128);
    //초록 계열
    public static Color Lime => new(0, 255, 0);
    public static Color DarkGreen => new(0, 128, 0);
    //연두 계열
    public static Color YellowGreen => new(154, 205, 50);
    public static Color GreenYellow => new(173, 255, 47);
    public static Color Chartreuse => new(127, 255, 0);
    public static Color GrassGreen => new(117, 166, 74);
    public static Color YellowishGreen => new(160, 176, 54);
    //청록 계열
    //추가 예정
    //노랑 계열
    public static Color Yellow => new(255, 255, 0);
    public static Color Turbo => new(255, 204, 33);
    public static Color MoonYellow => new(240, 196, 32);
    public static Color VividYellow => new(255, 227, 2);
    public static Color GoldenYellow => new(255, 140, 0);
    //주황 계열
    public static Color Orange => new(255, 127, 0);
    public static Color DarkOrange => new(255, 140, 0);

    //암묵적 형변환
    public static implicit operator Color((byte, byte, byte, byte) color) => new(color.Item1, color.Item2, color.Item3, color.Item4);
}
