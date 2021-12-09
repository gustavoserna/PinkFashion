using System;
using System.Collections.Generic;
using System.Text;

namespace PinkFashion.Models
{
    public class Subcategoria_
    {

        public string nombre { get; set; }
        public string idsubcategorias { get; set; }
        public string idcategoria { get; set; }
        string _imagen;
        public string imagen
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
