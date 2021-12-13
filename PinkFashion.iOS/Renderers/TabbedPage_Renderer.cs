using CoreGraphics;
using CoreText;
using Foundation;
using PinkFashion.Controls;
using PinkFashion.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage_R), typeof(TabbedPage_Renderer))]
namespace PinkFashion.iOS.Renderers
{
    public class TabbedPage_Renderer : TabbedRenderer
    {
        int Badge;
        UITabBarItem itemAux;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != e.OldElement)
            {
                if (e.OldElement != null)
                {
                    e.OldElement.PropertyChanged -= OnElementPropertyChanged;
                }
                if (e.NewElement != null)
                {
                    e.NewElement.PropertyChanged += OnElementPropertyChanged;
                }
            }

            if (e.NewElement != null)
            {
                var tabbedPage_R = e.NewElement as TabbedPage_R;
                Badge = tabbedPage_R.Badge;

            }
        }

        void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender != null)
            {
                var tabbedPage_R = sender as TabbedPage_R;
                Badge = tabbedPage_R.Badge;
            }

            if (itemAux != null)
            {
                itemAux.BadgeValue = Badge.ToString();
                /*itemAux.SetFinishedImages(
                        DibujarBadgeCarrito(itemAux.Image),
                        DibujarBadgeCarrito(itemAux.Image)
                        );*/
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            if (TabBar?.Items == null) return;

            var tabs = Element as TabbedPage;

            if (tabs != null)
            {
                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    UpdateItem(TabBar.Items[i], tabs.Children[i].IconImageSource.ToString());
                }
            }

            base.ViewWillAppear(animated);
        }

        private void UpdateItem(UITabBarItem item, string icon)
        {
            if (item == null || icon == null) return;
            //UITabBar.Appearance.SelectionIndicatorImage = BackroundSeleccion(Color.FromHex("#dd023b").ToUIColor(), new CGSize((UIScreen.MainScreen.Bounds.Width / base.ViewControllers.Length) * .5, TabBar.Bounds.Size.Height + 4), new CGSize(UIScreen.MainScreen.Bounds.Width / base.ViewControllers.Length, 4));

            if (item.Title.Equals("Bolsa")) //si el titulo es Mi carrito agrega la insignia
            {
                item.BadgeValue = Badge.ToString();
                itemAux = item;
                /*item.SetFinishedImages(
                    DibujarBadgeCarrito(item.Image),
                    DibujarBadgeCarrito(item.Image)
                    );
                itemAux = item;*/
            }

            item.SetTitleTextAttributes(new UITextAttributes()
            {
                Font = UIFont.FromName("Helvetica", 10),
                TextColor = Color.FromHex("#757575").ToUIColor()
            }, UIControlState.Normal);

            item.SetTitleTextAttributes(new UITextAttributes()
            {
                Font = UIFont.FromName("Helvetica", 10)
                //TextColor = Constantes.colorTexto.ToUIColor()
            }, UIControlState.Selected);

        }

        /*UIImage BackroundSeleccion(UIColor color, CGSize size, CGSize lineSize)
        {
            var rect = new CGRect(0, 0, size.Width, size.Height);
            var rectLine = new CGRect(0, size.Height - lineSize.Height, lineSize.Width, lineSize.Height);
            UIGraphics.BeginImageContextWithOptions(size, false, 0);
            UIColor.Clear.SetFill();
            UIGraphics.RectFill(rect);
            color.SetFill();
            UIGraphics.RectFill(rectLine);
            var img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return img;
        }*/

        UIImage DibujarBadgeCarrito(UIImage imagen)
        {
            var rectIcon = new CGRect(0, 0, imagen.Size.Width, imagen.Size.Height); //rectangulo de todo el contenido
            var widthRectBadge = imagen.Size.Width / 1.5;
            var heightRectBadge = imagen.Size.Height / 1.5;
            UIBezierPath rectBadge = null;

            var textoBadgeAttr = new NSAttributedString(Badge.ToString(),
                new CTStringAttributes
                {
                    Font = new CTFont("Helvetica", 12)

                }
            ); //atributos que tendrá la label

            var textoBadge = new UILabel()
            {
                AttributedText = textoBadgeAttr,
                TextColor = Color.FromRgb(255, 255, 255).ToUIColor()
            }; //label que lleva contenido al texto con atributos

            var widthTextoBadge = textoBadgeAttr.Size.Width;
            var HeigthTextoBadge = textoBadgeAttr.Size.Height;

            if (widthTextoBadge < widthRectBadge)
                rectBadge = UIBezierPath.FromRoundedRect(new CGRect(widthRectBadge * .9, 0, widthRectBadge, heightRectBadge), (float)10);
            else
                rectBadge = UIBezierPath.FromRoundedRect(new CGRect(widthRectBadge * .9, 0, widthTextoBadge + 8, heightRectBadge), (float)10);

            UIGraphics.BeginImageContextWithOptions(new CGSize(imagen.Size.Width * 1.7, imagen.Size.Height), false, 0); // se inicia el lienzo para dibujar
            UIColor.Clear.SetFill(); // se pone transparente el fondo
            UIGraphics.RectFill(rectIcon); // se agrega el rectangulo para el icono
            imagen.Draw(rectIcon); // se dibuja el icono
            UIColor.FromRGB(0, 0, 0).SetFill(); // se pinta del color que se quiere salga la insignia
            rectBadge.Fill(); // se agrega el rect de la insignia al lienzo
            textoBadge.DrawText(new CGRect((rectBadge.Bounds.Size.Width / 2) - (widthTextoBadge / 2) + (rectBadge.Bounds.X), 9.5, 0, 0));
            var img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return img;
        }
    }
}