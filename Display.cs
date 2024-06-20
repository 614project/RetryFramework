using RetryFramework.Scenes;
using RetryFramework.SDL2;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RetryFramework;

/// <summary>
/// 화면에 어떻게 표시할지에 대한 기능을 포함되어 있습니다. 장면을 관리하고, 프레임워크의 초당 프레임률을 설정할수 있습니다.
/// </summary>
public static class Display
{
    /// <summary>
    /// 현재 할당된 장면 목록들입니다. 앞선 순으로 가장 먼저 렌더링됩니다.
    /// </summary>
    public static IEnumerable<SceneInterface> Scenes => _scenes;
    public static void AddScene(SceneInterface Scene)
    {
        Scene.Prepare();
        _scenes.Add(Scene);
    }
    public static bool RemoveScene(SceneInterface Scene)
    {
        if (_scenes.Remove(Scene))
        {
            Scene.Prepare();
            return true;
        }
        return false;
    }




    /// <summary>
    /// 프레임워크의 주사율을 설정합니다.
    /// </summary>
    public static uint FrameRate {
        get => _framerate;
        set =>_delta_time = Stopwatch.Frequency / (_framerate = value);
    }
    //internal
    internal static List<SceneInterface> _scenes = new();
    internal static uint _framerate = 60;
    internal static long _delta_time = Stopwatch.Frequency / 60;
}