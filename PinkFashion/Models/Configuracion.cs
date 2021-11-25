using System;
namespace PinkFashion.Models
{
    public class Configuracion
    {
        public int idconfiguracion { get; set; }
        public decimal dolar { get; set; }
        public double porcentaje_sobreprecio { get; set; }
        public decimal envio_gratis { get; set; }
        public int sincroniza { get; set; }
        public double IVA { get; set; }
    }
}
