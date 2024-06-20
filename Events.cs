using RetryFramework.Input;

namespace RetryFramework.Events;

/// <summary>
/// 객체가 준비됨.
/// </summary>
public interface Prepare
{
    public void Prepare();
}

/// <summary>
/// 객체가 더이상 사용되지 않음.
/// </summary>
public interface Release
{
    public void Release();
}

/// <summary>
/// 글자를 입력함.
/// </summary>
public interface InputText
{
    public void InputText();
}

/// <summary>
/// 마우스 포커스를 잃음
/// </summary>
public interface MouseFocusOut
{
    public void MouseFocusOut();
}

/// <summary>
/// 마우스 포커스가 잡힘
/// </summary>
public interface MouseFocusIn
{
    public void MouseFocusIn();
}

/// <summary>
/// 키보드 포커스를 잃음
/// </summary>
public interface KeyFocusOut
{
    public void KeyFocusOut();
}

/// <summary>
/// 키보드 포커스가 잡힘
/// </summary>
public interface KeyFocusIn
{
    public void KeyFocusIn();
}

/// <summary>
/// 창에 파일을 끌어서 놓음
/// </summary>
public interface DropFile
{
    public void DropFile(string FileName);
}

/// <summary>
/// 창 크기가 바뀜 (아직 조정중)
/// </summary>
public interface Resize
{
    public void Resize();
}

/// <summary>
/// 창 크기가 조정됨 (창 조절이 완전히 끝남.)
/// </summary>
public interface Resized
{
    public void Resized();
}

/// <summary>
/// 업데이트 (렌더링과 같은 주기로 반복됨)
/// </summary>
public interface Update
{
    public void Update(float MilliSecond);
}

/// <summary>
/// 창이 이동됨
/// </summary>
public interface WindowMove
{
    public void WindowMove();
}

/// <summary>
/// 키보드에서 키를 누름
/// </summary>
public interface KeyDown
{
    public void KeyDown(Keycode key);
}

/// <summary>
/// 마우스 포인터가 움직임
/// </summary>
public interface MouseMove
{
    public void MouseMove();
}

/// <summary>
/// 창에서 나가기를 누름
/// </summary>
public interface WindowQuit
{
    public void WindowQuit();
}

/// <summary>
/// 마우스의 버튼이 눌림
/// </summary>
public interface MouseKeyDown
{
    public void MouseKeyDown(MouseKey key);
}

/// <summary>
/// 마우스의 버튼이 완화됨
/// </summary>
public interface MouseKeyUp
{
    public void MouseKeyUp(MouseKey key);
}

/// <summary>
/// 키보드에서 키가 완화됨
/// </summary>
public interface KeyUp
{
    public void KeyUp(Keycode key);
}

/// <summary>
/// 창이 최대화됨
/// </summary>
public interface WindowMaximized
{
    public void WindowMaximized();
}

/// <summary>
/// 창이 최소화됨
/// </summary>
public interface WindowMinimized
{
    public void WindowMinimized();
}

/// <summary>
/// 창이 복구됨
/// </summary>
public interface WindowRestore
{
    public void WindowRestore();
}

public abstract class AllEvents : Prepare, Release, InputText, MouseFocusIn, MouseFocusOut, KeyFocusIn, KeyFocusOut, DropFile, Resize, Resized, Update,WindowMove,KeyDown,KeyUp,WindowQuit, WindowRestore, WindowMinimized, WindowMaximized, MouseKeyDown, MouseKeyUp
{
    public abstract void Prepare();
    public abstract void Release();
    public abstract void Update(float MilliSecond);
    public abstract void WindowQuit();
    public abstract void KeyDown(Keycode key);
    public abstract void KeyUp(Keycode key);
    public abstract void DropFile(string FileName);
    public abstract void Resized();
    public abstract void Resize();
    public abstract void WindowMove();
    public abstract void InputText();
    public abstract void MouseFocusIn();
    public abstract void MouseFocusOut();
    public abstract void KeyFocusIn();
    public abstract void KeyFocusOut();
    public abstract void WindowRestore();
    public abstract void WindowMinimized();
    public abstract void WindowMaximized();
    public abstract void MouseKeyDown(MouseKey key);
    public abstract void MouseKeyUp(MouseKey key);
}