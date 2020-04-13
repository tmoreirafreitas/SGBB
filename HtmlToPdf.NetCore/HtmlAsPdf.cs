namespace HtmlToPdf.NetCore
{
    public class HtmlAsPdf : AsPdfResultBase
    {
        public byte[] GetPDF(string html)
        {
            return HtmlDriver.Convert(RotativaConfiguration.RotativaPath, this.GetConvertOptions(), html);
        }
    }
}
