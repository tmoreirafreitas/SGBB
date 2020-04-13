using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGBB.Service.ViewModel
{
    public class PagadorViewModel
    {
        public string Nome { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
        public EnderecoViewModel Endereco { get; set; } = new EnderecoViewModel();
        public string CPFCNPJ { get; set; }
    }
}
