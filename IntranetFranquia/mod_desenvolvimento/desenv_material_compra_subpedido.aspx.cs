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
    public partial class desenv_material_compra_subpedido : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();

        decimal qtdeAviamento = 0;
        decimal qtdeAviamentoMarcado = 0;

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

                DESENV_PEDIDO _pedido = desenvController.ObterPedidoNumero(numeroPedido);
                if (_pedido == null)
                    Response.Redirect("desenv_menu.aspx");

                hidCodigoPedido.Value = _pedido.CODIGO.ToString();

                txtPedidoNumero.Text = _pedido.NUMERO_PEDIDO.ToString();

                CarregarFornecedores();
                var f = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == _pedido.FORNECEDOR.Trim());
                if (f != null && f.Count() > 0)
                {
                    ddlPedidoFornecedor.SelectedValue = f.Take(1).SingleOrDefault().FORNECEDOR.Trim();
                    var email = f.Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                    if (email != null && email.Count() > 0)
                        txtEmail.Text = email[0].EMAIL;
                }

                CarregarGrupos();
                var g = desenvController.ObterMaterialGrupo(_pedido.GRUPO);
                if (g != null)
                    ddlMaterialGrupoPedido.SelectedValue = g.CODIGO_GRUPO;

                CarregarSubGrupos(_pedido.GRUPO);
                var s = desenvController.ObterMaterialSubGrupo(_pedido.GRUPO, _pedido.SUBGRUPO);
                if (s != null)
                    ddlMaterialSubGrupoPedido.SelectedValue = s.CODIGO_SUBGRUPO;

                CarregarCores();
                var c = prodController.ObterCoresBasicas(_pedido.COR);
                if (c != null)
                    ddlCorPedido.SelectedValue = c.COR;

                CarregarCorFornecedor(_pedido.GRUPO, _pedido.SUBGRUPO);
                ddlCorFornecedorPedido.SelectedValue = _pedido.COR_FORNECEDOR;
                txtPreco.Text = _pedido.VALOR.ToString();
                txtPedidoData.Text = DateTime.Today.ToString("dd/MM/yyyy");
                txtPedidoPrevEntrega.Text = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");

                CarregarAviamentoHB(_pedido.GRUPO, _pedido.SUBGRUPO, _pedido.COR, _pedido.COR_FORNECEDOR);


            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupos()
        {
            List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "" });

                ddlMaterialGrupoPedido.DataSource = _matGrupo;
                ddlMaterialGrupoPedido.DataBind();
            }
        }
        private void CarregarSubGrupos(string grupo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = null;

            _matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();

            if (_matSubGrupo != null)
            {
                _matSubGrupo = _matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "" });
                ddlMaterialSubGrupoPedido.DataSource = _matSubGrupo;
                ddlMaterialSubGrupoPedido.DataBind();
            }
        }
        private void CarregarCores()
        {
            var _coresBasicas = prodController.ObterCoresBasicas();

            _coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
            ddlCorPedido.DataSource = _coresBasicas;
            ddlCorPedido.DataBind();
        }
        private void CarregarCorFornecedor(string grupo, string subGrupo)
        {
            List<MATERIAI> filtroMaterial = new List<MATERIAI>();
            filtroMaterial = desenvController.ObterMaterial().Where(p => p.GRUPO.Trim() == grupo.Trim() && p.SUBGRUPO.Trim() == subGrupo.Trim()).ToList();

            List<MATERIAIS_CORE> materialCores = desenvController.ObterMaterialCor();
            materialCores = materialCores.Where(i => filtroMaterial.Any(g => g.MATERIAL.Trim() == i.MATERIAL.Trim())).OrderBy(p => p.DESC_COR_MATERIAL).ToList();

            materialCores.Insert(0, new MATERIAIS_CORE { REFER_FABRICANTE = "" });
            ddlCorFornecedorPedido.DataSource = materialCores.OrderBy(p => p.REFER_FABRICANTE);
            ddlCorFornecedorPedido.DataBind();
        }
        private void CarregarFornecedores()
        {
            List<PROD_FORNECEDOR> _fornecedor = prodController.ObterFornecedor().Where(p => p.STATUS == 'A').ToList();
            if (_fornecedor != null)
            {
                var _fornecedorAux = _fornecedor.Where(p => p.FORNECEDOR != null).Select(s => s.FORNECEDOR.Trim()).Distinct().ToList();

                _fornecedor = new List<PROD_FORNECEDOR>();
                foreach (var item in _fornecedorAux)
                    if (item.Trim() != "")
                        _fornecedor.Add(new PROD_FORNECEDOR { FORNECEDOR = item.Trim() });

                _fornecedor.OrderBy(p => p.FORNECEDOR);
                _fornecedor.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "" });

                ddlPedidoFornecedor.DataSource = _fornecedor;
                ddlPedidoFornecedor.DataBind();
            }
        }

        private void CarregarAviamentoHB(string grupo, string subgrupo, string cor, string corFornecedor)
        {
            List<SP_OBTER_HB_AVIAMENTO_PEDIDOResult> _pedidoHB = desenvController.ObterAviamentoHBPedido(grupo, subgrupo, cor, corFornecedor);

            _pedidoHB = _pedidoHB.Where(p => p.NUMERO_PEDIDO == null).ToList();

            gvAviamentoHB.DataSource = _pedidoHB.OrderBy(p => p.DESC_COLECAO).ThenBy(p => p.HB);
            gvAviamentoHB.DataBind();

            cbMarcar_CheckedChanged(null, null);
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFornecedorPedido.ForeColor = _OK;
            if (ddlPedidoFornecedor.SelectedValue.Trim() == "")
            {
                labFornecedorPedido.ForeColor = _notOK;
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

            labEmail.ForeColor = _OK;
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
                    labEmail.ForeColor = _notOK;
                    retorno = false;
                }

                if (txtEmail.Text.Trim().Contains(","))
                    txtEmail.Text = txtEmail.Text.Trim().Replace(",", ";");

                if (!Utils.WebControls.ValidarEmail(txtEmail.Text.Trim()))
                {
                    labEmail.ForeColor = _notOK;
                    retorno = false;
                }
            }

            return retorno;
        }
        #endregion

        protected void gvAviamentoHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HB_AVIAMENTO_PEDIDOResult _aviamentoHB = e.Row.DataItem as SP_OBTER_HB_AVIAMENTO_PEDIDOResult;
                    if (_aviamentoHB != null)
                    {

                        Literal _litMostruario = e.Row.FindControl("litMostruario") as Literal;
                        if (_litMostruario != null)
                            _litMostruario.Text = (_aviamentoHB.MOSTRUARIO == 'S') ? "Sim" : "Não";

                        if (_aviamentoHB.QTDE != null)
                            qtdeAviamento += Convert.ToDecimal(_aviamentoHB.QTDE);

                        CheckBox _cbMarcar = e.Row.FindControl("cbMarcar") as CheckBox;
                        if (_cbMarcar != null)
                        {
                            if (_cbMarcar.Checked)
                                qtdeAviamentoMarcado += Convert.ToDecimal(_aviamentoHB.QTDE);
                            else
                                qtdeAviamentoMarcado -= Convert.ToDecimal(_aviamentoHB.QTDE);
                        }
                    }
                }
            }
        }
        protected void gvAviamentoHB_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvAviamentoHB.FooterRow;
            if (footer != null)
            {
                footer.Cells[4].Text = qtdeAviamento.ToString("###,###,###,##0.000");
                footer.Cells[5].Text = qtdeAviamentoMarcado.ToString("###,###,###,##0.000");
            }
        }
        protected void cbMarcar_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _cbMarcar = null;
            Literal _litQtde = null;
            Literal _litHB = null;

            decimal qtde = 0;
            StringBuilder _texto = new StringBuilder();

            qtdeAviamento = 0;
            qtdeAviamentoMarcado = 0;

            _texto.Append("");
            foreach (GridViewRow row in gvAviamentoHB.Rows)
            {
                _cbMarcar = row.FindControl("cbMarcar") as CheckBox;
                _litQtde = row.FindControl("litQtde") as Literal;
                _litHB = row.FindControl("litHB") as Literal;

                qtde = (_litQtde.Text.Trim() == "") ? 0 : Convert.ToDecimal(_litQtde.Text.Trim());

                qtdeAviamento += qtde;

                if (_cbMarcar != null)
                {
                    if (_cbMarcar.Checked)
                    {
                        qtdeAviamentoMarcado += qtde;
                        _texto.Append("HB: " + _litHB.Text + " -     QTDE: " + qtde.ToString("###,###,###,##0.000"));
                        _texto.Append("\n");
                    }
                }
            }

            gvAviamentoHB_DataBound(gvAviamentoHB, null);
            txtQtde.Text = qtdeAviamentoMarcado.ToString();
            txtCorpoEmail.Text = _texto.ToString();
        }
        protected void chkMarcarTodos_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb.Checked)
                labMarcarTodos.Text = "Desmarcar Todos";
            else
                labMarcarTodos.Text = "Marcar Todos";

            //Des/Marcar todos
            CheckBox cbMarcar = null;
            foreach (GridViewRow item in gvAviamentoHB.Rows)
            {
                cbMarcar = item.FindControl("cbMarcar") as CheckBox;
                cbMarcar.Checked = cb.Checked;
            }

            cbMarcar_CheckedChanged(null, null);
        }

        protected void ddlPedidoFornecedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEmail.Text = "";
            var email = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == ddlPedidoFornecedor.SelectedValue.Trim() && p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            if (email != null && email.Count() > 0)
            {
                txtEmail.Text = email[0].EMAIL;
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

                DESENV_PEDIDO_SUB _pedidoSub = new DESENV_PEDIDO_SUB();
                _pedidoSub.DESENV_PEDIDO = Convert.ToInt32(hidCodigoPedido.Value);
                _pedidoSub.FORNECEDOR = ddlPedidoFornecedor.SelectedValue;
                _pedidoSub.QTDE = Convert.ToDecimal(txtQtde.Text);
                _pedidoSub.VALOR = Convert.ToDecimal(txtPreco.Text);
                _pedidoSub.DATA_PEDIDO = Convert.ToDateTime(txtPedidoData.Text);
                _pedidoSub.DATA_ENTREGA_PREV = Convert.ToDateTime(txtPedidoPrevEntrega.Text);
                _pedidoSub.EMAIL = txtCorpoEmail.Text;
                _pedidoSub.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _pedidoSub.DATA_INCLUSAO = DateTime.Now;

                var pedido = desenvController.ObterPedido(_pedidoSub.DESENV_PEDIDO);
                if (pedido == null)
                {
                    labErro.Text = "Pedido principal não foi encontrado.";
                    return;
                }

                pedido.QTDE = pedido.QTDE + Convert.ToDecimal(_pedidoSub.QTDE);

                desenvController.InserirPedidoSub(_pedidoSub);
                desenvController.AtualizarPedido(pedido);

                int codigoAviamento = 0;
                CheckBox _cbMarcar = null;
                foreach (GridViewRow row in gvAviamentoHB.Rows)
                {
                    _cbMarcar = row.FindControl("cbMarcar") as CheckBox;

                    if (_cbMarcar.Checked)
                    {
                        codigoAviamento = Convert.ToInt32(gvAviamentoHB.DataKeys[row.RowIndex].Value);

                        var _aviamento = prodController.ObterAviamentoHBCodigo(codigoAviamento);
                        if (_aviamento != null)
                        {
                            _aviamento.NUMERO_PEDIDO = Convert.ToInt32(txtPedidoNumero.Text);
                            prodController.AtualizarAviamentoHB(_aviamento);
                        }
                    }
                }

                //Enviar E-mail
                if (chkEmail.Checked)
                    EnviarEmail(_pedidoSub);

                labErro.Text = "Pedido inserido com sucesso.";
                txtQtde.Text = "";
                txtCorpoEmail.Text = "";

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
        private void EnviarEmail(DESENV_PEDIDO_SUB _pedidoSub)
        {
            string assuntoEmail = "";

            assuntoEmail = "HBF - Pedido de Compra: " + _pedidoSub.DESENV_PEDIDO1.NUMERO_PEDIDO.ToString().Trim() + " - " + _pedidoSub.DESENV_PEDIDO1.SUBGRUPO + " ";
            assuntoEmail = assuntoEmail + _pedidoSub.DESENV_PEDIDO1.COR_FORNECEDOR;

            email_envio email = new email_envio();
            email.ASSUNTO = assuntoEmail;
            email.REMETENTE = (USUARIO)Session["USUARIO"];
            email.MENSAGEM = MontarCorpoEmail(_pedidoSub, txtCorpoEmail.Text);

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
        private string MontarCorpoEmail(DESENV_PEDIDO_SUB _pedidoSub, string corpoEmail)
        {

            var _pedido = desenvController.ObterPedido(_pedidoSub.DESENV_PEDIDO);

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
            sb.Append("                                    Número:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + _pedido.NUMERO_PEDIDO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + _pedidoSub.FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Material:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + _pedido.SUBGRUPO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + _pedido.COR_FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Quantidade:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + Convert.ToDecimal(_pedidoSub.QTDE).ToString("###,###,###,##0.000") + " " + ((_pedido.UNIDADE_MEDIDA1 == null) ? "" : (_pedido.UNIDADE_MEDIDA1.DESCRICAO + "s")));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Preço:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    R$ " + Convert.ToDecimal(_pedidoSub.VALOR).ToString("###,###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Data do Pedido:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + Convert.ToDateTime(_pedidoSub.DATA_PEDIDO).ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            List<FORMA_PGTO> _formaPGTO = (new BaseController().ObterFormaPgto());
            if (_formaPGTO != null && _formaPGTO.Count > 0)
            {
                var fp = _formaPGTO.Where(p => p.CONDICAO_PGTO.Trim() == _pedido.CONDICAO_PGTO.Trim()).SingleOrDefault();
                if (fp != null)
                {
                    sb.Append("                            <tr>");
                    sb.Append("                                <td>");
                    sb.Append("                                    Condição de Pagamento:");
                    sb.Append("                                </td>");
                    sb.Append("                                <td style='font-family:Calibri;'>");
                    sb.Append("                                    " + ((_pedido.CONDICAO_PGTO.Trim() == "##") ? "" : fp.DESC_COND_PGTO.Trim()) + " " + _pedido.CONDICAO_PGTO_OUTRO.Trim());
                    sb.Append("                                </td>");
                    sb.Append("                            </tr>");
                }
            }

            if (_pedido.DATA_ENTREGA_PREV != null)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Previsão de Entrega:");
                sb.Append("                                </td>");
                sb.Append("                                <td style='font-family:Calibri;'>");
                sb.Append("                                    " + Convert.ToDateTime(_pedidoSub.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy"));
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
            sb.Append("                        " + corpoEmail.Replace("\n", "<br>"));
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion


    }
}
