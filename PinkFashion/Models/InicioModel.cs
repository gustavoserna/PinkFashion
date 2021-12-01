using System;
using System.Collections.ObjectModel;

namespace PinkFashion.Models
{
    public class InicioModel
    {
        public string idbannerapp_layout { get; set; }
        string _Banner_principal;
        public string Banner_principal
        {
            get
            {
                return Constantes.root_url + "/bannerapplayout/" + _Banner_principal;
            }
            set
            {
                _Banner_principal = value;
            }
        }

        string _Banner_promo;
        public string Banner_promo
        {
            get
            {
                return Constantes.root_url + "/bannerapplayout/" + _Banner_promo;
            }
            set
            {
                _Banner_promo = value;
            }
        }

        string _Banner_nuevos;
        public string Banner_nuevos
        {
            get
            {
                return Constantes.root_url + "/bannerapplayout/" + _Banner_nuevos;
            }
            set
            {
                _Banner_nuevos = value;
            }
        }

        string _Banner_vendidos;
        public string Banner_vendidos
        {
            get
            {
                return Constantes.root_url + "/bannerapplayout/" + _Banner_vendidos;
            }
            set
            {
                _Banner_vendidos = value;
            }
        }
        public ObservableCollection<Categoria_> Categorias { get; set; }
        public ObservableCollection<Familia> Familias { get; set; }
        public ObservableCollection<Layoutapp> LayoutApp { get; set; }
        public ObservableCollection<BannerPrincipal> Banners { get; set; }

    }
}
