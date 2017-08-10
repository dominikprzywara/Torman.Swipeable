using Android.Views;
using System;

namespace Torman.Swipeable
{
    public class InterceptTouchResolver
    {
        private float lastX = 0;

        private float lastY = 0;

        public bool OnInterceptTouchEvent(MotionEvent ev, Func<MotionEvent, bool> baseInterceptEvent)
        {
            switch (ev.Action)
            {
                case MotionEventActions.Down:
                    lastX = ev.GetX(ev.ActionIndex);
                    lastY = ev.GetY(ev.ActionIndex);
                    break;
            }

            var parentInterrupt = baseInterceptEvent(ev);

            if (parentInterrupt)
            {
                var x = lastX - ev.GetX(ev.ActionIndex);
                var y = lastY - ev.GetY(ev.ActionIndex);

                var angle = Math.Abs(Math.Atan2(y, x) * (180 / Math.PI));

                if (angle > 45 && angle < 135)
                {
                    return parentInterrupt;
                }
            }

            return false;
        }
    }
}