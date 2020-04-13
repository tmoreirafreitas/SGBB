using SGBB.Service.ViewModel.Enum;

namespace SGBB.Service.ViewModel
{
    public class ContaBancariaViewModel
    {
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string OperacaoConta { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string CarteiraPadrao { get; set; }
        public string VariacaoCarteiraPadrao { get; set; }
        public TipoCarteiraViewModel TipoCarteiraPadrao { get; set; }
        public TipoFormaCadastramentoViewModel TipoFormaCadastramento { get; set; }
        public TipoImpressaoBoletoViewModel TipoImpressaoBoleto { get; set; }
        public TipoDocumentoViewModel TipoDocumento { get; set; }
    }
}
