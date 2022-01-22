﻿using Xamarin.Forms.OpenWhatsApp;
using Xamarin.Essentials;
using Xamarin.Forms;
using System;
//using Plugin.FirebaseAnalytics;

namespace PinkFashion.Views
{
    public partial class Cuenta : ContentPage
    {
        public bool vEsBlogger = false;
        public Cuenta()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("Blogger"))
            {
                if (Application.Current.Properties["Blogger"].ToString() == "true")
                    vEsBlogger = true;
                else
                    vEsBlogger = false;
            }
            else
            {
                vEsBlogger = false;
            }

            var clickPerfil = new TapGestureRecognizer();
            clickPerfil.Tapped += async (s, e) =>
            {
                
                if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                {
                    if (Application.Current.Properties["sesion"].Equals("activa"))
                    {

                        await Navigation.PushAsync(new PerfilUsuario());                        
                    }
                    else
                    {
                        bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await Navigation.PushModalAsync(new Login());
                        }
                    }
                }
                else
                {
                    bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                    if (ac)
                    {
                        await Navigation.PushModalAsync(new Login());
                    }
                }

            };
            btnPerfil.GestureRecognizers.Add(clickPerfil);

            var clickBuzon = new TapGestureRecognizer();
            clickBuzon.Tapped += async (s, e) =>
            {

                if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                {
                    if (Application.Current.Properties["sesion"].Equals("activa"))
                    {

                        await Navigation.PushAsync(new Buzon());
                    }
                    else
                    {
                        bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await Navigation.PushModalAsync(new Login());
                        }
                    }
                }
                else
                {
                    bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                    if (ac)
                    {
                        await Navigation.PushModalAsync(new Login());
                    }
                }
            };
            btnBuzon.GestureRecognizers.Add(clickBuzon);

            var clickDirecciones = new TapGestureRecognizer();
            clickDirecciones.Tapped += async (s, e) =>
            {
                
                if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                {
                    if (Application.Current.Properties["sesion"].Equals("activa"))
                    {

                        await Navigation.PushAsync(new MisDirecciones());
                    }
                    else
                    {
                        bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await Navigation.PushModalAsync(new Login());
                        }
                    }
                }
                else
                {
                    bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                    if (ac)
                    {
                        await Navigation.PushModalAsync(new Login());
                    }
                }

            };
            btnDirecciones.GestureRecognizers.Add(clickDirecciones);
            
            var clickPago = new TapGestureRecognizer();
            clickPago.Tapped += async (s, e) =>
            {
                
                if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                {
                    if (Application.Current.Properties["sesion"].Equals("activa"))
                    {

                        await Navigation.PushAsync(new MetodoPago());
                    }
                    else
                    {
                        bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await Navigation.PushModalAsync(new Login());
                        }
                    }
                }
                else
                {
                    bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                    if (ac)
                    {
                        await Navigation.PushModalAsync(new Login());
                    }
                }

            };
            btnPago.GestureRecognizers.Add(clickPago);

            var clickPedidos = new TapGestureRecognizer();
            clickPedidos.Tapped += async (s, e) =>
            {
                
                if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                {
                    if (Application.Current.Properties["sesion"].Equals("activa"))
                    {

                        await Navigation.PushAsync(new MisPedidos());
                    }
                    else
                    {
                        bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await Navigation.PushModalAsync(new Login());
                        }
                    }
                }
                else
                {
                    bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                    if (ac)
                    {
                        await Navigation.PushModalAsync(new Login());
                    }
                }
            };
            btnPedidos.GestureRecognizers.Add(clickPedidos);

            var clickPinAfiliados = new TapGestureRecognizer();
            clickPinAfiliados.Tapped += async (s, e) =>
            {

                if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                {
                    if (Application.Current.Properties["sesion"].Equals("activa"))
                    {

                        await Navigation.PushAsync(new PinAfiliadosBloggers());
                    }
                    else
                    {
                        bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await Navigation.PushModalAsync(new Login());
                        }
                    }
                }
                else
                {
                    bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                    if (ac)
                    {
                        await Navigation.PushModalAsync(new Login());
                    }
                }

            };
            btnPinBloguers.GestureRecognizers.Add(clickPinAfiliados);
            btnPinBloguers.IsVisible = vEsBlogger;

            var clickAfiliados = new TapGestureRecognizer();
            clickAfiliados.Tapped += async (s, e) =>
            {

                if (Application.Current.Properties.ContainsKey("IdCliente") && Application.Current.Properties.ContainsKey("sesion"))
                {
                    if (Application.Current.Properties["sesion"].Equals("activa"))
                    {

                        await Navigation.PushAsync(new AfiliadosBloggers());
                    }
                    else
                    {
                        bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                        if (ac)
                        {
                            await Navigation.PushModalAsync(new Login());
                        }
                    }
                }
                else
                {
                    bool ac = await DisplayAlert("No te encuentras registrado.", "¿Deseas registrarte?", "Sí", "No");
                    if (ac)
                    {
                        await Navigation.PushModalAsync(new Login());
                    }
                }

            };
            btnConfBloguers.GestureRecognizers.Add(clickAfiliados);
            btnConfBloguers.IsVisible = vEsBlogger;

            var clickAyuda = new TapGestureRecognizer();
            clickAyuda.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new FAQ());
                //string strEvento = "Q&A|Pink Fashion Store";
                //App.eventTracker.SendScreen(strEvento, "PreguntasFrecuentes");
                
                //await Launcher.OpenAsync("https://pinkfashionstore.com/preguntas.php");

            };
            btnAyuda.GestureRecognizers.Add(clickAyuda);

            var clickEresBB = new TapGestureRecognizer();
            clickEresBB.Tapped += async (s, e) =>
            {
                string strEvento = "Eres BB|Pink Fashion Store";
                App.eventTracker.SendScreen(strEvento, "BB");
                await Launcher.OpenAsync("https://pinkfashionstore.com/eresBB.php");

            };
            btnFormularioBB.GestureRecognizers.Add(clickEresBB);

            var clickFacturar = new TapGestureRecognizer();
            clickFacturar.Tapped += (s, e) =>
            {
                Chat.Open("+528711223702", "Hola, deseo facturar");
                App.eventTracker.SendScreen("Whatsapp|PinkFashionStore", "Whatsapp");
                //string strEvento = "Facturación|Pink Fashion Store";
                //App.eventTracker.SendScreen(strEvento, "Facturacion");
                //await DisplayAlert("Información", "Para facturar tus pedidos entra a https://pinkfashionstore.com inicia sesión y en tu historial encontrarás la opción. Gracias por tu preferencia, estamos para atenderte.", "Ok");                
            };
            btnFacturar.GestureRecognizers.Add(clickFacturar);

            var clickTerminos = new TapGestureRecognizer();
            clickTerminos.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new TerminosCondiciones());
                //string strEvento = "Terminos y Condiciones|Pink Fashion Store";
                //App.eventTracker.SendScreen(strEvento, "TerminosyCondiciones");
                //await Launcher.OpenAsync("https://pinkfashionstore.com/terminoscondiciones.php");

            };
            btnTerminos.GestureRecognizers.Add(clickTerminos);

            var clickWhatsapp = new TapGestureRecognizer();
            clickWhatsapp.Tapped += async (s, e) =>
            {
                try
                {
                    string strEvento = "Whatsapp|Pink Fashion Store";
                    App.eventTracker.SendScreen(strEvento, "Whatsapp");
                    Chat.Open("+528711223702", "Hola");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }

            };
            btnWhatsapp.GestureRecognizers.Add(clickWhatsapp);
        }

        
    }
}
