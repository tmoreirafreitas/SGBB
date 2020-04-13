using Microsoft.AspNetCore.Mvc;
using RazorLight;
using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Rotativa.AspNetCore;

namespace HtmlToPdf.NetCore
{
    public class ViewAsPdf : HtmlAsPdf
    {
        private RazorLightEngine _engine;

        public ViewAsPdf(string viewsPath = null)
        {
            this._engine = new RazorLightEngineBuilder().UseFileSystemProject(viewsPath).UseMemoryCachingProvider().Build();
        }

        public ViewAsPdf()
        {
            this._engine = new RazorLightEngineBuilder().UseFileSystemProject(Directory.GetCurrentDirectory()).UseMemoryCachingProvider().Build();
        }

        public async Task<byte[]> GetByteArray<T>(string View, T model, ExpandoObject viewBag = null)
        {
            ViewAsPdf viewAsPdf = this;
            byte[] pdf;
            try
            {
                string html = await viewAsPdf._engine.CompileRenderAsync<T>(View, model, viewBag);
                pdf = viewAsPdf.GetPDF(html);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pdf;
        }

        public async Task<IActionResult> GetPdf<T>(
            string View,
            T model,
            ExpandoObject viewBag = null)
        {
            ViewAsPdf viewAsPdf = this;
            string html = await viewAsPdf._engine.CompileRenderAsync<T>(View, model, viewBag);
            byte[] pdf = viewAsPdf.GetPDF(html);
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(pdf, 0, pdf.Length);
            memoryStream.Position = 0L;
            return (IActionResult)new System.Web.Mvc.FileStreamResult((Stream)memoryStream, "application/pdf");
        }

        public async Task<byte[]> GetByteArrayViewInHtml<T>(string ViewInHtml, T model, ExpandoObject viewBag = null)
        {
            ViewAsPdf viewAsPdf = this;
            byte[] pdf;
            try
            {
                string html = await viewAsPdf._engine.CompileRenderAsync<T>("template", new { ViewInHtml, model }, viewBag);
                pdf = viewAsPdf.GetPDF(html);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pdf;
        }
    }
}
