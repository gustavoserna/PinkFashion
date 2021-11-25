using System;
namespace PinkFashion.Models
{
    public class Marcas_
    {
        public string Marca { get; set; }
        public string IdMarca { get; set; }
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
