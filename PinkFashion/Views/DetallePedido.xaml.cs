using System;
using System.Collections.Generic;
using PinkFashion.Models;
using PinkFashion.ViewModels;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class DetallePedido : ContentPage
    {
        DetallePedidoViewModel detallePedidoViewModel;
        string vNumPedido;
        string vFlujo;
        public DetallePedido(Pedido pedido)
        {
            InitializeComponent();
            Title = "Detalles del pedido";
            lblFecha.Text = pedido.Fecha;
            lblTotal.Text = pedido.Total.ToString("c");
            lblTotalFinal.Text = pedido.TotalFinal.Substring(6);
            lblDescuento.Text = pedido.Descuento.ToString("c");
            lblFormaPago.Text = pedido.FormaPago;
            lblAliasDireccion.Text = pedido.AliasDireccion;
            lblEnvioC.Text = pedido.CostoEnvio.ToString("c");
            lblNumPedido.Text = pedido.NumPedido;
            lblNumGuia.Text = pedido.NoGuia;
            vNumPedido = pedido.NumPedido;
            vFlujo = pedido.Flujo;
            float progress = 0;
            if (pedido.Flujo == "Nuevo")
            {
                progress = 0.15f;
                lblNuevo.Opacity = 1;
                lblPagado.Opacity = 0.3;
                lblEmpaque.Opacity = 0.3;
                lblGuia.Opacity = 0.3;
                lblEnviado.Opacity = 0.3;
                lblRecibido.Opacity = 0.3;
                
            }
            if (pedido.Flujo == "Pagado")
            {

                progress = 0.3f;
                lblNuevo.Opacity = 0.3;
                lblPagado.Opacity = 1;
                lblEmpaque.Opacity = 0.3;
                lblGuia.Opacity = 0.3;
                lblEnviado.Opacity = 0.3;
                lblRecibido.Opacity = 0.3;

            }

            if (pedido.Flujo == "Empaque")
            {
                progress = 0.45f;
                lblNuevo.Opacity = 0.3;
                lblPagado.Opacity = 0.3;
                lblEmpaque.Opacity = 1;
                lblGuia.Opacity = 0.3;
                lblEnviado.Opacity = 0.3;
                lblRecibido.Opacity = 0.3;

            }
            if (pedido.Flujo == "Guia")
            {
                progress = 0.60f;
                lblNuevo.Opacity = 0.3;
                lblPagado.Opacity = 0.3;
                lblEmpaque.Opacity = 0.3;
                lblGuia.Opacity = 1;
                lblEnviado.Opacity = 0.3;
                lblRecibido.Opacity = 0.3;
            }
            if (pedido.Flujo == "Enviado")
            {
                progress = 0.75f;
                lblNuevo.Opacity = 0.3;
                lblPagado.Opacity = 0.3;
                lblEmpaque.Opacity = 0.3;
                lblGuia.Opacity = 0.3;
                lblEnviado.Opacity = 1;
                lblRecibido.Opacity = 0.3;
            }
            if (pedido.Flujo == "Recibido")
            {
                progress = 1;
                lblNuevo.Opacity = 0.3;
                lblPagado.Opacity = 0.3;
                lblEmpaque.Opacity = 0.3;
                lblGuia.Opacity = 0.3;
                lblEnviado.Opacity = 0.3;
                lblRecibido.Opacity = 1;
            }
            if (pedido.Flujo == "Cancelado")
            {
                progress = 1;

                lblNuevo.Opacity = 0.3;
                lblPagado.Opacity = 0.3;
                lblEmpaque.Opacity = 0.3;
                lblGuia.Opacity = 0.3;
                lblEnviado.Opacity = 0.3;
                lblRecibido.Opacity = 0.3;
            }
            defaultProgressBar.Progress = progress;
            System.Diagnostics.Debug.WriteLine(pedido.NumPedido);
            BindingContext = detallePedidoViewModel = new DetallePedidoViewModel(pedido.NumPedido);


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (detallePedidoViewModel.Items.Count == 0)
                detallePedidoViewModel.LoadItemsCommand.Execute(null);
        }
    }
}
