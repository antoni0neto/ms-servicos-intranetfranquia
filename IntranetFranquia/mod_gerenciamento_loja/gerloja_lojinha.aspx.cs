using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Net;
using System.Drawing;
using System.Text;

namespace Relatorios
{
    public partial class gerloja_lojinha : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        decimal gPrecoOriginal = 0;
        decimal gPrecoDesconto = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                txtCodigoProduto.Attributes.Add("onKeyPress", "doClick('" + btGravar.ClientID + "',event)");
                txtCodigoProduto.Focus();

                labErro.Text = "";
                pnlProduto.Visible = false;

                btNovo_Click(null, null);
            }

            btGravar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGravar, null) + ";");
        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                string produto = "";
                string produtoBarra = "";
                string valorProduto = txtCodigoProduto.Text.Trim();

                if (valorProduto == "")
                {
                    labErro.Text = "Informe um PRODUTO.";
                    return;
                }

                if (valorProduto.Length == 5)
                    produto = valorProduto;
                else
                    produtoBarra = valorProduto;

                //BUSCAR PRODUTO
                var produtoVendaLojinha = lojaController.ObterProdutoVendaLojinha(produto, produtoBarra);

                if (produtoVendaLojinha == null)
                {
                    labErro.Text = "Produto não encontrado. Digite o CÓDIGO DO PRODUTO manualmente. Caso, o problema persista, entre em contato com TI.";
                    return;
                }

                if (txtCPF.Text.Trim() != "")
                {
                    //VALIDAR 30% DO SALARIO COM TOTAL DA COMPRA
                    if (!ValidarSalario30PORC(produtoVendaLojinha.PRECO_PAGO))
                    {
                        labErro.Text = "Esta venda irá ultrapassar os 30% de limite do Sálário deste Funcionário...";
                        return;
                    }
                }

                if (hidSEQ.Value != "")
                {
                    var p = new VENDA_LOJINHA();
                    p.SEQ = Convert.ToInt32(hidSEQ.Value);
                    p.CPF = txtCPF.Text;
                    p.PRODUTO = produtoVendaLojinha.PRODUTO.Trim();
                    p.DESC_PRODUTO = produtoVendaLojinha.DESC_PRODUTO.Trim();
                    p.GRUPO_PRODUTO = produtoVendaLojinha.GRUPO_PRODUTO.Trim();
                    p.COR = produtoVendaLojinha.DESC_COR;
                    p.TAMANHO = produtoVendaLojinha.GRADE;
                    p.PRECO_TL = produtoVendaLojinha.PRECO_TL;
                    p.PRECO_PAGO = produtoVendaLojinha.PRECO_PAGO;

                    lojaController.InserirVendaLojinha(p);

                    CarregarProduto(hidSEQ.Value);

                    txtCodigoProduto.Text = "";
                    txtCodigoProduto.Focus();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "Erro ao bipar: " + ex.Message;
            }
        }
        private bool ValidarSalario30PORC(decimal precoUltimaPeca)
        {
            if (cbSalario30.Checked)
                return true;

            //se for CNPJ, nao calcula
            if (txtCPF.Text.Trim().Length > 11)
                return true;

            decimal precoFinal = Convert.ToDecimal(hidValorVenda.Value) + precoUltimaPeca;
            decimal salario = Convert.ToDecimal(hidSalarioFuncionario.Value);

            if (precoFinal > salario)
                return false;

            return true;
        }

        private void CarregarProduto(string seq)
        {
            var vendaLojinha = lojaController.ObterVendaLojinha(seq);
            gvProduto.DataSource = vendaLojinha;
            gvProduto.DataBind();

            labDesconto.Text = CalcularPrecoFinal(vendaLojinha);
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    VENDA_LOJINHA vendaLojinha = e.Row.DataItem as VENDA_LOJINHA;

                    if (vendaLojinha != null)
                    {


                        Label _labPrecoTL = e.Row.FindControl("labPrecoTL") as Label;
                        if (_labPrecoTL != null)
                        {
                            _labPrecoTL.Text = "R$ " + Convert.ToDecimal(vendaLojinha.PRECO_TL).ToString("###,###,###,#00.00");
                            gPrecoOriginal += Convert.ToDecimal(vendaLojinha.PRECO_TL);
                        }

                        Label _labDesconto = e.Row.FindControl("labDesconto") as Label;
                        if (_labDesconto != null)
                        {
                            _labDesconto.Text = "R$ " + Convert.ToDecimal(vendaLojinha.PRECO_PAGO).ToString("###,###,###,#00.00");
                            gPrecoDesconto += Convert.ToDecimal(vendaLojinha.PRECO_PAGO);
                        }

                        ImageButton _btExcluir = e.Row.FindControl("btExcluir") as ImageButton;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = vendaLojinha.CODIGO.ToString();
                    }
                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow footerProduto = gvProduto.FooterRow;
            if (footerProduto != null)
            {
                footerProduto.Cells[4].Text = "R$ " + gPrecoOriginal.ToString("###,###,###,#00.00");
                footerProduto.Cells[5].Text = "R$ " + gPrecoDesconto.ToString("###,###,###,#00.00");
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton btExcluir = (ImageButton)sender;
                if (btExcluir != null)
                {
                    if (btExcluir.CommandArgument != "")
                    {
                        int codigo = Convert.ToInt32(btExcluir.CommandArgument);
                        lojaController.ExcluirVendaLojinhaPorCodigo(codigo);
                        if (hidSEQ.Value != "")
                            CarregarProduto(hidSEQ.Value);
                    }
                }

                txtCodigoProduto.Focus();
            }
            catch (Exception ex)
            {
                labErro.Text = "Erro ao excluir:" + ex.Message;
            }

        }

        protected void btLimpar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labDesconto.Text = "00,00";
                txtCodigoProduto.Text = "";
                btAbrirVenda.Visible = false;
                cbSalario30.Checked = false;

                if (hidSEQ.Value != "")
                {
                    gvProduto.DataSource = new List<VENDA_LOJINHA>();
                    gvProduto.DataBind();

                    pnlProduto.Visible = false;

                    hidSEQ.Value = "";
                    hidValorVenda.Value = "0";
                    hidSalarioFuncionario.Value = "0";
                    txtCPF.Enabled = true;
                    txtCPF.Text = "";
                    btBuscarCPF.Enabled = true;

                    txtNomeFuncionario.Text = "";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btNovo_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (hidSEQ.Value != "")
                    btLimpar_Click(null, null);

                var seqVendaLojinha = lojaController.ObterSequenciaVendaLojinha();
                if (seqVendaLojinha != null)
                    hidSEQ.Value = seqVendaLojinha.seq.ToString();

                pnlProduto.Visible = true;
                txtCodigoProduto.Enabled = false;
                btGravar.Enabled = false;
                cbSalario30.Checked = false;
                hidValorVenda.Value = "0";
                hidSalarioFuncionario.Value = "0";

                txtCodigoProduto.Focus();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private string CalcularPrecoFinal(List<VENDA_LOJINHA> lstVendaLojinha)
        {
            decimal valorDesconto = 0;

            foreach (var p in lstVendaLojinha)
                valorDesconto += Convert.ToDecimal(p.PRECO_PAGO);

            hidValorVenda.Value = valorDesconto.ToString();

            return valorDesconto.ToString("###,###,#00.00");
        }

        protected void btBuscarCPF_Click(object sender, EventArgs e)
        {
            try
            {
                labErroCPF.Text = "";
                btAbrirVenda.Visible = false;

                string cpf = txtCPF.Text.Trim();

                if (cpf == "")
                {
                    labErroCPF.Text = "Informe o CPF...";
                    return;
                }

                if (cpf.Length == 11)
                {
                    if (!Utils.WebControls.ValidarCPF(cpf))
                    {
                        labErroCPF.Text = "O CPF informado é inválido...";
                        return;
                    }
                }

                if (lojaController.ClienteJaComprou(cpf))
                {
                    labErroCPF.Text = "O CPF/CNPJ informado já realizou uma compra...";
                    btAbrirVenda.Visible = true;
                    return;
                }

                //Buscar Salário do Funcionário
                if (cpf.Length == 11)
                {
                    var salFun = lojaController.ObterSalarioFuncionario(cpf);
                    if (salFun != null)
                    {
                        hidSalarioFuncionario.Value = salFun.SAL_MINIMO.ToString();
                        txtNomeFuncionario.Text = salFun.NOME;
                    }
                    else
                    {
                        hidSalarioFuncionario.Value = "N";
                        txtNomeFuncionario.Text = "FUNCIONÁRIO NÃO ENCONTRADO.";
                        return;
                    }
                }
                else
                {
                    var c = baseController.ObterCadastroCLIFORPorCNPJ(cpf);
                    if (c != null)
                    {
                        hidSalarioFuncionario.Value = "0";
                        txtNomeFuncionario.Text = c.NOME_CLIFOR;
                    }
                    else
                    {
                        hidSalarioFuncionario.Value = "0";
                        txtNomeFuncionario.Text = "FORNECEDOR NÃO ENCONTRADO.";
                    }
                }

                txtCPF.Enabled = false;
                btBuscarCPF.Enabled = false;

                txtCodigoProduto.Enabled = true;
                btGravar.Enabled = true;

                txtCodigoProduto.Focus();

            }
            catch (Exception ex)
            {
                labErroCPF.Text = ex.Message;
            }
        }
        protected void btAbrirVenda_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroCPF.Text = "";

                string cpf = txtCPF.Text.Trim();

                if (cpf == "")
                {
                    labErroCPF.Text = "Informe o CPF...";
                    return;
                }

                if (cpf.Length == 11)
                {
                    if (!Utils.WebControls.ValidarCPF(cpf))
                    {
                        labErroCPF.Text = "O CPF informado é inválido...";
                        return;
                    }
                }

                var vendaCliente = lojaController.ObterVendaLojinhaPorCPF(cpf);
                if (vendaCliente != null && vendaCliente.Count() > 0)
                    hidSEQ.Value = vendaCliente[0].SEQ.ToString();

                CarregarProduto(hidSEQ.Value);

                //Buscar Salário do Funcionário
                if (cpf.Length == 11)
                {
                    var salFun = lojaController.ObterSalarioFuncionario(cpf);
                    if (salFun != null)
                    {
                        hidSalarioFuncionario.Value = salFun.SAL_MINIMO.ToString();
                        txtNomeFuncionario.Text = salFun.NOME;
                    }
                    else
                    {
                        hidSalarioFuncionario.Value = "N";
                        txtNomeFuncionario.Text = "FUNCIONÁRIO NÃO ENCONTRADO.";
                        return;
                    }
                }
                else
                {
                    var c = baseController.ObterCadastroCLIFORPorCNPJ(cpf);
                    if (c != null)
                    {
                        hidSalarioFuncionario.Value = "0";
                        txtNomeFuncionario.Text = c.NOME_CLIFOR;
                    }
                    else
                    {
                        hidSalarioFuncionario.Value = "0";
                        txtNomeFuncionario.Text = "FORNECEDOR NÃO ENCONTRADO.";
                    }
                }

                txtCPF.Enabled = false;
                btBuscarCPF.Enabled = false;

                txtCodigoProduto.Enabled = true;
                btGravar.Enabled = true;

                txtCodigoProduto.Focus();
            }
            catch (Exception ex)
            {
                labErroCPF.Text = ex.Message;
            }
        }

        protected void btFinalizarCompra_Click(object sender, EventArgs e)
        {
            try
            {
                labErroCPF.Text = "";

                string cpf = "";

                cpf = txtCPF.Text.Trim();
                if (cpf == "")
                {
                    labErroCPF.Text = "Informe o CPF...";
                    return;
                }

                if (gvProduto.Rows.Count <= 0)
                {
                    labErroCPF.Text = "Informe os Produtos...";
                    return;
                }

                lojaController.AtualizarVendaLojinhaPorCPF(cpf);

                //IMPRIMIR RECIBO
                var vendaLojinha = lojaController.ObterVendaLojinhaPorCPF(cpf);
                GerarRecibo(vendaLojinha);

                btNovo_Click(null, null);
            }
            catch (Exception ex)
            {
                labErroCPF.Text = ex.Message;
            }
        }

        private void GerarRecibo(List<VENDA_LOJINHA> vendaLojinha)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "RECIBO_LOJINHA_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(vendaLojinha));
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
        private StringBuilder MontarRelatorioHTML(List<VENDA_LOJINHA> vendaLojinha)
        {
            var texto = new StringBuilder();

            texto = MontarRecibo(texto, vendaLojinha);

            return texto;
        }
        private StringBuilder MontarRecibo(StringBuilder texto, List<VENDA_LOJINHA> vendaLojinha)
        {
            string cpf = "";
            string nome = "";

            cpf = vendaLojinha[0].CPF;
            nome = txtNomeFuncionario.Text.Trim();

            texto.AppendLine("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            texto.AppendLine("<html>");
            texto.AppendLine("<head>");
            texto.AppendLine("    <title>Recibo Lojinha</title>");
            texto.AppendLine("    <meta charset='UTF-8' />");
            texto.AppendLine("    <style type='text/css'>");
            texto.AppendLine("        @media print {");
            texto.AppendLine("            .background-force {");
            texto.AppendLine("                -webkit-print-color-adjust: exact;");
            texto.AppendLine("            }");
            texto.AppendLine("        }");
            texto.AppendLine("    </style>");
            texto.AppendLine("</head>");
            texto.AppendLine("<body onload='window.print();'>");
            texto.AppendLine("    <div id='dviReciboLojinha' align='center'>");
            texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt;");
            texto.AppendLine("            padding: 0px; color: black; font-size: 8.0pt; font-weight: 600; font-family: Arial;");
            texto.AppendLine("            background: white; white-space: nowrap;'>");
            texto.AppendLine("            <tr>");
            texto.AppendLine("                <td style='line-height: 23px;'>");
            texto.AppendLine("                    <table border='1' cellpadding='1' cellspacing='0' style='border-collapse: collapse;");
            texto.AppendLine("                        width: 800pt'>");
            texto.AppendLine("                        <tr>");
            texto.AppendLine("                            <td colspan='5' style='text-align: center; border-right: 1px solid #000; border-left: 1px solid #000; border-bottom:none;'>");
            texto.AppendLine("                                <div style='position:absolute;'>");
            texto.AppendLine("                                    <img alt='' width='200' src='http://cmaxweb.dnsalias.com:8585/image/handbooketclogo.png' />");
            texto.AppendLine("                                </div>");
            texto.AppendLine("                                <h1>");
            texto.AppendLine("                                    COMPROVANTE DE VENDA<br />");
            texto.AppendLine("                                    HANDBOOK");
            texto.AppendLine("                                </h1>");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                        </tr>");
            texto.AppendLine("                        <tr>");
            texto.AppendLine("                            <td colspan='5' style='line-height: 40px; border-top: none; border-bottom: none; text-align:right;'>");
            texto.AppendLine("                                " + DateTime.Now.ToString("dd/MM/yyyy") + "&nbsp;&nbsp;");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                        </tr>");
            texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000; border-bottom: 1px solid #000; background-color: #FAF0E6;' class='background-force'>");
            texto.AppendLine("                            <td style='width: 200px; border-left: 1px solid #000; border-right: 1px solid #000; text-align: left; '>");
            texto.AppendLine("                                &nbsp;PRODUTO");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                            <td style='width: 250px; border-right: 1px solid #000; text-align:left;'>");
            texto.AppendLine("                                &nbsp;NOME");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                            <td style='width: 200px; border-right: 1px solid #000; text-align: left'>");
            texto.AppendLine("                                &nbsp;COR");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                            <td style='width: 150px; border-right: 1px solid #000; text-align: left'>");
            texto.AppendLine("                                &nbsp;TAMANHO");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: left'>");
            texto.AppendLine("                                &nbsp;PREÇO");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                        </tr>");

            decimal total = 0;
            foreach (var vl in vendaLojinha)
            {
                total += Convert.ToDecimal(vl.PRECO_PAGO);

                texto.AppendLine("                        <tr style='text-align: left; border-bottom: 1px solid #000;'>");
                texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
                texto.AppendLine("                                &nbsp;" + vl.PRODUTO);
                texto.AppendLine("                            </td>");
                texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align:left;'>");
                texto.AppendLine("                                &nbsp;" + vl.DESC_PRODUTO);
                texto.AppendLine("                            </td>");
                texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align:left;'>");
                texto.AppendLine("                                &nbsp;" + vl.COR);
                texto.AppendLine("                            </td>");
                texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:left;'>");
                texto.AppendLine("                                &nbsp;" + vl.TAMANHO);
                texto.AppendLine("                            </td>");
                texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align: right; '>");
                texto.AppendLine("                                R$ " + Convert.ToDecimal(vl.PRECO_PAGO).ToString("###,###,###,##0.00") + "&nbsp;");
                texto.AppendLine("                            </td>");
                texto.AppendLine("                        </tr>");
            }
            texto.AppendLine("");
            texto.AppendLine("                        <tr style='text-align: left; border-bottom: 1px solid #000; background-color: #FAF0E6;' class='background-force'>");
            texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            texto.AppendLine("                                &nbsp;TOTAL");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            texto.AppendLine("                                &nbsp;");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            texto.AppendLine("                                &nbsp;");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            texto.AppendLine("                                &nbsp;");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align: right; '>");
            texto.AppendLine("                                R$ " + total.ToString("###,###,###,##0.00") + "&nbsp;");
            texto.AppendLine("                            </td>");
            texto.AppendLine("                        </tr>");
            texto.AppendLine("                    </table>");
            texto.AppendLine("                </td>");
            texto.AppendLine("            </tr>");
            texto.AppendLine("            <tr>");
            texto.AppendLine("                <td>");
            texto.AppendLine("                    <br /><br /><br /><br />");
            texto.AppendLine("                </td>");
            texto.AppendLine("            </tr>");
            texto.AppendLine("            <tr>");
            texto.AppendLine("                <td style='line-height: 23px; text-align:center;'>");
            texto.AppendLine("                    <table border='0' cellpadding='1' cellspacing='0' style='border-collapse: collapse;");
            texto.AppendLine("                        width: 800pt'>");
            texto.AppendLine("                        <tr>");
            texto.AppendLine("                            <td>__________________________________________________________________________________________</td>");
            texto.AppendLine("                        </tr>");
            texto.AppendLine("                        <tr>");
            texto.AppendLine("                            <td>" + nome + "</td>");
            texto.AppendLine("                        </tr>");
            texto.AppendLine("                        <tr>");
            texto.AppendLine("                            <td>" + cpf + "</td>");
            texto.AppendLine("                        </tr>");
            texto.AppendLine("                    </table>");
            texto.AppendLine("                </td>");
            texto.AppendLine("            </tr>");
            texto.AppendLine("");
            texto.AppendLine("        </table>");
            texto.AppendLine("    </div>");
            texto.AppendLine("</body>");
            texto.AppendLine("</html>");

            return texto;
        }


    }
}