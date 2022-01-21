using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PinkFashion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecuperarContrasena : ContentPage
    {
        string strEvento = "FAQ|Pink Fashion Store";
        public RecuperarContrasena()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.eventTracker.SendScreen(strEvento, nameof(FAQ));
            webViewElement.Source = "https://pinkfashionstore.com/recuperar.php";
            webViewElement.RegisterAction(DisplayDataFromJavascript);
        }

        private void DisplayDataFromJavascript(string data)
        {
            //
        }
    }
}