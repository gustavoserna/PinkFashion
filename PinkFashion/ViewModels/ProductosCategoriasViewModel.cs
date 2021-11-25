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

namespace PinkFashion.ViewModels
{
    public class ProductosCategoriasViewModel : InsigniaViewModel
    {
        public ObservableCollection<Producto_> Productos { get; set; }
        public ObservableCollection<Producto_> ProductosRef { get; set; }
        public ObservableCollection<Marcas_> Marcas { get; set; }
        public Command LoadProductosCommand { get; set; }
        json_object json_ob = new json_object();
        json_objectMarca json_obMarca = new json_objectMarca();
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
        string idCategoria;

        public ProductosCategoriasViewModel(string id_categoria)
        {
            this.idCategoria = id_categoria;
            Productos = new ObservableCollection<Producto_>();
            Marcas = new ObservableCollection<Marcas_>();
            ProductosRef = new ObservableCollection<Producto_>();
            LoadProductosCommand = new Command(async () =>
            {
                await ExecuteLoadProductosCommand();
            });
        }

        public ICommand MarcaCommand
        {
            get
            {
                return new Command<Marcas_>((Marcas_ model) =>
                {

                    try
                    {
                        List<Producto_> lista = new List<Producto_>();
                        for (int i = 0; i < ProductosRef.Count(); i++)
                        {
                            lista.Add(ProductosRef[i]);
                        }
                        Productos.Clear();

                        if (model.Marca == "TODAS")
                        {
                            for (int i = 0; i < lista.Count(); i++)
                            {
                                Productos.Add(lista.ElementAt(i));
                            }
                        }
                        else
                        {
                            var match = lista.Where(x => x.marca.ToLower().Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Equals(model.Marca.ToLower()));
                            for (int i = 0; i < match.Count(); i++)
                            {
                                Productos.Add(match.ElementAt(i));
                            }
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

                });
            }
        }

        public ICommand ItemTappedCommand
        {
            get
            {
                return new Command<Producto_>(async (Producto_ model) =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new Producto(model));


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
                IEnumerable<Producto_> productos = null;
                List<Producto_> lista = new List<Producto_>();
                IEnumerable<Marcas_> marca = null;
                List<Marcas_> listamarcas = new List<Marcas_>();
                await GetProductos(idCategoria).ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });

                await GetMarcas(idCategoria).ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            listamarcas.Add(t.Result[i]);
                        }
                    }
                });


                productos = lista;
                marca = listamarcas;

                foreach (var item in productos)
                {
                    Productos.Add(item);
                    ProductosRef.Add(item);
                }

                var itemmarcatmp = new Marcas_();
                itemmarcatmp.IdMarca = "0";
                itemmarcatmp.Marca = "TODAS";
                itemmarcatmp.Imagen = "";
                Marcas.Add(itemmarcatmp);
                foreach (var itemmarca in marca)
                {
                    Marcas.Add(itemmarca);
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

        public void Buscar(string key = "")
        {
            try
            {
                List<Producto_> lista = new List<Producto_>();
                for (int i = 0; i < ProductosRef.Count(); i++)
                {
                    lista.Add(ProductosRef[i]);
                }

                var match = lista.Where(x => x.producto.ToLower().Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Contains(key.ToLower()));
                Productos.Clear();

                for (int i = 0; i < match.Count(); i++)
                {
                    Productos.Add(match.ElementAt(i));
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
        }

        public async Task<Producto_[]> GetProductos(string idcategoria)
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=ObtenerProductosCategoria&idcategoria=" + idcategoria, Encoding.UTF8, "application/x-www-form-urlencoded");
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

        public async Task<Marcas_[]> GetMarcas(string idcategoria)
        {
            try
            {
                var client = new HttpClient();

                StringContent str = new StringContent("op=ObtenerProductosMarcaCategorias&idcategoria=" + idcategoria, Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Productos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("Categorias: " + json);


                if (json != "")
                {
                    json_obMarca = JsonConvert.DeserializeObject<json_objectMarca>(json);
                }
                else
                {
                    return json_obMarca.marcas = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_obMarca.marcas;

        }
        
        public class json_objectMarca
        {
            [JsonProperty("ListadoMarcas")]
            public Marcas_[] marcas { get; set; }

        }
    }
}
