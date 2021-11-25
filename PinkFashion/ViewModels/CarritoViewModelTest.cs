using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PinkFashion.Models;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    public class CarritoViewModelTest : InsigniaViewModel
    {
        public PedidoTemporal Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public static json_object json_ob = new json_object();

        public ObservableCollection<Grouping<string, ProductoTemporal>> GroupedData { get; set; }
        //public List<Grouping<string, ProductoTemporal>> GroupedDataTmp { get; set; }

        bool includeEmptyGroups;
        public ObservableCollection<MarcaGroup> Grupo { get; private set; } 

        public static int cantidadGlobal = 0;
        string idproducto;

        double _cantidad = 0;
        public double cantidad
        {
            get
            {
                return _cantidad;
            }
            set
            {
                SetProperty(ref _cantidad, value);
            }
        }

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

        string _Direc = "";
        public string Direc
        {
            get
            {
                return _Direc;
            }
            set
            {
                SetProperty(ref _Direc, value);
            }
        }

        string _CostoEnvio;
        public string CostoEnvio
        {
            get
            {
                return _CostoEnvio;
            }
            set
            {
                SetProperty(ref _CostoEnvio, value);
            }
        }

        double _Latitud;
        public double Latitud
        {
            get
            {
                return _Latitud;
            }
            set
            {
                SetProperty(ref _Latitud, value);
            }
        }

        double _Longitud;
        public double Longitud
        {
            get
            {
                return _Longitud;
            }
            set
            {
                SetProperty(ref _Longitud, value);
            }
        }

        string _Descuento;
        public string Descuento
        {
            get
            {
                return _Descuento;
            }
            set
            {
                SetProperty(ref _Descuento, value);
            }
        }

        double _totalpedido;
        public double totalpedido
        {
            get
            {
                return _totalpedido;
            }
            set
            {
                SetProperty(ref _totalpedido, value);
            }
        }

        double _subtotalpedido;
        public double subtotalpedido
        {
            get
            {
                return _subtotalpedido;
            }
            set
            {
                SetProperty(ref _subtotalpedido, value);
            }
        }

        string _stotalpedidostring;
        public string stotalpedidostring
        {
            get
            {
                return _stotalpedidostring;
            }
            set { SetProperty(ref _stotalpedidostring, value); }
        }

        double _monederocliente;
        public double MonederoCliente
        {
            get
            {
                return _monederocliente;
            }
            set
            {
                SetProperty(ref _monederocliente, value);
            }
        }

        string _monederostring;
        public string Monederostring
        {
            get
            {
                return _monederostring;
            }
            set { SetProperty(ref _monederostring, value); }
        }

        string _totalpedidostring;
        public string totalpedidostring
        {
            get
            {
                return _totalpedidostring;
            }
            set { SetProperty(ref _totalpedidostring, value); }
        }

        string _granTotal;
        public string granTotal
        {
            get
            {
                return _granTotal;
            }
            set { SetProperty(ref _granTotal, value); }
        }

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

        Page page;

        public CarritoViewModelTest(Page page)
        {
            this.page = page;
            Items = new PedidoTemporal();
            Items.productos = new ObservableCollection<ProductoTemporal>();
            GroupedData = new ObservableCollection<Grouping<string, ProductoTemporal>>();
            //GroupedDataTmp = new List< Grouping<string, ProductoTemporal>>();
            Grupo = new ObservableCollection<MarcaGroup>();
            
            LoadItemsCommand = new Command(async () =>
            {
                await ExecuteLoadItemsCommand();
                //await CargarDatos();
            });
            
            //Task.Run(() => ExecuteLoadItemsCommand());
            //Task.Run(() => CreateMarcasCollectionAsync());

            
        }

        async Task CargarDatos()
        {
            List<ProductoTemporal> lista = new List<ProductoTemporal>();
            List<MarcaGroup> Grupotmp = new List<MarcaGroup>();

            PedidoTemporal vPedidosTemporal = await GetPedidos();


            for (int i = 0; i < vPedidosTemporal.productos.Count; i++)
            {

                lista.Add(vPedidosTemporal.productos[i]);
            }

            
            string vMarca = "";

            foreach (var item in lista)
            {

                Items.productos.Add(item);
                if (vMarca != item.Marca)
                {
                    Grupotmp.Add(new MarcaGroup(item.Marca, lista));
                }
                vMarca = item.Marca;
            }
            //Grupo = Grupotmp;
            
        }

        public async Task CreateMarcasCollectionAsync()
        {



            Grupo.Add(new MarcaGroup("Bissu", new List<ProductoTemporal>
            {
                new ProductoTemporal
                {
                    NombreProducto = "American Black Bear",
                    _Precio = "North America",
                    Marca = "Bissu",
                    Imagen = "https://upload.wikimedia.org/wikipedia/commons/0/08/01_Schwarzbär.jpg"
                }
            }));
            /*
            Grupo.Add(new MarcaGroup("Cats", new List<ProductoTemporal>
            {
                new ProductoTemporal
                {
                    NombreProducto = "Abyssinian",
                    _Precio = "Ethopia",
                    Imagen = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9b/Gustav_chocolate.jpg/168px-Gustav_chocolate.jpg"
                },
                new ProductoTemporal
                {
                    NombreProducto = "Arabian Mau",
                    _Precio = "Arabian Peninsula",
                    Imagen = "https://upload.wikimedia.org/wikipedia/commons/d/d3/Bex_Arabian_Mau.jpg"
                },
                new ProductoTemporal
                {
                    NombreProducto = "Bengal",
                    _Precio = "Asia",
                    Imagen = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/ba/Paintedcats_Red_Star_standing.jpg/187px-Paintedcats_Red_Star_standing.jpg"
                }
            }));

            Grupo.Add(new MarcaGroup("Dogs", new List<ProductoTemporal>
            {
                new ProductoTemporal
                {
                    NombreProducto = "Afghan Hound",
                    _Precio = "Afghanistan",
                    Imagen = "https://upload.wikimedia.org/wikipedia/commons/6/69/Afghane.jpg"
                },
                new ProductoTemporal
                {
                    NombreProducto = "Alpine Dachsbracke",
                    _Precio = "Austria",
                    Imagen = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/23/Alpejski_gończy_krótkonożny_g99.jpg/320px-Alpejski_gończy_krótkonożny_g99.jpg"
                }
            }));

            Grupo.Add(new MarcaGroup("Elephants", new List<ProductoTemporal>
            {
                new ProductoTemporal
                {
                    NombreProducto = "African Bush Elephant",
                    _Precio = "Africa",
                    Imagen = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/African_Elephant_%28Loxodonta_africana%29_bull_%2831100819046%29.jpg/320px-African_Elephant_%28Loxodonta_africana%29_bull_%2831100819046%29.jpg"
                },
                
                new ProductoTemporal
                {
                    NombreProducto = "Pygmy Mammoth",
                    _Precio = "Extinct",
                    Imagen = "https://upload.wikimedia.org/wikipedia/commons/f/f6/Mammuthus_exilis.jpg"
                }
            }));

            Grupo.Add(new MarcaGroup("Monkeys", new List<ProductoTemporal>
            {
                new ProductoTemporal
                {
                    NombreProducto = "Baboon",
                    _Precio = "Africa & Asia",
                    Imagen = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Papio_anubis_%28Serengeti%2C_2009%29.jpg/200px-Papio_anubis_%28Serengeti%2C_2009%29.jpg"
                }
            }));
            */



            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                Items.productos.Clear();
                IEnumerable<ProductoTemporal> productos = null;
                List<ProductoTemporal> lista = new List<ProductoTemporal>();
                PedidoTemporal vPedidosTemporal = await GetPedidos();
                /*
                await GetPedidos().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        _Direc = t.Result.Direc;
                        CostoEnvio = t.Result.CostoEnvio.ToString("c");
                        double total = 0;
                        double _CostoE = t.Result.CostoEnvio;
                        for (int i = 0; i < t.Result.productos.Count; i++)
                        {
                            if (t.Result.productos[i].IdProducto.Equals(idproducto))
                                cantidad = t.Result.productos[i].Cantidad;

                            total += Convert.ToDouble(t.Result.productos[i].Total);
                            lista.Add(t.Result.productos[i]);
                        }

                        totalpedido = total;
                        MonederoCliente = Convert.ToDouble(t.Result.MonederoCliente);
                        subtotalpedido = total - MonederoCliente;
                        Monederostring = MonederoCliente.ToString("c");
                        totalpedidostring = total.ToString("c");
                        stotalpedidostring = subtotalpedido.ToString("c");
                        if (Application.Current.Properties.ContainsKey("CostoEnvio"))
                        {
                            if (!Application.Current.Properties["CostoEnvio"].Equals(""))
                            {
                                _CostoE = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString());
                                CostoEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString()).ToString("c");
                            }
                        }
                        granTotal = (subtotalpedido + _CostoE).ToString("c");
                        if (EnviosGratis == "0")
                        {
                            CostoEnvio = Convert.ToDouble("0").ToString("c");
                            granTotal = subtotalpedido.ToString("c");
                        }

                    }
                });
                */
                for (int i = 0; i < vPedidosTemporal.productos.Count; i++)
                {

                    lista.Add(vPedidosTemporal.productos[i]);
                }

                productos = lista;
                string vMarca = "";

                foreach (var item in productos)
                {

                    Items.productos.Add(item);
                    if (vMarca != item.Marca)
                    {
                        Grupo.Add(new MarcaGroup(item.Marca, lista));
                    }
                    vMarca = item.Marca;
                }
                /*
                var sorted = productos.OrderBy(x => x.Marca).GroupBy(y => y.Marca);

                foreach (var item in sorted)
                {

                    var TotalMarca = (from p in productos
                                      where p.Marca == item.Key
                                      select p).Sum(e => e.Total);
                    GroupedData.Add(new Grouping<string, ProductoTemporal>(item.Key, TotalMarca, productos.Where(x => x.Marca == item.Key)));



                }
                */

                await Task.Delay(1000);
                if (Items.productos.Count == 0)
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

        public ICommand AgregaCommand
        {
            get
            {
                return new Command<ProductoTemporal>(async (ProductoTemporal model) =>
                {
                    model.Total = model.Total + model.Precio;
                    await addPedidoTmp(model);

                    //mod total pedido
                    totalpedido = totalpedido + model.Precio;
                    totalpedidostring = totalpedido.ToString("c");
                    granTotal = (Convert.ToDouble(granTotal.Substring(1)) + model.Precio).ToString("c");
                });
            }
        }

        public ICommand MasCommand
        {
            get
            {
                return new Command<ProductoTemporal>(async (ProductoTemporal model) =>
                {
                    int cant = Int32.Parse(model.Cantidad.ToString());
                    cant++;
                    model.Accion = "mas";

                    string respuesta = await addPedidoTmp(model);
                    if (!(respuesta == "1"))
                    {
                        await page.DisplayAlert("Precaución", respuesta, "Ok");
                    }
                    else
                    {
                        model.Cantidad = cant;
                        model.Total = model.Total + model.Precio;
                        noProductos++;
                        App.Cart++;
                        //mod total pedido
                        totalpedido = totalpedido + model.Precio;
                        totalpedidostring = totalpedido.ToString("c");
                        //granTotal = (Convert.ToDouble(granTotal.Substring(1)) + model.Precio).ToString("c");
                        //await ExecuteLoadItemsCommand();
                        subtotalpedido = totalpedido - MonederoCliente;
                        Monederostring = MonederoCliente.ToString("c");
                        stotalpedidostring = subtotalpedido.ToString("c");
                        if (EnviosGratis == "0")
                        {
                            CostoEnvio = Convert.ToDouble("0").ToString("c");
                            granTotal = subtotalpedido.ToString("c");
                        }
                        else
                        {
                            CostoEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString()).ToString("c");

                            granTotal = (subtotalpedido + Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString())).ToString("c");

                        }
                    }

                });
            }
        }

        public ICommand MenosCommand
        {
            get
            {
                return new Command<ProductoTemporal>(async (ProductoTemporal model) =>
                {
                    int cant = Int32.Parse(model.Cantidad.ToString());

                    if (cant > 0)
                    {
                        cant--;
                        model.Accion = "menos";
                        model.Cantidad = cant;
                        model.Total = model.Total - model.Precio;
                        noProductos--;
                        App.Cart--;
                        await ModPedidoTmp(model);
                        //mod total pedido
                        totalpedido = totalpedido - model.Precio;
                        totalpedidostring = totalpedido.ToString("c");
                        subtotalpedido = totalpedido - MonederoCliente;
                        Monederostring = MonederoCliente.ToString("c");
                        stotalpedidostring = subtotalpedido.ToString("c");
                        //if (cant == 0)
                        // await ExecuteLoadItemsCommand();

                        if (EnviosGratis == "0")
                        {
                            CostoEnvio = Convert.ToDouble("0").ToString("c");
                            granTotal = subtotalpedido.ToString("c");
                        }
                        else
                        {
                            CostoEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString()).ToString("c");

                            granTotal = (subtotalpedido + Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString())).ToString("c");

                        }

                    }
                });
            }
        }

        public ICommand EliminarCommand
        {
            get
            {
                return new Command<ProductoTemporal>(async (ProductoTemporal model) =>
                {
                    int cant = Int32.Parse(model.Cantidad.ToString());
                    cant = 0;
                    int CantAux = Int32.Parse(model.Cantidad.ToString());   //Int32.Parse(Math.Truncate(model.Cantidad));
                    model.Cantidad = cant;
                    model.Total = model.Total - model.Precio;
                    await DelPedidoTmp(model);
                    noProductos = noProductos - CantAux;
                    App.Cart = App.Cart - CantAux;

                    //mod total pedido
                    totalpedido = totalpedido - model.Precio;
                    totalpedidostring = totalpedido.ToString("c");
                    granTotal = (Convert.ToDouble(granTotal.Substring(1)) - model.Precio).ToString("c");
                    await ExecuteLoadItemsCommand();
                });
            }
        }

        public ICommand VaciarCommand
        {
            get
            {
                return new Command<PedidoTemporal>(async (PedidoTemporal model) =>
                {
                    await DeletePedidoTemporal();

                    //mod total pedido
                    totalpedido = 0;
                    noProductos = 0;
                    App.Cart = 0;
                    totalpedidostring = totalpedido.ToString("c");
                    granTotal = (0).ToString("c");
                    await ExecuteLoadItemsCommand();
                });

            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.productos.Clear();
                IEnumerable<ProductoTemporal> productos = null;
                List<ProductoTemporal> lista = new List<ProductoTemporal>();

                await GetPedidos().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        _Direc = t.Result.Direc;
                        CostoEnvio = t.Result.CostoEnvio.ToString("c");
                        double total = 0;
                        double _CostoE = t.Result.CostoEnvio;
                        for (int i = 0; i < t.Result.productos.Count; i++)
                        {
                            if (t.Result.productos[i].IdProducto.Equals(idproducto))
                                cantidad = t.Result.productos[i].Cantidad;

                            total += Convert.ToDouble(t.Result.productos[i].Total);
                            lista.Add(t.Result.productos[i]);
                        }

                        totalpedido = total;
                        MonederoCliente = Convert.ToDouble(t.Result.MonederoCliente);
                        subtotalpedido = total - MonederoCliente;
                        Monederostring = MonederoCliente.ToString("c");
                        totalpedidostring = total.ToString("c");
                        stotalpedidostring = subtotalpedido.ToString("c");
                        if (Application.Current.Properties.ContainsKey("CostoEnvio"))
                        {
                            if (!Application.Current.Properties["CostoEnvio"].Equals(""))
                            {
                                _CostoE = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString());
                                CostoEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString()).ToString("c");
                            }
                        }
                        granTotal = (subtotalpedido + _CostoE).ToString("c");
                        if (EnviosGratis == "0")
                        {
                            CostoEnvio = Convert.ToDouble("0").ToString("c");
                            granTotal = subtotalpedido.ToString("c");
                        }

                    }
                });

                productos = lista;
                string vMarca = "";

                foreach (var item in productos)
                {

                    Items.productos.Add(item);
                    if (vMarca != item.Marca)
                    {
                        Grupo.Add(new MarcaGroup(item.Marca, new List<ProductoTemporal>(lista)));
                    }
                    vMarca = item.Marca;
                }
                /*
                var sorted = productos.OrderBy(x => x.Marca).GroupBy(y => y.Marca);

                foreach (var item in sorted)
                {

                    var TotalMarca = (from p in productos
                                      where p.Marca == item.Key
                                      select p).Sum(e => e.Total);
                    GroupedData.Add(new Grouping<string, ProductoTemporal>(item.Key, TotalMarca, productos.Where(x => x.Marca == item.Key)));



                }
                */



                if (Items.productos.Count == 0)
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

        public async Task DeletePedidoTemporal()
        {
            try
            {
                _loader = true;
                _cont = false;
                var client = new HttpClient();
                StringContent str = new StringContent("op=eliminarTemporal&idcliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                _loader = false;
                _cont = true;
                totalpedido = 0;
                noProductos = 0;
                App.Cart = 0;

                //Obtiene datos de variables iniciales

                var client2 = new HttpClient();
                StringContent str2 = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta2 = await client2.PostAsync(Constantes.url + "Pedidos/App.php", str2);
                var json2 = respuesta2.Content.ReadAsStringAsync().Result.Trim();

                var obj = JObject.Parse(json2);
                string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                string displayMonedero = (string)obj.SelectToken("Monedero");
                string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                App.Monedero = displayMonedero;
                App.EnvioGratis = displayEnvios;
                Monedero = App.Monedero;
                EnviosGratis = App.EnvioGratis;
                if (!string.IsNullOrEmpty(displayNoProductos))
                {
                    App.Cart = Int32.Parse(displayNoProductos);
                }
                else
                {
                    App.Cart = 0;
                }


                //fin

                totalpedidostring = totalpedido.ToString("c");
                granTotal = (0).ToString("c");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public async Task DelPedidoTmp(ProductoTemporal producto)
        {
            try
            {
                var item = JsonConvert.SerializeObject(producto);
                System.Diagnostics.Debug.WriteLine(item);
                var client = new HttpClient();
                StringContent str = new StringContent("op=DeleteProductoTemporal&pedido=" + item, Encoding.UTF8, "application/x-www-form-urlencoded");
                await client.PostAsync(Constantes.url + "Pedidos/App.php", str);

                //Obtiene datos de variables iniciales

                var client2 = new HttpClient();
                StringContent str2 = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta2 = await client2.PostAsync(Constantes.url + "Pedidos/App.php", str2);
                var json2 = respuesta2.Content.ReadAsStringAsync().Result.Trim();

                var obj = JObject.Parse(json2);
                string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                string displayMonedero = (string)obj.SelectToken("Monedero");
                string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                App.Monedero = displayMonedero;
                App.EnvioGratis = displayEnvios;
                Monedero = App.Monedero;
                EnviosGratis = App.EnvioGratis;
                if (!string.IsNullOrEmpty(displayNoProductos))
                {
                    App.Cart = Int32.Parse(displayNoProductos);
                }
                else
                {
                    App.Cart = 0;
                }


                //fin
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public async Task ModPedidoTmp(ProductoTemporal producto)
        {
            try
            {
                var item = JsonConvert.SerializeObject(producto);
                System.Diagnostics.Debug.WriteLine(item);
                var client = new HttpClient();
                StringContent str = new StringContent("op=actualizaTemporal&pedido=" + item, Encoding.UTF8, "application/x-www-form-urlencoded");
                var consulta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var respuesta = consulta.Content.ReadAsStringAsync().Result.Trim();
                string idPedidoTmp = respuesta;
                noProductos = noProductos + Int32.Parse(producto.Cantidad.ToString());

                //Obtiene datos de variables iniciales

                var client2 = new HttpClient();
                StringContent str2 = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta2 = await client2.PostAsync(Constantes.url + "Pedidos/App.php", str2);
                var json2 = respuesta2.Content.ReadAsStringAsync().Result.Trim();

                var obj = JObject.Parse(json2);
                string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                string displayMonedero = (string)obj.SelectToken("Monedero");
                string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                App.Monedero = displayMonedero;
                App.EnvioGratis = displayEnvios;
                Monedero = App.Monedero;
                EnviosGratis = App.EnvioGratis;
                if (!string.IsNullOrEmpty(displayNoProductos))
                {
                    App.Cart = Int32.Parse(displayNoProductos);
                }
                else
                {
                    App.Cart = 0;
                }

                //fin
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public async Task<string> addPedidoTmp(ProductoTemporal producto)
        {
            string idPedidoTmp = "";
            try
            {
                var item = JsonConvert.SerializeObject(producto);
                System.Diagnostics.Debug.WriteLine(item);
                var client = new HttpClient();
                StringContent str = new StringContent("op=actualizaTemporal&pedido=" + item, Encoding.UTF8, "application/x-www-form-urlencoded");
                var consulta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var respuesta = consulta.Content.ReadAsStringAsync().Result.Trim();
                idPedidoTmp = respuesta;
                noProductos = noProductos + Int32.Parse(producto.Cantidad.ToString());
                if (!(idPedidoTmp == "1"))
                {
                    await page.DisplayAlert("Precaución", idPedidoTmp, "Ok");
                }
                else
                {
                    //Obtiene datos de variables iniciales

                    var client2 = new HttpClient();
                    StringContent str2 = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                    var respuesta2 = await client2.PostAsync(Constantes.url + "Pedidos/App.php", str2);
                    var json2 = respuesta2.Content.ReadAsStringAsync().Result.Trim();

                    var obj = JObject.Parse(json2);
                    string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                    string displayMonedero = (string)obj.SelectToken("Monedero");
                    string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                    App.Monedero = displayMonedero;
                    App.EnvioGratis = displayEnvios;
                    Monedero = App.Monedero;
                    EnviosGratis = App.EnvioGratis;
                    if (!string.IsNullOrEmpty(displayNoProductos))
                    {
                        App.Cart = Int32.Parse(displayNoProductos);
                    }
                    else
                    {
                        App.Cart = 0;
                    }


                    //fin
                }


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return idPedidoTmp;
        }

        public async Task ConfirmaCompra(string idcliente, string idDireccion, string FPago, string Envio, string Token, string meses)
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=confirmarCompra&idcliente=" + idcliente + "&iddireccion=" + idDireccion + "&idformapago=" + FPago + "&Envio=" + Envio + "&Token=" + Token + "&Meses=" + meses, Encoding.UTF8, "application/x-www-form-urlencoded");
                await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                //noProductos = noProductos + Int32.Parse(producto.Cantidad.ToString());
                totalpedido = 0;
                noProductos = 0;
                App.Cart = 0;

                //Obtiene datos de variables iniciales

                var client2 = new HttpClient();
                StringContent str2 = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta2 = await client2.PostAsync(Constantes.url + "Pedidos/App.php", str2);
                var json2 = respuesta2.Content.ReadAsStringAsync().Result.Trim();

                var obj = JObject.Parse(json2);
                string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                string displayMonedero = (string)obj.SelectToken("Monedero");
                string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                App.Monedero = displayMonedero;
                App.EnvioGratis = displayEnvios;
                Monedero = App.Monedero;
                EnviosGratis = App.EnvioGratis;
                if (!string.IsNullOrEmpty(displayNoProductos))
                {
                    App.Cart = Int32.Parse(displayNoProductos);
                }
                else
                {
                    App.Cart = 0;
                }


                //fin

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public async Task<string> AplicaCodigo(string codigo)
        {
            string respuesta = "";
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=realizarPromoCodigo&idCliente=" + Application.Current.Properties["IdCliente"] + "&codigo=" + codigo + "&fecha=" + DateTime.Now.ToString("yyyy-MM-dd"), Encoding.UTF8, "application/x-www-form-urlencoded");
                var consulta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                respuesta = consulta.Content.ReadAsStringAsync().Result.Trim();

                if (respuesta == "1")
                {
                    StringContent strDescuentos = new StringContent("op=recalcularDescuentos&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                    var consultaDescuentos = await client.PostAsync(Constantes.url + "Pedidos/App.php", strDescuentos);
                    var respuestaDescuentos = consultaDescuentos.Content.ReadAsStringAsync().Result.Trim();
                    await ExecuteLoadItemsCommand();
                }


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return respuesta;

        }

        public async Task AplicaMonedero(string monedero)
        {
            string respuesta = "";
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=aplicarMonedero&idCliente=" + Application.Current.Properties["IdCliente"] + "&cantidad=" + monedero, Encoding.UTF8, "application/x-www-form-urlencoded");
                var consulta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                respuesta = consulta.Content.ReadAsStringAsync().Result.Trim();
                if (respuesta == "1")
                {
                    StringContent strDescuentos = new StringContent("op=recalcularDescuentos&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                    var consultaDescuentos = await client.PostAsync(Constantes.url + "Pedidos/App.php", strDescuentos);
                    var respuestaDescuentos = consultaDescuentos.Content.ReadAsStringAsync().Result.Trim();
                    await ExecuteLoadItemsCommand();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public async Task InicializaCarrito()
        {
            try
            {
                totalpedido = 0;
                noProductos = 0;
                App.Cart = 0;

                //Obtiene datos de variables iniciales

                var client2 = new HttpClient();
                StringContent str2 = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta2 = await client2.PostAsync(Constantes.url + "Pedidos/App.php", str2);
                var json2 = respuesta2.Content.ReadAsStringAsync().Result.Trim();

                var obj = JObject.Parse(json2);
                string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                string displayMonedero = (string)obj.SelectToken("Monedero");
                string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                App.Monedero = displayMonedero;
                App.EnvioGratis = displayEnvios;
                Monedero = App.Monedero;
                EnviosGratis = App.EnvioGratis;
                if (!string.IsNullOrEmpty(displayNoProductos))
                {
                    App.Cart = Int32.Parse(displayNoProductos);
                }
                else
                {
                    App.Cart = 0;
                }


                //fin

                totalpedidostring = totalpedido.ToString("c");
                granTotal = (0).ToString("c");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public static async Task<PedidoTemporal> GetPedidos()
        {
            try
            {
                var client = new HttpClient();

                StringContent strDescuentos = new StringContent("op=recalcularDescuentos&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var consultaDescuentos = await client.PostAsync(Constantes.url + "Pedidos/App.php", strDescuentos);
                var respuestaDescuentos = consultaDescuentos.Content.ReadAsStringAsync().Result.Trim();



                StringContent str = new StringContent("op=SelectPedidosTemporal&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("pedidos temporales: " + json);
                if (json != "")
                {
                    json_ob = JsonConvert.DeserializeObject<json_object>(json);
                }
                else
                {
                    return json_ob.pedido[0] = null;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return json_ob.pedido[0];
        }

        public class json_object
        {
            [JsonProperty("PedidoTemporal")]
            public PedidoTemporal[] pedido { get; set; }
        }
    }
}
