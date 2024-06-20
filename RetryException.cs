using RetryFramework.SDL2;

namespace RetryFramework;

/// <summary>
/// RetryFramework에서 발생하는 예외입니다.
/// </summary>
internal class RetryException : Exception
{
    public RetryException(string message) : base(message) { }
    public RetryException() : base("SDL Error: " + SDL.SDL_GetError()) { }
}
