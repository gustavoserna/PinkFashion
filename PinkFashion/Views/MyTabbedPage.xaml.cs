using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using PinkFashion.Controls;
using Plugin.Badge.Abstractions;
using PinkFashion.ViewModels;

namespace PinkFashion.Views
{
    public partial class MyTabbedPage : TabbedPage_R
    {
        MyTabbedPageViewModel myTabbedPageViewModel;

        public MyTabbedPage()
        {
            InitializeComponent();
            
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = myTabbedPageViewModel = new MyTabbedPageViewModel();
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

            NavigationPage bagNavigationPage = new NavigationPage(new Carrito())
            {
                BarBackgroundColor = App.bgColor,
                BarTextColor = App.textColor,
                IconImageSource = "gbag.png",
                Title = "Mi carrito"
            };

            //si es android se usa el plugin del badge
            if (Device.RuntimePlatform == Device.Android)
                bagNavigationPage.SetBinding(TabBadge.BadgeTextProperty, new Binding("Badge"));

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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            myTabbedPageViewModel.LoadingCommand.Execute(null);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if(CurrentPage.Title.Equals("Mi Carrito"))
            {

                if (!App.Current.Properties.ContainsKey("IdCliente") || !App.Current.Properties.ContainsKey("sesion"))
                {
                    Navigation.PushModalAsync(new Login());
                }
            }
        }
    }
}
