using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using TizenDotNet1.shared.Utils;
using Vix.services.Events;
using Vix.UI;
using static TizenDotNet1.shared.Dtos.BuilderDto;

namespace Vix
{
    public class Program : NUIApplication
    {
        public static SidebarView _sidebar;
        private View _contentArea;
        private TextLabel _contentTitle;
        private UiResponse ui;

        public static View sectionContainer;
        public static View detailHero;
        public static View thumbnails;
        public static View heroCarousel;
        public static View thumbnails2;
        public static List<View> _carouselRoots = new();

        protected override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        private void Initialize()
        {
            Window window = Window.Instance;
            window.BackgroundColor = Color.Black;

            var root = new View { WidthResizePolicy = ResizePolicyType.FillToParent, HeightResizePolicy = ResizePolicyType.FillToParent };
            window.Add(root);

            //carousel container
            sectionContainer = new View
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FitToChildren,
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size2D(0, 40)
                },
                Focusable = true,
                Sensitive = true
            };

            // 1. ÁREA DE CONTENIDO (Fondo)
            _contentArea = new View
            {
                BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 1.0f),
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                Padding = new Extents(120, 0, 0, 0), // Margen para el menú colapsado
                Focusable = true,
                Sensitive = true
            };

            // HOVER CONTENIDO: Contraer sidebar al salir de él
            _contentArea.HoverEvent += (s, e) =>
            {
                if (e.Hover.GetState(0) == PointStateType.Started)
                {
                    _sidebar.AnimateSidebar(false);
                    return true;
                }
                return false;
            };

            _contentArea.KeyEvent += (s, e) =>
            {
                if (e.Key.State == Key.StateType.Down && e.Key.KeyPressedName == "Left")
                {
                    Program._carouselRoots.Clear();
                    _sidebar.RestoreFocus();
                    return true;
                }
                return false;
            };
            root.Add(_contentArea);

            // 2. SIDEBAR (Al frente)
            _sidebar = new SidebarView();


            //agrego json de los carruseles de la seccion
            string json =
                "{\n" +
                "  \"data\": {\n" +
                "    \"uiPage\": {\n" +
                "      \"pageName\": \"Novelas\",\n" +
                "      \"urlPath\": \"/ondemand/novelas\",\n" +
                "      \"uiModules\": {\n" +
                "        \"totalCount\": 14,\n" +
                "        \"edges\": [\n" +

                "          {\n" +
                "            \"__typename\": \"UiModuleEdge\",\n" +
                "            \"cursor\": \"hero-carousel\",\n" +
                "            \"node\": {\n" +
                "              \"trackingMetadataJson\": {\n" +
                "                \"ui_module_id\": \"hero-001\",\n" +
                "                \"ui_module_title\": \"Hero\",\n" +
                "                \"ui_navigation_section\": \"/ondemand/novelas\",\n" +
                "                \"ui_is_recommendation\": false,\n" +
                "                \"ui_object_type\": \"HERO_CAROUSEL\",\n" +
                "                \"ui_carousel_slug\": \"hero-demo\"\n" +
                "              },\n" +
                "              \"moduleType\": \"HERO_CAROUSEL\",\n" +
                "              \"id\": \"hero-001\",\n" +
                "              \"title\": \"Hero\",\n" +
                "              \"scrollingTimeSeconds\": null,\n" +
                "              \"contents\": {\n" +
                "                \"totalCount\": 10,\n" +
                "                \"edges\": [\n" +
                "                  { \"cursor\": \"1\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 1\", \"heroImageUrl\": \"https://picsum.photos/id/1011/800/450\" } },\n" +
                "                  { \"cursor\": \"2\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 2\", \"heroImageUrl\": \"https://picsum.photos/id/1012/800/450\" } },\n" +
                "                  { \"cursor\": \"3\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 3\", \"heroImageUrl\": \"https://picsum.photos/id/1013/800/450\" } },\n" +
                "                  { \"cursor\": \"4\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 4\", \"heroImageUrl\": \"https://picsum.photos/id/1014/800/450\" } },\n" +
                "                  { \"cursor\": \"5\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 5\", \"heroImageUrl\": \"https://picsum.photos/id/1015/800/450\" } },\n" +
                "                  { \"cursor\": \"6\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 6\", \"heroImageUrl\": \"https://picsum.photos/id/1016/800/450\" } },\n" +
                "                  { \"cursor\": \"7\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 7\", \"heroImageUrl\": \"https://picsum.photos/id/1018/800/450\" } },\n" +
                "                  { \"cursor\": \"8\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 8\", \"heroImageUrl\": \"https://picsum.photos/id/1020/800/450\" } },\n" +
                "                  { \"cursor\": \"9\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 9\", \"heroImageUrl\": \"https://picsum.photos/id/1024/800/450\" } },\n" +
                "                  { \"cursor\": \"10\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 10\", \"heroImageUrl\": \"https://picsum.photos/id/1025/800/450\" } }\n" +
                "                ]\n" +
                "              }\n" +
                "            }\n" +
                "          },\n" +

                "          {\n" +
                "            \"__typename\": \"UiModuleEdge\",\n" +
                "            \"cursor\": \"8d4e2a87a9840549dbc87c80dbff5b27e30b927d\",\n" +
                "            \"node\": {\n" +
                "              \"trackingMetadataJson\": {\n" +
                "                \"ui_module_id\": \"8d4e2a87a9840549dbc87c80dbff5b27e30b927d\",\n" +
                "                \"ui_module_title\": \"Novelas más vistas\",\n" +
                "                \"ui_navigation_section\": \"/ondemand/novelas\",\n" +
                "                \"ui_is_recommendation\": false,\n" +
                "                \"ui_object_type\": \"VIDEO_CAROUSEL\",\n" +
                "                \"ui_carousel_slug\": \"content-list-87itejctbvi1749578927275\"\n" +
                "              },\n" +
                "              \"moduleType\": \"VIDEO_CAROUSEL\",\n" +
                "              \"Title\": \"Novelas más vistas11111111111111111111\",\n" +
                "              \"contents\": {\n" +
                "                \"totalCount\": 5,\n" +
                "                \"edges\": [\n" +
                "                  { \"cursor\": \"1\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 1\", \"heroImageUrl\": \"https://picsum.photos/id/1011/800/450\" } },\n" +
                "                  { \"cursor\": \"2\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 2\", \"heroImageUrl\": \"https://picsum.photos/id/1012/800/450\" } },\n" +
                "                  { \"cursor\": \"3\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 3\", \"heroImageUrl\": \"https://picsum.photos/id/1013/800/450\" } },\n" +
                "                  { \"cursor\": \"4\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 4\", \"heroImageUrl\": \"https://picsum.photos/id/1014/800/450\" } },\n" +
                "                  { \"cursor\": \"5\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 5\", \"heroImageUrl\": \"https://picsum.photos/id/1015/800/450\" } }\n" +
                "                ]\n" +
                "              }\n" +
                "            }\n" +
                "          },\n" +

                "          {\n" +
                "            \"__typename\": \"UiModuleEdge\",\n" +
                "            \"cursor\": \"8d4e2a87a9840549dbc87c80dbff5b27e30b927d\",\n" +
                "            \"node\": {\n" +
                "              \"trackingMetadataJson\": {\n" +
                "                \"ui_module_id\": \"8d4e2a87a9840549dbc87c80dbff5b27e30b927d\",\n" +
                "                \"ui_module_title\": \"Novelas más vistas2\",\n" +
                "                \"ui_navigation_section\": \"/ondemand/novelas\",\n" +
                "                \"ui_is_recommendation\": false,\n" +
                "                \"ui_object_type\": \"VIDEO_CAROUSEL\",\n" +
                "                \"ui_carousel_slug\": \"content-list-87itejctbvi1749578927275\"\n" +
                "              },\n" +
                "              \"moduleType\": \"VIDEO_CAROUSEL\",\n" +
                "              \"Title\": \"Novelas más vistas 222222222222222222222222222\",\n" +
                "              \"contents\": {\n" +
                "                \"totalCount\": 5,\n" +
                "                \"edges\": [\n" +
                "                  { \"cursor\": \"1\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 1\", \"heroImageUrl\": \"https://picsum.photos/id/1011/800/450\" } },\n" +
                "                  { \"cursor\": \"2\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 2\", \"heroImageUrl\": \"https://picsum.photos/id/1012/800/450\" } },\n" +
                "                  { \"cursor\": \"3\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 3\", \"heroImageUrl\": \"https://picsum.photos/id/1013/800/450\" } },\n" +
                "                  { \"cursor\": \"4\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 4\", \"heroImageUrl\": \"https://picsum.photos/id/1014/800/450\" } },\n" +
                "                  { \"cursor\": \"5\", \"node\": { \"textTitle\": null, \"heroTargetContentType\": \"VIDEO\", \"heroTarget\": {}, \"heroTitle\": \"Hero 5\", \"heroImageUrl\": \"https://picsum.photos/id/1015/800/450\" } }\n" +
                "                ]\n" +
                "              }\n" +
                "            }\n" +
                "          }\n" +

                "        ]\n" +
                "      }\n" +
                "    }\n" +
                "  },\n" +
                "  \"extensions\": {\n" +
                "    \"queryComplexity\": 64227,\n" +
                "    \"traceId\": \"5ce14caf7f0c096f84941a7ad92da66f\"\n" +
                "  }\n" +
                "}";

            //defino _contentTitle
            _contentTitle = new TextLabel
            {
                Text = "VI TEST",
                TextColor = Color.White,
                PointSize = 35,
                HorizontalAlignment = HorizontalAlignment.Center,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent
            };

            ui = JsonSerializer.Deserialize<UiResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


            _contentArea.FocusGained += (s, e) =>
            {
                if (_contentArea.Children.Any())
                    FocusManager.Instance.SetCurrentFocusView(_contentArea.Children[0]);
            };

            _sidebar.MenuItemSelected += (id) =>
            {
                ClearContentArea();

                if (id == "sports")
                {
                    FilmsHeroCarousel.StartAutoPlay();

                    if (heroCarousel == null)
                        heroCarousel = UiModuleBuilder.Build(ui.Data.UiPage.UiModules.Edges[0].Node);

                    if (thumbnails == null)
                        thumbnails = UiModuleBuilder.Build(ui.Data.UiPage.UiModules.Edges[1].Node);

                    if (thumbnails2 == null)
                        thumbnails2 = UiModuleBuilder.Build(ui.Data.UiPage.UiModules.Edges[2].Node);

                    if (detailHero == null)
                        detailHero = UiDetailHeroBuilder.Build();

                    sectionContainer.Add(heroCarousel);
                    sectionContainer.Add(thumbnails);
                    _carouselRoots.Add(thumbnails);
                    sectionContainer.Add(thumbnails2);
                    _carouselRoots.Add(thumbnails2);
                    _contentArea.Add(sectionContainer);

                    //input del control remoto
                    FilmsHeroCarousel.Events();
                    FilmsThumbnailsCarousel.Events();

                    //le doy foco cada vez que entra al primer thumbnail
                    sectionContainer.FocusGained += (s, e) =>
                    {
                        if (_contentArea.Children.Any())
                            FocusManager.Instance.SetCurrentFocusView(sectionContainer.Children[0]);
                    };
                }
                else
                {
                    Program._carouselRoots.Clear();
                    _contentTitle.Text = "Sección: " + id.ToUpper();
                    _contentArea.Add(_contentTitle);
                }
            };
            _sidebar.RequestCollapse += (s, e) => { FocusManager.Instance.SetCurrentFocusView(_contentArea); };

            // Importante: Agregar al final para que esté por encima de _contentArea
            root.Add(_sidebar);        

            _sidebar.RestoreFocus();
        }

        private void ClearContentArea()
        {
            while (sectionContainer.ChildCount > 0)
            {
                var child2 = sectionContainer.GetChildAt(0);
                sectionContainer.Remove(child2);
            }

            while (_contentArea.ChildCount > 0)
            {
                var child = _contentArea.GetChildAt(0);
                _contentArea.Remove(child);
            }
        }

        static void Main(string[] args)
        {
            new Program().Run(args);
        }
    }
}