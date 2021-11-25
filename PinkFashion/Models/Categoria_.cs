using System;
namespace PinkFashion.Models
{
    public class Categoria_
    {
        public string Categoria { get; set; }
        public string IdCategoria { get; set; }
        string _imagen;
        public string Imagen
        {
            get
            {
                return Constantes.root_url + _imagen;
            }
            set
            {
                _imagen = value;
            }
        }
    }
}
