using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PinkFashion.ViewModels;
//using Plugin.FirebaseAnalytics;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class CarritosAbandonados : ContentPage
    {
        CarritoAbandonadoViewModel productosAbandonadosViewModel;
        public static bool root = false;
        string strEvento = "Abandonados|Pink Fashion Store";
        public CarritosAbandonados()
        {
            InitializeComponent();
            BindingContext = productosAbandonadosViewModel = new CarritoAbandonadoViewModel();
        }

        async void OnBtnRechazarCarritoClicked(object sender, EventArgs args)
        {

            try
            {
                await RechazarCarrito();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayAlert("Error", "Intentalo de nuevo mas tarde", "Ok");

            }

        }

        async void OnBtnCargarCarritoClicked(object sender, EventArgs args)
        {

            try
            {
                await CargarCarrito();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayAlert("Error", "Intentalo de nuevo mas tarde", "Ok");

            }

        }

        public async Task RechazarCarrito()
        {
            bool ac = await DisplayAlert("¿Deses vacíar el carrito?", "Confirmar", "Sí", "No");
            if (ac)
            {
                //vaciar carrito
                string Respuesta = await productosAbandonadosViewModel.GetRechazarCarrito();
                productosAbandonadosViewModel.LoadProductosCommand.Execute(null);
                //carrito

                if (Respuesta == "1")
                {                                    
                    await DisplayAlert("Listo", "Carrito vacío.", "Ok");
                    Thread.Sleep(2000);
                    Application.Current.MainPage = new NavigationPage(new Inicio())
                    {
                        BarBackgroundColor = App.bgColor,
                        BarTextColor = App.textColor
                    };

                }
                else
                {
                    await DisplayAlert("Precaución", "Intenta nuevamente mas tarde", "Ok");                    
                }

            }

        }


        public async Task CargarCarrito()
        {
            bool ac = await DisplayAlert("¿Deses cargar el carrito?", "Confirmar", "Sí", "No");
            if (ac)
            {
                //vaciar carrito
                string Respuesta = await productosAbandonadosViewModel.GetRecuperaCarrito();
                productosAbandonadosViewModel.LoadProductosCommand.Execute(null);
                //carrito

                if (Respuesta == "1")
                {
                    await DisplayAlert("Listo", "Su Carrito ha sido cargado", "Ok");
                    Thread.Sleep(2000);
                    Application.Current.MainPage = new NavigationPage(new Inicio())
                    {
                        BarBackgroundColor = App.bgColor,
                        BarTextColor = App.textColor
                    };

                }
                else
                {
                    await DisplayAlert("Precaución", "Intenta nuevamente mas tarde", "Ok");
                }

            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (productosAbandonadosViewModel.Productos.Count == 0)
            {
                productosAbandonadosViewModel.LoadProductosCommand.Execute(null);
            }
            productosAbandonadosViewModel.noProductos = App.Cart;
            productosAbandonadosViewModel.Monedero = App.Monedero;
            productosAbandonadosViewModel.EnviosGratis = App.EnvioGratis;
            if (root)
            {
                Navigation.PopToRootAsync();
                root = false;
            }

            App.eventTracker.SendScreen(strEvento, nameof(CarritosAbandonados));
        }
    }
}
