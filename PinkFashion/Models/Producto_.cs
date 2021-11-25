using System;
using System.Collections.ObjectModel;

namespace PinkFashion.Models
{
    public class Producto_
    {
        public string idproducto { get; set; }
        public string idmarca { get; set; }
        public string marca { get; set; }
        public string producto { get; set; }
        public string descripcion { get; set; }
        public string upc { get; set; }
        public string idcategoria { get; set; }
        public string categoria { get; set; }
        public string idsubcategoria { get; set; }
        public string subcategoria { get; set; }
        
        public string idclasificacion { get; set; }
        public string clasificaciones { get; set; }
        public string Piezas { get; set; }
        public int Cantidad { get; set; }
        public int idUM { get; set; }
        public string UMedida { get; set; }
        public int Existencia { get; set; }
        public int ConVariante { get; set; }
        public int cantidadVariantes { get; set; }
        public int bandera_carrito { get; set; }
        public int activo { get; set; }
        public int idTemporada { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaModificacion { get; set; }
        public double peso { get; set; }
        public int ConImpuesto { get; set; }
        public string Ubicacion { get; set; }
        public double orden { get; set; }
        public string FechaVistoUlt { get; set; }
        public int vistas { get; set; }
        public int tipo_orden { get; set; }
        public double ImporteDouble { get; set; }
        public string idvariantes_producto { get; set; }
        string _Importe;
        public string Importe
        {
            get
            {
                double d = Convert.ToDouble(_Importe);
                return d.ToString("c");
            }
            set
            {
                _Importe = value;
            }
        }
        public double precioDouble { get; set; }
        public double precioCarritoDouble { get; set; }
        public double sobreprecio { get; set; }

        string _SobrePreciostring;
        public string SobrePreciostring
        {
            get
            {
                return sobreprecio.ToString("c") + " MXN";
            }
            set
            {
                _SobrePreciostring = value;
            }
        }

        string _precioCarrito;
        public string precioCarrito
        {
            get
            {
                double p = Convert.ToDouble(_precioCarrito);
                precioCarritoDouble = p;
                return p.ToString("c") + " MXN";
            }
            set
            {
                _precioCarrito = value;
            }
        }

        string _precio;
        public string Precio
        {
            get
            {
                double p = Convert.ToDouble(_precio);
                precioDouble = p;
                return p.ToString("c") + " MXN";
            }
            set
            {
                _precio = value;
            }
        }
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

        string _stringNoVariantes;
        public string StringNoVariantes
        {
            get
            {
                return "(" + cantidadVariantes.ToString() + ")";
            }
            set
            {
                _stringNoVariantes = value;
            }
        }


        string _TituloVer;
        public string TituloVer
        {
            get
            {
                if (ConVariante==0)
                {
                    _TituloVer = "Ver Producto";
                }
                else
                {
                    _TituloVer = "Ver Tonos " + "(" + cantidadVariantes.ToString() + ")";
                }
                return _TituloVer;
            }
            set
            {
                _TituloVer = value;
            }
        }

        string _ConPromo;
        public string ConPromo
        {
            get
            {
                if (precioCarrito == SobrePreciostring)
                    _ConPromo = "False";
                else
                    _ConPromo = "True";
                return _ConPromo;
            }
            set
            {
                _ConPromo = value;
            }
        }
        string _Agotado;
        public string Agotado
        {
            get
            {
                if (ConVariante == 0)
                {
                    if (Existencia > 0)
                        _Agotado = "False";
                    else
                        _Agotado = "True";
                }
                else
                {
                    _Agotado = "False";
                }
                return _Agotado;
            }
            set
            {
                _Agotado = value;
            }
        }


        public ObservableCollection<VariantesProducto> VariantesProductos { get; set; }
        public ObservableCollection<Producto_> ProductosRelacionados { get; set; }
    }
}
