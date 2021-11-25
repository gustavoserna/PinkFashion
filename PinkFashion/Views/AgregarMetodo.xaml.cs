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

namespace PinkFashion.Views
{
    public partial class AgregarMetodo : ContentPage
    {
        string tipo = "";
        string strEvento = "Método Pago|Pink Fashion Store";
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


        public async Task InsertarMetodo(Card metodo)
        {
            try
            {
                var item = JsonConvert.SerializeObject(metodo);
                System.Diagnostics.Debug.WriteLine(item);
                var client = new HttpClient();
                StringContent str = new StringContent("op=InsertaTarjetaCliente&item=" + item, Encoding.UTF8, "application/x-www-form-urlencoded");
                await client.PostAsync(Constantes.url + "Sesion/App.php", str);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error al registrar los datos, inténtalo de nuevo mas tarde", "Ok");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }

        public async Task Agregar()
        {
            string token = "";
            string[] exp = expira.Text.Split('/');
            int mes = Convert.ToInt32(exp[0]);
            int año = Convert.ToInt32(exp[1]);
            string cuenta = string.Concat(tarjeta.Text.Where(c => !char.IsWhiteSpace(c)));
            System.Diagnostics.Debug.WriteLine(cuenta);

            // Nuevas Llaves Coneckta 2020 
            //token = await new ConektaTokenizer("key_EkzSgKsDjq3oksxqBwGgnMA", RuntimePlatform.iOS).GetTokenAsync(cuenta, nombre.Text, cvc.Text, año, mes);  //Test
            token = await new ConektaTokenizer("key_a4KDtxki6vWpz4uXMx21U4w", RuntimePlatform.iOS).GetTokenAsync(cuenta, nombre.Text, cvc.Text, año, mes);      //Produccion


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
            metodoPago.Token = token;

            try
            {
                await InsertarMetodo(metodoPago);
                if (tipo.Equals("push"))
                    await Navigation.PopAsync();
                else
                    await Navigation.PopModalAsync();

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error al registrar los datos, inténtalo de nuevo mas tarde", "Ok");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
