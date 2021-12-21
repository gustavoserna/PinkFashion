using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using PinkFashion.Models;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    public class MetodoPagoViewModel : BaseViewModel
    {
        public ObservableCollection<Card> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        json_object json_ob = new json_object();

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

        public MetodoPagoViewModel()
        {
            Items = new ObservableCollection<Card>();

            LoadItemsCommand = new Command(async () =>
            {
                await ExecuteLoadItemsCommand();
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                IEnumerable<Card> tarjetas = null;
                List<Card> lista = new List<Card>();

                await GetTarjetas().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });

                tarjetas = lista;

                foreach (var item in tarjetas)
                {
                    Items.Add(item);
                }

                if (Items.Count == 0)
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

        public ICommand BorrarCommand
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

        public async Task<Card[]> GetTarjetas()
        {
            var client = new HttpClient();
            StringContent str = new StringContent("op=getTarjetasOpenPay&IdCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
            var respuesta = await client.PostAsync(Constantes.url + "Sesion/App.php", str);
            var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
            System.Diagnostics.Debug.WriteLine("Tarjetas: " + json);

            if (json != "")
            {
                json_ob = JsonConvert.DeserializeObject<json_object>(json);
            }
            else
            {
                return json_ob.cards = null;
            }

            return json_ob.cards;
        }

        public class json_object
        {
            [JsonProperty("EntidadMetodoPago")]
            public Card[] cards { get; set; }
        }
    }
}
