using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace Vix.services.Events;
public static class FilmsThumbnailsCarousel
{
    private static View _contentView;
    private static int _currentIndex;
    private static int _thumbWidth = 260;

    public static void Events()
    {
        var thumbnailIndex = 0;
        foreach (var item in Program._carouselRoots)
        {

            item.FocusGained += (s, e) =>
            {                
                var thumbnailSelected = item.Children[1];
                _contentView = thumbnailSelected.Children[0];
                FocusManager.Instance.SetCurrentFocusView(_contentView.Children[_currentIndex]);
                MoveToIndex(_currentIndex);
                Program.ScrollToViewInNovelSection(item);
            };

            item.KeyEvent += (s, e) =>
            {
                if (e.Key.State != Key.StateType.Down)
                    return false;

                var viewportCurrentThumbnail = item.Children[1];
                _contentView = viewportCurrentThumbnail.Children[0];
                var currentPrevious = int.Parse(FocusManager.Instance.GetCurrentFocusView().Name);

                switch (e.Key.KeyPressedName)
                {
                    case "Up":
                        var up = int.Parse(FocusManager.Instance.GetCurrentFocusView().Name) - 1;
                        if (up >= 0)
                        {
                            var prevCarousel = Program._carouselRoots[up];

                            Program._carouselRoots[currentPrevious].Opacity = 0f;
                            prevCarousel.Opacity = 1f;

                            var viewportPreviousThumbnail = prevCarousel.Children[1];
                            _contentView = viewportPreviousThumbnail.Children[0];
                            FocusManager.Instance.SetCurrentFocusView(prevCarousel);
                            //Program.ScrollToView(prevCarousel);
                        }
                        else
                        {
                            FocusManager.Instance.SetCurrentFocusView(Program.heroCarousel);
                            //Program.ScrollToView(Program.heroCarousel);
                        }
                        return true;

                    case "Left":
                        if (_currentIndex > 0)
                        {
                            MoveToIndex(_currentIndex - 1);
                            FocusManager.Instance.SetCurrentFocusView(_contentView.Children[_currentIndex]);
                        }
                        else
                        {
                            Program._carouselRoots.Clear();
                            Program._sidebar.RestoreFocus();
                        }
                        return true;

                    case "Right":
                        if (_contentView.Children.Count > 0)
                        {
                            MoveToIndex(_currentIndex + 1);
                            FocusManager.Instance.SetCurrentFocusView(_contentView.Children[_currentIndex]);
                        }
                        return true;

                    case "Down":
                        var last = int.Parse(FocusManager.Instance.GetCurrentFocusView().Name) + 1;
                        if (last < Program._carouselRoots.Count)
                        {
                            var nextCarousel = Program._carouselRoots[last];

                            Program._carouselRoots[currentPrevious].Opacity = 0f;
                            nextCarousel.Opacity = 1f;

                            var viewportLastThumbnail = nextCarousel.Children[1];
                            _contentView = viewportLastThumbnail.Children[0];
                            FocusManager.Instance.SetCurrentFocusView(nextCarousel);
                            //Program.ScrollToView(nextCarousel);
                        }
                        return true;
                }
                return false;
            };

            thumbnailIndex++;
        }
    }

    private static void MoveToIndex(int index)
    {
        if (index < 0 || index >= _contentView.Children.Count)
            return;

        _currentIndex = index;

        var animation = new Animation(250);
        animation.AnimateTo(
            _contentView,
            "PositionX",
            -index * (_thumbWidth + 30),
            new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseInOut)
        );
        animation.Play();
    }
}
