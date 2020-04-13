namespace SGBB.Service.ViewModel
{
    public class BancoViewModel
    {
        public int? Codigo { get; set; }
        public string Nome { get; set; }
        public string Digito { get; set; }
        public BeneficiarioViewModel Beneficiario { get; set; }
    }
}
