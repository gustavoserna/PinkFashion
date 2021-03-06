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
        public Producto_ Productos { get; set; }

        public Command LoadProductosCommand { get; set; }

        json_object json_ob = new json_object();

        string id_producto;

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

        public ProductoViewModel(string id_producto)
        {
            this.id_producto = id_producto;
            Productos = new Producto_();
            Productos.VariantesProductos = new ObservableCollection<VariantesProducto>();
            Productos.ProductosRelacionados = new ObservableCollection<Producto_>();
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
                Productos.VariantesProductos.Clear();
                IEnumerable<VariantesProducto> variantes = null;
                List<VariantesProducto> lista = new List<VariantesProducto>();

                Productos.ProductosRelacionados.Clear();
                IEnumerable<Producto_> relacionados = null;
                List<Producto_> listarelaciones = new List<Producto_>();

                await GetProductos(id_producto).ContinueWith(t =>
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
                        for (int i = 0; i < t.Result.VariantesProductos.Count; i++)
                        {
                            lista.Add(t.Result.VariantesProductos[i]);
                        }
                        for (int i = 0; i < t.Result.ProductosRelacionados.Count; i++)
                        {
                            listarelaciones.Add(t.Result.ProductosRelacionados[i]);
                        }

                    }
                });

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

        public ICommand AgregaCommand
        {
            get
            {
                return new Command<Producto_>(async (Producto_ model) =>
                {
                    model.Cantidad = 1;
                    model.ImporteDouble = model.ImporteDouble + model.precioDouble;
                    await addPedidoTmp(model);
                });
            }
        }

        public ICommand MasCommand
        {
            get
            {
                return new Command<Producto_>(async (Producto_ model) =>
                {
                    int cant = Int32.Parse(model.Cantidad.ToString());
                    cant++;
                    model.Cantidad = cant;
                    model.ImporteDouble = model.ImporteDouble + model.precioDouble;
                    await ModPedidoTmp(model);
                });
            }
        }

        public ICommand MenosCommand
        {
            get
            {
                return new Command<Producto_>(async (Producto_ model) =>
                {
                    int cant = Int32.Parse(model.Cantidad.ToString());
                    if (cant == 0)
                    {
                        //App.Cart = 0;
                        noProductos = 0;
                        await ExecuteLoadProductosCommand();
                    }
                    if (cant > 0)
                    {
                        cant--;
                        model.Cantidad = cant;
                        model.ImporteDouble = model.ImporteDouble - model.precioDouble;
                        await ModPedidoTmp(model);

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
                temporal.IdProducto = id_producto;
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
                temporal.IdProducto = id_producto;
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
