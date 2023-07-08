using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class AlteraQtdeProforma : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListColecao();
                CarregaDropDownListJanela();
                CarregaDropDownListProforma();
            }
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecao();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListJanela()
        {
            ddlJanela.DataSource = baseController.BuscaJanela();
            ddlJanela.DataBind();
        }

        private void CarregaDropDownListProforma()
        {
            ddlProforma.DataSource = baseController.BuscaProformasProvisoria();
            ddlProforma.DataBind();
        }

        private void CarregaGridViewProdutos()
        {
            List<IMPORTACAO_PROFORMA_PRODUTO> listaProformaProduto = baseController.BuscaProformaProduto(Convert.ToInt32(ddlColecao.SelectedValue), Convert.ToInt32(ddlJanela.SelectedValue), Convert.ToInt32(ddlProforma.SelectedValue));

            GridViewProdutos.DataSource = listaProformaProduto;
            GridViewProdutos.DataBind();
            
            if (listaProformaProduto != null && listaProformaProduto.Count() > 0)
            {
                IMPORTACAO_PROFORMA_PRODUTO proformaProduto = listaProformaProduto[0];

                if (proformaProduto != null)
                {
                    if(proformaProduto.CODIGO_PROFORMA == 1 || proformaProduto.CODIGO_PROFORMA == 2)
                        GridViewProdutos.Columns[16].Visible = GridViewProdutos.Columns[17].Visible = GridViewProdutos.Columns[18].Visible = GridViewProdutos.Columns[19].Visible = GridViewProdutos.Columns[20].Visible = false;
                    else
                        GridViewProdutos.Columns[16].Visible = GridViewProdutos.Columns[17].Visible = GridViewProdutos.Columns[18].Visible = GridViewProdutos.Columns[19].Visible = GridViewProdutos.Columns[20].Visible = true;
                }
            }
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlJanela_DataBound(object sender, EventArgs e)
        {
            ddlJanela.Items.Add(new ListItem("Selecione", "0"));
            ddlJanela.SelectedValue = "0";
        }

        protected void ddlProforma_DataBound(object sender, EventArgs e)
        {
            ddlProforma.Items.Add(new ListItem("Selecione", "0"));
            ddlProforma.SelectedValue = "0";
        }

        protected void btBuscarProdutos_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals("") ||
                ddlProforma.SelectedValue.ToString().Equals("0") ||
                ddlProforma.SelectedValue.ToString().Equals("") ||
                ddlJanela.SelectedValue.ToString().Equals("0") ||
                ddlJanela.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewProdutos();

            btGravar.Enabled = true;
        }

        protected void ButtonDeletar_Click(object sender, EventArgs e)
        {
            Button buttonDeletar = sender as Button;
            
            if (buttonDeletar != null)
            {
                IMPORTACAO_PROFORMA_PRODUTO itemProformaProduto = baseController.BuscaProformaProduto(Convert.ToInt32(buttonDeletar.CommandArgument));
                
                if (itemProformaProduto != null)
                {
                    usuarioController.ExcluiProformaProduto(itemProformaProduto);

                    CarregaGridViewProdutos();
                }
            }
        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            int contProduto = 0;

            IMPORTACAO_PROFORMA_PRODUTO proformaProduto = new IMPORTACAO_PROFORMA_PRODUTO();

            int qtTotal;
            int qtXp;
            int qtPp;
            int qtPq;
            int qtMd;
            int qtGd;
            int qtGg;
            string fob;
            string tipoProduto;
            string codigoFornecedor;
            int codigoArmario;
            int codigoPackGrade;
            int codigoPackGroup;

            foreach (GridViewRow item in GridViewProdutos.Rows)
            {
                qtTotal = 0;
                qtXp = 0;
                qtPp = 0;
                qtPq = 0;
                qtMd = 0;
                qtGd = 0;
                qtGg = 0;
                fob = "0";
                tipoProduto = "";
                codigoFornecedor = "";
                codigoArmario = 0;
                codigoPackGrade = 0;
                codigoPackGroup = 0;

                CheckBox cbAlterado = item.FindControl("cbAlterado") as CheckBox;

                if (cbAlterado != null)
                {
                    if (cbAlterado.Checked)
                    {
                        TextBox txtQtdeXp = item.FindControl("txtQtdeXp") as TextBox;

                        if (txtQtdeXp != null)
                        {
                            if (!txtQtdeXp.Text.Equals(""))
                            {
                                qtXp = Convert.ToInt32(txtQtdeXp.Text);
                                qtTotal += qtXp;
                            }
                        }

                        TextBox txtQtdePp = item.FindControl("txtQtdePp") as TextBox;

                        if (txtQtdePp != null)
                        {
                            if (!txtQtdePp.Text.Equals(""))
                            {
                                qtPp = Convert.ToInt32(txtQtdePp.Text);
                                qtTotal += qtPp;
                            }
                        }

                        TextBox txtQtdePq = item.FindControl("txtQtdePq") as TextBox;

                        if (txtQtdePq != null)
                        {
                            if (!txtQtdePq.Text.Equals(""))
                            {
                                qtPq = Convert.ToInt32(txtQtdePq.Text);
                                qtTotal += qtPq;
                            }
                        }

                        TextBox txtQtdeMd = item.FindControl("txtQtdeMd") as TextBox;

                        if (txtQtdeMd != null)
                        {
                            if (!txtQtdeMd.Text.Equals(""))
                            {
                                qtMd = Convert.ToInt32(txtQtdeMd.Text);
                                qtTotal += qtMd;
                            }
                        }

                        TextBox txtQtdeGd = item.FindControl("txtQtdeGd") as TextBox;

                        if (txtQtdeGd != null)
                        {
                            if (!txtQtdeGd.Text.Equals(""))
                            {
                                qtGd = Convert.ToInt32(txtQtdeGd.Text);
                                qtTotal += qtGd;
                            }
                        }

                        TextBox txtQtdeGg = item.FindControl("txtQtdeGg") as TextBox;

                        if (txtQtdeGg != null)
                        {
                            if (!txtQtdeGg.Text.Equals(""))
                            {
                                qtGg = Convert.ToInt32(txtQtdeGg.Text);
                                qtTotal += qtGg;
                            }
                        }

                        TextBox txtFob = item.FindControl("txtFob") as TextBox;

                        if (txtFob != null)
                        {
                            if (!txtFob.Text.Equals(""))
                                fob = txtFob.Text;
                        }

                        TextBox txtTipoProduto = item.FindControl("txtTipoProduto") as TextBox;

                        if (txtTipoProduto != null)
                        {
                            if (!txtTipoProduto.Text.Equals(""))
                                tipoProduto = txtTipoProduto.Text;
                        }

                        DropDownList ddlArmario = item.FindControl("ddlArmario") as DropDownList;

                        if (ddlArmario != null)
                        {
                            codigoArmario = Convert.ToInt32(ddlArmario.SelectedValue);

                            TextBox txtCodigoFornecedor = item.FindControl("txtCodigoFornecedor") as TextBox;

                            if (txtCodigoFornecedor != null)
                            {
                                if (!txtCodigoFornecedor.Text.Equals("") & txtCodigoFornecedor.Text.Length < 5)
                                    codigoFornecedor = ddlArmario.SelectedItem.ToString().Substring(0, 1) + ddlJanela.SelectedItem.ToString().Substring(0, 1) + "W" + txtCodigoFornecedor.Text;
                                if (txtCodigoFornecedor.Text.Length > 4)
                                    codigoFornecedor = txtCodigoFornecedor.Text;
                            }

                        }

                        DropDownList ddlPackGrade = item.FindControl("ddlPackGrade") as DropDownList;

                        if (ddlPackGrade != null & !ddlPackGrade.SelectedValue.Equals(""))
                            codigoPackGrade = Convert.ToInt32(ddlPackGrade.SelectedValue);

                        DropDownList ddlPackGroup = item.FindControl("ddlPackGroup") as DropDownList;

                        if (ddlPackGroup != null & !ddlPackGroup.SelectedValue.Equals(""))
                            codigoPackGroup = Convert.ToInt32(ddlPackGroup.SelectedValue);

                        if (codigoPackGrade > 0 & codigoPackGroup > 0)
                        {
                            IMPORTACAO_PACK_GRADE packGrade = baseController.BuscaPackGrade(Convert.ToInt32(ddlPackGrade.SelectedValue));
                            IMPORTACAO_PACK_GROUP packGroup = baseController.BuscaPackGroup(Convert.ToInt32(ddlPackGroup.SelectedValue));

                            if (packGrade != null & packGroup != null)
                            {
                                {
                                    qtXp = packGroup.QTDE_PACK_A * packGrade.QTDE_XP_PACK_A + packGroup.QTDE_PACK_B * packGrade.QTDE_XP_PACK_B + packGroup.QTDE_PACK_C * packGrade.QTDE_XP_PACK_C;
                                    qtPp = packGroup.QTDE_PACK_A * packGrade.QTDE_PP_PACK_A + packGroup.QTDE_PACK_B * packGrade.QTDE_PP_PACK_B + packGroup.QTDE_PACK_C * packGrade.QTDE_PP_PACK_C;

                                    qtPq = packGroup.QTDE_PACK_A * packGrade.QTDE_PQ_PACK_A + packGroup.QTDE_PACK_B * packGrade.QTDE_PQ_PACK_B + packGroup.QTDE_PACK_C * packGrade.QTDE_PQ_PACK_C;
                                    qtMd = packGroup.QTDE_PACK_A * packGrade.QTDE_MD_PACK_A + packGroup.QTDE_PACK_B * packGrade.QTDE_MD_PACK_B + packGroup.QTDE_PACK_C * packGrade.QTDE_MD_PACK_C;

                                    qtGd = packGroup.QTDE_PACK_A * packGrade.QTDE_GD_PACK_A + packGroup.QTDE_PACK_B * packGrade.QTDE_GD_PACK_B + packGroup.QTDE_PACK_C * packGrade.QTDE_GD_PACK_C;
                                    qtGg = packGroup.QTDE_PACK_A * packGrade.QTDE_GG_PACK_A + packGroup.QTDE_PACK_B * packGrade.QTDE_GG_PACK_B + packGroup.QTDE_PACK_C * packGrade.QTDE_GG_PACK_C;

                                    qtTotal = qtXp + qtPp + qtPq + qtMd + qtGd + qtGg;
                                }
                            }
                        }
                        
                        //baseController.AtualizaProformaProduto(item.Cells[0].Text, txtQtdeTotal.Text); visible = true;
                        baseController.AtualizaProformaProduto(Convert.ToInt32(GridViewProdutos.DataKeys[item.RowIndex].Value), qtTotal, qtXp, qtPp, qtPq, qtMd, qtGd, qtGg, fob, codigoFornecedor, codigoArmario, codigoPackGrade, codigoPackGroup, tipoProduto); // visible = false

                        contProduto++;

                        CarregaGridViewProdutos();
                    }
                }
            }

            LabelFeedBack.Text = "Foram gravados " + contProduto + " Produtos !!!";
        }

        protected void GridViewProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_PROFORMA_PRODUTO proformaProduto = e.Row.DataItem as IMPORTACAO_PROFORMA_PRODUTO;

            if (proformaProduto != null)
            {
                Literal literalDescricao = e.Row.FindControl("LiteralDescricao") as Literal;

                if (literalDescricao != null)
                    literalDescricao.Text = baseController.BuscaProduto(proformaProduto.CODIGO_PRODUTO.ToString()).DESC_PRODUTO;

                Literal literalCor = e.Row.FindControl("LiteralCor") as Literal;

                if (literalCor != null)
                {
                    Sp_Busca_Produto_CorResult produtoCor = baseController.BuscaProdutoCor(proformaProduto.CODIGO_PRODUTO.ToString(), proformaProduto.CODIGO_PRODUTO_COR.ToString());

                    if (produtoCor != null)
                        literalCor.Text = produtoCor.DESC_COR_PRODUTO;
                }

                Literal literalFornecedor = e.Row.FindControl("LiteralFornecedor") as Literal;

                if (literalFornecedor != null)
                    literalFornecedor.Text = baseController.BuscaFornecedor(proformaProduto.CODIGO_FORNECEDOR).DESCRICAO;

                TextBox txtQtdeTotal = e.Row.FindControl("txtQtdeTotal") as TextBox;

                if (txtQtdeTotal != null)
                    txtQtdeTotal.Text = proformaProduto.QTDE_TOTAL.ToString();

                TextBox txtQtdeXp = e.Row.FindControl("txtQtdeXp") as TextBox;

                if (txtQtdeXp != null)
                    txtQtdeXp.Text = proformaProduto.QTDE_XP.ToString();

                TextBox txtQtdePp = e.Row.FindControl("txtQtdePp") as TextBox;

                if (txtQtdePp != null)
                    txtQtdePp.Text = proformaProduto.QTDE_PP.ToString();

                TextBox txtQtdePq = e.Row.FindControl("txtQtdePq") as TextBox;

                if (txtQtdePq != null)
                    txtQtdePq.Text = proformaProduto.QTDE_PQ.ToString();

                TextBox txtQtdeMd = e.Row.FindControl("txtQtdeMd") as TextBox;

                if (txtQtdeMd != null)
                    txtQtdeMd.Text = proformaProduto.QTDE_MD.ToString();

                TextBox txtQtdeGd = e.Row.FindControl("txtQtdeGd") as TextBox;

                if (txtQtdeGd != null)
                    txtQtdeGd.Text = proformaProduto.QTDE_GD.ToString();

                TextBox txtQtdeGg = e.Row.FindControl("txtQtdeGg") as TextBox;

                if (txtQtdeGg != null)
                    txtQtdeGg.Text = proformaProduto.QTDE_GG.ToString();

                TextBox txtFob = e.Row.FindControl("txtFob") as TextBox;

                if (txtFob != null)
                    txtFob.Text = proformaProduto.FOB.ToString();

                TextBox txtTipoProduto = e.Row.FindControl("txtTipoProduto") as TextBox;

                if (txtTipoProduto != null)
                    txtTipoProduto.Text = proformaProduto.TIPO_PRODUTO;

                TextBox txtCodigoFornecedor = e.Row.FindControl("txtCodigoFornecedor") as TextBox;

                if (txtCodigoFornecedor != null)
                    txtCodigoFornecedor.Text = proformaProduto.DESCRICAO_FORNECEDOR;

                DropDownList ddlArmario = e.Row.FindControl("ddlArmario") as DropDownList;

                if (ddlArmario != null)
                {
                    ddlArmario.DataSource = baseController.BuscaArmario();
                    ddlArmario.DataBind();
                    ddlArmario.Items.Add(new ListItem("Selecione", "0"));
                    ddlArmario.SelectedValue = proformaProduto.CODIGO_ARMARIO.ToString();
                }

                if (proformaProduto.CODIGO_PROFORMA == 1 ||
                    proformaProduto.CODIGO_PROFORMA == 2)
                {
                }
                else
                {
                    DropDownList ddlPackGrade = e.Row.FindControl("ddlPackGrade") as DropDownList;

                    if (ddlPackGrade != null)
                    {
                        ddlPackGrade.DataSource = baseController.BuscaPackGrade(proformaProduto.GRUPO_PRODUTO, proformaProduto.CODIGO_GRIFFE);
                        ddlPackGrade.DataBind();
                        ddlPackGrade.Items.Add(new ListItem("Selecione", "0"));
                        ddlPackGrade.SelectedValue = proformaProduto.CODIGO_PACK_GRADE.ToString();
                    }

                    DropDownList ddlPackGroup = e.Row.FindControl("ddlPackGroup") as DropDownList;

                    if (ddlPackGroup != null)
                    {
                        ddlPackGroup.Items.Clear();

                        if (txtTipoProduto.Text.Equals(""))
                            ddlPackGroup.DataSource = baseController.BuscaPackGroup(proformaProduto.GRUPO_PRODUTO, proformaProduto.CODIGO_GRIFFE);
                        else
                            ddlPackGroup.DataSource = baseController.BuscaPackGroupTipo(proformaProduto.GRUPO_PRODUTO, proformaProduto.CODIGO_GRIFFE, proformaProduto.TIPO_PRODUTO);

                        ddlPackGroup.DataBind();
                        ddlPackGroup.Items.Add(new ListItem("Selecione", "0"));
                        ddlPackGroup.SelectedValue = proformaProduto.CODIGO_PACK_GROUP.ToString();
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button buttonDeletar = e.Row.FindControl("ButtonDeletar") as Button;

                    if (buttonDeletar != null)
                    {
                        if (e.Row.DataItem != null)
                        {
                            IMPORTACAO_PROFORMA_PRODUTO itemProformaProduto = e.Row.DataItem as IMPORTACAO_PROFORMA_PRODUTO;

                            buttonDeletar.CommandArgument = itemProformaProduto.CODIGO_PROFORMA_PRODUTO.ToString();
                        }
                    }
                }
            }
        }
    }
}
