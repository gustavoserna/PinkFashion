using System;
using PinkFashion.Models;
using PinkFashion.ViewModels;
using Xamarin.Forms;

namespace PinkFashion.Views
{
    public partial class SeleccionaDireccion : ContentPage
    {

        ElegirDireccionViewModel elegirDireccionView;
        Direccion dirAux = null;

        
        public SeleccionaDireccion(Carrito pago)
        {
            InitializeComponent();
            Title = "Dirección de entrega";
            BindingContext = elegirDireccionView = new ElegirDireccionViewModel();

            listview.ItemTapped += async (s, e) =>
            {
                if (dirAux != null)
                    dirAux.imagen = "checkgris.png";
                try
                {

                    var item = e.Item as Direccion;
                    item.imagen = "checkpink.png";
                    dirAux = item;
                    Application.Current.Properties["IDDireccion"] = item.IdClienteDir;
                    Application.Current.Properties["Direccion"] = item.MiDireccion;
                    Application.Current.Properties["CostoEnvio"] = item.CostoEnvio;
                    await Application.Current.SavePropertiesAsync();
                    await Navigation.PopModalAsync();
                    await pago.QuitarSelDireccionAsync();

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

        async void OnbtnAgregarClicked(object sender, EventArgs args)
        {

            try
            {
                var page = new NavigationPage(new AgregarDireccion("modal"));
                page.BarBackgroundColor = App.bgColor;
                page.BarTextColor = App.textColor;
                await Navigation.PushModalAsync(page);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayAlert("Error", "Intentalo de nuevo mas tarde", "Ok");

            }

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            dirAux = null;
            elegirDireccionView.LoadDireccionesCommand.Execute(null);
        }
    }
}