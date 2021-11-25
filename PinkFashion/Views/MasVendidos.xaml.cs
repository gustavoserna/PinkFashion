using System;
using PinkFashion.ViewModels;
using PinkFashion.Models;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class MasVendidos : ContentPage
    {
        MasVendidosViewModel masvendidosViewModel;
        public static bool root = false;
        string strEvento = "Más Vendidos | Pink Fashion Store";
        public MasVendidos()
        {
            InitializeComponent();
            BindingContext = masvendidosViewModel = new MasVendidosViewModel();
            
            if (Application.Current.Properties.ContainsKey("Abreviado"))
            {
                //lbNombre.Text = "Hola " + Application.Current.Properties["Abreviado"].ToString();
                StackName.IsVisible = true;
            }
            else
            {
                StackName.IsVisible = false;
            }
            
            buscar.Placeholder = "Buscar un producto";

            listview.ItemTapped += (s, e) =>
            {
                var item = e.Item as Producto_;
                if (item.ConVariante > 0)
                { Navigation.PushAsync(new Producto(item));
                }
                else
                {
                    Navigation.PushAsync(new Producto(item));
                }

            };

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

            buscar.TextChanged += (s, e) =>
            {
                masvendidosViewModel.Buscar(buscar.Text);
            };

            Device.StartTimer(new TimeSpan(0, 0, 5), () =>
            {
                // do something every 30 seconds
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (StackAbandonado.IsVisible == true)
                    {
                        if (lbMonedero.IsVisible == true)
                        {
                            lbMonedero.IsVisible = false;
                            lbEnvios.IsVisible = true;
                            lbAbandonado.IsVisible = false;

                        }
                        else if (lbEnvios.IsVisible == true)
                        {
                            lbMonedero.IsVisible = false;
                            lbEnvios.IsVisible = false;
                            lbAbandonado.IsVisible = true;
                        }
                        else if (lbAbandonado.IsVisible == true)
                        {
                            lbMonedero.IsVisible = true;
                            lbEnvios.IsVisible = false;
                            lbAbandonado.IsVisible = false;
                        }
                    }
                    else
                    {
                        lbAbandonado.IsVisible = false;
                        if (lbMonedero.IsVisible == true)
                        {
                            lbMonedero.IsVisible = false;
                            lbEnvios.IsVisible = true;
                        }
                        else
                        {
                            lbMonedero.IsVisible = true;
                            lbEnvios.IsVisible = false;
                        }
                    }
                });
                return true; // runs again, or false to stop
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (masvendidosViewModel.Productos.Count == 0)
            {
                masvendidosViewModel.LoadProductosCommand.Execute(null);
            }
            masvendidosViewModel.noProductos = App.Cart;
            masvendidosViewModel.Monedero = App.Monedero;
            masvendidosViewModel.EnviosGratis = App.EnvioGratis;
            masvendidosViewModel.Abandonados = App.Abandonados;
            masvendidosViewModel.TituloEnviosGratis = App.TituloEnvioGratis;
            masvendidosViewModel.TituloMonedero = App.TituloMonedero;
            if (App.Abandonados == 1)
            {
                masvendidosViewModel.visibleAbandonado = true;
            }
            else
            {
                masvendidosViewModel.visibleAbandonado = false;
            }
            if (root)
            {
                Navigation.PopToRootAsync();
                root = false;
            }
            App.eventTracker.SendScreen(strEvento, nameof(MasVendidos));
        }
    }
}
