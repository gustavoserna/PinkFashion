using System;
namespace PinkFashion.Models
{
    public class VariantesProducto
    {
        public string idvariantes_producto { get; set; }
        public string idproducto { get; set; }
        public string descripcion_variante { get; set; }
        public string sku { get; set; }
        public string codBarras { get; set; }
        public string url_variante { get; set; }
        public int status { get; set; }
        public string idTemporada { get; set; }
        public int Existencia { get; set; }

        string _imagen;
        public string Imagen_Variante
        {
            get
            {
                return String.Format(Constantes.url_img, this.idproducto, this._imagen);
                //return Constantes.root_url + url_variante;
            }
            set
            {
                _imagen = value;
            }
        }

        string _Agotado;
        public string Agotado
        {
            get
            {
                if (Existencia > 0)
                    _Agotado = "False";
                else
                    _Agotado = "True";
                return _Agotado;
            }
            set
            {
                _Agotado = value;
            }
        }

    }
}
