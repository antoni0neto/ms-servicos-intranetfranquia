using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;
using System.Text;

namespace Relatorios
{
    public partial class pacab_pedido_produto_editv2 : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        int qtdeProduto = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataEntrega.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {

                var fornecedor = Request.QueryString["f"].ToString();

                CarregarFilial();
                CarregarFabricante(fornecedor);
                CarregarCarrinho();

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFabricante(string fornecedor)
        {
            var fornecedores = prodController.ObterFornecedor();

            fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });
            fornecedores.Where(p => ((p.STATUS == 'A' && p.TIPO == 'C') || p.STATUS == 'S')).GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (fornecedores != null)
            {
                ddlFabricante.DataSource = fornecedores;
                ddlFabricante.DataBind();

                if (fornecedor != "")
                    ddlFabricante.SelectedValue = fornecedores.Where(p => p.FORNECEDOR.Trim() == fornecedor.Trim()).FirstOrDefault().FORNECEDOR;

            }
        }
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais();

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                filial.Insert(1, new FILIAI { COD_FILIAL = "1055", FILIAL = "C-MAX (NOVA)" });
                filial.Insert(2, new FILIAI { COD_FILIAL = "000029", FILIAL = "CD - LUGZY               " });
                filial.Insert(3, new FILIAI { COD_FILIAL = "000041", FILIAL = "CD MOSTRUARIO            " });
                filial.Insert(4, new FILIAI { COD_FILIAL = "1029", FILIAL = "ATACADO HANDBOOK         " });
                filial.Insert(5, new FILIAI { COD_FILIAL = "1054", FILIAL = "HANDBOOK ONLINE          " });

                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();

                ddlFilial.Enabled = true;
                if (filial.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFornecedor.ForeColor = _OK;
            if (ddlFabricante.SelectedValue.Trim() == "Selecione")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labDataEntrega.ForeColor = _OK;
            if (txtDataEntrega.Text.Trim() == "")
            {
                labDataEntrega.ForeColor = _notOK;
                retorno = false;
            }

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue == "")
            {
                labFilial.ForeColor = _notOK;
                retorno = false;
            }

            labStatus.ForeColor = _OK;
            if (ddlStatus.SelectedValue.Trim() == "")
            {
                labStatus.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarGrade()
        {
            var carrinho = desenvController.ObterCarrinhoProdutoPorUsuario(((USUARIO)Session["USUARIO"]).CODIGO_USUARIO).Where(p => p.GRADE_TOTAL <= 0);
            if (carrinho != null && carrinho.Count() > 0)
                return false;
            return true;
        }
        private bool ValidarCusto()
        {
            var carrinho = desenvController.ObterCarrinhoProdutoPorUsuario(((USUARIO)Session["USUARIO"]).CODIGO_USUARIO);

            foreach (var c in carrinho)
            {
                var p = desenvController.ObterProduto(c.DESENV_PRODUTO);
                if (p != null)
                {
                    if (p.CUSTO <= 0 || p.CUSTO == null)
                        return false;
                }
            }

            return true;
        }
        #endregion

        private void CarregarCarrinho()
        {
            var carrinho = desenvController.ObterCarrinhoProdutoPorUsuario(((USUARIO)Session["USUARIO"]).CODIGO_USUARIO);
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO_CARRINHOV2 carrinho = e.Row.DataItem as DESENV_PRODUTO_CARRINHOV2;

                    if (carrinho != null)
                    {
                        var produto = desenvController.ObterProduto(carrinho.DESENV_PRODUTO);

                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = produto.MODELO;

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = baseController.BuscaProduto(produto.MODELO).DESC_PRODUTO.Trim();

                        Literal litCor = e.Row.FindControl("litCor") as Literal;
                        litCor.Text = prodController.ObterCoresBasicas(produto.COR).DESC_COR;

                        Literal litRefFabricante = e.Row.FindControl("litRefFabricante") as Literal;
                        litRefFabricante.Text = produto.FORNECEDOR;

                        Literal litCusto = e.Row.FindControl("litCusto") as Literal;
                        litCusto.Text = (produto.CUSTO == null) ? "" : ("R$ " + produto.CUSTO.ToString());

                        Literal litGradeTotal = e.Row.FindControl("litGradeTotal") as Literal;
                        litGradeTotal.Text = carrinho.GRADE_TOTAL.ToString();

                        Button _btAdicionarGrade = e.Row.FindControl("btAdicionarGrade") as Button;
                        if (_btAdicionarGrade != null)
                            _btAdicionarGrade.CommandArgument = carrinho.CODIGO.ToString();

                        Button _btExcluirCarrinho = e.Row.FindControl("btExcluirCarrinho") as Button;
                        if (_btExcluirCarrinho != null)
                            _btExcluirCarrinho.CommandArgument = carrinho.CODIGO.ToString();

                        if (carrinho.GRADE_TOTAL > 0 && produto.CUSTO > 0)
                            e.Row.BackColor = Color.PaleGreen;

                        qtdeProduto += carrinho.GRADE_TOTAL;

                    }
                }
            }
        }
        protected void gvCarrinho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCarrinho.FooterRow;
            if (footer != null)
            {

                footer.Cells[1].Text = "Total";
                footer.Cells[6].Text = qtdeProduto.ToString();
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            string fornecedor;
            DateTime limiteEntrega = DateTime.Now.Date;
            string filial;
            string codigoFilialRateio;
            char status;
            string aprovadoPor;

            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Por favor, preencher os campos em vermelho.";
                    return;
                }

                if (!ValidarGrade())
                {
                    labErro.Text = "Por favor, preencher a grade de todos os produtos.";
                    return;
                }

                if (!ValidarCusto())
                {
                    labErro.Text = "Por favor, preencher o custo de todos os produtos.";
                    return;
                }

                fornecedor = ddlFabricante.SelectedValue.Trim();
                limiteEntrega = Convert.ToDateTime(txtDataEntrega.Text);
                filial = ddlFilial.SelectedItem.Text.Trim();
                codigoFilialRateio = ddlFilial.SelectedValue;
                status = 'A';

                var usuario = (USUARIO)Session["USUARIO"];
                if (usuario != null)
                    aprovadoPor = usuario.NOME_USUARIO.Trim();
                else
                    aprovadoPor = "USUARIO-INTRANET";

                var pedidoCompra = desenvController.GerarINTRAPrePedidoCompraProduto(fornecedor, limiteEntrega, filial, codigoFilialRateio, status, aprovadoPor, usuario.CODIGO_USUARIO);

                if (pedidoCompra != null && pedidoCompra.NUMERO_PEDIDO != "")
                {
                    labErro.Text = "Pré-Pedido de Compra " + ddlFilial.SelectedItem.Text.Trim() + " gerado com sucesso na Intranet: " + pedidoCompra.NUMERO_PEDIDO;

                    btSalvar.Enabled = false;


                    var prepedidoIntra = desenvController.ObterComprasProdutoPrePedido(pedidoCompra.NUMERO_PEDIDO).Where(p => p.DATA_EXCLUSAO == null && (p.DATA_BAIXA_TAG == null || p.DATA_BAIXA_AVIAMENTO == null || p.OBS != "")).ToList();
                    if (prepedidoIntra != null && prepedidoIntra.Count() > 0)
                        EnviarEmail(pedidoCompra.NUMERO_PEDIDO);
                }
                else
                {
                    labErro.Text = "Erro ao gerar pedido. Contate o TI.";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btNovoPedido_Click(object sender, EventArgs e)
        {
            try
            {
                btSalvar.Enabled = true;
                ddlFilial.SelectedValue = "";
                labErro.Text = "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void EnviarEmail(string pedidoIntra)
        {

            try
            {
                var relatorioPedido = MontarRelatorioHTML(pedidoIntra);

                USUARIO usuario = (USUARIO)Session["USUARIO"];

                email_envio email = new email_envio();
                string assunto = "";
                assunto = "Intranet: Pedido de Produto Acabado: " + pedidoIntra.Trim() + " - " + DateTime.Today.ToString("dd/MM/yyyy");
                email.ASSUNTO = assunto;
                email.REMETENTE = usuario;
                email.MENSAGEM = relatorioPedido.ToString();

                List<string> destinatario = new List<string>();
                //Adiciona e-mails 
                var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(33, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                foreach (var usu in usuarioEmail)
                    if (usu != null)
                        destinatario.Add(usu.EMAIL);
                //Adicionar remetente
                destinatario.Add(usuario.EMAIL);

                email.DESTINATARIOS = destinatario;

                if (destinatario.Count > 0)
                    email.EnviarEmail();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }
        private StringBuilder MontarRelatorioHTML(string pedidoIntra)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto, "Pedido de Compra - Produto Acabado");
            _texto = MontarPedidoCompra(_texto, pedidoIntra);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        public StringBuilder MontarCabecalho(StringBuilder _texto, string titulo)
        {
            _texto.AppendLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine(" <html>");
            _texto.AppendLine("     <head>");
            _texto.AppendLine("         <title>" + titulo + "</title>   ");
            _texto.AppendLine("         <meta charset='UTF-8'>          ");
            _texto.AppendLine("         <style type='text/css'>");
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
            return _texto;
        }
        public StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.AppendLine("</body></html>");
            return _texto;
        }
        public StringBuilder MontarPedidoCompra(StringBuilder _texto, string pedidoIntra)
        {

            int qtdeTotal = 0;

            int co1 = 0;
            int co2 = 0;
            int co3 = 0;
            int co4 = 0;
            int co5 = 0;
            int co6 = 0;
            int co7 = 0;
            int co8 = 0;

            var prepedidoIntra = desenvController.ObterComprasProdutoPrePedido(pedidoIntra).Where(p => p.DATA_EXCLUSAO == null).ToList();

            _texto.Append("");
            _texto.Append("<br />");
            _texto.Append("<br />");
            _texto.Append("");
            _texto.Append("<div id='divPrePedido' align='left'>");
            _texto.Append("    <table border='0' cellpadding='0' cellspacing='0' style='width: 1000pt; padding: 0px;");
            _texto.Append("                    color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("                    background: white; white-space: nowrap;'>");
            _texto.Append("        <tr>");
            _texto.Append("            <td style='line-height: 20px;'>");
            _texto.Append("                <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.Append("                                width: 1000pt'>");
            _texto.Append("                    <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
            _texto.Append("                        <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp; Produto");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp; Cor");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp; Itens");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp; Obs");
            _texto.Append("                        </td>");

            var gradeTamanho = desenvController.ObterProdutoAcabadoIntranetTamanho(prepedidoIntra[0].PEDIDO, prepedidoIntra[0].PRODUTO, prepedidoIntra[0].COR_PRODUTO, prepedidoIntra[0].ENTREGA);

            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + ((gradeTamanho.TAMANHO_1 == null || gradeTamanho.TAMANHO_1 == "") ? "-" : gradeTamanho.TAMANHO_1));
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + ((gradeTamanho.TAMANHO_2 == null || gradeTamanho.TAMANHO_2 == "") ? "-" : gradeTamanho.TAMANHO_2));
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + ((gradeTamanho.TAMANHO_3 == null || gradeTamanho.TAMANHO_3 == "") ? "-" : gradeTamanho.TAMANHO_3));
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + ((gradeTamanho.TAMANHO_4 == null || gradeTamanho.TAMANHO_4 == "") ? "-" : gradeTamanho.TAMANHO_4));
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + ((gradeTamanho.TAMANHO_5 == null || gradeTamanho.TAMANHO_5 == "") ? "-" : gradeTamanho.TAMANHO_5));
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + ((gradeTamanho.TAMANHO_6 == null || gradeTamanho.TAMANHO_6 == "") ? "-" : gradeTamanho.TAMANHO_6));
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + ((gradeTamanho.TAMANHO_7 == null || gradeTamanho.TAMANHO_7 == "") ? "-" : gradeTamanho.TAMANHO_7));
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + ((gradeTamanho.TAMANHO_8 == null || gradeTamanho.TAMANHO_8 == "") ? "-" : gradeTamanho.TAMANHO_8));
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; text-align: right; font-weight:bold;' valign='top'>");
            _texto.Append("                            Total &nbsp;");
            _texto.Append("                        </td>");
            _texto.Append("                    </tr>");
            _texto.Append("");

            foreach (var m in prepedidoIntra)
            {
                if (m.DATA_BAIXA_TAG == null || m.DATA_BAIXA_AVIAMENTO == null || m.OBS != "")
                {
                    var itens = "-";
                    if (m.DATA_BAIXA_TAG == null)
                        itens = "TAG";
                    if (m.DATA_BAIXA_AVIAMENTO == null)
                    {
                        if (itens != "-")
                            itens = itens + " e AVIAMENTO";
                        else
                            itens = "AVIAMENTO";
                    }

                    _texto.Append("                    <tr style='border: 1px solid #000;'>");
                    _texto.Append("                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    _texto.Append("                            &nbsp; " + m.PRODUTO.Trim() + " - " + baseController.BuscaProduto(m.PRODUTO).DESC_PRODUTO.Trim());
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000;'>");
                    _texto.Append("                            &nbsp; " + m.COR_PRODUTO.Trim() + " - " + prodController.ObterCoresBasicas(m.COR_PRODUTO).DESC_COR.Trim());
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000;'>");
                    _texto.Append("                            &nbsp; " + itens);
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000;'>");
                    _texto.Append("                            &nbsp; " + m.OBS);
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
                    _texto.Append("                            " + m.CO1.ToString());
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
                    _texto.Append("                            " + m.CO2.ToString());
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
                    _texto.Append("                            " + m.CO3.ToString());
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
                    _texto.Append("                            " + m.CO4.ToString());
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
                    _texto.Append("                            " + m.CO5.ToString());
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
                    _texto.Append("                            " + m.CO6.ToString());
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
                    _texto.Append("                            " + m.CO7.ToString());
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
                    _texto.Append("                            " + m.CO8.ToString());
                    _texto.Append("                        </td>");
                    _texto.Append("                        <td style='border-right: 1px solid #000; text-align:right;'>");
                    _texto.Append("                            " + m.QTDE_ORIGINAL.ToString() + "&nbsp;&nbsp;");
                    _texto.Append("                        </td>");
                    _texto.Append("                    </tr>");

                    qtdeTotal = qtdeTotal + m.QTDE_ORIGINAL;
                    co1 = co1 + m.CO1;
                    co2 = co2 + m.CO2;
                    co3 = co3 + m.CO3;
                    co4 = co4 + m.CO4;
                    co5 = co5 + m.CO5;
                    co6 = co6 + m.CO6;
                    co7 = co7 + m.CO7;
                    co8 = co8 + m.CO8;
                }
            }

            _texto.Append("");
            _texto.Append("                    <tr style='border: 1px solid #000;'>");
            _texto.Append("                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;  font-weight:bold;'>");
            _texto.Append("                            &nbsp; Total");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000;'>");
            _texto.Append("                            &nbsp;");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000;'>");
            _texto.Append("                            &nbsp;");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000;'>");
            _texto.Append("                            &nbsp;");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + co1.ToString());
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + co2.ToString());
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + co3.ToString());
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + co4.ToString());
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + co5.ToString());
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + co6.ToString());
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + co7.ToString());
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold; text-align: center;'>");
            _texto.Append("                            " + co8.ToString());
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; text-align:right;'>");
            _texto.Append("                            " + qtdeTotal.ToString() + "&nbsp;&nbsp;");
            _texto.Append("                        </td>");
            _texto.Append("                    </tr>");
            _texto.Append("                </table>");
            _texto.Append("            </td>");
            _texto.Append("        </tr>");
            _texto.Append("    </table>");
            _texto.Append("</div>");
            _texto.Append("<br />");

            _texto.Append("");
            return _texto;
        }

    }
}