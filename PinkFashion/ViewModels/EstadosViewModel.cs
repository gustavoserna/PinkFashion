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
    public class EstadosViewModel : BaseViewModel
    {
        public ObservableCollection<Estados> Items { get; set; }

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
        public EstadosViewModel()
        {
            Items = new ObservableCollection<Estados>();

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
                IEnumerable<Estados> estados = null;
                List<Estados> lista = new List<Estados>();

                await GetEstados().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });

                estados = lista;

                foreach (var item in estados)
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
        
        public async Task<Estados[]> GetEstados()
        {
            var client = new HttpClient();
            StringContent str = new StringContent("op=getEstados", Encoding.UTF8, "application/x-www-form-urlencoded");
            var respuesta = await client.PostAsync(Constantes.url + "Sesion/App.php", str);
            var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
            System.Diagnostics.Debug.WriteLine("Tarjetas: " + json);

            if (json != "")
            {
                json_ob = JsonConvert.DeserializeObject<json_object>(json);
            }
            else
            {
                return json_ob.estados = null;
            }

            return json_ob.estados;
        }

        public class json_object
        {
            [JsonProperty("EntidadEstados")]
            public Estados[] estados { get; set; }
        }
    }
}
