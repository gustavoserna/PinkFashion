using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using PinkFashion.ViewModels;
using PinkFashion.Models;
using PinkFashion.Views;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    public class ElegirDireccionViewModel : BaseViewModel
    {
        public ObservableCollection<Direccion> Direcciones { get; set; }
        public Command LoadDireccionesCommand { get; set; }
        json_object json_ob = new json_object();
       
        public ElegirDireccionViewModel()
        {
            Direcciones = new ObservableCollection<Direccion>();

            LoadDireccionesCommand = new Command(async () =>
            {
                await ExecuteLoadDireccionesCommand();
            });
        }

        async Task ExecuteLoadDireccionesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Direcciones.Clear();
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
                    Direcciones.Add(item);
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
