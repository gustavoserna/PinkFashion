using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using PinkFashion.ViewModels;
using Xamarin.Essentials;
using Xamarin.Auth;
using PinkFashion.AuthHelpers;
using System.Diagnostics;
using System.Linq;

namespace PinkFashion.Views
{
    public partial class Login : ContentPage
    {
        SM sM = new SM();
        string pushTokenString = "";
        Account account;
        AccountStore store;
        string strEvento = "Login | Pink Fashion Store";
        public Login()
        {
            InitializeComponent();


            GetPushToken();

            App.eventTracker.SendScreen(strEvento, nameof(Login));
            store = AccountStore.Create();
            NavigationPage.SetHasNavigationBar(this, false);

            if (Device.RuntimePlatform == Device.Android)
            {
                usuario.TextColor = Color.Black;
                clave.TextColor = Color.Black;
                usuario.PlaceholderColor = Color.Gray;
                clave.PlaceholderColor = Color.Gray;
            }

            var clickIniciarSesion = new TapGestureRecognizer();
            clickIniciarSesion.Tapped += async (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //datos.IsVisible = false;
                    loader.IsVisible = true;
                });

                await sM.iniciarSesion(usuario.Text, clave.Text, Navigation);

                Device.BeginInvokeOnMainThread(() =>
                {
                    //datos.IsVisible = true;
                    loader.IsVisible = false;
                });


                try
                {
                    await SendToken();
                }
                catch (Exception)
                {//
                }
            };
            btnLogin.GestureRecognizers.Add(clickIniciarSesion);

            var clickCrear = new TapGestureRecognizer();
            clickCrear.Tapped += (s, e) =>
            {
                Navigation.PushModalAsync(
                    new NavigationPage(new Registro())
                    {
                        BarBackgroundColor = App.bgColor,
                        BarTextColor = App.textColor
                    }
             );

            };
            btnCrear.GestureRecognizers.Add(clickCrear);

            var clickRecuperar = new TapGestureRecognizer();
            clickRecuperar.Tapped += async (s, e) =>
            {
                //await Navigation.PushAsync(new NavigationPage(new RecuperarContrasena()));
                //await Launcher.OpenAsync("https://pinkfashionstore.com/recuperar.php");
                await Navigation.PushModalAsync(new RecuperarContrasena());
            };
            btnRecupera.GestureRecognizers.Add(clickRecuperar);

            var clickMostrarPass = new TapGestureRecognizer();
            clickMostrarPass.Tapped += (s, e) =>
            {
                if (clave.IsPassword == true)
                {
                    clave.IsPassword = false;
                    btnMostrarPass.Source = "visible.png";
                    

                }
                else 
                {
                    clave.IsPassword = true;
                    btnMostrarPass.Source = "novisible.png";

                }
            };
            btnMostrarPass.GestureRecognizers.Add(clickMostrarPass);
            /*
            var clickClose = new TapGestureRecognizer();
            clickClose.Tapped += (s, e) =>
            {
                Navigation.PopAsync();
            };
            CloseBack.GestureRecognizers.Add(clickClose);
            */
        }


        void GetPushToken()
        {
            //OneSignal.Current.IdsAvailable(IdsAvailable);
        }

        private void IdsAvailable(string userID, string pushToken)
        {
            //print("UserID:" + userID);
            System.Diagnostics.Debug.WriteLine("userid:" + userID);
            pushTokenString = userID;
        }

        public async Task SendToken()
        {
            System.Diagnostics.Debug.WriteLine("cliente: " + Application.Current.Properties["IdCliente"]);
            var client = new HttpClient();
            StringContent str = new StringContent("op=InsertTokenCliente&IDCliente=" + Application.Current.Properties["IdCliente"] + "&Token=" + pushTokenString, Encoding.UTF8, "application/x-www-form-urlencoded");
            var respuesta = await client.PostAsync(Constantes.url + "Usuario/App.php", str);
            string res = respuesta.Content.ReadAsStringAsync().Result.Trim();
            System.Diagnostics.Debug.WriteLine("respuesta: " + res);
        }

        void btnLoginClicked(System.Object sender, System.EventArgs e)
        {
            string clientId = null;
            string redirectUri = null;
            switch(Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constantes.iOSClientId;
                    redirectUri = Constantes.iOSRedirectUrl;
                    break;
                case Device.Android:
                    clientId = Constantes.AndroidClientId;
                    redirectUri = Constantes.AndroidRedirectUrl;
                    break;

            }

            account = store.FindAccountsForService(Constantes.AppName).FirstOrDefault();
            var authenticator = new OAuth2Authenticator(
                clientId, null, Constantes.Scope, new Uri(Constantes.AuthorizeUrl), new Uri(redirectUri), new Uri(Constantes.AccessTokenUrl), null, true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);

        }

        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            User user = null;
            if (e.IsAuthenticated)
            {
                //If de user is authenticated, reques thebasic user data from google
                var request = new OAuth2Request("GET", new Uri(Constantes.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response!=null)
                {
                    string userJson = await response.GetResponseTextAsync();
                    user = JsonConvert.DeserializeObject<User>(userJson);
                }

                if (account != null)
                {
                    store.Delete(account, Constantes.AppName);
                }

                await store.SaveAsync(account = e.Account, Constantes.AppName);
                await DisplayAlert("Email ", user.Email, "Ok");

            }

        }

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }
            Debug.WriteLine("Authentication Error" + e.Message);
        }


    }
}
