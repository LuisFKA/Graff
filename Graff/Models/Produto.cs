using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graff.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public float Valor { get; set; }

        public ICollection<Lance> Lances { get;set; }

        public Produto()
        {

        }
    }
}
