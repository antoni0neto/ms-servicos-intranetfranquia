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
    public partial class crm_rfv : System.Web.UI.Page
    {
        CRMController crmController = new CRMController();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();

                if (tela == "1")
                    hrefVoltar.HRef = "crm_menu.aspx";

                CarregarTrimestre();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private void CarregarTrimestre()
        {
            var tri = crmController.ObterTrimestre().Where(p => p.ATIVO == true).ToList();

            tri.Insert(0, new TRIMESTRE { DATA = new DateTime(1900, 1, 1), TRI = "Selecione" });
            ddlTri.DataSource = tri;
            ddlTri.DataBind();
        }

        private List<SP_OBTER_CRM_RFVResult> ObterCRMRFV()
        {
            var rfv = crmController.ObterCRMRFV(Convert.ToDateTime(ddlTri.SelectedValue));
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

                var rfvs = new List<SP_OBTER_CRM_RFVResult>();
                var rfv = ObterCRMRFV();

                // RECENCIA
                rfvs.Add(new SP_OBTER_CRM_RFVResult { ORDEM = 1, DESCR = "Recência" });
                var recencia = rfv.Where(p => p.ORDEM == 10);
                rfvs.AddRange(recencia);
                rfvs.Add(new SP_OBTER_CRM_RFVResult
                {
                    ORDEM = 11,
                    FAIXA = 0,
                    DESCR = "",
                    QTDE_TRI5 = recencia.Sum(p => p.QTDE_TRI5),
                    PORC_TRI5 = 100,
                    QTDE_TRI4 = recencia.Sum(p => p.QTDE_TRI4),
                    PORC_TRI4 = 100,
                    QTDE_TRI3 = recencia.Sum(p => p.QTDE_TRI3),
                    PORC_TRI3 = 100,
                    QTDE_TRI2 = recencia.Sum(p => p.QTDE_TRI2),
                    PORC_TRI2 = 100,
                    QTDE_TRI1 = recencia.Sum(p => p.QTDE_TRI1),
                    PORC_TRI1 = 100
                });
                rfvs.Add(new SP_OBTER_CRM_RFVResult { ORDEM = 12 });

                // FREQUENCIA
                rfvs.Add(new SP_OBTER_CRM_RFVResult { ORDEM = 15, DESCR = "Frequência" });
                var frequencia = rfv.Where(p => p.ORDEM == 20);
                rfvs.AddRange(frequencia);
                rfvs.Add(new SP_OBTER_CRM_RFVResult
                {
                    ORDEM = 21,
                    FAIXA = 0,
                    DESCR = "",
                    QTDE_TRI5 = frequencia.Sum(p => p.QTDE_TRI5),
                    PORC_TRI5 = 100,
                    QTDE_TRI4 = frequencia.Sum(p => p.QTDE_TRI4),
                    PORC_TRI4 = 100,
                    QTDE_TRI3 = frequencia.Sum(p => p.QTDE_TRI3),
                    PORC_TRI3 = 100,
                    QTDE_TRI2 = frequencia.Sum(p => p.QTDE_TRI2),
                    PORC_TRI2 = 100,
                    QTDE_TRI1 = frequencia.Sum(p => p.QTDE_TRI1),
                    PORC_TRI1 = 100
                });
                rfvs.Add(new SP_OBTER_CRM_RFVResult { ORDEM = 22 });

                // VALOR
                rfvs.Add(new SP_OBTER_CRM_RFVResult { ORDEM = 25, DESCR = "Valor" });
                var valor = rfv.Where(p => p.ORDEM == 30);
                rfvs.AddRange(valor);
                rfvs.Add(new SP_OBTER_CRM_RFVResult
                {
                    ORDEM = 31,
                    FAIXA = 0,
                    DESCR = "",
                    QTDE_TRI5 = valor.Sum(p => p.QTDE_TRI5),
                    PORC_TRI5 = 100,
                    QTDE_TRI4 = valor.Sum(p => p.QTDE_TRI4),
                    PORC_TRI4 = 100,
                    QTDE_TRI3 = valor.Sum(p => p.QTDE_TRI3),
                    PORC_TRI3 = 100,
                    QTDE_TRI2 = valor.Sum(p => p.QTDE_TRI2),
                    PORC_TRI2 = 100,
                    QTDE_TRI1 = valor.Sum(p => p.QTDE_TRI1),
                    PORC_TRI1 = 100
                });
                rfvs.Add(new SP_OBTER_CRM_RFVResult { ORDEM = 32 });


                gvRFV.DataSource = rfvs.OrderBy(p => p.ORDEM).ThenBy(p => p.FAIXA);
                gvRFV.DataBind();


                var trimestre = crmController.ObterTrimestre().Where(p => p.DATA <= Convert.ToDateTime(ddlTri.SelectedValue)).OrderByDescending(p => p.DATA).Take(5).ToList();
                int i = 1;
                foreach (var tri in trimestre)
                {
                    if (i == 1) labTri1.Text = tri.TRI;
                    else if (i == 2) labTri2.Text = tri.TRI;
                    else if (i == 3) labTri3.Text = tri.TRI;
                    else if (i == 4) labTri4.Text = tri.TRI;
                    else if (i == 5) labTri5.Text = tri.TRI;

                    i = i + 1;
                }

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
                    SP_OBTER_CRM_RFVResult rfv = e.Row.DataItem as SP_OBTER_CRM_RFVResult;

                    if (rfv.ORDEM == 10 || rfv.ORDEM == 20 || rfv.ORDEM == 30
                        || rfv.ORDEM == 11 || rfv.ORDEM == 21 || rfv.ORDEM == 31)
                    {
                        Literal litFaixa = e.Row.FindControl("litFaixa") as Literal;
                        litFaixa.Text = (rfv.FAIXA == 0) ? "Total" : rfv.FAIXA.ToString();

                        Literal litDescr = e.Row.FindControl("litDescr") as Literal;
                        litDescr.Text = rfv.DESCR;

                        Literal litQtdeCliente5 = e.Row.FindControl("litQtdeCliente5") as Literal;
                        litQtdeCliente5.Text = rfv.QTDE_TRI5.ToString("###,###,###,###,###");
                        Literal litQtdeCliente5Porc = e.Row.FindControl("litQtdeCliente5Porc") as Literal;
                        litQtdeCliente5Porc.Text = Convert.ToDecimal(rfv.PORC_TRI5).ToString("###,##0.00") + "%";

                        Literal litQtdeCliente4 = e.Row.FindControl("litQtdeCliente4") as Literal;
                        litQtdeCliente4.Text = rfv.QTDE_TRI4.ToString("###,###,###,###,###");
                        Literal litQtdeCliente4Porc = e.Row.FindControl("litQtdeCliente4Porc") as Literal;
                        litQtdeCliente4Porc.Text = Convert.ToDecimal(rfv.PORC_TRI4).ToString("###,##0.00") + "%";

                        Literal litQtdeCliente3 = e.Row.FindControl("litQtdeCliente3") as Literal;
                        litQtdeCliente3.Text = rfv.QTDE_TRI3.ToString("###,###,###,###,###");
                        Literal litQtdeCliente3Porc = e.Row.FindControl("litQtdeCliente3Porc") as Literal;
                        litQtdeCliente3Porc.Text = Convert.ToDecimal(rfv.PORC_TRI3).ToString("###,##0.00") + "%";

                        Literal litQtdeCliente2 = e.Row.FindControl("litQtdeCliente2") as Literal;
                        litQtdeCliente2.Text = rfv.QTDE_TRI2.ToString("###,###,###,###,###");
                        Literal litQtdeCliente2Porc = e.Row.FindControl("litQtdeCliente2Porc") as Literal;
                        litQtdeCliente2Porc.Text = Convert.ToDecimal(rfv.PORC_TRI2).ToString("###,##0.00") + "%";

                        Literal litQtdeCliente1 = e.Row.FindControl("litQtdeCliente1") as Literal;
                        litQtdeCliente1.Text = rfv.QTDE_TRI1.ToString("###,###,###,###,###");
                        Literal litQtdeCliente1Porc = e.Row.FindControl("litQtdeCliente1Porc") as Literal;
                        litQtdeCliente1Porc.Text = Convert.ToDecimal(rfv.PORC_TRI1).ToString("###,##0.00") + "%";
                    }

                    if (rfv.ORDEM == 1 || rfv.ORDEM == 15 || rfv.ORDEM == 25)
                    {
                        e.Row.Height = Unit.Pixel(25);
                        e.Row.BackColor = Color.WhiteSmoke;
                        e.Row.Font.Bold = true;

                        Literal litFaixa = e.Row.FindControl("litFaixa") as Literal;
                        litFaixa.Text = rfv.DESCR;
                    }

                    if (rfv.ORDEM == 11 || rfv.ORDEM == 21 || rfv.ORDEM == 31)
                    {
                        e.Row.Height = Unit.Pixel(25);
                        e.Row.BackColor = Color.FloralWhite;
                        e.Row.Font.Bold = true;
                    }

                    if (rfv.ORDEM == 12 || rfv.ORDEM == 22 || rfv.ORDEM == 32)
                    {
                        e.Row.Height = Unit.Pixel(6);
                        e.Row.BackColor = Color.LightGray;
                    }

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
        }

    }
}
