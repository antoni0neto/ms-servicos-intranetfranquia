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
    public partial class desenv_pedido_aviamento_gerarprepedido : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        decimal qtdeMaterial = 0;
        decimal valTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataEntrega.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {

                CarregarFilial();
                CarregarFabricante();

                var codigoCarrinhoCab = "";
                if (Request.QueryString["ccc"] != null)
                    codigoCarrinhoCab = Request.QueryString["ccc"].ToString();

                hidCodigoCarrinhoCab.Value = codigoCarrinhoCab;

                var carrinhoCab = desenvController.ObterCarrinhoMaterialCab(Convert.ToInt32(codigoCarrinhoCab));
                if (carrinhoCab.DATA_PEDIDO != null)
                {
                    Response.Write("Este pedido já foi gerado. Por favor, crie um novo pedido.");
                    Response.End();
                    return;
                }

                CarregarCarrinho(codigoCarrinhoCab);

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFabricante()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor().ToList();
            _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });
            _fornecedores.Where(p => ((p.STATUS == 'A' && p.TIPO == 'A') || p.STATUS == 'S')).GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (_fornecedores != null)
            {
                ddlFabricante.DataSource = _fornecedores;
                ddlFabricante.DataBind();
            }
        }
        protected void ddlFabricante_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFabricante.SelectedValue != "Selecione")
            {
                var forn = prodController.ObterFornecedor(ddlFabricante.SelectedValue, 'A');
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
                filial.Insert(1, new FILIAI { COD_FILIAL = "1055  ", FILIAL = "C-MAX (NOVA)" });
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
            var carrinho = desenvController.ObterMaterialCarrinhoPrePedido(Convert.ToInt32(hidCodigoCarrinhoCab.Value)).Where(p => p.QTDE <= 0 || p.CUSTO <= 0).Where(p => p.PEDIDO_ORIGEM == "C");
            if (carrinho != null && carrinho.Count() > 0)
                return false;
            return true;
        }
        #endregion

        private void CarregarCarrinho(string codigoCarrinhoCab)
        {
            var carrinho = desenvController.ObterMaterialCarrinhoPrePedido(Convert.ToInt32(codigoCarrinhoCab));
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_CARRINHO_PREPEDIDOResult carrinho = e.Row.DataItem as SP_OBTER_MATERIAL_CARRINHO_PREPEDIDOResult;

                    if (carrinho != null)
                    {
                        Literal litOrigem = e.Row.FindControl("litOrigem") as Literal;
                        litOrigem.Text = carrinho.DESC_PEDIDO_ORIGEM;

                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = carrinho.PRODUTO;

                        Literal litHB = e.Row.FindControl("litHB") as Literal;
                        litHB.Text = (carrinho.HB == null || carrinho.HB == 0) ? "" : carrinho.HB.ToString();

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = carrinho.DESC_PRODUTO;

                        Literal litCor = e.Row.FindControl("litCor") as Literal;
                        litCor.Text = carrinho.COR_FORNECEDOR_PRODUTO;

                        Literal litDetalhe = e.Row.FindControl("litDetalhe") as Literal;
                        litDetalhe.Text = carrinho.DETALHE;

                        Literal litSubGrupo = e.Row.FindControl("litSubGrupo") as Literal;
                        litSubGrupo.Text = carrinho.SUBGRUPO;

                        Literal litCorMaterial = e.Row.FindControl("litCorMaterial") as Literal;
                        litCorMaterial.Text = carrinho.COR_FORNECEDOR_MATERIAL;

                        Literal litUnidadeMedida = e.Row.FindControl("litUnidadeMedida") as Literal;
                        litUnidadeMedida.Text = desenvController.ObterMaterial(carrinho.MATERIAL).UNID_FICHA_TEC;

                        Literal litQtde = e.Row.FindControl("litQtde") as Literal;
                        litQtde.Text = carrinho.QTDE.ToString();

                        Literal litCusto = e.Row.FindControl("litCusto") as Literal;
                        litCusto.Text = carrinho.CUSTO.ToString();

                        Literal litDesconto = e.Row.FindControl("litDesconto") as Literal;
                        litDesconto.Text = carrinho.DESCONTO_ITEM.ToString();

                        Literal litTotal = e.Row.FindControl("litTotal") as Literal;
                        litTotal.Text = Convert.ToDecimal(carrinho.VALOR_TOTAL).ToString("###,###,###,##0.00000");

                        if ((carrinho.QTDE > 0 && carrinho.CUSTO > 0) || carrinho.PEDIDO_ORIGEM == "E")
                            e.Row.BackColor = Color.PaleGreen;

                        qtdeMaterial += carrinho.QTDE;
                        valTotal += Convert.ToDecimal(carrinho.VALOR_TOTAL);

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
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[10].Text = qtdeMaterial.ToString();
                footer.Cells[13].Text = valTotal.ToString("###,###,###,##0.00000");
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
                    labErro.Text = "Por favor, preencher a Grade e Custo de todos os produtos.";
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

                var pedidoCompra = desenvController.GerarINTRAPrePedidoCompraMaterial(Convert.ToInt32(hidCodigoCarrinhoCab.Value), fornecedor, limiteEntrega, filial, codigoFilialRateio, status, aprovadoPor, usuario.CODIGO_USUARIO);

                if (pedidoCompra != null && pedidoCompra.NUMERO_PEDIDO != "")
                {
                    btSalvar.Enabled = false;

                    if (cbEnviarEmail.Checked)
                        EnviarEmail(pedidoCompra.NUMERO_PEDIDO);

                    labErro.Text = "Pré-Pedido de Compra " + ddlFilial.SelectedItem.Text.Trim() + " gerado com sucesso na INTRANET: " + pedidoCompra.NUMERO_PEDIDO;
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

        private void EnviarEmail(string pedidoCompra)
        {

            try
            {
                var relatorioPedidoCompra = MontarRelatorioHTML(pedidoCompra);

                USUARIO usuario = (USUARIO)Session["USUARIO"];

                email_envio email = new email_envio();
                string assunto = "";
                assunto = "Handbook: Pedido de Compra " + pedidoCompra + " - " + DateTime.Today.ToString("dd/MM/yyyy");
                email.ASSUNTO = assunto;
                email.REMETENTE = usuario;
                email.MENSAGEM = relatorioPedidoCompra.ToString();

                List<string> destinatario = new List<string>();
                //Adiciona e-mails 
                var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(22, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
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
        private StringBuilder MontarRelatorioHTML(string pedidoCompra)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto, "Pedido de Compra Material");
            _texto = MontarPedidoCompraMaterial(_texto, pedidoCompra);
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
        public StringBuilder MontarPedidoCompraMaterial(StringBuilder _texto, string pedidoCompra)
        {
            var dataPedido = DateTime.Today;
            decimal valorTotalPedido = 0;

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
            _texto.Append("                            Fornecedor");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            Pré-pedido Intranet");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            Data do Pedido");
            _texto.Append("                        </td>");
            _texto.Append("                    </tr>");
            _texto.Append("                    <tr style='border: 1px solid #000;'>");
            _texto.Append("                        <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                            " + ddlFabricante.SelectedItem.Text.Trim());
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                            " + pedidoCompra);
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                            " + dataPedido);
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
            _texto.Append("                            Produto-HB");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            Cor");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            Material");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;'>");
            _texto.Append("                            Grupo");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;' valign='top'>");
            _texto.Append("                            SubGrupo");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;' valign='top'>");
            _texto.Append("                            Cor Fornecedor");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;' valign='top'>");
            _texto.Append("                            Qtde");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;' valign='top'>");
            _texto.Append("                            Preço Unitário");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; font-weight:bold;' valign='top'>");
            _texto.Append("                            Desconto Unitário");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; text-align: right; font-weight:bold;' valign='top'>");
            _texto.Append("                            Total");
            _texto.Append("                        </td>");
            _texto.Append("                    </tr>");
            _texto.Append("");

            var materialPrePedido = desenvController.ObterMaterialCarrinhoPrePedidoEmail(Convert.ToInt32(hidCodigoCarrinhoCab.Value));
            foreach (var m in materialPrePedido)
            {


                _texto.Append("                    <tr style='border: 1px solid #000;'>");
                _texto.Append("                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
                _texto.Append("                            " + m.PRODUTO);
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                            " + m.COR);
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                            " + m.MATERIAL);
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                            " + m.GRUPO);
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
                _texto.Append("                            " + m.SUBGRUPO);
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
                _texto.Append("                            " + m.COR_FORNECEDOR);
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
                _texto.Append("                            " + m.QTDE.ToString());
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
                _texto.Append("                            R$" + m.CUSTO.ToString());
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
                _texto.Append("                            R$" + m.DESCONTO_ITEM.ToString());
                _texto.Append("                        </td>");
                _texto.Append("                        <td style='border-right: 1px solid #000; text-align:right;'>");
                _texto.Append("                            R$" + Convert.ToDecimal(m.VAL_TOTAL).ToString("###,###,###,##0.00000"));
                _texto.Append("                        </td>");
                _texto.Append("                    </tr>");

                valorTotalPedido = valorTotalPedido + Convert.ToDecimal(m.VAL_TOTAL);
            }
            _texto.Append("");
            _texto.Append("                    <tr style='border: 1px solid #000;'>");
            _texto.Append("                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;  font-weight:bold;'>");
            _texto.Append("                            Total");
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
            _texto.Append("                            &nbsp;");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
            _texto.Append("                            &nbsp;");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
            _texto.Append("                            &nbsp;");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000;' valign='top'>");
            _texto.Append("                            &nbsp;");
            _texto.Append("                        </td>");
            _texto.Append("                        <td style='border-right: 1px solid #000; text-align:right;'>");
            _texto.Append("                            R$ " + valorTotalPedido.ToString("###,###,###,##0.00000"));
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
