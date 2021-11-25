using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class VerificarTelefono : ContentPage
    {
        public VerificarTelefono()
        {
            InitializeComponent();
            Title = "Verificación de teléfono";
            string strEvento = "Verifica Telefono|Pink Fashion Store";
            App.eventTracker.SendScreen(strEvento, nameof(VerificarTelefono));
            NavigationPage.SetHasBackButton(this, false);

            var clickEnviarSMS = new TapGestureRecognizer();
            clickEnviarSMS.Tapped += async (s, e) =>
            {
                if (!numero.Text.Equals(""))
                {
                    contNumero.IsVisible = false;
                    contNumero.IsVisible = false;
                    loader.IsVisible = true;

                    string codigoString = RandomString(4);
                    Application.Current.Properties["codigo"] = codigoString;
                    await Application.Current.SavePropertiesAsync();
                    await EnviarSMS(codigoString);

                    contNumero.IsVisible = false;
                    contCodigo.IsVisible = true;
                    loader.IsVisible = false;
                }
                else if (numero.Text.Length != 10)
                {
                    await DisplayAlert("Error", "El número debe ser de 10 dígitos", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", "Por favor proporciona un número", "Ok");
                }
            };
            btnNumero.GestureRecognizers.Add(clickEnviarSMS);
            btnCodigo2.GestureRecognizers.Add(clickEnviarSMS);

            var clickValidarCodigo = new TapGestureRecognizer();
            clickValidarCodigo.Tapped += async (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    contNumero.IsVisible = false;
                    contCodigo.IsVisible = false;
                    loader.IsVisible = true;
                });

                if (codigo.Text.Equals(Application.Current.Properties["codigo"]))
                {
                    await BuscarNumero().ContinueWith(t =>
                    {
                        System.Diagnostics.Debug.WriteLine("estatus t: " + t.Status);
                        if (t.Status == TaskStatus.RanToCompletion)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (t.Result.Equals("existe"))
                                {
                                    await DisplayAlert("Error", "Número de teléfono ya en uso", "Ok");
                                    codigo.Text = "";
                                    numero.Text = "";
                                    contCodigo.IsVisible = false;
                                    contNumero.IsVisible = true;
                                    loader.IsVisible = false;
                                }
                                else
                                {
                                    //hola
                                    System.Diagnostics.Debug.WriteLine("Entró");
                                    await InsertarNumero();
                                    Application.Current.Properties["NIT"] = numero.Text;
                                    App.Current.Properties["Encontrado"] = "Verificado";
                                    await Application.Current.SavePropertiesAsync();

                                    Application.Current.MainPage = new NavigationPage(new Inicio())
                                    {
                                        BarBackgroundColor = App.bgColor,
                                        BarTextColor = App.textColor
                                    };

                                }
                            });
                        }
                    });
                }
                else
                {
                    await DisplayAlert("Error", "Código incorrecto", "Ok");
                    codigo.Text = "";
                    loader.IsVisible = false;
                    contCodigo.IsVisible = true;
                }
            };
            btnCodigo.GestureRecognizers.Add(clickValidarCodigo);

            var clickLogin = new TapGestureRecognizer();
            clickLogin.Tapped += (s, e) =>
            {
                Application.Current.MainPage = new NavigationPage(new Login())
                {
                    BarBackgroundColor = App.bgColor,
                    BarTextColor = App.textColor
                };
            };
            btnLogin.GestureRecognizers.Add(clickLogin);
        }

        public async Task EnviarSMS(string codigo)
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=EnviarSMS&pNumero=" + numero.Text + "&pCodigo=" + codigo, Encoding.UTF8, "application/x-www-form-urlencoded");
                await client.PostAsync(Constantes.url + "Sesion/App.php", str);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public async Task<string> BuscarNumero()
        {
            var client = new HttpClient();
            StringContent str = new StringContent("op=BuscarTelefono&pTelefono=" + numero.Text, Encoding.UTF8, "application/x-www-form-urlencoded");
            var respuesta = await client.PostAsync(Constantes.url + "Sesion/App.php", str);
            var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
            System.Diagnostics.Debug.WriteLine("respuesta de telefono: " + json);
            return json;
        }

        public async Task InsertarNumero()
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=verificarNum&Numero=" + numero.Text + "&IDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                await client.PostAsync(Constantes.url + "Sesion/App.php", str);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
