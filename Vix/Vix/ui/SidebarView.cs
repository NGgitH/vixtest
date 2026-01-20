using System;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using TizenDotNet1.shared.Dtos;

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
            // ESTILO: Fondo negro profundo (Match con diseño ViX)
            this.BackgroundColor = new Color(0.05f, 0.05f, 0.05f, 1.0f);
            this.HeightResizePolicy = ResizePolicyType.FillToParent;
            this.Size2D = new Size2D(120, 1080);
            this.ClippingMode = ClippingModeType.ClipToBoundingBox;

            this.Sensitive = true;

            this.HoverEvent += (s, e) => {
                if (e.Hover.GetState(0) == PointStateType.Started)
                {
                    AnimateSidebar(true);
                    return false;
                }
                return false;
            };

            this.Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                LinearAlignment = LinearLayout.Alignment.Center
            };

            CreateLogo();

            _menuItemsContainer = new View
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FitToChildren,
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    // ESTILO: Menor separación entre items para que se vea compacto
                    CellPadding = new Size2D(0, 10)
                },
                // ESTILO: Margen izquierdo para dar espacio a la forma de cápsula
                Margin = new Extents(15, 15, 50, 0)
            };
            this.Add(_menuItemsContainer);
        }

        private void CreateLogo()
        {
            var logoContainer = new View
            {
                Size2D = new Size2D(80, 80),
                Margin = new Extents(0, 0, 40, 20),
                BackgroundColor = new Color(1.0f, 0.4f, 0.0f, 1.0f) // Naranja ViX
            };
            logoContainer.Add(new TextLabel
            {
                Text = "ViX",
                TextColor = Color.White,
                PointSize = 20,
                // ESTILO: Fuente más gruesa para el logo si es posible, o mantenemos default
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent
            });
            this.Add(logoContainer);
        }

        public void LoadMenuItems(List<NavigationItem> list)
        {
            // Limpieza previa necesaria para no duplicar si recargas
            foreach (var item in _itemsList)
            {
                _menuItemsContainer.Remove(item);
                item.Dispose();
            }
            _itemsList.Clear();

            foreach (var item in list)
            {
                // TODO: Aquí deberías mapear item.IconName a un emoji o imagen real
                // Por ahora mantengo el emoji default como pediste no cambiar lógica
                string iconChar = "🏠";
                if (item.IconName == "SEARCH") iconChar = "🔍";
                if (item.IconName == "USER") iconChar = "👤";

                AddMenuItem(item.Id, item.Text, iconChar);
            }
        }

        private void AddMenuItem(string id, string text, string icon)
        {
            var item = new SidebarItem(id, text, icon);

            item.Sensitive = true;

            item.FocusGained += (s, e) => {
                _lastFocusedItem = item;
                AnimateSidebar(true);
                MenuItemSelected?.Invoke(id);
            };

            item.KeyEvent += (s, e) => {
                if (e.Key.State == Key.StateType.Down && e.Key.KeyPressedName == "Right")
                {
                    AnimateSidebar(false);
                    RequestCollapse?.Invoke(this, EventArgs.Empty);
                    return true;
                }
                return false;
            };

            item.HoverEvent += (s, e) => {
                if (e.Hover.GetState(0) == PointStateType.Started)
                {
                    AnimateSidebar(true);
                    return true;
                }
                return false;
            };

            item.TouchEvent += (s, e) => {
                PointStateType state = e.Touch.GetState(0);
                if (state == PointStateType.Down) return true;
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

            // ESTILO: Ancho ajustado a 400px (más elegante)
            int targetWidth = expand ? 400 : 120;
            float targetOpacity = expand ? 1.0f : 0.0f;

            if (_expandAnimation != null)
            {
                _expandAnimation.Stop();
                _expandAnimation.Finished -= OnAnimationFinished;
                _expandAnimation.Clear();
            }

            _expandAnimation = new Animation(300);
            _expandAnimation.DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOut);

            // Ajuste de vector3
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
            this.Size2D = new Size2D(_isExpanded ? 400 : 120, 1080);
        }

        public void RestoreFocus()
        {
            if (_lastFocusedItem != null) FocusManager.Instance.SetCurrentFocusView(_lastFocusedItem);
            else if (_itemsList.Count > 0) FocusManager.Instance.SetCurrentFocusView(_itemsList[0]);
        }
    }
    // Clase interna para los botones con ESTILOS STRICTOS DE DOCUMENTACIÓN
    internal class SidebarItem : View
    {
        public TextLabel Icon { get; }
        public TextLabel Label { get; }

        // Colores según documentación implícita (Blanco sobre fondo oscuro)
        private readonly Color ColorInactive = new Color(0.6f, 0.6f, 0.6f, 1.0f); 
        private readonly Color ColorActive = new Color(1.0f, 1.0f, 1.0f, 1.0f); // #FFFFFF
        private readonly Color BgFocus = new Color(1.0f, 1.0f, 1.0f, 0.15f);
        private readonly Color BgTransparent = Color.Transparent;

        public SidebarItem(string id, string text, string iconEmoji)
        {
            this.Focusable = true;
            this.Sensitive = true;
            this.WidthResizePolicy = ResizePolicyType.FillToParent;
            
            // Altura ajustada para contener el texto. 
            // Si la fuente es grande, el item debe ser más alto.
            this.Size2D = new Size2D(0, 80); 
            
            this.Padding = new Extents(30, 0, 0, 0);
            
            // Borde redondeado (Pill shape)
           // this.CornerRadius = 40.0f; // Mitad de la altura (80/2)
            this.BackgroundColor = BgTransparent;

            this.Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal,
                LinearAlignment =  LinearLayout.Alignment.Center,
                CellPadding = new Size2D(25, 0)
            };

            // ICONO
            Icon = new TextLabel 
            { 
                Text = iconEmoji, 
                // Ajustamos el icono para que esté balanceado con el texto
                PointSize = 32.0f, 
                TextColor = ColorInactive,
                FontFamily = "Inter", // Intento de carga directa si está instalada
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            // TEXTO (Label) - APLICANDO REGLAS DE DOC
            Label = new TextLabel 
            { 
                Text = text, 
                TextColor = ColorInactive, // #FFFFFF en activo, Gris en inactivo
                
                // 1. Font Family
                FontFamily = "Inter", 
                
                // 2. Font Size 
                // NOTA: 120px es para 4K o diseño web escalado. 
                // Para 1080p usamos ~30-40px. Si tu app es 4K nativa, pon 120.0f
                PointSize = 32.0f, 

                // 3. Font Weight: 500 (Medium)
                // Tizen usa strings para el peso. 
                // "Medium" suele mapear a FontWeight "Normal" o "SemiBold" dependiendo del sistema.
                // Intentamos pasarlo explícitamente si la fuente lo soporta.
              
                // 4. Line Height: 100%
                // En Tizen, LineHeight se suele manejar relativo o absoluto.
                // 100% suele significar que no hay espaciado extra.
                // LineHeight = 32.0f, // Igual al PointSize

                // 5. Letter Spacing: -6%
                // Tizen toma el valor en píxeles negativos aprox.
                // -6% de 32px es aprox -1.92px
                LineSpacing = -1.92f, 

                Opacity = 0.0f,
                VerticalAlignment = VerticalAlignment.Center
            };

            this.Add(Icon);
            this.Add(Label);

            // --- ESTADOS ---

            this.FocusGained += (s, e) => {
                // background: #FFFFFF (según doc para activo, aplicamos opacidad para no tapar)
                Icon.TextColor = ColorActive; // #FFFFFF
                Label.TextColor = ColorActive; // #FFFFFF
                this.BackgroundColor = BgFocus; 
            };
            
            this.FocusLost += (s, e) => {
                Icon.TextColor = ColorInactive;
                Label.TextColor = ColorInactive;
                this.BackgroundColor = BgTransparent;
            };
        }
    }

}
