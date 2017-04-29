﻿using Android.Animation;
using Android.Views;
using System;

namespace Torman.Swipeable
{
    internal class CustomGestureDetector : GestureDetector.SimpleOnGestureListener, View.IOnTouchListener
    {
        private static int MaxClickDuration = 1000;

        private static int MaxClickDistance = 15;

        private static float MoveMultiplication = 0.5f;

        private readonly SwipeLayout parentView;

        private float lastX;

        private float initX;

        private bool clickPerformed;

        private float distance;

        private DateTime pressStartTime;

        public CustomGestureDetector(SwipeLayout swipeView, float offset)
        {
            parentView = swipeView;
            LeftSwipe = true;
            RightSwipe = true;
            Offset = offset;
        }

        public bool LeftSwipe { get; set; }

        public bool RightSwipe { get; set; }

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
                case MotionEventActions.Up:
                    var duration = DateTime.UtcNow - pressStartTime;
                    if (duration.TotalMilliseconds < MaxClickDuration && distance < MaxClickDistance)
                    {
                        clickPerformed = true;
                        if (ViewPosition != 0)
                        {
                            TranslateTo(0);
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

            return true;
        }

        private void SwipeWhenHaveButtons(MotionEvent e, float movement)
        {
            switch (e.Action)
            {
                case MotionEventActions.Move:
                    {
                        SwipeInBounds(movement);
                        parentView.Movement(movement);
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
                        TranslateTo(0);
                    }
                    else
                    {
                        if (initX != 0)
                        {
                            TranslateTo(0);
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
                        TranslateTo(0);
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
                if (!LeftSwipe && futurePosition < 0 ||
                    !RightSwipe && futurePosition > 0)
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
                if (LeftSwipe || ViewPosition > 0)
                {
                    return true;
                }
            }
            else if (movement > 0)
            {
                if (RightSwipe || ViewPosition < 0)
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
    }
}