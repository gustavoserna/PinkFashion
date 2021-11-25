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
    public class MisDireccionesViewModel : BaseViewModel
    {
        public ObservableCollection<Direccion> Items { get; set; }

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

        public MisDireccionesViewModel()
        {
            Items = new ObservableCollection<Direccion>();

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
                IEnumerable<Direccion> direcciones = null;
                List<Direccion> lista = new List<Direccion>();

                await GetDirecciones().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });

                direcciones = lista;

                foreach (var item in direcciones)
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

        public async Task BorrarDireccion(Direccion dir)
        {
            var client = new HttpClient();
            StringContent str = new StringContent("op=eliminarDireccion&id=" + dir.IdClienteDir, Encoding.UTF8, "application/x-www-form-urlencoded");
            await client.PostAsync(Constantes.url + "Sesion/App.php", str);
        }

        public async Task<Direccion[]> GetDirecciones()
        {

            System.Diagnostics.Debug.WriteLine("entramos");
            var client = new HttpClient();
            StringContent str = new StringContent("op=getDirecciones&IdCliente=" + Application.Current.Properties["IdCliente"], Encoding.UTF8, "application/x-www-form-urlencoded");
            var respuesta = await client.PostAsync(Constantes.url + "Sesion/App.php", str);
            var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
            System.Diagnostics.Debug.WriteLine("direcciones: " + json);
            if (json != "")
            {
                json_ob = JsonConvert.DeserializeObject<json_object>(json);
            }
            else
            {
                return json_ob.direcciones = null;
            }

            return json_ob.direcciones;
        }

        public class json_object
        {
            [JsonProperty("DireccionesCliente")]
            public Direccion[] direcciones { get; set; }
        }
    }
}
