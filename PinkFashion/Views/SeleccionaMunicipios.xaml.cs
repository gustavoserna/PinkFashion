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
    public partial class SeleccionaMunicipios : ContentPage
    {
        MunicipiosViewModel municipiosViewmodel;
        //Municipios EstadosAux = null;

        public SeleccionaMunicipios(AgregarDireccion pDireccion, string pEstado)
        {
            InitializeComponent();
            Title = "Dirección de entrega";
            BindingContext = municipiosViewmodel = new MunicipiosViewModel(pEstado);

            listview.ItemTapped += async (s, e) =>
            {

                try
                {

                    var item = e.Item as Municipios;

                    Application.Current.Properties["IDMunicipio"] = item.IdMunicipio;
                    Application.Current.Properties["Municipio"] = item.Municipio;

                    await Application.Current.SavePropertiesAsync();
                    await Navigation.PopModalAsync();
                    pDireccion.QuitarSelMunicipio();

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
            municipiosViewmodel.LoadItemsCommand.Execute(null);
        }
    }
}