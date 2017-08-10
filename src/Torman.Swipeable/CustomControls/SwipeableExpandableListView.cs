using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Torman.Swipeable
{
    public class SwipeableExpandableListView : ExpandableListView
    {
        private InterceptTouchResolver interceptTouchResolver = new InterceptTouchResolver();

        public SwipeableExpandableListView(Context context) : base(context)
        {
        }

        public SwipeableExpandableListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public SwipeableExpandableListView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public SwipeableExpandableListView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return interceptTouchResolver.OnInterceptTouchEvent(ev, base.OnInterceptTouchEvent);
        }
    }
}