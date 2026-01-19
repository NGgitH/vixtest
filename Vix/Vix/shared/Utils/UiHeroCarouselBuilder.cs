using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using TizenDotNet1.shared.Dtos;

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
            Name = "novelHeroCarousel",
            SizeHeight = 1100,
            Layout = null,
            Size = new Size(1920, 1100), //tamaño
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

        //carousel 0
        carousel.Add(_contentView);

        int index = 0;

        //aca creo las cards del carrusel
        foreach (var item in node.contents.edges)
        {
            // demo
            var card = CreateHeroCard(
                item.node.landscapeFillImage.link, //imagen
                item.node.clickTrackingJson.ui_content_title, //titulo
                item.node.logoImage.link
            );
            int capturedIndex = index;
            _contentView.Add(card);
            index++;
        }

        //Se crean tantos puntos como imagenes.
        //carousel 1
        carousel.Add(CreateIndicators(node.contents.edges.Count));

        return carousel;
    }

    // crear Card individual
    private static View CreateHeroCard(string imageUrl, string title, string titleImageUrl)
    {
        var card = new View
        {
            Name = imageUrl,
            Size = new Size(1920, 1100),
            Focusable = true,
            ClippingMode = ClippingModeType.Disabled
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

        var content = new View
        {
            Name = title,
            Size = new Size(900, 500),
            Position = new Position(120, 420),
            Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                CellPadding = new Size2D(0, 20)
            },
            Padding = new Extents(50, 0, 0, 0),
            ClippingMode = ClippingModeType.Disabled
        };

        // 🖼️ Logo del título
        var titleImage = new ImageView
        {
            ResourceUrl = titleImageUrl, // 👈 tu imagen
            Size = new Size(400, 250),
            FittingMode = FittingModeType.FitHeight
        };

        var fontStyle = new PropertyMap();
        fontStyle.Insert("family", new PropertyValue("SamsungOne"));
        fontStyle.Insert("weight", new PropertyValue("bold"));

        var metadata = new TextLabel
        {
            Text = "Drama  •  Romance  •  Acción",
            TextColor = Color.White,
            PointSize = 22,
            FontStyle = fontStyle,
            ClippingMode = ClippingModeType.Disabled,
        };

        var buttonsRow = new View
        {
            Name= "boton1",
            Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                CellPadding = new Size2D(0, 24)
            },
            ClippingMode = ClippingModeType.Disabled,
        };

        var primaryButton = CreatePrimaryButton("Ver ahora");
        primaryButton.Name = "PrimaryButton " + title; //necesario para saber cual boton le pego
        buttonsRow.Add(primaryButton);

        var secondaryButton = CreateSecondaryButton("Más información");
        secondaryButton.Name = "SecondaryButton " + title; //necesario para saber cual boton le pego
        buttonsRow.Add(secondaryButton);

        // 🧱 Armado
        content.Add(titleImage);
        content.Add(metadata);
        content.Add(buttonsRow);

        card.Add(background);
        card.Add(overlay);
        card.Add(content);

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

    private static View CreateBaseButton(
        string text,
        Color backgroundColor)
    {
        var button = new View
        {
            Size = new Size(306, 84),
            BackgroundColor = backgroundColor,
            CornerRadius = 8,
            Focusable = true,
            BorderlineWidth = 0
        };

        var fontStyle = new PropertyMap();
        fontStyle.Insert("family", new PropertyValue("SamsungOne"));
        fontStyle.Insert("weight", new PropertyValue("bold"));

        var label = new TextLabel
        {
            Text = text,
            TextColor = Color.White,
            PointSize = 28,
            HorizontalAlignment = HorizontalAlignment.Begin,
            VerticalAlignment = VerticalAlignment.Center,
            WidthResizePolicy = ResizePolicyType.FillToParent,
            HeightResizePolicy = ResizePolicyType.FillToParent,
            MultiLine = false,
            Padding = new Extents(20, 0, 0, 0),
            FontStyle = fontStyle
        };

        button.Add(label);

        button.KeyEvent += (s, e) =>
        {
            if (e.Key.State == Key.StateType.Down &&
                e.Key.KeyPressedName == "Return")
            {
                var asdasda = FocusManager.Instance.GetCurrentFocusView().Name;
                return true;
            }
            return false;
        };

        // 🔥 FOCO VISUAL REAL
        button.FocusGained += (s, e) =>
        {
            button.BorderlineWidth = 3;
            button.BorderlineColor = Color.White;
        };

        button.FocusLost += (s, e) =>
        {
            button.BorderlineWidth = 0;
        };
       
        return button;
    }

    private static View CreatePrimaryButton(string text)
    {
        return CreateBaseButton(
            "▶  " + text,
            new Color(1f, 0.45f, 0f, 1f)
        );
    }

    private static View CreateSecondaryButton(string text)
    {
        return CreateBaseButton(
            text,
            new Color(0.25f, 0.25f, 0.25f, 1f)
        );
    }

    //funcion para saber a que boton le estoy pegando
    public static View GetPrimaryButtonAt(int index)
    {
        if (_contentView == null) return null;

        if (index < 0 || index >= _contentView.Children.Count)
            return null;

        var card = _contentView.Children[index];

        return FindPrimaryButton(card);
    }

    private static View FindPrimaryButton(View parent)
    {
        foreach (var child in parent.Children)
        {
            if (child.Name.Contains("PrimaryButton "))
                return child;

            var found = FindPrimaryButton(child);
            if (found != null)
                return found;
        }

        return null;
    }

    public static View GetSecondaryButtonAt(int index)
    {
        if (_contentView == null) return null;

        if (index < 0 || index >= _contentView.Children.Count)
            return null;

        var card = _contentView.Children[index];

        return FindSecondaryButton(card);
    }

    private static View FindSecondaryButton(View parent)
    {
        foreach (var child in parent.Children)
        {
            if (child.Name.Contains("SecondaryButton "))
                return child;

            var found = FindSecondaryButton(child);
            if (found != null)
                return found;
        }

        return null;
    }

}
