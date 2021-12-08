using Newtonsoft.Json;
using PinkFashion.Models;
using PinkFashion.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    class CategoriaViewModel : BaseViewModel
    {
        Categoria_ categoria;
        string idCategoria = "";
        string idMarca = "";
        json_object json_ob = new json_object();
        public ObservableCollection<ColeccionSubcategorias> ColSubCategorias { get; set; }
        public ObservableCollection<Subcategoria_> Subcategorias { get; set; }
        public ObservableCollection<Producto_> Productos { get; set; }

        public Command LoadProductosCommand { get; set; }
        public Command LoadSubcategoriasCommand { get; set; }

        public string _NameCategoria;
        
        public string NameCategoria
        {
            get { return _NameCategoria; }
            set { SetProperty(ref _NameCategoria, value); }
        }

        bool _ListViewVisible = true;
        public bool ListViewVisible
        {
            get { return _ListViewVisible; }
            set { SetProperty(ref _ListViewVisible, value); }
        }

        bool _NoEncontradoVisible = false;
        public bool NoEncontradoVisible
        {
            get { return _NoEncontradoVisible; }
            set { SetProperty(ref _NoEncontradoVisible, value); }
        }

        public CategoriaViewModel(Categoria_ categoria)
        {
            this.categoria = categoria;
            Productos = new ObservableCollection<Producto_>();
            ColSubCategorias = new ObservableCollection<ColeccionSubcategorias>();
            Subcategorias = new ObservableCollection<Subcategoria_>();
            NameCategoria = categoria.Categoria;

            LoadProductosCommand = new Command(async () =>
            {
                await ExecuteLoadProductosCommand();
            });

            LoadSubcategoriasCommand = new Command(async () =>
            {
                await ExecuteLoadSubcategoriasCommand();
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
                await GetProductos(this.categoria).ContinueWith(t =>
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

                if (Productos.Count == 0)
                {
                    ListViewVisible = false;
                    NoEncontradoVisible = true;
                }
                else
                {
                    ListViewVisible = true;
                    NoEncontradoVisible = false;
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

        async Task ExecuteLoadSubcategoriasCommand()
        {

        }

        public ICommand AbrirMarcasCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessagingCenter.Subscribe<MarcasViewModel, string>(this, "idMarca", (sender, arg) =>
                    {
                        this.idMarca = arg;
                        LoadProductosCommand.Execute(null);
                    });

                    //var page = new NavigationPage(new MarcasVista(this.));
                    //page.BarBackgroundColor = App.bgColor;
                    //page.BarTextColor = App.textColor;
                    //App.Current.MainPage.Navigation.PushModalAsync(page);
                });
            }
        }

        public async Task<Producto_[]> GetProductos(Categoria_ categoria)
        {
            try
            {
                var client = new HttpClient();
                StringContent str = null;
                if (idCategoria.Equals(""))
                {
                    str = new StringContent("op=ObtenerProductosFamilia&idfamilia=" + categoria.IdCategoria, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
                else
                {
                    str = new StringContent("op=ObtenerProductosFamilia&idfamilia=" + categoria.IdCategoria + "&idCategoria=" + idCategoria, Encoding.UTF8, "application/x-www-form-urlencoded");

                }
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

        //public async Task<Categoria_[]> GetSubcategorias()
        //{
            //try
            //{
            //    var client = new HttpClient();
            //    StringContent str = new StringContent("op=categorias&idFamilia=" + this.familia.id_clasificacion, Encoding.UTF8, "application/x-www-form-urlencoded");
            //    var respuesta = await client.PostAsync(Constantes.url + "Listas/App.php", str);
            //    var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
            //    System.Diagnostics.Debug.WriteLine("Categorias: " + json);


            //    if (json != "")
            //    {
            //        json_ob = JsonConvert.DeserializeObject<json_object>(json);
            //    }
            //    else
            //    {
            //        return json_ob.categorias = null;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine(ex.Message);
            //}
            //return json_ob.categorias;
        //}
    }

    public class json_object
    {
        [JsonProperty("ListadoProductos")]
        public Producto_[] productos { get; set; }

        [JsonProperty("EntidadCategorias")]
        public Categoria_[] categorias { get; set; }

    }
}
