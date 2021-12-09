using System;
using System.Collections.Generic;
using PinkFashion.Models;
using PinkFashion.ViewModels;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class FamiliaVista : ContentPage
    {
        FamiliaVistaViewModel familiaVistaViewModel;

        public FamiliaVista(Familia familia)
        {
            InitializeComponent();

            /*var clickCuenta = new TapGestureRecognizer();
            clickCuenta.Tapped += async (s, e) =>
            {

                var page = new NavigationPage(new Cuenta());
                page.BarBackgroundColor = App.bgColor;
                page.BarTextColor = App.textColor;
                await Navigation.PushModalAsync(page);


            };
            cuenta.GestureRecognizers.Add(clickCuenta);*/

            var clickCarrito = new TapGestureRecognizer();
            clickCarrito.Tapped += async (s, e) =>
            {
                if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                {
                    if (Application.Current.Properties["sesion"].Equals("activa"))
                    {
                        var page = new NavigationPage(new Carrito());
                        page.BarBackgroundColor = App.bgColor;
                        page.BarTextColor = App.textColor;
                        await Navigation.PushModalAsync(page);

                    }
                    else
                    {
                        bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await Navigation.PushAsync(new Login());
                        }
                    }
                }
                else
                {
                    bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                    if (ac)
                    {
                        await Navigation.PushAsync(new Login());
                    }
                }

            };
            carrito.GestureRecognizers.Add(clickCarrito);

            BindingContext = familiaVistaViewModel = new FamiliaVistaViewModel(familia);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(familiaVistaViewModel.Productos.Count == 0)
            {
                familiaVistaViewModel.LoadProductosCommand.Execute(null);
            }

            if(familiaVistaViewModel.Categorias.Count == 0)
            {
                familiaVistaViewModel.LoadCategoriasCommand.Execute(null);
            }
        }
    }
}
