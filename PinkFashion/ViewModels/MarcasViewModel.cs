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
    public class MarcasViewModel : InsigniaViewModel
    {
        json_object json_ob = new json_object();

        Familia familia;
        Categoria_ categoria;

        public ObservableCollection<Marcas_> Marcas { get; set; }

        public Command LoadMarcasCommand { get; set; }

        public MarcasViewModel(Categoria_ categoria)
        {
            this.categoria = categoria;
            Marcas = new ObservableCollection<Marcas_>();

            LoadMarcasCommand = new Command(async () =>
            {
                await ExecuteLoadMarcasCommand();
            });
        }

        public MarcasViewModel(Familia familia)
        {
            this.familia = familia;
            Marcas = new ObservableCollection<Marcas_>();

            LoadMarcasCommand = new Command(async () =>
            {
                await ExecuteLoadMarcasCommand();
            });
        }

        async Task ExecuteLoadMarcasCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Marcas.Clear();
                IEnumerable<Marcas_> marcas = null;
                List<Marcas_> lista = new List<Marcas_>();
                await GetMarcas().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });


                marcas = lista;

                foreach (var item in marcas)
                {
                    Marcas.Add(item);
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

        public ICommand ItemTappedCommand
        {

            get
            {
                return new Command<Marcas_>(async (Marcas_ model) =>
                {
                    MessagingCenter.Send<MarcasViewModel, string>(this, "idMarca", model.IdMarca);
                    await App.Current.MainPage.Navigation.PopModalAsync();
                });
            }
        }

        public async Task<Marcas_[]> GetMarcas()
        {
            try
            {
                var client = new HttpClient();
                StringContent str;
                if(this.familia != null)
                {
                    str = new StringContent("op=marcas&idFamilia=" + this.familia.id_clasificacion, Encoding.UTF8, "application/x-www-form-urlencoded");
                } else
                {
                    str = new StringContent("op=marcas&idFamilia=" + this.categoria.IdCategoria, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
                var respuesta = await client.PostAsync(Constantes.url + "Listas/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("Marcas: " + json);


                if (json != "")
                {
                    json_ob = JsonConvert.DeserializeObject<json_object>(json);
                }
                else
                {
                    return json_ob.marcas = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_ob.marcas;

        }

        public class json_object
        {
            [JsonProperty("ListadoMarcas")]
            public Marcas_[] marcas { get; set; }

        }
    }
}
