using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Collections.ObjectModel;
using Torman.Swipeable;
using Torman.Swipeable.Builders;
using Torman.Swipeable.Enums;

namespace SwipeDemo
{
    public class CustomAdapter : BaseAdapter
    {
        private ObservableCollection<string> array = new ObservableCollection<string>
        {
            "Left with image button and animation",
            "Right with 2 buttons with animation",
            "Both with image and text buttons per side and animation",
            "Right with button and animation",
            "Left with text button and no animation",
        };

        private Context context;

        public CustomAdapter(Context context)
        {
            this.context = context;
        }

        public override int Count => array.Count;

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

            
            switch (position)
            {
                case 0:
                    swipeableView
                        .SetOnlyLeftSwipe()
                        .EnablePossibleSwipeAnimationOnClick(true)
                        .AddSwipeButton(
                            new ButtonBuilder(() => ToastButtonClicked(position, "Ok"), ColorHelper.Teal)
                                .SetDrawable(Resource.Drawable.ic_done_white_36dp)
                                .SetForegroundColor(Color.White)
                                .SetButtonSize(ButtonSize.Huge),
                            SwipeDirection.Left);
                    break;
                case 1:
                    swipeableView
                        .SetOnlyRightSwipe()
                        .EnablePossibleSwipeAnimationOnClick(true)
                        .AddSwipeButton(
                            new ButtonBuilder(() => ToastButtonClicked(position, "Land"), ColorHelper.Green)
                                .SetDrawable(Resource.Drawable.ic_flight_land_white_36dp)
                                .SetForegroundColor(Color.White)
                                .SetButtonSize(ButtonSize.Small),
                            SwipeDirection.Right)
                        .AddSwipeButton(
                            new ButtonBuilder(() => ToastButtonClicked(position, "Take off"), ColorHelper.Pink)
                                .SetDrawable(Resource.Drawable.ic_flight_takeoff_white_36dp)
                                .SetForegroundColor(Color.White)
                                .SetButtonSize(ButtonSize.Medium),
                            SwipeDirection.Right);
                    break;
                case 2:
                    swipeableView
                        .EnablePossibleSwipeAnimationOnClick(true)
                        .SetBothWaysSwipe()
                        .AddSwipeButton(
                            new ButtonBuilder(() => ToastButtonClicked(position, "Cart"), ColorHelper.Green)
                                .SetDrawable(Resource.Drawable.ic_done_white_36dp)
                                .SetTitle("Buy it")
                                .SetForegroundColor(Color.White)
                                .SetButtonSize(ButtonSize.Huge),
                            SwipeDirection.Left)
                        .AddSwipeButton(
                            new ButtonBuilder(() => ToastButtonClicked(position, "Accsessible"), ColorHelper.Purple)
                                .SetDrawable(Resource.Drawable.ic_add_shopping_cart_white_36dp)
                                .SetForegroundColor(Color.Yellow)
                                .SetTitle("Help")
                                .SetButtonSize(ButtonSize.Medium),
                            SwipeDirection.Right);
                    break;
                case 3:
                    swipeableView
                        .EnablePossibleSwipeAnimationOnClick(true)
                        .SetOnlyRightSwipe()
                        .AddSwipeButton(
                            new ButtonBuilder(() => ToastButtonClicked(position, "Test button"), Color.MidnightBlue)
                                .SetTitle("Test button")
                                .SetForegroundColor(Color.White)
                                .SetButtonSize(ButtonSize.Huge),
                            SwipeDirection.Right);
                    break;
                default:
                    swipeableView
                        .SetOnlyLeftSwipe()
                        .AddSwipeButton(
                            new ButtonBuilder(() => { array.RemoveAt(position); NotifyDataSetChanged(); }, ColorHelper.Red)
                                .SetTitle("Delete"),
                            SwipeDirection.Left);
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

