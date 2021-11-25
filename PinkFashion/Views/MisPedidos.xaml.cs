using System;
using System.Collections.Generic;
using PinkFashion.Models;
using PinkFashion.ViewModels;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class MisPedidos : ContentPage
    {
        MisPedidosViewModel misPedidosViewModel;
        string strEvento = "Mis Pedidos|Pink Fashion Store";
        public MisPedidos(string tipo = "push")
        {
            InitializeComponent();
            Title = "Mis pedidos";
            
            BindingContext = misPedidosViewModel = new MisPedidosViewModel();

            if (tipo.Equals("modal"))
            {//
                var cerrar = new ToolbarItem();
                cerrar.Text = "Cerrar";
                cerrar.Command = new Command(o =>
                {
                    Navigation.PopModalAsync();
                });
                ToolbarItems.Add(cerrar);
            }

            //
            listview.ItemTapped += (s, e) =>
            {
                var item = e.Item as Pedido;
                Navigation.PushAsync(new DetallePedido(item));
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (misPedidosViewModel.Items.Count == 0)
                misPedidosViewModel.LoadItemsCommand.Execute(null);

            App.eventTracker.SendScreen(strEvento, nameof(MisPedidos));
        }
    }
}
