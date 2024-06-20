using RetryFramework.SDL2;

namespace RetryFramework.Input;

/// <summary>
/// 텍스트 입력과 관련된 클래스입니다.
/// IME를 사용하여 다국어를 훨씬 수월하게 입력받을수 있습니다.
/// (0.8 이후부터 지원될 예정입니다.)
/// </summary>
[Obsolete("미완성")]
public static class TextInput
{
    public static string InputedText = string.Empty;

    public static int CursorPosition = 0;
    public static int SelectionLenght = 0;
    public static string SelectedText = string.Empty;

    public static uint WaitTime = 614;
    public static uint RemoveRepeatTime = 100;
    public static bool Removing { get; internal set; } = false;
    /// <summary>
    /// 텍스트 입력 활성/비활성
    /// </summary>
    [Obsolete("아직 구현되지 않은 기능")]
    public static bool Enable {
        get => SDL.SDL_IsTextInputActive() == SDL.SDL_bool.SDL_TRUE;
        set {
            if (value) SDL.SDL_StartTextInput();
            else SDL.SDL_StopTextInput();
        }
    }
    /// <summary>
    /// 텍스트가 입력되는 동안 KeyDown/KeyUp 이벤트를 무시합니다. (아직 구현되지 않음, 다음 업데이트를 위해 미리 생성)
    /// </summary>
    [Obsolete("미구현")]
    public static bool BlockKeyEvent { get; set; } = false;
}