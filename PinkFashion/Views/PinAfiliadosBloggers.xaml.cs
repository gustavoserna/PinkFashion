using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class PinAfiliadosBloggers : ContentPage
    {
        string strEvento = "PIN Afiliados| Pink Fashion Store";
        private string vCliente;
        public PinAfiliadosBloggers()
        {
            InitializeComponent();
            Title = "Afiliados";
                        
            if (Application.Current.Properties.ContainsKey("IdCliente"))
            {
                vCliente = Convert.ToString(Application.Current.Properties["IdCliente"]);
            }
            else
            {
                vCliente = "";
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            webViewElement.Source = "https://pinkfashionstore.com/app_configpin.php?idcliente=" + vCliente + "";

            webViewElement.RegisterAction(DisplayDataFromJavascript);
            App.eventTracker.SendScreen(strEvento, nameof(PinAfiliadosBloggers));
        }

        private void DisplayDataFromJavascript(string data)
        {
            //
        }
    }
}
