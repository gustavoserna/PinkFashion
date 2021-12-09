using PinkFashion.Models;
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
    public partial class CategoriaVista : ContentPage
    {
        CategoriaViewModel categoriaViewModel;

        public CategoriaVista(Categoria_ categoria)
        {
            InitializeComponent();

            BindingContext = categoriaViewModel = new CategoriaViewModel(categoria);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (categoriaViewModel.Productos.Count == 0)
            {
                categoriaViewModel.LoadProductosCommand.Execute(null);
            }

            if (categoriaViewModel.Subcategorias.Count == 0)
            {
                categoriaViewModel.LoadSubcategoriasCommand.Execute(null);
            } 
        }
    }
}