using RetryFramework.SDL2;

namespace RetryFramework.Unit;

/// <summary>
/// 2차원의 범위를 저장하는 클래스입니다.
/// </summary>
public class Range
{
    internal SDL.SDL_FRect _rect; //ref 넘겨야됨. 그러므로 set은 제한하지 않음.
    public Range(float X, float Y , float Width , float Height)
    {
        _rect = new() {
            x = X ,
            y = Y ,
            w = Width ,
            h = Height
        };
    }

    public float Width { get => _rect.w; set => _rect.w = value; }
    public float Height { get => _rect.h; set => _rect.h = value; }
    public float X { get => _rect.x; set => _rect.x = value; }
    public float Y { get => _rect.y; set => _rect.y = value; }

    public override string ToString() => $"Size: {Width}×{Height}, Position: ({X},{Y}).";
}
