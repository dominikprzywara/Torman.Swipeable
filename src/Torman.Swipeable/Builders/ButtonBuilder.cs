using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using System;
using Torman.Swipeable.Enums;
using Torman.Swipeable.Helpers;

namespace Torman.Swipeable.Builders
{
    public class ButtonBuilder
    {
        public ButtonBuilder(Action click, Color backgroundColor)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = Color.White;
            Click = click;
            ButtonSize = ButtonSize.Wrap;
        }

        public Color BackgroundColor { get; private set; }

        public Color ForegroundColor { get; private set; }

        public Action Click { get; private set; }

        public Drawable Drawable { get; private set; }

        public string Title { get; private set; }

        public int? ResourceId { get; private set; }

        public ButtonSize ButtonSize { get; private set; }

        public ButtonBuilder SetTitle(string title)
        {
            Title = title;
            return this;
        }

        public ButtonBuilder SetDrawable(Drawable drawable)
        {
            Drawable = drawable;
            return this;
        }

        public ButtonBuilder SetDrawable(int resourceId)
        {
            ResourceId = resourceId;
            Drawable = null;
            return this;
        }

        public ButtonBuilder SetForegroundColor(Color color)
        {
            ForegroundColor = color;
            return this;
        }

        public ButtonBuilder SetButtonSize(ButtonSize size)
        {
            ButtonSize = size;
            return this;
        }

        internal View Construct(Context context)
        {
            CheckIfBuilderIsComplete();

            View button = null;

            if (Drawable != null || ResourceId.HasValue)
            {
                if (!String.IsNullOrWhiteSpace(Title))
                {
                    button = ImageAndTextButton(context);
                }
                else
                {
                    button = ImagedButton(context);
                }
            }
            else if (!String.IsNullOrWhiteSpace(Title))
            {
                button = TextButton(context);
            }

            button.SetBackgroundColor(BackgroundColor);
            button.Clickable = true;
            button.Click += (s, e) => Click();

            if (ButtonSize != ButtonSize.Wrap)
            {
                button.SetMinimumWidth((int)(ButtonSizeHelper.GetButtonWidth(ButtonSize) * context.Resources.DisplayMetrics.Density));
            }

            return button;
        }

        private void CheckIfBuilderIsComplete()
        {
            if (Drawable is null && !ResourceId.HasValue && String.IsNullOrWhiteSpace(Title))
            {
                throw new Exception("You have to add any combination of following: Drawable/ResourceId, Title");
            }
        }

        private View ImagedButton(Context context)
        {
            var button = Inflate(context, Resource.Layout.NoMarginImageButton);
            SetImage(button);
            return button;
        }

        private View ImageAndTextButton(Context context)
        {
            var button = Inflate(context, Resource.Layout.ImageAndTextButton);
            SetTitle(button);
            SetImage(button);
            return button;
        }

        private View TextButton(Context context)
        {
            var button = Inflate(context, Resource.Layout.NoMarginButton);
            SetTitle(button);
            return button;
        }

        private View Inflate(Context context, int resourceId)
        {
            return LayoutInflater.FromContext(context).Inflate(resourceId, null);
        }

        private void SetTitle(View button)
        {
            var textView = button.FindViewById<TextView>(Resource.Id.ButtonText);
            textView.Text = Title;
            textView.SetTextColor(ForegroundColor);
        }

        private void SetImage(View button)
        {
            var imageView = button.FindViewById<ImageView>(Resource.Id.Image);

            if (ResourceId.HasValue)
            {
                imageView.SetImageResource(ResourceId.Value);
            }
            else
            {
                imageView.SetImageDrawable(Drawable);
            }

            imageView.SetColorFilter(ForegroundColor);
        }
    }
}