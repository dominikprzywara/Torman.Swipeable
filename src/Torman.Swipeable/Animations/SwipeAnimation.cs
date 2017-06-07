using Android.Views.Animations;

namespace Torman.Swipeable.Animations
{
    public abstract class SwipeAnimation
    {
        private float density;

        public SwipeAnimation(float density)
        {
            this.density = density;
        }

        public int BounceAnimationDuration { get; set; } = 800;

        public int TranslateAnimationDuration { get; set; } = 300;

        public int OffsetInDp { get; set; } = 30;

        public int TotalAnimationDuration => BounceAnimationDuration + TranslateAnimationDuration;

        private float AnimationOffset => OffsetInDp + 30  * density;

        public Animation ShowLeftSwipeAnimation()
        {
            return SwipePossibilityAnimation(-AnimationOffset);
        }

        public Animation ShowRightSwipeAnimation()
        {
            return SwipePossibilityAnimation(AnimationOffset);
        }

        protected abstract AnimationSet SwipePossibilityAnimation(float farthestPoint);
    }
}