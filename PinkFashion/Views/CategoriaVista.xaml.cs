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
        }
    }
}