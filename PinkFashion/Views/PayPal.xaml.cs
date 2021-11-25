using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PinkFashion.Models;
//using Plugin.FirebaseAnalytics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PinkFashion.Views
{
    public partial class PayPal : ContentPage
    {
        private string vCliente;
        private string vDireccion;
        private string vTotal;
        private string vEnvio;
        string strEvento = "PayPal|Pink Fashion Store";
        public PayPal(string pCliente, string pDireccion, string pTotal, string pEnvio)
        {
            vCliente = pCliente;
            vDireccion = pDireccion;
            vTotal = pTotal;
            vEnvio = pEnvio;
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            webViewElement.Source = "https://pinkfashionstore.com/app/paypal.php?costofinal=" + vTotal + "&idcliente=" + vCliente + "&id_direccion=" + vDireccion + "&costo_envio=" + vEnvio + "";


            webViewElement.RegisterAction(DisplayDataFromJavascript);
            App.eventTracker.SendScreen(strEvento, nameof(PayPal));
        }

        private void DisplayDataFromJavascript(string data)
        {
            Device.InvokeOnMainThreadAsync(async () =>
            {
                if (data == "1")
                {
                    //await DisplayAlert("MENSAJE", data, "ok");
                    
                    await DisplayAlert("Listo", "Pink Fashion Store agradece mucho tu compra. Tu pedido ha sido confirmado. Cualquier duda al respecto puedes contactarnos a través de nuestras redes sociales, y con gusto te atenderemos.", "Ok");
                    await Application.Current.MainPage.Navigation.PopAsync();

                    await Application.Current.MainPage.Navigation.PushAsync(new MisPedidos());
                    /*
                    Application.Current.MainPage = new NavigationPage(new Inicio())
                    {
                        BarBackgroundColor = App.bgColor,
                        BarTextColor = App.textColor
                    };*/

                }
                else
                {
                    await DisplayAlert("MENSAJE", data, "ok");
                    
                }
            });
        }

    }
}
