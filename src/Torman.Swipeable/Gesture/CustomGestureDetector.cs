using Android.Animation;
using Android.Content;
using Android.Views;
using System;

namespace Torman.Swipeable.Gesture
{
    internal class CustomGestureDetector : GestureDetector.SimpleOnGestureListener, View.IOnTouchListener
    {
        public event EventHandler<float> MovementChanged;

        private static int MaxClickDuration = 1000;

        private static int MaxClickDistance = 15;

        private static float MoveMultiplication = 0.5f;

        private readonly SwipeLayout parentView;

        private readonly Context context;

        private float lastX;

        private float initX;

        private bool clickPerformed;

        private float distance;

        private DateTime pressStartTime;

        public CustomGestureDetector(SwipeLayout swipeView, float offset, Context context)
        {
            parentView = swipeView;
            LeftSwipePossible = true;
            RightSwipePossible = true;
            Offset = offset;
            this.context = context;
        }

        public bool LeftSwipePossible { get; private set; }

        public bool RightSwipePossible { get; private set; }

        public float Offset { get; }

        private float ViewPosition => swipeableView.TranslationX;

        private View swipeableView => parentView.TopView;

        public bool OnTouch(View v, MotionEvent e)
        {
            float movement = 0;

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    lastX = e.GetX(e.ActionIndex);
                    initX = ViewPosition;
                    pressStartTime = DateTime.UtcNow;
                    distance = 0;
                    clickPerformed = false;
                    break;
                case MotionEventActions.Move:
                    if (parentView.TopView.Animation != null)
                    {
                        return true;
                    }

                    var x = e.GetX(e.ActionIndex);
                    movement = (x - lastX) * MoveMultiplication;
                    distance += Math.Abs(movement);

                    if (!CanSwipe(movement))
                    {
                        return true;
                    }

                    lastX = x;
                    break;
                //case MotionEventActions.Cancel:
                case MotionEventActions.Up:
                    var duration = DateTime.UtcNow - pressStartTime;
                    if (duration.TotalMilliseconds < MaxClickDuration && distance < MaxClickDistance * context.Resources.DisplayMetrics.Density)
                    {
                        clickPerformed = true;
                        if (ViewPosition != 0)
                        {
                            Close();
                        }
                        else
                        {
                            swipeableView.CallOnClick();
                        }
                    }
                    break;
            }

            if (!parentView.HaveButtons)
            {
                NoButtonsSwipe(e, movement);
            }
            else
            {
                SwipeWhenHaveButtons(e, movement);
            }

            if (movement != 0)
            {
                OnMovementChanged(movement);
            }

            return true;
        }

        public void SetOnlyLeftSwipe()
        {
            LeftSwipePossible = true;
            RightSwipePossible = false;
        }

        public void SetOnlyRightSwipe()
        {
            LeftSwipePossible = false;
            RightSwipePossible = true;
        }

        public void SetBothWaysSwipe()
        {
            LeftSwipePossible = true;
            RightSwipePossible = true;
        }

        public void DisableSwipe()
        {
            LeftSwipePossible = false;
            RightSwipePossible = false;
        }

        public void Close()
        {
            TranslateTo(0);
        }

        private void SwipeWhenHaveButtons(MotionEvent e, float movement)
        {
            switch (e.Action)
            {
                case MotionEventActions.Move:
                    {
                        SwipeInBounds(movement);
                    }
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    if (clickPerformed)
                    {
                        return;
                    }

                    var buttonsWidth = ButtonsWidth(ViewPosition);

                    if (Math.Abs(ViewPosition) < Offset && buttonsWidth > Offset)
                    {
                        Close();
                    }
                    else
                    {
                        if (initX != 0)
                        {
                            Close();
                        }
                        else
                        {
                            if (ViewPosition > 0)
                            {
                                TranslateTo(buttonsWidth);
                            }
                            else if (ViewPosition < 0)
                            {
                                TranslateTo(-buttonsWidth);
                            }
                        }
                    }
                    break;
            }
        }

        private void NoButtonsSwipe(MotionEvent e, float movement)
        {
            switch (e.Action)
            {
                case MotionEventActions.Move:
                    {
                        SwipeInBounds(movement);
                    }
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    if (ViewPosition != 0)
                    {
                        Close();
                    }
                    break;
            }
        }

        private void TranslateTo(int x, int duration = 200)
        {
            var transAnimation = ObjectAnimator.OfFloat(swipeableView, "TranslationX", ViewPosition, x);
            transAnimation.SetDuration(duration);
            transAnimation.Start();
        }

        private void SwipeInBounds(float movement)
        {
            var futurePosition = ViewPosition + movement;

            var offset = Offset;
            if (parentView.HaveButtons)
            {
                offset += ButtonsWidth(futurePosition);
            }

            if (Math.Abs(futurePosition) <= offset)
            {
                if (!LeftSwipePossible && futurePosition < 0 ||
                    !RightSwipePossible && futurePosition > 0)
                {
                    futurePosition = 0;
                }
            }

            swipeableView.TranslationX = futurePosition;
        }

        private bool CanSwipe(float movement)
        {
            if (movement < 0)
            {
                if (LeftSwipePossible || ViewPosition > 0)
                {
                    return true;
                }
            }
            else if (movement > 0)
            {
                if (RightSwipePossible || ViewPosition < 0)
                {
                    return true;
                }
            }

            return false;
        }

        private int ButtonsWidth(float position)
        {
            if (position < 0)
            {
                return parentView.LeftSwipeButtonsWidth;
            }
            else if (position > 0)
            {
                return parentView.RightSwipeButtonsWidth;
            }

            return 0;
        }

        private void OnMovementChanged(float movement)
        {
            MovementChanged?.Invoke(this, movement);
        }
    }
}