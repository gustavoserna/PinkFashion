using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace PinkFashion.Models
{
    public class Pedido
    {
        //datos de envio de pedido
        string _IdCliente = "";
        public string IdCliente
        {
            get
            {
                return _IdCliente;
            }
            set
            {
                _IdCliente = value;
            }
        }

        string _NumPedido = "";
        public string NumPedido
        {
            get
            {
                return "No. pedido #" + _NumPedido;
            }
            set
            {
                _NumPedido = value;
            }
        }

        string _instrucciones = "";
        public string Instrucciones
        {
            get
            {
                return _instrucciones;
            }
            set
            {
                _instrucciones = value;
            }
        }

        string _IdTarjeta = "";
        public string IdTarjeta
        {
            get
            {
                return _IdTarjeta;
            }
            set
            {
                _IdTarjeta = value;
            }
        }

        //fecha =
        string _Fecha = "";
        public string Fecha
        {
            get
            {
                DateTime fecha = Convert.ToDateTime(_Fecha);
                string fechaS = fecha.ToString("dddd, dd MMMM yyyy", new CultureInfo("es-ES"));
                return "Pedido hecho el " + fechaS;
            }
            set
            {
                _Fecha = value;
            }
        }

        string _FormaPago = "";
        public string FormaPago
        {
            get
            {
                return _FormaPago;
            }
            set
            {
                _FormaPago = value;
            }
        }

        string _IdDireccion = "";
        public string IdDireccion
        {
            get
            {
                return _IdDireccion;
            }
            set
            {
                _IdDireccion = value;
            }
        }

        string _NoGuia = "";
        public string NoGuia
        {
            get
            {
                return _NoGuia;
            }
            set
            {
                _NoGuia = value;
            }
        }

        string _Flujo = "";
        public string Flujo
        {
            get
            {
                return _Flujo;
            }
            set
            {
                _Flujo = value;
            }
        }

        string _EstatusPedido = "";
        public string EstatusPedido
        {
            get
            {
                return _EstatusPedido;
            }
            set
            {
                _EstatusPedido = value;
            }
        }

        string _CodigoPromo = "";
        public string CodigoPromo
        {
            get
            {
                return _CodigoPromo;
            }
            set
            {
                _CodigoPromo = value;
            }
        }

        double _CostoEnvio = 0;
        public double CostoEnvio
        {
            get
            {
                return _CostoEnvio;
            }
            set
            {
                _CostoEnvio = value;
            }
        }

        string _Encuesta = "";
        public string Encuesta
        {
            get
            {
                return _Encuesta;
            }
            set
            {
                _Encuesta = value;
            }
        }

        double _Descuento = 0;
        public double Descuento
        {
            get
            {
                return _Descuento;
            }
            set
            {
                _Descuento = value;
            }
        }

        double _Total = 0;
        public double Total
        {
            get
            {
                return _Total;
            }
            set
            {
                _Total = value;
            }
        }

        string _OrderId = "";
        public string OrderId
        {
            get
            {
                return _OrderId;
            }
            set
            {
                _OrderId = value;
            }
        }

        string _SubtotalString;
        public string SubTotalString
        {
            get
            {
                double t = Total;
                return "SubTotal: " + t.ToString("c");
            }
            set
            {
                _SubtotalString = value;
            }
        }

        string _DescuentoString;
        public string DescuentoString
        {
            get
            {
                double t = Descuento;
                return "Descuento: " + t.ToString("c");
            }
            set
            {
                _DescuentoString = value;
            }
        }

        string _EnvioString;
        public string EnvioString
        {
            get
            {
                double t = CostoEnvio;
                return "Envío: " + t.ToString("c");
            }
            set
            {
                _EnvioString = value;
            }
        }
        string _TotalFinal;
        public string TotalFinal
        {
            get
            {
                double t = Total + CostoEnvio - Descuento;
                return "Total: " + t.ToString("c");
            }
            set
            {
                _TotalFinal = value;
            }
        }

        string _AliasDireccion;
        public string AliasDireccion
        {
            get
            {
                try
                {
                    return "Dirección de entrega: " + _AliasDireccion;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            set
            {
                _AliasDireccion = value;
            }
        }
    }
}
