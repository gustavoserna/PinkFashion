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
        json_object json_ob = new json_object();
        public Command GetBloques { get; set; }
        public Command GetContador { get; set; }
        uint scrollRate = 1000; // in milliseconds

        ObservableCollection<Layoutapp>  BloquesLayoutApp = new ObservableCollection<Layoutapp>();
        public Inicio()
        {
            InitializeComponent();
            
            BindingContext = inicioViewModel = new InicioViewModel(Navigation);

            if (Application.Current.Properties.ContainsKey("Abreviado"))
            {
                //lbNombre.Text = "Hola " + Application.Current.Properties["Abreviado"].ToString().Trim();
                StackName.IsVisible = true;
            }
            else
            {
                StackName.IsVisible = false;
            }
            var clickCuenta = new TapGestureRecognizer();
            clickCuenta.Tapped += async (s, e) =>
            {

                        var page = new NavigationPage(new Cuenta());
                        page.BarBackgroundColor = App.bgColor;
                        page.BarTextColor = App.textColor;
                        await Navigation.PushModalAsync(page);

                
            };
            cuenta.GestureRecognizers.Add(clickCuenta);
            
            var clickNovedades = new TapGestureRecognizer();
            clickNovedades.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Novedades());
            };
            btnNovedades.GestureRecognizers.Add(clickNovedades);

            var clickOfertas = new TapGestureRecognizer();
            clickOfertas.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Ofertas());
            };
            btnOfertas.GestureRecognizers.Add(clickOfertas);

            var clickVendidos = new TapGestureRecognizer();
            clickVendidos.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new MasVendidos());

            };
            btnVendidos.GestureRecognizers.Add(clickVendidos);
            
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
            
            GetContador = new Command(async () =>
            {
                await ExecuteGetContadorCommand();
            });
            
            
            GetBloques = new Command(async () =>
            {
                await ExecuteGetBloquesCommand(); 

            });

            var clickAbandonados = new TapGestureRecognizer();
            clickAbandonados.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new CarritosAbandonados());

            };
            lbAbandonado.GestureRecognizers.Add(clickAbandonados);

            Device.StartTimer(new TimeSpan(0, 0, 5), () =>
            {
                // do something every 30 seconds
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (StackAbandonado.IsVisible == true)
                    {
                        if (lbEnvios.IsVisible == true)
                        {
                            lbEnvios.IsVisible = false;
                            lbAbandonado.IsVisible = true;
                        }
                        else 
                        {
                            lbEnvios.IsVisible = true;
                            lbAbandonado.IsVisible = false;
                        }
                    }
                    else
                    {
                        lbAbandonado.IsVisible = false;
                        if (lbEnvios.IsVisible == true)
                        {
                            lbEnvios.IsVisible = false;
                        }
                        else
                        {
                            lbEnvios.IsVisible = true;
                        }
                    }
                });
                return true; // runs again, or false to stop
            });


            Device.StartTimer(TimeSpan.FromSeconds(5), HandleTimer);
        }

        bool HandleTimer()
        {
            double offset = scrollView.ScrollX >= (inicioViewModel.InicioVista.Banners.Count - 1) * this.Width ? 0 : scrollView.ScrollX + this.Width;
            var animation = new Animation(x => scrollView.ScrollToAsync(x, scrollView.ScrollY, false), scrollView.ScrollX, offset);
            animation.Commit(this, "Scroll", length: scrollRate);

            return true;
        }

        async Task ExecuteGetBloquesCommand()
        {

            IEnumerable<Layoutapp> layoutapp = null;
            List<Layoutapp> listalayout = new List<Layoutapp>();


            await GetInicio().ContinueWith(t =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    for (int i = 0; i < t.Result.LayoutApp.Count; i++)
                    {
                        listalayout.Add(t.Result.LayoutApp[i]);
                    }

                }
            });

            layoutapp = listalayout;

            int CountRow = 0;
            foreach (var itemlay in layoutapp)
            {
                var navImage1 = new Image
                {
                    Source = itemlay.Imagen_marca1,
                    BackgroundColor = Color.White,
                    HeightRequest = 85,
                    /*HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,*/
                    Aspect = Aspect.AspectFill

                };

                Grid GridImage1 = new Grid
                {
                    Margin = new Thickness(0, 0, 0, 0),
                    //VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                    
                };
                GridImage1.Children.Add(navImage1);


                var navImage2 = new Image
                {
                    Source = itemlay.Imagen_marca2,
                    BackgroundColor = Color.White,
                    HeightRequest = 85,
                    /*HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,*/
                    Aspect = Aspect.AspectFill

                };

                Grid GridImage2 = new Grid
                {
                    Margin = new Thickness(0, 0, 0, 0),
                    //VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                    

                };
                GridImage2.Children.Add(navImage2);

                if (itemlay.Bloque == "2")
                {
                    GridMarcas.Children.Add(GridImage1, 0, CountRow);
                    GridMarcas.Children.Add(GridImage2, 1, CountRow);
                    var clickMarca1 = new TapGestureRecognizer();
                    clickMarca1.Tapped += (s, e) =>
                    {
                        Navigation.PushAsync(new ProductosMarca(itemlay.IdMarca1, itemlay.Marca1));

                    };
                    navImage1.GestureRecognizers.Add(clickMarca1);
                    var clickMarca2 = new TapGestureRecognizer();
                    clickMarca2.Tapped += (s, e) =>
                    {
                        Navigation.PushAsync(new ProductosMarca(itemlay.IdMarca2, itemlay.Marca2));

                    };
                    navImage2.GestureRecognizers.Add(clickMarca2);
                }
                else
                {
                    GridMarcas.Children.Add(GridImage1, 0, CountRow);
                    Grid.SetColumnSpan(GridImage1, 2);
                    var clickMarca1 = new TapGestureRecognizer();
                    clickMarca1.Tapped += (s, e) =>
                    {
                        Navigation.PushAsync(new ProductosMarca(itemlay.IdMarca1, itemlay.Marca1));

                    };
                    navImage1.GestureRecognizers.Add(clickMarca1);
                }

                CountRow++;
            }

            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            inicioViewModel.LoadInicioCommand.Execute(null);
            this.GetBloques.Execute(null);
            inicioViewModel.noProductos = App.Cart;
            inicioViewModel.Monedero = App.Monedero;
            inicioViewModel.EnviosGratis = App.EnvioGratis;
            inicioViewModel.Abandonados = App.Abandonados;
            App.eventTracker.SendScreen("Inicio|PinkFashionStore", nameof(Inicio));

        }

        public async Task<InicioModel> GetInicio()
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=ObtenerInicio", Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Productos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("inicio: " + json);
                if (json != "")
                {
                    json_ob = JsonConvert.DeserializeObject<json_object>(json);
                }
                else
                {
                    return json_ob.iniciomodel[0] = null;
                }


                //Obtiene datos de variables iniciales
                if (Application.Current.Properties.ContainsKey("IdCliente"))
                {
                    var client2 = new HttpClient();
                    StringContent str2 = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                    var respuesta2 = await client2.PostAsync(Constantes.url + "Pedidos/App.php", str2);
                    var json2 = respuesta2.Content.ReadAsStringAsync().Result.Trim();

                    var obj = JObject.Parse(json2);
                    string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                    string displayMonedero = (string)obj.SelectToken("Monedero");
                    string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                    App.Monedero = displayMonedero;
                    App.EnvioGratis = displayEnvios;
                    inicioViewModel.EnviosGratis = App.EnvioGratis;
                    inicioViewModel.Monedero = App.Monedero;
                    if (!string.IsNullOrEmpty(displayNoProductos))
                    {
                        App.Cart = Int32.Parse(displayNoProductos);
                        inicioViewModel.noProductos = App.Cart;
                    }
                    else
                    {
                        App.Cart = 0;
                        inicioViewModel.noProductos = App.Cart;
                    }
                }



                return json_ob.iniciomodel[0];

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_ob.iniciomodel[0];

        }

        public class json_object
        {
            [JsonProperty("EntidadInicio")]
            public InicioModel[] iniciomodel { get; set; }

        }


        async Task ExecuteGetContadorCommand()
        {
            await Contador();
        }

        public async Task Contador()
        {
            try
            {
                var client = new HttpClient(new HttpClientHandler
                {
                    UseProxy = false
                });
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Keep-Alive", "600");
                StringContent str = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();

                var obj = JObject.Parse(json);
                string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                string displayMonedero = (string)obj.SelectToken("Monedero");
                string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                App.Monedero = displayMonedero;
                App.EnvioGratis = displayEnvios;
                inicioViewModel.EnviosGratis = App.EnvioGratis;
                inicioViewModel.Monedero = App.Monedero;
                if (!string.IsNullOrEmpty(displayNoProductos))
                {
                    App.Cart = Int32.Parse(displayNoProductos);
                    inicioViewModel.noProductos = App.Cart;
                }
                else
                {
                    App.Cart = 0;
                    inicioViewModel.noProductos = App.Cart;
                }
                /*
                if(!string.IsNullOrEmpty(displayEnvios))
                {
                    App.EnvioGratis = double.Parse(displayEnvios);
                }
                else
                {
                    App.EnvioGratis = 0;
                }*/

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error:" + ex.Message);
            }

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
