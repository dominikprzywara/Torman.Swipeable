using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;

namespace Torman.Swipeable
{
    public class SwipeableListView : ListView
    {
        private float lastX = 0;

        private float lastY = 0;
        
        public SwipeableListView(Context context) : base(context)
        {
        }

        public SwipeableListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public SwipeableListView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public SwipeableListView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            switch (ev.Action)
            {
                case MotionEventActions.Down:
                    lastX = ev.GetX(ev.ActionIndex);
                    lastY = ev.GetY(ev.ActionIndex);
                    break;
            }

            var parentInterrupt = base.OnInterceptTouchEvent(ev);

            if (parentInterrupt)
            {
                var x = lastX - ev.GetX(ev.ActionIndex);
                var y = lastY - ev.GetY(ev.ActionIndex);

                var angle = Math.Abs(Math.Atan2(y, x) * (180 / Math.PI));

                if (angle > 60 && angle < 120)
                {
                    return parentInterrupt;
                }
            }

            return false;
        }
    }
}