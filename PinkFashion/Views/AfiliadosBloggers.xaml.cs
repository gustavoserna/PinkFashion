using System;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class AfiliadosBloggers : ContentPage
    {
        private string vCliente;
        string strEvento = "Beauty Bloguer|Pink Fashion Store";
        public AfiliadosBloggers()
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
            App.eventTracker.SendScreen(strEvento, nameof(AfiliadosBloggers));
            webViewElement.Source = "https://pinkfashionstore.com/app_configBlogger.php?idcliente=" + vCliente + "";

            webViewElement.RegisterAction(DisplayDataFromJavascript);
            
        }

        private void DisplayDataFromJavascript(string data)
        {
            //
        }
    }
}
