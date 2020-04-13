using System.Collections.Generic;

namespace SGBB.Service.ViewModel
{
    public class BoletosViewModel
    {
        public int? CodigoBanco { get; set; }
        public ICollection<BoletoViewModel> Boletos { get; set; }
    }
}
