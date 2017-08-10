using System.Collections.Generic;

namespace Torman.Swipeable.Managers
{
    public class SingleOpenedItemSwipeManager : ISwipeManager
    {
        IList<SwipeLayout> layouts = new List<SwipeLayout>();

        SwipeLayout openedLayout = null;

        public void Bind(SwipeLayout view)
        {
            if (!layouts.Contains(view))
            {
                layouts.Add(view);
                view.Moved += (s, e) => Open(view);
            }
            else
            {
                view.Close();
            }
        }

        public void Unbind(SwipeLayout view)
        {
            if (!layouts.Contains(view))
            {
                layouts.Remove(view);
            }
        }

        public void Open(SwipeLayout view)
        {
            if (layouts.Contains(view))
            {
                if (openedLayout != null)
                {
                    Close(openedLayout);
                }

                openedLayout = view;
            }
        }

        public void Close(SwipeLayout view)
        {
            view.Close();
        }

        public void UnbindAll()
        {
            foreach (var layout in layouts)
            {
                Unbind(layout);
            }
        }

        public void CloseAll()
        {
            if (openedLayout != null)
            {
                Close(openedLayout);
            }
        }
    }
}