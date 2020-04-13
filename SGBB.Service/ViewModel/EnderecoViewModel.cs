using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGBB.Service.ViewModel
{
    public class EnderecoViewModel
    {
        public string LogradouroEndereco { get; set; }
        public string LogradouroNumero { get; set; }
        public string LogradouroComplemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
    }
}
