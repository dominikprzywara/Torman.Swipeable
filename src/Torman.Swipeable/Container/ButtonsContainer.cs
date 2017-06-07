using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Torman.Swipeable.Enums;

namespace Torman.Swipeable.Container
{
    internal class ButtonsContainer
    {
        public ButtonsContainer(LinearLayout buttonsContainerView)
        {
            ButtonsContainerView = buttonsContainerView;
        }

        public List<View> Buttons { get; } = new List<View>();

        public LinearLayout ButtonsContainerView { get; private set; }

        public bool HaveButtons => Buttons.Count > 0;

        public ViewStates Visibility
        {
            get => ButtonsContainerView.Visibility;
            set => ButtonsContainerView.Visibility = value;
        }

        public int ButtonsTotalWidth()
        {
            var width = 0;
            foreach (var button in Buttons)
            {
                width += button.Width;
            }

            return width;
        }

        public void AddButton(View button, ButtonSize size = ButtonSize.Wrap)
        {
            Buttons.Add(button);
            ButtonsContainerView.AddView(button);
            ButtonsContainerView.Visibility = ViewStates.Visible;
        }

        public void RemoveButtons()
        {
            Buttons.Clear();
            ButtonsContainerView.RemoveAllViews();
        }
    }
}