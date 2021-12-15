using System;
using System.Collections.Generic;
using PinkFashion.ViewModels;
//using Plugin.FirebaseAnalytics;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class MetodoPago : ContentPage
    {
        MetodoPagoViewModel metodoPagoViewModel;
        string strEvento = "Métodos pago|Pink Fashion Store";
        public MetodoPago()
        {
            InitializeComponent();
            Title = "Métodos de Pago";

            BindingContext = metodoPagoViewModel = new MetodoPagoViewModel();

            var nuevo = new ToolbarItem();
            nuevo.Text = "Nuevo";//->
            nuevo.Command = new Command(o =>
            {
                Navigation.PushAsync(new AgregarMetodo());
            });
            ToolbarItems.Add(nuevo);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            metodoPagoViewModel.LoadItemsCommand.Execute(null);
            App.eventTracker.SendScreen(strEvento, nameof(MetodoPago));
        }
    }
}
