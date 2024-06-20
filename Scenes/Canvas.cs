using RetryFramework.SDL2;

namespace RetryFramework.Scenes;

/// <summary>
/// 직접 점,선,면 또는 이미지를 화면에 그려볼수 있는 장면입니다.
/// </summary>
public abstract class Canvas : SceneInterface
{
    public Canvas()
    {
        EventHandler = new CanvasEventHandler(this); 
    }
    public SceneEventHandler EventHandler { get; protected set; }
    public virtual void Prepare() { }
    public virtual void Update(float ms) { }
    public virtual void Release() { }
    /// <summary>
    /// 이 함수에는 화면에 어떻게 내용을 그릴지에 대한 코드를 담아두는 곳입니다. 재정의해서 사용하세요.
    /// </summary>
    public abstract void Render();

    protected static class Renderer
    {
        /// <summary>
        /// 도형을 그릴때 어떤 색을 사용할지 선택합니다.
        /// </summary>
        /// <param name="Color">색깔</param>
        /// <returns>성공시 true</returns>
        public static bool SetDrawColor(Unit.Color Color) => SDL.SDL_SetRenderDrawColor(Framework._renderer , Color.Red , Color.Green , Color.Blue , Color.Alpha) == 0;
        /// <summary>
        /// 직사각형을 그립니다. (도형 안쪽도 렌더러에 지정한 색으로 채워집니다.)
        /// </summary>
        /// <param name="Range">범위</param>
        /// <returns>성공시 true</returns>
        public static bool DrawRectangle(Unit.Range Range) => SDL.SDL_RenderFillRectF(Framework._renderer , ref Range._rect) == 0;
        /// <summary>
        /// 직사각형 모양의 테두리를 그립니다. (즉, 속이 빈 직사각형을 그립니다.)
        /// </summary>
        /// <param name="Range">범위</param>
        /// <returns>성공시 true</returns>
        public static bool DrawRectangleLine(Unit.Range Range) => SDL.SDL_RenderDrawRectF(Framework._renderer , ref Range._rect) == 0;
    }

    /// <summary>
    /// Canvas 장면을 위한 이벤트 핸들러입니다. 이 클래스를 상속해서 일부를 커스텀할수 있습니다.
    /// </summary>
    public class CanvasEventHandler : SceneEventHandler
    {
        public CanvasEventHandler(Canvas Canvas) : base(Canvas) { this.CanvasScene = Canvas; }
        public Canvas CanvasScene { get; private set; }
        internal override void _draw()
        {
            CanvasScene.Render();
        }
    }
}