using RetryFramework.Objects;

namespace RetryFramework;

public sealed partial class Animation
{
    public static double DefaultAnimateTime = 0.2;
    public static TimeCalculator DefaultAnimationEffect = Effects.Default;
    public abstract class Information
    {
        public virtual double StartTime { get; private set; }
        public virtual double AnimateTime { get; private set;}
        public virtual TimeCalculator Effect { get; set; } = DefaultAnimationEffect;

        public virtual bool Start(double? animate_time = null, double? start_time = null,bool reset_starting_point = true)
        {
            AnimateTime = animate_time ?? DefaultAnimateTime;
            StartTime = start_time ?? Window.RunningTime;
            if (reset_starting_point) reset_start();
            if (!Queue.Exist(this))
            {
                return Queue.Add(this);
            }
            return false;
        }
        /// <summary>
        /// 애니메이션을 갱신합니다.
        /// </summary>
        /// <returns>애니메이션이 끝나면 true, 아니면 false를 반환합니다.</returns>
        internal abstract bool update();
        internal abstract void done();
        internal abstract void reset_start();
    }

    public delegate double TimeCalculator(double progress);
}