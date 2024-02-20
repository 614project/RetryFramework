namespace RetryFramework.Events;
/* 
 * 인터페이스 명명 규칙:
 * 인터페이스의 이름과 요구하는 액션 변수 이름이 일치해야됨.
 */

/// <summary>
/// 리소스를 불러올때
/// </summary>
public interface Prepare
{
    public void Prepare();
}
/// <summary>
/// 리소스를 해제할때
/// </summary>
public interface Release
{
    public void Release();
}
/// <summary>
/// 요소의 크기가 조절될때
/// </summary>
public interface Resize
{
    public void Resize();
}
/// <summary>
/// 업데이트가 필요할때
/// </summary>
public interface Update
{
    public void Update();
}
/// <summary>
/// 창이 움직일때
/// </summary>
public interface WindowMove
{
    public void WindowMove();
}
/// <summary>
/// 창 제목표시줄에 있는 닫기 버튼을 눌렀을때
/// </summary>
public interface WindowClose
{
    public void WindowClose(); 
}
/// <summary>
/// 렌더링 용
/// </summary>
internal interface Rendering
{
    internal void Rendering();
}
