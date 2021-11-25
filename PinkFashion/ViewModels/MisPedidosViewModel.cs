using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;
using PinkFashion.Models;
using System.Text;

namespace PinkFashion.ViewModels
{
    public class MisPedidosViewModel : BaseViewModel
    {
        public ObservableCollection<Pedido> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        json_object json_ob = new json_object();

        bool _noencontrado = false;
        public bool noencontrado
        {
            get
            {
                return _noencontrado;
            }
            set
            {
                SetProperty(ref _noencontrado, value);
            }
        }

        public MisPedidosViewModel()
        {
            Items = new ObservableCollection<Pedido>();

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
                IEnumerable<Pedido> pedidos = null;
                List<Pedido> lista = new List<Pedido>();

                await GetPedidos().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });

                pedidos = lista;

                foreach (var item in pedidos)
                {
                    Items.Add(item);
                }

                if (Items.Count == 0)
                    noencontrado = true;
                else
                    noencontrado = false;
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


        public async Task<Pedido[]> GetPedidos()
        {

            var client = new HttpClient();
            StringContent str = new StringContent("op=SelectPedidoApp&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
            var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
            var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
            System.Diagnostics.Debug.WriteLine("MisPedidos: " + json);


            if (json != "")
            {
                json_ob = JsonConvert.DeserializeObject<json_object>(json);
            }
            else
            {
                return json_ob.pedidos = null;
            }

            return json_ob.pedidos;
        }

        public class json_object
        {
            [JsonProperty("EntidadPedidos")]
            public Pedido[] pedidos { get; set; }
        }
    }
}
