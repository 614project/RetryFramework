﻿using RetryFramework.Objects;
using RetryFramework.SDL2;

namespace RetryFramework;

public class Renderer
{
    internal static IntPtr ptr;

    /// <summary>
    /// Retry 객체를 연산합니다. 그릴수 있는 객체는 렌더링합니다.
    /// </summary>
    /// <param name="obj">객체</param>
    /// <param name="opt">렌더링 옵션</param>
    internal static void RenderRetryObject(RetryObject obj, RenderOption opt)
    {
        if (obj.Hide) return;
        if (obj is Drawable drobj)
        {
            if (drobj.Hide) return;
            drobj.rendering(CalcRenderRect(drobj,opt),opt);
            return;
        }
        if (obj is Group group)
        {
            RenderOption newopt = new(
                opt.scale_x * group.Scale.X,
                opt.scale_y * group.Scale.Y,
                opt.opacity * group.Opacity,
                opt.x + group.Position.X + (group.Origin.X * opt.width),
                opt.y + group.Position.Y + (group.Origin.Y * opt.height),
                opt.width * group.Scale.X,
                opt.height * group.Scale.Y
            );
            newopt.x -= (group.Origin.X * newopt.width);
            newopt.y -= (group.Origin.Y * newopt.height);
            foreach(var member in group.Member.List)
            {
                RenderRetryObject(member, newopt);
            }
        }
    }

    internal static SDL.SDL_Rect CalcRenderRect(Drawable drawable,RenderOption opt)
    {
        SDL.SDL_Rect rect = new()
        {
            x = (int)(drawable.Position.X * opt.scale_x + opt.x),
            y = (int)(drawable.Position.Y * opt.scale_y + opt.y),
            w = (int)(drawable.actual_width * opt.scale_x),
            h = (int)(drawable.actual_height * opt.scale_y)
        };
        rect.x += (int)(opt.width * drawable.Origin.X);
        rect.y += (int)(opt.height * drawable.Origin.Y);
        rect.x -= (int)(rect.w * drawable.RenderPosition.X);
        rect.y -= (int)(rect.h * drawable.RenderPosition.Y);
        return rect;
    }

    internal class RenderOption {
        public double scale_x;
        public double scale_y;
        public double opacity;
        public double x;
        public double y;
        public double width;
        public double height;

        public RenderOption(
            double scale_x = 1,
            double scale_y = 1,
            double opacity = 1,
            double x = 0,
            double y = 0,
            double width = 0,
            double height = 0
        )
        {
            this.scale_x = scale_x;
            this.scale_y = scale_y;
            this.opacity = opacity;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    };
}