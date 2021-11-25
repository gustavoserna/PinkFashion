using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PinkFashion.Models;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    public class DetallePedidoViewModel : BaseViewModel
    {
        public ObservableCollection<Producto_> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        json_object json_ob = new json_object();
        public string numPedido;

        public DetallePedidoViewModel(string numPedido)
        {
            this.numPedido = numPedido;//
            Items = new ObservableCollection<Producto_>();

            LoadItemsCommand = new Command(async () =>
            {
                await ExecuteLoadItemsCommand();
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                IEnumerable<Producto_> productos = null;
                List<Producto_> lista = new List<Producto_>();

                await GetProductos(numPedido).ContinueWith(t =>
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
                    Items.Add(item);
                }
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

        public async Task<Producto_[]> GetProductos(string numPedido)
        {

            var client = new HttpClient();
            StringContent str = new StringContent("op=GetDetallePedido&numpedido=" + numPedido.Substring(12), Encoding.UTF8, "application/x-www-form-urlencoded");
            var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
            var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
            System.Diagnostics.Debug.WriteLine("DetallePedido: " + json);

            if (json != "")
            {
                json_ob = JsonConvert.DeserializeObject<json_object>(json);
            }
            else
            {
                return json_ob.productos = null;
            }

            return json_ob.productos;
        }

        /*
        public async Task<string> CancelaPedido(string numPedido)
        {
            string Regresa = "";
            try
            {

                var client = new HttpClient();
                StringContent str = new StringContent("op=CancelaPedido&pNumPedido=" + numPedido, Encoding.UTF8, "application/x-www-form-urlencoded");
                var consulta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var respuesta = consulta.Content.ReadAsStringAsync().Result.Trim();
                Regresa = respuesta;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return Regresa;
        }
        */

        public class json_object
        {
            [JsonProperty("EntidadDetallePedidos")]
            public Producto_[] productos { get; set; }
        }
    }
}
