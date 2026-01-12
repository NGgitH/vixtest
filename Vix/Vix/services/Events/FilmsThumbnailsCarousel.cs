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
        Program.thumbnails.FocusGained += (s, e) =>
        {
            var thumbnailSelected = Program._carouselRoots[0].Children[1];
            _contentView = thumbnailSelected.Children[0];
            FocusManager.Instance.SetCurrentFocusView(_contentView.Children[_currentIndex]);
        };
        Program.thumbnails2.FocusGained += (s, e) =>
        {
            var thumbnailSelected = Program._carouselRoots[1].Children[1];
            _contentView = thumbnailSelected.Children[0];
            FocusManager.Instance.SetCurrentFocusView(_contentView.Children[_currentIndex]);
        };

        Program.thumbnails.KeyEvent += (s, e) =>
        {
            if (e.Key.State != Key.StateType.Down)
                return false;

            var viewportFirstThumbnail = Program.thumbnails.Children[1];
            _contentView = viewportFirstThumbnail.Children[0];

            switch (e.Key.KeyPressedName)
            {
                case "Up":
                    FocusManager.Instance.SetCurrentFocusView(Program.heroCarousel);
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
                    var viewportLastThumbnail = Program._carouselRoots[1].Children[1];
                    _contentView = viewportLastThumbnail.Children[0];
                    FocusManager.Instance.SetCurrentFocusView(Program._carouselRoots[1]);
                    MoveToIndex(_currentIndex);
                    return true;
            }
            return false;
        };

        Program.thumbnails2.KeyEvent += (s, e) =>
        {
            if (e.Key.State != Key.StateType.Down)
                return false;

            var viewportLastThumbnail = Program.thumbnails2.Children[1];
            _contentView = viewportLastThumbnail.Children[0];

            switch (e.Key.KeyPressedName)
            {
                case "Up":
                    var viewportFirstThumbnail = Program._carouselRoots[0].Children[1];
                    _contentView = viewportFirstThumbnail.Children[0];
                    FocusManager.Instance.SetCurrentFocusView(Program._carouselRoots[0]);
                    MoveToIndex(_currentIndex);
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
            }
            return false;
        };
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
