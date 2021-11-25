
using System.ComponentModel;

namespace PinkFashion.Models
{
    public class ProductoTemporal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int IdPedidoTemporal { get; set; }
        public string FechaHora { get; set; }
        public string IdCliente { get; set; }
        public string IdProducto { get; set; }
        public string IdVariante_Producto { get; set; }
        public int ConVariante { get; set; }
        public string IdMarca { get; set; }
        public string Marca { get; set; }
        public string IdCategoria { get; set; }
        public string Categoria { get; set; }
        public string Accion { get; set; }

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

        string _imagen_marca;
        public string Imagen_Marca
        {
            get
            {
                return Constantes.root_url + _imagen_marca;
            }
            set
            {
                _imagen_marca = value;
            }
        }

        double _cantidad;
        public double Cantidad
        {
            get
            {
                return _cantidad;
            }
            set
            {
                _cantidad = value;
                OnPopertyChanged("Cantidad");
            }
        }

        double precio;
        public double Precio
        {
            get
            {
                return precio;
            }
            set
            {
                precio = value;
                OnPopertyChanged("Precio");
            }
        }

        double _PrecioDescuento;
        public double PrecioDescuento
        {
            get
            {
                return _PrecioDescuento;
            }
            set
            {
                _PrecioDescuento = value;
                OnPopertyChanged("PrecioDescuento");
            }
        }

        string _precio;
        public string _Precio
        {
            get
            {
                return "Precio " + Precio.ToString("c");
            }
            set
            {
                _precio = value;
                OnPopertyChanged("_Precio");
            }
        }

        /*Producto*/
        string _Producto = "";
        public string Producto
        {
            get
            {
                return _Producto;
            }
            set
            {
                _Producto = value;
                OnPopertyChanged("Producto");
            }
        }


        string _DescripcionVariante = "";
        public string DescripcionVariante
        {
            get
            {
                return _DescripcionVariante;
            }
            set
            {
                _DescripcionVariante = value;
                OnPopertyChanged("DescripcionVariante");
            }
        }

        string _NombreProducto;
        public string NombreProducto
        {
            get
            {
                return Producto + " " + DescripcionVariante;
            }
            set
            {
                _NombreProducto = value;
            }
        }

        double total;
        public double Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
                OnPopertyChanged("Total");
                OnPopertyChanged("_Total");
            }
        }

        string _total;
        public string _Total
        {
            get
            {
                return "Total " + Total.ToString("c");
            }
            set
            {
                _total = value;
                OnPopertyChanged("_Total");
            }
        }

        double descuento;
        public double Descuento
        {
            get
            {
                return descuento;
            }
            set
            {
                descuento = value;
                OnPopertyChanged("_Descuento");
            }
        }

        string _descuento;
        public string _Descuento
        {
            get
            {
                return "Dscto " + Descuento.ToString("c");
            }
            set
            {
                _descuento = value;
                OnPopertyChanged("_Descuento");
            }
        }

        void OnPopertyChanged(string obj)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));
        }


        public override string ToString()
        {
            return Marca;
        }

       
    }
}
