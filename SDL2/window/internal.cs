using RetryFramework.SDL2;

namespace RetryFramework;

public partial class Window {
    internal double running_time_for_update { get; private set; } = 0;
    internal int now_display_index => SDL.SDL_GetWindowDisplayIndex(_winptr);
    [Obsolete("This will be removed when SDL 3.0 release.")]
    internal bool bypass_rendering_stopped_when_resizing { get; private set; } = false;
}