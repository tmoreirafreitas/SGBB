using BoletoNetCore;
using BoletoNetCore.Pdf.BoletoImpressao;
using SGBB.Service.ViewModel;
using SGBB.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SGBB.Service
{
    public class GerenciadorBoletoBancario
    {
        public byte[] GerarBoletoPdf(BoletoViewModel model)
        {
            var novoBoletoBancarioPdf = new BoletoBancarioPdf();
            if (model == null) return null;
            var boleto = Popula(model);
            if (boleto != null)
            {
                novoBoletoBancarioPdf.Boleto = boleto;
            }
            else
            {
                return null;
            }

            return novoBoletoBancarioPdf.MontaBytesPDF();
        }

        public byte[] ObterBoletoHtml(BoletoViewModel model)
        {
            var boleto = Popula(model);
            return boleto?.ImprimirHtml();
        }

        public string MontaHtmlEmbedded(BoletoViewModel model)
        {
            var novoBoletoBancarioPdf = new BoletoBancarioPdf();
            if (model == null) return null;
            var boleto = Popula(model);
            if (boleto != null)
            {
                novoBoletoBancarioPdf.Boleto = boleto;
            }
            else
            {
                return null;
            }
            return novoBoletoBancarioPdf.MontaHtmlEmbedded();
        }

        public byte[] GerarArquivoRemessa(BoletosViewModel models)
        {
            var boletos = new Boletos();
            var boletoViewModels = models.Boletos;
            boletos.AddRange(boletoViewModels.Select(Popula));
            if (models.CodigoBanco != null) boletos.Banco = Banco.Instancia(models.CodigoBanco.Value);
            using (var ms = new MemoryStream())
            {
                var remessa = new ArquivoRemessa(boletos.Banco, TipoArquivo.CNAB400, 1);
                remessa.GerarArquivoRemessa(boletos, ms);
                ms.Position = 0;
                using (var lerArquivo = new StreamReader(ms))
                {
                    var sbRefazArquivo = new StringBuilder();
                    string strTexto;
                    int conta;
                    while (lerArquivo.Peek() != -1)
                    {
                        strTexto = lerArquivo.ReadLine();
                        if (strTexto != null)
                        {
                            conta = strTexto.Length;
                            if (conta < 240)
                            {
                                conta = 240 - conta;
                                string strEspaco = null;
                                for (int I = 1; I <= conta; I++)
                                    strEspaco = strEspaco + " ";
                                sbRefazArquivo.AppendLine(strTexto + strEspaco);
                            }
                            else
                                sbRefazArquivo.AppendLine(strTexto);
                        }
                    }

                    return sbRefazArquivo.ToString().ToBytes();
                }
            }
        }

        private Boleto Popula(BoletoViewModel model)
        {
            if (model.Banco.Codigo != null)
            {
                var banco = Banco.Instancia(model.Banco.Codigo.Value);
                banco.Beneficiario = new Beneficiario
                {
                    Codigo = model.Banco.Beneficiario.Codigo,
                    CPFCNPJ = model.Banco.Beneficiario.CPFCNPJ,
                    Nome = model.Banco.Beneficiario.Nome,
                    Observacoes = model.Banco.Beneficiario.Observacoes,
                    CodigoDV = model.Banco.Beneficiario.CodigoDV,
                    CodigoTransmissao = model.Banco.Beneficiario.CodigoTransmissao,
                    ContaBancaria = new ContaBancaria
                    {
                        Agencia = model.Banco.Beneficiario.ContaBancaria.Agencia,
                        DigitoAgencia = model.Banco.Beneficiario.ContaBancaria.DigitoAgencia,
                        OperacaoConta = model.Banco.Beneficiario.ContaBancaria.OperacaoConta,
                        Conta = model.Banco.Beneficiario.ContaBancaria.Conta,
                        DigitoConta = model.Banco.Beneficiario.ContaBancaria.DigitoConta,
                        CarteiraPadrao = model.Banco.Beneficiario.ContaBancaria.CarteiraPadrao,
                        VariacaoCarteiraPadrao = model.Banco.Beneficiario.ContaBancaria.VariacaoCarteiraPadrao,

                        TipoCarteiraPadrao = (TipoCarteira)Enum.ToObject(typeof(TipoCarteira),
                            (int)model.Banco.Beneficiario.ContaBancaria.TipoCarteiraPadrao),

                        TipoFormaCadastramento = (TipoFormaCadastramento)Enum.ToObject(typeof(TipoFormaCadastramento),
                            (int)model.Banco.Beneficiario.ContaBancaria.TipoFormaCadastramento),

                        TipoImpressaoBoleto = (TipoImpressaoBoleto)Enum.ToObject(typeof(TipoImpressaoBoleto),
                            (int)model.Banco.Beneficiario.ContaBancaria.TipoImpressaoBoleto),

                        TipoDocumento = (TipoDocumento)Enum.ToObject(typeof(TipoDocumento),
                            (int)model.Banco.Beneficiario.ContaBancaria.TipoDocumento)
                    },

                    Endereco = new Endereco
                    {
                        LogradouroEndereco = model.Banco.Beneficiario.Endereco.LogradouroEndereco,
                        LogradouroNumero = model.Banco.Beneficiario.Endereco.LogradouroNumero,
                        LogradouroComplemento = model.Banco.Beneficiario.Endereco.LogradouroComplemento,
                        Bairro = model.Banco.Beneficiario.Endereco.Bairro,
                        Cidade = model.Banco.Beneficiario.Endereco.Cidade,
                        UF = model.Banco.Beneficiario.Endereco.UF,
                        CEP = model.Banco.Beneficiario.Endereco.CEP
                    }
                };
                banco.FormataBeneficiario();

                // 'CRIAÇÃO DO TITULO
                var titulo = new Boleto(banco)
                {
                    Pagador = new Pagador()
                    {
                        CPFCNPJ = model.Pagador.CPFCNPJ,
                        Nome = model.Pagador.Nome,
                        Observacoes = model.Pagador.Observacoes,
                        Endereco = new Endereco
                        {
                            Bairro = model.Pagador.Endereco.Bairro,
                            CEP = model.Pagador.Endereco.CEP,
                            Cidade = model.Pagador.Endereco.Cidade,
                            UF = model.Pagador.Endereco.UF,
                            LogradouroEndereco = model.Pagador.Endereco.LogradouroEndereco,
                            LogradouroComplemento = model.Pagador.Endereco.LogradouroComplemento,
                            LogradouroNumero = model.Pagador.Endereco.LogradouroNumero,
                        }
                    },
                    CodigoMotivoOcorrencia = model.CodigoMotivoOcorrencia,
                    NumeroDocumento = model.NumeroDocumento,
                    NumeroControleParticipante = model.NumeroControleParticipante,
                    NossoNumero = model.NossoNumero,
                    DataEmissao = DateTime.Now.Date,
                    DataVencimento = DateTime.Now.Date.AddDays(30),
                    DataDesconto = DateTime.Now.Date.AddDays(30),
                    ValorDesconto = model.ValorDesconto,
                    ValorTitulo = model.ValorTitulo,
                    Aceite = model.Aceite,
                    EspecieDocumento =
                        (TipoEspecieDocumento)Enum.ToObject(typeof(TipoEspecieDocumento), (int)model.EspecieDocumento),

                    // PARTE DA MULTA
                    DataMulta = DateTime.Now.Date.AddDays(30),
                    PercentualMulta = 2,
                    ValorMulta = model.ValorTitulo * model.PercentualMulta / 100,
                    MensagemInstrucoesCaixa = $"Cobrar multa de {(model.ValorTitulo * model.PercentualMulta / 100).ToString("F", CultureInfo.InvariantCulture)} após a data de vencimento.",

                    // PARTE JUROS DE MORA
                    DataJuros = DateTime.Now.Date.AddDays(30),
                    PercentualJurosDia = 10m / 30,
                    ValorJurosDia = model.ValorTitulo * model.PercentualJurosDia / 100,
                    CodigoProtesto = TipoCodigoProtesto.NaoProtestar,
                    DiasProtesto = 0,
                    CodigoBaixaDevolucao = TipoCodigoBaixaDevolucao.NaoBaixarNaoDevolver,
                    DiasBaixaDevolucao = 0
                };

                var instrucoes = $"Cobrar juros de {titulo.PercentualJurosDia.ToString("F", CultureInfo.InvariantCulture)} por dia.";
                if (string.IsNullOrEmpty(titulo.MensagemInstrucoesCaixa))
                    titulo.MensagemInstrucoesCaixa = instrucoes;
                else
                    titulo.MensagemInstrucoesCaixa += Environment.NewLine + instrucoes;

                titulo.ValidarDados();
                return titulo;
            }
            return null;
        }
    }
}