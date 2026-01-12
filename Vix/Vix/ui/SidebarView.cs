using System;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Vix.UI
{
    public delegate void MenuItemSelectedEventHandler(string menuId);

    public class SidebarView : View
    {
        private View _menuItemsContainer;
        private List<SidebarItem> _itemsList = new List<SidebarItem>();
        private Animation _expandAnimation;
        private bool _isExpanded = false;
        private SidebarItem _lastFocusedItem = null;

        public event MenuItemSelectedEventHandler MenuItemSelected;
        public event EventHandler RequestCollapse;

        public SidebarView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackgroundColor = new Color(0.05f, 0.05f, 0.05f, 1.0f);
            this.HeightResizePolicy = ResizePolicyType.FillToParent;
            this.Size2D = new Size2D(120, 1080);
            this.ClippingMode = ClippingModeType.ClipToBoundingBox;

            this.Sensitive = true;

            this.HoverEvent += (s, e) => {
                if (e.Hover.GetState(0) == PointStateType.Started)
                {
                    AnimateSidebar(true);
                    // CAMBIO CRÍTICO: Retornamos false para que el evento 
                    // continúe hacia los botones hijos (SidebarItem)
                    return false;
                }
                return false;
            };

            this.Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            CreateLogo();

            _menuItemsContainer = new View
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FitToChildren,
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size2D(0, 20)
                },
                Margin = new Extents(0, 0, 50, 0)
            };
            this.Add(_menuItemsContainer);

            AddMenuItem("home", "Inicio", "🏠");
            AddMenuItem("sports", "Deportes", "⚽");
            AddMenuItem("news", "Noticias", "📰");
            AddMenuItem("premium", "Premium", "💎");
            AddMenuItem("account", "Cuenta", "👤");
        }

        private void CreateLogo()
        {
            var logoContainer = new View
            {
                Size2D = new Size2D(80, 80),
                Margin = new Extents(0, 0, 40, 20),
                BackgroundColor = new Color(1.0f, 0.4f, 0.0f, 1.0f)
            };
            logoContainer.Add(new TextLabel
            {
                Text = "ViX",
                TextColor = Color.White,
                PointSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent
            });
            this.Add(logoContainer);
        }

        private void AddMenuItem(string id, string text, string icon)
        {
            var item = new SidebarItem(id, text, icon);


            item.Sensitive = true;

            // Foco con teclado/control
            item.FocusGained += (s, e) => {
                _lastFocusedItem = item;
                AnimateSidebar(true);
                MenuItemSelected?.Invoke(id);
            };

            // Tecla Derecha para salir del menú
            item.KeyEvent += (s, e) => {
                if (e.Key.State == Key.StateType.Down && e.Key.KeyPressedName == "Right")
                {
                    AnimateSidebar(false);
                    RequestCollapse?.Invoke(this, EventArgs.Empty);
                    return true;
                }
                return false;
            };

            // HOVER: Detectar entrada del mouse
            item.HoverEvent += (s, e) => {
                if (e.Hover.GetState(0) == PointStateType.Started)
                {
                    AnimateSidebar(true);
                    return true;
                }
                return false;
            };

            // TOUCH/CLICK: Detectar clics
            item.TouchEvent += (s, e) => {
                PointStateType state = e.Touch.GetState(0);
                if (state == PointStateType.Down) return true; // OBLIGATORIO para recibir el Up
                if (state == PointStateType.Up)
                {
                    FocusManager.Instance.SetCurrentFocusView(item);
                    MenuItemSelected?.Invoke(id);
                    return true;
                }
                return false;
            };

            _itemsList.Add(item);
            _menuItemsContainer.Add(item);

            if (_itemsList.Count > 1)
            {
                _itemsList[_itemsList.Count - 2].DownFocusableView = item;
                item.UpFocusableView = _itemsList[_itemsList.Count - 2];
            }
        }

        public void AnimateSidebar(bool expand)
        {
            if (_isExpanded == expand) return;
            _isExpanded = expand;

            int targetWidth = expand ? 450 : 120;
            float targetOpacity = expand ? 1.0f : 0.0f;

            if (_expandAnimation != null)
            {
                _expandAnimation.Stop();
                _expandAnimation.Finished -= OnAnimationFinished;
                _expandAnimation.Clear();
            }

            _expandAnimation = new Animation(300);
            _expandAnimation.DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOut);

            // SOLUCIÓN AL CRASH: "Size" es Vector3, por lo tanto usamos 'new Size' con 3 parámetros (X, Y, Z)
            _expandAnimation.AnimateTo(this, "Size", new Size(targetWidth, 1080, 0));

            foreach (var item in _itemsList)
            {
                _expandAnimation.AnimateTo(item.Label, "Opacity", targetOpacity);
            }

            _expandAnimation.Finished += OnAnimationFinished;
            _expandAnimation.Play();
        }

        private void OnAnimationFinished(object sender, EventArgs e)
        {
            this.Size2D = new Size2D(_isExpanded ? 450 : 120, 1080);
        }

        public void RestoreFocus()
        {
            if (_lastFocusedItem != null) FocusManager.Instance.SetCurrentFocusView(_lastFocusedItem);
            else if (_itemsList.Count > 0) FocusManager.Instance.SetCurrentFocusView(_itemsList[0]);
        }
    }

    // Clase interna para los botones
    internal class SidebarItem : View
    {
        public TextLabel Icon { get; }
        public TextLabel Label { get; }
        public SidebarItem(string id, string text, string iconEmoji)
        {
            this.Focusable = true;
            this.Sensitive = true;
            this.WidthResizePolicy = ResizePolicyType.FillToParent;
            this.Size2D = new Size2D(0, 85);
            this.Padding = new Extents(30, 0, 0, 0);
            this.Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                CellPadding = new Size2D(25, 0)
            };

            Icon = new TextLabel { Text = iconEmoji, PointSize = 28, TextColor = new Color(0.8f, 0.8f, 0.8f, 1.0f) };
            Label = new TextLabel { Text = text, PointSize = 22, TextColor = new Color(0.8f, 0.8f, 0.8f, 1.0f), Opacity = 0.0f };

            this.Add(Icon);
            this.Add(Label);

            this.FocusGained += (s, e) => {
                Icon.TextColor = Color.White;
                Label.TextColor = Color.White;
                this.BackgroundColor = new Color(1, 1, 1, 0.1f);
            };
            this.FocusLost += (s, e) => {
                Icon.TextColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
                Label.TextColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
                this.BackgroundColor = Color.Transparent;
            };
        }
    }
}