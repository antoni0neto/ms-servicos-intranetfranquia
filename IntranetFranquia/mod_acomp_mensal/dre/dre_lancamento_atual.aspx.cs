using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class dre_lancamento_atual : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        DREController dreController = new DREController();
        ContabilidadeController contabilController = new ContabilidadeController();

        decimal totDebito = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDataAno();
                CarregarFilial();
                CarregarContaContabil();
                CarregarCentroCusto();
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
            List<FILIAI1> filial = new List<FILIAI1>();
            //List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.ListarFiliais();
            //filialDePara = baseController.BuscaFilialDePara();

            //filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI1 { COD_FILIAL = "", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        private void CarregarContaContabil()
        {
            ddlContaContabil.DataSource = contabilController.ObterContasFiltroOrdemNome("3.%", "", "", "", "", "");

            ddlContaContabil.DataBind();
            ddlContaContabil.Items.Insert(0, new ListItem("", ""));
        }
        private void CarregarCentroCusto()
        {
            var centroCusto = dreController.ObterCentroCusto().Where(p => !p.INATIVA).ToList();
            if (centroCusto != null)
            {
                centroCusto.Insert(0, new CTB_CENTRO_CUSTO { CENTRO_CUSTO = "", DESC_CENTRO_CUSTO = "" });

                ddlCCusto.DataSource = centroCusto;
                ddlCCusto.DataBind();
            }
        }
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labAno.ForeColor = _OK;
            labMes.ForeColor = _OK;
            if (txtLancamento.Text.Trim() == "")
            {

                if (ddlAno.SelectedValue.Trim() == "")
                {
                    labAno.ForeColor = _notOK;
                    retorno = false;
                }
                if (ddlMes.SelectedValue.Trim() == "")
                {
                    labMes.ForeColor = _notOK;
                    retorno = false;
                }
            }
            return retorno;
        }
        #endregion

        #region "LANCAMENTOS"
        private List<SP_OBTER_DRE_LANCAMENTOResult> ObterLancamento()
        {
            DateTime? dataIni = null;
            DateTime? dataFim = null;
            string tipo = ddlTipo.SelectedValue;
            string codigoFilial = ddlFilial.SelectedValue;
            string contaContabil = ddlContaContabil.SelectedValue;
            string centroCusto = ddlCCusto.SelectedValue;
            int? lancamento = null;

            string ano = ddlAno.SelectedValue;
            string mes = ddlMes.SelectedValue;

            if (txtLancamento.Text.Trim() != "")
            {
                lancamento = Convert.ToInt32(txtLancamento.Text.Trim());
            }
            else
            {
                dataIni = Convert.ToDateTime(ano + "-" + mes + "-01");
                dataFim = Convert.ToDateTime(ano + "-" + mes + "-" + DateTime.DaysInMonth(Convert.ToInt32(ano), Convert.ToInt32(mes)).ToString());
            }

            return dreController.ObterDRELancamento(dataIni, dataFim, tipo, codigoFilial, contaContabil, centroCusto, lancamento);
        }
        private void CarregarLancamento()
        {
            gvLancamento.DataSource = ObterLancamento();
            gvLancamento.DataBind();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho.";
                    return;
                }

                CarregarLancamento();

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void gvLancamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DRE_LANCAMENTOResult lanc = e.Row.DataItem as SP_OBTER_DRE_LANCAMENTOResult;

                    if (lanc != null)
                    {
                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = Convert.ToDateTime(lanc.DATA_LANCAMENTO).ToString("dd/MM/yyyy");

                        Literal _litDebito = e.Row.FindControl("litDebito") as Literal;
                        if (_litDebito != null)
                            _litDebito.Text = "R$ " + Convert.ToDecimal(lanc.DEBITO).ToString("###,###,###,##0.00");

                        totDebito += Convert.ToDecimal(lanc.DEBITO);

                    }
                }
            }
        }
        protected void gvLancamento_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvLancamento.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";


                footer.Cells[9].Text = "R$ " + totDebito.ToString("###,###,###,##0.00");

            }
        }
        #endregion

        protected void btEditar_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    string lancamento = "";
                    string item = "";

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        lancamento = ((Literal)row.FindControl("litLancamento")).Text.Trim();
                        item = ((Literal)row.FindControl("litItem")).Text.Trim();

                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('dre_lancamento_atual_edit.aspx?l=" + lancamento + "&i=" + item + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

        protected void gvLancamento_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_DRE_LANCAMENTOResult> _lanc = ObterLancamento();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _lanc = _lanc.OrderBy(e.SortExpression + sortDirection);
            gvLancamento.DataSource = _lanc;
            gvLancamento.DataBind();
        }

    }
}
