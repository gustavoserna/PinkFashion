using System;
using System.ComponentModel;

namespace PinkFashion.Models
{
    public class Direccion : INotifyPropertyChanged
    {
        public string IdClienteDir { get; set; }
        public string IdCliente { get; set; }
        public string NIT { get; set; }
        public string NoInt { get; set; }
        public string Instrucciones { get; set; }
        public string Calle { get; set; }
        public string Colonia { get; set; }
        public string NoExt { get; set; }
        public int IdMunicipio { get; set; }
        public string Municipio { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public string Alias { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string CodigoPostal { get; set; }
        public string Guardar { get; set; }
        public double Kilometros { get; set; }
        public int Direc_Default { get; set; }
        public string CostoEnvio { get; set; }
        
        string _DireccionCompleta;
        public string DireccionCompleta
        {
            get
            {
                return Calle + ", " + NoExt + ", " + NoInt + "," + Colonia + ", " + CodigoPostal + ", " + Municipio + ", " + Estado;
            }
            set
            {
                _DireccionCompleta = value;
            }
        }

        string _Abreviacion;
        public string Abreviacion
        {
            get
            {
                return Calle + ", " + NoExt;
            }
            set
            {
                _Abreviacion = value;
            }
        }

        string _MiDireccion;
        public string MiDireccion
        {
            get
            {
                return Alias + " - " + Calle + ", " + NoExt;
            }
            set
            {
                _MiDireccion = value;
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
            /*
            get
            {
                if (Direc_Default == 1)
                {
                    _imagen = "checkpink.png";
                }
                else
                {
                    _imagen = "checkgris.png";
                }
                return _imagen;
            }
            set
            {
                _imagen = value;
                OnPopertyChanged("imagen");
            }*/
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPopertyChanged(string obj)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));
        }
    }
}
