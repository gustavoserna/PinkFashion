using System;
namespace PinkFashion.Models
{
    public class Layoutpromo
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
    }
}
