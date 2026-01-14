using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Pims.Contacts.ContactsViews;
using TizenDotNet1.shared.Dtos;
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

    public static View BuildVideoCarousel(Node node, string name)
    {
        var root = new View
        {
            Focusable = true, // CLAVE
            Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                CellPadding = new Size2D(0, 20)
            }, 
            Name = name
        };
        // 🏷 TÍTULO
        root.Add(new TextLabel
        {
            Text = node.trackingMetadataJson.ui_module_title ?? "Recomendado para ti",
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
        foreach (var item in node.contents.edges)
        {
            // demo
            var card = CreateThumbnail(
                item.node.image.link, //imagen
                node.trackingMetadataJson.ui_module_title, //titulo
                name
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

    private static View CreateThumbnail(string imageUrl, string title, string name)
    {
        var thumb = new View
        {
            Size = new NUISize(260, 140),
            Focusable = true,
            CornerRadius = 8,
            BackgroundColor = Color.Black,
            Name = name
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
}
