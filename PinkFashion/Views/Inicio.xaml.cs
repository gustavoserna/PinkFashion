using System;
using System.Collections.Generic;
using PinkFashion.ViewModels;
using Xamarin.Forms;
using Sharpnado.Presentation.Forms.RenderedViews;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using PinkFashion.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using Xamarin.Forms.OpenWhatsApp;

namespace PinkFashion.Views
{
    public partial class Inicio : ContentPage
    {

        InicioViewModel inicioViewModel;

        public Inicio()
        {
            InitializeComponent();

            BindingContext = inicioViewModel = new InicioViewModel(Navigation);

            if (Application.Current.Properties.ContainsKey("Abreviado"))
            {
                //lbNombre.Text = "Hola " + Application.Current.Properties["Abreviado"].ToString().Trim();
                //StackName.IsVisible = true;
            }
            else
            {
                //StackName.IsVisible = false;
            }
            var clickCuenta = new TapGestureRecognizer();
            clickCuenta.Tapped += async (s, e) =>
            {

                var page = new NavigationPage(new Cuenta());
                page.BarBackgroundColor = App.bgColor;
                page.BarTextColor = App.textColor;
                await Navigation.PushModalAsync(page);


            };
            //cuenta.GestureRecognizers.Add(clickCuenta);

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
            //carrito.GestureRecognizers.Add(clickCarrito);

            var clickAbandonados = new TapGestureRecognizer();
            clickAbandonados.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new CarritosAbandonados());

            };
            //lbAbandonado.GestureRecognizers.Add(clickAbandonados);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (inicioViewModel.ColFamilias.Count == 0)
            {
                inicioViewModel.LoadInicioCommand.Execute(null);
            }

            inicioViewModel.noProductos = App.Cart;
            inicioViewModel.Monedero = App.Monedero;
            inicioViewModel.EnviosGratis = App.EnvioGratis;
            inicioViewModel.Abandonados = App.Abandonados;
            App.eventTracker.SendScreen("Inicio|PinkFashionStore", nameof(Inicio));

        }


        async void OnBtnWhatsappClicked(object sender, EventArgs args)
        {
            try
            {
                Chat.Open("+528711223702", "Hola");
                App.eventTracker.SendScreen("Whatsapp|PinkFashionStore", "Whatsapp");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

    }
}
