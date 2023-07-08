using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;

namespace Relatorios
{
    public partial class cont_entrada60mais : System.Web.UI.Page
    {
        ContagemController contController = new ContagemController();
        BaseController baseController = new BaseController();
        EstoqueController estoqueController = new EstoqueController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                pnlBaixa.Visible = false;

                CarregarFilial();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btBaixarProduto.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBaixarProduto, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (ddlFilial.SelectedValue == "" || ddlFilial.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Filial.";
                    return;
                }

                CarregarProduto60Mais(ddlFilial.SelectedValue);

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        #region "GRID INICIAL"
        private void CarregarProduto60Mais(string codigoFilial)
        {
            var loja60mais = contController.ObterLoja60Mais(codigoFilial);
            gvProdutoEstoque.DataSource = loja60mais;
            gvProdutoEstoque.DataBind();

            pnlBaixa.Visible = false;
            if (loja60mais != null && loja60mais.Count() > 0)
                pnlBaixa.Visible = true;
        }
        protected void gvProdutoEstoque_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CONT_LOJA_60MAI _loja60Mais = e.Row.DataItem as CONT_LOJA_60MAI;

                    if (_loja60Mais != null)
                    {

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                            _litNome.Text = baseController.BuscaProduto(_loja60Mais.PRODUTO).DESC_PRODUTO.Trim();

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(_loja60Mais.COR_PRODUTO).DESC_COR;



                    }
                }
            }
        }
        #endregion

        #region "BAIXA"
        protected void btBaixarProduto_Click(object sender, EventArgs e)
        {
            try
            {
                labBaixa.Text = "";
                labErro.Text = "";

                if (txtResponsavel.Text.Trim() == "")
                {
                    labBaixa.Text = "Informe o Nome do responsável pela Contagem.";
                    return;
                }

                bool contagemPreenchida = true;
                foreach (GridViewRow r in gvProdutoEstoque.Rows)
                {
                    TextBox _txtGradeEXP = r.FindControl("txtGradeEXP") as TextBox;
                    TextBox _txtGradeXP = r.FindControl("txtGradeXP") as TextBox;
                    TextBox _txtGradePP = r.FindControl("txtGradePP") as TextBox;
                    TextBox _txtGradeP = r.FindControl("txtGradeP") as TextBox;
                    TextBox _txtGradeM = r.FindControl("txtGradeM") as TextBox;
                    TextBox _txtGradeG = r.FindControl("txtGradeG") as TextBox;
                    TextBox _txtGradeGG = r.FindControl("txtGradeGG") as TextBox;

                    if (_txtGradeEXP.Text.Trim() == "" ||
                        _txtGradeXP.Text.Trim() == "" ||
                        _txtGradePP.Text.Trim() == "" ||
                        _txtGradeP.Text.Trim() == "" ||
                        _txtGradeM.Text.Trim() == "" ||
                        _txtGradeG.Text.Trim() == "" ||
                        _txtGradeGG.Text.Trim() == "")
                    {
                        contagemPreenchida = false;
                        break;
                    }
                }

                if (!contagemPreenchida)
                {
                    labErro.Text = "ERRO - Informe a contagem de TODOS os TAMANHOS, de EXP a GG, se não existir, coloque o valor 0 (zero).";
                    labBaixa.Text = "ERRO - Informe a contagem de TODOS os TAMANHOS, de EXP a GG, se não existir, coloque o valor 0 (zero).";
                    return;
                }

                int codigo = 0;
                int qtdeEXP = 0;
                int qtdeXP = 0;
                int qtdePP = 0;
                int qtdeP = 0;
                int qtdeM = 0;
                int qtdeG = 0;
                int qtdeGG = 0;
                List<CONT_LOJA_60MAI> loja60MaisLista = new List<CONT_LOJA_60MAI>();
                foreach (GridViewRow r in gvProdutoEstoque.Rows)
                {
                    TextBox _txtGradeEXP = r.FindControl("txtGradeEXP") as TextBox;
                    TextBox _txtGradeXP = r.FindControl("txtGradeXP") as TextBox;
                    TextBox _txtGradePP = r.FindControl("txtGradePP") as TextBox;
                    TextBox _txtGradeP = r.FindControl("txtGradeP") as TextBox;
                    TextBox _txtGradeM = r.FindControl("txtGradeM") as TextBox;
                    TextBox _txtGradeG = r.FindControl("txtGradeG") as TextBox;
                    TextBox _txtGradeGG = r.FindControl("txtGradeGG") as TextBox;

                    codigo = Convert.ToInt32(gvProdutoEstoque.DataKeys[r.RowIndex].Value);

                    qtdeEXP = Convert.ToInt32(_txtGradeEXP.Text);
                    qtdeXP = Convert.ToInt32(_txtGradeXP.Text);
                    qtdePP = Convert.ToInt32(_txtGradePP.Text);
                    qtdeP = Convert.ToInt32(_txtGradeP.Text);
                    qtdeM = Convert.ToInt32(_txtGradeM.Text);
                    qtdeG = Convert.ToInt32(_txtGradeG.Text);
                    qtdeGG = Convert.ToInt32(_txtGradeGG.Text);

                    var l60mais = contController.ObterLoja60Mais(codigo);

                    /*int qtdEstoqueRet = 0;
                    var estProduto = contController.ObterEstoqueProdutoPorLoja(l5mais.FILIAL, l5mais.PRODUTO, null);
                    if (estProduto != null)
                        qtdEstoqueRet = Convert.ToInt32(estProduto.ESTOQUE);
                    */
                    int qtdTransito = 0;
                    var traProduto = contController.ObterTransitoProdutoPorLoja(l60mais.FILIAL, l60mais.PRODUTO, null);
                    if (traProduto != null)
                        qtdTransito = Convert.ToInt32(traProduto.QTDE_TRANSITO);

                    l60mais.ESTOQUE_LOJA_EXP = qtdeEXP;
                    l60mais.ESTOQUE_LOJA_XP = qtdeXP;
                    l60mais.ESTOQUE_LOJA_PP = qtdePP;
                    l60mais.ESTOQUE_LOJA_P = qtdeP;
                    l60mais.ESTOQUE_LOJA_M = qtdeM;
                    l60mais.ESTOQUE_LOJA_G = qtdeG;
                    l60mais.ESTOQUE_LOJA_GG = qtdeGG;

                    l60mais.DATA_ESTOQUE_LOJA = DateTime.Now;
                    l60mais.RESPONSAVEL = txtResponsavel.Text.Trim().ToUpper();

                    /*l5mais.QTDE_ESTOQUE_RET = qtdEstoqueRet;
                    l5mais.DATA_ESTOQUE_RET = DateTime.Now;*/

                    l60mais.QTDE_TRANSITO = qtdTransito;

                    contController.AtualizarLoja60Mais(l60mais);

                    loja60MaisLista.Add(l60mais);

                }

                //recarregar grid
                CarregarProduto60Mais(ddlFilial.SelectedValue);
                txtResponsavel.Text = "";

                if (Constante.enviarEmail)
                    EnviarEmail(loja60MaisLista);

                labErro.Text = "Produtos enviados com sucesso.";

            }
            catch (Exception ex)
            {
                labBaixa.Text = ex.Message;
            }

        }
        #endregion

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = baseController.BuscaFiliais(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").ToList();

                var filialDePara = baseController.BuscaFilialDePara().ToList();
                lstFilial = lstFilial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                        btBuscar_Click(null, null);
                    }
                }
            }
        }
        #endregion

        #region "EMAIL"
        private void EnviarEmail(List<CONT_LOJA_60MAI> loja60MaisLista)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Contagem de Estoque - 5 Peças - " + loja60MaisLista[0].FILIAL;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(loja60MaisLista);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(11, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(List<CONT_LOJA_60MAI> loja60MaisLista)
        {

            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Etiqueta de Composicao</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divEtiqueta' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:left;'>");
            sb.Append("                        <h3>Contagem de Estoque " + " - " + loja60MaisLista[0].FILIAL.Trim() + "</h3>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 600pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr>");
            sb.Append("                                <td style='width: 130px;'>");
            sb.Append("                                    Responsável:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='width: 300px;'>");
            sb.Append("                                    " + loja60MaisLista[0].RESPONSAVEL);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td style='width: 130px;'>");
            sb.Append("                                    Data:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='width: 300px;'>");
            sb.Append("                                    " + Convert.ToDateTime(loja60MaisLista[0].DATA_ESTOQUE_LOJA).ToString("dd/MM/yyyy HH:mm"));
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                             <tr>");
            sb.Append("                                 <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
            sb.Append("                             <tr>");
            sb.Append("                                 <td colspan='3'>");
            sb.Append("                                     <table cellpadding='0' cellspacing='0' style='width: 100%; padding: 0px; color: black;");
            sb.Append("                                         font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
            sb.Append("                                         border: 1px solid #ccc;'>");
            sb.Append("                                         <tr style='background-color: #ccc;'>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 Produto");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style=''>");
            sb.Append("                                                 Nome");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style=''>");
            sb.Append("                                                 Cor");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 EXP");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 XP");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 PP");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 P");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 M");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 G");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 GG");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 Total");
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");

            foreach (var l60 in loja60MaisLista)
            {
                sb.Append("                                         <tr>");
                sb.Append("                                             <td style='border: 1px solid #ccc; text-align: center; width: 170px;'>");
                sb.Append("                                                 " + l60.PRODUTO);
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='border: 1px solid #ccc; width: 200px;'>");
                sb.Append("                                                  " + baseController.ObterProdutoLinx(l60.PRODUTO).DESC_PRODUTO);
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='border: 1px solid #ccc; width: 200px;'>");
                sb.Append("                                                  " + prodController.ObterCoresBasicas(l60.COR_PRODUTO).DESC_COR);
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='border: 1px solid #ccc; text-align: center; width: 100px;'>");
                sb.Append("                                                  " + l60.ESTOQUE_LOJA_EXP);
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='border: 1px solid #ccc; text-align: center; width: 100px;'>");
                sb.Append("                                                  " + l60.ESTOQUE_LOJA_XP);
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='border: 1px solid #ccc; text-align: center; width: 100px;'>");
                sb.Append("                                                  " + l60.ESTOQUE_LOJA_PP);
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='border: 1px solid #ccc; text-align: center; width: 100px;'>");
                sb.Append("                                                  " + l60.ESTOQUE_LOJA_P);
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='border: 1px solid #ccc; text-align: center; width: 100px;'>");
                sb.Append("                                                  " + l60.ESTOQUE_LOJA_M);
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='border: 1px solid #ccc; text-align: center; width: 100px;'>");
                sb.Append("                                                  " + l60.ESTOQUE_LOJA_G);
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='border: 1px solid #ccc; text-align: center; width: 100px;'>");
                sb.Append("                                                  " + l60.ESTOQUE_LOJA_GG);
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='border: 1px solid #ccc; text-align: center; width: 130px;'>");
                sb.Append("                                                  " + (l60.ESTOQUE_LOJA_EXP + l60.ESTOQUE_LOJA_XP + l60.ESTOQUE_LOJA_PP + l60.ESTOQUE_LOJA_P + l60.ESTOQUE_LOJA_M + l60.ESTOQUE_LOJA_G + l60.ESTOQUE_LOJA_GG));
                sb.Append("                                             </td>");
                sb.Append("                                         </tr>");
            }
            sb.Append("                                         <tr style='background-color: #ccc;'>");
            sb.Append("                                             <td colspan='11' style='text-align: center;'>");
            sb.Append("                                                 &nbsp;");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style=''>");
            sb.Append("                                                 &nbsp;");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 &nbsp;");
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");
            sb.Append("                                     </table>");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
            sb.Append("                             <tr>");
            sb.Append("                                 <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + (new BaseController().BuscaUsuario(codigoUsuario).NOME_USUARIO.ToUpper()) + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion
    }
}
