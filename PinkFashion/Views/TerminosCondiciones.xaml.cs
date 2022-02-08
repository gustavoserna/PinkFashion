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
    public partial class TerminosCondiciones : ContentPage
    {
        string strEvento = "Términos y condiciones|Pink Fashion Store";
        public TerminosCondiciones()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.eventTracker.SendScreen(strEvento, nameof(FAQ));
            webViewElement.Source = "https://pink.resosistemas.mx/TerminosApp.php";

            webViewElement.RegisterAction(DisplayDataFromJavascript);

        }

        private void DisplayDataFromJavascript(string data)
        {
            //
        }
    }
}