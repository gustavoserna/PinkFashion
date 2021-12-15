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
using Newtonsoft.Json.Linq;

namespace PinkFashion.ViewModels
{
    public class ProductoViewModel : InsigniaViewModel
    {
        INavigation Navigation;

        Producto_ producto_;

        public Producto_ Productos { get; set; }

        public ObservableCollection<ColeccionProductos> ColRelacionados { get; set; }
        public ObservableCollection<ColeccionVariantes> ColVariantes { get; set; }

        public Command LoadProductosCommand { get; set; }

        json_object json_ob = new json_object();

        int _cantidad = 0;
        public int Cantidad
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

        int _EntCantidad = 1;
        public int EntCantidad
        {
            get
            {
                return _EntCantidad;
            }
            set
            {
                SetProperty(ref _EntCantidad, value);
            }
        }

        string _Marca = "";
        public string marca
        {
            get
            {
                return _Marca;
                ;
            }
            set
            {
                SetProperty(ref _Marca, value);
            }
        }

        string _Producto = "";
        public string producto
        {
            get
            {
                return _Producto;
            }
            set
            {
                SetProperty(ref _Producto, value);
            }
        }

        string _Idvariantes_producto = "";
        public string idvariantes_producto
        {
            get
            {
                return _Idvariantes_producto;
            }
            set
            {
                SetProperty(ref _Idvariantes_producto, value);
            }
        }

        string _Variantes_producto = "";
        public string Variantes_producto
        {
            get
            {
                return _Variantes_producto;
            }
            set
            {
                SetProperty(ref _Variantes_producto, value);
            }
        }

        string _Info = "";
        public string descripcion
        {
            get
            {
                return _Info;
            }
            set
            {
                SetProperty(ref _Info, value);
            }
        }

        string _Imagen = "";
        public string Imagen
        {
            get
            {
                return _Imagen;
            }
            set
            {
                SetProperty(ref _Imagen, value);
            }
        }

        string _Agotado = "False";
        public string Agotado
        {
            get
            {
                return _Agotado;
            }
            set
            {
                SetProperty(ref _Agotado, value);
            }
        }

        string _Precio;
        public string Precio
        {
            get
            {
                return _Precio;
            }
            set
            {
                SetProperty(ref _Precio, value);
            }
        }

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

