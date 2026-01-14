using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using TizenDotNet1.shared.Dtos;

namespace TizenDotNet1.shared.Utils;
public static class UiDetailBuilder
{
    private static View detailView;

    public static void OpenDetailScreen(int movieIndex, UiModules module)
    {
        detailView = new View
        {
            BackgroundColor = Color.Black,
            WidthSpecification = LayoutParamPolicies.MatchParent,
            HeightSpecification = LayoutParamPolicies.MatchParent,
            Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center
            },
            Focusable = true
        };

        var poster = new ImageView
        {
            WidthSpecification = 450,
            HeightSpecification = 650,
            ResourceUrl = Tizen.Applications.Application.Current.DirectoryInfo.Resource +
              $"Images/movie{movieIndex + 1}.jpg",
            Margin = new Extents(0, 0, 40, 40)
        };
        detailView.Add(poster);

        var title = new TextLabel
        {
            Text = $"Película #{movieIndex + 1}",
            TextColor = Color.White,
            PointSize = 30,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Extents(20, 20, 20, 20)
        };
        detailView.Add(title);

        // Botón visible (usar COMPONENTS)
        var backBtn = new Button
        {
            Text = "Volver",
            WidthSpecification = 300,
            HeightSpecification = 120,
            BackgroundColor = Color.White,
            TextColor = Color.Black,
            CornerRadius = 10,
            Margin = new Extents(0, 0, 40, 40)
        };

        backBtn.Clicked += (s, e) => CloseDetail();

        detailView.Add(backBtn);

        Window.Instance.GetDefaultLayer().Add(detailView);

        FocusManager.Instance.SetCurrentFocusView(detailView);

        //Input del control remoto en detail vista
        detailView.KeyEvent += (s, e) =>
        {
            if (e.Key.State != Key.StateType.Down)
                return false;

            switch (e.Key.KeyPressedName)
            {
                case "XF86Back":
                    CloseDetail();
                    return true;
            }

            return false;
        };
    }

    private static void CloseDetail()
    {
        Window.Instance.GetDefaultLayer().Remove(detailView);
        FocusManager.Instance.SetCurrentFocusView(
            UiHeroCarouselBuilder.CarouselRoot
        );
    }

}
