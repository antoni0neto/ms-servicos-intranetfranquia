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
    public partial class pacab_entrada_produto_baixar_old : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtEmissao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtRecebimento.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {

                string produto = "";
                string cor = "";
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Request.QueryString["c"] == null || Request.QueryString["c"] == "" ||
                    Session["USUARIO"] == null)
                    Response.Redirect("pacab_menu.aspx");

                produto = Request.QueryString["p"].ToString();
                cor = Request.QueryString["c"].ToString();

                var _entrada = desenvController.ObterProdutoAcabadoEntrada("", "", "").FirstOrDefault();
                if (_entrada == null)
                    Response.Redirect("pacab_menu.aspx");

                //if (_entrada.STATUS != 'A' && _entrada.STATUS != 'P')
                //{
                //    Response.Write("ENTRADA DE PRODUTO JÁ FOI REALIZADA.");
                //    Response.End();
                //}

                //hidCodigoEntrada.Value = codigoEntrada.ToString();

                txtColecao.Text = new BaseController().BuscaColecaoAtual(_entrada.COLECAO).DESC_COLECAO.Trim();
                txtProduto.Text = _entrada.PRODUTO;
                txtNome.Text = _entrada.NOME;
                txtCor.Text = _entrada.DESC_COR;
                txtQtde.Text = _entrada.QTDE_ENTREGAR.ToString();
                txtGriffe.Text = _entrada.GRIFFE;
                txtFornecedor.Text = _entrada.FORNECEDOR;


                txtGradeEXP_E.Text = _entrada.CE7.ToString();
                txtGradeXP_E.Text = _entrada.CE1.ToString();
                txtGradePP_E.Text = _entrada.CE2.ToString();
                txtGradeP_E.Text = _entrada.CE3.ToString();
                txtGradeM_E.Text = _entrada.CE4.ToString();
                txtGradeG_E.Text = _entrada.CE5.ToString();
                txtGradeGG_E.Text = _entrada.CE6.ToString();
                txtGradeTotal_E.Text = _entrada.QTDE_ENTREGAR.ToString();

                if (_entrada.CE7 <= 0)
                {
                    txtGradeEXP_R.Text = "0";
                    txtGradeEXP_R.Enabled = false;
                }
                if (_entrada.CE1 <= 0)
                {
                    txtGradeXP_R.Text = "0";
                    txtGradeXP_R.Enabled = false;
                }
                if (_entrada.CE2 <= 0)
                {
                    txtGradePP_R.Text = "0";
                    txtGradePP_R.Enabled = false;
                }
                if (_entrada.CE3 <= 0)
                {
                    txtGradeP_R.Text = "0";
                    txtGradeP_R.Enabled = false;
                }
                if (_entrada.CE4 <= 0)
                {
                    txtGradeM_R.Text = "0";
                    txtGradeM_R.Enabled = false;
                }
                if (_entrada.CE5 <= 0)
                {
                    txtGradeG_R.Text = "0";
                    txtGradeG_R.Enabled = false;
                }
                if (_entrada.CE6 <= 0)
                {
                    txtGradeGG_R.Text = "0";
                    txtGradeGG_R.Enabled = false;
                }

                txtGradeTotal_R.Enabled = false;

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue == "")
            {
                labFilial.ForeColor = _notOK;
                retorno = false;
            }

            labQtdeNota.ForeColor = _OK;
            if (txtQtdeNota.Text.Trim() == "")
            {
                labQtdeNota.ForeColor = _notOK;
                retorno = false;
            }

            labNF.ForeColor = _OK;
            if (txtNF.Text.Trim() == "")
            {
                labNF.ForeColor = _notOK;
                retorno = false;
            }

            labSerie.ForeColor = _OK;
            if (txtSerie.Text.Trim() == "")
            {
                labSerie.ForeColor = _notOK;
                retorno = false;
            }

            labEmissao.ForeColor = _OK;
            if (txtEmissao.Text.Trim() == "")
            {
                labEmissao.ForeColor = _notOK;
                retorno = false;
            }

            labRecebimento.ForeColor = _OK;
            if (txtRecebimento.Text.Trim() == "")
            {
                labRecebimento.ForeColor = _notOK;
                retorno = false;
            }

            labStatus.ForeColor = _OK;
            if (ddlStatus.SelectedValue == "")
            {
                labStatus.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var _entrada = desenvController.ObterProdutoPedidoLinxEntrada(Convert.ToInt32(hidCodigoEntrada.Value));

                if (_entrada == null)
                {
                    labErro.Text = "Nenhuma entrada de produto encontrada. Entre em contato com TI.";
                    return;
                }

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                if (ddlStatus.SelectedValue == "B" && (txtGradeTotal_R.Text == "" || txtGradeTotal_R.Text == "0"))
                {
                    labErro.Text = "Informe a grade recebida.";
                    return;
                }

                _entrada.CODIGO_FILIAL = ddlFilial.SelectedValue;

                _entrada.NF_ENTRADA = txtNF.Text.Trim();
                _entrada.SERIE_NF = txtSerie.Text.Trim();
                _entrada.EMISSAO = Convert.ToDateTime(txtEmissao.Text);
                _entrada.RECEBIMENTO = Convert.ToDateTime(txtRecebimento.Text);
                _entrada.USUARIO_RECEBIMENTO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                if (txtGradeEXP_R.Text.Trim() != "")
                    _entrada.GRADE_EXP = Convert.ToInt32(txtGradeEXP_R.Text.Trim());
                if (txtGradeXP_R.Text.Trim() != "")
                    _entrada.GRADE_XP = Convert.ToInt32(txtGradeXP_R.Text.Trim());
                if (txtGradePP_R.Text.Trim() != "")
                    _entrada.GRADE_PP = Convert.ToInt32(txtGradePP_R.Text.Trim());
                if (txtGradeP_R.Text.Trim() != "")
                    _entrada.GRADE_P = Convert.ToInt32(txtGradeP_R.Text.Trim());
                if (txtGradeM_R.Text.Trim() != "")
                    _entrada.GRADE_M = Convert.ToInt32(txtGradeM_R.Text.Trim());
                if (txtGradeG_R.Text.Trim() != "")
                    _entrada.GRADE_G = Convert.ToInt32(txtGradeG_R.Text.Trim());
                if (txtGradeGG_R.Text.Trim() != "")
                    _entrada.GRADE_GG = Convert.ToInt32(txtGradeGG_R.Text.Trim());

                int gradeTotal = 0;
                if (txtGradeTotal_R.Text.Trim() != "")
                {
                    gradeTotal = (_entrada.GRADE_EXP + _entrada.GRADE_XP + _entrada.GRADE_PP + _entrada.GRADE_P + _entrada.GRADE_M + _entrada.GRADE_G + _entrada.GRADE_GG);
                    _entrada.GRADE_TOTAL = gradeTotal;
                }

                _entrada.STATUS = Convert.ToChar(ddlStatus.SelectedValue);
                _entrada.QTDE_NOTA = Convert.ToInt32(txtQtdeNota.Text.Trim());

                desenvController.AtualizarProdutoPedidoLinxEntrada(_entrada);

                if (Constante.enviarEmail)
                    if (_entrada.STATUS == 'B')
                    {
                        var entradaProdutoAcabado = desenvController.ObterProdutoAcabadoEntrada("", "", "").FirstOrDefault();
                        EnviarEmail(entradaProdutoAcabado);
                    }

                labErro.Text = "Entrada de Produto Acabado salva com sucesso.";

                btSalvar.Enabled = false;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "EMAIL"
        private void EnviarEmail(SP_OBTER_PRODUTO_ACABADO_ENTRADAResult entradaProdutoAcabado)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            string assunto = "";

            //var titulo = "ENT PRODUTO ACABADO TERCEIRO";

            //assunto = "Intranet: " + titulo + " - PEDIDO COMPRA: " + entradaProdutoAcabado.PEDIDO;
            email.ASSUNTO = assunto;
            email.REMETENTE = usuario;
            email.MENSAGEM = "";// MontarCorpoEmail(entradaProdutoAcabado, assunto);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails 
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(12, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        //private string MontarCorpoEmail(SP_OBTER_PRODUTO_ACABADO_ENTRADAResult entradaProdutoAcabado, string assunto)
        //{
        //    int codigoUsuario = 0;
        //    codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("");

        //    sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
        //    sb.Append("<html>");
        //    sb.Append("<head>");
        //    sb.Append("    <title>ENTRADA DE PRODUTO ACABADO</title>");
        //    sb.Append("    <meta charset='UTF-8' />");
        //    sb.Append("</head>");
        //    sb.Append("<body>");
        //    sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
        //    sb.Append("        background: white; white-space: nowrap;'>");
        //    sb.Append("        <br />");
        //    sb.Append("        <div id='divSaida' align='left'>");
        //    sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
        //    sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
        //    sb.Append("                background: white; white-space: nowrap;'>");

        //    sb.Append("                <tr>");
        //    sb.Append("                    <td style='text-align:center;'>");
        //    sb.Append("                        <h2>" + assunto.Replace("Intranet:", "") + "</h2>");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td style='text-align:center;'>");
        //    sb.Append("                        <hr />");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td style='line-height: 20px;'>");
        //    sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
        //    sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
        //    sb.Append("                            background: white; white-space: nowrap;'>");
        //    sb.Append("                            <tr style='text-align: left;'>");
        //    sb.Append("                                <td style='width: 235px;'>");
        //    sb.Append("                                    PEDIDO:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + entradaProdutoAcabado.PEDIDO);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Coleção:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + new BaseController().BuscaColecaoAtual(entradaProdutoAcabado.COLECAO).DESC_COLECAO.Trim());
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Produto:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + entradaProdutoAcabado.PRODUTO);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Nome");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + entradaProdutoAcabado.NOME);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Cor:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + entradaProdutoAcabado.COR_PRODUTO + " - " + entradaProdutoAcabado.DESC_COR);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Quantidade:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + entradaProdutoAcabado.GRADE_TOTAL);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");

        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Fornecedor:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + entradaProdutoAcabado.FORNECEDOR);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Filial:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + entradaProdutoAcabado.CODIGO_FILIAL + " - " + (new BaseController().BuscaFilialCodigoInt(Convert.ToInt32(entradaProdutoAcabado.CODIGO_FILIAL)).FILIAL.Trim()));
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Nota Fiscal:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + entradaProdutoAcabado.NF_ENTRADA);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Série:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + entradaProdutoAcabado.SERIE_NF);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Emissão:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + Convert.ToDateTime(entradaProdutoAcabado.EMISSAO).ToString("dd/MM/yyyy"));
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Recebimento:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + Convert.ToDateTime(entradaProdutoAcabado.RECEBIMENTO).ToString("dd/MM/yyyy"));
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    &nbsp;");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    &nbsp;");
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");
        //    sb.Append("                             <tr>");
        //    sb.Append("                                 <td colspan='2'>");
        //    sb.Append("                                     <table cellpadding='0' cellspacing='0' style='width: 550pt; padding: 0px; color: black;");
        //    sb.Append("                                         font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
        //    sb.Append("                                         border: 1px solid #ccc;'>");
        //    sb.Append("                                         <tr style='background-color: #ccc;'>");
        //    sb.Append("                                             <td>");
        //    sb.Append("                                                 &nbsp;");
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center;'>");
        //    sb.Append("                                                  EXP");
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center;'>");
        //    sb.Append("                                                  XP");
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center;'>");
        //    sb.Append("                                                  PP");
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center;'>");
        //    sb.Append("                                                  P");
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center;'>");
        //    sb.Append("                                                  M");
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center;'>");
        //    sb.Append("                                                  G");
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center;'>");
        //    sb.Append("                                                  GG");
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center;'>");
        //    sb.Append("                                                  &nbsp;");
        //    sb.Append("                                             </td>");
        //    sb.Append("                                         </tr>");
        //    sb.Append("                                         <tr>");
        //    sb.Append("                                             <td>");
        //    sb.Append("                                                 Grade Recebida");
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center; width: 75px;'>");
        //    sb.Append("                                                  " + entradaProdutoAcabado.GRADE_EXP.ToString());
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center; width: 75px;'>");
        //    sb.Append("                                                  " + entradaProdutoAcabado.GRADE_XP.ToString());
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center; width: 75px;'>");
        //    sb.Append("                                                  " + entradaProdutoAcabado.GRADE_PP.ToString());
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center; width: 75px;'>");
        //    sb.Append("                                                  " + entradaProdutoAcabado.GRADE_P.ToString());
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center; width: 75px;'>");
        //    sb.Append("                                                  " + entradaProdutoAcabado.GRADE_M.ToString());
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center; width: 75px;'>");
        //    sb.Append("                                                  " + entradaProdutoAcabado.GRADE_G.ToString());
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center; width: 75px;'>");
        //    sb.Append("                                                  " + entradaProdutoAcabado.GRADE_GG.ToString());
        //    sb.Append("                                             </td>");
        //    sb.Append("                                             <td style='text-align: center; width: 75px;'>");
        //    sb.Append("                                                  " + entradaProdutoAcabado.GRADE_TOTAL.ToString());
        //    sb.Append("                                             </td>");
        //    sb.Append("                                         </tr>");
        //    sb.Append("                                     </table>");
        //    sb.Append("                                 </td>");
        //    sb.Append("                             </tr>");
        //    sb.Append("                             <tr>");
        //    sb.Append("                                 <td>");
        //    sb.Append("                                     &nbsp;");
        //    sb.Append("                                 </td>");
        //    sb.Append("                                 <td>");
        //    sb.Append("                                     &nbsp;");
        //    sb.Append("                                 </td>");
        //    sb.Append("                             </tr>");
        //    sb.Append("                        </table>");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td>");
        //    sb.Append("                        &nbsp;");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td>");
        //    sb.Append("                        &nbsp;");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("            </table>");
        //    sb.Append("        </div>");
        //    sb.Append("        <br />");
        //    sb.Append("        <br />");
        //    sb.Append("        <span>Enviado por: " + (new BaseController().BuscaUsuario(codigoUsuario).NOME_USUARIO.ToUpper()) + "</span>");
        //    sb.Append("    </div>");
        //    sb.Append("</body>");
        //    sb.Append("</html>");


        //    return sb.ToString();
        //}
        #endregion

    }
}
