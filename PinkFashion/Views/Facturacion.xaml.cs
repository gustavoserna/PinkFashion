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
    public partial class Facturacion : ContentPage
    {
        string strEvento = "Facturación | Pink Fashion Store";
        private string vCliente;
        public Facturacion()
        {
            InitializeComponent();
            Title = "Deseas Facturar";

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
            if(!String.IsNullOrEmpty(vCliente))
            {
                webViewElement.Source = String.Format("https://pink.resosistemas.mx/FacturaApp.php?idcliente={0}", vCliente);

                webViewElement.RegisterAction(DisplayDataFromJavascript);
            }
            
        }

        private void DisplayDataFromJavascript(string data)
        {
            //
        }
    }
}