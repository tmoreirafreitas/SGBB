namespace SGBB.Service.ViewModel
{
    public class BeneficiarioViewModel
    {
        public string CPFCNPJ { get; set; }
        public string Nome { get; set; }
        public string Observacoes { get; set; }
        public string Codigo { get; set; }
        public string CodigoDV { get; set; }
        public string CodigoTransmissao { get; set; }
        public ContaBancariaViewModel ContaBancaria { get; set; }
        public EnderecoViewModel Endereco { get; set; }
    }
}
