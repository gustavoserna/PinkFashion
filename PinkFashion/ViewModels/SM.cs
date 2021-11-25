using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PinkFashion.Views;
using Xamarin.Forms;
using PinkFashion.Models;

namespace PinkFashion.ViewModels
{
    public class SM : InsigniaViewModel
    {
        public static Datos datosPerfil = new Datos();
        string clave = string.Empty;

        public async Task iniciarSesion(string correo, string pass)
        {
            try
            {
                string vFechaNac = "";
                var client = new HttpClient();
                StringContent str = new StringContent("op=login&username=" + correo + "&password=" + pass, Encoding.UTF8, "application/x-www-form-urlencoded");
                var consulta = await client.PostAsync(new Uri(Constantes.url + "Sesion/App.php"), str);
                var json = consulta.Content.ReadAsStringAsync().Result.Trim();

                System.Diagnostics.Debug.WriteLine("perfil login:  " + json);

                JArray obj = JArray.Parse(json);
                Perfil perfil = new Perfil();
                perfil.IdCliente = obj[0]["IdCliente"].ToString();
                perfil.NIT = obj[0]["NIT"].ToString();
                perfil.Nombre = obj[0]["Nombre"].ToString();
                perfil.Apellidos = obj[0]["Apellidos"].ToString();
                perfil.clave = obj[0]["Clave"].ToString();
                perfil.Cuenta = obj[0]["correo"].ToString();
                perfil.Genero = obj[0]["Genero"].ToString();
                perfil.EsBlogger = obj[0]["EsBlogger"].ToString();

                if (obj[0]["FechaNac"].ToString()==null || obj[0]["FechaNac"].ToString() == "")
                {
                    vFechaNac = "";
                }
                else
                {
                    
                    vFechaNac = obj[0]["FechaNac"].ToString().Substring(8, 2) + "/" + obj[0]["FechaNac"].ToString().Substring(5, 2) + "/" + obj[0]["FechaNac"].ToString().Substring(0, 4);
                }
                perfil.FechaNac = vFechaNac;
                perfil.AceptaPublicidad = obj[0]["AceptaPublicidad"].ToString();
                perfil.Encontrado = obj[0]["Encontrado"].ToString();
                await guardarDatos(perfil);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Usuario y/o contraseña incorrectos", "Ok");
                System.Diagnostics.Debug.WriteLine("Error:" + ex.Message);
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

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error:" + ex.Message);
            }

        }

        public async Task guardarDatos(Perfil perfil)
        {
            Application.Current.Properties["sesion"] = "activa";
            Application.Current.Properties["password"] = perfil.clave;
            Application.Current.Properties["IdCliente"] = perfil.IdCliente == null ? "" : perfil.IdCliente;
            Application.Current.Properties["NIT"] = perfil.NIT == null ? "" : perfil.NIT;
            Application.Current.Properties["Nombre"] = perfil.Nombre == null ? "" : perfil.Nombre;
            Application.Current.Properties["Abreviado"] = perfil.Abreviado == null ? "" : perfil.Abreviado;
            Application.Current.Properties["Apellidos"] = perfil.Apellidos == null ? "" : perfil.Apellidos;
            Application.Current.Properties["Cuenta"] = perfil.Cuenta == null ? "" : perfil.Cuenta;
            Application.Current.Properties["clave"] = perfil.clave == null ? "" : perfil.clave;
            Application.Current.Properties["FechaNac"] = perfil.FechaNac == null ? "" : perfil.FechaNac;
            Application.Current.Properties["Genero"] = perfil.Genero == null ? "" : perfil.Genero;
            Application.Current.Properties["Encontrado"] = perfil.Encontrado == null ? "" : perfil.Encontrado;
            Application.Current.Properties["AceptaPublicidad"] = perfil.AceptaPublicidad == null ? "0" : perfil.AceptaPublicidad;
            Application.Current.Properties["Blogger"] = perfil.EsBlogger == null ? "false" : perfil.EsBlogger;
            Application.Current.Properties["IDDireccion"] = "";
            Application.Current.Properties["Direccion"] = "";
            await Application.Current.SavePropertiesAsync();
            
            if (!perfil.Encontrado.Equals("vacio"))
            {
                Application.Current.MainPage = new NavigationPage(new Inicio())
                {
                    BarBackgroundColor = App.bgColor,
                    BarTextColor = App.textColor
                };

            }
            else
            {
                
                Application.Current.MainPage = new NavigationPage(new VerificarTelefono())
                {
                    BarBackgroundColor = App.bgColor,
                    BarTextColor = App.textColor
                };
            }

        }

        public async Task vaciarDatos()
        {
            Application.Current.Properties["sesion"] = "";
            Application.Current.Properties["password"] = "";
            Application.Current.Properties["IdCliente"] = "";
            Application.Current.Properties["NIT"] = "";
            Application.Current.Properties["Nombre"] = "";
            Application.Current.Properties["Abreviado"] = "";
            Application.Current.Properties["Apellidos"] = "";
            Application.Current.Properties["Cuenta"] = "";
            Application.Current.Properties["clave"] = "";
            Application.Current.Properties["Encontrado"] = "";
            Application.Current.Properties["Cobertura"] = "";
            Application.Current.Properties["FechaNac"] = "";
            Application.Current.Properties["Genero"] = "";
            Application.Current.Properties["AceptaPublicidad"] = "";
            Application.Current.Properties.Clear();
            await Application.Current.SavePropertiesAsync();
        }

        public async Task<string> RegistrarUsuario(Perfil usuarioRegistro)
        {
            var json = JsonConvert.SerializeObject(usuarioRegistro);
            System.Diagnostics.Debug.WriteLine("PERFIL->" + json);
            var cliente = new HttpClient();
            StringContent str = new StringContent("op=registro&perfil=" + json, Encoding.UTF8, "application/x-www-form-urlencoded");
            var envia = cliente.PostAsync(new Uri(Constantes.url + "Sesion/App.php"), str);
            var respuesta = await envia.Result.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine("respuesta: " + respuesta);

            return respuesta;
        }

        public async Task<string> verificarUsuario(string fbid)
        {
            string respuesta = "nada";
            try
            {

                var client = new HttpClient();
                StringContent str = new StringContent("op=BuscarFacebook&pFaceID=" + fbid, Encoding.UTF8, "application/x-www-form-urlencoded");
                var consulta = await client.PostAsync(Constantes.url + "Usuario/App.php", str);
                System.Diagnostics.Debug.WriteLine("respuesta verificacion fb: " + respuesta);
                var json = consulta.Content.ReadAsStringAsync().Result.Trim();
                if (json == "")
                {
                    respuesta = "nada";
                }
                else
                {
                    respuesta = json;
                }

            }
            catch (Exception ex) { await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok"); }

            return respuesta;
        }

        public class Datos
        {
            public Perfil perfil { get; set; }
        }
    }
}
