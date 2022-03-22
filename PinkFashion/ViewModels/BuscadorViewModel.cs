using PinkFashion.Helpers;
using PinkFashion.Models;
using PinkFashion.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace PinkFashion.ViewModels
{
    class BuscadorViewModel : BaseViewModel
    {
        INavigation Navigation;
        public BuscadorViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            Caja_Visibility = true;
            Consulta = "op=Buscar&key=" + Key.Trim();
            Articulos = new ObservableCollection<ProductoAlgolia>();
        }

        public ICommand PerformSearchCommand => new Command<string>((string query) =>
        {
            System.Diagnostics.Debug.WriteLine("Buscando " + query);
            this.Key = query;
            ExecuteLoadAlgoliaSearchCommand();
        });


        public ICommand ItemTappedCommand
        {
            get
            {
                return new Command<ProductoAlgolia>(model =>
                {
                    System.Diagnostics.Debug.WriteLine("Model: " + model);
                    GetProductos(model.idproducto.ToString());

                });
                //return new Command<ProductoAlgoria_>((ProductoAlgoria_ model) =>
                //{
                //    Navigation.PushAsync(new ProductoAlgoria(model));

                //    //IDictionary<string, string> firebaseLogList = new Dictionary<string, string>();
                //    //firebaseLogList.Add("Page", "BuscadorPage");
                //    //firebaseLogList.Add("Action", "ArticuloSeleccionado");
                //    //firebaseLogList.Add("Articulo", model.articulo);
                //    //service?.LogEvent("view_item", firebaseLogList);
                //});
            }
        }

        public ICommand CerrarVentanaCommand
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PopModalAsync();
                });
            }
        }

        //----------------------------------------------------------------------------
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

        public async Task ExecuteLoadAlgoliaSearchCommand()
        {
            IsBusy = true;
            System.Diagnostics.Debug.WriteLine("Ejecutando AlgoliaSearchCommand");
            try
            {
                if (Articulos != null)
                {
                    Articulos.Clear();
                }
                IEnumerable<ProductoAlgolia> articulos = null;

                List<ProductoAlgolia> lista = new List<ProductoAlgolia>();
                System.Diagnostics.Debug.WriteLine("Voy a buscar");
                await AlgoliaSearchAsync().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            System.Diagnostics.Debug.WriteLine("Resultado: " + t.Result[i]);
                            lista.Add(t.Result[i]);
                        }
                    }
                });

                articulos = lista;
                foreach (var item in articulos)
                {
                    Articulos.Add(item);
                }
                System.Diagnostics.Debug.WriteLine("Out: ");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Excepcion: " + ex.ToString());
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

        public class json_object
        {
            [JsonProperty("hits")]
            public ProductoAlgolia[] algolia_hits { get; set; }
        }

        json_objectprod json_obprod = new json_objectprod();
        public async Task<Producto_> GetProductos(string id_producto)
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=ObtenerProducto&Idproducto=" + id_producto, Encoding.UTF8, "application/x-www-form-urlencoded");
                System.Diagnostics.Debug.WriteLine("Writeline - Antes de respuesta");
                var respuesta = await client.PostAsync(Constantes.url + "Productos/App.php", str);
                System.Diagnostics.Debug.WriteLine("Writeline - Antes de JSON");
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("Writeline - productos: " + json);
                if (json != "")
                {
                    System.Diagnostics.Debug.WriteLine("Writeline - Json no cad vacia");
                    json_obprod = JsonConvert.DeserializeObject<json_objectprod>(json);
                    await Navigation.PushAsync(new Producto(json_obprod.productos[0]));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Writeline - Json null");
                    return json_obprod.productos[0] = null;
                }

                return json_obprod.productos[0];

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception en productos: " + ex.Message);
            }
            return json_obprod.productos[0];
        }

        public class json_objectprod
        {
            [JsonProperty("EntProducto")]
            public Producto_[] productos { get; set; }

        }
    }
}
