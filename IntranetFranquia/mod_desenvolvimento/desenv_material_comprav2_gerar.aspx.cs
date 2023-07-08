using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_material_comprav2_gerar : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        decimal qtdeTotal = 0;
        decimal valTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataEntrega.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarFornecedores();
                CarregarCarrinho();

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecao.DataSource = colecoes;
                ddlColecao.DataBind();
            }
        }
        private void CarregarFornecedores()
        {
            var fornecedores = prodController.ObterFornecedor().Where(p => p.TIPO == 'T' && p.STATUS == 'A').ToList();
            if (fornecedores != null)
            {
                var fornecedoresAux = fornecedores.Where(p => p.FORNECEDOR != null).Select(s => s.FORNECEDOR.Trim()).Distinct().ToList();

                fornecedores = new List<PROD_FORNECEDOR>();
                foreach (var item in fornecedoresAux)
                    if (item.Trim() != "")
                        fornecedores.Add(new PROD_FORNECEDOR { FORNECEDOR = item.Trim() });

                fornecedores.OrderBy(p => p.FORNECEDOR);
                fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione" });

                ddlFornecedor.DataSource = fornecedores;
                ddlFornecedor.DataBind();
            }
        }
        protected void ddlFornecedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFornecedor.SelectedValue != "Selecione")
            {
                var forn = prodController.ObterFornecedor(ddlFornecedor.SelectedValue, 'T');
                if (forn != null)
                {
                    txtEmail.Text = forn.EMAIL;
                }
            }
            else
            {
                txtEmail.Text = "";
            }
        }
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFornecedor.ForeColor = _OK;
            if (ddlFornecedor.SelectedValue.Trim() == "Selecione")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labColecao.ForeColor = _OK;
            if (ddlColecao.SelectedValue == "")
            {
                labColecao.ForeColor = _notOK;
                retorno = false;
            }

            labDataEntrega.ForeColor = _OK;
            if (txtDataEntrega.Text == "")
            {
                labDataEntrega.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }


        #endregion

        private void CarregarCarrinho()
        {
            var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var carrinho = desenvController.ObterCarrinhoPedidoPorUsuario(codigoUsuario);
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO_CARRINHO carrinho = e.Row.DataItem as DESENV_PEDIDO_CARRINHO;

                    if (carrinho != null)
                    {

                        Literal litPedido = e.Row.FindControl("litPedido") as Literal;
                        litPedido.Text = carrinho.DESENV_PEDIDO1.NUMERO_PEDIDO.ToString();

                        Literal litMaterial = e.Row.FindControl("litMaterial") as Literal;
                        var materiais = desenvController.ObterMaterial(carrinho.DESENV_PEDIDO1.GRUPO.Trim(), carrinho.DESENV_PEDIDO1.SUBGRUPO.Trim());
                        if (materiais != null)
                        {
                            var materialCor = desenvController.ObterMaterialCor().Where(p => p.COR_MATERIAL.Trim() == carrinho.DESENV_PEDIDO1.COR.Trim() && p.REFER_FABRICANTE.Trim() == carrinho.DESENV_PEDIDO1.COR_FORNECEDOR.Trim() && materiais.Any(x => x.MATERIAL == p.MATERIAL)).FirstOrDefault();
                            if (materialCor != null)
                                litMaterial.Text = materialCor.MATERIAL.Trim();
                        }

                        Literal litTecido = e.Row.FindControl("litTecido") as Literal;
                        litTecido.Text = carrinho.DESENV_PEDIDO1.SUBGRUPO.Trim();

                        Literal litCorFornecedor = e.Row.FindControl("litCorFornecedor") as Literal;
                        litCorFornecedor.Text = carrinho.DESENV_PEDIDO1.COR_FORNECEDOR;

                        Literal litUnidadeMedida = e.Row.FindControl("litUnidadeMedida") as Literal;
                        litUnidadeMedida.Text = carrinho.DESENV_PEDIDO1.UNIDADE_MEDIDA1.DESCRICAO;

                        var qtde = (carrinho.QTDE != null) ? carrinho.QTDE : 0.00M;
                        Literal litQtde = e.Row.FindControl("litQtde") as Literal;
                        litQtde.Text = qtde.ToString();

                        var preco = (carrinho.PRECO != null) ? carrinho.PRECO : 0.00M;
                        Literal litPreco = e.Row.FindControl("litPreco") as Literal;
                        litPreco.Text = preco.ToString();

                        Literal litValTotal = e.Row.FindControl("litValTotal") as Literal;
                        litValTotal.Text = "R$ " + Convert.ToDecimal(qtde * preco).ToString("###,###,###,##0.00");

                        ImageButton btExcluirCarrinho = e.Row.FindControl("btExcluirItemCarrinho") as ImageButton;
                        if (btExcluirCarrinho != null)
                            btExcluirCarrinho.CommandArgument = carrinho.CODIGO.ToString();

                        if (carrinho.QTDE > 0 && carrinho.PRECO > 0)
                            e.Row.BackColor = Color.PaleGreen;

                        qtdeTotal += Convert.ToDecimal(qtde);
                        valTotal += Convert.ToDecimal(qtde * preco);

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvCarrinho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCarrinho.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[5].Text = qtdeTotal.ToString();
                footer.Cells[8].Text = "R$ " + valTotal.ToString("###,###,##0.00");
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Por favor, preencher os campos em vermelho.";
                    return;
                }

                if (cbEnviarEmail.Checked && txtEmail.Text.Trim() == "")
                {
                    labErro.Text = "Por favor, preencher o campo Email.";
                    return;
                }

                if (txtEmail.Text.Trim() != "" && !Utils.WebControls.ValidarEmail(txtEmail.Text.Trim()))
                {
                    labErro.Text = "Por favor, preencher um email válido.";
                    return;
                }

                var usuario = (USUARIO)Session["USUARIO"];
                var fornecedor = ddlFornecedor.SelectedValue.Trim();


                var pedidoIntra = desenvController.GerarPedidoCompraTecidoIntra(fornecedor, ddlColecao.SelectedValue, Convert.ToDateTime(txtDataEntrega.Text), usuario.CODIGO_USUARIO);

                if (pedidoIntra != null && pedidoIntra.OK > 0)
                {
                    btSalvar.Enabled = false;

                    if (cbEnviarEmail.Checked)
                        EnviarEmail(usuario, fornecedor);

                    labErro.Text = "Os pedidos foram gerados com sucesso!";
                }
                else
                {
                    labErro.Text = "Erro ao gerar pedidos. Verifique os dados informados...";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void EnviarEmail(USUARIO usuario, string fornecedor)
        {

            try
            {
                var relatorioPedidoCompra = MontarRelatorioHTML(usuario);

                email_envio email = new email_envio();
                string assunto = "";
                assunto = "Handbook: Pedido de Compra Tecido " + fornecedor.Trim() + " - " + DateTime.Today.ToString("dd/MM/yyyy");
                email.ASSUNTO = assunto;
                email.REMETENTE = usuario;
                email.MENSAGEM = relatorioPedidoCompra.ToString();

                List<string> destinatario = new List<string>();
                //Adiciona e-mails 
                var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(22, 2).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                foreach (var usu in usuarioEmail)
                    if (usu != null)
                        destinatario.Add(usu.EMAIL);
                //Adicionar remetente
                destinatario.Add(usuario.EMAIL);
                destinatario.Add(txtEmail.Text.Trim().ToLower());

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
        private StringBuilder MontarRelatorioHTML(USUARIO usuario)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto, "Pedido de Compra de Tecido");
            _texto = MontarPedidoCompraTecido(_texto, usuario);
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
        public StringBuilder MontarPedidoCompraTecido(StringBuilder _texto, USUARIO usuario)
        {
            var dataPedido = DateTime.Today;
            decimal qtdeTotal = 0;
            decimal valorTotal = 0;

            _texto.Append("");
            _texto.Append("<br />");
            _texto.Append("<p>");
            _texto.Append(txtMensagem.Text.Trim());
            _texto.Append("</p>");
            _texto.Append("<br />");
            _texto.Append("<div id='divPrepEdido' align='left'>");
            _texto.Append("    <table border='0' cellpadding='0' cellspacing='0' style='width: 1000pt; padding: 0px;");
            _texto.Append("                    color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("                    background: white; white-space: nowrap;'>");
            _texto.Append("        <tr>");
            _texto.Append("            <td style='line-height: 20px;'>");
            _texto.Append("                <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.Append("                                width: 1000pt'>");
            _texto.Append("                    <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
            _texto.Append("                        <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp;Fornecedor");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp;Coleção");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp;Data do Pedido");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp;Previsão de Entrega");
            _texto.Append("                        </td>");
            _texto.Append("                    </tr>");
            _texto.Append("                    <tr style='border: 1px solid #000;'>");
            _texto.Append("                        <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                            &nbsp; " + ddlFornecedor.SelectedItem.Text.Trim());
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                            &nbsp; " + ddlColecao.SelectedItem.Text.Trim());
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                            &nbsp; " + dataPedido.ToString("dd/MM/yyyy"));
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                            &nbsp; " + Convert.ToDateTime(txtDataEntrega.Text).ToString("dd/MM/yyyy"));
            _texto.Append("                        </td>");
            _texto.Append("                    </tr>");
            _texto.Append("                </table>");
            _texto.Append("            </td>");
            _texto.Append("        </tr>");
            _texto.Append("    </table>");
            _texto.Append("</div>");
            _texto.Append("<br />");
            _texto.Append("<div id='divPreMaterial' align='left'>");
            _texto.Append("    <table border='0' cellpadding='0' cellspacing='0' style='width: 1000pt; padding: 0px;");
            _texto.Append("                    color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("                    background: white; white-space: nowrap;'>");
            _texto.Append("        <tr>");
            _texto.Append("            <td style='line-height: 20px;'>");
            _texto.Append("                <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.Append("                                width: 1000pt'>");
            _texto.Append("                    <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
            _texto.Append("                        <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp; Pedido");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp; Tecido");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp; Cor");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp; Preço");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            &nbsp; Med.");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;' valign='top'>");
            _texto.Append("                            &nbsp; Qtde");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; text-align: right; font-weight:bold;' valign='top'>");
            _texto.Append("                            Total &nbsp;");
            _texto.Append("                        </td>");
            _texto.Append("                    </tr>");
            _texto.Append("");

            var pedidos = desenvController.ObterCarrinhoPedidoPorUsuario(usuario.CODIGO_USUARIO).OrderBy(p => p.DESENV_PEDIDO1.SUBGRUPO).ThenBy(p => p.DESENV_PEDIDO1.COR_FORNECEDOR);
            foreach (var p in pedidos)
            {
                var total = Convert.ToDecimal(p.QTDE * p.PRECO);

                _texto.Append("                    <tr style='border: 1px solid #000;'>");
                _texto.Append("                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
                _texto.Append("                            &nbsp;" + p.DESENV_PEDIDO1.NUMERO_PEDIDO.ToString());
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                            &nbsp;" + p.DESENV_PEDIDO1.SUBGRUPO.Trim());
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                            &nbsp;" + p.DESENV_PEDIDO1.COR_FORNECEDOR.Trim());
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
                _texto.Append("                            &nbsp;R$ " + ((p.PRECO == null) ? "-" : Convert.ToDecimal(p.PRECO).ToString("###,###,###,##0.00")));
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
                _texto.Append("                            &nbsp;" + p.DESENV_PEDIDO1.UNIDADE_MEDIDA1.DESCRICAO);
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
                _texto.Append("                            &nbsp;" + p.QTDE.ToString());
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000; text-align:right;'>");
                _texto.Append("                            R$ " + total.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                        </td>");
                _texto.Append("                    </tr>");

                qtdeTotal = qtdeTotal + Convert.ToDecimal(p.QTDE);
                valorTotal = valorTotal + total;
            }
            _texto.Append("");
            _texto.Append("                    <tr style='border: 1px solid #000;'>");
            _texto.Append("                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;  font-weight:bold;'>");
            _texto.Append("                            Total &nbsp;");
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
            _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
            _texto.Append("                            &nbsp;");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
            _texto.Append("                            &nbsp;" + qtdeTotal.ToString("###,###,###,##0.000"));
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; text-align:right;'>");
            _texto.Append("                            R$ " + valorTotal.ToString("###,###,###,##0.00") + "&nbsp;");
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
