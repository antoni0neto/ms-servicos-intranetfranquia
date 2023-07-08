using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Collections;
using Relatorios.mod_ecom.mag;
using Relatorios.mod_ecomv2.mag2;

namespace Relatorios
{
    public partial class ecom_cad_produto_coleta : System.Web.UI.Page
    {
        EcomController eController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtTrackNumber.Attributes.Add("onKeyPress", "doClick('" + btGravar.ClientID + "', event)");
            txtTrackNumber.Focus();

            if (!Page.IsPostBack)
            {
                CarregarColeta();
            }
        }

        #region "COLETA"
        private void CarregarColeta()
        {
            var coletas = eController.ObterCorreiosColetaAberto();

            var coletasCorreio = new List<ECOM_CORREIOS_COLETA>();
            coletasCorreio.AddRange(coletas.Where(p =>
                                                    !p.TRACK_NUMBER.ToLower().Trim().Contains("hand") &&
                                                    p.TRACK_NUMBER.ToLower().Trim().Substring(0, 2) != "lg"));
            gvColetaCorreio.DataSource = coletasCorreio;
            gvColetaCorreio.DataBind();

            var coletasLoggi = new List<ECOM_CORREIOS_COLETA>();
            coletasLoggi.AddRange(coletas.Where(p => p.TRACK_NUMBER.ToLower().Trim().Substring(0, 2) == "lg"));
            gvColetaLoggi.DataSource = coletasLoggi;
            gvColetaLoggi.DataBind();

            var coletasMandae = new List<ECOM_CORREIOS_COLETA>();
            coletasMandae.AddRange(coletas.Where(p => p.TRACK_NUMBER.ToLower().Trim().Contains("hand")));
            gvColetaMandae.DataSource = coletasMandae;
            gvColetaMandae.DataBind();
        }
        protected void gvColeta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_CORREIOS_COLETA coleta = e.Row.DataItem as ECOM_CORREIOS_COLETA;

