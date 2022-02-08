using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Conekta.Xamarin;
using Newtonsoft.Json;
using PinkFashion.ViewModels;
using Xamarin.Forms;
using PinkFashion.Models;
using Openpay.Xamarin;
using Openpay.Xamarin.Abstractions;
using Card = PinkFashion.Models.Card;

namespace PinkFashion.Views
{
    public partial class AgregarMetodo : ContentPage
    {
        string tipo = "";
        string strEvento = "Método Pago|Pink Fashion Store";
        private Token _token;
        private string _deviceSessionId = "";
        public AgregarMetodo(string tipo = "push")
        {
            InitializeComponent();
            this.tipo = tipo;
            this.BindingContext = new PagoViewModel();
            lblTitulo.TextColor = App.textColor;
            lblCerrar.TextColor = App.textColor;
            if (tipo.Equals("push"))
            {
                NavigationPage.SetTitleView(this, null);
                gridHeader.IsVisible = false;
                Title = "Agregar tarjeta";
                /*var listo = new ToolbarItem();
                listo.Text = "Agregar";
                listo.Command = new Command(async o =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        cont.IsVisible = false;
                        loader.IsVisible = true;
                    });
                    NavigationPage.SetHasNavigationBar(this, false);

                    await Agregar();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        cont.IsVisible = true;
                        loader.IsVisible = false;
                    });
                    NavigationPage.SetHasNavigationBar(this, true);
                });
                ToolbarItems.Add(listo);*/
            }
            else
            {
                gridHeader.IsVisible = true;
                var clickCerrar = new TapGestureRecognizer();
                clickCerrar.Tapped += (s, e) =>
                {
                    Navigation.PopModalAsync();
                };
                cerrar.GestureRecognizers.Add(clickCerrar);
                /*
                var clickAgregar = new TapGestureRecognizer();
                clickAgregar.Tapped += async (s, e) =>
                {
                    await Agregar();
                };
                btnAceptar.GestureRecognizers.Add(clickAgregar);*/
            }
        }

        async void OnbtnAgregarClicked(object sender, EventArgs args)
        {

            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    cont.IsVisible = false;
                    loader.IsVisible = true;
                });
                NavigationPage.SetHasNavigationBar(this, false);

                await Agregar();

                Device.BeginInvokeOnMainThread(() =>
                {
                    cont.IsVisible = true;
                    loader.IsVisible = false;
                });
                NavigationPage.SetHasNavigationBar(this, true);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayAlert("Error", "Intentalo de nuevo mas tarde", "Ok");

            }

        }


        public async Task<string> InsertarMetodo(Card metodo)
        {
            var respuesta = "";
            try
            {
                string[] exp = expira.Text.Split('/');
                int mes = Convert.ToInt32(exp[0]);
                int año = Convert.ToInt32(exp[1]);
                int v_cvc = Convert.ToInt32(cvc.Text);

                var item = JsonConvert.SerializeObject(metodo);
                System.Diagnostics.Debug.WriteLine(item);
                var client = new HttpClient();
                StringContent str = new StringContent("" +
                    "op=guardarTarjetaOpenPay" +
                    "&token_id=" + metodo.Token +
                    "&deviceSesion=" + metodo.DeviceSession +
                    "&Smes=" + mes +
                    "&Sano=" + año +
                    "&noTarjeta=" + metodo.Cuenta +
                    "&inputCVC=" + v_cvc +
                    "&idCliente=" + Application.Current.Properties["IdCliente"].ToString()
                    , Encoding.UTF8, "application/x-www-form-urlencoded");
                var consulta = await client.PostAsync(Constantes.open_pay_url + "Sesion/App.php", str);
                respuesta = consulta.Content.ReadAsStringAsync().Result;
                //var res = respuesta; //respuesta.Replace('"', '-');

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error al registrar los datos, inténtalo de nuevo mas tarde", "Ok");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                respuesta = ex.Message;

            }
            return respuesta;

        }

        public async Task Agregar()
        {
            string[] exp = expira.Text.Split('/');
            int mes = Convert.ToInt32(exp[0]);
            int año = Convert.ToInt32(exp[1]);
            string cuenta = string.Concat(tarjeta.Text.Where(c => !char.IsWhiteSpace(c)));

            string Tarjetabiente = nombre.Text;
            int v_cvc = Convert.ToInt32(cvc.Text);
            System.Diagnostics.Debug.WriteLine(cuenta);
            try
            {
                if (CrossOpenpay.IsSupported)
                {
                    Openpay.Xamarin.Abstractions.Card card = new Openpay.Xamarin.Abstractions.Card
                    {
                        HolderName = Tarjetabiente,
                        Number = cuenta,
                        ExpirationMonth = mes.ToString(),
                        ExpirationYear = año.ToString(),
                        Cvv2 = v_cvc
                    };

                    _token = await CrossOpenpay.Current.CreateTokenFromCard(card);
                    _deviceSessionId = await CrossOpenpay.Current.CreateDeviceSessionId();

                    Card metodoPago = new Card();
                    metodoPago.IdTarjeta = 0;
                    metodoPago.IdCliente = Application.Current.Properties["IdCliente"].ToString();
                    if (cuenta.Length == 16)
                    {
                        metodoPago.Cuenta = cuenta[12].ToString() + cuenta[13].ToString() + cuenta[14].ToString() + cuenta[15].ToString();
                    }
                    else
                    {
                        metodoPago.Cuenta = cuenta[11].ToString() + cuenta[12].ToString() + cuenta[13].ToString() + cuenta[14].ToString();
                    }
                    metodoPago.Token = _token.Id.ToString();
                    metodoPago.DeviceSession = _deviceSessionId.ToString();
                    string respuesta = await InsertarMetodo(metodoPago);
                    respuesta = respuesta == null ? "" : respuesta;
                    if (tipo.Equals("push"))
                        await Navigation.PopAsync();
                    else
                        await Navigation.PopModalAsync();

                }
            }
            catch (Exception exception)
            {
                await Application.Current.MainPage.DisplayAlert($"Error: {exception.Message}", "Error", "Ok");
            }
        }
    }
}
