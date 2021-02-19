using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graff.Models
{
    public class Lance: IComparable<Lance>
    {
        public int Id { get; set; }
        public float Valor { get; set; }

        //Foreign Key para Produto
        [ForeignKey("Produto")]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        //ForeignKey para Pessoa
        [ForeignKey("Pessoa")]
        public int PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }

        public int CompareTo(Lance l2)
        {
            if (Valor < l2.Valor)
                return -1;
            else if (Valor == l2.Valor)
                return 0;
            else
                return 1;
        }
    }
}
