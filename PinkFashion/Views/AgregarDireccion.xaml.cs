using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PinkFashion.ViewModels;
using Xamarin.Forms;
using PinkFashion.Models;


namespace PinkFashion.Views
{
    public partial class AgregarDireccion : ContentPage
    {
        string tipo = "";
        string strEvento = "Direccón|Pink Fashion Store";

        public AgregarDireccion(string tipo = "push")
        {
            InitializeComponent();
            this.tipo = tipo;
            this.BindingContext = new MisDireccionesViewModel();
            lblTitulo.TextColor = App.textColor;
            lblCerrar.TextColor = App.textColor;
            lblAgregar.TextColor = App.textColor;

            if (tipo.Equals("push"))
            {
                NavigationPage.SetTitleView(this, null);
                gridHeader.IsVisible = false;
                Title = "Agregar Dirección";
                var listo = new ToolbarItem();
                listo.Text = "Agregar";
                listo.Command = new Command(async o =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        cont.IsVisible = false;
                        loader.IsVisible = true;
                    });
                    NavigationPage.SetHasNavigationBar(this, false);

                    await Agregar();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        cont.IsVisible = true;
                        loader.IsVisible = false;
                    });
                    NavigationPage.SetHasNavigationBar(this, true);
                });
                ToolbarItems.Add(listo);
            }
            else
            {
                gridHeader.IsVisible = true;
                var clickCerrar = new TapGestureRecognizer();
                clickCerrar.Tapped += (s, e) =>
                {
                    Navigation.PopModalAsync();
                };
                cerrar.GestureRecognizers.Add(clickCerrar);

                var clickAgregar = new TapGestureRecognizer();
                clickAgregar.Tapped += async (s, e) =>
                {
                    await Agregar();
                };
                btnAceptar.GestureRecognizers.Add(clickAgregar);

                
            }
            var clickAgregarEstado = new TapGestureRecognizer();
            clickAgregarEstado.Tapped += async (s, e) =>
            {
                await Navigation.PushModalAsync(new SeleccionaEstados(this));
            };
            SelEstado.GestureRecognizers.Add(clickAgregarEstado);

            var clickAgregarMunicipio = new TapGestureRecognizer();
            clickAgregarMunicipio.Tapped += async (s, e) =>
            {
                if (Application.Current.Properties.ContainsKey("IDEstado"))
                {
                    if (!Application.Current.Properties["IDEstado"].Equals(""))
                    {
                        await Navigation.PushModalAsync(new SeleccionaMunicipios(this, idEstado.Text));

                    }
                    else
                    {
                        await DisplayAlert("Precaución", "Es necesario introduccir el Estado", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Precaución", "Es necesario introduccir el Estado", "Ok");
                }

            };
            SelMunicipio.GestureRecognizers.Add(clickAgregarMunicipio);
        }

        public bool quitarselEstado = false;
        public void QuitarSelEstado()
        {
            if (Application.Current.Properties.ContainsKey("IDEstado"))
            {
                if (!Application.Current.Properties["IDEstado"].Equals(""))
                {
                    idEstado.Text = Application.Current.Properties["IDEstado"].ToString();
                    estado.Text = Application.Current.Properties["Estado"].ToString();

                }
            }
            quitarselEstado = true;

        }

        public bool quitarselMunicipio = false;
        public void QuitarSelMunicipio()
        {
            if (Application.Current.Properties.ContainsKey("IDMunicipio"))
            {
                if (!Application.Current.Properties["IDMunicipio"].Equals(""))
                {
                    idMunicipio.Text = Application.Current.Properties["IDMunicipio"].ToString();
                    municipio.Text = Application.Current.Properties["Municipio"].ToString();

                }
            }
            quitarselMunicipio = true;

        }

        public async Task InsertarDireccion(Direccion pDireccion)
        {
            try
            {
                var item = JsonConvert.SerializeObject(pDireccion);
                System.Diagnostics.Debug.WriteLine(item);
                var client = new HttpClient();
                StringContent str = new StringContent("op=agregarDireccion&item=" + item, Encoding.UTF8, "application/x-www-form-urlencoded");
                await client.PostAsync(Constantes.url + "Sesion/App.php", str);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error al registrar los datos, inténtalo de nuevo mas tarde", "Ok");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }

        public async Task Agregar()
        {
            try
            {
                if (!idEstado.Text.Equals("") && !idMunicipio.Text.Equals(""))
                {
                    
                    if (!string.IsNullOrEmpty(calle.Text) && !string.IsNullOrEmpty(noext.Text) && !string.IsNullOrEmpty(cp.Text) && !string.IsNullOrEmpty(colonia.Text))
                    {
                        Direccion vDireccion = new Direccion();
                        vDireccion.Alias = alias.Text;
                        vDireccion.IdCliente = Application.Current.Properties["IdCliente"].ToString();
                        vDireccion.Calle = calle.Text;
                        vDireccion.NoExt = noext.Text;
                        vDireccion.NoInt = noint.Text == null ? "" : noint.Text;
                        vDireccion.CodigoPostal = cp.Text;
                        vDireccion.Colonia = colonia.Text;
                        vDireccion.IdMunicipio = Convert.ToInt32(idMunicipio.Text);
                        vDireccion.IdEstado = Convert.ToInt32(idEstado.Text); 
                        vDireccion.Instrucciones = instrucciones.Text;

            
                        await InsertarDireccion(vDireccion);
                        if (tipo.Equals("push"))
                            await Navigation.PopAsync();
                        else
                            await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Precaución", "Es necesario introduccir los datos necesarios para su envío", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Precaución", "Es necesario introduccir la Ciudad y Estado", "Ok");
                }


            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error al registrar los datos, inténtalo de nuevo mas tarde", "Ok");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
