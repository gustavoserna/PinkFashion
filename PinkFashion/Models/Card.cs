using System;
using System.ComponentModel;

namespace PinkFashion.Models
{
    public class Card : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int IdTarjeta { get; set; }
        public string IdCliente { get; set; }
        public string Cuenta { get; set; }
        public string Token { get; set; }

        string _cuentadigitos;
        public string CuentaDigitos
        {
            get
            {
                return "**** " + Cuenta;
            }
            set
            {
                _cuentadigitos = value;
            }
        }

        string _imagen = "checkgris.png";
        public string imagen
        {
            get
            {
                return _imagen;
            }
            set
            {
                _imagen = value;
                OnPopertyChanged("imagen");
            }
        }

        void OnPopertyChanged(string obj)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));
        }
    }
}
