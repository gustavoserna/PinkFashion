using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PinkFashion.Models;
using PinkFashion.ViewModels;
//using Plugin.FirebaseAnalytics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class Registro : ContentPage
    {
        Perfil usuarioRegistroFB = null;
        string strEvento = "Registro|Pink Fashion Store";
        public Registro()
        {
            InitializeComponent();
            Title = "Crear cuenta";
            App.eventTracker.SendScreen(strEvento, nameof(Registro));
            this.usuarioRegistroFB = usuarioRegistroFB;

            var clickTos = new TapGestureRecognizer();
            clickTos.Tapped += async (s, e) =>
            {
                await Launcher.OpenAsync("https://pinkfashionstore.com/terminoscondiciones.php");
            };
            tos.GestureRecognizers.Add(clickTos);

            if (usuarioRegistroFB == null)
            {
                contNombre.IsVisible = true;
                contApellidos.IsVisible = true;
                contCorreo.IsVisible = true;
                contClave.IsVisible = true;
                contClave2.IsVisible = true;
                //contTelefono.IsVisible = true;
            }
            else
            {
                if (usuarioRegistroFB.Nombre.Equals(""))
                {
                    contNombre.IsVisible = true;
                }

                if (usuarioRegistroFB.Apellidos.Equals(""))
                {
                    contApellidos.IsVisible = true;
                }

                if (usuarioRegistroFB.correo.Equals(""))
                {
                    contCorreo.IsVisible = true;
                }

                if (usuarioRegistroFB.Telefono.Equals(""))
                {
                    //contTelefono.IsVisible = true;
                }
                nombre.Text = usuarioRegistroFB.Nombre;
                apellidos.Text = usuarioRegistroFB.Apellidos;
                mail.Text = usuarioRegistroFB.correo;
                pass1.Text = usuarioRegistroFB.clave;
                pass2.Text = usuarioRegistroFB.clave;
            }


            var clickCrear = new TapGestureRecognizer();
            clickCrear.Tapped += async (s, e) =>
            {
                if (mail.Text.Equals("") || pass1.Text.Equals("") || pass2.Text.Equals("") || nombre.Text.Equals("") || apellidos.Text.Equals("") || /*telefono.Text.Equals("") ||*/ !switchT.IsToggled)
                {
                    await DisplayAlert("Error", "Tienes que llenar todos los campos o aceptar terminos y condiciones", "Ok");
                }
                else
                {
                    try
                    {
                        //Valida que el formato del correo sea valido
                        bool isEmail = Regex.IsMatch(mail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                        if (!isEmail)
                        {
                            await this.DisplayAlert("Advertencia", "El formato del correo electrónico es incorrecto, revíselo e intente de nuevo.", "OK");
                        }
                        else
                        {
                            await enviarUsuario();
                        }

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                        await DisplayAlert("Error", "Ocurrió un error, inténtalo de nuevo mas tarde.", "Ok");
                    }
                }
            };
            btnCrearCuenta.GestureRecognizers.Add(clickCrear);
        }

        public async Task enviarUsuario()
        {
            string nit_, correo_, clave_, clave2_, nombre_, apellidos_, respuesta = "",
                telefono_ = "";
            bool publicidad;
            nit_ = "";
            correo_ = mail.Text;
            clave_ = pass1.Text.Trim();
            clave2_ = pass2.Text.Trim();
            nombre_ = nombre.Text;
            apellidos_ = apellidos.Text;
            nit_ = ""; // telefono.Text;
            telefono_ = ""; // telefono.Text;
            publicidad = Publicidad.IsToggled;

            if (!clave_.Equals(clave2_))
            {
                await DisplayAlert("Error", "Tu clave no coincide", "Ok");
            }
            else
            {
                Perfil usuarioRegistro = new Perfil();
                usuarioRegistro.Nombre = nombre_;
                usuarioRegistro.Apellidos = apellidos_;
                usuarioRegistro.NIT = nit_;
                usuarioRegistro.correo = correo_;
                usuarioRegistro.Cuenta = correo_;
                usuarioRegistro.clave = clave_;
                
                usuarioRegistro.Telefono = telefono_;
                
                usuarioRegistro.AceptaPublicidad = publicidad.ToString();
                usuarioRegistro.IdCliente = "0";
                /*try
                {
                    usuarioRegistro.fbid = usuarioRegistroFB.fbid;
                }
                catch (Exception ex)
                {
                    usuarioRegistro.fbid = "";
                }*/

                respuesta = await enviar(usuarioRegistro);

                if (!respuesta.Equals("Bienvenid@"))
                {
                    await DisplayAlert("Error", respuesta, "Ok");
                }
                else
                {
                    SM sM = new SM();
                    await sM.iniciarSesion(usuarioRegistro.correo, usuarioRegistro.clave, this.Navigation);

                }
            }
        }

        async Task<string> enviar(Perfil usuarioRegistro)
        {
            var json = JsonConvert.SerializeObject(usuarioRegistro);
            System.Diagnostics.Debug.WriteLine("PERFIL->" + json);

            var client = new HttpClient();
            StringContent str = new StringContent("op=registro&perfil=" + json, Encoding.UTF8, "application/x-www-form-urlencoded");
            var respuesta = await client.PostAsync(Constantes.url + "Sesion/App.php", str);
            string res = respuesta.Content.ReadAsStringAsync().Result.Trim();
            System.Diagnostics.Debug.WriteLine("respuesta: " + res);

            return res;
        }

        public void cerrar()
        {
            Navigation.PopModalAsync();
        }
    }
}
