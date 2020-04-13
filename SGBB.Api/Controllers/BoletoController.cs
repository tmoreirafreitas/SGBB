using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGBB.Service;
using SGBB.Service.ViewModel;
using SGBB.Utilities;
using System;
using System.Threading.Tasks;

namespace SGBB.Api.Controllers
{
    [Consumes("application/json")]
    [Route("api/boleto")]
    [ApiController]
    public class BoletoController : ControllerBase
    {
        private readonly GerenciadorBoletoBancario _gerenciador;

        public BoletoController(GerenciadorBoletoBancario gerenciador)
        {
            _gerenciador = gerenciador;
        }
        // GET: api/Boleto
        [HttpPost("gerar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterBoleto([FromBody] BoletoViewModel model)
        {
            return await Task.Run(() =>
            {
                var dataBytes = _gerenciador.GerarBoletoPdf(model);
                var nomeArquivo =
                    $"Boleto_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}" +
                    $"{DateTime.Now.Minute}{DateTime.Now.Second}";
                HttpContext.Response.Headers.Add("Content-Type", "application/pdf");
                HttpContext.Response.Headers.Add("Content-Filename", nomeArquivo);
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Filename");
                HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{nomeArquivo}.pdf\"");
                var ms = dataBytes.ToStream();
                return File(ms, "application/pdf", $"{nomeArquivo}.pdf");
            });
        }

        [HttpPost("remessa/{codigoBanco}")]
        public async Task<IActionResult> GerarRemessa(int codigoBanco, [FromBody] BoletosViewModel model)
        {
            return await Task.Run(() =>
            {
                model.CodigoBanco = codigoBanco;
                var dataBytes = _gerenciador.GerarArquivoRemessa(model);
                var nomeArquivo =
                    $"Boleto_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}" +
                    $"{DateTime.Now.Minute}{DateTime.Now.Second}";
                HttpContext.Response.Headers.Add("Content-Type", "application/octet-stream");
                HttpContext.Response.Headers.Add("Content-Filename", nomeArquivo);
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Filename");
                HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{nomeArquivo}.txt\"");
                return File(dataBytes.ToStream(), "application/pdf", $"{nomeArquivo}.txt");
            });
        }
    }
}
