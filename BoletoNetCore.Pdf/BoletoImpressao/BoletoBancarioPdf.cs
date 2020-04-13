using System;

namespace BoletoNetCore.Pdf.BoletoImpressao
{
    public class BoletoBancarioPdf : BoletoBancario
    {
        public byte[] MontaBytesPDF(bool convertLinhaDigitavelToImage = false, string urlImagemLogoBeneficiario = null)
        {
#if NETSTANDARD2
            var html = MontaHtmlEmbedded(convertLinhaDigitavelToImage, true, urlImagemLogoBeneficiario);
            var htmlToPdf = new Wkhtmltopdf.NetCore.HtmlAsPdf();
            return htmlToPdf.GetPDF(html);
#else
            throw new NotImplementedException();
#endif


        }
    }
}
