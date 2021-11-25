using System;
using PinkFashion.ViewModels;
using PinkFashion.Models;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class Novedades : ContentPage
    {
        NovedadesViewModel novedadesViewModel;
        string strEvento = "Novedades|Pink Fashion Store";
        public Novedades()
        {
            InitializeComponent();
                        
            BindingContext = novedadesViewModel = new NovedadesViewModel();
            buscar.Placeholder = "Buscar un producto";
            
            if (Application.Current.Properties.ContainsKey("Abreviado"))
            {
                //lbNombre.Text = "Hola " + Application.Current.Properties["Abreviado"].ToString();
                StackName.IsVisible = true;
            }
            else
            {
                StackName.IsVisible = false;
            }
            
            listview.ItemTapped += (s, e) =>
            {
                var item = e.Item as Producto_;
                if (item.ConVariante > 0)
                {
                    Navigation.PushAsync(new Producto(item));
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
                novedadesViewModel.Buscar(buscar.Text);
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
            if (novedadesViewModel.Productos.Count == 0)
            {
                novedadesViewModel.LoadProductosCommand.Execute(null);
            }
            novedadesViewModel.noProductos = App.Cart;
            novedadesViewModel.Monedero = App.Monedero;
            novedadesViewModel.EnviosGratis = App.EnvioGratis;
            novedadesViewModel.Abandonados = App.Abandonados;
            novedadesViewModel.TituloEnviosGratis = App.TituloEnvioGratis;
            novedadesViewModel.TituloMonedero = App.TituloMonedero;
            if (App.Abandonados == 1)
            {
                novedadesViewModel.visibleAbandonado = true;
            }
            else
            {
                novedadesViewModel.visibleAbandonado = false;
            }
            App.eventTracker.SendScreen(strEvento, nameof(Novedades));
        }
    }
}
