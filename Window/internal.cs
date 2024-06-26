﻿using RetryFramework.SDL2;
using System.Diagnostics;

namespace RetryFramework;

public partial class Window {
    internal double running_time_for_update { get; private set; } = 0;
    internal int now_display_index => SDL.SDL_GetWindowDisplayIndex(_winptr);
    internal static Stopwatch stopwatch = new();
}