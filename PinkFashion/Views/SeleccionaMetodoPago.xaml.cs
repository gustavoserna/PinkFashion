using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using PinkFashion.Models;
using PinkFashion.ViewModels;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class SeleccionaMetodoPago : ContentPage
    {

        PagarViewModel pagarViewModel;
        Card cardAux = null;
        string formaPago = "tarjeta";
        //bool efectivoCheck = false;

        public SeleccionaMetodoPago(Carrito pago)
        {
            InitializeComponent();
            Title = "Método de Pago";
            lblCerrar.TextColor = App.textColor;
            BindingContext = pagarViewModel = new PagarViewModel();

            listview.ItemTapped += async (s, e) =>
            {
                if (cardAux != null)
                    cardAux.imagen = "checkgris.png";

                formaPago = "tarjeta";
                var item = e.Item as Card;
                item.imagen = "checkpink.png";
                cardAux = item;

                Application.Current.Properties["IDMetodoPago"] = item.IdTarjeta;
                Application.Current.Properties["MetodoPago"] = item.Cuenta;
                Application.Current.Properties["TokenTarjeta"] = item.Token;
                await Application.Current.SavePropertiesAsync();
                await Navigation.PopModalAsync();
                pago.QuitarSelPago();

            };
            /*
            var clickEfectivo = new TapGestureRecognizer();
            clickEfectivo.Tapped += (s, e) =>
            {
                formaPago = "efectivo";
                if (cardAux != null)
                    cardAux.imagen = "checkgris.png";
                imgEfectivo.Source = "checkverde.png";
                
            };
            cash.GestureRecognizers.Add(clickEfectivo);
            */
            var clickAgregar = new TapGestureRecognizer();
            clickAgregar.Tapped += (s, e) =>
            {
                var page = new NavigationPage(new AgregarMetodo("modal"));
                page.BarBackgroundColor = App.bgColor;
                page.BarTextColor = App.textColor;
                Navigation.PushModalAsync(page);
            };
            btnAgregar.GestureRecognizers.Add(clickAgregar);

            gridHeader.IsVisible = true;
            var clickCerrar = new TapGestureRecognizer();
            clickCerrar.Tapped += (s, e) =>
            {
                Navigation.PopModalAsync();
            };
            cerrar.GestureRecognizers.Add(clickCerrar);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cardAux = null;
            pagarViewModel.LoadTarjetasCommand.Execute(null);
        }
    }
}
