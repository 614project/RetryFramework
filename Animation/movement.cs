using RetryFramework.Objects;
using static RetryFramework.Types;

namespace RetryFramework;

partial class Animation
{
    public class Movement : Information
    {
        public RetryObject Target { get; set; }
        public PinnedPoint ArrivalPoint
        {
            get => _arrival_point; set
            {
                _arrival_point = value;
                _refresh_distance();
            }
        }
        public PinnedPoint StartingPoint
        {
            get => _starting_point; set
            {
                _starting_point = value;
                _refresh_distance();
            }
        }
        /// <summary>
        /// 이동 애니메이션을 설정합니다.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="arrival"></param>
        public Movement(RetryObject target, PinnedPoint arrival)
        {
            this.Target = target;
            _arrival_point = arrival;
            _starting_point = target.Position;
            _refresh_distance();
        }

        internal override bool update()
        {
            double progress = (Window.RunningTime - base.StartTime) / base.AnimateTime;
            if (progress >= 1)
            {
                done();
                return true;
            }
            Target.Position.X = (int)Math.Round(_starting_point.X + _distance.X * base.Effect(progress));
            Target.Position.Y = (int)Math.Round(_starting_point.Y + _distance.Y * base.Effect(progress));
            return false;
        }
        internal override void done()
        {
            Target.Position.X = ArrivalPoint.X;
            Target.Position.Y = ArrivalPoint.Y;
        }
        internal override void reset_start()
        {
            StartingPoint = Target.Position;
        }

        private void _refresh_distance()
        {
            _distance.X = _arrival_point.X - _starting_point.X;
            _distance.Y = _arrival_point.Y - _starting_point.Y;
        }
        private PinnedPoint _arrival_point, _starting_point;
        private Point _distance = new();
    }
}
