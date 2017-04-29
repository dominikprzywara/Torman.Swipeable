using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Torman.Swipeable;

namespace SwipeDemo
{
    public class CustomAdapter : BaseAdapter
    {
        private string[] array = new string[]
        {
            "Left with image button and animation",
            "Right with 2 image buttons and animation",
            "Both with 1 button per side with animation",
            "Left with button and animation",
            "Right with button and animation",
            "Both with 1 button per side and animation",
            "Left with 2 buttons and no animation",
            "Right with 2 buttons and no animation",
            "Both with 2 buttons per side and animation",
        };

        private Context context;

        public CustomAdapter(Context context)
        {
            this.context = context;
        }

        public override int Count => array.Length;

        public override Java.Lang.Object GetItem(int position)
        {
            return array[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var swipeableView = convertView as SwipeLayout;

            if (swipeableView == null)
            {
                swipeableView = new SwipeLayout(context);
            }

            var topView = swipeableView.SetTopView(Resource.Layout.Main);
            topView.FindViewById<TextView>(Resource.Id.textView).Text = array[position];

            swipeableView.EnablePossibleSwipeAnimationOnClick(true);
            
            switch (position)
            {
                case 0:
                    swipeableView.SetOnlyLeftSwipe();
                    swipeableView.AddSwipeButton(Resource.Drawable.ic_delete_black_24dp, Color.DarkGray, () => ToastButtonClicked(position, "Delete"), SwipeDirection.Left);
                    break;
                case 1:
                    swipeableView.SetOnlyRightSwipe();
                    swipeableView.AddSwipeButton(Resource.Drawable.ic_folder_open_black_24dp, Color.LightGray, () => ToastButtonClicked(position, "Folder"), SwipeDirection.Right);
                    swipeableView.AddSwipeButton(Resource.Drawable.ic_help_outline_black_24dp, Color.Gray, () => ToastButtonClicked(position, "Help"), SwipeDirection.Right);
                    break;
                case 2:
                    swipeableView.AddSwipeButton("Buy it", Resource.Drawable.ic_add_shopping_cart_white_24dp, Color.DarkGreen, Color.White, () => ToastButtonClicked(position, "Cart"), SwipeDirection.Right);
                    swipeableView.AddSwipeButton("Help", Resource.Drawable.ic_accessible_white_24dp, Color.DarkRed, Color.White, () => ToastButtonClicked(position, "Accsessible"), SwipeDirection.Left);
                    swipeableView.SetBothWaysSwipe();
                    break;
                case 3:
                    swipeableView.AddSwipeButton("Btn", Color.Purple, Color.White, () => ToastButtonClicked(position, "Purple"), SwipeDirection.Left);
                    swipeableView.SetOnlyLeftSwipe();
                    break;
                case 4:
                    swipeableView.SetOnlyRightSwipe();
                    swipeableView.AddSwipeButton("Button", Color.Red, Color.White, () => ToastButtonClicked(position, "Red"), SwipeDirection.Right);
                    break;
                case 5:
                    swipeableView.SetBothWaysSwipe();
                    swipeableView.AddSwipeButton("Btn", Color.Purple, Color.White, () => ToastButtonClicked(position, "Purple"), SwipeDirection.Left);
                    swipeableView.AddSwipeButton("Button", Color.Red, Color.White, () => ToastButtonClicked(position, "Red"), SwipeDirection.Right);
                    break;
                case 6:
                    swipeableView.SetOnlyLeftSwipe();
                    swipeableView.AddSwipeButton("Btn", Color.Purple, Color.White, () => ToastButtonClicked(position, "Purple"), SwipeDirection.Left);
                    swipeableView.AddSwipeButton("Button", Color.Red, Color.White, () => ToastButtonClicked(position, "Red"), SwipeDirection.Left );
                    swipeableView.EnablePossibleSwipeAnimationOnClick(false);
                    break;
                case 7:
                    swipeableView.SetOnlyRightSwipe();
                    swipeableView.AddSwipeButton("Btn", Color.Purple, Color.White, () => ToastButtonClicked(position, "Purple"), SwipeDirection.Right);
                    swipeableView.AddSwipeButton("Button", Color.Red, Color.White, () => ToastButtonClicked(position, "Red"), SwipeDirection.Right);
                    swipeableView.EnablePossibleSwipeAnimationOnClick(false);
                    break;
                case 8:
                    swipeableView.SetBothWaysSwipe();
                    swipeableView.AddSwipeButton("Puprle Btn", Color.Purple, Color.White, () => ToastButtonClicked(position, "Purple"), SwipeDirection.Left);
                    swipeableView.AddSwipeButton("DarkRed", Color.DarkRed, Color.White, () => ToastButtonClicked(position, "DarkRed"), SwipeDirection.Left);
                    swipeableView.AddSwipeButton("MidnightBlue", Color.MidnightBlue, Color.White, () => ToastButtonClicked(position, "MidnightBlue"), SwipeDirection.Right);
                    swipeableView.AddSwipeButton("Orange", Color.DarkOrange, Color.White, () => ToastButtonClicked(position, "DarkoOrange"), SwipeDirection.Right);
                    swipeableView.EnablePossibleSwipeAnimationOnClick(false);
                    break;
            }

            return swipeableView;
        }

        private void ToastButtonClicked(int row, string buttonName)
        {
            Toast.MakeText(context, $"{buttonName} from row {row + 1} clicked!", ToastLength.Short).Show();
        }
    }
}

