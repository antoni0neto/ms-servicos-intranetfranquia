using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class CadastroPackGrade : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        PerfilController perfilController = new PerfilController();
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListGrupo();
                CarregaDropDownListGriffe();
                CarregaGridViewPackGrade();
            }
        }

        protected void ddlGrupo_DataBound(object sender, EventArgs e)
        {
            ddlGrupo.Items.Add(new ListItem("Selecione", "0"));
            ddlGrupo.SelectedValue = "0";
        }

        private void CarregaDropDownListGrupo()
        {
            ddlGrupo.DataSource = baseController.BuscaGrupos();
            ddlGrupo.DataBind();
        }

        protected void ddlGriffe_DataBound(object sender, EventArgs e)
        {
            ddlGriffe.Items.Add(new ListItem("Selecione", "0"));
            ddlGriffe.SelectedValue = "0";
        }

        private void CarregaDropDownListGriffe()
        {
            ddlGriffe.DataSource = baseController.BuscaGriffe();
            ddlGriffe.DataBind();
        }

        private void CarregaPackGrade(int codigoPackGrade)
        {
            IMPORTACAO_PACK_GRADE packGrade = usuarioController.BuscaPorCodigoPackGrade(codigoPackGrade);

            if (packGrade != null)
            {
                ddlGrupo.SelectedValue = packGrade.CODIGO_GRUPO.ToString();

                txtDescricaoTipo.Text = packGrade.DESCRICAO_TIPO;

                ddlGriffe.SelectedValue = packGrade.CODIGO_GRIFFE.ToString();
                
                txtQtdeXpPackA.Text = packGrade.QTDE_XP_PACK_A.ToString();
                txtQtdePpPackA.Text = packGrade.QTDE_PP_PACK_A.ToString();
                txtQtdePqPackA.Text = packGrade.QTDE_PQ_PACK_A.ToString();
                txtQtdeMdPackA.Text = packGrade.QTDE_MD_PACK_A.ToString();
                txtQtdeGdPackA.Text = packGrade.QTDE_GD_PACK_A.ToString();
                txtQtdeGgPackA.Text = packGrade.QTDE_GG_PACK_A.ToString();
                txtQtdeXpPackB.Text = packGrade.QTDE_XP_PACK_B.ToString();
                txtQtdePpPackB.Text = packGrade.QTDE_PP_PACK_B.ToString();
                txtQtdePqPackB.Text = packGrade.QTDE_PQ_PACK_B.ToString();
                txtQtdeMdPackB.Text = packGrade.QTDE_MD_PACK_B.ToString();
                txtQtdeGdPackB.Text = packGrade.QTDE_GD_PACK_B.ToString();
                txtQtdeGgPackB.Text = packGrade.QTDE_GG_PACK_B.ToString();
                txtQtdeXpPackC.Text = packGrade.QTDE_XP_PACK_C.ToString();
                txtQtdePpPackC.Text = packGrade.QTDE_PP_PACK_C.ToString();
                txtQtdePqPackC.Text = packGrade.QTDE_PQ_PACK_C.ToString();
                txtQtdeMdPackC.Text = packGrade.QTDE_MD_PACK_C.ToString();
                txtQtdeGdPackC.Text = packGrade.QTDE_GD_PACK_C.ToString();
                txtQtdeGgPackC.Text = packGrade.QTDE_GG_PACK_C.ToString();
                
                HiddenFieldCodigoPackGrade.Value = packGrade.CODIGO_PACK_GRADE.ToString();
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            if (ddlGrupo.SelectedValue.ToString().Equals("0") ||
                ddlGrupo.SelectedValue.ToString().Equals("") ||
                ddlGriffe.SelectedValue.ToString().Equals("0") ||
                ddlGriffe.SelectedValue.ToString().Equals(""))
                return;

            try
            {
                IMPORTACAO_PACK_GRADE packGrade;

                if (Convert.ToInt32(HiddenFieldCodigoPackGrade.Value) > 0)
                    packGrade = usuarioController.BuscaPorCodigoPackGrade(Convert.ToInt32(HiddenFieldCodigoPackGrade.Value));
                else
                {
                    IMPORTACAO_PACK_GRADE packGradeOld = usuarioController.BuscaPackGrade(ddlGrupo.SelectedValue, txtDescricaoTipo.Text, Convert.ToInt32(ddlGriffe.SelectedValue));

                    if (packGradeOld != null)
                        throw new Exception("PackGrade já existe!");

                    packGrade = new IMPORTACAO_PACK_GRADE();
                }

                packGrade.CODIGO_GRUPO = ddlGrupo.SelectedValue;
                packGrade.DESCRICAO_TIPO = txtDescricaoTipo.Text;
                packGrade.CODIGO_GRIFFE = Convert.ToInt32(ddlGriffe.SelectedValue);

                if (txtQtdeXpPackA != null)
                {
                    if (!txtQtdeXpPackA.Text.Equals(""))
                        packGrade.QTDE_XP_PACK_A = Convert.ToInt32(txtQtdeXpPackA.Text);
                }
                if (txtQtdePpPackA != null)
                {
                    if (!txtQtdePpPackA.Text.Equals(""))
                        packGrade.QTDE_PP_PACK_A = Convert.ToInt32(txtQtdePpPackA.Text);
                }
                if (txtQtdePqPackA != null)
                {
                    if (!txtQtdePqPackA.Text.Equals(""))
                        packGrade.QTDE_PQ_PACK_A = Convert.ToInt32(txtQtdePqPackA.Text);
                }
                if (txtQtdeMdPackA != null)
                {
                    if (!txtQtdeMdPackA.Text.Equals(""))
                        packGrade.QTDE_MD_PACK_A = Convert.ToInt32(txtQtdeMdPackA.Text);
                }
                if (txtQtdeGdPackA != null)
                {
                    if (!txtQtdeGdPackA.Text.Equals(""))
                        packGrade.QTDE_GD_PACK_A = Convert.ToInt32(txtQtdeGdPackA.Text);
                }
                if (txtQtdeGgPackA != null)
                {
                    if (!txtQtdeGgPackA.Text.Equals(""))
                        packGrade.QTDE_GG_PACK_A = Convert.ToInt32(txtQtdeGgPackA.Text);
                }

                if (txtQtdeXpPackB != null)
                {
                    if (!txtQtdeXpPackB.Text.Equals(""))
                        packGrade.QTDE_XP_PACK_B = Convert.ToInt32(txtQtdeXpPackB.Text);
                }
                if (txtQtdePpPackB != null)
                {
                    if (!txtQtdePpPackB.Text.Equals(""))
                        packGrade.QTDE_PP_PACK_B = Convert.ToInt32(txtQtdePpPackB.Text);
                }
                if (txtQtdePqPackB != null)
                {
                    if (!txtQtdePqPackB.Text.Equals(""))
                        packGrade.QTDE_PQ_PACK_B = Convert.ToInt32(txtQtdePqPackB.Text);
                }
                if (txtQtdeMdPackB != null)
                {
                    if (!txtQtdeMdPackB.Text.Equals(""))
                        packGrade.QTDE_MD_PACK_B = Convert.ToInt32(txtQtdeMdPackB.Text);
                }
                if (txtQtdeGdPackB != null)
                {
                    if (!txtQtdeGdPackB.Text.Equals(""))
                        packGrade.QTDE_GD_PACK_B = Convert.ToInt32(txtQtdeGdPackB.Text);
                }
                if (txtQtdeGgPackB != null)
                {
                    if (!txtQtdeGgPackB.Text.Equals(""))
                        packGrade.QTDE_GG_PACK_B = Convert.ToInt32(txtQtdeGgPackB.Text);
                }

                if (txtQtdeXpPackC != null)
                {
                    if (!txtQtdeXpPackC.Text.Equals(""))
                        packGrade.QTDE_XP_PACK_C = Convert.ToInt32(txtQtdeXpPackC.Text);
                }
                if (txtQtdePpPackC != null)
                {
                    if (!txtQtdePpPackC.Text.Equals(""))
                        packGrade.QTDE_PP_PACK_C = Convert.ToInt32(txtQtdePpPackC.Text);
                }
                if (txtQtdePqPackC != null)
                {
                    if (!txtQtdePqPackC.Text.Equals(""))
                        packGrade.QTDE_PQ_PACK_C = Convert.ToInt32(txtQtdePqPackC.Text);
                }
                if (txtQtdeMdPackC != null)
                {
                    if (!txtQtdeMdPackC.Text.Equals(""))
                        packGrade.QTDE_MD_PACK_C = Convert.ToInt32(txtQtdeMdPackC.Text);
                }
                if (txtQtdeGdPackC != null)
                {
                    if (!txtQtdeGdPackC.Text.Equals(""))
                        packGrade.QTDE_GD_PACK_C = Convert.ToInt32(txtQtdeGdPackC.Text);
                }
                if (txtQtdeGgPackC != null)
                {
                    if (!txtQtdeGgPackC.Text.Equals(""))
                        packGrade.QTDE_GG_PACK_C = Convert.ToInt32(txtQtdeGgPackC.Text);
                }

                if (Convert.ToInt32(HiddenFieldCodigoPackGrade.Value) > 0)
                    usuarioController.AtualizaPackGrade(packGrade);
                else
                    usuarioController.InserePackGrade(packGrade);

                LabelFeedBack.Text = "Gravado com sucesso!";

                CarregaGridViewPackGrade();

                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewPackGrade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_PACK_GRADE packGrade = e.Row.DataItem as IMPORTACAO_PACK_GRADE;

            if (packGrade != null)
            {
                Literal literalGrupo = e.Row.FindControl("LiteralGrupo") as Literal;

                if (literalGrupo != null)
                {
                    string codigo = "";

                    if (packGrade.CODIGO_GRUPO.ToString().Length < 2)
                        codigo = "0" + packGrade.CODIGO_GRUPO.ToString();
                    else
                        codigo = packGrade.CODIGO_GRUPO.ToString();

                    PRODUTOS_GRUPO grupo = baseController.BuscaGrupoProdutoPeloCodigo(codigo);

                    if (grupo != null)
                        literalGrupo.Text = grupo.GRUPO_PRODUTO;
                }

                Literal literalGriffe = e.Row.FindControl("LiteralGriffe") as Literal;

                if (literalGriffe != null)
                    literalGriffe.Text = baseController.BuscaGriffe(packGrade.CODIGO_GRIFFE).DESCRICAO;

                Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;

                if (buttonEditar != null)
                    buttonEditar.CommandArgument = packGrade.CODIGO_PACK_GRADE.ToString();

                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;

                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = packGrade.CODIGO_PACK_GRADE.ToString();
            }
        }

        private void CarregaGridViewPackGrade()
        {
            GridViewPackGrade.DataSource = usuarioController.BuscaPackGrade();
            GridViewPackGrade.DataBind();
        }

        protected void ButtonExcluirPackGrade_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                IMPORTACAO_PACK_GRADE packGrade = usuarioController.BuscaPorCodigoPackGrade(Convert.ToInt32(buttonExcluir.CommandArgument));

                if (packGrade != null)
                {
                    usuarioController.ExcluiPackGrade(packGrade);

                    CarregaGridViewPackGrade();

                    LimpaTela();
                }
            }
        }

        protected void ButtonEditarPackGrade_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;

            if (buttonEditar != null)
            {
                CarregaPackGrade(Convert.ToInt32(buttonEditar.CommandArgument));
            
                LimpaFeedBack();
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            ddlGrupo.Items.Add(new ListItem("Selecione", "0"));
            ddlGrupo.SelectedValue = "0";

            txtDescricaoTipo.Text = string.Empty;
            
            ddlGriffe.Items.Add(new ListItem("Selecione", "0"));
            ddlGriffe.SelectedValue = "0";

            txtQtdeXpPackA.Text = string.Empty;
            txtQtdePpPackA.Text = string.Empty;
            txtQtdePqPackA.Text = string.Empty;
            txtQtdeMdPackA.Text = string.Empty;
            txtQtdeGdPackA.Text = string.Empty;
            txtQtdeGgPackA.Text = string.Empty;
            txtQtdeXpPackB.Text = string.Empty;
            txtQtdePpPackB.Text = string.Empty;
            txtQtdePqPackB.Text = string.Empty;
            txtQtdeMdPackB.Text = string.Empty;
            txtQtdeGdPackB.Text = string.Empty;
            txtQtdeGgPackB.Text = string.Empty;
            txtQtdeXpPackC.Text = string.Empty;
            txtQtdePpPackC.Text = string.Empty;
            txtQtdePqPackC.Text = string.Empty;
            txtQtdeMdPackC.Text = string.Empty;
            txtQtdeGdPackC.Text = string.Empty;
            txtQtdeGgPackC.Text = string.Empty;

            HiddenFieldCodigoPackGrade.Value = "0";
        }
    }
}