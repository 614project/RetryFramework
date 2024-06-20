using RetryFramework.Events;
using RetryFramework.Input;
using RetryFramework.SDL2;

namespace RetryFramework;

public class FrameworkEventHandler : AllEvents
{
    //public
    /// <summary>
    /// Window.Show = true; 를 실행합니다.
    /// </summary>
    public override void Prepare()
    {
        SDL.SDL_ShowWindow(Framework._window);
    }
    public override void Release()
    {
    }
    public override void DropFile(string FileName)
    {
        
    }
    public override void InputText()
    {
        
    }
    public override void KeyDown(Keycode key)
    {
        
    }
    public override void KeyUp(Keycode key)
    {
        
    }
    public override void KeyFocusIn()
    {
        
    }
    public override void KeyFocusOut()
    {
        
    }
    public override void MouseFocusIn()
    {
        
    }
    public override void MouseFocusOut()
    {
        
    }
    public override void MouseKeyDown(MouseKey key)
    {
        
    }
    public override void MouseKeyUp(MouseKey key)
    {
        
    }
    public override void Resize()
    {
        
    }
    public override void Resized()
    {
        
    }
    /// <summary>
    /// 모든 장면의 Update 함수를 호출합니다.
    /// </summary>
    /// <param name="MilliSecond">이전 프레임과의 시간차 (밀리초 단위)</param>
    public override void Update(float MilliSecond)
    {
        Display._scenes.ForEach(sc => sc.Update(MilliSecond));
    }
    public override void WindowMaximized()
    {
        
    }
    public override void WindowMinimized()
    {
        
    }
    public override void WindowMove()
    {
        
    }
    /// <summary>
    /// 'Window.FrameworkStopWhenClose'의 값이 true일때 'Framework.Stop'을 호출합니다.
    /// </summary>
    public override void WindowQuit()
    {
        if (Window.FrameworkStopWhenClose)
        {
            Framework.Stop();
        }
    }
    public override void WindowRestore()
    {
        
    }
    //internal
    internal virtual void _draw()
    {
        //모든 장면에 그리기 함수 호출
        Display._scenes.ForEach(sc => sc.EventHandler._draw());
    }
}
