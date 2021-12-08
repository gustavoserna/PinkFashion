using System;
using System.Collections.Generic;
using PinkFashion.Models;
using PinkFashion.ViewModels;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class MarcasVista : ContentPage
    {
        MarcasViewModel marcasViewModel;

        public MarcasVista(Familia familia)
        {
            InitializeComponent();

            Title = "Marcas";

            var close = new ToolbarItem()
            {
                Text = "Cerrar"
            };
            close.Command = new Command(() =>
            {
                Navigation.PopModalAsync();
            });
            ToolbarItems.Add(close);

            BindingContext = marcasViewModel = new MarcasViewModel(familia);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(marcasViewModel.Marcas.Count == 0)
            {
                marcasViewModel.LoadMarcasCommand.Execute(null);
            }
        }

        void ListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
        }
    }
}
