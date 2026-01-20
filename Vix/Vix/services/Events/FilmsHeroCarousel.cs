using Tizen.NUI;

namespace Vix.services.Events;
public static class FilmsHeroCarousel
{
    private static Timer _autoPlayTimer; //timer para avanzar automaticamente 
    private static int _currentHeroIndex = 0;
    private static int _cardWidth = 1920; //ancho de cada tarjeta (1920px)

    public static void Events()
    {
        Program.heroCarousel.FocusGained += (s, e) =>
        {
            StartAutoPlay();

            Program.ScrollToTheBeginPosition(Program.heroCarousel);

            if (Program.currentButton.Contains("PrimaryButton "))
            {
                FocusManager.Instance.SetCurrentFocusView(Program.primaryButton[_currentHeroIndex]);
            }
            if (Program.currentButton.Contains("SecondaryButton "))
            {
                FocusManager.Instance.SetCurrentFocusView(Program.secondaryButton[_currentHeroIndex]);
            }
        };

        //corusel key events
        Program.heroCarousel.KeyEvent += (s, e) =>
        {
            if (e.Key.State != Key.StateType.Down)
                return false;

            if (_currentHeroIndex == 0 && e.Key.KeyPressedName == "Left")
            {
                Program._carouselRoots.Clear();
                Program._sidebar.RestoreFocus();
            }

            switch (e.Key.KeyPressedName)
            {
                case "Up":
                    Program.currentButton = Program.primaryButton[_currentHeroIndex].Name;
                    FocusManager.Instance.SetCurrentFocusView(Program.primaryButton[_currentHeroIndex]);
                    return true;
                case "Down":
                    if (FocusManager.Instance.GetCurrentFocusView().Name.Contains("PrimaryButton "))
                    {
                        Program.currentButton = Program.secondaryButton[_currentHeroIndex].Name;
                        FocusManager.Instance.SetCurrentFocusView(Program.secondaryButton[_currentHeroIndex]);
                    }
                    else
                    {
                        StopAutoPlay();
                        FocusManager.Instance.SetCurrentFocusView(Program._carouselRoots[0]);
                        Program.currentButton = Program.primaryButton[_currentHeroIndex].Name;
                    }
                    return true;
                case "Left":
                    MoveHeroToIndex(_currentHeroIndex - 1);
                    return true;
                case "Right":
                    MoveHeroToIndex(_currentHeroIndex + 1);
                    return true;

            }

            return false;
        };
    }

    //hero
    public static void MoveHeroToIndex(int index)
    {
        if (index < 0 || index >= Program.heroCarousel.Children[0].Children.Count)
            return;

        _currentHeroIndex = index;

        if (Program.currentButton.Contains("PrimaryButton "))
        {
            FocusManager.Instance.SetCurrentFocusView(Program.primaryButton[_currentHeroIndex]);
        }
        if (Program.currentButton.Contains("SecondaryButton "))
        {
            FocusManager.Instance.SetCurrentFocusView(Program.secondaryButton[_currentHeroIndex]);
        }

        var animation = new Animation(1000); //Suave, 350ms, easing.
        animation.AnimateTo(
            Program.heroCarousel.Children[0],
            "PositionX",
            -index * (_cardWidth + 40), //posicion actual - ancho card - padding
            new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseInOut)
        );
        animation.Play();

        UpdateIndicators(); // Mantiene sincronizado:     imagen actual    -      puntito blanco
    }

    private static void StopAutoPlay()
    {
        _autoPlayTimer?.Stop();
    }

    // Autoplay
    public static void StartAutoPlay()
    {
        _autoPlayTimer?.Stop();
        _autoPlayTimer = new Timer(5000); //cada 5 seg
        _autoPlayTimer.Tick += (s, e) =>
        {
            MoveHeroToIndex((_currentHeroIndex + 1) % Program.heroCarousel.Children[0].Children.Count); //vuelve al inicio cuando llega al final.
            return true;
        };
        _autoPlayTimer.Start();
    }

    private static void UpdateIndicators() //Esto es lo que hace que el puntito se mueva cuando avanza el carrusel
    {
        for (int i = 0; i < Program.heroCarousel.Children[1].Children.Count; i++)
        {
            Program.heroCarousel.Children[1].Children[i].BackgroundColor =
                i == _currentHeroIndex
                    ? Color.White
                    : new Color(1, 1, 1, 0.4f);
        }
    }

}
