using DAL;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class prod_def_logistica : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarDefinicaoLogistica("", "", "");

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();
            }
        }

        #endregion

        #region "LOGISTICA"
        private void CarregarDefinicaoLogistica(string colecao, string hb, string produto)
        {
            var prodSemCad = ObterDefinicaoGradeAtacado(colecao, hb, produto);

            gvLogistica.DataSource = prodSemCad;
            gvLogistica.DataBind();

        }
        protected void gvLogistica_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DEFINICAO_GRADE_ATACADOResult logistica = e.Row.DataItem as SP_OBTER_DEFINICAO_GRADE_ATACADOResult;

                    if (logistica != null)
                    {
                        Button _btDefinir = e.Row.FindControl("btDefinir") as Button;
                        if (_btDefinir != null)
                            _btDefinir.CommandArgument = logistica.CODIGO.ToString();

                        //Popular GRID VIEW FILHO
                        if (logistica.FOTO_PECA != null && logistica.FOTO_PECA.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<PROD_HB> _fotoProduto = new List<PROD_HB>();
                                _fotoProduto.Add(new PROD_HB { FOTO_PECA = logistica.FOTO_PECA });
                                gvFoto.DataSource = _fotoProduto;
                                gvFoto.DataBind();
                            }
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                            if (img != null)
                                img.Visible = false;
                        }
                    }
                }
            }
        }

        protected void gvFoto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _produto = e.Row.DataItem as PROD_HB;
                    if (_produto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _produto.FOTO_PECA;

                    }
                }
            }
        }
        #endregion

        #region "DEFINIR"
        protected void btDefinir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    string codigoHB = b.CommandArgument;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('prod_def_logistica_edit.aspx?c=" + codigoHB + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        #endregion

        private List<SP_OBTER_DEFINICAO_GRADE_ATACADOResult> ObterDefinicaoGradeAtacado(string colecao, string hb, string produto)
        {
            int? hbValue = null;
            if (hb != "")
                hbValue = Convert.ToInt32(hb);

            var logistica = prodController.ObterDefinicaoGradeAtacado(colecao, hbValue, produto);

            return logistica;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarDefinicaoLogistica(ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim(), txtProduto.Text.Trim().ToUpper());

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

    }
}

