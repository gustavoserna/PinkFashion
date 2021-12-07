using System;
using System.Collections.Generic;
using PinkFashion.Models;
using PinkFashion.ViewModels;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class FamiliaVista : ContentPage
    {
        FamiliaVistaViewModel familiaVistaViewModel;

        public FamiliaVista(Familia familia)
        {
            InitializeComponent();

            BindingContext = familiaVistaViewModel = new FamiliaVistaViewModel(familia);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(familiaVistaViewModel.Productos.Count == 0)
            {
                familiaVistaViewModel.LoadProductosCommand.Execute(null);
            }
        }
    }
}
