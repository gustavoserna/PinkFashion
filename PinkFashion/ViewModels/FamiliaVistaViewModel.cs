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

        public ObservableCollection<ColeccionCategorias> ColCategorias { get; set; }
        public ObservableCollection<Categoria_> Categorias { get; set; }
        public ObservableCollection<Producto_> Productos { get; set; }

        public Command LoadProductosCommand { get; set; }

        public FamiliaVistaViewModel(Familia familia)
        {
            this.familia = familia;
            Productos = new ObservableCollection<Producto_>();
            ColCategorias = new ObservableCollection<ColeccionCategorias>();
            Categorias = new ObservableCollection<Categoria_>();

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

                ColCategorias.Clear();
                List<Categoria_> listacategorias_for_col = new List<Categoria_>();

                Categorias.Clear();
                List<Categoria_> listacategorias = new List<Categoria_>();

                await GetCategorias().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        foreach (Categoria_ categoria in t.Result)
                        {
                            listacategorias.Add(categoria);
                            listacategorias_for_col.Add(categoria);
                        }
                    }
                });

                //carousel categorias
                double totalSlidesCategorias = Math.Ceiling((Double)listacategorias_for_col.Count / 3);
                for (int i = 0; i < totalSlidesCategorias; i++)
                {
                    ColeccionCategorias coleccion = new ColeccionCategorias();
                    coleccion.categorias = new List<Categoria_>();

                    for (int k = 0; k <= listacategorias_for_col.Count; k++)
                    {
                        if (k < 3 && listacategorias_for_col.Count > 0)
                        {
                            coleccion.categorias.Add(listacategorias_for_col[0]);
                            listacategorias_for_col.RemoveAt(0);
                        }
                        else
                        {
                            break;
                        }
                    }

                    ColCategorias.Add(coleccion);
                }


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

        public async Task<Categoria_[]> GetCategorias()
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=categorias", Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Listas/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("Categorias: " + json);


                if (json != "")
                {
                    json_ob = JsonConvert.DeserializeObject<json_object>(json);
                }
                else
                {
                    return json_ob.categorias = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_ob.categorias;

        }

        public class json_object
        {
            [JsonProperty("ListadoProductos")]
            public Producto_[] productos { get; set; }

            [JsonProperty("EntidadCategorias")]
            public Categoria_[] categorias { get; set; }

        }
    }
}
