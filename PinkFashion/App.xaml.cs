using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using PinkFashion.Views;
using Sharpnado.Presentation.Forms.RenderedViews;
using Newtonsoft.Json.Linq;
using Com.OneSignal;
using PinkFashion.Renderers;

//using Plugin.FirebaseAnalytics;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PinkFashion
{
    public partial class App : Application
    {
        public static string pushTokenString = string.Empty;
        public static Color bgColor = Color.White;
        public static Color textColor = Color.FromHex("#eb068c");
        public static int Cart = 0;
        public static string EnvioGratis = "";
        public static string Monedero = "";
        public static string TituloEnvioGratis = "";
        public static string TituloMonedero = "";
        public static int Abandonados = 0;
        public Command GetContador { get; set; }

        public static string AppName = "pinkfashionstore.com";

        public static IEventTracker eventTracker;

        public App()
        {
            InitializeComponent();
            eventTracker = DependencyService.Get<IEventTracker>();
            Device.SetFlags(new string[] { "RadioButton_Experimental", "Shapes__Experimental" });
            eventTracker.SendEvent("PinkFashionStore");


            /*
            CrossFirebaseAnalytics.Current.LogEvent(EventName.SelectContent,
                                                        new Parameter(ParameterName.ItemId, 1),
                                                        new Parameter(ParameterName.ItemName, "PinkFashion"));
            */

            Sharpnado.Shades.Initializer.Initialize(false);
            if ((!Application.Current.Properties.ContainsKey("IDDireccion")) || (!Application.Current.Properties.ContainsKey("CostoEnvio")))
            {
                Application.Current.Properties["IDDireccion"] = "";
                Application.Current.Properties["Direccion"] = "Selecciona tu dirección";
                Application.Current.Properties["CostoEnvio"] = "0";
                Application.Current.SavePropertiesAsync();
                
            }


            MainPage = Iniciar();
            OneSignal.Current.StartInit("5e431b45-a0a3-4076-b948-27070889771e").EndInit();

        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            if (uri.Host.EndsWith("pinkfashionstore.com", StringComparison.OrdinalIgnoreCase))
            {

                if (uri.Segments != null && uri.Segments.Length == 3)
                {
                    var action = uri.Segments[1].Replace("/", "");
                    var msg = uri.Segments[2];

                    switch (action)
                    {
                        case "hello":
                            if (!string.IsNullOrEmpty(msg))
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await Current.MainPage.DisplayAlert("hello", msg.Replace("&", " "), "ok");
                                });
                            }

                            break;

                        default:
                            //Device.OpenUri(uri);
                            Launcher.OpenAsync(uri);
                            break;
                    }
                }
            }
        }

        protected override void OnStart()
        {
            //await ExecuteGetContadorCommand();
            

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public NavigationPage Iniciar()
        {
            GetPushToken();
            return new NavigationPage(new Inicio())
            {
                BarBackgroundColor = bgColor,
                BarTextColor = textColor
            };
            
        }

        async Task ExecuteGetContadorCommand()
        {
            await Contador();
        }

        public async Task Contador()
        {
            try
            {

                var client = new HttpClient();
                StringContent str = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();

                var obj = JObject.Parse(json);
                string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                string displayMonedero = (string)obj.SelectToken("Monedero");
                string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                int displayAbandono = (int)obj.SelectToken("Abandonados");
                App.Abandonados = displayAbandono;
                App.Monedero = displayMonedero;
                App.EnvioGratis = displayEnvios;
                if (!string.IsNullOrEmpty(displayNoProductos))
                {
                    App.Cart = Int32.Parse(displayNoProductos);
                }
                else
                {
                    Cart = 0;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error:" + ex.Message);
            }

        }

        void GetPushToken()
        {

            try
            {
                OneSignal.Current.IdsAvailable(IdsAvailable);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void IdsAvailable(string userID, string pushToken)
        {


            try
            {
                System.Diagnostics.Debug.WriteLine("token onesignal: " + userID);
                pushTokenString = userID;
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }


        /*
        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            string appDomain = "https://" + App.AppName.ToLowerInvariant() + "/";
            if (!uri.ToString().ToLowerInvariant().StartsWith(appDomain, StringComparison.Ordinal))
                return;

            string pageUrl = uri.ToString().Replace(appDomain, string.Empty).Trim();
            var parts = pageUrl.Split('?');
            string page = parts[0];
            string pageParameter = parts[1].Replace("id=", string.Empty);

            var formsPage = Activator.CreateInstance(Type.GetType(page));
            var todoItemPage = formsPage as TodoItemPage;
            if (todoItemPage != null)
            {
                var todoItem = await App.Database.GetItemAsync(int.Parse(pageParameter));
                todoItemPage.BindingContext = todoItem;
                await MainPage.Navigation.PushAsync(formsPage as Page);
            }

            base.OnAppLinkRequestReceived(uri);
        }*/
    }
}
