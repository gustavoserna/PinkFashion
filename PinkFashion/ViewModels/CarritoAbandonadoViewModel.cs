using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using PinkFashion.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Windows.Input;
using PinkFashion.Views;
using Newtonsoft.Json.Linq;

namespace PinkFashion.ViewModels
{
    public class CarritoAbandonadoViewModel : InsigniaViewModel
    {
        public ObservableCollection<CarritoAbandonado> Productos { get; set; }
        public Command LoadProductosCommand { get; set; }
        json_object json_ob = new json_object();
        bool _nodisponible = false;
        public bool nodisponible
        {
            get
            {
                return _nodisponible;
            }
            set
            {
                SetProperty(ref _nodisponible, value);
            }
        }
        
        public CarritoAbandonadoViewModel()
        {            
            Productos = new ObservableCollection<CarritoAbandonado>();            
            LoadProductosCommand = new Command(async () =>
            {
                await ExecuteLoadProductosCommand();
            });
        }

        public ICommand ItemTappedCommand
        {
            get
            {
                return new Command<CarritoAbandonado>(async (CarritoAbandonado model) =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new CarritosAbandonados());


                });
            }
        }

        public ICommand RecuperarCommand
        {
            get
            {
                return new Command<CarritoAbandonado>(async (CarritoAbandonado model) =>
                {
                    string Respuesta = await GetRecuperaCarrito();

                    
                    //await ExecuteLoadItemsCommand();
                });

            }
        }

        public ICommand RechazarCommand
        {
            get
            {
                return new Command<CarritoAbandonado>(async (CarritoAbandonado model) =>
                {
                    string Respuesta = await GetRechazarCarrito();


                    //await ExecuteLoadItemsCommand();
                });

            }
        }

        public ICommand EliminarCommand
        {
            get
            {
                return new Command<CarritoAbandonado>(async (CarritoAbandonado model) =>
                {
                    
                    string Respuesta = await SetEliminaAbandonado(model.numcarrito,model.idvariante_producto, model.idproducto);


                    //await ExecuteLoadItemsCommand();
                });

            }
        }

        async Task ExecuteLoadProductosCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Productos.Clear();
                IEnumerable<CarritoAbandonado> productos = null;
                List<CarritoAbandonado> lista = new List<CarritoAbandonado>();

                await GetProductos().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });

                productos = lista;

                foreach (var item in productos)
                {
                    Productos.Add(item);
                }

                if (Productos.Count == 0)

                    nodisponible = true;
                else
                    nodisponible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<string> GetRecuperaCarrito()
        {
            string Respuesta = "";
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=recuperaCarrito&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("Productos: " + json);


                if (json != "")
                {
                    Respuesta = json;

                    if (Application.Current.Properties.ContainsKey("IdCliente"))
                    {
                        var client2 = new HttpClient();
                        StringContent str2 = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                        var respuesta2 = await client2.PostAsync(Constantes.url + "Pedidos/App.php", str2);
                        var json2 = respuesta2.Content.ReadAsStringAsync().Result.Trim();

                        var obj = JObject.Parse(json2);
                        string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                        string displayMonedero = (string)obj.SelectToken("Monedero");
                        string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                        int displayAbandono = (int)obj.SelectToken("Abandonados");
                        Abandonados = displayAbandono;
                        App.Monedero = displayMonedero;
                        App.EnvioGratis = displayEnvios;
                        EnviosGratis = App.EnvioGratis;
                        Monedero = App.Monedero;

                        if (!string.IsNullOrEmpty(displayNoProductos))
                        {
                            App.Cart = Int32.Parse(displayNoProductos);
                            noProductos = App.Cart;
                        }
                        else
                        {
                            App.Cart = 0;
                            noProductos = App.Cart;
                        }
                    }
                }
                else
                {
                    Respuesta = "";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return Respuesta;

        }

        public async Task<string> GetRechazarCarrito()
        {
            string Respuesta = "";
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=rechazaCarrito&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("Productos: " + json);


                if (json != "")
                {
                    Respuesta = json;

                    if (Application.Current.Properties.ContainsKey("IdCliente"))
                    {
                        var client2 = new HttpClient();
                        StringContent str2 = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                        var respuesta2 = await client2.PostAsync(Constantes.url + "Pedidos/App.php", str2);
                        var json2 = respuesta2.Content.ReadAsStringAsync().Result.Trim();

                        var obj = JObject.Parse(json2);
                        string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                        string displayMonedero = (string)obj.SelectToken("Monedero");
                        string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                        int displayAbandono = (int)obj.SelectToken("Abandonados");
                        Abandonados = displayAbandono;
                        App.Monedero = displayMonedero;
                        App.EnvioGratis = displayEnvios;
                        EnviosGratis = App.EnvioGratis;
                        Monedero = App.Monedero;

                        if (!string.IsNullOrEmpty(displayNoProductos))
                        {
                            App.Cart = Int32.Parse(displayNoProductos);
                            noProductos = App.Cart;
                        }
                        else
                        {
                            App.Cart = 0;
                            noProductos = App.Cart;
                        }
                    }
                }
                else
                {
                    Respuesta = "";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return Respuesta;

        }

        public async Task<string> SetEliminaAbandonado(int vNumCarrito, string vIdVariante, string vIdProducto)
        {
            string Respuesta = "";
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=eliminaAbandonado&numcarrito=" + vNumCarrito.ToString() + "&idCliente=" + Application.Current.Properties["IdCliente"] + "&idvariante=" + vIdVariante + "&idproducto=" + vIdProducto, Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("Productos: " + json);


                if (json != "")
                {
                    Respuesta = json;
                }
                else
                {
                    Respuesta = "";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return Respuesta;

        }

        public async Task<CarritoAbandonado[]> GetProductos()
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=getCarritoAbandonado&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("Productos: " + json);


                if (json != "")
                {
                    json_ob = JsonConvert.DeserializeObject<json_object>(json);
                }
                else
                {
                    return json_ob.productos = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_ob.productos;

        }

        public class json_object
        {
            [JsonProperty("EntidadCarritoAbandonado")]
            public CarritoAbandonado[] productos { get; set; }

        }
    }
}
