using PinkFashion.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace PinkFashion.Models
{
    public class Subcategoria_ : INotifyPropertyChanged
    {

        public string nombre { get; set; }
        public string idsubcategorias { get; set; }
        public string idcategoria { get; set; }

        private bool isUnderlined = false;

        public bool IsUnderlined
        {
            get { return isUnderlined; }
            set
            {
                SetProperty(ref isUnderlined, value);
            }
        }

        //public bool isUnderlined { get;
        //    set
        //    {
        //        SetProperty(ref isU)
        //    }
        //}


        public TextDecorations LabelTextDecoration
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
                LabelTextDecoration = value;
            }
        }
        string _imagen;
        public string imagen
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

        protected bool SetProperty<T>(ref T backingStore, T value,
    [CallerMemberName] string propertyName = "",
    Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
