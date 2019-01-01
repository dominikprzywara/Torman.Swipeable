# Torman.Swipeable
Xamarin.Android custom controls with swipe actions.

![alt text][swipeAnimation]

## Download
[NuGet version](https://www.nuget.org/packages/Torman.Swipeable).

## Demo app
You can find sample app in sample/ directory.

## More to come
* Swipe as action (without buttons below),
* Swipe manager (f.e. only one row in ListView can show buttons below),
* Different animations.

## Configuration
You can use swipeable view in your custom adapter.

## Set top view
```
var topView = swipeableView.SetTopView(Resource.Layout.Main);
```

## Configure swipe
It's fluent!
```
swipeableView
    .SetOnlyLeftSwipe()
    .EnablePossibleSwipeAnimationOnClick(true)
    .AddSwipeButton(
        new ButtonBuilder(() => ToastButtonClicked(position, "Ok"), ColorHelper.Teal)
            .SetDrawable(Resource.Drawable.ic_done_white_36dp)
            .SetForegroundColor(Color.White)
            .SetButtonSize(ButtonSize.Huge),
        SwipeDirection.Left);
```

[swipeAnimation]: https://github.com/dominikprzywara/Torman.Swipeable/blob/master/img/NewSwipeDemo.gif "Swipe demo animation"
