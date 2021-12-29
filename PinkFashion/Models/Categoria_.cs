using System;
using Xamarin.Forms;

namespace PinkFashion.Models
{
    public class Categoria_
    {
        public int IdFamilia { get; set; }
        public string Categoria { get; set; }
        public string IdCategoria { get; set; }
        string _imagen;

        private bool isUnderlined = false;

        public bool IsUnderlined
        {
            get
            {
                return isUnderlined;
            }
            set
            {
                this.isUnderlined = value;
            }
        }

        private TextDecorations categoriaTextDecoration = TextDecorations.None;
        
        public TextDecorations CategoriaTextDecoration
        {
            get
            {
                if(IsUnderlined)
                {
                    return TextDecorations.Underline;
                } else
                {
                    return TextDecorations.None;
                }
            }
            set
            {
                this.categoriaTextDecoration = value;
            }
        }

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
    }
}
