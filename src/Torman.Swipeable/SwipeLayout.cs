using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Torman.Swipeable
{
    public class SwipeLayout : FrameLayout
    {
        private static int BounceAnimationDuration = 800;

        private static int TranslateAnimationDuration = 300;

        private static int MaxButtons = 2;

        private float offsetInDp = 30;

        private CustomGestureDetector gestureDetector;

        private View topView;

        private LinearLayout leftSwipeButtonsView;

        private LinearLayout rightSwipeButtonsView;

        private List<View> leftSwipeButtons = new List<View>();

        private List<View> rightSwipeButtons = new List<View>();

        public SwipeLayout(Context context) : base(context)
        {
            Init();
        }

        public SwipeLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public SwipeLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public SwipeLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        internal bool HaveButtons => rightSwipeButtons.Any() || leftSwipeButtons.Any();

        internal int LeftSwipeButtonsWidth => ButtonsTotalWidth(leftSwipeButtons);

        internal int RightSwipeButtonsWidth => ButtonsTotalWidth(rightSwipeButtons);

        private int TotalAnimationDuration => TranslateAnimationDuration + BounceAnimationDuration;

        private float AnimationOffset => offsetInDp + 30 * Resources.DisplayMetrics.Density;

        public View TopView => topView;

        public View SetTopView(int resourceId, bool removeButtons = true)
        {
            var view = LayoutInflater.FromContext(Context).Inflate(resourceId, null);

            SetTopView(view, removeButtons);

            return view;
        }

        public void SetTopView(View view, bool removeButtons = true)
        {
            if (topView != null)
            {
                RemoveView(topView);
                gestureDetector = null;
                topView.SetOnTouchListener(null);
                topView = null;

                if (removeButtons)
                {
                    RemoveButtons();
                }
            }

            topView = view;
            AddView(TopView);
            gestureDetector = new CustomGestureDetector(this, offsetInDp);
            TopView.SetOnTouchListener(gestureDetector);
        }

        public void EnablePossibleSwipeAnimationOnClick(bool enable)
        {
            if (enable)
            {
                TopView.Click += (s, e) => ShowSwipeAnimation();
            }
            else
            {
                TopView.Click -= (s, e) => ShowSwipeAnimation();
            }
        }

        public void ShowSwipeAnimation()
        {
            if (TopView.TranslationX != 0 || TopView.Animation != null)
            {
                return;
            }

            SetButtonsViewHoldersVisibility(ViewStates.Visible, ViewStates.Visible);

            if (gestureDetector.LeftSwipe)
            {
                TopView.StartAnimation(ShowLeftSwipeAnimation());
            }

            if (gestureDetector.RightSwipe)
            {
                int delay = 0;

                if (gestureDetector.LeftSwipe)
                {
                    delay = TotalAnimationDuration;
                }

                TopView.PostOnAnimationDelayed(new Runnable(() => TopView.StartAnimation(ShowRightSwipeAnimation())), delay);
            }
        }

        public void RemoveButtons()
        {
            leftSwipeButtons.Clear();
            rightSwipeButtons.Clear();
            leftSwipeButtonsView.RemoveAllViews();
            rightSwipeButtonsView.RemoveAllViews();
        }

        public void AddSwipeButton(
            int imageResourceId,
            Color backgroundColor,
            Action action,
            SwipeDirection direction)
        {
            if (CannotAddButton(direction))
            {
                return;
            }

            var button = LayoutInflater.FromContext(Context).Inflate(Resource.Layout.NoMarginImageButton, null);

            AddImageToButton(button, imageResourceId);
            AddButton(button, backgroundColor, action, direction);
        }

        public void AddSwipeButton(
            string text,
            int imageResourceId,
            Color backgroundColor,
            Color fontColor,
            Action action, 
            SwipeDirection direction)
        {
            var button = LayoutInflater.FromContext(Context).Inflate(Resource.Layout.ImageAndTextButton, null);

            AddImageToButton(button, imageResourceId);
            AddTextToButton(button, text, fontColor);
            AddButton(button, backgroundColor, action, direction);
        }

        public void AddSwipeButton(
            string text,
            Color backgroundColor,
            Color fontColor,
            Action action,
            SwipeDirection direction)
        {
            if (CannotAddButton(direction))
            {
                return;
            }

            var button = LayoutInflater.FromContext(Context).Inflate(Resource.Layout.NoMarginButton, null);

            AddTextToButton(button, text, fontColor);
            AddButton(button, backgroundColor, action, direction);
        }

        public void SetOnlyLeftSwipe()
        {
            gestureDetector.LeftSwipe = true;
            gestureDetector.RightSwipe = false;
        }

        public void SetOnlyRightSwipe()
        {
            gestureDetector.LeftSwipe = false;
            gestureDetector.RightSwipe = true;
        }

        public void SetBothWaysSwipe()
        {
            gestureDetector.LeftSwipe = true;
            gestureDetector.RightSwipe = true;
        }

        internal void Movement(float movement)
        {
            var futurePosition = TopView.TranslationX + movement;

            if (futurePosition < 0)
            {
                SetButtonsViewHoldersVisibility(ViewStates.Visible, ViewStates.Gone);
            }
            else if (futurePosition > 0)
            {
                SetButtonsViewHoldersVisibility(ViewStates.Gone, ViewStates.Visible);
            }
            else
            {
                SetButtonsViewHoldersVisibility(ViewStates.Visible, ViewStates.Visible);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            ChangeButtonsHeight(MeasuredHeight, leftSwipeButtons);
            ChangeButtonsHeight(MeasuredHeight, rightSwipeButtons);
        }

        private void AddButton(
            View button, 
            Color backgroundColor,
            Action action,
            SwipeDirection direction)
        {
            button.SetBackgroundColor(backgroundColor);
            button.Clickable = true;
            button.Click += (s, e) => action();

            if (direction == SwipeDirection.Left)
            {
                leftSwipeButtons.Add(button);
                leftSwipeButtonsView.AddView(button);
                leftSwipeButtonsView.Visibility = ViewStates.Visible;
            }
            else if (direction == SwipeDirection.Right)
            {
                rightSwipeButtons.Add(button);
                rightSwipeButtonsView.AddView(button);
                rightSwipeButtonsView.Visibility = ViewStates.Visible;
            }
        }

        private bool CannotAddButton(SwipeDirection direction)
        {
            return direction == SwipeDirection.Left && leftSwipeButtons.Count == MaxButtons ||
                   direction == SwipeDirection.Right && rightSwipeButtons.Count == MaxButtons;
        }

        private void ChangeButtonsHeight(int height, List<View> collection)
        {
            foreach (var btn in collection)
            {
                btn.LayoutParameters.Height = height;
                btn.Invalidate();
            }
        }

        private void SetButtonsViewHoldersVisibility(
            ViewStates leftSwipeButtonsVisibility,
            ViewStates rightSwipeButtonsVisibility)
        {
            leftSwipeButtonsView.Visibility = leftSwipeButtonsVisibility;
            rightSwipeButtonsView.Visibility = rightSwipeButtonsVisibility;
        }

        private Animation SwipePossibilityAnimation(float farthestPoint)
        {
            var animation = new TranslateAnimation(0, farthestPoint, 0, 0);
            animation.Duration = TranslateAnimationDuration;
            animation.AnimationEnd += (s, e) =>
            {
                animation = new TranslateAnimation(farthestPoint, 0, 0, 0);
                var interpolator = new BounceInterpolator();
                animation.Interpolator = interpolator;
                animation.Duration = BounceAnimationDuration;
                TopView.StartAnimation(animation);
            };

            return animation;
        }

        private Animation ShowLeftSwipeAnimation()
        {
            return SwipePossibilityAnimation(-AnimationOffset);
        }

        private Animation ShowRightSwipeAnimation()
        {
            return SwipePossibilityAnimation(AnimationOffset);
        }

        private int ButtonsTotalWidth(List<View> buttonsColletion)
        {
            var width = 0;
            foreach (var btn in buttonsColletion)
            {
                width += btn.Width;
            }

            return width;
        }

        private void AddTextToButton(
            View button,
            string text,
            Color fontColor)
        {
            var textView = button.FindViewById<TextView>(Resource.Id.ButtonText);
            textView.Text = text;
            textView.SetTextColor(fontColor);
        }

        private void AddImageToButton(
            View button,
            int imageResourceId)
        {
            var imageView = button.FindViewById<ImageView>(Resource.Id.Image);
            imageView.SetImageResource(imageResourceId);
        }

        private void Init()
        {
            var inflater = LayoutInflater.FromContext(Context);
            inflater.Inflate(Resource.Layout.SwipeBase, this);

            leftSwipeButtonsView = FindViewById<LinearLayout>(Resource.Id.LeftSwipeButtons);
            rightSwipeButtonsView = FindViewById<LinearLayout>(Resource.Id.RightSwipeButtons);

            offsetInDp = offsetInDp * Resources.DisplayMetrics.Density;
        }
    }
}