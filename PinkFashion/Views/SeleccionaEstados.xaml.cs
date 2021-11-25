using System;
using System.Linq;
using System.Collections.Generic;
using PinkFashion.Models;
using PinkFashion.ViewModels;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace PinkFashion.Views
{
    public partial class SeleccionaEstados : ContentPage
    {
        EstadosViewModel estadosViewmodel;
        //Estados EstadosAux = null;

        public SeleccionaEstados(AgregarDireccion pDireccion)
        {
            InitializeComponent();
            Title = "Dirección de entrega";
            BindingContext = estadosViewmodel = new EstadosViewModel();

            listview.ItemTapped += async (s, e) =>
            {
                
                try
                {

                    var item = e.Item as Estados;
                    
                    Application.Current.Properties["IDEstado"] = item.IdEstado;
                    Application.Current.Properties["Estado"] = item.Estado;

                    await Application.Current.SavePropertiesAsync();
                    await Navigation.PopModalAsync();
                    pDireccion.QuitarSelEstado();

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    await DisplayAlert("Error", "Intentalo de nuevo", "Ok");

                }

            };


            var cerrar = new ToolbarItem();
            cerrar.Text = "Cerrar";
            cerrar.Command = new Command(async o =>
            {
                await Navigation.PopModalAsync();
            });
            ToolbarItems.Add(cerrar);

        }

        


        protected override void OnAppearing()
        {
            base.OnAppearing();
            //EstadosAux = null;
            estadosViewmodel.LoadItemsCommand.Execute(null);
        }
    }
}