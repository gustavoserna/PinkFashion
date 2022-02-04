using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    class MyTabbedPageViewModel : BaseViewModel
    {
        int _badge = 0;

        public ICommand LoadingCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await this.Contador();
                });
            }
        }

        public async Task Contador()
        {
            try
            {
                var client = new HttpClient(new HttpClientHandler
                {
                    UseProxy = false
                });
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Keep-Alive", "600");
                StringContent str = new StringContent("op=getTotalPedidoTemporal&pIDCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Pedidos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();

                var obj = JObject.Parse(json);
                string displayNoProductos = (string)obj.SelectToken("NoArticulos");
                string displayIDAlianza = (string)obj.SelectToken("IDAlianza");

                if (!string.IsNullOrEmpty(displayNoProductos))
                {
                    //App.Cart = Int32.Parse(displayNoProductos);
                    //inicioViewModel.noProductos = App.Cart;
                    this.Badge = Int32.Parse(displayNoProductos);
                }
                else
                {
                    //App.Cart = 0;
                    //inicioViewModel.noProductos = 0;
                    this.Badge = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error:" + ex.Message);
            }
        }

        public int Badge
        {
            get { return _badge; }
            set { SetProperty(ref _badge, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName] string propertyName = "",
        Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public MyTabbedPageViewModel()
        {
            _badge = 0;
            MessagingCenter.Subscribe<CarritoViewModel, int>(this, "Badge", async (sender, arg) =>
            {
                /*if (!(arg == -1 && Badge <= 0))
                {
                    Badge = Badge + (arg);
                }*/
                await this.Contador();
            });
            MessagingCenter.Subscribe<CarritoViewModel, int>(this, "BadgeCero", (sender, arg) =>
            {
                Badge = 0;
            });
            MessagingCenter.Subscribe<ProductoViewModel, int>(this, "Badge", (sender, arg) =>
            {
                Badge = arg;
            });
        }
    }
}
