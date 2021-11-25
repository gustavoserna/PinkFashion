using System;
namespace PinkFashion.Models
{
    public class Perfil
    {
        public string IdCliente { get; set; }
        public string NIT { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string correo { get; set; }
        public string Telefono { get; set; }
        public string clave { get; set; }
        public string FechaNac { get; set; }
        public string Genero { get; set; }
        public string IDDireccion { get; set; }
        public string direccion { get; set; }
        public string NoInt { get; set; }
        public string NoExt { get; set; }
        public string CP { get; set; }
        public string CelID { get; set; }
        public string Cuenta { get; set; }
        public string fbId { get; set; }
        public string Encontrado { get; set; }
        public string AceptaPublicidad { get; set; }
        public string EsBlogger { get; set; }

        string _Abreviado;
        public string Abreviado
        {
            get
            {
                int i = 0;
                string s = Nombre;
                string _Abreviado = Nombre;
                foreach (char c in s)
                {
                    if (c==' ')
                    {
                        _Abreviado = Nombre.Substring(0,i);
                    }
                    i++; 
                }
                
                return _Abreviado;

            }
            set
            {
                _Abreviado = value;
            }
        }
    }
}
