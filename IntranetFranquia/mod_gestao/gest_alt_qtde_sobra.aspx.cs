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
    public partial class gest_alt_qtde_sobra : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListCategoria();
                CarregaDropDownListColecao();
                CarregaDropDownListGriffe();
                cbMarcarTodos.Enabled = false;
            }
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListCategoria()
        {
            ddlCategoria.DataSource = baseController.BuscaCategorias();
            ddlCategoria.DataBind();
        }

        private void CarregaDropDownListGriffe()
        {
            ddlGriffe.DataSource = baseController.BuscaGriffes();
            ddlGriffe.DataBind();
        }

        private void CarregaGridViewSobras()
        {
            List<ESTOQUE_SOBRA> _estoque = baseController.BuscaEstoqueSobra(ddlCategoria.SelectedValue, ddlColecao.SelectedValue, ddlGriffe.SelectedItem.ToString());

            cbMarcarTodos.Enabled = false;
            btGravar.Enabled = false;
            LabelFeedBack.Text = "";
            if (_estoque != null)
            {
                GridViewSobras.DataSource = baseController.BuscaEstoqueSobra(ddlCategoria.SelectedValue, ddlColecao.SelectedValue, ddlGriffe.SelectedItem.ToString());
                GridViewSobras.DataBind();

                if (_estoque.Count > 0)
                {
                    cbMarcarTodos.Enabled = true;
                    btGravar.Enabled = true;
                }
            }
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlCategoria_DataBound(object sender, EventArgs e)
        {
            ddlCategoria.Items.Add(new ListItem("Selecione", "0"));
            ddlCategoria.SelectedValue = "0";
        }
        protected void ddlGriffe_DataBound(object sender, EventArgs e)
        {
            ddlGriffe.Items.Add(new ListItem("Selecione", "0"));
            ddlGriffe.SelectedValue = "0";
        }

        protected void btBuscarSobra_Click(object sender, EventArgs e)
        {
            if (ddlCategoria.SelectedValue.ToString().Equals("0") ||
                ddlCategoria.SelectedValue.ToString().Equals("") ||
                ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals("") ||
                ddlGriffe.SelectedValue.ToString().Equals("0") ||
                ddlGriffe.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewSobras();

        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            int contProduto = 0;

            ESTOQUE_SOBRA estoqueSobra = new ESTOQUE_SOBRA();

            int qtde;
            decimal valor;

            foreach (GridViewRow item in GridViewSobras.Rows)
            {
                qtde = 0;
                valor = 0;

                CheckBox cbAlterado = item.FindControl("cbAlterado") as CheckBox;

                if (cbAlterado != null)
                {
                    if (cbAlterado.Checked)
                    {
                        TextBox txtQtdeVirado = item.FindControl("txtQtde") as TextBox;
                        TextBox txtValorVirado = item.FindControl("txtValor") as TextBox;

                        if (txtQtdeVirado != null)
                        {
                            if (!txtQtdeVirado.Text.Equals(""))
                                qtde = Convert.ToInt32(txtQtdeVirado.Text);
                        }

                        if (txtValorVirado != null)
                        {
                            if (!txtValorVirado.Text.Equals(""))
                            {
                                valor = Convert.ToDecimal(txtValorVirado.Text);
                            }
                        }

                        baseController.AtualizaEstoqueSobra(GridViewSobras.DataKeys[item.RowIndex].Value.ToString(), qtde.ToString(), valor);

                        contProduto++;

                        CarregaGridViewSobras();
                    }
                }
            }

            if (contProduto == 1)
                LabelFeedBack.Text = "Foi atualizado " + contProduto.ToString() + " produto!";
            else if (contProduto <= 0)
                LabelFeedBack.Text = "Nenhum produto foi atualizado";
            else
                LabelFeedBack.Text = "Foram atualizados " + contProduto.ToString() + " produtos!";

            cbMarcarTodos.Checked = false;
            labMarcarTodos.Text = "Marcar Todos";

        }

        protected void GridViewSobras_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void cbMarcarTodos_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb.Checked)
                labMarcarTodos.Text = "Desmarcar Todos";
            else
                labMarcarTodos.Text = "Marcar Todos";

            //Des/Marcar todos
            foreach (GridViewRow item in GridViewSobras.Rows)
            {
                CheckBox cbAlterado = item.FindControl("cbAlterado") as CheckBox;
                cbAlterado.Checked = cb.Checked;
            }
        }
    }
}
