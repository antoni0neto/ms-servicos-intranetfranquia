using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class crm_rfv_score : System.Web.UI.Page
    {
        CRMController crmController = new CRMController();

        decimal qtdeAtivacao = 0;
        decimal qtdePotencializacao = 0;
        decimal qtdeFidelizacao = 0;
        decimal qtdeRecuperacao = 0;
        decimal qtdeCliAtivo = 0;
        decimal qtdeReativacao = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                var tela = Request.QueryString["t"].ToString();
                var data = Request.QueryString["da"].ToString();

                DateTime? dataTri = null;
                if (data != "")
                    dataTri = Convert.ToDateTime(data);

                if (tela == "1")
                    hrefVoltar.HRef = "crm_menu.aspx";

                CarregarTrimestre(dataTri);
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private void CarregarTrimestre(DateTime? dataTri)
        {
            var tri = crmController.ObterTrimestre();

            tri.Insert(0, new TRIMESTRE { DATA = new DateTime(1900, 1, 1), TRI = "Selecione" });
            ddlTri.DataSource = tri;
            ddlTri.DataBind();

            if (dataTri != null)
            {
                ddlTri.SelectedValue = dataTri.ToString();
                btBuscar_Click(btBuscar, null);
            }
        }

        private List<SP_OBTER_CRM_RFV_SCOREResult> ObterCRMRFVSCORE()
        {
            var rfv = crmController.ObterCRMRFVSCORE(Convert.ToDateTime(ddlTri.SelectedValue));
            return rfv;
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlTri.SelectedValue == "01/01/1900 00:00:00")
                {
                    labErro.Text = "Selecione o trimestre para comparação...";
                    return;
                }

                var scores = ObterCRMRFVSCORE();

                qtdeAtivacao = scores.Sum(p => p.ATIVACAO);
                qtdePotencializacao = scores.Sum(p => p.POTENCIALIZACAO);
                qtdeFidelizacao = scores.Sum(p => p.FIDELIZACAO);
                qtdeRecuperacao = scores.Sum(p => p.RECUPERACAO);
                qtdeCliAtivo = Convert.ToDecimal(scores.Sum(p => p.CLIENTES_ATIVOS));
                qtdeReativacao = scores.Sum(p => p.REATIVACAO);

                gvRFV.DataSource = scores.OrderByDescending(p => p.SCORE);
                gvRFV.DataBind();


            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void gvRFV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CRM_RFV_SCOREResult scores = e.Row.DataItem as SP_OBTER_CRM_RFV_SCOREResult;

                    Literal litScore = e.Row.FindControl("litScore") as Literal;
                    litScore.Text = scores.SCORE.ToString();

                    Literal litAtivacao = e.Row.FindControl("litAtivacao") as Literal;
                    litAtivacao.Text = scores.ATIVACAO.ToString("###,###,###,###");
                    Literal litAtivacaoPorc = e.Row.FindControl("litAtivacaoPorc") as Literal;
                    litAtivacaoPorc.Text = (qtdeAtivacao > 0 && scores.ATIVACAO > 0) ? (((scores.ATIVACAO * 1.00M) / qtdeAtivacao * 100.00M).ToString("###,###,##0.00") + " %") : "";

                    Literal litPotencializacao = e.Row.FindControl("litPotencializacao") as Literal;
                    litPotencializacao.Text = scores.POTENCIALIZACAO.ToString("###,###,###,###");
                    Literal litPotencializacaoPorc = e.Row.FindControl("litPotencializacaoPorc") as Literal;
                    litPotencializacaoPorc.Text = (qtdePotencializacao > 0 && scores.POTENCIALIZACAO > 0) ? (((scores.POTENCIALIZACAO * 1.00M) / qtdePotencializacao * 100.00M).ToString("###,###,##0.00") + " %") : "";

                    Literal litFidelizacao = e.Row.FindControl("litFidelizacao") as Literal;
                    litFidelizacao.Text = scores.FIDELIZACAO.ToString("###,###,###,###");
                    Literal litFidelizacaoPorc = e.Row.FindControl("litFidelizacaoPorc") as Literal;
                    litFidelizacaoPorc.Text = (qtdeFidelizacao > 0 && scores.FIDELIZACAO > 0) ? (((scores.FIDELIZACAO * 1.00M) / qtdeFidelizacao * 100.00M).ToString("###,###,##0.00") + " %") : "";

                    Literal litRecuperacao = e.Row.FindControl("litRecuperacao") as Literal;
                    litRecuperacao.Text = scores.RECUPERACAO.ToString("###,###,###,###");
                    Literal litRecuperacaoPorc = e.Row.FindControl("litRecuperacaoPorc") as Literal;
                    litRecuperacaoPorc.Text = (qtdeRecuperacao > 0 && scores.RECUPERACAO > 0) ? (((scores.RECUPERACAO * 1.00M) / qtdeRecuperacao * 100.00M).ToString("###,###,##0.00") + " %") : "";

                    Literal litCliAtivo = e.Row.FindControl("litCliAtivo") as Literal;
                    litCliAtivo.Text = Convert.ToInt32(scores.CLIENTES_ATIVOS).ToString("###,###,###,###");
                    Literal litCliAtivoPorc = e.Row.FindControl("litCliAtivoPorc") as Literal;
                    litCliAtivoPorc.Text = (qtdeCliAtivo > 0 && scores.CLIENTES_ATIVOS > 0) ? (((Convert.ToInt32(scores.CLIENTES_ATIVOS) * 1.00M) / qtdeCliAtivo * 100.00M).ToString("###,###,##0.00") + " %") : "";

                    Literal litReativacao = e.Row.FindControl("litReativacao") as Literal;
                    litReativacao.Text = scores.REATIVACAO.ToString("###,###,###,###");
                    Literal litReativacaoPorc = e.Row.FindControl("litReativacaoPorc") as Literal;
                    litReativacaoPorc.Text = (qtdeReativacao > 0 && scores.REATIVACAO > 0) ? (((scores.REATIVACAO * 1.00M) / qtdeReativacao * 100.00M).ToString("###,###,##0.00") + " %") : "";

                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvRFV_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvRFV.FooterRow;
            if (footer != null)
            {
                footer.Cells[0].Text = "Total";

                footer.Cells[2].Text = qtdeAtivacao.ToString("###,###,###,###");
                footer.Cells[3].Text = "100,00 %";

                footer.Cells[5].Text = qtdePotencializacao.ToString("###,###,###,###");
                footer.Cells[6].Text = "100,00 %";

                footer.Cells[8].Text = qtdeFidelizacao.ToString("###,###,###,###");
                footer.Cells[9].Text = "100,00 %";

                footer.Cells[11].Text = qtdeRecuperacao.ToString("###,###,###,###");
                footer.Cells[12].Text = "100,00 %";

                footer.Cells[14].Text = qtdeCliAtivo.ToString("###,###,###,###");
                footer.Cells[15].Text = "100,00 %";

                footer.Cells[17].Text = qtdeReativacao.ToString("###,###,###,###");
                footer.Cells[18].Text = "100,00 %";
            }
        }

    }
}