                    if (coleta != null)
                    {
                        Literal _litDataColeta = e.Row.FindControl("litDataColeta") as Literal;
                        if (_litDataColeta != null)
                            _litDataColeta.Text = coleta.DATA_COLETA.ToString("dd/MM/yyyy");

                    }
                }
            }
        }
        protected void btGravar_Click(object sender, EventArgs e)
        {
            try
            {
                string trackNumber = "";

                if (ObterUsuario() == null)
                {
                    Response.Redirect("../Login.aspx");
                    return;
                }

                trackNumber = txtTrackNumber.Text;

                if (trackNumber == "")
                {
                    labErro.Text = "Informe o Código de Rastreio...";
                    return;
                }

                var correioTrackNumber = eController.ObterCorreiosPostTrackNumber(trackNumber);
                if (correioTrackNumber != null)
                {
                    var correioColetaTrackNumber = eController.ObterCorreiosColetaTrackNumber(trackNumber);
                    if (correioColetaTrackNumber == null)
                    {
                        var correioColeta = new ECOM_CORREIOS_COLETA();
                        correioColeta.PEDIDO = correioTrackNumber.PEDIDO;
                        correioColeta.TRACK_NUMBER = trackNumber;
                        correioColeta.DATA_COLETA = DateTime.Today;

                        eController.InserirCorreiosColeta(correioColeta);

                        CarregarColeta();
                    }
                    else
                    {
                        labErro.Text = "Código de Rastreio já foi inserido. Entre em contato com o Suporte.";
                    }
                }
                else
                {
                    labErro.Text = "Código de Rastreio não encontrado. Verifique o código novamente ou entre em contato com Suporte.";
                }

                txtTrackNumber.Text = "";
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        private USUARIO ObterUsuario()
        {
            if (Session["USUARIO"] != null)
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];
                return usuario;
            }

            return null;
        }

        protected void btVerHoje_Click(object sender, EventArgs e)
        {
            var coletas = eController.ObterCorreiosColetaHoje();

            foreach (var c in coletas)
            {
                c.DATA_CONTROLE = null;
                eController.AtualizarCorreiosColeta(c);
            }

            CarregarColeta();
        }
        protected void btLimpar_Click(object sender, EventArgs e)
        {
            var coletas = eController.ObterCorreiosColetaHoje();

            foreach (var c in coletas)
            {
                c.DATA_CONTROLE = DateTime.Today;
                eController.AtualizarCorreiosColeta(c);
            }

            CarregarColeta();
        }

        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                var itens = new List<SP_OBTER_ROMANEIO_MANDAEResult>();

                foreach (GridViewRow r in gvColetaCorreio.Rows)
                {
                    CheckBox cb = r.FindControl("cbMarcar") as CheckBox;
                    if (cb.Checked)
                    {
                        var trackNumber = gvColetaCorreio.DataKeys[r.RowIndex].Value.ToString();

                        var item = eController.ObterItemRomaneioMandae(trackNumber);
                        if (item != null)
                        {
                            itens.Add(new SP_OBTER_ROMANEIO_MANDAEResult
                            {
                                CLIENTE_ATACADO = item.CLIENTE_ATACADO,
                                TRACK_NUMBER = item.TRACK_NUMBER,
                                FORMA_ENTREGA = item.FORMA_ENTREGA,
                                PEDIDO_EXTERNO = item.PEDIDO_EXTERNO,
                                NF_SAIDA = item.NF_SAIDA
                            });
                        }
                    }
                }

                if (itens != null && itens.Count() > 0)
                    GerarRelatorio(itens, CarrierCode.correios);
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void btImprimirMandae_Click(object sender, EventArgs e)
        {
            try
            {
                var itens = new List<SP_OBTER_ROMANEIO_MANDAEResult>();

                foreach (GridViewRow r in gvColetaMandae.Rows)
                {
                    CheckBox cb = r.FindControl("cbMarcar") as CheckBox;
                    if (cb.Checked)
                    {
                        var trackNumber = gvColetaMandae.DataKeys[r.RowIndex].Value.ToString();
                        var item = eController.ObterItemRomaneioMandae(trackNumber);
                        if (item != null)
                        {
                            itens.Add(new SP_OBTER_ROMANEIO_MANDAEResult
                            {
                                CLIENTE_ATACADO = item.CLIENTE_ATACADO,
                                TRACK_NUMBER = item.TRACK_NUMBER,
                                FORMA_ENTREGA = item.FORMA_ENTREGA,
                                PEDIDO_EXTERNO = item.PEDIDO_EXTERNO,
                                NF_SAIDA = item.NF_SAIDA
                            });
                        }
                    }
                }

                if (itens != null && itens.Count() > 0)
                    GerarRelatorio(itens, CarrierCode.mandae);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected void btImprimirLoggi_Click(object sender, EventArgs e)
        {
            try
            {
                var itens = new List<SP_OBTER_ROMANEIO_MANDAEResult>();

                foreach (GridViewRow r in gvColetaLoggi.Rows)
                {
                    CheckBox cb = r.FindControl("cbMarcar") as CheckBox;
                    if (cb.Checked)
                    {
                        var trackNumber = gvColetaLoggi.DataKeys[r.RowIndex].Value.ToString();

                        var item = eController.ObterItemRomaneioMandae(trackNumber);
                        if (item != null)
                        {
                            itens.Add(new SP_OBTER_ROMANEIO_MANDAEResult
                            {
                                CLIENTE_ATACADO = item.CLIENTE_ATACADO,
                                TRACK_NUMBER = item.TRACK_NUMBER,
                                FORMA_ENTREGA = item.FORMA_ENTREGA,
                                PEDIDO_EXTERNO = item.PEDIDO_EXTERNO,
                                NF_SAIDA = item.NF_SAIDA
                            });
                        }

                    }
                }

                if (itens != null && itens.Count() > 0)
                    GerarRelatorio(itens, CarrierCode.loggi);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GerarRelatorio(List<SP_OBTER_ROMANEIO_MANDAEResult> itens, CarrierCode carrierCode)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "ECOM_COLETA_MANDAE_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(itens, carrierCode));
                wr.Flush();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        private StringBuilder MontarRelatorioHTML(List<SP_OBTER_ROMANEIO_MANDAEResult> itens, CarrierCode carrierCode)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarHTML(_texto, itens, carrierCode);

            return _texto;
        }
        private StringBuilder MontarHTML(StringBuilder _texto, List<SP_OBTER_ROMANEIO_MANDAEResult> itens, CarrierCode carrierCode)
        {

            var urlImagem = "";
            var carrier = "CORREIOS";

            if (carrierCode == CarrierCode.mandae)
            {
                urlImagem = "http://cmaxweb.dnsalias.com:8585/image/mandae_logo.jpg";
                carrier = "MANDAÊ";
            }
            else if(carrierCode == CarrierCode.loggi)
            {
                urlImagem = "http://cmaxweb.dnsalias.com:8585/image/loggi_logo.png";
                carrier = "LOGGI";
            }

            _texto.AppendLine("            <!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine("            <html>");
            _texto.AppendLine("            <head>");
            _texto.AppendLine("                <title>Mandaê</title>");
            _texto.AppendLine("                <meta charset='UTF-8' />");
            _texto.AppendLine("                <style type='text/css'>");
            _texto.AppendLine("                    @media print {");
            _texto.AppendLine("                        .background-force {");
            _texto.AppendLine("                            -webkit-print-color-adjust: exact;");
            _texto.AppendLine("                        }");
            _texto.AppendLine("                    }");
            _texto.AppendLine("                </style>");
            _texto.AppendLine("            </head>");
            _texto.AppendLine("            <body onload='window.print();'>");
            _texto.AppendLine("                <div id='divMandae' align='center'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='width: 689pt; padding: 0px; color: black; font-size: 8.0pt; font-weight: 600; font-family: Arial; background: white; white-space: nowrap;'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='line-height: 23px;'>");
            _texto.AppendLine("                                <table border='1' cellpadding='1' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                                    width: 689pt'>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='6' style='text-align: center; border-right: 1px solid #000; border-left: 1px solid #000; border-bottom:none;'>");
            _texto.AppendLine("                                            <div style='position:absolute;'>");
            _texto.AppendLine("                                                <img alt='' width='200' src='" + urlImagem + "' />");
            _texto.AppendLine("                                            </div>");
            _texto.AppendLine("                                            <h1>");
            _texto.AppendLine("                                                LISTA DE POSTAGEM " + carrier + "<br />" + DateTime.Today.ToString("dd/MM/yyyy"));
            _texto.AppendLine("                                            </h1>");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='6' style='line-height: 10px; border-top: none; border-bottom: none; '>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='6' style='line-height: 30px; border-top: none; border-bottom: none; '>");
            _texto.AppendLine("                                            <h2>");
            _texto.AppendLine("                                                &nbsp;&nbsp;&nbsp;Nome do Cliente: REWORKED COMERCIO DE ROUPAS EIRELI EPP<br />&nbsp;&nbsp;&nbsp;CNPJ: 28.337.789/0001-36");
            _texto.AppendLine("                                            </h2>");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='6' style='line-height: 20px; border-top: none; border-bottom: none; text-align:center; '>");
            _texto.AppendLine("                                            __________________________________________________________________________________<br />");
            _texto.AppendLine("                                            Nome Completo<br /><br />");
            _texto.AppendLine("                                            __________________________________________________________________________________<br />");
            _texto.AppendLine("                                            CPF<br /><br />");
            _texto.AppendLine("                                            __________________________________________________________________________________<br />");
            _texto.AppendLine("                                            Assinatura<br />");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='6' style='line-height: 40px; border-top: none; border-bottom: none; '>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='text-align: center; border-top: 1px solid #000; border-bottom: 1px solid #000; background-color: #FAF0E6;' class='background-force'>");
            _texto.AppendLine("                                        <td style='width: 50px; border-right: 1px solid #000; text-align:center;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-left: 1px solid #000; border-right: 1px solid #000; text-align: left;'>");
            _texto.AppendLine("                                            Destinatário");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 200px; border-right: 1px solid #000; text-align:center;'>");
            _texto.AppendLine("                                            Rastreamento");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 180px; border-right: 1px solid #000; text-align: left; '>");
            _texto.AppendLine("                                            Serviço de Envio");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 200px; border-right: 1px solid #000; text-align: center; '>");
            _texto.AppendLine("                                            Pedido Externo");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            Nota Fiscal");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");

            int count = 1;
            foreach (var it in itens)
            {
                _texto.AppendLine("                                    <tr style='text-align: left; border-bottom: 1px solid #000;'>");
                _texto.AppendLine("                                        <td style='width: 50px; border-right: 1px solid #000; text-align:center;'>");
                _texto.AppendLine("                                            " + count.ToString());
                _texto.AppendLine("                                        </td>");
                _texto.AppendLine("                                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
                _texto.AppendLine("                                            &nbsp;" + it.CLIENTE_ATACADO);
                _texto.AppendLine("                                        </td>");
                _texto.AppendLine("                                        <td style='border-right: 1px solid #000; text-align:center;'>");
                _texto.AppendLine("                                            &nbsp;" + it.TRACK_NUMBER);
                _texto.AppendLine("                                        </td>");
                _texto.AppendLine("                                        <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:left;'>");
                _texto.AppendLine("                                            &nbsp;" + it.FORMA_ENTREGA);
                _texto.AppendLine("                                        </td>");
                _texto.AppendLine("                                        <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:center;'>");
                _texto.AppendLine("                                            &nbsp;" + it.PEDIDO_EXTERNO);
                _texto.AppendLine("                                        </td>");
                _texto.AppendLine("                                        <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:center;'>");
                _texto.AppendLine("                                            &nbsp;" + it.NF_SAIDA);
                _texto.AppendLine("                                        </td>");
                _texto.AppendLine("                                    </tr>");

                count = count + 1;
            }

            _texto.AppendLine("                                </table>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </div>");
            _texto.AppendLine("            </body>");
            _texto.AppendLine("            </html>");

            return _texto;

        }

        protected void cbMarcarTodos_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            foreach (GridViewRow r in gvColetaCorreio.Rows)
            {
                CheckBox cbM = r.FindControl("cbMarcar") as CheckBox;
                cbM.Checked = cb.Checked;
            }
        }
        protected void cbMarcarTodosMandae_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            foreach (GridViewRow r in gvColetaMandae.Rows)
            {
                CheckBox cbM = r.FindControl("cbMarcar") as CheckBox;
                cbM.Checked = cb.Checked;
            }
        }
        protected void cbMarcarTodosLoggi_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            foreach (GridViewRow r in gvColetaLoggi.Rows)
            {
                CheckBox cbM = r.FindControl("cbMarcar") as CheckBox;
                cbM.Checked = cb.Checked;
            }
        }
    }
}