using System;
namespace PinkFashion.Models
{
    public class Familia
    {
        public int id_clasificacion { get; set; }
        public string clasificaciones { get; set; }
        public int status { get; set; }
        public string imgPreview { get; set; }

        public string imagen
        {
            get
            {
                return imagen;
            }
            set
            {
                imagen = Constantes.root_url + imgPreview;
            }
        }
    }
}
