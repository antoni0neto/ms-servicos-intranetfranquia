using DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_cad_produto_expedicao : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        EcomController eController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {
                CarregarExpedicao();
            }
        }

        #region "EXPEDICAO"
        private List<SP_OBTER_ECOM_PEDIDO_EXPEDICAOResult> ObterExpedicao()
        {
            var exp = eController.ObterPedidoExpedicao("", 'A').Where(p => p.TIPO_FRETE != "L").ToList();

            if (txtDataIni.Text != "")
                exp = exp.Where(p => p.DATA_ACAO >= Convert.ToDateTime(txtDataIni.Text)).ToList();

            if (txtDataFim.Text != "")
                exp = exp.Where(p => p.DATA_ACAO <= Convert.ToDateTime(txtDataFim.Text).AddDays(1).AddSeconds(-1)).ToList();

            if (ddlFretePago.SelectedValue != "")
                exp = exp.Where(p => p.TIPO_FRETE == ddlFretePago.SelectedValue).ToList();

            var operadora = ddlCarrier.SelectedValue;
            if (operadora != "")
            {
                exp = exp.Where(x => x.FORMA_ENTREGA.Contains(operadora)).ToList();
            }

            return exp;
        }
        private List<SP_OBTER_ECOM_PEDIDO_EXPEDICAO_LISTAPRODResult> ObterPedidoExpedicaoProduto()
        {
            var frete = ddlFretePago.SelectedValue.Trim();
            var dataIni = DateTime.Now.AddDays(-60);
            var dataFim = DateTime.Now.AddDays(7);

            if (txtDataIni.Text != "")
                dataIni = Convert.ToDateTime(txtDataIni.Text);

            if (txtDataFim.Text != "")
                dataFim = Convert.ToDateTime(txtDataFim.Text);

            var exp = eController.ObterPedidoExpedicaoProduto(frete, dataIni, dataFim);

            return exp;
        }
        private void CarregarExpedicao()
        {
            var exp = ObterExpedicao();

            gvExpedicao.DataSource = exp.Where(p => p.PROBLEMA == 'N');
            gvExpedicao.DataBind();

            gvExpedicaoProblema.DataSource = exp.Where(p => p.PROBLEMA == 'S'); ;
            gvExpedicaoProblema.DataBind();
        }
        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarExpedicao();
            }
            catch (Exception)
            {
            }
        }
        protected void gvExpedicao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PEDIDO_EXPEDICAOResult magExpedicao = e.Row.DataItem as SP_OBTER_ECOM_PEDIDO_EXPEDICAOResult;

                    if (magExpedicao != null)
                    {
                        Literal litDataPedido = e.Row.FindControl("litDataPedido") as Literal;
                        litDataPedido.Text = Convert.ToDateTime(magExpedicao.EMISSAO).ToString("dd/MM/yyyy");

                        Literal litQtdeProduto = e.Row.FindControl("litQtdeProduto") as Literal;
                        if (litQtdeProduto != null)
                            litQtdeProduto.Text = Convert.ToInt32(magExpedicao.QTDE_PRODUTO).ToString();

                        Literal litAbrirPedido = e.Row.FindControl("litAbrirPedido") as Literal;
                        litAbrirPedido.Text = CriarLinkPedido(magExpedicao.PEDIDO_EXTERNO);

                        Literal litDataAprovacao = e.Row.FindControl("litDataAprovacao") as Literal;
                        litDataAprovacao.Text = (magExpedicao.DATA_ACAO == null) ? "-" : Convert.ToDateTime(magExpedicao.DATA_ACAO).ToString("dd/MM/yyyy HH:mm");

                        Button btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (btBaixar != null)
                            btBaixar.CommandArgument = magExpedicao.PEDIDO;

                        Button btProblema = e.Row.FindControl("btProblema") as Button;
                        if (btProblema != null)
                            btProblema.CommandArgument = magExpedicao.PEDIDO_EXTERNO;

                        Button btOK = e.Row.FindControl("btOK") as Button;
                        if (btOK != null)
                            btOK.CommandArgument = magExpedicao.PEDIDO_EXTERNO;

                        ImageButton btVoltar = e.Row.FindControl("btVoltar") as ImageButton;
                        btVoltar.CommandArgument = magExpedicao.PEDIDO.Trim();

                        TextBox txtProblema = e.Row.FindControl("txtProblema") as TextBox;
                        if (txtProblema != null)
                            txtProblema.Text = magExpedicao.DESC_PROBLEMA;
                    }
                }
            }
        }

        protected void btVoltar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var pedido = ((ImageButton)sender).CommandArgument;

                var pedidoVenda = baseController.ObterPedido(pedido);
                if (pedidoVenda != null)
                {
                    baseController.AtualizarStatusPedido(pedido, 'E');
                    eController.ExcluirEcomAprovacaoHist(pedido);
                    var expedicao = eController.ObterMagentoExpedicaoPorPedido(pedido.Trim());
                    if (expedicao != null)
                    {
                        eController.ExcluirMagentoExpedicao(expedicao.CODIGO);
                    }
                    CarregarExpedicao();
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        private string CriarLinkPedido(string pedidoExterno)
        {
            var link = "../mod_ecomv2/ecomv2_pedido_mag_det.aspx?p=" + pedidoExterno.Trim();
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }
        protected void btBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                if (bt != null)
                {
                    string pedido = bt.CommandArgument;

                    //Abrir pop-up
                    string url = "fnAbrirTelaCadastroMaior('ecom_cad_produto_expedicao_pop.aspx?pedido=" + pedido + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private USUARIO ObterUsuario()
        {
            if (Session["USUARIO"] != null)
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];
                return usuario;
            }

            return null;
        }

        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                GerarRelatorio(REL.Pedido);
            }
            catch (Exception)
            {
            }
        }

        private void GerarRelatorio(REL rel)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "ECOM_PRODEXPEDICAO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(rel));
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
        private StringBuilder MontarRelatorioHTML(REL rel)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            if (rel == REL.Pedido)
                _texto = MontarExpedicaoPedido(_texto);
            else
                _texto = MontarExpedicaoProduto(_texto);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        private StringBuilder MontarCabecalho(StringBuilder _texto)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            string dia_da_semana = dtfi.GetDayName(DateTime.Now.DayOfWeek);

            _texto.AppendLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine(" <html>");
            _texto.AppendLine("     <head>");
            _texto.AppendLine("         <title>Expedição - Produtos</title>   ");
            _texto.AppendLine("         <meta charset='UTF-8'>          ");
            _texto.AppendLine("         <style type='text/css'>");
            _texto.AppendLine("             @media print");
            _texto.AppendLine("             {");
            _texto.AppendLine("                 .tdback");
            _texto.AppendLine("                 {");
            _texto.AppendLine("                     background-color: WindowFrame !important;");
            _texto.AppendLine("                     -webkit-print-color-adjust: exact;");
            _texto.AppendLine("                 }");
            _texto.AppendLine("             }");
            _texto.AppendLine("             .breakafter");
            _texto.AppendLine("             {");
            _texto.AppendLine("                 page-break-after: always;");
            _texto.AppendLine("             }");
            _texto.AppendLine("             .breakbefore");
            _texto.AppendLine("             {");
            _texto.AppendLine("                 page-break-before: always;");
            _texto.AppendLine("             }");

            _texto.AppendLine("         </style>");
            _texto.AppendLine("     </head>");
            _texto.AppendLine("");
            _texto.AppendLine("<body onLoad='window.print();'>");
            _texto.AppendLine("     <div id='fichaProdutoExp' align='center' style='border: 0px solid #000;'>");
            _texto.AppendLine("        <br />");
            _texto.AppendLine("        <br />");
            _texto.AppendLine("        <div align='center' style='border: 2px solid #000; background-color: transparent;");
            _texto.AppendLine("            width: 720pt;'>");
            _texto.AppendLine("            <table cellpadding='0' cellspacing='0' style='width: 721pt; padding: 0px; color: black;");
            _texto.AppendLine("                font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; background: white;");
            _texto.AppendLine("                white-space: nowrap; border: 1px solid #000; border-top: 2px solid #000;'>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td colspan='2'>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr style='line-height:15px; text-align:center;'>");
            _texto.AppendLine("                    <td colspan='2' style='width: 150px; text-align:center;'>");
            _texto.AppendLine("                         <h2>");
            _texto.AppendLine("                            EXPEDIÇÃO - PRODUTOS");
            _texto.AppendLine("                         </h2>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td colspan='2'>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("            </table>");

            return _texto;
        }
        private StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.AppendLine("            <table cellpadding='0' cellspacing='0' style='width: 721pt; padding: 0px; color: black;");
            _texto.AppendLine("                font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; background: white;");
            _texto.AppendLine("                white-space: nowrap; border: 1px solid #000; border-top: 2px solid #000;'>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td colspan='2'>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td colspan='2'>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("            </table>");
            _texto.AppendLine("        </div>");
            _texto.AppendLine("    </div>");
            _texto.AppendLine("</body>");
            _texto.AppendLine("</html>");

            return _texto;
        }
        private StringBuilder MontarExpedicaoPedido(StringBuilder _texto)
        {
            var exp = ObterExpedicao();

            var breakClass = "";
            var totalLinhaFolha = 0;

            //_texto.AppendLine("    <div id='divDetalhe" + d.PROD_DETALHE.ToString() + "' align='center' class='" + breakClass + "'>");

            var orderKey = "";
            foreach (var expp in exp)
            {
                _texto.AppendLine("    <div id='divDetalhe" + expp.PEDIDO.Trim().ToString() + "' align='center' class='" + breakClass + "'>");
                _texto.AppendLine("            <table cellpadding='0' cellspacing='0' style='width: 721pt; padding: 0px; color: black;");
                _texto.AppendLine("                font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; background: white;");
                _texto.AppendLine("                white-space: nowrap; border: 1px solid #000; border-top: 2px solid #000;'>");

                _texto.AppendLine("<tr style='line-height: 19px;'>");
                _texto.AppendLine("    <td colspan='2' style='padding: 15px 0px 0px 10px; border: 0px solid #000;'>");

                _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' style='width: 100%; padding: 0px;");
                _texto.AppendLine("            color: black; font-size: 12px; font-weight: 500; font-family: Arial, sans-serif;");
                _texto.AppendLine("            background: white; white-space: nowrap;'>");

                _texto.AppendLine("            <tr>");
                _texto.AppendLine("                <td style='width: 15%;' class='tdback'>");
                _texto.AppendLine("                    PEDIDO #" + expp.PEDIDO);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td style='width: 20%;' class='tdback'>");
                _texto.AppendLine("                    CLIFOR: " + expp.CLIFOR);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td style='width: 20%;' class='tdback'>");
                _texto.AppendLine("                    " + expp.CLIENTE_ATACADO);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td style='width: 20%;' class='tdback'>");
                _texto.AppendLine("                    R$ " + expp.VALOR_FRETE.ToString("###,###,##0.00"));
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td class='tdback'>");
                _texto.AppendLine("                    ENTREGA: " + expp.FORMA_ENTREGA);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td class='tdback'>");

                if (expp.ORDER_KEY.Contains("-"))
                    orderKey = expp.ORDER_KEY.Trim().Split('-')[1].Trim();
                _texto.AppendLine("                    MP: " + (orderKey ?? "-"));
                orderKey = "";

                _texto.AppendLine("                </td>");
                _texto.AppendLine("            </tr>");

                _texto.AppendLine("        </table>");


                _texto.AppendLine("        <span style='font-size: 13px;'></span>");
                _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' style='width: 100%; padding: 0px;");
                _texto.AppendLine("            color: black; font-size: 11px; font-weight: 500; font-family: Arial, sans-serif;");
                _texto.AppendLine("            background: white; white-space: nowrap;'>");
                _texto.AppendLine("            <tr style='line-height: 5px;'>");
                _texto.AppendLine("                <td colspan='6'>");
                _texto.AppendLine("                    &nbsp;");
                _texto.AppendLine("                </td>");
                _texto.AppendLine("            </tr>");
                _texto.AppendLine("            <tr>");
                _texto.AppendLine("                <td style='width: 15%;' class='tdback'>");
                _texto.AppendLine("                    Produto");
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td style='width: 20%;' class='tdback'>");
                _texto.AppendLine("                    Nome");
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td style='width: 20%;' class='tdback'>");
                _texto.AppendLine("                    Cor");
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td style='width: 20%;' class='tdback'>");
                _texto.AppendLine("                    Quantidade");
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td class='tdback'>");
                _texto.AppendLine("                    Tamanho");
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td class='tdback'>");
                _texto.AppendLine("                    &nbsp;");
                _texto.AppendLine("                </td>");
                _texto.AppendLine("            </tr>");

                var qtdeTotal = 0;
                var expProduto = eController.ObterProdutoPedidoExpedicao(expp.PEDIDO, "", "", "");
                foreach (var p in expProduto)
                {

                    _texto.AppendLine("            <tr>");
                    _texto.AppendLine("                <td>");
                    _texto.AppendLine("                    " + p.COLECAO + " - " + p.PRODUTO);
                    _texto.AppendLine("                </td>");
                    _texto.AppendLine("                <td>");
                    _texto.AppendLine("                    " + p.DESC_PRODUTO);
                    _texto.AppendLine("                </td>");
                    _texto.AppendLine("                <td>");
                    _texto.AppendLine("                    " + p.DESC_COR_PRODUTO);
                    _texto.AppendLine("                </td>");
                    _texto.AppendLine("                <td>");
                    _texto.AppendLine("                    " + p.QTDE.ToString());
                    _texto.AppendLine("                </td>");
                    _texto.AppendLine("                <td>");
                    _texto.AppendLine("                    " + p.TAMANHO);
                    _texto.AppendLine("                </td>");
                    _texto.AppendLine("                <td>");
                    _texto.AppendLine("                    &nbsp;");
                    _texto.AppendLine("                </td>");
                    _texto.AppendLine("            </tr>");

                    qtdeTotal += Convert.ToInt32(p.QTDE);
                    totalLinhaFolha += 1;
                }

                totalLinhaFolha += 1;

                _texto.AppendLine("            <tr>");
                _texto.AppendLine("                <td style='width: 15%;' class='tdback'>");
                _texto.AppendLine("                    DATA " + Convert.ToDateTime(expp.EMISSAO).ToString("dd/MM/yyyy"));
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td style='width: 20%;' class='tdback'>");
                _texto.AppendLine("                    -");
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td style='width: 20%;' class='tdback'>");
                _texto.AppendLine("                    -");
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td style='width: 20%;' class='tdback'>");
                _texto.AppendLine("                    " + qtdeTotal.ToString());
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td class='tdback'>");
                _texto.AppendLine("                    -");
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td class='tdback'>");
                _texto.AppendLine("                    &nbsp;");
                _texto.AppendLine("                </td>");
                _texto.AppendLine("            </tr>");

                totalLinhaFolha += 1;

                _texto.AppendLine(" ");
                _texto.AppendLine("        </table>");
                _texto.AppendLine("    </td>");
                _texto.AppendLine("</tr>");
                _texto.AppendLine("<tr>");
                _texto.AppendLine("    <td>");
                _texto.AppendLine("         <hr />");
                _texto.AppendLine("    </td>");
                _texto.AppendLine("</tr>");

                if (totalLinhaFolha >= 55)
                {
                    breakClass = "";
                    totalLinhaFolha = 0;
                }
                else
                {
                    breakClass = "";
                }

                _texto.AppendLine("        </table>");
                _texto.AppendLine("        </div>");
            }


            return _texto;
        }
        private StringBuilder MontarExpedicaoProduto(StringBuilder _texto)
        {
            var exp = ObterPedidoExpedicaoProduto();


            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' style='width: 100%; padding: 0px;");
            _texto.AppendLine("            color: black; font-size: 11px; font-weight: 500; font-family: Arial, sans-serif;");
            _texto.AppendLine("            background: white; white-space: nowrap;'>");
            _texto.AppendLine("            <tr style='line-height: 5px;'>");
            _texto.AppendLine("                <td colspan='11'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='width: 70px;' class='tdback'>");
            _texto.AppendLine("                    Produto");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td style='width: 140px;' class='tdback'>");
            _texto.AppendLine("                    Nome");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td style='width: 120px;' class='tdback'>");
            _texto.AppendLine("                    Cor");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td class='tdback'>");
            _texto.AppendLine("                    Griffe");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td style='width: 70px;' class='tdback'>");
            _texto.AppendLine("                    Q1");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td style='width: 70px;' class='tdback'>");
            _texto.AppendLine("                    Q2");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td style='width: 70px;' class='tdback'>");
            _texto.AppendLine("                    Q3");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td style='width: 70px;' class='tdback'>");
            _texto.AppendLine("                    Q4");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td style='width: 70px;' class='tdback'>");
            _texto.AppendLine("                    Q5");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td style='width: 70px;' class='tdback'>");
            _texto.AppendLine("                    Q6");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td style='width: 70px;' class='tdback'>");
            _texto.AppendLine("                    Q7");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            foreach (var p in exp)
            {
                _texto.AppendLine("            <tr>");
                _texto.AppendLine("                <td>");
                _texto.AppendLine("                    " + p.PRODUTO);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td>");
                _texto.AppendLine("                    " + p.DESC_PRODUTO);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td>");
                _texto.AppendLine("                    " + p.COR_PRODUTO);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td>");
                _texto.AppendLine("                    " + p.GRIFFE.ToString());
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td>");
                _texto.AppendLine("                    " + p.QTDE_01);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td>");
                _texto.AppendLine("                    " + p.QTDE_02);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td>");
                _texto.AppendLine("                    " + p.QTDE_03);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td>");
                _texto.AppendLine("                    " + p.QTDE_04);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td>");
                _texto.AppendLine("                    " + p.QTDE_05);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td>");
                _texto.AppendLine("                    " + p.QTDE_06);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("                <td>");
                _texto.AppendLine("                    " + p.QTDE_07);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("            </tr>");
            }

            _texto.AppendLine("        </table>");
            return _texto;
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            try
            {
                var pedidoExterno = ((Button)sender).CommandArgument;
                var pedidoMag = eController.ObterPedidoMag(pedidoExterno);

                if (pedidoMag != null)
                {
                    pedidoMag.PROBLEMA = 'N';
                    pedidoMag.DESC_PROBLEMA = "";
                    eController.AtualizarPedidoMag(pedidoMag);
                    CarregarExpedicao();
                }
            }
            catch (Exception)
            {
            }
        }

        protected void btProblema_Click(object sender, EventArgs e)
        {
            try
            {
                var pedidoExterno = ((Button)sender).CommandArgument;
                var pedidoMag = eController.ObterPedidoMag(pedidoExterno);

                if (pedidoMag != null)
                {
                    pedidoMag.PROBLEMA = 'S';
                    eController.AtualizarPedidoMag(pedidoMag);
                    CarregarExpedicao();
                }
            }
            catch (Exception)
            {
            }
        }

        protected void txtProblema_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var txtDescProblema = ((TextBox)sender);

                GridViewRow row = (GridViewRow)txtDescProblema.NamingContainer;
                if (row != null)
                {
                    string pedidoExterno = gvExpedicaoProblema.DataKeys[row.RowIndex].Value.ToString();
                    var pedidoMag = eController.ObterPedidoMag(pedidoExterno);
                    if (pedidoMag != null)
                    {
                        pedidoMag.DESC_PROBLEMA = txtDescProblema.Text;
                        eController.AtualizarPedidoMag(pedidoMag);
                        //CarregarExpedicao();
                    }

                }
            }
            catch (Exception)
            {
            }
        }

        protected void btImprimirPorProduto_Click(object sender, EventArgs e)
        {
            try
            {
                GerarRelatorio(REL.Produto);
            }
            catch (Exception)
            {
            }
        }
    }
}

public enum REL
{
    Pedido,
    Produto
}