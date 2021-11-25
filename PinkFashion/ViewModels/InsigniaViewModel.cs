using System;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    public class InsigniaViewModel : BaseViewModel
    {
        int _noProductos = 0;
        int _Abandonados = 0;
        string _Monedero = "";
        string _TituloMonedero = "";
        string _EnviosGratis = "0";
        string _TituloEnviosGratis = "";
        string _AliasUsuario = "";
        //string _StringEnviosGratis = "";
        public int noProductos
        {
            get
            {
                return _noProductos;
            }
            set
            {
                SetProperty(ref _noProductos, value);
            }
        }

        public int Abandonados
        {
            get
            {
                return _Abandonados;
            }
            set
            {
                SetProperty(ref _Abandonados, value);
            }
        }

        bool _visibleAbandonado = false;
        public bool visibleAbandonado
        {
            get
            {
                if (Abandonados==1)
                {
                    _visibleAbandonado = true;
                }
                else
                {
                    _visibleAbandonado = false;
                }
                return _visibleAbandonado;
            }
            set
            {
                SetProperty(ref _visibleAbandonado, value);
            }
        }

        public string Monedero
        {
            get
            {
                return _Monedero;
            }
            set
            {
                SetProperty(ref _Monedero, value);
            }
        }

        public string TituloMonedero
        {
            get
            {
                return _TituloMonedero;
            }
            set
            {
                SetProperty(ref _TituloMonedero, value);
            }
        }
        /*
        public string StringEnviosGratis
        {
            get
            {
                if (App.EnvioGratis <= 0)
                {
                    _StringEnviosGratis = "Ya cuentas con envío gratis";
                }
                else
                {
                    _StringEnviosGratis = "Te faltan " + App.EnvioGratis.ToString() + " para envío gratis";
                }
                return _StringEnviosGratis;
            }
            set
            {
                SetProperty(ref _StringEnviosGratis, value);
            }
        }*/

        public string EnviosGratis
        {
            get
            {
                return _EnviosGratis;
            }
            set
            {
                SetProperty(ref _EnviosGratis, value);
            }
        }

        public string TituloEnviosGratis
        {
            get
            {
                return _TituloEnviosGratis;
            }
            set
            {
                SetProperty(ref _TituloEnviosGratis, value);
            }
        }

        public string AliasUsuario
        {
            get
            {
                return _AliasUsuario;
            }
            set
            {
                SetProperty(ref _AliasUsuario, value);
            }
        }

        public Command GetContador { get; set; }

        public InsigniaViewModel()
        {


        }

        
    }
}
