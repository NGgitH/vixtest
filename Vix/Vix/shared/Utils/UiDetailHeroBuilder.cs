using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace TizenDotNet1.shared.Utils;
public static class UiDetailHeroBuilder
{
    public static ImageView Image;
    public static TextLabel Title;
    public static TextLabel Description;
    private static View _detailHero;

    public static View Build()
    {
        var root = new View
        {
           // SizeHeight = 320,
            SizeHeight = 500,
            Layout = null,
            Size = new Size(1920, 500), //tamaño
            /*Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal,
                CellPadding = new Size2D(40, 0)
            }*/
        };

        Image = new ImageView
        {
            Size = new Size(560, 315),
            FittingMode = FittingModeType.ScaleToFill
        };

        var textContainer = new View
        {
            Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                CellPadding = new Size2D(0, 20)
            }
        };

        Title = new TextLabel
        {
            TextColor = Color.White,
            PointSize = 32
        };

        Description = new TextLabel
        {
            TextColor = new Color(1, 1, 1, 0.8f),
            PointSize = 20,
            MultiLine = true
        };

        textContainer.Add(Title);
        textContainer.Add(Description);

        root.Add(Image);
        root.Add(textContainer);

        return root;
    }

    public static void Update(string image, string title, string description)
    {
        Image.ResourceUrl = image;
        Title.Text = title;
        Description.Text = description;
    }
}
