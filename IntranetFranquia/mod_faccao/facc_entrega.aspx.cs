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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Globalization;


namespace Relatorios
{
    public partial class facc_entrega : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "facc_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                if (tela == "3")
                    hrefVoltar.HRef = "facc_menu_relatorio.aspx";

                CarregarFornecedores();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_FACCAO_ENTREGAResult> ObterFaccaoEntrega()
        {
            return faccController.ObterFaccaoEntrega("", null, 'N', ddlFornecedor.SelectedValue.Trim()).Where(p =>
                                                                                                p.VOL_FAL > 0 || p.QTDE_DET > 0).OrderBy(p =>
                                                                                                                                                p.FORNECEDOR).ThenBy(o => o.COLECAO).ThenBy(u => u.HB).ToList();
        }
        private void CarregarFaccaoEntrega()
        {
            var entrega = ObterFaccaoEntrega();

            gvFaccaoEntrega.DataSource = entrega;
            gvFaccaoEntrega.DataBind();

            if (entrega == null || entrega.Count() <= 0)
                labErro.Text = "Nenhum registro encontrado. Refaça sua pesquisa.";
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                /*if (ddlFornecedor.SelectedValue.Trim() == "")
                {
                    labErro.Text = "Selecione o Fornecedor.";
                    return;
                }*/

                CarregarFaccaoEntrega();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvFaccaoEntrega_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_ENTREGAResult faccaoEntrega = e.Row.DataItem as SP_OBTER_FACCAO_ENTREGAResult;

                    if (faccaoEntrega != null)
                    {

                        if (faccaoEntrega.VOL_FAL <= 0)
                            ((TextBox)e.Row.FindControl("txtVolume")).Enabled = false;

                        if (faccaoEntrega.QTDE_DET > 0)
                            e.Row.BackColor = Color.LightGray;

                        Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (_btBaixar != null)
                        {
                            _btBaixar.CommandArgument = faccaoEntrega.CODIGO_SAIDA.ToString();

                            if (faccaoEntrega.VOL_FAL <= 0)
                                _btBaixar.Enabled = false;
                        }
                    }
                }
            }
        }
        protected void btBaixar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        int codigoSaida = Convert.ToInt32(b.CommandArgument);
                        string fornecedor = ((Label)row.FindControl("labFornecedor")).Text.Trim();

                        TextBox _txtVolume = ((TextBox)row.FindControl("txtVolume"));
                        if (_txtVolume.Text == "" || (Convert.ToInt32(_txtVolume.Text) > Convert.ToInt32(row.Cells[6].Text)))
                        {
                            labErro.Text = "Quantidade do VOLUME não pode ser MAIOR.";
                            msg = "Quantidade do VOLUME não pode ser MAIOR.";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'AVISO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        int qtdeVolume = 0;
                        PROD_HB_SAIDA_VOLUME _volumeSaida = null;

                        qtdeVolume = Convert.ToInt32(_txtVolume.Text);

                        _volumeSaida = new PROD_HB_SAIDA_VOLUME();
                        _volumeSaida.PROD_HB_SAIDA = codigoSaida;
                        _volumeSaida.VOLUME = qtdeVolume;
                        _volumeSaida.FORNECEDOR = fornecedor;
                        _volumeSaida.DATA_BAIXA_VOLUME = DateTime.Now;
                        _volumeSaida.USUARIO_BAIXA_VOLUME = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        faccController.InserirVolumeSaida(_volumeSaida);

                        CarregarFaccaoEntrega();
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFornecedores()
        {

            List<PROD_FORNECEDOR> _fornecedores = new ProducaoController().ObterFornecedor().Where(p => p.CODIGO != 106 && p.CODIGO != 94).OrderBy(p => p.FORNECEDOR).ToList();

            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });

                ddlFornecedor.DataSource = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'S') || p.STATUS == 'S');
                ddlFornecedor.DataBind();
            }

        }
        #endregion

    }
}
