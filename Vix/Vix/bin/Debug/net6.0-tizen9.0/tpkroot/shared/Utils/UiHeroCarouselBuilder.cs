using System.Collections.Generic;
using System.Linq;
using Tizen.Multimedia.Util;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using TizenDotNet1.shared.Dtos;
using Vix;

namespace TizenDotNet1.shared.Utils;

public static class UiHeroCarouselBuilder
{
    private static View _contentView; //view que se mueve horizontalmente
    private static View _indicatorContainer; //contenedor visual de los puntos
    public static View CarouselRoot { get; private set; }

    public static View BuildHeroCarousel(Node node)
    {
        // view visible (viewport)
        var carousel = new View
        {
            SizeHeight = 1080,  
            Layout = null, 
            Size = new Size(1920, 1080), //tamaño
            ClippingMode = ClippingModeType.ClipChildren, //oculta lo que se sale (clave para efecto carrusel)
            Focusable = true //necesitamos para hacer el foco en las diferentes ubicaciones del carrusel
        };

        CarouselRoot = carousel; // GUARDAMOS REFERENCIA

        //view que se mueve
        _contentView = new View
        {
            Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal, //que orientacion tiene el carrusel
                CellPadding = new Size2D(40, 60) //espacio entre cards del carrusel,
            }
        };

        carousel.Add(_contentView);

        int index = 0;

        //aca creo las cards del carrusel
        foreach (var item in node.contents.edges)
        {
            // demo
            var card = CreateHeroCard(
                item.node.landscapeFillImage.link, //imagen
                item.node.clickTrackingJson.ui_content_title //titulo
            );

            int capturedIndex = index;

            var viewName = new ImageView //imagen
            {
                ResourceUrl = item.node.landscapeFillImage.link,
                Size = new Size(60, 60),
                FittingMode = FittingModeType.ScaleToFill,
                SamplingMode = SamplingModeType.Box
            };
            _contentView.Add(viewName);

            _contentView.Add(card);
            index++;
        }

        //Se crean tantos puntos como imagenes.
        carousel.Add(CreateIndicators(node.contents.edges.Count));




        return carousel;
    }

    // crear Card individual
    private static View CreateHeroCard(string imageUrl, string title)
    {
        var card = new View
        {
            Size = new Size(1920, 1100),
            Focusable = true
        };

        var background = new ImageView //imagen
        {
            ResourceUrl = imageUrl,
            Size = new Size(1920, 1100),
            FittingMode = FittingModeType.ScaleToFill,
            SamplingMode = SamplingModeType.Box
        };

        var overlay = new View
        {
            BackgroundColor = new Color(0, 0, 0, 0.4f) //fondo oscuro semitransparente
        };

        var titleLabel = new TextLabel //título abajo
        {
            Text = title,
            TextColor = Color.White,
            VerticalAlignment = VerticalAlignment.Bottom,
            Padding = new Extents(20, 20, 20, 20)
        };

        card.Add(background);
        card.Add(overlay);
        card.Add(titleLabel);

        return card;
    }

    ///dots
    private static View CreateIndicators(int count)
    {
        _indicatorContainer = new View //Los centra abajo del carrusel.
        {
            Size = new Size(1920, 60),
            Position = new Position(0, 950),
            Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                CellPadding = new Size2D(14, 0)
            }
        };

        for (int i = 0; i < count; i++)
        {
            var dot = new View //círculo perfecto y solo el primero activo
            {
                Name = i.ToString(),
                Size = new Size(20, 20),
                CornerRadius = 10,
                BackgroundColor = i == 0 ? Color.White : new Color(1, 1, 1, 0.4f)
            };

            _indicatorContainer.Add(dot);
        }

        return _indicatorContainer;
    }

    //agregar o quitar main hero cuando voy a un thumbnail
    public static void HideMainHero()
    {
        if (CarouselRoot == null) return;

        CarouselRoot.Opacity = 0f;
        CarouselRoot.Sensitive = false; // no recibe input

        CarouselRoot.Dispose();
    }

    public static void ShowMainHero()
    {
        if (CarouselRoot == null) return;

        CarouselRoot.Opacity = 1f;
        CarouselRoot.Sensitive = true;
    }
}
