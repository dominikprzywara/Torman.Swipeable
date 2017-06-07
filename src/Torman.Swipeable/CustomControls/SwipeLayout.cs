using Android.Content;
using Android.Content.Res;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Torman.Swipeable.Animations;
using Torman.Swipeable.Builders;
using Torman.Swipeable.Container;
using Torman.Swipeable.Enums;
using Torman.Swipeable.Gesture;

namespace Torman.Swipeable
{
    public class SwipeLayout : FrameLayout
    {
        private ButtonsContainer leftSwipeButtonsContainer;

        private ButtonsContainer rightSwipeButtonsContainer;

        private CustomGestureDetector gestureDetector;

        private View topView;

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

        internal bool HaveButtons => rightSwipeButtonsContainer.HaveButtons || leftSwipeButtonsContainer.HaveButtons;

        internal int LeftSwipeButtonsWidth => leftSwipeButtonsContainer.ButtonsTotalWidth();

        internal int RightSwipeButtonsWidth => rightSwipeButtonsContainer.ButtonsTotalWidth();

        public View TopView => topView;

        public SwipeAnimation SwipePossibilityAnimation { get; set; }

        public View SetTopView(int resourceId, bool removeButtons = true)
        {
            var view = LayoutInflater.FromContext(Context).Inflate(resourceId, null);
            SetTopView(view, removeButtons);

            return view;
        }

        public SwipeLayout SetTopView(View view, bool removeButtons = true)
        {
            RemoveTopView(removeButtons);

            topView = view;
            AddView(TopView);

            gestureDetector = new CustomGestureDetector(this, SwipePossibilityAnimation.OffsetInDp);
            gestureDetector.MovementChanged += GestureDetector_MovementChanged;
            TopView.SetOnTouchListener(gestureDetector);

            return this;
        }

        public SwipeLayout EnablePossibleSwipeAnimationOnClick(bool enable)
        {
            if (enable)
            {
                TopView.Click += (s, e) => ShowSwipeAnimation();
            }
            else
            {
                TopView.Click -= (s, e) => ShowSwipeAnimation();
            }

            return this;
        }

        public void ShowSwipeAnimation()
        {
            if (TopView.TranslationX != 0 || TopView.Animation != null)
            {
                return;
            }

            SetButtonsViewHoldersVisibility(ViewStates.Visible, ViewStates.Visible);

            if (gestureDetector.LeftSwipePossible)
            {
                TopView.StartAnimation(SwipePossibilityAnimation.ShowLeftSwipeAnimation());
            }

            if (gestureDetector.RightSwipePossible)
            {
                int delay = 0;

                if (gestureDetector.LeftSwipePossible)
                {
                    delay = SwipePossibilityAnimation.TotalAnimationDuration;
                }

                TopView.PostOnAnimationDelayed(new Runnable(() => TopView.StartAnimation(SwipePossibilityAnimation.ShowRightSwipeAnimation())), delay);
            }
        }

        public void RemoveButtons()
        {
            leftSwipeButtonsContainer.RemoveButtons();
            rightSwipeButtonsContainer.RemoveButtons();
        }

        public SwipeLayout AddCustomSwipeButton(
            View customButtonView,
            SwipeDirection direction)
        {
            AddButton(customButtonView, direction);
            return this;
        }

        public SwipeLayout AddCustomSwipeButton(
            int customButtonLayoutId,
            SwipeDirection direction)
        {
            var button = LayoutInflater.FromContext(Context).Inflate(customButtonLayoutId, null);

            AddButton(button, direction);
            return this;
        }


        public SwipeLayout AddSwipeButton(
            ButtonBuilder buttonBuilder,
            SwipeDirection direction)
        {
            AddButton(buttonBuilder.Construct(Context), direction);
            return this;
        }

        public SwipeLayout SetOnlyLeftSwipe()
        {
            gestureDetector.SetOnlyLeftSwipe();
            return this;
        }

        public SwipeLayout SetOnlyRightSwipe()
        {
            gestureDetector.SetOnlyRightSwipe();
            return this;
        }

        public SwipeLayout SetBothWaysSwipe()
        {
            gestureDetector.SetBothWaysSwipe();
            return this;
        }

        private void AddButton(
            View button,
            SwipeDirection direction)
        {
            if (direction == SwipeDirection.Left)
            {
                leftSwipeButtonsContainer.AddButton(button);
            }
            else if (direction == SwipeDirection.Right)
            {
                rightSwipeButtonsContainer.AddButton(button);
            }
        }

        private void SetButtonsViewHoldersVisibility(
            ViewStates leftSwipeButtonsVisibility,
            ViewStates rightSwipeButtonsVisibility)
        {
            leftSwipeButtonsContainer.Visibility = leftSwipeButtonsVisibility;
            rightSwipeButtonsContainer.Visibility = rightSwipeButtonsVisibility;
        }

        private void Init()
        {
            LayoutInflater.FromContext(Context).Inflate(Resource.Layout.SwipeBase, this);

            leftSwipeButtonsContainer = new ButtonsContainer(FindViewById<LinearLayout>(Resource.Id.LeftSwipeButtons));
            rightSwipeButtonsContainer = new ButtonsContainer(FindViewById<LinearLayout>(Resource.Id.RightSwipeButtons));

            SwipePossibilityAnimation = new JumpyAnimation(Resources.DisplayMetrics.Density);
        }

        private void RemoveTopView(bool removeButtons)
        {
            if (topView != null)
            {
                RemoveView(topView);
                gestureDetector.MovementChanged -= GestureDetector_MovementChanged;
                gestureDetector = null;
                topView.SetOnTouchListener(null);
                topView = null;

                if (removeButtons)
                {
                    RemoveButtons();
                }
            }
        }

        private void GestureDetector_MovementChanged(object sender, float movement)
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
    }
}