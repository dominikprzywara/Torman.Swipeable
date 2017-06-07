using Android.App;
using Android.OS;
using Torman.Swipeable;

namespace SwipeDemo
{
    [Activity(Label = "SwipeDemo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.MainLayout);
            var listView = FindViewById<SwipeableListView>(Resource.Id.listView);
            listView.Adapter = new CustomAdapter(this);
        }
    }
}

