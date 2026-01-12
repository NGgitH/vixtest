using System.Linq;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Vix;
using static TizenDotNet1.shared.Dtos.BuilderDto;
using Color = Tizen.NUI.Color;
using NUISize = Tizen.NUI.Size;

namespace TizenDotNet1.shared.Utils;
public static class UiVideoCarouselBuilder
{
    private static View _contentView;
    private static View _firstThumbnail;
    private static View _detailHero;
    private static View _root;
    private static View currentHero;

    public static View BuildVideoCarousel(UiModule module)
    {
        var root = new View
        {
            Focusable = true, // CLAVE
            Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                CellPadding = new Size2D(0, 20)
            },
            Name = module.Title
        };
        // 🏷 TÍTULO
        root.Add(new TextLabel
        {
            Text = module.Title ?? "Recomendado para ti",
            TextColor = Color.White,
            PointSize = 30,
            Padding = new Extents(0, 0, 0, 0)
        });

        var viewport = new View
        {
            SizeHeight = 160,
            ClippingMode = ClippingModeType.ClipChildren,
            Layout = null, // NO layout vertical acá
        };

        //View que se mueve
        _contentView = new View
        {
            Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal, //que orientacion tiene el carrusel
                CellPadding = new Size2D(30, 0)
            }
        };

        viewport.Add(_contentView);

        int index = 0;

        //aca creo las thumnails del carrusel
        foreach (var item in module.Contents.Edges)
        {
            // demo
            var card = CreateThumbnail(
                item.Node.HeroImageUrl, //imagen
                item.Node.HeroTitle //titulo
            );

            int capturedIndex = index;
            if (_firstThumbnail == null)
                _firstThumbnail = card;

            _contentView.Add(card);
            index++;
        }

        root.Add(viewport);

        //para que funcione el herodetail
        //_root = root;

        return root;
    }

    private static View CreateThumbnail(string imageUrl, string title)
    {
        var thumb = new View
        {
            Size = new NUISize(260, 140),
            Focusable = true,
            CornerRadius = 8,
            BackgroundColor = Color.Black
        };

        var image = new ImageView
        {
            ResourceUrl = imageUrl,
            Size = new NUISize(260, 140),
            FittingMode = FittingModeType.ScaleToFill
        };

        thumb.Add(image);
        return thumb;
    }

    private static void HideDetailHero()
    {
        if (_detailHero == null)
            return;

        _root.Remove(_detailHero);
        _detailHero.Dispose();
        _detailHero = null;
    }

    //logica para mostrar arriba del hero main el hero detail
    private static void ShowDetailHero(UiModule module, int index)
    {
        var current = Program.sectionContainer.GetChildAt(0);
        var children = Program.sectionContainer.Children.ToList();

        foreach (var child in children)
        {
            Program.sectionContainer.Remove(child);
        }

        var casda = Program.sectionContainer.Children.Count;
        Program.sectionContainer.Add(Program.detailHero);
        Program.sectionContainer.Add(Program.thumbnails);

        var item = module.Contents.Edges[index].Node;
        UiDetailHeroBuilder.Update(
            item.HeroImageUrl,
            item.HeroTitle,
            "Descripción demo"
        );

        FocusManager.Instance.SetCurrentFocusView(Program.sectionContainer.Children[0]);
    }

    public static void RestoreHeroCarousel()
    {
        var current = Program.sectionContainer.GetChildAt(0);

        Program.sectionContainer.Remove(currentHero);

        Program.sectionContainer.Add(Program.heroCarousel);
        Program.sectionContainer.Add(Program.thumbnails);

        FocusManager.Instance.SetCurrentFocusView(Program.heroCarousel.Children[0]);
    }
}
