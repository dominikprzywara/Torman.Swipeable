using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Torman.Swipeable
{

    public class SwipeableListView : ListView
    {
        private InterceptTouchResolver interceptTouchResolver = new InterceptTouchResolver();

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
            return interceptTouchResolver.OnInterceptTouchEvent(ev, base.OnInterceptTouchEvent);
        }
    }
}