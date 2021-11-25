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
    public class ProductosMarcasViewModel : InsigniaViewModel
    {
        public ObservableCollection<Producto_> Productos { get; set; }
        public ObservableCollection<Categoria_> Categorias { get; set; }
        public ObservableCollection<Producto_> ProductosRef { get; set; }
        public Command LoadProductosCommand { get; set; }
        json_object json_ob = new json_object();
        json_objectCat json_obCat = new json_objectCat();
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
        string IdMarca;
        public ProductosMarcasViewModel(string vidmarca)
        {
            this.IdMarca = vidmarca;
            Productos = new ObservableCollection<Producto_>();
            Categorias = new ObservableCollection<Categoria_>();
            ProductosRef = new ObservableCollection<Producto_>();
            LoadProductosCommand = new Command(async () =>
            {
                await ExecuteLoadProductosCommand();
            });
        }

        public ICommand CategoriaCommand
        {
            get
            {
                return new Command<Categoria_>((Categoria_ model) =>
                {                    

                    try
                    {
                        List<Producto_> lista = new List<Producto_>();
                        for (int i = 0; i < ProductosRef.Count(); i++)
                        {
                            lista.Add(ProductosRef[i]);
                        }
                        Productos.Clear();

                        if (model.Categoria=="TODAS")
                        {
                            for (int i = 0; i < lista.Count(); i++)
                            {
                                Productos.Add(lista.ElementAt(i));
                            }
                        }
                        else
                        {
                            var match = lista.Where(x => x.categoria.ToLower().Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Equals(model.Categoria.ToLower()));
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
                IEnumerable<Categoria_> categoria = null;
                List<Categoria_> listacat = new List<Categoria_>();
                await GetProductos().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });

                await GetCategorias().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            listacat.Add(t.Result[i]);
                        }
                    }
                });


                productos = lista;
                categoria = listacat;

                foreach (var item in productos)
                {
                    Productos.Add(item);
                    ProductosRef.Add(item);
                }

                var itemcatmtp = new Categoria_();
                itemcatmtp.IdCategoria = "0";
                itemcatmtp.Categoria = "TODAS";
                itemcatmtp.Imagen = "";
                Categorias.Add(itemcatmtp);
                foreach (var itemcat in categoria)
                {
                    Categorias.Add(itemcat);                    
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

        public async Task<Producto_[]> GetProductos()
        {
            try
            {
                var client = new HttpClient();
                
                StringContent str = new StringContent("op=ObtenerProductosMarca&IdMarca=" + IdMarca, Encoding.UTF8, "application/x-www-form-urlencoded");
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

        public async Task<Categoria_[]> GetCategorias()
        {
            try
            {
                var client = new HttpClient();

                StringContent str = new StringContent("op=ObtenerProductosCategoriasMarca&IdMarca=" + IdMarca, Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Productos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("Categorias: " + json);


                if (json != "")
                {
                    json_obCat = JsonConvert.DeserializeObject<json_objectCat>(json);
                }
                else
                {
                    return json_obCat.categorias = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_obCat.categorias;

        }

        public class json_objectCat
        {
            [JsonProperty("ListadoCategorias")]
            public Categoria_[] categorias { get; set; }

        }
    }
}
