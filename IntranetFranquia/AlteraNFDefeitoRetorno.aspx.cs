using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;

namespace Relatorios
{
    public partial class AlteraNFDefeitoRetorno : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        BaseController baseController = new BaseController();
        int i = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListDefeito();
                CarregaDropDownListDestino();
                CarregaDatasRetorno();
            }
        }

        private void CarregaDatasRetorno()
        {
            List<Sp_Busca_Nf_Datas_RetornoResult> datas = baseController.BuscaDatasRetorno();

            if (datas != null)
            {
                foreach (Sp_Busca_Nf_Datas_RetornoResult item in datas)
                {
                    CalendarDatasDestino.SelectedDates.Add(Convert.ToDateTime(item.data));
                }
            }
        }

        private void CarregaDropDownListDefeito()
        {
            ddlDefeito.DataSource = baseController.BuscaDefeitos();
            ddlDefeito.DataBind();
        }

        private void CarregaDropDownListDestino()
        {
            ddlDestino.DataSource = baseController.BuscaDestinos();
            ddlDestino.DataBind();
        }

        protected void ddlDefeito_DataBound(object sender, EventArgs e)
        {
            ddlDefeito.Items.Add(new ListItem("Selecione", "0"));
            ddlDefeito.SelectedValue = "0";
        }

        protected void ddlDestino_DataBound(object sender, EventArgs e)
        {
            ddlDestino.Items.Add(new ListItem("Selecione", "0"));
            ddlDestino.SelectedValue = "0";
        }

        protected void CalendarData_SelectionChanged(object sender, EventArgs e)
        {
            txtData.Text = CalendarData.SelectedDate.ToString("yyyy-MM-dd");
        }

        protected void btBuscarProdutos_Click(object sender, EventArgs e)
        {
            CarregaGridViewNotaRetiradaItem();

            btGravar.Enabled = true;
        }

        private void CarregaGridViewNotaRetiradaItem()
        {
            GridViewNotaRetiradaItem.DataSource = baseController.BuscaNotaRetiradaItemEmAberto(ddlDefeito.SelectedValue, ddlDestino.SelectedValue, txtData.Text);
            GridViewNotaRetiradaItem.DataBind();
        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            int totalRegistros = 0;

            foreach (GridViewRow item in GridViewNotaRetiradaItem.Rows)
            {
                CheckBox cbAlterado = item.FindControl("cbAlterado") as CheckBox;

                if (cbAlterado != null)
                {
                    if (cbAlterado.Checked)
                    {
                        usuarioController.AtualizaNotaRetiradaItem(Convert.ToInt32(GridViewNotaRetiradaItem.DataKeys[item.RowIndex].Value));

                        totalRegistros++;
                    }
                }
            }

            lblMensagem.Text = totalRegistros + " Gravados com sucesso !!!";

            CarregaGridViewNotaRetiradaItem();
        }

        protected void GridViewNotaRetiradaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_RETIRADA_ITEM notaRetiradaItem = e.Row.DataItem as NOTA_RETIRADA_ITEM;

            if (notaRetiradaItem != null)
            {
                Button btExcluir = e.Row.FindControl("btExcluir") as Button;

                if (btExcluir != null)
                    btExcluir.CommandArgument = notaRetiradaItem.CODIGO_NOTA_RETIRADA_ITEM.ToString();
            }
            
            DropDownList ddlDestino = e.Row.FindControl("ddlDestino") as DropDownList;

            if (ddlDestino != null)
            {
                ddlDestino.DataSource = baseController.BuscaDestinos();
                ddlDestino.DataBind();
                ddlDestino.Items.Add(new ListItem("Selecione", "0"));
                ddlDestino.SelectedValue = notaRetiradaItem.CODIGO_DESTINO;
            }

            Literal literalItem = e.Row.FindControl("LiteralItem") as Literal;

            if (literalItem != null)
                literalItem.Text = i++.ToString();
        }

        private void CarregaGridViewNotaRetiradaItens(int codigoNotaRetirada)
        {
            GridViewNotaRetiradaItem.DataSource = usuarioController.BuscaNotaRetiradaItens(codigoNotaRetirada);
            GridViewNotaRetiradaItem.DataBind();
        }

        private void LimpaFeedBack()
        {
            lblMensagem.Text = string.Empty;
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button btDeletar = sender as Button;

            if (btDeletar != null)
            {
                NOTA_RETIRADA_ITEM notaRetiradaItem = baseController.BuscaNotaRetiradaItem(Convert.ToInt32(btDeletar.CommandArgument));

                if (notaRetiradaItem != null)
                {
                    usuarioController.ExcluiNotaRetiradaItem(notaRetiradaItem.CODIGO_NOTA_RETIRADA_ITEM);

                    CarregaGridViewNotaRetiradaItens(notaRetiradaItem.CODIGO_NOTA_RETIRADA);
                }
            }
        }
    }
}