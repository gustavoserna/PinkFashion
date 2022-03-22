using Newtonsoft.Json;
using PinkFashion.Models;
using PinkFashion.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PinkFashion.Helpers
{
    class BuscadorHelper : BaseViewModel
    {
        bool _Caja_Visibility = false;
        public string Consulta;
        public bool Caja_Visibility
        {
            get { return _Caja_Visibility; }
            set { SetProperty(ref _Caja_Visibility, value); }
        }
        string _Key = "";
        public ObservableCollection<ProductoAlgolia> Articulos { get; set; }
        public string Key
        {
            get { return _Key; }
            set { SetProperty(ref _Key, value); }
        }

        async Task ExecuteLoadAlgoliaSearchCommand()
        {
            IsBusy = true;
            System.Diagnostics.Debug.WriteLine("Ejecutando AlgoliaSearchCommand");
            try
            {
                Articulos.Clear();
                IEnumerable<ProductoAlgolia> articulos = null;

                List<ProductoAlgolia> lista = new List<ProductoAlgolia>();
                await AlgoliaSearchAsync().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });

                articulos = lista;

                foreach (var item in articulos)
                {
                    Articulos.Add(item);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public json_object json_ob = new json_object();
        public async Task<ProductoAlgolia[]> AlgoliaSearchAsync()
        {
            var search = Key.Replace(" ", "%20");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Algolia-Application-Id", Constantes.algolia_app_id);
            client.DefaultRequestHeaders.Add("X-Algolia-API-Key", Constantes.algolia_api_key);
            StringContent stringContent = new StringContent("{\"params\": \"query=" + search + "&hitsPerPage=10\"}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var consulta = await client.PostAsync(Constantes.algolia_url, stringContent);
            var respuesta = consulta.Content.ReadAsStringAsync().Result.Trim();
            System.Diagnostics.Debug.WriteLine("aloglia search hits: " + respuesta);
            if (respuesta != "")
            {
                json_ob = JsonConvert.DeserializeObject<json_object>(respuesta);
            }
            else
            {
                return json_ob.algolia_hits = null;
            }

            return json_ob.algolia_hits;
        }
    }

    public class json_object
    {
        [JsonProperty("hits")]
        public ProductoAlgolia[] algolia_hits { get; set; }
    }
}
