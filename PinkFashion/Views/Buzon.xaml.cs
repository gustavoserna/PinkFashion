using System;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class Buzon : ContentPage
    {
        private string vCliente;
        string strEvento = "Buzón|Pink Fashion Store";
        public Buzon()
        {
            InitializeComponent();
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
            App.eventTracker.SendScreen(strEvento, nameof(Buzon));
            webViewElement.Source = "https://pinkfashionstore.com/app_buzon.php?IdCliente=" + vCliente + "";

            webViewElement.RegisterAction(DisplayDataFromJavascript);
            
        }

        private void DisplayDataFromJavascript(string data)
        {
            //
        }
    }
}
