using SGBB.Service.ViewModel.Enum;

namespace SGBB.Service.ViewModel
{
    public class BoletoViewModel
    {
        public string CodigoMotivoOcorrencia { get; set; }
        public string NumeroDocumento { get; set; }
        public string NumeroControleParticipante { get; set; }
        public string NossoNumero { get; set; }
        public decimal ValorTitulo { get; set; }
        public string Aceite { get; set; }
        public TipoEspecieDocumentoViewModel EspecieDocumento { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal PercentualMulta { get; set; }
        public decimal PercentualJurosDia { get; set; }
        public BancoViewModel Banco { get; set; }
        public PagadorViewModel Pagador { get; set; }
        public PagadorViewModel Avalista { get; set; }
    }
}
