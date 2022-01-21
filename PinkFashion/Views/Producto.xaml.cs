using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PinkFashion.Models;
using PinkFashion.ViewModels;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class Producto : ContentPage
    {
        ProductoViewModel productoViewModel;
        Producto_ producto = new Producto_();
        //string idProducto;
        public static bool root = false;

        public Producto(Producto_ producto)
        {
            InitializeComponent();
            Title = producto.producto;
            System.Diagnostics.Debug.WriteLine("PRODUCTO:" + JsonConvert.SerializeObject(producto));

            //lblTitulo.Text = producto.producto;
            this.producto = producto;
            BindingContext = productoViewModel = new ProductoViewModel(producto, Navigation);

            string strEvento = producto.producto + "|" + producto.marca + "|Pink Fashion Store";
            App.eventTracker.SendScreen(strEvento, nameof(Producto));

            vMarca.Text = producto.marca;
            vProducto.Text = producto.producto;
            vPrecio.Text = producto.precioCarrito;
            vSobrePrecio.Text = producto.SobrePreciostring;
            BloqueSobrePrecio.IsVisible = Convert.ToBoolean(producto.ConPromo);
            //vAgotado.IsVisible = Convert.ToBoolean(producto.Agotado);
            vDescripcion.Text = producto.descripcion;
            vTituloVer.Text = producto.TituloVer;
            if (producto.ConVariante == 0)
            {
                ListaVariantes.IsVisible = false;
            }

            /*EntCantidad.Text = "1";
            var clickMas = new TapGestureRecognizer();
            clickMas.Tapped += async (s, e) =>
            {
                if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                {
                    int cantidad = Int32.Parse(EntCantidad.Text);
                    cantidad++;
                    EntCantidad.Text = cantidad.ToString();
                }
                else
                {
                    bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                    if (ac)
                    {
                        await Navigation.PushAsync(new Login());


                    }
                }


            };
            btnMas.GestureRecognizers.Add(clickMas);*/

            /*var clickMenos = new TapGestureRecognizer();
            clickMenos.Tapped += async (s, e) =>
            {
                if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                {
                    int cantidad = Int32.Parse(EntCantidad.Text);
                    if (cantidad > 0)
                    {
                        cantidad--;
                        EntCantidad.Text = cantidad.ToString();
                    }
                    if (cantidad == 0)
                    {
                        cantidad = 0;
                        EntCantidad.Text = cantidad.ToString();
                    }
                }
                else
                {
                    bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                    if (ac)
                    {
                        await Navigation.PushAsync(new Login());


                    }
                }

            };*/
            //btnMenos.GestureRecognizers.Add(clickMenos);

            var clickAgrega = new TapGestureRecognizer();
            clickAgrega.Tapped += async (s, e) =>
            {

                
            };
            //btnAgregar.GestureRecognizers.Add(clickAgrega);
        }


        

        public async Task Contador()
        {
            try
            {
                var client = new HttpClient(new HttpClientHandler
                {
                    UseProxy = false
                });
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Keep-Alive", "600");
                StringContent str = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();

                var obj = JObject.Parse(json);
                string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                string displayIDAlianza = (string)obj.SelectToken("IDAlianza");
                
                if (!string.IsNullOrEmpty(displayNoProductos))
                {
                    App.Cart = Int32.Parse(displayNoProductos);
                    productoViewModel.noProductos = App.Cart;
                }
                else
                {
                    App.Cart = 0;
                    productoViewModel.noProductos = 0;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error:" + ex.Message);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            productoViewModel.LoadProductosCommand.Execute(null);
            //productoViewModel.noProductos = App.Cart;
            productoViewModel.Monedero = App.Monedero;
            productoViewModel.EnviosGratis = App.EnvioGratis;
            if (root)
            {
                Navigation.PopToRootAsync();
                root = false;
            }

        }
    }
}
