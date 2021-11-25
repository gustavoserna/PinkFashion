using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using PinkFashion.ViewModels;
using PinkFashion.Models;
using PinkFashion.Views;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    public class PagarViewModel : BaseViewModel
    {
        public ObservableCollection<Card> Tarjetas { get; set; }
        public Command LoadTarjetasCommand { get; set; }
        json_object json_ob = new json_object();

        public PagarViewModel()
        {
            Tarjetas = new ObservableCollection<Card>();

            LoadTarjetasCommand = new Command(async () =>
            {
                await ExecuteLoadTarjetasCommand();
            });
        }

        async Task ExecuteLoadTarjetasCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Tarjetas.Clear();
                IEnumerable<Card> tarjetas = null;
                List<Card> lista = new List<Card>();

                await GetTarjetas().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });

                tarjetas = lista;

                foreach (var item in tarjetas)
                {
                    Tarjetas.Add(item);
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

        /*
        public async Task<string> EnviarPedido(Pedido pedido)
        {
            var json = JsonConvert.SerializeObject(pedido);
            System.Diagnostics.Debug.WriteLine("pedido: " + json);
            var client = new HttpClient();
            StringContent str = new StringContent("op=InsertaPedido&pedido=" + json, Encoding.UTF8, "application/x-www-form-urlencoded");
            var consulta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
            var respuesta = consulta.Content.ReadAsStringAsync().Result;
            var res = respuesta; //respuesta.Replace('"', '-');
            return res;

        }

        public async Task<Promociones> AplicarPromo(string codigo)
        {
            var client = new HttpClient();
            StringContent str = new StringContent("op=getAplicarPromo&codigo=" + codigo, Encoding.UTF8, "application/x-www-form-urlencoded");
            var consulta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
            var respuesta = consulta.Content.ReadAsStringAsync().Result.Trim();
            return JsonConvert.DeserializeObject<Promociones>(respuesta);
        }

        public async Task<string> AplicarCargo(double Total)
        {
            var client = new HttpClient();
            StringContent str = new StringContent("op=getAplicarCargoTarjeta&Total=" + Total, Encoding.UTF8, "application/x-www-form-urlencoded");
            var consulta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
            var respuesta = consulta.Content.ReadAsStringAsync().Result.Trim();
            return JsonConvert.DeserializeObject<string>(respuesta);
        }
        */

        public async Task<Card[]> GetTarjetas()
        {

            var client = new HttpClient();
            StringContent str = new StringContent("op=getTarjetas&IdCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
            var respuesta = await client.PostAsync(Constantes.url + "Sesion/App.php", str);
            var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
            System.Diagnostics.Debug.WriteLine("Tarjetas: " + json);

            if (json != "")
            {
                json_ob = JsonConvert.DeserializeObject<json_object>(json);
            }
            else
            {
                return json_ob.cards = null;
            }

            return json_ob.cards;
        }

        public class json_object
        {
            [JsonProperty("EntidadMetodoPago")]
            public Card[] cards { get; set; }
        }


    }
}
