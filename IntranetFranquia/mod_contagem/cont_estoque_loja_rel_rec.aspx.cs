using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;
using System.Data.OleDb;

namespace Relatorios
{
    public partial class cont_estoque_loja_rel_rec : System.Web.UI.Page
    {
        ContagemController contController = new ContagemController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                CarregarFilial();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = baseController.BuscaFiliais(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").ToList();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                        ddlFilial.SelectedIndex = 1;


                }
            }
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";

            try
            {
                CarregarrRecontagemFisica();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void CarregarrRecontagemFisica()
        {
            var est = contController.ObterEstoqueLojaContagem().Where(p => p.CODIGO_PAI != null);

            if (ddlFilial.SelectedValue != "")
                est = est.Where(p => p.CODIGO_FILIAL == ddlFilial.SelectedValue).ToList();

            if (txtDataIni.Text != "")
                est = est.Where(p => p.DATA_CONTAGEM >= Convert.ToDateTime(txtDataIni.Text)).ToList();

            if (txtDataFim.Text != "")
                est = est.Where(p => p.DATA_CONTAGEM <= Convert.ToDateTime(txtDataFim.Text)).ToList();


            gvContagemFisica.DataSource = est;
            gvContagemFisica.DataBind();
        }
        protected void gvContagemFisica_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ESTOQUE_LOJA_CONT estoqueLojaCont = e.Row.DataItem as ESTOQUE_LOJA_CONT;

                    if (estoqueLojaCont != null)
                    {

                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = baseController.ObterFilialIntranet(estoqueLojaCont.CODIGO_FILIAL).FILIAL;

                        Literal _litDataContagem = e.Row.FindControl("litDataContagem") as Literal;
                        if (_litDataContagem != null)
                            _litDataContagem.Text = estoqueLojaCont.DATA_CONTAGEM.ToString("dd/MM/yyyy");

                        Button _btAbrir = e.Row.FindControl("btAbrir") as Button;
                        if (_btAbrir != null)
                            _btAbrir.CommandArgument = estoqueLojaCont.CODIGO.ToString();

                    }
                }
            }
        }


        protected void btAbrirRelatorio_Click(object sender, EventArgs e)
        {
            string url = "";
            string msg = "";
            try
            {
                Button _bt = (Button)sender;
                if (_bt != null)
                {


                    string codigoEstoqueLojaCont = _bt.CommandArgument;

                    //Abrir pop-up
                    url = "fnAbrirTelaCadastroMaior('cont_estoque_loja_rel_rec_detalhes.aspx?p=" + codigoEstoqueLojaCont + "&e=0');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
                }
            }
            catch (Exception)
            {
                msg = "Erro ao abrir o Relatório.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }
        }

    }
}
