using System;
using System.Collections.Generic;
using PinkFashion.ViewModels;
//using Plugin.FirebaseAnalytics;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class MisDirecciones : ContentPage
    {
        MisDireccionesViewModel misDireccionesViewModel;
        string strEvento = "Mis Direcciones|Pink Fashion Store";
        public MisDirecciones()
        {
            InitializeComponent();
            Title = "Mis direcciones";
                        
            BindingContext = misDireccionesViewModel = new MisDireccionesViewModel();


            var agregar = new ToolbarItem();
            agregar.Text = "Nueva";
            agregar.Command = new Command(o =>
            {
                Navigation.PushAsync(new AgregarDireccion());
            });
            ToolbarItems.Add(agregar);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            misDireccionesViewModel.LoadItemsCommand.Execute(null);
            App.eventTracker.SendScreen(strEvento, nameof(MisDirecciones));
        }
    }
}
