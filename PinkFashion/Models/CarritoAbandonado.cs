using System;
namespace PinkFashion.Models
{
    public class CarritoAbandonado
    {
        public string idCliente { get; set; }
        public int numcarrito { get; set; }
        public string idcarrito { get; set; }
        public int idcarrito_abandonado { get; set; }
        public string idproducto { get; set; }
        public string idvariante_producto { get; set; }
        public int cantidad { get; set; }
        public int existencia_disponible { get; set; }
        public string producto { get; set; }
        public string idmarca { get; set; }
        public string marca { get; set; }
        public string variante { get; set; }
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
