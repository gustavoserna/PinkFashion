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
    public class FamiliaVistaViewModel : InsigniaViewModel
    {
        json_object json_ob = new json_object();

        Familia familia;

        public ObservableCollection<Producto_> Productos { get; set; }

        public Command LoadProductosCommand { get; set; }

        public FamiliaVistaViewModel(Familia familia)
        {
            this.familia = familia;
            Productos = new ObservableCollection<Producto_>();

            LoadProductosCommand = new Command(async () =>
            {
                await ExecuteLoadProductosCommand();
            });
        }

        async Task ExecuteLoadProductosCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Productos.Clear();
                IEnumerable<Producto_> productos = null;
                List<Producto_> lista = new List<Producto_>();
                await GetProductos(this.familia).ContinueWith(t =>
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

        public async Task<Producto_[]> GetProductos(Familia familia)
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=ObtenerProductosFamilia&idfamilia=" + familia.id_clasificacion, Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Productos/App.php", str);
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
            [JsonProperty("ListadoProductos")]
            public Producto_[] productos { get; set; }

        }
    }
}
