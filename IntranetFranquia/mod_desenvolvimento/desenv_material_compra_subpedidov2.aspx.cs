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
    public partial class desenv_material_compra_subpedidov2 : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        decimal gTotConsumo = 0;
        decimal gTotConsumoTotal = 0;
        decimal gTotTecido = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPedidoData.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPedidoPrevEntrega.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

                int numeroPedido = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                numeroPedido = Convert.ToInt32(Request.QueryString["p"].ToString());

                var codigoSubPedido = 0;
                if (Request.QueryString["sub"] != null && Request.QueryString["sub"] != "")
                {
                    codigoSubPedido = Convert.ToInt32(Request.QueryString["sub"].ToString());
                    hidCodigoSubPedido.Value = codigoSubPedido.ToString();
                }

                var pedido = desenvController.ObterPedidoNumero(numeroPedido);
                if (pedido == null)
                    Response.Redirect("desenv_menu.aspx");

                hidCodigoPedido.Value = pedido.CODIGO.ToString();
                txtPedidoNumero.Text = pedido.NUMERO_PEDIDO.ToString();

                CarregarColecoes();
                CarregarPedido(pedido, codigoSubPedido);

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupos()
        {
            var matGrupo = desenvController.ObterMaterialGrupo();
            if (matGrupo != null)
            {
                matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "" });

                ddlMaterialGrupoPedido.DataSource = matGrupo;
                ddlMaterialGrupoPedido.DataBind();
            }
        }
        private void CarregarSubGrupos(string grupo)
        {
            var matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();

            if (matSubGrupo != null)
            {
                matSubGrupo = matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
                matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "" });
                ddlMaterialSubGrupoPedido.DataSource = matSubGrupo;
                ddlMaterialSubGrupoPedido.DataBind();
            }
        }
        private void CarregarCores()
        {
            var coresBasicas = prodController.ObterCoresBasicas();

            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
            ddlCorPedido.DataSource = coresBasicas;
            ddlCorPedido.DataBind();
        }
        private void CarregarCorFornecedor(string grupo, string subGrupo)
        {

            var filtroMaterial = desenvController.ObterMaterial().Where(p => p.GRUPO.Trim() == grupo.Trim() && p.SUBGRUPO.Trim() == subGrupo.Trim()).ToList();

            var materialCores = desenvController.ObterMaterialCor();
            materialCores = materialCores.Where(i => filtroMaterial.Any(g => g.MATERIAL.Trim() == i.MATERIAL.Trim())).OrderBy(p => p.DESC_COR_MATERIAL).ToList();

            materialCores.Insert(0, new MATERIAIS_CORE { REFER_FABRICANTE = "" });
            ddlCorFornecedorPedido.DataSource = materialCores.OrderBy(p => p.REFER_FABRICANTE);
            ddlCorFornecedorPedido.DataBind();
        }
        private void CarregarFornecedores()
        {
            var fornecedores = prodController.ObterFornecedor().Where(p => p.STATUS == 'A').ToList();
            if (fornecedores != null)
            {
                var fornecedoresAux = fornecedores.Where(p => p.FORNECEDOR != null).Select(s => s.FORNECEDOR.Trim()).Distinct().ToList();

                fornecedores = new List<PROD_FORNECEDOR>();
                foreach (var item in fornecedoresAux)
                    if (item.Trim() != "")
                        fornecedores.Add(new PROD_FORNECEDOR { FORNECEDOR = item.Trim() });

                fornecedores.OrderBy(p => p.FORNECEDOR);
                fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "" });

                ddlFornecedor.DataSource = fornecedores;
                ddlFornecedor.DataBind();
            }
        }
        protected void ddlPedidoFornecedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var f = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == ddlFornecedor.SelectedValue.Trim());
            if (f != null)
            {
                var email = f.Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                if (email != null && email.Count() > 0)
                    txtEmail.Text = email[0].EMAIL;
            }
        }
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

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labColecao.ForeColor = _OK;
            if (ddlColecao.SelectedValue.Trim() == "")
            {
                labColecao.ForeColor = _notOK;
                retorno = false;
            }

            labPagamento.ForeColor = _OK;
            if (ddlPagamento.SelectedValue.Trim() == "0")
            {
                labPagamento.ForeColor = _notOK;
                retorno = false;
            }

            labFornecedor.ForeColor = _OK;
            if (ddlFornecedor.SelectedValue.Trim() == "")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labQuantidade.ForeColor = _OK;
            decimal qtdeVal = 0;
            if (txtQtde.Text.Trim() == "" || !Decimal.TryParse(txtQtde.Text, out qtdeVal))
            {
                labQuantidade.ForeColor = _notOK;
                retorno = false;
            }

            labPreco.ForeColor = _OK;
            if (txtPreco.Text.Trim() == "")
            {
                labPreco.ForeColor = _notOK;
                retorno = false;
            }

            labDataPedido.ForeColor = _OK;
            if (txtPedidoData.Text.Trim() == "")
            {
                labDataPedido.ForeColor = _notOK;
                retorno = false;
            }

            labDataPrevisaoEntrega.ForeColor = _OK;
            if (txtPedidoPrevEntrega.Text.Trim() == "")
            {
                labDataPrevisaoEntrega.ForeColor = _notOK;
                retorno = false;
            }

            labStatus.ForeColor = _OK;
            if (ddlStatus.SelectedValue == "")
            {
                labStatus.ForeColor = _notOK;
                retorno = false;
            }


            chkEmail.ForeColor = _OK;
            labCorpoEmail.ForeColor = _OK;
            if (chkEmail.Checked)
            {
                if (txtCorpoEmail.Text.Trim() == "")
                {
                    labCorpoEmail.ForeColor = _notOK;
                    retorno = false;
                }

                if (txtEmail.Text.Trim() == "")
                {
                    chkEmail.ForeColor = _notOK;
                    retorno = false;
                }

                if (txtEmail.Text.Trim().Contains(","))
                    txtEmail.Text = txtEmail.Text.Trim().Replace(",", ";");

                if (txtEmail.Text.Trim() == "" || !Utils.WebControls.ValidarEmail(txtEmail.Text.Trim()))
                {
                    chkEmail.ForeColor = _notOK;
                    retorno = false;
                }
            }

            return retorno;
        }
        #endregion

        private void CarregarPedido(DESENV_PEDIDO pedido, int codigoPedidoSub = 0)
        {

            CarregarFornecedores();
            var f = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == pedido.FORNECEDOR.Trim());
            if (f != null && f.Count() > 0)
            {
                ddlFornecedor.SelectedValue = f.Take(1).SingleOrDefault().FORNECEDOR.Trim();
                var email = f.Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                if (email != null && email.Count() > 0)
                    txtEmail.Text = email[0].EMAIL;
            }

            CarregarGrupos();
            var g = desenvController.ObterMaterialGrupo(pedido.GRUPO);
            if (g != null)
                ddlMaterialGrupoPedido.SelectedValue = g.CODIGO_GRUPO;

            CarregarSubGrupos(pedido.GRUPO);
            var s = desenvController.ObterMaterialSubGrupo(pedido.GRUPO, pedido.SUBGRUPO);
            if (s != null)
                ddlMaterialSubGrupoPedido.SelectedValue = s.CODIGO_SUBGRUPO;

            CarregarCores();
            var c = prodController.ObterCoresBasicas(pedido.COR);
            if (c != null)
                ddlCorPedido.SelectedValue = c.COR;

            CarregarCorFornecedor(pedido.GRUPO, pedido.SUBGRUPO);
            ddlCorFornecedorPedido.SelectedValue = pedido.COR_FORNECEDOR;
            txtPreco.Text = pedido.VALOR.ToString();
            txtPedidoData.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtPedidoPrevEntrega.Text = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");

            if (codigoPedidoSub > 0)
            {
                var subPedido = desenvController.ObterPedidoSub(codigoPedidoSub);
                if (subPedido != null)
                {
                    ddlColecao.Enabled = false;
                    ddlColecao.SelectedValue = subPedido.COLECAO;
                    //ddlFornecedor.Enabled = false;
                    ddlFornecedor.SelectedValue = subPedido.FORNECEDOR;
                    hidQtdeAnt.Value = subPedido.QTDE.ToString();
                    txtQtde.Text = subPedido.QTDE.ToString();
                    txtPreco.Text = subPedido.VALOR.ToString();
                    txtPedidoData.Enabled = false;
                    txtPedidoData.Text = (subPedido.DATA_PEDIDO == null) ? "" : Convert.ToDateTime(subPedido.DATA_PEDIDO).ToString("dd/MM/yyyy");
                    txtPedidoPrevEntrega.Text = (subPedido.DATA_PREVISAO_FINAL == null) ? Convert.ToDateTime(subPedido.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy") : Convert.ToDateTime(subPedido.DATA_PREVISAO_FINAL).ToString("dd/MM/yyyy");
                    txtEmail.Text = subPedido.EMAIL;

                    if (subPedido.STATUS != null)
                        ddlStatus.SelectedValue = subPedido.STATUS.ToString();

                    ddlPagamento.Enabled = false;
                    var pagto = desenvController.ObterPedidoSubPgtoPorDesenvPedidoSub(subPedido.CODIGO);
                    if (pagto != null && pagto.Count() > 0)
                        ddlPagamento.SelectedValue = pagto.Count().ToString();

                    //chkEmail.Enabled = false;
                    chkEmail.Checked = false;
                    chkEmail_CheckedChanged(chkEmail, null);

                    CarregarPrePedido();
                }
            }

        }
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho.";
                    return;
                }

                var inc = false;
                var pedidoSub = new DESENV_PEDIDO_SUB();

                if (hidCodigoSubPedido.Value != "" && hidCodigoSubPedido.Value != "0")
                {
                    pedidoSub = desenvController.ObterPedidoSub(Convert.ToInt32(hidCodigoSubPedido.Value));
                }
                else
                {
                    inc = true;
                    // pedido novo
                    pedidoSub.DESENV_PEDIDO = Convert.ToInt32(hidCodigoPedido.Value);
                    pedidoSub.DATA_PEDIDO = Convert.ToDateTime(txtPedidoData.Text);
                    pedidoSub.DATA_ENTREGA_PREV = Convert.ToDateTime(txtPedidoPrevEntrega.Text);
                    pedidoSub.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    pedidoSub.DATA_INCLUSAO = DateTime.Now;
                    pedidoSub.COLECAO = ddlColecao.SelectedValue;
                    pedidoSub.ITEM = 1;

                    var subPedidos = desenvController.ObterPedidoSubPorDesenvPedido(pedidoSub.DESENV_PEDIDO).OrderByDescending(p => p.ITEM).ToList();
                    if (subPedidos != null && subPedidos.Count() > 0)
                        pedidoSub.ITEM = ((subPedidos[0].ITEM == null) ? 0 : subPedidos[0].ITEM) + 1;
                }

                pedidoSub.FORNECEDOR = ddlFornecedor.SelectedValue;
                pedidoSub.QTDE = Convert.ToDecimal(txtQtde.Text);
                pedidoSub.VALOR = Convert.ToDecimal(txtPreco.Text);
                pedidoSub.DATA_PREVISAO_FINAL = Convert.ToDateTime(txtPedidoPrevEntrega.Text);
                pedidoSub.EMAIL = txtCorpoEmail.Text;

                pedidoSub.STATUS = Convert.ToChar(ddlStatus.SelectedValue);
                pedidoSub.DATA_BAIXA = null;
                if (pedidoSub.STATUS == 'B')
                    pedidoSub.DATA_BAIXA = DateTime.Now;

                //PEDIDO PRINCIPAL -- ATUALIZAR QTDE
                var pedido = desenvController.ObterPedido(pedidoSub.DESENV_PEDIDO);
                if (pedido == null)
                {
                    labErro.Text = "Pedido principal não foi encontrado.";
                    return;
                }
                pedido.QTDE = (pedido.QTDE - Convert.ToDecimal(hidQtdeAnt.Value)) + Convert.ToDecimal(pedidoSub.QTDE);
                desenvController.AtualizarPedido(pedido);
                // FIM pedido PRINCIPAL

                if (inc)
                {
                    var codigoSubPedido = desenvController.InserirPedidoSub(pedidoSub);
                    if (codigoSubPedido > 0)
                    {
                        // inserir pagamentos
                        int i = 1;
                        int parcelas = Convert.ToInt32(ddlPagamento.SelectedValue);
                        var dataPagamento = pedidoSub.DATA_PEDIDO;
                        while (i <= parcelas)
                        {
                            var parcelaPedido = new DESENV_PEDIDO_SUB_PGTO();
                            parcelaPedido.DESENV_PEDIDO_SUB = codigoSubPedido;
                            parcelaPedido.PARCELA = i;
                            parcelaPedido.VALOR = (Convert.ToDecimal(pedidoSub.QTDE * pedidoSub.VALOR) / (parcelas * 1.00M));
                            parcelaPedido.PORC = 0;
                            parcelaPedido.DATA_INCLUSAO = DateTime.Now;
                            dataPagamento = dataPagamento.Value.AddDays(30);
                            parcelaPedido.DATA_PAGAMENTO = dataPagamento;

                            desenvController.InserirPedidoSubPgto(parcelaPedido);

                            i += 1;
                        }

                        labErro.Text = "Pedido enviado com sucesso.";

                        btSalvar.Enabled = false;
                        hidCodigoSubPedido.Value = codigoSubPedido.ToString();

                        CarregarPrePedido();
                    }
                }
                else
                {
                    // atualizar subpedido
                    desenvController.AtualizarPedidoSub(pedidoSub);
                    labErro.Text = "Pedido atualizado com sucesso. Confira os valores de Pagamento do Pedido.";
                    btSalvar.Enabled = false;
                }

                //Enviar E-mail
                if (chkEmail.Checked)
                    EnviarEmail(pedidoSub);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "EMAIL"
        protected void chkEmail_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk != null)
            {
                txtCorpoEmail.Enabled = chk.Checked;
                txtCorpoEmail.Text = "";
                txtEmail.Enabled = chk.Checked;
                txtEmail.Text = "";
            }
        }
        private void EnviarEmail(DESENV_PEDIDO_SUB pedidoSub)
        {
            string assuntoEmail = "";

            assuntoEmail = "Handbook - Pedido de Compra: " + pedidoSub.DESENV_PEDIDO1.NUMERO_PEDIDO.ToString().Trim() + "-" + pedidoSub.ITEM.ToString().Trim() + " - " + pedidoSub.DESENV_PEDIDO1.SUBGRUPO + " ";
            assuntoEmail = assuntoEmail + pedidoSub.DESENV_PEDIDO1.COR_FORNECEDOR;

            email_envio email = new email_envio();
            email.ASSUNTO = assuntoEmail;
            email.REMETENTE = (USUARIO)Session["USUARIO"];
            email.MENSAGEM = MontarCorpoEmail(pedidoSub, txtCorpoEmail.Text);

            List<string> destinatario = new List<string>();
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(3, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);

            if (txtEmail.Text.Trim() != "")
            {
                string[] sEmail = txtEmail.Text.Trim().Split(';');
                if (sEmail != null && sEmail.Count() > 0)
                    foreach (string s in sEmail)
                        destinatario.Add(s);
            }

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(DESENV_PEDIDO_SUB pedidoSub, string corpoEmail)
        {

            var pedido = desenvController.ObterPedido(pedidoSub.DESENV_PEDIDO);

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>COMPRA DE MATERIAL</title>");
            sb.Append("    <meta charset='UTF-8'>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <span style='font-family:Calibri; font-size:medium;'>Compra de Material</span>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <div id='divMaterial' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 1000pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            sb.Append("                            width: 750pt'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 180px; font-family:Calibri;'>");
            sb.Append("                                    Pedido:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + pedido.NUMERO_PEDIDO + "-" + pedidoSub.ITEM.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + pedidoSub.FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Material:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + pedido.SUBGRUPO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + pedido.COR_FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Quantidade:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + Convert.ToDecimal(pedidoSub.QTDE).ToString("###,###,###,##0.000") + " " + ((pedido.UNIDADE_MEDIDA1 == null) ? "" : (pedido.UNIDADE_MEDIDA1.DESCRICAO + "s")));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Preço:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    R$ " + Convert.ToDecimal(pedidoSub.VALOR).ToString("###,###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Data do Pedido:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + Convert.ToDateTime(pedidoSub.DATA_PEDIDO).ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            if (pedidoSub.DATA_ENTREGA_PREV != null)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Previsão de Entrega:");
                sb.Append("                                </td>");
                sb.Append("                                <td style='font-family:Calibri;'>");
                sb.Append("                                    " + Convert.ToDateTime(pedidoSub.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy"));
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        " + corpoEmail.Replace("\n", "<br>").Replace("\r", "<br>"));
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        Atenciosamente, <br />");
            sb.Append("        <span>" + ((USUARIO)Session["USUARIO"]).NOME_USUARIO + "</span>");
            sb.Append("        Handbook");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion

        private void CarregarPrePedido()
        {
            var prePedido = desenvController.ObterPrePedidoMarca("HANDBOOK", ddlColecao.SelectedValue, "").Where(
                p => p.COR.Trim() == ddlCorPedido.SelectedValue.Trim());

            prePedido = prePedido.Where(p => p.TECIDO.Contains(ddlMaterialGrupoPedido.SelectedItem.Text.Trim())
            || p.TECIDO.Contains(ddlMaterialSubGrupoPedido.SelectedItem.Text.Trim())).ToList();

            gvPrePedido.DataSource = prePedido;
            gvPrePedido.DataBind();
        }
        protected void gvPrePedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PREPEDIDOResult pre = e.Row.DataItem as SP_OBTER_PREPEDIDOResult;

                    Literal litConsumo = e.Row.FindControl("litConsumo") as Literal;
                    litConsumo.Text = pre.CONSUMO.ToString("###,###,##0.000");

                    Literal litPrecoTecido = e.Row.FindControl("litPrecoTecido") as Literal;
                    litPrecoTecido.Text = "R$ " + pre.PRECO_TECIDO.ToString("###,###,###,##0.00");

                    Literal litConsumoTotal = e.Row.FindControl("litConsumoTotal") as Literal;
                    litConsumoTotal.Text = pre.CONSUMO_TOTAL.ToString("###,###,###,##0.000");

                    Literal litValorTecido = e.Row.FindControl("litValorTecido") as Literal;
                    litValorTecido.Text = "R$ " + (pre.PRECO_TECIDO * pre.CONSUMO_TOTAL).ToString("###,###,###,##0.00");

                    Literal litPedido = e.Row.FindControl("litPedido") as Literal;
                    litPedido.Text = (pre.NUMERO_PEDIDO == null) ? "-" : (pre.NUMERO_PEDIDO.ToString() + "-" + pre.ITEM.ToString());

                    CheckBox cbAssociar = e.Row.FindControl("cbAssociar") as CheckBox;
                    ImageButton btExcluir = e.Row.FindControl("btExcluir") as ImageButton;
                    if (pre.CODIGO_PREPEDIDOSUB == 0)
                    {
                        btExcluir.Visible = false;
                    }
                    else
                    {
                        cbAssociar.Checked = true;
                        cbAssociar.Enabled = false;
                    }



                    gTotConsumo += Convert.ToDecimal(pre.CONSUMO);
                    gTotConsumoTotal += Convert.ToDecimal(pre.CONSUMO_TOTAL);

                    gTotTecido += (pre.PRECO_TECIDO * pre.CONSUMO_TOTAL);
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvPrePedido_DataBound(object sender, EventArgs e)
        {
            var footer = gvPrePedido.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[8].Text = gTotConsumo.ToString("###,###,###,##0.000");
                footer.Cells[10].Text = gTotConsumoTotal.ToString("###,###,###,##0.000");

                footer.Cells[11].Text = "R$ " + gTotTecido.ToString("###,###,###,##0.00");

            }
        }
        protected void btExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton bt = (ImageButton)sender;
                GridViewRow row = (GridViewRow)bt.NamingContainer;

                var codigoPrePedidoSub = Convert.ToInt32(gvPrePedido.DataKeys[row.RowIndex][1].ToString());
                desenvController.ExcluirPrePedidoSubPorCodigo(codigoPrePedidoSub);
                CheckBox cb = row.FindControl("cbAssociar") as CheckBox;
                cb.Checked = false;
                cb.Enabled = true;
                bt.Visible = false;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void cbAssociar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                CheckBox cb = (CheckBox)sender;
                GridViewRow row = (GridViewRow)cb.NamingContainer;
                if (cb.Checked)
                {
                    var codigoPrePedido = Convert.ToInt32(gvPrePedido.DataKeys[row.RowIndex][0].ToString());
                    var codigoSubPedido = Convert.ToInt32(hidCodigoSubPedido.Value);

                    var existe = desenvController.ObterPrepedidoSub(codigoPrePedido, codigoSubPedido);
                    if (existe == null)
                    {
                        var preSub = new DESENV_PREPEDIDO_SUB();
                        preSub.DESENV_PEDIDO_SUB = codigoSubPedido;
                        preSub.DESENV_PREPEDIDO = codigoPrePedido;
                        preSub.DATA_INCLUSAO = DateTime.Now;
                        desenvController.InserirPrepedidoSub(preSub);
                    }

                    row.BackColor = Color.PaleGreen;
                }
                else
                {
                    row.BackColor = Color.White;
                }

                row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(row.BackColor) + "';this.style.cursor='hand'");
            }
            catch (Exception)
            {
            }
        }
    }
}
