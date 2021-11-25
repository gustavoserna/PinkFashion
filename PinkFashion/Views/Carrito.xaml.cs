using System;
using System.Threading;
using System.Threading.Tasks;
using PinkFashion.ViewModels;
using Xamarin.Forms;
using PinkFashion.Models;

namespace PinkFashion.Views
{
    public partial class Carrito : ContentPage
    {
        CarritoViewModel carritoViewModel;
        public static bool refresco = false;
        public static bool root = false;
        public static string tmpDireccion = string.Empty;
        public static string TipoPago = "NA";
        public static string SelTarjeta = "NA";
        Direccion DirAux = null;
        Card CardAux = null;
        string strEvento = "Carrito|Pink Fashion Store";
        public Carrito()
        {
            InitializeComponent();
            Title = "Carrito";
            lblTitulo.TextColor = App.textColor;
            lblCerrar.TextColor = App.textColor;

            if (Application.Current.Properties.ContainsKey("Abreviado"))
            {
                //lbNombre.Text = "Hola " + Application.Current.Properties["Abreviado"].ToString();
                StackName.IsVisible = true;
            }
            else
            {
                StackName.IsVisible = false;
            }


            BindingContext = carritoViewModel = new CarritoViewModel(this);
            var clickCerrar = new TapGestureRecognizer();
            clickCerrar.Tapped += (s, e) =>
            {
                Navigation.PopModalAsync();
            };
            cerrar.GestureRecognizers.Add(clickCerrar);

            var clickDireccion = new TapGestureRecognizer();
            clickDireccion.Tapped += (s, e) =>
            {
                var page = new NavigationPage(new AgregarDireccion("modal"));
                page.BarBackgroundColor = App.bgColor;
                page.BarTextColor = App.textColor;
                Navigation.PushModalAsync(page);
            };
            btnAgregarDireccion.GestureRecognizers.Add(clickDireccion);

            var clickAgregarTarjeta = new TapGestureRecognizer();
            clickAgregarTarjeta.Tapped += (s, e) =>
            {
                var page = new NavigationPage(new AgregarMetodo("modal"));
                page.BarBackgroundColor = App.bgColor;
                page.BarTextColor = App.textColor;
                Navigation.PushModalAsync(page);
            };
            btnAgregarTarjeta.GestureRecognizers.Add(clickAgregarTarjeta);
           
            var clickCodigo = new TapGestureRecognizer();
            clickCodigo.Tapped += async (s, e) =>
            {
                if (!vCodigoPromo.Text.Equals(""))
                {
                    string respuesta = await carritoViewModel.AplicaCodigo(vCodigoPromo.Text);
                    if (respuesta == "1")
                    {}else { await DisplayAlert("Precaución", respuesta, "Ok"); }
                }
                else
                {
                    await DisplayAlert("Precaución", "Es necesario introduccir un código", "Ok");
                }
                

            };
            lblConfirmaPromo.GestureRecognizers.Add(clickCodigo);

            var clickMonedero = new TapGestureRecognizer();
            clickMonedero.Tapped += async (s, e) =>
            {
                if (!vMonedero.Text.Equals(""))
                {
                    string respuesta = await carritoViewModel.AplicaMonedero(vMonedero.Text);
                    if (respuesta == "1")
                    { }
                    else { await DisplayAlert("Precaución", respuesta, "Ok"); }
                }
                else
                {
                    await DisplayAlert("Precaución", "Es necesario introduccir un monto", "Ok");
                }


            };
            lblConfirmaMonedero.GestureRecognizers.Add(clickMonedero);
            
            Device.StartTimer(new TimeSpan(0, 0, 5), () =>
            {
                // do something every 30 seconds
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (lbAbandonado.IsVisible == true)
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
        
        async void OnBtnVaciarClicked(object sender, EventArgs args)
        {

            try
            {
                await VaciarCarrito();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayAlert("Error", "Intentalo de nuevo mas tarde", "Ok");

            }

        }

        async void OnBtnRegresarClicked(object sender, EventArgs args)
        {

            try
            {
                Application.Current.MainPage = new NavigationPage(new Inicio())
                {
                    BarBackgroundColor = App.bgColor,
                    BarTextColor = App.textColor
                };

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayAlert("Error", "Inténtalo de nuevo mas tarde", "Ok");

            }

        }

        async void OnBtnConfirmarClicked(object sender, EventArgs args)
        {

            try
            {
                if (!carritoViewModel.noencontrado)
                {
                    if (Application.Current.Properties.ContainsKey("IDDireccion"))
                    {
                        if (Application.Current.Properties["IDDireccion"].Equals(""))   //(lbDireccion.Text == "Selecciona tu dirección")
                        {
                            await DisplayAlert("Precaución", "Debes seleccionar una dirección de envío", "Ok");
                        }
                        else
                        {
                            string vCliente = Application.Current.Properties["IdCliente"].ToString();
                            string vDireccion = Application.Current.Properties["IDDireccion"].ToString();
                            string vFpago = TipoPago;
                            string vEnvio = "0";
                            if (App.EnvioGratis == "0")
                            {
                                vEnvio = "0";
                            }
                            else
                            {
                                vEnvio = Application.Current.Properties["CostoEnvio"].ToString();
                            }
                            
                            string Meses = "1";
                            string vToken = "";
                            if (TipoPago == "TC" || TipoPago == "E" || TipoPago == "Paypal")
                            {

                                if (TipoPago == "TC") //&& Meses3.IsChecked == true)
                                {
                                    if (Application.Current.Properties.ContainsKey("TokenTarjeta"))
                                    {
                                        if (Application.Current.Properties["TokenTarjeta"].Equals(""))  
                                        {
                                            await DisplayAlert("Precaución", "Debes seleccionar una tarjeta", "Ok");
                                        }
                                        else
                                        {
                                            vToken = Application.Current.Properties["TokenTarjeta"].ToString();
                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                cargando.IsVisible = true;
                                            });
                                            NavigationPage.SetHasNavigationBar(this, false);
                                            //string respuesta = "Test";
                                            string respuesta = await carritoViewModel.ConfirmaCompra(vCliente, vDireccion, vFpago, vEnvio, vToken, Meses);
                                            await DisplayAlert("Información", respuesta, "Ok");
                                            /*Application.Current.MainPage = new NavigationPage(new Inicio())
                                            {
                                                BarBackgroundColor = App.bgColor,
                                                BarTextColor = App.textColor
                                            };
                                            */
                                            
                                            await Application.Current.MainPage.Navigation.PopModalAsync();

                                            await Application.Current.MainPage.Navigation.PushAsync(new MisPedidos());


                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                cargando.IsVisible = false;
                                            });
                                            NavigationPage.SetHasNavigationBar(this, true);
                                        }
                                    }
                                }
                                if (TipoPago == "E")
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        cargando.IsVisible = true;
                                    });
                                    NavigationPage.SetHasNavigationBar(this, false);

                                    string respuesta = await carritoViewModel.ConfirmaCompra(vCliente, vDireccion, vFpago, vEnvio, vToken, Meses);
                                    await DisplayAlert("Información", respuesta, "Ok");
                                    /*Application.Current.MainPage = new NavigationPage(new Inicio())
                                    {
                                        BarBackgroundColor = App.bgColor,
                                        BarTextColor = App.textColor
                                    };
                                    */
                                    await Application.Current.MainPage.Navigation.PopModalAsync();

                                    await Application.Current.MainPage.Navigation.PushAsync(new MisPedidos());

                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        cargando.IsVisible = false;
                                    });
                                    NavigationPage.SetHasNavigationBar(this, true);
                                }
                                if (TipoPago == "Paypal")
                                {
                                    double vtotal = Convert.ToDouble(lblTotal.Text.Substring(1));
                                    await Navigation.PushAsync(new PayPal(vCliente, vDireccion, vtotal.ToString(), vEnvio));
                                    await carritoViewModel.InicializaCarrito();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Precaución", "Debes seleccionar un método de pago", "Ok");
                            };
                        }
                    }
                    else
                    {
                        await DisplayAlert("Precaución", "Debes seleccionar una dirección de envío", "Ok");
                    }

                }
                else
                {
                    await DisplayAlert("Vacío", "No hay artículos en tu carrito", "Ok");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayAlert("Error", "Intentalo de nuevo mas tarde", "Ok");
            }

        }

        public bool quitarSelDireccion = false;
        public async Task QuitarSelDireccionAsync()
        {
            if (Application.Current.Properties.ContainsKey("IDDireccion"))
            {
                if (!Application.Current.Properties["IDDireccion"].Equals(""))
                {
                    lbDireccion.Text = Application.Current.Properties["Direccion"].ToString();
                    await carritoViewModel.CalcularCostoEnvio();
                    
                }
            }
            quitarSelDireccion = true;
            
        }

        public void pushDirecciones()
        {
            var page = new NavigationPage(new SeleccionaDireccion(this));
            page.BarBackgroundColor = App.bgColor;
            page.BarTextColor = App.textColor;
            Navigation.PushModalAsync(page);
        }

        public bool quitarSelPago = false;
        public void QuitarSelPago()
        {
            if (Application.Current.Properties.ContainsKey("IDMetodoPago"))
            {
                if (!Application.Current.Properties["IDMetodoPago"].Equals(""))
                {
                    //lbPago.Text = Application.Current.Properties["MetodoPago"].ToString();
                }
            }
            quitarSelPago = true;


        }
        
        public async Task VaciarCarrito()
        {
            bool ac = await DisplayAlert("¿Deses vacíar el carrito?", "Confirmar", "Sí", "No");
            if (ac)
            {
                //vaciar carrito
                await carritoViewModel.DeletePedidoTemporal();
                carritoViewModel.LoadItemsCommand.Execute(null);
                //carrito
                
                Application.Current.Properties["CostoEnvio"] = 0;
                await Application.Current.SavePropertiesAsync();
                await DisplayAlert("Listo", "Carrito vacío. Intenta agregar de nuevo el artículo deseado", "Ok");
                Thread.Sleep(2000);
                Application.Current.MainPage = new NavigationPage(new Inicio())
                {
                    BarBackgroundColor = App.bgColor,
                    BarTextColor = App.textColor
                };
            }

        }

        async void OnPagoCheckedChanged(object sender, EventArgs args)
        {

            try
            {
                RadioButton button = sender as RadioButton;
                SelPago.Text = $"Tu forma de pago es : {button.Content}";

                if (button.Content.ToString() == "Tarjeta")
                {
                    MostrarTarjeta.IsVisible = true;
                    TipoPago = "TC";
                    if (Application.Current.Properties.ContainsKey("TokenTarjeta"))
                    {
                        SelTarjeta = Application.Current.Properties["TokenTarjeta"].ToString();
                    }
                    else
                    {
                        SelTarjeta = "";
                    }
                }
                if (button.Content.ToString() == "OXXO PAY")
                {
                    MostrarTarjeta.IsVisible = false;
                    TipoPago = "E";
                    SelTarjeta = "";
                }
                if (button.Content.ToString() == "PAYPAL")
                {
                    MostrarTarjeta.IsVisible = false;
                    TipoPago = "Paypal";
                    SelTarjeta = "";
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayAlert("Error", "Intentalo de nuevo mas tarde", "Ok");

            }

        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.eventTracker.SendScreen(strEvento, nameof(Carrito));
            carritoViewModel.LoadItemsCommand.Execute(null);
            carritoViewModel.noProductos = App.Cart;
            carritoViewModel.Monedero = App.Monedero;
            carritoViewModel.EnviosGratis = App.EnvioGratis;
            carritoViewModel.Abandonados = App.Abandonados;
            carritoViewModel.TituloEnviosGratis = App.TituloEnvioGratis;
            carritoViewModel.TituloMonedero = App.TituloMonedero;
            if (App.Abandonados == 1)
            {
                carritoViewModel.visibleAbandonado = true;
            }
            else
            {
                carritoViewModel.visibleAbandonado = false;
            }

            if (root)
            {
                Producto.root = true;
                Navigation.PopModalAsync();
                root = false;
            }

            
        }

        
    }
}
