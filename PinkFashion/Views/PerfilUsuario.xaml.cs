using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PinkFashion.ViewModels;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class PerfilUsuario : ContentPage
    {
        string strEvento = "Perfil|Pink Fashion Store";
        public PerfilUsuario()
        {
            InitializeComponent();
            Title = "Perfil";
            App.eventTracker.SendScreen(strEvento, nameof(PerfilUsuario));
            SM sM = new SM();
            //telefono.IsEnabled = false;
            nombre.Text = Application.Current.Properties["Nombre"].ToString();
            apellidos.Text = Application.Current.Properties["Apellidos"].ToString();
            telefono.Text = Application.Current.Properties["NIT"].ToString();
            usuario.Text = Application.Current.Properties["Cuenta"].ToString();
            mail.Text = Application.Current.Properties["Cuenta"].ToString();
            if (Application.Current.Properties.ContainsKey("AceptaPublicidad"))
            {
                if ((Application.Current.Properties["AceptaPublicidad"]).ToString()=="0" || (Application.Current.Properties["AceptaPublicidad"]).ToString() == "")
                { Publicidad.IsToggled = false; }
                else
                { Publicidad.IsToggled = true; }
            }
            
            if (Application.Current.Properties.ContainsKey("FechaNac"))
            {
                FechaNac.Text = Application.Current.Properties["FechaNac"].ToString();
            }
            
            if (Application.Current.Properties.ContainsKey("Genero"))
            {
                Genero.SelectedItem = Application.Current.Properties["Genero"].ToString();
            }
            

            var actualizar = new ToolbarItem();
            actualizar.Text = "Actualizar";
            actualizar.Command = new Command(async o =>
            {
                cont.IsVisible = false;
                loader.IsVisible = true;
                await Actualizar();
                Application.Current.Properties["Nombre"] = nombre.Text;
                Application.Current.Properties["Apellidos"] = apellidos.Text;
                Application.Current.Properties["FechaNac"] = FechaNac.Text;
                Application.Current.Properties["Genero"] = Genero.SelectedItem;
                await Application.Current.SavePropertiesAsync();
                cont.IsVisible = true;
                loader.IsVisible = false;
                await DisplayAlert("Listo", "Perfil actualizado", "Ok");
            });

            ToolbarItems.Add(actualizar);

            var clickLogout = new TapGestureRecognizer();
            clickLogout.Tapped += async (s, e) =>
            {
                await sM.vaciarDatos();
                Application.Current.MainPage = new NavigationPage(new Inicio())
                {
                    BarBackgroundColor = App.bgColor,
                    BarTextColor = App.textColor
                };
            };
            btnlogout.GestureRecognizers.Add(clickLogout);

            var clickClave = new TapGestureRecognizer();
            clickClave.Tapped += async (s, e) =>
            {
                cont.IsVisible = false;
                loader.IsVisible = true;

                if (claveActual.Text.Equals(Application.Current.Properties["clave"]))
                {
                    if (claveNueva.Text.Equals(claveRepetir.Text))
                    {
                        await ActualizarClave();
                        claveActual.Text = "";
                        claveNueva.Text = "";
                        claveRepetir.Text = "";
                        await DisplayAlert("Listo", "Contraseña actualizada", "Ok");
                    }
                    else
                        await DisplayAlert("Error", "Las contraseñas no coinciden", "Ok");
                }
                else
                    await DisplayAlert("Error", "Contraseña actual incorrecta", "Ok");

                cont.IsVisible = true;
                loader.IsVisible = false;
            };
            btnClave.GestureRecognizers.Add(clickClave);
        }

        public async Task Actualizar()
        {
            string vFechaNac = "";
            if (FechaNac.Text == null || FechaNac.Text == "")
            {
                vFechaNac = "";
            }
            else
            {
                vFechaNac = FechaNac.Text.Substring(6, 4) + "-" + FechaNac.Text.Substring(3, 2) + "-" + FechaNac.Text.Substring(0, 2);
            }
            
            var client = new HttpClient();
            StringContent str = new StringContent("op=actualizarPerfil&IDCliente=" + Application.Current.Properties["IdCliente"] + "&Nombre=" + nombre.Text + "&Apellidos=" + apellidos.Text + "&Telefono=" + telefono.Text + "&FechaNac=" + vFechaNac + "&Genero=" + Genero.SelectedItem, Encoding.UTF8, "application/x-www-form-urlencoded");
            await client.PostAsync(Constantes.url + "Sesion/App.php", str);


        }

        public async Task ActualizarClave()
        {

            var client = new HttpClient();
            StringContent str = new StringContent("op=UpdateClientePassword&IDCliente=" + Application.Current.Properties["IdCliente"] + "&passActual=" + claveActual.Text + "&passNuevo=" + claveNueva.Text + "&passNuevoValida=" + claveRepetir.Text, Encoding.UTF8, "application/x-www-form-urlencoded");
            await client.PostAsync(Constantes.url + "Sesion/App.php", str);
            Application.Current.Properties["clave"] = claveNueva.Text;
            await Application.Current.SavePropertiesAsync();
        }
    }
}
