using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using PinkFashion.Models;
using PinkFashion.Views;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    public class FamiliaVistaViewModel : InsigniaViewModel
    {
        public INavigation Navigation { get; set; }

        json_object json_ob = new json_object();

        Familia familia;
        string idMarca = "";
        string filtroPrecio = "";

        public ObservableCollection<ColeccionCategorias> ColCategorias { get; set; }
        public ObservableCollection<Producto_> Productos { get; set; }

        public Command LoadProductosCommand { get; set; }
        public Command LoadCategoriasCommand { get; set; }

        string _NameFamilia;
        public string NameFamilia
        {
            get { return _NameFamilia; }
            set { SetProperty(ref _NameFamilia, value); }
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

        public FamiliaVistaViewModel(Familia familia, INavigation navigation)
        {
            this.familia = familia;
            this.Navigation = navigation;
            Productos = new ObservableCollection<Producto_>();
            ColCategorias = new ObservableCollection<ColeccionCategorias>();
            NameFamilia = familia.clasificaciones;

            LoadProductosCommand = new Command(async () =>
            {
                await ExecuteLoadProductosCommand();
            });

            LoadCategoriasCommand = new Command(async () =>
            {
                await ExecuteLoadCategoriasCommand();
            });
        }

        async Task ExecuteLoadCategoriasCommand()
        {
            try
            {
                ColCategorias.Clear();
                List<Categoria_> listacategorias_for_col = new List<Categoria_>();

                List<Categoria_> listacategorias = new List<Categoria_>();

                await GetCategorias().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        foreach (Categoria_ categoria in t.Result)
                        {
                            categoria.IdFamilia = familia.id_clasificacion;
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
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

                if(Productos.Count == 0)
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

        public ICommand ProductoTappedCommand
        {
            get
            {
                return new Command<Producto_>(async (Producto_ model) =>
                {
                    await Navigation.PushAsync(new Producto(model));
                });
            }
        }

        public ICommand ItemTappedCommand
        {
            get
            {
                return new Command<Categoria_>(async (Categoria_ model) =>
                {
                    await Navigation.PushAsync(new CategoriaVista(model));
                });
            }
        }

        public ICommand RemoverFiltrosCommand
        {
            get
            {
                return new Command(() =>
                {
                    this.idMarca = "";
                    this.filtroPrecio = "";
                    LoadProductosCommand.Execute(null);
                });
            }
        }

        public ICommand FiltroPrecioCommand
        {
            get
            {
                return new Command(() =>
                {
                    this.filtroPrecio = "menorMayor";
                    LoadProductosCommand.Execute(null);
                });
            }
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

                    var page = new NavigationPage(new MarcasVista(this.familia));
                    page.BarBackgroundColor = App.bgColor;
                    page.BarTextColor = App.textColor;
                    App.Current.MainPage.Navigation.PushModalAsync(page);
                });
            }
        }

        public async Task<Producto_[]> GetProductos(Familia familia)
        {
            try
            {
                var client = new HttpClient();
                StringContent str = null;

                if(!idMarca.Equals("") && filtroPrecio.Equals(""))
                {
                    str = new StringContent("op=ObtenerProductosFamilia&idfamilia=" + familia.id_clasificacion + "&idMarca=" + idMarca, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
                else if(!filtroPrecio.Equals("") && idMarca.Equals(""))
                {
                    str = new StringContent("op=ObtenerProductosFamilia&idfamilia=" + familia.id_clasificacion + "&filtroPrecio=" + filtroPrecio, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
                else if(!filtroPrecio.Equals("") && !idMarca.Equals(""))
                {
                    str = new StringContent("op=ObtenerProductosFamilia&idfamilia=" + familia.id_clasificacion + "&filtroPrecio=" + filtroPrecio + "&idMarca=" + idMarca, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
                else
                {
                    str = new StringContent("op=ObtenerProductosFamilia&idfamilia=" + familia.id_clasificacion, Encoding.UTF8, "application/x-www-form-urlencoded");
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

        public async Task<Categoria_[]> GetCategorias()
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=categorias&idFamilia=" + this.familia.id_clasificacion, Encoding.UTF8, "application/x-www-form-urlencoded");
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
