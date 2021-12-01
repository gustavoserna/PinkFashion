using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PinkFashion.Models;
using PinkFashion.Views;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    public class InicioViewModel : InsigniaViewModel
    {
        public INavigation Navigation { get; set; }
        public InicioModel InicioVista { get; set; }
        public ObservableCollection<Layoutpromo> LayoutPromos { get; set; }
        public ObservableCollection<ColeccionCategorias> ColCategorias { get; set; }
        public ObservableCollection<ColeccionFamilias> ColFamilias { get; set; }
        public Command LoadInicioCommand { get; set; }

        json_object json_ob = new json_object();
        json_objectprod json_obprod = new json_objectprod();
        json_objectc json_obc = new json_objectc();
        json_objectm json_obm = new json_objectm();

        bool _loader = false;
        public bool loader
        {
            get
            {
                return _loader;
            }
            set { SetProperty(ref _loader, value); }
        }

        bool _cont = true;
        public bool cont
        {
            get
            {
                return _cont;
            }
            set { SetProperty(ref _cont, value); }
        }

        string _Banner_principal;
        public string Banner_principal
        {
            get
            {
                return _Banner_principal;
            }
            set
            {
                SetProperty(ref _Banner_principal, value);
            }
        }

        string _Banner_promo;
        public string Banner_promo
        {
            
            get
            {
                return _Banner_promo;
            }
            set
            {
                SetProperty(ref _Banner_promo, value);
            }
        }

        string _Banner_nuevos;
        public string Banner_nuevos
        {
            get
            {
                return _Banner_nuevos;
            }
            set
            {
                SetProperty(ref _Banner_nuevos, value);
            }
        }

        string _Banner_vendidos;
        public string Banner_vendidos
        {
            get
            {
                return _Banner_vendidos;
            }
            set
            {
                SetProperty(ref _Banner_vendidos, value);
            }
        }

        public InicioViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            InicioVista = new InicioModel();
            InicioVista.Categorias = new ObservableCollection<Categoria_>();
            InicioVista.Familias = new ObservableCollection<Familia>();
            InicioVista.LayoutApp = new ObservableCollection<Layoutapp>();
            InicioVista.Banners = new ObservableCollection<BannerPrincipal>();
            ColCategorias = new ObservableCollection<ColeccionCategorias>();
            ColFamilias = new ObservableCollection<ColeccionFamilias>();
            LoadInicioCommand = new Command(async () =>
            {
                await ExecuteLoadInicioCommand();
            });
        }

        public ICommand CategoriaCommand
        {
            get
            {
                return new Command<Categoria_>(async (Categoria_ model) =>
                {
                     await Navigation.PushAsync(new ProductosCategoria(model.IdCategoria, model.Categoria));
                });
            }
        }

        public ICommand SelBannerCommand
        {
            get
            {
                return new Command<BannerPrincipal>(async (BannerPrincipal model) =>
                {

                    try
                    {
                        if (model.Tipo == 1)
                        {
                            Producto_ vProducto = await GetProductos(model.id_Producto);
                            await Navigation.PushAsync(new Producto(vProducto));

                        }
                        if (model.Tipo == 2)
                        {
                            Marcas_ vMarcas = await GetMarca(model.idmarca);
                            await Navigation.PushAsync(new ProductosMarca(model.idmarca, vMarcas.Marca));

                        }
                        if (model.Tipo == 3)
                        {
                            Categoria_ vCategoria = await GetCategoria(model.id_Categoria);
                            await Navigation.PushAsync(new ProductosCategoria(model.id_Categoria, vCategoria.Categoria));

                        }

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }

                });
            }
        }
        async Task ExecuteLoadInicioCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ColCategorias.Clear();
                List<Categoria_> listacategorias_for_col = new List<Categoria_>();

                InicioVista.Categorias.Clear();
                IEnumerable<Categoria_> categorias = null;
                List<Categoria_> listacategorias = new List<Categoria_>();

                ColFamilias.Clear();
                List<Familia> listafamilias_for_col = new List<Familia>();

                InicioVista.Familias.Clear();
                IEnumerable<Familia> familias = null;
                List<Familia> listafamilias = new List<Familia>();

                InicioVista.LayoutApp.Clear();
                IEnumerable<Layoutapp> layoutapp = null;
                List<Layoutapp> listalayout = new List<Layoutapp>();

                InicioVista.Banners.Clear();
                IEnumerable<BannerPrincipal> banner = null;
                List<BannerPrincipal> listabanner = new List<BannerPrincipal>();

                await GetInicio().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        Banner_principal = t.Result.Banner_principal;
                        Banner_promo = t.Result.Banner_promo;
                        Banner_nuevos = t.Result.Banner_nuevos;
                        Banner_vendidos = t.Result.Banner_vendidos;
                        for (int i = 0; i < t.Result.Categorias.Count; i++)
                        {
                            listacategorias.Add(t.Result.Categorias[i]);
                            listacategorias_for_col.Add(t.Result.Categorias[i]);
                        }
                        for(int i = 0; i < t.Result.Familias.Count; i++)
                        {
                            listafamilias.Add(t.Result.Familias[i]);
                            listafamilias_for_col.Add(t.Result.Familias[i]);
                        }
                        for (int i = 0; i < t.Result.LayoutApp.Count; i++)
                        {
                            listalayout.Add(t.Result.LayoutApp[i]);
                        }

                        for (int i = 0; i < t.Result.Banners.Count; i++)
                        {
                            listabanner.Add(t.Result.Banners[i]);
                        }

                    }
                });

                //carousel categorias
                double totalSlidesCategorias = Math.Ceiling((Double)listacategorias_for_col.Count / 3);
                for (int i = 0; i < totalSlidesCategorias; i++)
                {
                    ColeccionCategorias coleccion = new ColeccionCategorias();
                    coleccion.categorias = new List<Categoria_>();

                    for(int k = 0; k <= listacategorias_for_col.Count; k++)
                    {
                        if(k < 3 && listacategorias_for_col.Count > 0)
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

                //carousel familias
                double totalSlidesFamilias = Math.Ceiling((Double)listafamilias_for_col.Count / 3);
                for (int i = 0; i < totalSlidesFamilias; i++)
                {
                    ColeccionFamilias coleccion = new ColeccionFamilias();
                    coleccion.familias = new List<Familia>();

                    for (int k = 0; k <= listafamilias_for_col.Count; k++)
                    {
                        if (k < 3 && listafamilias_for_col.Count > 0)
                        {
                            coleccion.familias.Add(listafamilias_for_col[0]);
                            listafamilias_for_col.RemoveAt(0);
                        }
                        else
                        {
                            break;
                        }
                    }

                    ColFamilias.Add(coleccion);
                }

                categorias = listacategorias;
                layoutapp = listalayout;
                banner = listabanner;
                if (Application.Current.Properties.ContainsKey("Abreviado"))
                {
                    AliasUsuario = "Hola " + Application.Current.Properties["Abreviado"].ToString();
                    
                }
                else
                {
                    AliasUsuario = "";
                }
                foreach (var itemcat in categorias)
                {
                    InicioVista.Categorias.Add(itemcat);                    
                }

                foreach (var itemlay in layoutapp)
                {
                    InicioVista.LayoutApp.Add(itemlay);
                }

                foreach (var itemban in banner)
                {
                    InicioVista.Banners.Add(itemban);
                }
                if (Application.Current.Properties.ContainsKey("IdCliente"))
                {
                    await SendToken();
                }
                

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        public async Task<InicioModel> GetInicio()
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=ObtenerInicio", Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Productos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("inicio: " + json);
                if (json != "")
                {
                    json_ob = JsonConvert.DeserializeObject<json_object>(json);
                }
                else
                {
                    return json_ob.iniciomodel[0] = null;
                }

                //Obtiene datos de variables iniciales
                if (Application.Current.Properties.ContainsKey("IdCliente"))
                {
                    var client2 = new HttpClient();
                    StringContent str2 = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                    var respuesta2 = await client2.PostAsync(Constantes.url + "Pedidos/App.php", str2);
                    var json2 = respuesta2.Content.ReadAsStringAsync().Result.Trim();

                    var obj = JObject.Parse(json2);
                    string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                    string displayMonedero = (string)obj.SelectToken("Monedero");
                    string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                    string displayTituloMonedero = (string)obj.SelectToken("TituloMonedero");
                    string displayTituloEnvios = (string)obj.SelectToken("TituloEnvioGratis");
                    int displayAbandono = (int)obj.SelectToken("Abandonados");
                    Abandonados = displayAbandono;
                    if (Abandonados == 1)
                    {
                        visibleAbandonado = true;
                    }
                    else
                    {
                        visibleAbandonado = false;
                    }
                    App.Abandonados = Abandonados;
                    App.Monedero = displayMonedero;
                    App.EnvioGratis = displayEnvios;
                    App.TituloEnvioGratis = displayTituloEnvios;
                    App.TituloMonedero = displayTituloMonedero;
                    EnviosGratis = App.EnvioGratis;
                    Monedero = App.Monedero;
                    TituloEnviosGratis = App.TituloEnvioGratis;
                    TituloMonedero = App.TituloMonedero;
                    if (!string.IsNullOrEmpty(displayNoProductos))
                    {
                        App.Cart = Int32.Parse(displayNoProductos);
                        noProductos = App.Cart;
                    }
                    else
                    {
                        App.Cart = 0;
                        noProductos = App.Cart;
                    }
                }


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_ob.iniciomodel[0];

        }

        public class json_object
        {
            [JsonProperty("EntidadInicio")]
            public InicioModel[] iniciomodel { get; set; }

        }

        public async Task<Producto_> GetProductos(string id_producto)
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=ObtenerProducto&Idproducto=" + id_producto, Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Productos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("productos: " + json);
                if (json != "")
                {
                    json_obprod = JsonConvert.DeserializeObject<json_objectprod>(json);
                }
                else
                {
                    return json_obprod.productos[0] = null;
                }

                return json_obprod.productos[0];

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_obprod.productos[0];

        }

        public class json_objectprod
        {
            [JsonProperty("EntProducto")]
            public Producto_[] productos { get; set; }

        }

        /****** Obtener categoria *******/
        public async Task<Categoria_> GetCategoria(string id_categoria)
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=ObtenerCategoria&IdCategoria=" + id_categoria, Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Productos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("categoria: " + json);
                if (json != "")
                {
                    json_obc = JsonConvert.DeserializeObject<json_objectc>(json);
                }
                else
                {
                    return json_obc.categorias[0] = null;
                }

                return json_obc.categorias[0];

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_obc.categorias[0];

        }

        public class json_objectc
        {
            [JsonProperty("EntCategoria")]
            public Categoria_[] categorias { get; set; }

        }


        /****** Obtener marca *******/
        public async Task<Marcas_> GetMarca(string id_marca)
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=ObtenerMarca&IdMarca=" + id_marca, Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Productos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("marca: " + json);
                if (json != "")
                {
                    json_obm = JsonConvert.DeserializeObject<json_objectm>(json);
                }
                else
                {
                    return json_obm.marcas[0] = null;
                }

                return json_obm.marcas[0];

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_obm.marcas[0];

        }

        public class json_objectm
        {
            [JsonProperty("EntMarca")]
            public Marcas_[] marcas { get; set; }

        }

        public async Task SendToken()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("token onesignal2: " + App.pushTokenString);
                var client = new HttpClient();
                StringContent str = new StringContent("op=InsertTokenCliente&IDCliente=" + Application.Current.Properties["IdCliente"] + "&Token=" + App.pushTokenString, Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Sesion/App.php", str);
                string res = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("respuesta: " + res);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}