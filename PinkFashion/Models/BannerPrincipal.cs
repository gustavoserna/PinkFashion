using System;
namespace PinkFashion.Models
{
    public class BannerPrincipal
    {
        public string idBanner_principal { get; set; }
        public string Banner_principal { get; set; }
        public int Tipo { get; set; }  // 1. Producto 2. Marca 3. Categoria
        public string idmarca { get; set; }
        public string id_Producto { get; set; }
        public string id_Categoria { get; set; }

        string _ImgBanner;
        public string ImagenBanner
        {
            get
            {
                return Constantes.root_url + "/SliderApp/" + _ImgBanner;
            }
            set
            {
                _ImgBanner = value;
            }
        }
    }
}
