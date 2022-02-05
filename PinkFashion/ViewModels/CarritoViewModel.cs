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
using Openpay.Xamarin;
using PinkFashion.Models;
using PinkFashion.Views;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    public class CarritoViewModel : InsigniaViewModel
    {
        string deviceSession = "";
        public INavigation Navigation { get; set; }

        public PedidoTemporal Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public static json_object json_ob = new json_object();
        public static json_objectprod json_obprod = new json_objectprod();

        public ObservableCollection<Grouping<string, ProductoTemporal>> GroupedData { get; set; }
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

        bool _SesionIniciada = false;
        public bool SesionIniciada
        {
            get
            {
                return _SesionIniciada;
            }
            set
            {
                SetProperty(ref _SesionIniciada, value);
            }
        }

        bool _SesionNoIniciada = true;
        public bool SesionNoIniciada
        {
            get
            {
                return _SesionNoIniciada;
            }
            set
            {
                SetProperty(ref _SesionNoIniciada, value);
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
        public string TotalCostoEnvio
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

        double _doubleEnvio=0;
        public double doubleEnvio
        {
            get
            {
                return _doubleEnvio;
            }
            set
            {
                SetProperty(ref _doubleEnvio, value);
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

        int heightDir;
        public int HeightDir
        {
            get
            {
                return heightDir;
            }
            set
            {
                heightDir = value;
                OnPropertyChanged();
            }
        }

        int heightTar;
        public int HeightTar
        {
            get
            {
                return heightTar;
            }
            set
            {
                heightTar = value;
                OnPropertyChanged();
            }
        }
        Page page;

        public CarritoViewModel(Page page, INavigation navigation)
        {
            this.page = page;
            Navigation = navigation;
            Items = new PedidoTemporal();
            Items.productos = new ObservableCollection<ProductoTemporal>();
            Items.Tarjetas = new ObservableCollection<Card>();
            Items.Direcciones = new ObservableCollection<Direccion>();
            GroupedData = new ObservableCollection<Grouping<string, ProductoTemporal>>();
            Grupo = new ObservableCollection<MarcaGroup>();
            if (EnviosGratis == "0")
            {
                doubleEnvio = 0;
                TotalCostoEnvio = Convert.ToDouble("0").ToString("c");
            }
            else
            {
                if (Application.Current.Properties.ContainsKey("CostoEnvio"))
                {
                    if (!Application.Current.Properties["CostoEnvio"].Equals(""))
                    {
                        doubleEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString());
                        TotalCostoEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString()).ToString("c");

                    }
                }

            }
            LoadItemsCommand = new Command(async () =>
            {
                await ExecuteLoadItemsCommand();
            });
    
        }

        public ICommand IniciarSesionCommand
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PushModalAsync(new Login());
                });
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
                    model.Cantidad = cant;
                    model.Descuento = (model.Precio * model.Cantidad) - (model.PrecioDescuento * model.Cantidad);
                    model.Total = (model.Precio * model.Cantidad) - model.Descuento;
                    string respuesta = await addPedidoTmp(model);
                    if (respuesta == "1")
                    {
                       MessagingCenter.Send<CarritoViewModel, int>(this, "Badge", +1);
                       noProductos++;
                        App.Cart++;
                        //mod total pedido
                        totalpedido = totalpedido + model.Precio;
                        totalpedidostring = totalpedido.ToString("c");
                        subtotalpedido = totalpedido - MonederoCliente;
                        Monederostring = MonederoCliente.ToString("c");
                        stotalpedidostring = subtotalpedido.ToString("c");
                        TotalCostoEnvio = doubleEnvio.ToString("c");
                        granTotal = (subtotalpedido + doubleEnvio).ToString("c");
                    }
                    else
                    {
                        await ExecuteLoadItemsCommand();
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
                        model.Descuento = (model.Precio * model.Cantidad) - (model.PrecioDescuento * model.Cantidad);
                        model.Total = (model.Precio * model.Cantidad) - model.Descuento;

                        string respuesta = await ModPedidoTmp(model);
                        if (respuesta == "1")
                        {
                            noProductos--;
                            App.Cart--;
                            MessagingCenter.Send<CarritoViewModel, int>(this, "Badge", -1);
                            //mod total pedido
                            totalpedido = totalpedido - model.Precio;
                            totalpedidostring = totalpedido.ToString("c");
                            subtotalpedido = totalpedido - MonederoCliente;
                            Monederostring = MonederoCliente.ToString("c");
                            stotalpedidostring = subtotalpedido.ToString("c");
                            granTotal = (subtotalpedido + doubleEnvio).ToString("c");
                            TotalCostoEnvio = doubleEnvio.ToString("c");
                            if (cant == 0)
                                await ExecuteLoadItemsCommand();

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

        public ICommand BorrarDireccionCommand
        {
            get
            {
                return new Command<Direccion>(async (Direccion model) =>
                {
                    var action = await Application.Current.MainPage.DisplayActionSheet("¿Eliminar dirección?", "Cancelar", "Sí", "No");
                    if (action.Equals("Sí"))
                    {
                        try
                        {
                            await BorrarDireccion(model);
                            await Application.Current.MainPage.DisplayAlert("Listo", "Dirección eliminada", "Ok");
                            LoadItemsCommand.Execute(null);
                        }
                        catch (Exception ex)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un error, inténtalo de nuevo mas tarde", "Ok");
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    }
                });
            }
        }

        public ICommand SelDireccionCommand
        {
            get
            {
                return new Command<Direccion>(async (Direccion model) =>
                {
                    
                        try
                        {
                            await SelDireccion(model);
                            
                        }
                        catch (Exception ex)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un error, inténtalo de nuevo mas tarde", "Ok");
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    
                });
            }
        }

        public async Task BorrarDireccion(Direccion dir)
        {
            var client = new HttpClient();
            StringContent str = new StringContent("op=eliminarDireccion&id=" + dir.IdClienteDir, Encoding.UTF8, "application/x-www-form-urlencoded");
            await client.PostAsync(Constantes.url + "Sesion/App.php", str);

            if (dir.Direc_Default == 1)
            {                
                Application.Current.Properties["IDDireccion"] = "";
                Application.Current.Properties["Direccion"] = "";
                Application.Current.Properties["CostoEnvio"] = "";
                await Application.Current.SavePropertiesAsync();
            }
        }

        public ICommand SelTarjetaCommand
        {
            get
            {
                return new Command<Card>(async (Card model) =>
                {                    
                    await SelTarjeta(model);                    
                });
            }
        }

        public ICommand SelProductoCarritoCommand
        {
            get
            {
                return new Command<ProductoTemporal>(async (ProductoTemporal model) =>
                {
                    Producto_ modelProducto = await GetProductos(model.IdProducto);
                    //await Navigation.PushModalAsync(new Producto(modelProducto));
                    await Application.Current.MainPage.Navigation.PopModalAsync();

                    await Application.Current.MainPage.Navigation.PushAsync(new Producto(modelProducto));

                });
            }
        }

        public ICommand BorrarTarjetaCommand
        {
            get
            {
                return new Command<Card>(async (Card model) =>
                {
                    var action = await Application.Current.MainPage.DisplayActionSheet("¿Eliminar tarjeta?", "Cancelar", "Sí", "No");
                    if (action.Equals("Sí"))
                    {
                        try
                        {
                            await BorrarTarjeta(model);
                            await Application.Current.MainPage.DisplayAlert("Listo", "Tarjeta eliminada", "Ok");
                            LoadItemsCommand.Execute(null);
                        }
                        catch (Exception ex)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un error, inténtalo de nuevo mas tarde", "Ok");
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    }
                });
            }
        }

        public async Task BorrarTarjeta(Card card)
        {

            var json = JsonConvert.SerializeObject(card);
            var client = new HttpClient();
            StringContent str = new StringContent("op=DeleteTarjetaOpenPay&IdTarjeta=" + card.IdTarjeta + "&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
            await client.PostAsync(Constantes.url + "Sesion/App.php", str);
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.productos.Clear();
                Items.Tarjetas.Clear();
                Items.Direcciones.Clear();
                IEnumerable<ProductoTemporal> productos = null;
                IEnumerable<Card> tarjetas = null;
                IEnumerable<Direccion> direcciones = null;
                List<ProductoTemporal> lista = new List<ProductoTemporal>();
                List<Card> listaTarjetas = new List<Card>();
                List<Direccion> listaDirec = new List<Direccion>();
                Grupo.Clear();
                await GetPedidos().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        _Direc = t.Result.Direc;
                        TotalCostoEnvio = t.Result.CostoEnvio.ToString("c");
                        double total = 0;
                        double _CostoE = t.Result.CostoEnvio;
                        for (int i = 0; i < t.Result.productos.Count; i++)
                        {
                            if (t.Result.productos[i].IdProducto.Equals(idproducto))
                                cantidad = t.Result.productos[i].Cantidad;

                            total += Convert.ToDouble(t.Result.productos[i].Total);
                            lista.Add(t.Result.productos[i]);
                        }

                        for (int i = 0; i < t.Result.Tarjetas.Count; i++)
                        {                            
                            listaTarjetas.Add(t.Result.Tarjetas[i]);
                        }
                        for (int i = 0; i < t.Result.Direcciones.Count; i++)
                        {
                            listaDirec.Add(t.Result.Direcciones[i]);
                        }

                        totalpedido = total;
                        MonederoCliente = Convert.ToDouble(t.Result.MonederoCliente);
                        subtotalpedido = total - MonederoCliente;
                        Monederostring = MonederoCliente.ToString("c");
                        totalpedidostring = total.ToString("c");
                        stotalpedidostring = subtotalpedido.ToString("c");
                        TotalCostoEnvio = doubleEnvio.ToString("c");
                        granTotal = (subtotalpedido + doubleEnvio).ToString("c");
                    }
                });

                productos = lista;
                tarjetas = listaTarjetas;
                if (listaTarjetas.Count == 0)
                {
                    Application.Current.Properties["IDMetodoPago"] = "";
                    Application.Current.Properties["MetodoPago"] = "";
                    Application.Current.Properties["TokenTarjeta"] = "";
                    await Application.Current.SavePropertiesAsync();
                }
                HeightTar = (listaTarjetas.Count * 50);
                direcciones = listaDirec;
                if (listaDirec.Count == 0)
                {
                    Application.Current.Properties["IDDireccion"] = "";
                    Application.Current.Properties["Direccion"] = "";
                    Application.Current.Properties["CostoEnvio"] = "";
                    await Application.Current.SavePropertiesAsync();                    
                }
                HeightDir = (listaDirec.Count * 50);

                foreach (var item in productos)
                {
                    Items.productos.Add(item);                    
                }
                foreach (var itemTarj in tarjetas)
                {
                    Items.Tarjetas.Add(itemTarj);
                }
                foreach (var itemDir in direcciones)
                {
                    if (itemDir.Direc_Default == 1)
                    {
                        itemDir.imagen = "checkpink.png";
                        Application.Current.Properties["IDDireccion"] = itemDir.IdClienteDir;
                        Application.Current.Properties["Direccion"] = itemDir.MiDireccion;
                        Application.Current.Properties["CostoEnvio"] = itemDir.CostoEnvio;
                        await Application.Current.SavePropertiesAsync();
                        await CalcularCostoEnvio();
                    } else
                    {
                        itemDir.imagen = "checkgrey.png";
                    }
                    Items.Direcciones.Add(itemDir);
                }
                

                var sorted = productos.OrderBy(x => x.Marca).GroupBy(y => y.Marca);

                foreach (var item in sorted)
                {
                    Grupo.Add(new MarcaGroup(item.Key, new List<ProductoTemporal>(productos.Where(x => x.Marca == item.Key))));
                }

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
                string displayTituloMonedero = (string)obj.SelectToken("TituloMonedero");
                string displayTituloEnvios = (string)obj.SelectToken("TituloEnvioGratis");
                App.Monedero = displayMonedero;
                App.EnvioGratis = displayEnvios;
                App.TituloEnvioGratis = displayTituloEnvios;
                App.TituloMonedero = displayTituloMonedero;
                Monedero = App.Monedero;
                EnviosGratis = App.EnvioGratis;
                TituloEnviosGratis = App.TituloEnvioGratis;
                TituloMonedero = App.TituloMonedero;
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

        public async Task SelDireccion(Direccion SelDirec)
        {

            System.Diagnostics.Debug.WriteLine("Direccion seleccionada: "+SelDirec.IdClienteDir);
            try
            {

                foreach (var itemDir in Items.Direcciones)
                {
                    if (SelDirec.IdClienteDir == itemDir.IdClienteDir)
                    {
                        itemDir.imagen = "checkpink.png";
                        System.Diagnostics.Debug.WriteLine("La dirección ha sido encontrada");
                        Application.Current.Properties["IDDireccion"] = itemDir.IdClienteDir;
                        Application.Current.Properties["Direccion"] = itemDir.MiDireccion;
                        Application.Current.Properties["CostoEnvio"] = itemDir.CostoEnvio;
                        await Application.Current.SavePropertiesAsync();
                        await CalcularCostoEnvio();
                    }
                    else
                    {
                        itemDir.imagen = "checkgrey.png";                        
                    }
                }
                //fin
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public async Task SelTarjeta(Card card)
        {
            try
            {

                foreach (var itemTar in Items.Tarjetas)
                {
                    if (card.IdTarjeta == itemTar.IdTarjeta)
                    {
                        itemTar.imagen = "checkpink.png";
                        Application.Current.Properties["IDMetodoPago"] = itemTar.IdTarjeta;
                        Application.Current.Properties["MetodoPago"] = itemTar.Cuenta;
                        Application.Current.Properties["TokenTarjeta"] = itemTar.Token;
                        //SelTarjeta = itemTar.Token;
                        await Application.Current.SavePropertiesAsync();
                    }
                    else
                    {
                        itemTar.imagen = "checkgris.png";
                    }
                }
                //fin
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public async Task<string> ModPedidoTmp(ProductoTemporal producto)
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
                    
                    if ((producto.Accion== "menos") && (producto.Cantidad==0))
                    {
                        var client1 = new HttpClient();
                        StringContent str1 = new StringContent("op=getLimpiaMonedero&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                        var consulta1 = await client.PostAsync(Constantes.url + "Pedidos/App.php", str1);
                        var respuesta1 = consulta.Content.ReadAsStringAsync().Result.Trim();
                    }
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

                    if (EnviosGratis == "0")
                    {
                        doubleEnvio = 0;
                        TotalCostoEnvio = Convert.ToDouble("0").ToString("c");
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey("CostoEnvio"))
                        {
                            if (!Application.Current.Properties["CostoEnvio"].Equals(""))
                            {
                                doubleEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString());
                                TotalCostoEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString()).ToString("c");

                            }
                        }

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

        public async Task CalcularCostoEnvio()
        {
            try
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
                if (EnviosGratis == "0")
                {
                    doubleEnvio = 0;
                    TotalCostoEnvio = Convert.ToDouble("0").ToString("c");
                    granTotal = (subtotalpedido + doubleEnvio).ToString("c");
                }
                else
                {
                    if (Application.Current.Properties.ContainsKey("CostoEnvio"))
                    {
                        if (!Application.Current.Properties["CostoEnvio"].Equals(""))
                        {
                            doubleEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString());
                            TotalCostoEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString()).ToString("c");
                            granTotal = (subtotalpedido + doubleEnvio).ToString("c");
                        }
                    }

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
                if (!(idPedidoTmp=="1"))
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

                    if (EnviosGratis == "0")
                    {
                        doubleEnvio = 0;
                        TotalCostoEnvio = Convert.ToDouble("0").ToString("c");                        
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey("CostoEnvio"))
                        {
                            if (!Application.Current.Properties["CostoEnvio"].Equals(""))
                            {
                                doubleEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString());
                                TotalCostoEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString()).ToString("c");
                                
                            }
                        }

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

        public async Task<string> ConfirmaCompra(string idcliente, string idDireccion, string FPago, string Envio, string Token, string meses)
        {
            string regresar = "";
            try
            {
                deviceSession = await CrossOpenpay.Current.CreateDeviceSessionId();
                string idTarjeta = "";
                if(Application.Current.Properties.ContainsKey("IDMetodoPago"))
                {
                    idTarjeta = Application.Current.Properties["IDMetodoPago"].ToString();
                } 
                var client = new HttpClient();
                StringContent str = new StringContent("op=PagoOpenPay&idcliente=" + idcliente + "&iddireccion=" + idDireccion + "&idformapago=" + FPago + "&Envio=" + Envio + "&idTarjeta=" + idTarjeta + "&Meses=" + meses + "&deviceSesion=" + this.deviceSession, Encoding.UTF8, "application/x-www-form-urlencoded");
                var consulta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var respuesta = consulta.Content.ReadAsStringAsync().Result;
                regresar = respuesta; //respuesta.Replace('"', '-');
                System.Diagnostics.Debug.WriteLine("RESPUESTA EFECTIVO: " + respuesta);

                totalpedido = 0;
                noProductos = 0;
                MessagingCenter.Send<CarritoViewModel, int>(this, "BadgeCero", 0);

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
                    MessagingCenter.Send<CarritoViewModel, int>(this, "BadgeCero", 0);
                }


                //fin

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return regresar;
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
                
                if (respuesta=="1")
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

        public async Task<string> AplicaMonedero(string monedero)
        {
            string respuesta = "";
            try
            {
                if (totalpedido> Convert.ToDouble(monedero))
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
                else
                {
                    respuesta = "El monedero supera el importe total, ingresa una cantidad inferior.";
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return respuesta;
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

        public async Task<PedidoTemporal> GetPedidos()
        {
            try
            {
                var client = new HttpClient();

                StringContent strDescuentos = new StringContent("op=recalcularDescuentos&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var consultaDescuentos = await client.PostAsync(Constantes.url + "Pedidos/App.php", strDescuentos);
                var respuestaDescuentos = consultaDescuentos.Content.ReadAsStringAsync().Result.Trim();
                

                
                StringContent str = new StringContent("op=SelectPedidosTemporalOpenPay&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
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

                //Obtiene datos de variables iniciales

                var client2 = new HttpClient();
                StringContent str2 = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta2 = await client2.PostAsync(Constantes.url + "Pedidos/App.php", str2);
                var json2 = respuesta2.Content.ReadAsStringAsync().Result.Trim();

                var obj = JObject.Parse(json2);
                string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                string displayMonedero = (string)obj.SelectToken("Monedero");
                string displayEnvios = (string)obj.SelectToken("EnvioGratis");
                int displayAbandono = (int)obj.SelectToken("Abandonados");
                Abandonados = displayAbandono;
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

                if (EnviosGratis == "0")
                {
                    doubleEnvio = 0;
                    TotalCostoEnvio = Convert.ToDouble("0").ToString("c");
                }
                else
                {
                    if (Application.Current.Properties.ContainsKey("CostoEnvio"))
                    {
                        if (!Application.Current.Properties["CostoEnvio"].Equals(""))
                        {
                            doubleEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString());
                            TotalCostoEnvio = Convert.ToDouble(Application.Current.Properties["CostoEnvio"].ToString()).ToString("c");

                        }
                    }

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