        public ProductoViewModel(Producto_ producto, INavigation navigation)
        {
            Navigation = navigation;
            producto_ = producto;
            Productos = new Producto_();
            Productos.VariantesProductos = new ObservableCollection<VariantesProducto>();
            Productos.ProductosRelacionados = new ObservableCollection<Producto_>();
            ColVariantes = new ObservableCollection<ColeccionVariantes>();
            ColRelacionados = new ObservableCollection<ColeccionProductos>();

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
                ColVariantes.Clear();
                List<VariantesProducto> listavariantes_for_col = new List<VariantesProducto>();

                Productos.VariantesProductos.Clear();
                IEnumerable<VariantesProducto> variantes = null;
                List<VariantesProducto> lista = new List<VariantesProducto>();

                ColRelacionados.Clear();
                List<Producto_> listarelacionados_for_col = new List<Producto_>();

                Productos.ProductosRelacionados.Clear();
                IEnumerable<Producto_> relacionados = null;
                List<Producto_> listarelaciones = new List<Producto_>();

                await GetProductos(producto_.idproducto).ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        marca = t.Result.marca;
                        _Marca = t.Result.marca;
                        Imagen = t.Result.Imagen;
                        Agotado = t.Result.Agotado;
                        descripcion = t.Result.descripcion;
                        producto = t.Result.producto;
                        Precio = t.Result.Precio;
                        foreach(VariantesProducto variante in t.Result.VariantesProductos)
                        {
                            lista.Add(variante);
                            listavariantes_for_col.Add(variante);
                        }
                        foreach(Producto_ producto in t.Result.ProductosRelacionados)
                        {
                            listarelaciones.Add(producto);
                            listarelacionados_for_col.Add(producto);
                        }

                    }
                });

                //carousel variantes
                double totalSlidesVariantes = Math.Ceiling((Double)listavariantes_for_col.Count / 2);
                for (int i = 0; i < totalSlidesVariantes; i++)
                {
                    ColeccionVariantes coleccion = new ColeccionVariantes();
                    coleccion.variantes = new List<VariantesProducto>();

                    for (int k = 0; k <= listavariantes_for_col.Count; k++)
                    {
                        if (k < 2 && listavariantes_for_col.Count > 0)
                        {
                            coleccion.variantes.Add(listavariantes_for_col[0]);
                            listavariantes_for_col.RemoveAt(0);
                        }
                        else
                        {
                            break;
                        }
                    }

                    ColVariantes.Add(coleccion);
                }

                //carousel relacionados
                double totalSlidesRelacionados = Math.Ceiling((Double)listarelacionados_for_col.Count / 2);
                for (int i = 0; i < totalSlidesRelacionados; i++)
                {
                    ColeccionProductos coleccion = new ColeccionProductos();
                    coleccion.productos = new List<Producto_>();

                    for (int k = 0; k <= listarelacionados_for_col.Count; k++)
                    {
                        if (k < 2 && listarelacionados_for_col.Count > 0)
                        {
                            coleccion.productos.Add(listarelacionados_for_col[0]);
                            listarelacionados_for_col.RemoveAt(0);
                        }
                        else
                        {
                            break;
                        }
                    }

                    ColRelacionados.Add(coleccion);
                }

                variantes = lista;
                relacionados = listarelaciones;
                int cuentaVariante = 0;
                foreach (var item in variantes)
                {
                    Productos.VariantesProductos.Add(item);
                    if(cuentaVariante==0)
                    {
                        idvariantes_producto = item.idvariantes_producto;
                        Variantes_producto = item.descripcion_variante;
                        Imagen = item.Imagen_Variante;
                        Agotado = item.Agotado;
                    }
                    cuentaVariante++;
                }

                foreach (var itemrel in relacionados)
                {
                    Productos.ProductosRelacionados.Add(itemrel);
                }

                /*
                if (Productos.Count == 0)

                    nodisponible = true;
                else
                    nodisponible = false;
                */
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

        public async Task Enviar()
        {
            ProductoTemporal pedido = new ProductoTemporal();
            pedido.Cantidad = Convert.ToDouble(EntCantidad);
            pedido.Precio = producto_.precioCarritoDouble;
            pedido.ConVariante = producto_.ConVariante;
            pedido.IdVariante_Producto = idvariantes_producto;
            pedido.IdCliente = Application.Current.Properties["IdCliente"].ToString();
            pedido.IdProducto = producto_.idproducto;
            pedido.FechaHora = "";

            string respuesa = await addProdPedidoTmp(pedido);
            if (respuesa == "1")
            {
                await App.Current.MainPage.DisplayAlert("Listo", "Producto agregado al carrito", "Ok");
            }
            else
            {
                if (respuesa == "")
                {
                    await App.Current.MainPage.DisplayAlert("Precaución", "Verifica la existencia", "Ok");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Precaución", respuesa, "Ok");
                }

            }
        }

        public ICommand AgregaCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                    {
                        if (Application.Current.Properties["sesion"].Equals("activa"))
                        {
                            noProductos += EntCantidad;
                            MessagingCenter.Send<ProductoViewModel, int>(this, "Badge", noProductos);
                            await Enviar();
                        }
                        else
                        {
                            bool ac = await App.Current.MainPage.DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                            if (ac)
                            {
                                await Navigation.PushModalAsync(new Login());
                            }
                        }
                    }
                    else
                    {
                        bool ac = await App.Current.MainPage.DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await Navigation.PushModalAsync(new Login());
                        }
                    }

                });
            }
        }

        public ICommand MasCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                    {
                        EntCantidad++;
                    }
                    else
                    {
                        bool ac = await App.Current.MainPage.DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await App.Current.MainPage.Navigation.PushModalAsync(new Login());


                        }
                    }
                });
            }
        }

        public ICommand MenosCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                    {
                        if (EntCantidad > 0)
                        {
                            EntCantidad--;
                        }
                        if (EntCantidad == 0)
                        {
                            EntCantidad = 0;
                        }
                    }
                    else
                    {
                        bool ac = await App.Current.MainPage.DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await App.Current.MainPage.Navigation.PushModalAsync(new Login());


                        }
                    }
                });
            }
        }

        public ICommand SelVarianteCommand
        {
            get
            {
                return new Command<VariantesProducto>((VariantesProducto model) =>
                {
                    idvariantes_producto = model.idvariantes_producto;
                    Variantes_producto = model.descripcion_variante;
                    Imagen = model.Imagen_Variante;
                    Agotado = model.Agotado;
                });
            }
        }

        public ICommand SelRelacionadoCommand
        {
            get
            {
                return new Command<Producto_>(async (Producto_ model) =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new Producto(model));

                });
            }
        }


        public async Task DeletePedidoTemporal()
        {
            _loader = true;
            _cont = false;
            var client = new HttpClient();
            StringContent str = new StringContent("op=DeletePedidosTemporales&idCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
            await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
            _loader = false;
            _cont = true;
            //App.Cart = 0;
            noProductos = 0;
            
        }

        public async Task ModPedidoTmp(Producto_ vproducto)
        {
            try
            {
                
                ProductoTemporal temporal = new ProductoTemporal();                
                temporal.IdCliente = Application.Current.Properties["IdCliente"].ToString();
                temporal.IdProducto = producto_.idproducto;
                temporal.IdVariante_Producto = vproducto.idvariantes_producto;
                temporal.Cantidad = vproducto.Cantidad;
                temporal.Precio = vproducto.precioDouble;
                temporal.Total = vproducto.ImporteDouble;
                var item = JsonConvert.SerializeObject(temporal);
                System.Diagnostics.Debug.WriteLine(item);
                var client = new HttpClient();
                StringContent str = new StringContent("op=UpdatePedidoTemporalSP&pedido=" + item, Encoding.UTF8, "application/x-www-form-urlencoded");
                await client.PostAsync(Constantes.url + "Pedidos/App.php", str);

                //InsertPedidoTemporal
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public async Task<string> addPedidoTmp(Producto_ vproducto)
        {
            string idPedidoTmp = "";
            try
            {
                ProductoTemporal temporal = new ProductoTemporal();
                temporal.IdCliente = Application.Current.Properties["IdCliente"].ToString();
                temporal.IdProducto = producto_.idproducto;
                temporal.IdVariante_Producto = vproducto.idvariantes_producto;
                temporal.Cantidad = vproducto.Cantidad;
                temporal.Precio = vproducto.precioDouble;
                temporal.Total = vproducto.ImporteDouble;
                var item = JsonConvert.SerializeObject(temporal);
                System.Diagnostics.Debug.WriteLine(item);
                var client = new HttpClient();
                StringContent str = new StringContent("op=InsertPedidoTemporalSP&pedido=" + item, Encoding.UTF8, "application/x-www-form-urlencoded");
                var enviar = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var respuesta = enviar.Content.ReadAsStringAsync().Result.ToString();
                idPedidoTmp = respuesta;
                System.Diagnostics.Debug.WriteLine("idPedidoTmp: " + idPedidoTmp);

                //App.Cart++;
                
                //noProductos = App.Cart;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return idPedidoTmp;
        }

        public async Task<string> addProdPedidoTmp(ProductoTemporal producto)
        {
            string idPedidoTmp = "";
            try
            {
                var item = JsonConvert.SerializeObject(producto);
                System.Diagnostics.Debug.WriteLine(item);
                var client = new HttpClient();
                StringContent str = new StringContent("op=insertaTemporal&pedido=" + item, Encoding.UTF8, "application/x-www-form-urlencoded");
                var consulta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var respuesta = consulta.Content.ReadAsStringAsync().Result.Trim();
                idPedidoTmp = respuesta;

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

            return idPedidoTmp;
        }

        public async Task ModProdPedidoTmp(ProductoTemporal producto)
        {
            try
            {

                var item = JsonConvert.SerializeObject(producto);
                System.Diagnostics.Debug.WriteLine(item);
                var client = new HttpClient();
                StringContent str = new StringContent("op=UpdatePedidoTemporal&pedido=" + item, Encoding.UTF8, "application/x-www-form-urlencoded");
                await client.PostAsync(Constantes.url + "Pedidos/App.php", str);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public async Task DelProdPedidoTmp(ProductoTemporal producto)
        {
            try
            {
                var item = JsonConvert.SerializeObject(producto);
                System.Diagnostics.Debug.WriteLine(item);
                var client = new HttpClient();
                StringContent str = new StringContent("op=DeleteProductoTemporal&pedido=" + item, Encoding.UTF8, "application/x-www-form-urlencoded");
                await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
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
                    json_ob = JsonConvert.DeserializeObject<json_object>(json);
                }
                else
                {
                    return json_ob.productos[0] = null;
                }

                return json_ob.productos[0];

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_ob.productos[0];

        }

        public class json_object
        {
            [JsonProperty("EntProducto")]
            public Producto_[] productos { get; set; }

        }
    }
}
