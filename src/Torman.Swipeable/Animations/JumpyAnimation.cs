using Android.Views.Animations;

namespace Torman.Swipeable.Animations
{
    public class JumpyAnimation : SwipeAnimation
    {
        public JumpyAnimation(float density) : base(density)
        {
        }

        protected override AnimationSet SwipePossibilityAnimation(float farthestPoint)
        {
            var animationSet = new AnimationSet(false);

            var animation = new TranslateAnimation(0, farthestPoint, 0, 0);
            animation.Duration = TranslateAnimationDuration;
            animationSet.AddAnimation(animation);

            var animation1 = new TranslateAnimation(0, -farthestPoint, 0, 0);
            animation1.Interpolator = new BounceInterpolator();
            animation1.Duration = BounceAnimationDuration;
            animation1.StartOffset = TranslateAnimationDuration;

            animationSet.AddAnimation(animation1);

            return animationSet;
        }
    }
}