using System;
namespace PinkFashion.Models
{
    public class Layoutapp
    {
        public string IdLayout { get; set; }
        public int Posicion { get; set; }
        public string Bloque { get; set; }
        public string IdMarca1 { get; set; }
        public string Marca1 { get; set; }
        public string IdMarca2 { get; set; }
        public string Marca2 { get; set; }
        string _VisibleBloque1;
        public string VisibleBloque1
        {
            get
            {
                if (Bloque == "1")
                    _VisibleBloque1 = "True";
                else
                    _VisibleBloque1 = "False";
                return _VisibleBloque1;
            }
            set
            {
                _VisibleBloque1 = value;
            }
        }

        string _VisibleBloque2;
        public string VisibleBloque2
        {
            get
            {
                if (Bloque == "2")
                    _VisibleBloque2 = "True";
                else
                    _VisibleBloque2 = "False";
                return _VisibleBloque2;
            }
            set
            {
                _VisibleBloque2 = value;
            }
        }


        string _imagen_marca1;
        public string Imagen_marca1
        {
            get
            {
                return Constantes.root_url + "/LayoutApp/" + IdLayout + "/" + _imagen_marca1;
            }
            set
            {
                _imagen_marca1 = value;
            }
        }

        string _imagen_marca2;
        public string Imagen_marca2
        {
            get
            {
                return Constantes.root_url + "/LayoutApp/" + IdLayout + "/" + _imagen_marca2;
            }
            set
            {
                _imagen_marca2 = value;
            }
        }
    }
}
