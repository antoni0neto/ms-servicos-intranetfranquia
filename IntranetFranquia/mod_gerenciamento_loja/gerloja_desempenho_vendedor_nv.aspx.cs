using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class gerloja_desempenho_vendedor_nv : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        RHController rhController = new RHController();
        DesempenhoController desempenhoController = new DesempenhoController();

        List<VEND_TOTAL> vendTotal = new List<VEND_TOTAL>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDataAno();
                CarregarFilial();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            var usuario = (USUARIO)Session["USUARIO"];

            if (usuario.CODIGO_PERFIL == 3 || usuario.CODIGO_PERFIL == 1)
                filial = baseController.BuscaFiliais();
            else
                filial = baseController.BuscaFiliais(usuario);

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();

                ddlFilial.Enabled = true;
                if (filial.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }
        }
        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarFuncionario(ddlFilial.SelectedValue);
        }
        private void CarregarFuncionario(string filial)
        {
            var funcionario = rhController.ObterFuncionarioComCodigo(null, "", filial, "", "").Where(p => p.DATA_DEMISSAO == null).OrderBy(p => p.VENDEDOR_APELIDO).ToList();

            funcionario.Insert(0, new SP_OBTER_FUNCIONARIOResult { CODIGO = 0, NOME = "" });
            ddlFuncionario.DataSource = funcionario;
            ddlFuncionario.DataBind();

            if (ddlFuncionario.Items.Count == 2)
                ddlFuncionario.SelectedIndex = 1;
        }
        #endregion

        #region "VENDEDOR"

        private List<SP_OBTER_VENDA_CLIENTENVResult> ObterDesempenhoVendedorNV()
        {
            var vendedor = "";
            if (ddlFuncionario.SelectedValue != "0")
            {
                var funcionario = rhController.ObterFuncionario(Convert.ToInt32(ddlFuncionario.SelectedValue));
                if (funcionario != null)
                    vendedor = funcionario.VENDEDOR.Trim();
            }

            var nv = desempenhoController.ObterDesempenhoVendedorNV(ddlAno.SelectedValue, ddlFilial.SelectedValue, vendedor);

            return nv;
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlAno.SelectedValue == "0")
                {
                    labErro.Text = "Selecione o Ano.";
                    return;
                }
                if (ddlFilial.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Filial.";
                    return;
                }

                CarregarVendedor();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void CarregarVendedor()
        {

            for (int i = 1; i <= 13; i++)
                vendTotal.Add(new VEND_TOTAL { ATENDIMENTO = 0, NOVO = 0, RECORRENTE = 0 });

            gvVendedor.DataSource = ObterDesempenhoVendedorNV();
            gvVendedor.DataBind();
        }
        protected void gvVendedor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDA_CLIENTENVResult nv = e.Row.DataItem as SP_OBTER_VENDA_CLIENTENVResult;

                    if (nv != null)
                    {
                        Literal litNomeVendedor = e.Row.FindControl("litNomeVendedor") as Literal;
                        litNomeVendedor.Text = CriarLinkTicket(nv.VENDEDOR, nv.NOME_VENDEDOR);


                        Literal litNV_JANEIRO_PORC = e.Row.FindControl("litNV_JANEIRO_PORC") as Literal;
                        litNV_JANEIRO_PORC.Text = nv.NV_JANEIRO_PORC.ToString("###,##0.00");
                        Literal litRC_JANEIRO_PORC = e.Row.FindControl("litRC_JANEIRO_PORC") as Literal;
                        litRC_JANEIRO_PORC.Text = nv.RC_JANEIRO_PORC.ToString("###,##0.00");

                        vendTotal[0].ATENDIMENTO += nv.AT_JANEIRO;
                        vendTotal[0].NOVO += nv.NV_JANEIRO;
                        vendTotal[0].RECORRENTE += nv.RC_JANEIRO;

                        Literal litNV_FEVEREIRO_PORC = e.Row.FindControl("litNV_FEVEREIRO_PORC") as Literal;
                        litNV_FEVEREIRO_PORC.Text = nv.NV_FEVEREIRO_PORC.ToString("###,##0.00");
                        Literal litRC_FEVEREIRO_PORC = e.Row.FindControl("litRC_FEVEREIRO_PORC") as Literal;
                        litRC_FEVEREIRO_PORC.Text = nv.RC_FEVEREIRO_PORC.ToString("###,##0.00");

                        vendTotal[1].ATENDIMENTO += nv.AT_FEVEREIRO;
                        vendTotal[1].NOVO += nv.NV_FEVEREIRO;
                        vendTotal[1].RECORRENTE += nv.RC_FEVEREIRO;

                        Literal litNV_MARCO_PORC = e.Row.FindControl("litNV_MARCO_PORC") as Literal;
                        litNV_MARCO_PORC.Text = nv.NV_MARCO_PORC.ToString("###,##0.00");
                        Literal litRC_MARCO_PORC = e.Row.FindControl("litRC_MARCO_PORC") as Literal;
                        litRC_MARCO_PORC.Text = nv.RC_MARCO_PORC.ToString("###,##0.00");

                        vendTotal[2].ATENDIMENTO += nv.AT_MARCO;
                        vendTotal[2].NOVO += nv.NV_MARCO;
                        vendTotal[2].RECORRENTE += nv.RC_MARCO;

                        Literal litNV_ABRIL_PORC = e.Row.FindControl("litNV_ABRIL_PORC") as Literal;
                        litNV_ABRIL_PORC.Text = nv.NV_ABRIL_PORC.ToString("###,##0.00");
                        Literal litRC_ABRIL_PORC = e.Row.FindControl("litRC_ABRIL_PORC") as Literal;
                        litRC_ABRIL_PORC.Text = nv.RC_ABRIL_PORC.ToString("###,##0.00");

                        vendTotal[3].ATENDIMENTO += nv.AT_ABRIL;
                        vendTotal[3].NOVO += nv.NV_ABRIL;
                        vendTotal[3].RECORRENTE += nv.RC_ABRIL;

                        Literal litNV_MAIO_PORC = e.Row.FindControl("litNV_MAIO_PORC") as Literal;
                        litNV_MAIO_PORC.Text = nv.NV_MAIO_PORC.ToString("###,##0.00");
                        Literal litRC_MAIO_PORC = e.Row.FindControl("litRC_MAIO_PORC") as Literal;
                        litRC_MAIO_PORC.Text = nv.RC_MAIO_PORC.ToString("###,##0.00");

                        vendTotal[4].ATENDIMENTO += nv.AT_MAIO;
                        vendTotal[4].NOVO += nv.NV_MAIO;
                        vendTotal[4].RECORRENTE += nv.RC_MAIO;

                        Literal litNV_JUNHO_PORC = e.Row.FindControl("litNV_JUNHO_PORC") as Literal;
                        litNV_JUNHO_PORC.Text = nv.NV_JUNHO_PORC.ToString("###,##0.00");
                        Literal litRC_JUNHO_PORC = e.Row.FindControl("litRC_JUNHO_PORC") as Literal;
                        litRC_JUNHO_PORC.Text = nv.RC_JUNHO_PORC.ToString("###,##0.00");

                        vendTotal[5].ATENDIMENTO += nv.AT_JUNHO;
                        vendTotal[5].NOVO += nv.NV_JUNHO;
                        vendTotal[5].RECORRENTE += nv.RC_JUNHO;

                        Literal litNV_JULHO_PORC = e.Row.FindControl("litNV_JULHO_PORC") as Literal;
                        litNV_JULHO_PORC.Text = nv.NV_JULHO_PORC.ToString("###,##0.00");
                        Literal litRC_JULHO_PORC = e.Row.FindControl("litRC_JULHO_PORC") as Literal;
                        litRC_JULHO_PORC.Text = nv.RC_JULHO_PORC.ToString("###,##0.00");

                        vendTotal[6].ATENDIMENTO += nv.AT_JULHO;
                        vendTotal[6].NOVO += nv.NV_JULHO;
                        vendTotal[6].RECORRENTE += nv.RC_JULHO;

                        Literal litNV_AGOSTO_PORC = e.Row.FindControl("litNV_AGOSTO_PORC") as Literal;
                        litNV_AGOSTO_PORC.Text = nv.NV_AGOSTO_PORC.ToString("###,##0.00");
                        Literal litRC_AGOSTO_PORC = e.Row.FindControl("litRC_AGOSTO_PORC") as Literal;
                        litRC_AGOSTO_PORC.Text = nv.RC_AGOSTO_PORC.ToString("###,##0.00");

                        vendTotal[7].ATENDIMENTO += nv.AT_AGOSTO;
                        vendTotal[7].NOVO += nv.NV_AGOSTO;
                        vendTotal[7].RECORRENTE += nv.RC_AGOSTO;

                        Literal litNV_SETEMBRO_PORC = e.Row.FindControl("litNV_SETEMBRO_PORC") as Literal;
                        litNV_SETEMBRO_PORC.Text = nv.NV_SETEMBRO_PORC.ToString("###,##0.00");
                        Literal litRC_SETEMBRO_PORC = e.Row.FindControl("litRC_SETEMBRO_PORC") as Literal;
                        litRC_SETEMBRO_PORC.Text = nv.RC_SETEMBRO_PORC.ToString("###,##0.00");

                        vendTotal[8].ATENDIMENTO += nv.AT_SETEMBRO;
                        vendTotal[8].NOVO += nv.NV_SETEMBRO;
                        vendTotal[8].RECORRENTE += nv.RC_SETEMBRO;

                        Literal litNV_OUTUBRO_PORC = e.Row.FindControl("litNV_OUTUBRO_PORC") as Literal;
                        litNV_OUTUBRO_PORC.Text = nv.NV_OUTUBRO_PORC.ToString("###,##0.00");
                        Literal litRC_OUTUBRO_PORC = e.Row.FindControl("litRC_OUTUBRO_PORC") as Literal;
                        litRC_OUTUBRO_PORC.Text = nv.RC_OUTUBRO_PORC.ToString("###,##0.00");

                        vendTotal[9].ATENDIMENTO += nv.AT_OUTUBRO;
                        vendTotal[9].NOVO += nv.NV_OUTUBRO;
                        vendTotal[9].RECORRENTE += nv.RC_OUTUBRO;

                        Literal litNV_NOVEMBRO_PORC = e.Row.FindControl("litNV_NOVEMBRO_PORC") as Literal;
                        litNV_NOVEMBRO_PORC.Text = nv.NV_NOVEMBRO_PORC.ToString("###,##0.00");
                        Literal litRC_NOVEMBRO_PORC = e.Row.FindControl("litRC_NOVEMBRO_PORC") as Literal;
                        litRC_NOVEMBRO_PORC.Text = nv.RC_NOVEMBRO_PORC.ToString("###,##0.00");

                        vendTotal[10].ATENDIMENTO += nv.AT_NOVEMBRO;
                        vendTotal[10].NOVO += nv.NV_NOVEMBRO;
                        vendTotal[10].RECORRENTE += nv.RC_NOVEMBRO;

                        Literal litNV_DEZEMBRO_PORC = e.Row.FindControl("litNV_DEZEMBRO_PORC") as Literal;
                        litNV_DEZEMBRO_PORC.Text = nv.NV_DEZEMBRO_PORC.ToString("###,##0.00");
                        Literal litRC_DEZEMBRO_PORC = e.Row.FindControl("litRC_DEZEMBRO_PORC") as Literal;
                        litRC_DEZEMBRO_PORC.Text = nv.RC_DEZEMBRO_PORC.ToString("###,##0.00");

                        vendTotal[11].ATENDIMENTO += nv.AT_DEZEMBRO;
                        vendTotal[11].NOVO += nv.NV_DEZEMBRO;
                        vendTotal[11].RECORRENTE += nv.RC_DEZEMBRO;

                        Literal litNV_TOTAL_PORC = e.Row.FindControl("litNV_TOTAL_PORC") as Literal;
                        litNV_TOTAL_PORC.Text = nv.NV_TOTAL_PORC.ToString("###,##0.00");
                        Literal litRC_TOTAL_PORC = e.Row.FindControl("litRC_TOTAL_PORC") as Literal;
                        litRC_TOTAL_PORC.Text = nv.RC_TOTAL_PORC.ToString("###,##0.00");

                        vendTotal[12].ATENDIMENTO += nv.AT_TOTAL;
                        vendTotal[12].NOVO += nv.NV_TOTAL;
                        vendTotal[12].RECORRENTE += nv.RC_TOTAL;

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }

        private string CriarLinkTicket(string vendedor, string nomeVendedor)
        {
            var link = "gerloja_desempenho_vendedor_nvtkt.aspx?p=" + vendedor.Trim();
            var linkOk = "<a class='adre' href=\"javascript: openwindow('" + link + "')\">" + nomeVendedor + "</a>";
            return linkOk;
        }

        protected void gvVendedor_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvVendedor.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                int ind = 3;
                for (int i = 0; i < 13; i++)
                {
                    footer.Cells[ind].Text = vendTotal[i].ATENDIMENTO.ToString();
                    footer.Cells[ind + 1].Text = vendTotal[i].NOVO.ToString();
                    footer.Cells[ind + 2].Text = (vendTotal[i].ATENDIMENTO <= 0) ? "0.00%" : (vendTotal[i].NOVO * 1.00 / vendTotal[i].ATENDIMENTO * 100.00).ToString("###,##0.00") + "%";
                    footer.Cells[ind + 3].Text = vendTotal[i].RECORRENTE.ToString();
                    footer.Cells[ind + 4].Text = (vendTotal[i].ATENDIMENTO <= 0) ? "0.00%" : (vendTotal[i].RECORRENTE * 1.00 / vendTotal[i].ATENDIMENTO * 100.00).ToString("###,##0.00") + "%";
                    ind = ind + 5;
                }

                ///*Fevereiro*/
                //footer.Cells[8].Text = vendTotal[1].ATENDIMENTO.ToString();
                //footer.Cells[9].Text = vendTotal[1].NOVO.ToString();
                //footer.Cells[10].Text = (vendTotal[1].NOVO * 1.00 / vendTotal[1].ATENDIMENTO).ToString("###,##0.00") + "%";
                //footer.Cells[11].Text = vendTotal[1].RECORRENTE.ToString();
                //footer.Cells[12].Text = (vendTotal[1].RECORRENTE * 1.00 / vendTotal[1].ATENDIMENTO).ToString("###,##0.00") + "%";

                ///*Março*/
                //footer.Cells[13].Text = vendTotal[2].ATENDIMENTO.ToString();
                //footer.Cells[14].Text = vendTotal[2].NOVO.ToString();
                //footer.Cells[15].Text = (vendTotal[2].NOVO * 1.00 / vendTotal[2].ATENDIMENTO).ToString("###,##0.00") + "%";
                //footer.Cells[16].Text = vendTotal[2].RECORRENTE.ToString();
                //footer.Cells[17].Text = (vendTotal[2].RECORRENTE * 1.00 / vendTotal[2].ATENDIMENTO).ToString("###,##0.00") + "%";

                ///*Abril*/
                //footer.Cells[18].Text = "";
                //footer.Cells[19].Text = "";
                //footer.Cells[20].Text = "";
                //footer.Cells[21].Text = "";
                //footer.Cells[22].Text = "";

                ///*Maio*/
                //footer.Cells[23].Text = "";
                //footer.Cells[24].Text = "";
                //footer.Cells[25].Text = "";
                //footer.Cells[26].Text = "";
                //footer.Cells[27].Text = "";

                ///*Junho*/
                //footer.Cells[28].Text = "";
                //footer.Cells[29].Text = "";
                //footer.Cells[30].Text = "";
                //footer.Cells[31].Text = "";
                //footer.Cells[32].Text = "";

                ///*Julho*/
                //footer.Cells[33].Text = "";
                //footer.Cells[34].Text = "";
                //footer.Cells[35].Text = "";
                //footer.Cells[36].Text = "";
                //footer.Cells[37].Text = "";

                ///*Agosto*/
                //footer.Cells[38].Text = "";
                //footer.Cells[39].Text = "";
                //footer.Cells[40].Text = "";
                //footer.Cells[41].Text = "";
                //footer.Cells[42].Text = "";

                ///*Setembro*/
                //footer.Cells[43].Text = "";
                //footer.Cells[44].Text = "";
                //footer.Cells[45].Text = "";
                //footer.Cells[46].Text = "";
                //footer.Cells[47].Text = "";

                ///*Outubro*/
                //footer.Cells[48].Text = "";
                //footer.Cells[49].Text = "";
                //footer.Cells[50].Text = "";
                //footer.Cells[51].Text = "";
                //footer.Cells[52].Text = "";

                ///*Novembro*/
                //footer.Cells[53].Text = "";
                //footer.Cells[54].Text = "";
                //footer.Cells[55].Text = "";
                //footer.Cells[56].Text = "";
                //footer.Cells[57].Text = "";

                ///*Dezembro*/
                //footer.Cells[58].Text = "";
                //footer.Cells[59].Text = "";
                //footer.Cells[60].Text = "";
                //footer.Cells[61].Text = "";
                //footer.Cells[62].Text = "";

                ///*Total*/
                //footer.Cells[63].Text = "";
                //footer.Cells[64].Text = "";
                //footer.Cells[65].Text = "";
                //footer.Cells[66].Text = "";
                //footer.Cells[67].Text = "X";


            }
        }
        protected void gvVendedor_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_VENDA_CLIENTENVResult> nv = ObterDesempenhoVendedorNV();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(gvVendedor, e, out sort);

            if (sort == SortDirection.Ascending)
            {
                Utils.WebControls.GetBoundFieldIndexByName(gvVendedor, e.SortExpression, " - >>");
            }
            else
            {
                Utils.WebControls.GetBoundFieldIndexByName(gvVendedor, e.SortExpression, " - <<");
            }

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            nv = nv.OrderBy(e.SortExpression + sortDirection);
            gvVendedor.DataSource = nv;
            gvVendedor.DataBind();

        }

        #endregion




    }
}
class VEND_TOTAL
{
    public int ATENDIMENTO { get; set; }
    public int NOVO { get; set; }
    public int RECORRENTE { get; set; }
}