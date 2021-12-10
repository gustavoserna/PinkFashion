using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace PinkFashion.Views
{
    public partial class MyTabbedPage : Xamarin.Forms.TabbedPage
    {
        public MyTabbedPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            //deshabilitar el swipe en android
            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);

            BarBackgroundColor = App.bgColor;
            SelectedTabColor = App.textColor;
            UnselectedTabColor = Color.FromHex("#ccc");

            NavigationPage homeNavigationPage = new NavigationPage(new Inicio())
            {
                BarBackgroundColor = App.bgColor,
                BarTextColor = App.textColor,
                IconImageSource = "ghome.png",
                Title = "Inicio",
            };

            /*Page homePage = new Inicio()
            {
                IconImageSource = "ghome.png",
                Title = "Inicio",
            };*/

            NavigationPage bagNavigationPage = new NavigationPage(new Carrito())
            {
                BarBackgroundColor = App.bgColor,
                BarTextColor = App.textColor,
                IconImageSource = "gbag.png",
                Title = "Mi Bolsa"
            };

            NavigationPage settingsNavigationPage = new NavigationPage(new Cuenta())
            {
                BarBackgroundColor = App.bgColor,
                BarTextColor = App.textColor,
                IconImageSource = "gsettings.png",
                Title = "Mi Cuenta"
            };

            Children.Add(homeNavigationPage);
            Children.Add(bagNavigationPage);
            Children.Add(settingsNavigationPage);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            if(CurrentPage.Title.Equals("Mi Bolsa"))
            {
                if (!App.Current.Properties.ContainsKey("IdCliente") || !App.Current.Properties.ContainsKey("sesion"))
                {
                    Device.BeginInvokeOnMainThread(async () => {

                        bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await Navigation.PushModalAsync(new Login());
                        }

                    });
                }
            }
        }
    }
}
