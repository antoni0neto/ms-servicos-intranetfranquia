using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Drawing;
using System.IO;

namespace Relatorios
{
    public partial class atac_rel_representante_cota : System.Web.UI.Page
    {
        AtacadoController atacController = new AtacadoController();
        BaseController baseController = new BaseController();
        USUARIO usuario = null;

        List<SP_OBTER_CLIENTE_COLECAO_COTA_COLResult> colecaoColuna = null;
        List<SP_OBTER_CLIENTE_COLECAO_COTAResult> gTotal = new List<SP_OBTER_CLIENTE_COLECAO_COTAResult>();

        string callGridViewScroll = "$(function () { gridviewScroll(); });";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["USUARIO"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                    usuario = (USUARIO)Session["USUARIO"];

                CarregarColecoes();
                CarregarRepresentante();
                CarregarUF();

                Session["CLIENTE_COL_COTA"] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btCopiarSugestaoCota.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btCopiarSugestaoCota, null) + ";");
        }

        #region "DADOS INICIAS"
        private void CarregarColecoes()
        {

            var colecao = baseController.ObterColecaoAtacado();
            if (colecao != null)
            {
                colecao.Insert(0, new SP_OBTER_COLECAO_ATACADOResult { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecao.DataSource = colecao;
                ddlColecao.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecao.SelectedValue = Session["COLECAO"].ToString();
            }

        }
        private void CarregarRepresentante()
        {
            var rep = atacController.ObterRepresentanteUF();
            if (rep != null)
            {
                rep.Insert(0, new ATACADO_REP { REPRESENTANTE = "" });
                ddlRepresentante.DataSource = rep;
                ddlRepresentante.DataBind();
            }
        }
        private void CarregarUF()
        {
            var uf = baseController.ObterUF();
            if (uf != null)
            {
                uf.Insert(0, new LCF_LX_UF { UF = "" });
                ddlUF.DataSource = uf;
                ddlUF.DataBind();
            }
        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlColecao.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                int? nota = null;
                if (ddlNota.SelectedValue != "")
                    nota = Convert.ToInt32(ddlNota.SelectedValue);

                CarregarClienteColecao(ddlColecao.SelectedValue.Trim(), ddlRepresentante.SelectedValue.Trim(), txtNomeCliente.Text.Trim().ToUpper(), ddlUF.SelectedValue.Trim(), nota, ddlCredito.SelectedValue.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }

        private List<SP_OBTER_CLIENTE_COLECAO_COTAResult> ObterClienteColecaoCota()
        {
            string credito = "";
            string temCota = "";
            string comprouVerao = "";
            string comprouPrimavera = "";
            string comprouInverno = "";
            string comprouOutono = "";

            int? nota = null;
            if (ddlNota.SelectedValue != "")
                nota = Convert.ToInt32(ddlNota.SelectedValue);
            if (ddlCredito.SelectedValue != "")
                credito = ddlCredito.SelectedValue;
            if (ddlCota.SelectedValue != "")
                temCota = ddlCota.SelectedValue;
            if (ddlComprouVerao.SelectedValue != "")
                comprouVerao = ddlComprouVerao.SelectedValue;
            if (ddlComprouPrimavera.SelectedValue != "")
                comprouPrimavera = ddlComprouPrimavera.SelectedValue;
            if (ddlComprouInverno.SelectedValue != "")
                comprouInverno = ddlComprouInverno.SelectedValue;
            if (ddlComprouOutono.SelectedValue != "")
                comprouOutono = ddlComprouOutono.SelectedValue;


            var clienteColecaoCota = atacController.ObterClienteColecaoCota(ddlColecao.SelectedValue.Trim(), ddlRepresentante.SelectedValue, txtNomeCliente.Text.Trim().ToUpper(), ddlUF.SelectedValue);
            if (nota != null)
                clienteColecaoCota = clienteColecaoCota.Where(p => p.NOTA_CIDADE == nota).ToList();

            if (credito != "")
            {
                if (credito == "VAZIO")
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.CREDITO == "").ToList();
                else
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.CREDITO == credito).ToList();
            }

            if (temCota != "")
            {
                if (temCota == "S")
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.COTA_VALOR_COL_ATUAL > 0).ToList();
                else
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.COTA_VALOR_COL_ATUAL <= 0).ToList();
            }

            if (comprouVerao != "")
            {
                if (comprouVerao == "S")
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.QTDE_COLECAO_1 > 0).ToList();
                else
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.QTDE_COLECAO_1 <= 0).ToList();
            }

            if (comprouPrimavera != "")
            {
                if (comprouPrimavera == "S")
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.QTDE_COLECAO_2 > 0).ToList();
                else
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.QTDE_COLECAO_2 <= 0).ToList();
            }

            if (comprouInverno != "")
            {
                if (comprouInverno == "S")
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.QTDE_COLECAO_3 > 0).ToList();
                else
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.QTDE_COLECAO_3 <= 0).ToList();
            }

            if (comprouOutono != "")
            {
                if (comprouOutono == "S")
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.QTDE_COLECAO_4 > 0).ToList();
                else
                    clienteColecaoCota = clienteColecaoCota.Where(p => p.QTDE_COLECAO_4 <= 0).ToList();
            }

            gTotal.Clear();
            gTotal.AddRange(clienteColecaoCota);

            return clienteColecaoCota;
        }
        private void CarregarClienteColecao(string colecao, string representante, string cliente, string uf, int? nota, string credito)
        {

            Utils.WebControls.GetBoundFieldIndexByName(gvClienteColecao, " - >>");
            Utils.WebControls.GetBoundFieldIndexByName(gvClienteColecao, " - <<");

            btExcel.Enabled = false;

            //Session["CLIENTE_COL_COTA"] = null;
            var clienteColecaoCota = ObterClienteColecaoCota();

            gvClienteColecao.DataSource = clienteColecaoCota;
            gvClienteColecao.DataBind();
            //Session["CLIENTE_COL_COTA"] = clienteColecaoCota;

            if (gvClienteColecao.Rows.Count > 0 && !cbEditarCota.Checked)
                btExcel.Enabled = true;

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);

        }
        private void CarregarClienteColecaoOrdenado(string sortExpression, string sortDirection)
        {

            //if (Session["CLIENTE_COL_COTA"] != null)
            //{
            IEnumerable<SP_OBTER_CLIENTE_COLECAO_COTAResult> clienteColecaoCota = ObterClienteColecaoCota();

            if (sortExpression != "" && sortDirection != "")
                clienteColecaoCota = clienteColecaoCota.OrderBy(sortExpression + " " + sortDirection);

            gvClienteColecao.DataSource = clienteColecaoCota.ToList();
            gvClienteColecao.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
            //}
        }
        private void AlterarNomeColuna(GridViewRow r)
        {
            string colecao = ddlColecao.SelectedValue.Trim();
            string colNome = "";

            colecaoColuna = atacController.ObterClienteColecaoCotaCol(colecao);

            colNome = "ERRO";
            if (colecaoColuna != null && colecaoColuna.Count() > 0 && colecaoColuna[0] != null)
                colNome = Utils.WebControls.AlterarPrimeiraLetraMaiscula(colecaoColuna[0].COLECAO.Substring(4, colecaoColuna[0].COLECAO.Length - 4));

            r.Cells[31].Text = "Qtde Orig. " + colNome; //QtdeOrigCol4
            r.Cells[32].Text = "Valor Orig. " + colNome; //ValOrigCol4
            r.Cells[33].Text = "Qtde Entr. " + colNome; //QtdeEntCol4
            r.Cells[34].Text = "Valor Entr. " + colNome; //ValEntCol4
            r.Cells[35].Text = "Qtde Canc. " + colNome; //QtdeCancCol4
            r.Cells[36].Text = "Valor Canc. " + colNome; //ValCancCol4

            colNome = "ERRO";
            if (colecaoColuna != null && colecaoColuna.Count() > 0 && colecaoColuna[1] != null)
                colNome = Utils.WebControls.AlterarPrimeiraLetraMaiscula(colecaoColuna[1].COLECAO.Substring(4, colecaoColuna[1].COLECAO.Length - 4));

            r.Cells[37].Text = "Qtde Orig. " + colNome; //QtdeOrigCol3
            r.Cells[38].Text = "Valor Orig. " + colNome; //ValOrigCol3
            r.Cells[39].Text = "Qtde Entr. " + colNome; //QtdeEntCol3
            r.Cells[40].Text = "Valor Entr. " + colNome; //ValEntCol3
            r.Cells[41].Text = "Qtde Canc. " + colNome; //QtdeCancCol3
            r.Cells[42].Text = "Valor Canc. " + colNome; //ValCancCol3

            colNome = "ERRO";
            if (colecaoColuna != null && colecaoColuna.Count() > 0 && colecaoColuna[2] != null)
                colNome = Utils.WebControls.AlterarPrimeiraLetraMaiscula(colecaoColuna[2].COLECAO.Substring(4, colecaoColuna[2].COLECAO.Length - 4));

            r.Cells[43].Text = "Qtde Orig. " + colNome; //QtdeOrigCol2
            r.Cells[44].Text = "Valor Orig. " + colNome; //ValOrigCol2
            r.Cells[45].Text = "Qtde Entr. " + colNome; //QtdeEntCol2
            r.Cells[46].Text = "Valor Entr. " + colNome; //ValEntCol2
            r.Cells[47].Text = "Qtde Canc. " + colNome; //QtdeCancCol2
            r.Cells[48].Text = "Valor Canc. " + colNome; //ValCancCol2

            colNome = "ERRO";
            if (colecaoColuna != null && colecaoColuna.Count() > 0 && colecaoColuna[3] != null)
                colNome = Utils.WebControls.AlterarPrimeiraLetraMaiscula(colecaoColuna[3].COLECAO.Substring(4, colecaoColuna[3].COLECAO.Length - 4));

            r.Cells[49].Text = "Qtde Orig. " + colNome; //QtdeOrigCol1
            r.Cells[50].Text = "Valor Orig. " + colNome; //ValOrigCol1
            r.Cells[51].Text = "Qtde Entr. " + colNome; //QtdeEntCol1
            r.Cells[52].Text = "Valor Entr. " + colNome; //ValEntCol1
            r.Cells[53].Text = "Qtde Canc. " + colNome; //QtdeCancCol1
            r.Cells[54].Text = "Valor Canc. " + colNome; //ValCancCol1

        }

        protected void gvClienteColecao_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                AlterarNomeColuna(e.Row);
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }

            SP_OBTER_CLIENTE_COLECAO_COTAResult clienteCota = e.Row.DataItem as SP_OBTER_CLIENTE_COLECAO_COTAResult;
            if (clienteCota != null)
            {
            }
        }
        protected void gvClienteColecao_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvClienteColecao.FooterRow;

            if (footer != null)
            {
                footer.Cells[2].Text = "Total - Clientes: " + gTotal.Count(p => p.CLIENTE != "").ToString();

                //IEnumerable<SP_OBTER_CLIENTE_COLECAO_COTAResult> clienteColecaoCota = Session["CLIENTE_COL_COTA"] as IEnumerable<SP_OBTER_CLIENTE_COLECAO_COTAResult>;


                footer.Cells[3].Text = gTotal.Count(p => p.MUNICIPIO != "").ToString();
                footer.Cells[7].Text = gTotal.Sum(p => p.COTA_QTDE_COL_ATUAL).ToString();
                footer.Cells[8].Text = "R$ " + gTotal.Sum(p => p.COTA_VALOR_COL_ATUAL).ToString("###,###,###,###,###,##0.00");
                footer.Cells[9].Text = "R$ " + gTotal.Sum(p => p.COTA_VAL_SUGESTAO).ToString("###,###,###,###,###,##0.00");

                footer.Cells[10].Text = gTotal.Sum(p => p.TOT_QTDE_ORIGINAL_COLATUAL).ToString();
                footer.Cells[10].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[11].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_ORIGINAL_COLATUAL).ToString("###,###,###,###,###,##0.00");

                footer.Cells[12].Text = "R$ " + gTotal.Sum(p => p.MEDIA_VAL_A).ToString("###,###,###,###,###,##0.00");
                footer.Cells[13].Text = "R$ " + gTotal.Sum(p => p.MEDIA_VAL_P).ToString("###,###,###,###,###,##0.00");
                footer.Cells[14].Text = "R$ " + gTotal.Sum(p => p.MEDIA_VAL_AP).ToString("###,###,###,###,###,##0.00");
                footer.Cells[15].Text = "R$ " + gTotal.Sum(p => p.MEDIA_VAL_W).ToString("###,###,###,###,###,##0.00");
                footer.Cells[16].Text = "R$ " + gTotal.Sum(p => p.MEDIA_VAL_U).ToString("###,###,###,###,###,##0.00");
                footer.Cells[17].Text = "R$ " + gTotal.Sum(p => p.MEDIA_VAL_WU).ToString("###,###,###,###,###,##0.00");
                footer.Cells[18].Text = "R$ " + gTotal.Sum(p => p.MEDIA_VAL_TOT).ToString("###,###,###,###,###,##0.00");

                footer.Cells[31].Text = gTotal.Sum(p => p.TOT_QTDE_ORIGINAL_COL4).ToString();
                footer.Cells[31].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[32].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_ORIGINAL_COL4).ToString("###,###,###,###,###,##0.00");
                footer.Cells[33].Text = gTotal.Sum(p => p.TOT_QTDE_ENTREGUE_COL4).ToString();
                footer.Cells[33].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[34].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_ENTREGUE_COL4).ToString("###,###,###,###,###,##0.00");
                footer.Cells[35].Text = gTotal.Sum(p => p.TOT_QTDE_CANCELADA_COL4).ToString();
                footer.Cells[35].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[36].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_CANCELADO_COL4).ToString("###,###,###,###,###,##0.00");

                footer.Cells[37].Text = gTotal.Sum(p => p.TOT_QTDE_ORIGINAL_COL3).ToString();
                footer.Cells[37].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[38].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_ORIGINAL_COL3).ToString("###,###,###,###,###,##0.00");
                footer.Cells[39].Text = gTotal.Sum(p => p.TOT_QTDE_ENTREGUE_COL3).ToString();
                footer.Cells[39].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[40].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_ENTREGUE_COL3).ToString("###,###,###,###,###,##0.00");
                footer.Cells[41].Text = gTotal.Sum(p => p.TOT_QTDE_CANCELADA_COL3).ToString();
                footer.Cells[41].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[42].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_CANCELADO_COL3).ToString("###,###,###,###,###,##0.00");

                footer.Cells[43].Text = gTotal.Sum(p => p.TOT_QTDE_ORIGINAL_COL2).ToString();
                footer.Cells[43].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[44].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_ORIGINAL_COL2).ToString("###,###,###,###,###,##0.00");
                footer.Cells[45].Text = gTotal.Sum(p => p.TOT_QTDE_ENTREGUE_COL2).ToString();
                footer.Cells[45].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[46].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_ENTREGUE_COL2).ToString("###,###,###,###,###,##0.00");
                footer.Cells[47].Text = gTotal.Sum(p => p.TOT_QTDE_CANCELADA_COL2).ToString();
                footer.Cells[47].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[48].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_CANCELADO_COL2).ToString("###,###,###,###,###,##0.00");

                footer.Cells[49].Text = gTotal.Sum(p => p.TOT_QTDE_ORIGINAL_COL1).ToString();
                footer.Cells[49].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[50].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_ORIGINAL_COL1).ToString("###,###,###,###,###,##0.00");
                footer.Cells[51].Text = gTotal.Sum(p => p.TOT_QTDE_ENTREGUE_COL1).ToString();
                footer.Cells[51].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[52].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_ENTREGUE_COL1).ToString("###,###,###,###,###,##0.00");
                footer.Cells[53].Text = gTotal.Sum(p => p.TOT_QTDE_CANCELADA_COL1).ToString();
                footer.Cells[53].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[54].Text = "R$ " + gTotal.Sum(p => p.TOT_VALOR_CANCELADO_COL1).ToString("###,###,###,###,###,##0.00");

            }
        }
        protected void gvClienteColecao_Sorting(object sender, GridViewSortEventArgs e)
        {
            //if (Session["CLIENTE_COL_COTA"] != null)
            //{
            //IEnumerable<SP_OBTER_CLIENTE_COLECAO_COTAResult> clienteColecaoCota = Session["CLIENTE_COL_COTA"] as IEnumerable<SP_OBTER_CLIENTE_COLECAO_COTAResult>;

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            CarregarClienteColecaoOrdenado(sortExpression, sortDirection);
            hidSortExpression.Value = sortExpression;
            hidSortDirection.Value = sortDirection;

            //}
        }
        protected void gvClienteColecao_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClienteColecao.PageIndex = e.NewPageIndex;
            CarregarClienteColecaoOrdenado(hidSortExpression.Value, hidSortDirection.Value);
        }
        protected void cbEditarCota_CheckedChanged(object sender, EventArgs e)
        {
            gvClienteColecao.DataSource = new List<SP_OBTER_CLIENTE_COLECAO_COTAResult>();
            gvClienteColecao.DataBind();

            btExcel.Enabled = false;

            if (cbEditarCota.Checked)
            {
                labPrecoMedio.Visible = true;
                txtPrecoMedio.Visible = true;
                labPorcAumentoCota.Visible = true;
                txtPorcAumentoCota.Visible = true;
                imgAtualizarCota.Visible = true;
                btCopiarSugestaoCota.Visible = true;

            }
            else
            {
                labPrecoMedio.Visible = false;
                txtPrecoMedio.Visible = false;
                labPorcAumentoCota.Visible = false;
                txtPorcAumentoCota.Visible = false;
                imgAtualizarCota.Visible = false;
                btCopiarSugestaoCota.Visible = false;
            }

            btBuscar_Click(null, null);
        }

        protected void txtCotaQtde_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                if (txt != null)
                {
                    GridViewRow g = (GridViewRow)txt.NamingContainer;
                    if (g != null)
                    {
                        int qtdeCota = 0;
                        if (txt.Text != "")
                            qtdeCota = Convert.ToInt32(txt.Text);
                        string cliente = "";
                        cliente = ((Label)g.FindControl("labCliente")).Text.Trim();

                        if (cliente != "")
                        {
                            var cc = atacController.ObterClienteCota(ddlColecao.SelectedValue.Trim(), cliente);
                            if (cc != null)
                            {
                                //Atualizar cota qtde
                                cc.QTDE = qtdeCota;
                                atacController.AtualizarClienteCota(cc);
                            }
                            else
                            {
                                ATACADO_REP_COTA n = new ATACADO_REP_COTA();
                                n.COLECAO = ddlColecao.SelectedValue.Trim();
                                n.CLIENTE = cliente.Trim();
                                n.QTDE = qtdeCota;
                                n.VALOR = 0;
                                atacController.InserirClienteCota(n);
                            }
                        }

                        //AlterarNomeColuna(gvClienteColecao.HeaderRow);
                        CarregarClienteColecaoOrdenado(hidSortExpression.Value, hidSortDirection.Value);
                    }


                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void txtCotaValor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                if (txt != null)
                {
                    GridViewRow g = (GridViewRow)txt.NamingContainer;
                    if (g != null)
                    {
                        decimal valorCota = 0;
                        if (txt.Text != "")
                            valorCota = Convert.ToDecimal(txt.Text);
                        string cliente = "";
                        cliente = ((Label)g.FindControl("labCliente")).Text.Trim();

                        if (cliente != "")
                        {
                            var cc = atacController.ObterClienteCota(ddlColecao.SelectedValue.Trim(), cliente);
                            if (cc != null)
                            {
                                //Atualizar cota valor
                                cc.VALOR = valorCota;
                                if (txtPrecoMedio.Text != "" && Convert.ToDecimal(txtPrecoMedio.Text) != 0)
                                    cc.QTDE = Convert.ToInt32(valorCota / Convert.ToDecimal(txtPrecoMedio.Text));
                                atacController.AtualizarClienteCota(cc);
                            }
                            else
                            {
                                ATACADO_REP_COTA n = new ATACADO_REP_COTA();
                                n.COLECAO = ddlColecao.SelectedValue.Trim();
                                n.CLIENTE = cliente.Trim();
                                if (txtPrecoMedio.Text != "" && Convert.ToDecimal(txtPrecoMedio.Text) != 0)
                                    n.QTDE = Convert.ToInt32(valorCota / Convert.ToDecimal(txtPrecoMedio.Text));
                                n.VALOR = valorCota;
                                atacController.InserirClienteCota(n);
                            }
                        }

                        // AlterarNomeColuna(gvClienteColecao.HeaderRow);
                        CarregarClienteColecaoOrdenado(hidSortExpression.Value, hidSortDirection.Value);
                    }


                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void imgAtualizarCota_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (txtPrecoMedio.Text == "" || Convert.ToDecimal(txtPrecoMedio.Text) <= 0)
                {
                    labErro.Text = "Informe o Preço Médio para Cálculo da Cota.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
                    return;
                }
                if (txtPorcAumentoCota.Text == "" || (Convert.ToDecimal(txtPorcAumentoCota.Text) == 0))
                {
                    labErro.Text = "Informe a Porcentagem para Cálculo da Cota.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
                    return;
                }

                //Calcular cota com base na porcentagem
                decimal porcAumentoCota = 0;
                decimal precoMedio = 0;

                porcAumentoCota = Convert.ToDecimal(txtPorcAumentoCota.Text);
                precoMedio = Convert.ToDecimal(txtPrecoMedio.Text);

                var cliente = ObterClienteColecaoCota().Where(p => p.CLIENTE != null && p.CLIENTE.Trim() != "");
                foreach (var c in cliente)
                {
                    var cota = atacController.ObterClienteCota(c.COLECAO.Trim(), c.CLIENTE.Trim());
                    if (cota != null)
                    {
                        cota.VALOR = (cota.VALOR + (cota.VALOR * (porcAumentoCota / 100)));
                        cota.QTDE = Convert.ToInt32(cota.VALOR / precoMedio);
                        atacController.AtualizarClienteCota(cota);
                    }
                }

                CarregarClienteColecaoOrdenado(hidSortExpression.Value, hidSortDirection.Value);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Session["CLIENTE_COL_COTA"] != null)
                //{
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "CLIENTE_COLECAO_" + DateTime.Today.ToString("yyyy-MM-dd") + ".xls"));
                Response.ContentType = "application/ms-excel";
                //Abaixo codifica os caracteres para o alfabeto latino
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");
                Response.Charset = "ISO-8859-1";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gvClienteColecao.AllowPaging = false;
                gvClienteColecao.PageSize = 10000;
                gvClienteColecao.DataSource = ObterClienteColecaoCota();
                gvClienteColecao.DataBind();

                gvClienteColecao.HeaderRow.Style.Add("background-color", "#FFFFFF");

                for (int i = 0; i < gvClienteColecao.HeaderRow.Cells.Count; i++)
                {
                    gvClienteColecao.HeaderRow.Cells[i].Attributes.Remove("href");
                    gvClienteColecao.HeaderRow.Cells[i].Style.Add("pointer-events", "none");
                    gvClienteColecao.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                    gvClienteColecao.HeaderRow.Cells[i].Style.Add("color", "#333333");
                }

                AlterarNomeColuna(gvClienteColecao.HeaderRow);

                gvClienteColecao.RenderControl(htw);
                Response.Write(sw.ToString().Replace("<a", "<p").Replace("</a>", "</p>"));
                Response.Flush();
                Response.End();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
            }
            catch (Exception ex)
            {
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void btCopiarSugestaoCota_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                //Incluir sugestoes de cota para clientes sem cota
                var cliCota = ObterClienteColecaoCota().Where(p => p.CLIENTE.Trim() != "");
                if (cliCota != null && cliCota.Count() > 0)
                {
                    ATACADO_REP_COTA n = null;
                    foreach (var c in cliCota)
                    {
                        var cc = atacController.ObterClienteCota(c.COLECAO.Trim(), c.CLIENTE.Trim());
                        if (cc != null)
                        {
                            //Atualizar cota valor
                            cc.QTDE = 0;
                            if (txtPrecoMedio.Text != "" && Convert.ToDecimal(txtPrecoMedio.Text) != 0)
                                cc.QTDE = Convert.ToInt32(c.COTA_VAL_SUGESTAO / Convert.ToDecimal(txtPrecoMedio.Text));
                            cc.VALOR = c.COTA_VAL_SUGESTAO;
                            atacController.AtualizarClienteCota(cc);
                        }
                        else
                        {
                            n = new ATACADO_REP_COTA();
                            n.COLECAO = c.COLECAO.Trim();
                            n.CLIENTE = c.CLIENTE.Trim();
                            n.QTDE = 0;
                            if (txtPrecoMedio.Text != "" && Convert.ToDecimal(txtPrecoMedio.Text) != 0)
                                n.QTDE = Convert.ToInt32(c.COTA_VAL_SUGESTAO / Convert.ToDecimal(txtPrecoMedio.Text));
                            n.VALOR = c.COTA_VAL_SUGESTAO;
                            atacController.InserirClienteCota(n);
                        }
                    }

                    //Recarregar cotas
                    var ccc = ObterClienteColecaoCota();
                    gvClienteColecao.DataSource = ccc;
                    gvClienteColecao.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }


    }
}
