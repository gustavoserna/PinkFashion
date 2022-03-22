using PinkFashion.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PinkFashion.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuscadorPage : ContentPage
    {
        public BuscadorPage()
        {
            InitializeComponent();
            this.BindingContext = new BuscadorViewModel(this.Navigation);
        }

        async private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar bar = (SearchBar)sender;
            string texto = bar.Text;

            var viewModel = (BuscadorViewModel)this.BindingContext;
            viewModel.Key = texto;
            await viewModel.ExecuteLoadAlgoliaSearchCommand();
            System.Diagnostics.Debug.WriteLine("Terminado");
        }

        //private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    SearchBar bar = (SearchBar) sender;
        //    this.BindingContext.
        //}
    }
}