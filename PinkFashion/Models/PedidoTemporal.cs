using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PinkFashion.Models
{
    public class PedidoTemporal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
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
                OnPopertyChanged("IdCliente");
            }
        }

        string _Direc = "";
        public string Direc
        {
            get
            {
                return _Direc;
            }
            set
            {
                _Direc = value;
                OnPopertyChanged("Direc");
            }
        }

        double _CostoEnvio;
        public double CostoEnvio
        {
            get
            {
                return _CostoEnvio;
            }
            set
            {
                _CostoEnvio = value;
                OnPopertyChanged("CostoEnvio");
            }
        }

        double _MonederoCliente;
        public double MonederoCliente
        {
            get
            {
                return _MonederoCliente;
            }
            set
            {
                _MonederoCliente = value;
                OnPopertyChanged("MonederoCliente");
            }
        }

        double _Latitud;
        public double Latitud
        {
            get
            {
                return _Latitud;
            }
            set
            {
                _Latitud = value;
                OnPopertyChanged("Latitud");
            }
        }

        double _Longitud;
        public double Longitud
        {
            get
            {
                return _Longitud;
            }
            set
            {
                _Longitud = value;
                OnPopertyChanged("Longitud");
            }
        }

        //List<ProductoTemporal> _productos;
        public ObservableCollection<ProductoTemporal> productos { get; set; }
        public ObservableCollection<Card> Tarjetas { get; set; }
        public ObservableCollection<Direccion> Direcciones { get; set; }
        void OnPopertyChanged(string obj)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));
        }
    }
}
