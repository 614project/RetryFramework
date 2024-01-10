namespace RetryFramework.Error;

/// <summary>
/// 프레임워크 내 무시할수 없는 문제가 발생될 경우에 생기는 예외입니다.
/// </summary>
public class RetryFrameworkException : Exception
{
    public RetryFrameworkException(string message) : base(message) { }
    public RetryFrameworkException() { }
}
