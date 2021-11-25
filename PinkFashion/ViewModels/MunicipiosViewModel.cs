using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using PinkFashion.Models;
using Xamarin.Forms;
namespace PinkFashion.ViewModels
{
    public class MunicipiosViewModel : BaseViewModel
    {
        public ObservableCollection<Municipios> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        string pidEstado;
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
        public MunicipiosViewModel(string pEstado)
        {
            pidEstado = pEstado;
            Items = new ObservableCollection<Municipios>();

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
                IEnumerable<Municipios> municipios = null;
                List<Municipios> lista = new List<Municipios>();

                await GetMunicipios(pidEstado).ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });

                municipios = lista;

                foreach (var item in municipios)
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

        public async Task<Municipios[]> GetMunicipios(string vEstado)
        {
            var client = new HttpClient();
            StringContent str = new StringContent("op=getMunicipios&IdEstado=" + vEstado, Encoding.UTF8, "application/x-www-form-urlencoded");
            var respuesta = await client.PostAsync(Constantes.url + "Sesion/App.php", str);
            var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
            System.Diagnostics.Debug.WriteLine("Tarjetas: " + json);

            if (json != "")
            {
                json_ob = JsonConvert.DeserializeObject<json_object>(json);
            }
            else
            {
                return json_ob.municipios = null;
            }

            return json_ob.municipios;
        }

        public class json_object
        {
            [JsonProperty("EntidadMunicipios")]
            public Municipios[] municipios { get; set; }
        }
    }
}
