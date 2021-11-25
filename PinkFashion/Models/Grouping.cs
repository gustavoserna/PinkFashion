using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkFashion.Models
{
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }
        public string Suma { get; private set; }
        public Grouping(K key, double suma, IEnumerable<T> items)
        {
            Key = key;
            Suma = suma.ToString("c"); ;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
