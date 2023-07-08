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
    public partial class VerificarNotaDefeito : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListFilial();
        }

        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> filiais = baseController.BuscaFiliais(usuario);

                if (filiais != null)
                {
                    ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                    ddlFilial.DataBind();
                }
            }
        }        

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void btNotaRetirada_Click(object sender, EventArgs e)
        {
            GridViewNotaRetirada.DataSource = baseController.BuscaNotasRetiradaFechada(Convert.ToInt32(ddlFilial.SelectedValue));
            GridViewNotaRetirada.DataBind();
        }

        protected void btAlteraItem_Click(object sender, EventArgs e)
        {
            Button btAlteraItem = sender as Button;

            if (btAlteraItem != null)
                Response.Redirect(string.Format("AlteraItemNotaDefeito.aspx?CodigoNotaRetirada={0}", btAlteraItem.CommandArgument));
        }

        protected void btAlteraNota_Click(object sender, EventArgs e)
        {
            Button btAlteraNota = sender as Button;

            if (btAlteraNota != null)
                Response.Redirect(string.Format("AlteraNotaDefeito.aspx?CodigoNotaRetirada={0}", btAlteraNota.CommandArgument));
        }

        protected void btAtualizaItens_Click(object sender, EventArgs e)
        {
            Button btAtualizaItens = sender as Button;

            if (btAtualizaItens != null)
                Response.Redirect(string.Format("AtualizaDefeitoDestino.aspx?CodigoNotaRetirada={0}", btAtualizaItens.CommandArgument));
        }

        protected void GridViewNotaRetirada_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewNotaRetirada.Rows)
            {
                if (item.Cells[1].Text == null)
                {
                    item.Cells[0].BackColor = System.Drawing.Color.Red;
                    item.Cells[1].BackColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void GridViewNotaRetirada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_RETIRADA notaRetirada = e.Row.DataItem as NOTA_RETIRADA;

            if (notaRetirada != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                    literalFilial.Text = baseController.BuscaFilialCodigoInt(notaRetirada.CODIGO_FILIAL).FILIAL;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btAlteraItem = e.Row.FindControl("btAlteraItem") as Button;

                if (btAlteraItem != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        NOTA_RETIRADA itemNotaRetirada = e.Row.DataItem as NOTA_RETIRADA;

                        btAlteraItem.CommandArgument = itemNotaRetirada.CODIGO_NOTA_RETIRADA.ToString();
                    }
                }

                Button btAlteraNota = e.Row.FindControl("btAlteraNota") as Button;

                if (btAlteraNota != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        NOTA_RETIRADA itemNotaRetirada = e.Row.DataItem as NOTA_RETIRADA;

                        btAlteraNota.CommandArgument = itemNotaRetirada.CODIGO_NOTA_RETIRADA.ToString();
                    }
                }

                Button btAtualizaItens = e.Row.FindControl("btAtualizaItens") as Button;

                if (btAtualizaItens != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        NOTA_RETIRADA itemNotaRetirada = e.Row.DataItem as NOTA_RETIRADA;

                        btAtualizaItens.CommandArgument = itemNotaRetirada.CODIGO_NOTA_RETIRADA.ToString();
                    }
                }
            }
        }
    }
}
