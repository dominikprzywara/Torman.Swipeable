using Torman.Swipeable.Enums;

namespace Torman.Swipeable.Helpers
{
    internal static class ButtonSizeHelper
    {
        internal static int GetButtonWidth(ButtonSize buttonSize)
        {
            var size = 0;

            switch (buttonSize)
            {
                case ButtonSize.Huge:
                    size = 200;
                    break;
                case ButtonSize.Large:
                    size = 160;
                    break;
                case ButtonSize.Medium:
                    size = 120;
                    break;
                case ButtonSize.Small:
                    size = 80;
                    break;
            }

            return size;
        }
    }
}