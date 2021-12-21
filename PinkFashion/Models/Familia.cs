using System;
namespace PinkFashion.Models
{
    public class Familia
    {
        public int id_clasificacion { get; set; }
        public string clasificaciones { get; set; }
        public int status { get; set; }

        string _imagen;
        public string imagen
        {
            get
            {
                return Constantes.root_url + "BannersFamilias/Autorizadas/" + _imagen;
            }
            set
            {
                _imagen = value;
            }
        }
    }
}
