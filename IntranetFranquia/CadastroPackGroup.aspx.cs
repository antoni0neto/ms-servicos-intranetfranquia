using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class CadastroPackGroup : System.Web.UI.Page
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
                CarregaGridViewPackGroup();
                CarregaDropDownListGriffe();
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

        private void CarregaPackGroup(int codigoPackGroup)
        {
            IMPORTACAO_PACK_GROUP packGroup = usuarioController.BuscaPorCodigoPackGroup(codigoPackGroup);

            if (packGroup != null)
            {
                ddlGrupo.SelectedValue = packGroup.CODIGO_GRUPO.ToString();

                txtDescricaoTipo.Text = packGroup.DESCRICAO_TIPO;
                
                ddlGriffe.SelectedValue = packGroup.CODIGO_GRIFFE.ToString();

                txtQtdeTotal.Text = packGroup.QTDE_TOTAL.ToString();
                txtQtdePackA.Text = packGroup.QTDE_PACK_A.ToString();
                txtQtdePackB.Text = packGroup.QTDE_PACK_B.ToString();
                txtQtdePackC.Text = packGroup.QTDE_PACK_C.ToString();
                
                HiddenFieldCodigoPackGroup.Value = packGroup.CODIGO_PACK_GROUP.ToString();
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            if (ddlGrupo.SelectedValue.ToString().Equals("0") ||
                ddlGrupo.SelectedValue.ToString().Equals("") ||
                ddlGriffe.SelectedValue.ToString().Equals("0") ||
                ddlGriffe.SelectedValue.ToString().Equals("") ||
                txtDescricaoTipo.Text.Equals("") ||
                txtQtdeTotal.Text.Equals(""))
                return;

            try
            {
                IMPORTACAO_PACK_GROUP packGroup;

                if (Convert.ToInt32(HiddenFieldCodigoPackGroup.Value) > 0)
                    packGroup = usuarioController.BuscaPorCodigoPackGroup(Convert.ToInt32(HiddenFieldCodigoPackGroup.Value));
                else
                {
                    IMPORTACAO_PACK_GROUP packGroupOld = usuarioController.BuscaPackGroup(ddlGrupo.SelectedValue, 
                                                                                          Convert.ToInt32(ddlGriffe.SelectedValue.ToString()),
                                                                                          txtDescricaoTipo.Text,
                                                                                          Convert.ToInt32(txtQtdeTotal.Text));

                    if (packGroupOld != null)
                        throw new Exception("PackGroup já existe!");

                    packGroup = new IMPORTACAO_PACK_GROUP();
                }

                if (ddlGrupo.SelectedValue != null)
                    packGroup.CODIGO_GRUPO = ddlGrupo.SelectedValue;

                packGroup.DESCRICAO_TIPO = txtDescricaoTipo.Text;

                if (ddlGriffe.SelectedValue != null)
                    packGroup.CODIGO_GRIFFE = Convert.ToInt32(ddlGriffe.SelectedValue);

                if (txtQtdeTotal != null)
                {
                    if (!txtQtdeTotal.Text.Equals(""))
                        packGroup.QTDE_TOTAL = Convert.ToInt32(txtQtdeTotal.Text);
                }

                if (txtQtdePackA != null)
                {
                    if (!txtQtdePackA.Text.Equals(""))
                        packGroup.QTDE_PACK_A = Convert.ToInt32(txtQtdePackA.Text);
                }

                if (txtQtdePackB != null)
                {
                    if (!txtQtdePackB.Text.Equals(""))
                        packGroup.QTDE_PACK_B = Convert.ToInt32(txtQtdePackB.Text);
                }

                if (txtQtdePackC != null)
                {
                    if (!txtQtdePackC.Text.Equals(""))
                        packGroup.QTDE_PACK_C = Convert.ToInt32(txtQtdePackC.Text);
                }

                if (Convert.ToInt32(HiddenFieldCodigoPackGroup.Value) > 0)
                    usuarioController.AtualizaPackGroup(packGroup);
                else
                    usuarioController.InserePackGroup(packGroup);

                LabelFeedBack.Text = "Gravado com sucesso!";

                CarregaGridViewPackGroup();

                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewPackGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_PACK_GROUP packGroup = e.Row.DataItem as IMPORTACAO_PACK_GROUP;

            if (packGroup != null)
            {
                Literal literalGrupo = e.Row.FindControl("LiteralGrupo") as Literal;

                if (literalGrupo != null)
                {
                    string codigo = "";

                    if (packGroup.CODIGO_GRUPO.ToString().Length < 2)
                        codigo = "0" + packGroup.CODIGO_GRUPO.ToString();
                    else
                        codigo = packGroup.CODIGO_GRUPO.ToString();

                    PRODUTOS_GRUPO grupo = baseController.BuscaGrupoProdutoPeloCodigo(codigo);

                    if (grupo != null)
                        literalGrupo.Text = grupo.GRUPO_PRODUTO;
                }

                Literal literalGriffe = e.Row.FindControl("LiteralGriffe") as Literal;

                if (literalGriffe != null)
                    literalGriffe.Text = baseController.BuscaGriffe(packGroup.CODIGO_GRIFFE).DESCRICAO;

                Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;

                if (buttonEditar != null)
                    buttonEditar.CommandArgument = packGroup.CODIGO_PACK_GROUP.ToString();

                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;

                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = packGroup.CODIGO_PACK_GROUP.ToString();
            }
        }

        private void CarregaGridViewPackGroup()
        {
            GridViewPackGroup.DataSource = usuarioController.BuscaPackGroup();
            GridViewPackGroup.DataBind();
        }

        protected void ButtonExcluirPackGroup_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                IMPORTACAO_PACK_GROUP packGroup = usuarioController.BuscaPorCodigoPackGroup(Convert.ToInt32(buttonExcluir.CommandArgument));

                if (packGroup != null)
                {
                    usuarioController.ExcluiPackGroup(packGroup);

                    CarregaGridViewPackGroup();

                    LimpaTela();
                }
            }
        }

        protected void ButtonEditarPackGroup_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;

            if (buttonEditar != null)
            {
                CarregaPackGroup(Convert.ToInt32(buttonEditar.CommandArgument));
            
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

            txtQtdeTotal.Text = string.Empty;
            txtQtdePackA.Text = string.Empty;
            txtQtdePackB.Text = string.Empty;
            txtQtdePackC.Text = string.Empty;

            HiddenFieldCodigoPackGroup.Value = "0";
        }
    }
}