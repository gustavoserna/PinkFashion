using System;
using System.Collections.Generic;

namespace PinkFashion.Models
{
    public class MarcaGroup : List<ProductoTemporal>
    {
        public string Marca { get; private set; }

        public MarcaGroup(string name, List<ProductoTemporal> products) : base(products)
        {
            Marca = name;
        }

        public override string ToString()
        {
            return Marca;
        }
    }
}
