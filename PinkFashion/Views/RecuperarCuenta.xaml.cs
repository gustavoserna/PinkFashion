using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
//using Plugin.FirebaseAnalytics;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class RecuperarCuenta : ContentPage
    {
        string strEvento = "Recuperar Cuenta|Pink Fashion Store";
        public RecuperarCuenta()
        {
            InitializeComponent();
            Title = "Recuperar cuenta";
            App.eventTracker.SendScreen(strEvento, nameof(RecuperarCuenta));
            var clickEnviarMail = new TapGestureRecognizer();
            clickEnviarMail.Tapped += async (s, e) =>
            {
                if (!mail.Text.Equals(""))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        loader.IsVisible = true;
                    });


                    /*}
                    else
                    {
                        await DisplayAlert("Error", "Número de teléfono no registrado, verificar", "Ok");
                    }*/


                    await EnviarMail().ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (!t.Result.Equals("inexistente"))
                                {
                                    await DisplayAlert("Información", "se envío la información, verificar", "Ok");
                                    loader.IsVisible = false;
                                }
                                else
                                {
                                    await DisplayAlert("Error", "Correo no registrado, verificar", "Ok");
                                    loader.IsVisible = false;
                                }
                            });
                        }
                    });

                }
                else
                {
                    await DisplayAlert("Error", "Por favor proporciona un correo", "Ok");
                }
            };
            btnNumero.GestureRecognizers.Add(clickEnviarMail);

        }


        async void OnBtnRecuperarClicked(object sender, EventArgs args)
        {
            try
            {
                if (!mail.Text.Equals(""))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        loader.IsVisible = true;
                    });

                    await EnviarMail().ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (!t.Result.Equals("0"))
                                {
                                    await DisplayAlert("Información", "se envío la información, verificar", "Ok");
                                    loader.IsVisible = false;
                                }
                                else
                                {
                                    await DisplayAlert("Error", "Correo no registrado, verificar", "Ok");
                                    loader.IsVisible = false;
                                }
                            });
                        }
                    });
                }
                else
                {
                    await DisplayAlert("Error", "Por favor proporciona un correo", "Ok");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayAlert("Error", "Intentalo de nuevo mas tarde", "Ok");

            }

        }

        async void OnBtnCerrarClicked(object sender, EventArgs args)
        {
            try
            {
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayAlert("Error", "Intentalo de nuevo mas tarde", "Ok");

            }

        }

        public async Task<string> EnviarMail()
        {

            try
            {

                var client = new HttpClient();
                StringContent str = new StringContent("op=recuperar&username=" + mail.Text, Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                //var regresa = JsonConvert.DeserializeObject<string>(json);
                var regresa = json;
                return regresa;

            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.Message);
                return "0";
            }

        }

        /*
        public async Task<string> BuscarNumero()
        {
            var client = new HttpClient();
            StringContent str = new StringContent("op=BuscarTelefono&pTelefono=" + numero.Text, Encoding.UTF8, "application/x-www-form-urlencoded");
            var respuesta = await client.PostAsync(Constantes.url + "Usuario/App.php", str);
            var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
            string regresa = JsonConvert.DeserializeObject<string>(json);
            return regresa;
        }
        */

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
