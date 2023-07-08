using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class gerloja_griffe_porc : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        int qtdeVFem = 0;
        int qtdeEFem = 0;
        decimal valVFem = 0;
        decimal valEFem = 0;

        int qtdeVPet = 0;
        int qtdeEPet = 0;
        decimal valVPet = 0;
        decimal valEPet = 0;

        int qtdeVMasc = 0;
        int qtdeEMasc = 0;
        decimal valVMasc = 0;
        decimal valEMasc = 0;

        int qtdeVCalc = 0;
        int qtdeECalc = 0;
        decimal valVCalc = 0;
        decimal valECalc = 0;

        int qtdeVOut = 0;
        int qtdeEOut = 0;
        decimal valVOut = 0;
        decimal valEOut = 0;

        decimal valVTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";

                CarregarSemanas();
                CarregarFiliais();
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarSemanas()
        {
            var semana = lojaController.ObterSemanaVenda();

            semana.Insert(0, new LOJA_VENDA_SEMANA { CODIGO = 0, SEMANA = "Selecione" });

            ddlSemana.DataSource = semana;
            ddlSemana.DataBind();
        }
        private void CarregarFiliais()
        {
            var filial = baseController.ObterFiliaisLojaIntranet();

            filial.Insert(0, new FILIAI1 { COD_FILIAL = "", FILIAL = "" });
            ddlFilial.DataSource = filial;
            ddlFilial.DataBind();

            if (filial.Count() == 2)
            {
                ddlFilial.SelectedIndex = 1;
                ddlFilial.Enabled = false;
            }

        }
        #endregion

        private List<SP_OBTER_VENDA_PORGRIFFEResult> ObterVendaPorGriffe()
        {
            var semanaSel = lojaController.ObterSemanaVenda(Convert.ToInt32(ddlSemana.SelectedValue));
            bool? filialEtc = null;

            if (ddlFilialEtc.SelectedValue != "")
                filialEtc = (ddlFilialEtc.SelectedValue == "S") ? true : false;

            var usuario = ((USUARIO)Session["USUARIO"]);

            var venda = lojaController.ObterVendaPorGriffe(semanaSel.DATA_INI, semanaSel.DATA_FIM, filialEtc, ddlFilial.SelectedValue.Trim(), usuario.CODIGO_USUARIO);

            return venda;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlSemana.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Semana...";
                    return;
                }

                CarregarGriffePorc();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }


        }
        private void CarregarGriffePorc()
        {
            var venda = ObterVendaPorGriffe();

            gvGriffePorc.DataSource = venda;
            gvGriffePorc.DataBind();
        }

        protected void gvGriffePorc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDA_PORGRIFFEResult venda = e.Row.DataItem as SP_OBTER_VENDA_PORGRIFFEResult;

                    if (venda != null)
                    {
                        Label labValVendaFem = e.Row.FindControl("labValVendaFem") as Label;
                        labValVendaFem.Text = Convert.ToDecimal(venda.VALOR_VENDA_FEM).ToString("###,###,###,###,##0.00");
                        Label labValEstoqueFem = e.Row.FindControl("labValEstoqueFem") as Label;
                        labValEstoqueFem.Text = Convert.ToDecimal(venda.VALOR_ESTOQUE_FEM).ToString("###,###,###,###,##0.00");
                        Label labPorcVendaFem = e.Row.FindControl("labPorcVendaFem") as Label;
                        labPorcVendaFem.Text = Convert.ToDecimal(venda.PORC_VENDA_FEM).ToString("N2") + "%";

                        Label labValVendaPetit = e.Row.FindControl("labValVendaPetit") as Label;
                        labValVendaPetit.Text = Convert.ToDecimal(venda.VALOR_VENDA_PETIT).ToString("###,###,###,###,##0.00");
                        Label labValEstoquePetit = e.Row.FindControl("labValEstoquePetit") as Label;
                        labValEstoquePetit.Text = Convert.ToDecimal(venda.VALOR_ESTOQUE_PETIT).ToString("###,###,###,###,##0.00");
                        Label labPorcVendaPetit = e.Row.FindControl("labPorcVendaPetit") as Label;
                        labPorcVendaPetit.Text = Convert.ToDecimal(venda.PORC_VENDA_PETIT).ToString("N2") + "%";

                        Label labValVendaMasc = e.Row.FindControl("labValVendaMasc") as Label;
                        labValVendaMasc.Text = Convert.ToDecimal(venda.VALOR_VENDA_MASC).ToString("###,###,###,###,##0.00");
                        Label labValEstoqueMasc = e.Row.FindControl("labValEstoqueMasc") as Label;
                        labValEstoqueMasc.Text = Convert.ToDecimal(venda.VALOR_ESTOQUE_MASC).ToString("###,###,###,###,##0.00");
                        Label labPorcVendaMasc = e.Row.FindControl("labPorcVendaMasc") as Label;
                        labPorcVendaMasc.Text = Convert.ToDecimal(venda.PORC_VENDA_MASC).ToString("N2") + "%";

                        Label labValVendaCalc = e.Row.FindControl("labValVendaCalc") as Label;
                        labValVendaCalc.Text = Convert.ToDecimal(venda.VALOR_VENDA_CALC).ToString("###,###,###,###,##0.00");
                        Label labValEstoqueCalc = e.Row.FindControl("labValEstoqueCalc") as Label;
                        labValEstoqueCalc.Text = Convert.ToDecimal(venda.VALOR_ESTOQUE_CALC).ToString("###,###,###,###,##0.00");
                        Label labPorcVendaCalc = e.Row.FindControl("labPorcVendaCalc") as Label;
                        labPorcVendaCalc.Text = Convert.ToDecimal(venda.PORC_VENDA_CALC).ToString("N2") + "%";

                        Label labValVendaOut = e.Row.FindControl("labValVendaOut") as Label;
                        labValVendaOut.Text = Convert.ToDecimal(venda.VALOR_VENDA_OUTRO).ToString("###,###,###,###,##0.00");
                        Label labValEstoqueOut = e.Row.FindControl("labValEstoqueOut") as Label;
                        labValEstoqueOut.Text = Convert.ToDecimal(venda.VALOR_ESTOQUE_OUTRO).ToString("###,###,###,###,##0.00");
                        Label labPorcVendaOut = e.Row.FindControl("labPorcVendaOut") as Label;
                        labPorcVendaOut.Text = Convert.ToDecimal(venda.PORC_VENDA_OUTRO).ToString("N2") + "%";

                        Label labValVendaTot = e.Row.FindControl("labValVendaTot") as Label;
                        labValVendaTot.Text = Convert.ToDecimal(venda.VALOR_VENDA_FEM + venda.VALOR_VENDA_PETIT + venda.VALOR_VENDA_MASC + venda.VALOR_VENDA_CALC + venda.VALOR_VENDA_OUTRO).ToString("###,###,###,###,##0.00");

                        qtdeVFem += Convert.ToInt32(venda.QTDE_VENDA_FEM);
                        qtdeEFem += Convert.ToInt32(venda.QTDE_ESTOQUE_FEM);
                        valVFem += Convert.ToDecimal(venda.VALOR_VENDA_FEM);
                        valEFem += Convert.ToDecimal(venda.VALOR_ESTOQUE_FEM);

                        qtdeVPet += Convert.ToInt32(venda.QTDE_VENDA_PETIT);
                        qtdeEPet += Convert.ToInt32(venda.QTDE_ESTOQUE_PETIT);
                        valVPet += Convert.ToDecimal(venda.VALOR_VENDA_PETIT);
                        valEPet += Convert.ToDecimal(venda.VALOR_ESTOQUE_PETIT);

                        qtdeVMasc += Convert.ToInt32(venda.QTDE_VENDA_MASC);
                        qtdeEMasc += Convert.ToInt32(venda.QTDE_ESTOQUE_MASC);
                        valVMasc += Convert.ToDecimal(venda.VALOR_VENDA_MASC);
                        valEMasc += Convert.ToDecimal(venda.VALOR_ESTOQUE_MASC);

                        qtdeVCalc += Convert.ToInt32(venda.QTDE_VENDA_CALC);
                        qtdeECalc += Convert.ToInt32(venda.QTDE_ESTOQUE_CALC);
                        valVCalc += Convert.ToDecimal(venda.VALOR_VENDA_CALC);
                        valECalc += Convert.ToDecimal(venda.VALOR_ESTOQUE_CALC);

                        qtdeVOut += Convert.ToInt32(venda.QTDE_VENDA_OUTRO);
                        qtdeEOut += Convert.ToInt32(venda.QTDE_ESTOQUE_OUTRO);
                        valVOut += Convert.ToDecimal(venda.VALOR_VENDA_OUTRO);
                        valEOut += Convert.ToDecimal(venda.VALOR_ESTOQUE_OUTRO);

                        valVTotal += (Convert.ToDecimal(venda.VALOR_VENDA_FEM + venda.VALOR_VENDA_PETIT + venda.VALOR_VENDA_MASC + venda.VALOR_VENDA_CALC + venda.VALOR_VENDA_OUTRO));
                    }
                }
            }
        }
        protected void gvGriffePorc_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvGriffePorc.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[2].Text = qtdeVFem.ToString();
                footer.Cells[3].Text = qtdeEFem.ToString();
                footer.Cells[4].Text = valVFem.ToString("###,###,###0.00");
                footer.Cells[5].Text = valEFem.ToString("###,###,###0.00");
                var porcFem = (valVTotal <= 0) ? 0M : (valVFem / valVTotal * 100.00M);
                footer.Cells[6].Text = porcFem.ToString("N2") + "%";

                footer.Cells[7].Text = qtdeVPet.ToString();
                footer.Cells[8].Text = qtdeEPet.ToString();
                footer.Cells[9].Text = valVPet.ToString("###,###,###0.00");
                footer.Cells[10].Text = valEPet.ToString("###,###,###0.00");
                var porcPet = (valVTotal <= 0) ? 0M : (valVPet / valVTotal * 100.00M);
                footer.Cells[11].Text = porcPet.ToString("N2") + "%";

                footer.Cells[12].Text = qtdeVMasc.ToString();
                footer.Cells[13].Text = qtdeEMasc.ToString();
                footer.Cells[14].Text = valVMasc.ToString("###,###,###0.00");
                footer.Cells[15].Text = valEMasc.ToString("###,###,###0.00");
                var porcMasc = (valVTotal <= 0) ? 0M : (valVMasc / valVTotal * 100.00M);
                footer.Cells[16].Text = porcMasc.ToString("N2") + "%";

                footer.Cells[17].Text = qtdeVCalc.ToString();
                footer.Cells[18].Text = qtdeECalc.ToString();
                footer.Cells[19].Text = valVCalc.ToString("###,###,###0.00");
                footer.Cells[20].Text = valECalc.ToString("###,###,###0.00");
                var porcCalc = (valVTotal <= 0) ? 0M : (valVCalc / valVTotal * 100.00M);
                footer.Cells[21].Text = porcCalc.ToString("N2") + "%";

                footer.Cells[22].Text = qtdeVOut.ToString();
                footer.Cells[23].Text = qtdeEOut.ToString();
                footer.Cells[24].Text = valVOut.ToString("###,###,###0.00");
                footer.Cells[25].Text = valEOut.ToString("###,###,###0.00");
                var porcOutro = (valVTotal <= 0) ? 0M : (valVOut / valVTotal * 100.00M);
                footer.Cells[26].Text = porcOutro.ToString("N2") + "%";

                footer.Cells[27].Text = valVTotal.ToString("###,###,###0.00");

            }
        }
        protected void gvGriffePorc_Sorting(object sender, GridViewSortEventArgs e)
        {

            IEnumerable<SP_OBTER_VENDA_PORGRIFFEResult> venda = ObterVendaPorGriffe();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            venda = venda.OrderBy(e.SortExpression + sortDirection);
            gvGriffePorc.DataSource = venda;
            gvGriffePorc.DataBind();

        }


    }
}
