using DAL;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using System.Drawing;
using System.IO;

namespace Relatorios
{
    public partial class desenv_controle_producao_calen_det : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        int qtdeVarejo = 0;
        int qtdeAtacado = 0;
        int qtdeTotal = 0;

        decimal valorFaccaoTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPrevIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPrevFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                //p=" + produto.Trim() + "&c=" + cor + "&di=" + dataIni + "&df=" + dataFim + "";


                if (Request.QueryString["co"] == null || Request.QueryString["co"] == "" ||
                    Request.QueryString["loc"] == null || Request.QueryString["loc"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("desenv_menu.aspx");

                var colecao = Request.QueryString["co"].ToString();
                var local = Request.QueryString["loc"].ToString();

                hidColecao.Value = colecao;
                ddlLocal.SelectedValue = local;

                var descColecao = baseController.BuscaColecaoAtual(colecao).DESC_COLECAO.Trim();
                txtColecao.Text = descColecao;

                CarregarGriffe();
                CarregarGrupoProduto();
                CarregarProdutos();
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INI"
        private void CarregarGrupoProduto()
        {
            var _grupo = (prodController.ObterGrupoProduto("01"));
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        #endregion

        #region "PRODUTOs"
        private List<VW_CALPRODUCAO> ObterProdutos()
        {
            var mod = new List<VW_CALPRODUCAO>();

            var local = ddlLocal.SelectedValue.ToLower();

            if (local == "pre-modelo")
                mod = ObterModCriacaoEEstampas(mod, 0);
            else if (local == "modelagem_de_criacao")
                mod = ObterModCriacaoEEstampas(mod, 1);
            else if (local == "design_de_estampas")
                mod = ObterModCriacaoEEstampas(mod, 2);
            else if (local == "modelagem_e_design")
                mod = ObterModCriacaoEEstampas(mod, 3);
            else if (local == "1ª_peca")
                mod = Obter1Peca(mod);
            else if (local == "modelagem")
                mod = ObterModelagem(mod);
            else if (local == "pre-risco")
                mod = ObterPreRisco(mod);
            else if (local == "risco")
                mod = ObterRisco(mod);
            else if (local == "corte")
                mod = ObterCorte(mod);
            else if (local == "encaixe_faccao")
                mod = ObterEnxaixeFaccao(mod, 20);
            else if (local == "faccao")
                mod = ObterFaccaoFal(mod, 1);
            else if (local == "encaixe_acabamento")
                mod = ObterEnxaixeFaccao(mod, 21);
            else if (local == "acabamento")
                mod = ObterFaccaoFal(mod, 4);


            if (ddlGriffe.SelectedValue != "")
                mod = mod.Where(p => p.GRIFFE == ddlGriffe.SelectedValue.Trim()).ToList();

            if (ddlGrupo.SelectedValue != "")
                mod = mod.Where(p => p.GRUPO_PRODUTO == ddlGrupo.SelectedValue.Trim()).ToList();

            if (txtDataPrevIni.Text.Trim() != "")
                mod = mod.Where(p => p.DATA_PREVISAO != null && p.DATA_PREVISAO >= Convert.ToDateTime(txtDataPrevIni.Text.Trim())).ToList();

            if (txtDataPrevFim.Text.Trim() != "")
                mod = mod.Where(p => p.DATA_PREVISAO != null && p.DATA_PREVISAO <= Convert.ToDateTime(txtDataPrevFim.Text.Trim())).ToList();

            if (ddl90Porc.SelectedValue != "")
                if (ddl90Porc.SelectedValue == "S")
                    mod = mod.Where(p => p.PORC_QTDE_FALT >= 10).ToList();

            return mod;
        }

        private List<VW_CALPRODUCAO> ObterModCriacaoEEstampas(List<VW_CALPRODUCAO> mod, int modDesign)
        {
            var m = desenvController.ObterCalProducaoMODESTAMPA(hidColecao.Value, modDesign);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = vw.QTDE_VAREJO,
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100,
                    FORNECEDOR = "-",
                    VALOR_TOT_FACCAO = 0
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> Obter1Peca(List<VW_CALPRODUCAO> mod)
        {
            var m = desenvController.ObterCalProducao1PECA(hidColecao.Value);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = vw.QTDE_VAREJO,
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100,
                    FORNECEDOR = "-",
                    VALOR_TOT_FACCAO = 0
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterModelagem(List<VW_CALPRODUCAO> mod)
        {
            var m = desenvController.ObterCalProducaoMODELAGEM(hidColecao.Value);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = vw.QTDE_VAREJO,
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100,
                    FORNECEDOR = "-",
                    VALOR_TOT_FACCAO = 0
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterPreRisco(List<VW_CALPRODUCAO> mod)
        {
            var m = desenvController.ObterCalProducaoPRERISCO(hidColecao.Value);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = Convert.ToInt32(vw.QTDE_VAREJO),
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100,
                    FORNECEDOR = "-",
                    VALOR_TOT_FACCAO = 0
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterRisco(List<VW_CALPRODUCAO> mod)
        {
            var m = desenvController.ObterCalProducaoRISCO(hidColecao.Value);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = Convert.ToInt32(vw.QTDE_VAREJO),
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100,
                    FORNECEDOR = "-",
                    VALOR_TOT_FACCAO = 0
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterCorte(List<VW_CALPRODUCAO> mod)
        {
            var m = desenvController.ObterCalProducaoCORTE(hidColecao.Value);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = Convert.ToInt32(vw.QTDE_VAREJO),
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = 100,
                    FORNECEDOR = "-",
                    VALOR_TOT_FACCAO = 0
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterEnxaixeFaccao(List<VW_CALPRODUCAO> mod, int codigoProcesso)
        {
            var m = desenvController.ObterCalProducaoEncaixeFACCAO(hidColecao.Value, codigoProcesso);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.LOCAL,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = vw.CODIGO_ORIGEM,
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB,
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = Convert.ToInt32(vw.QTDE_VAREJO),
                    QTDE_ATACADO = vw.QTDE_ATACADO,
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = Convert.ToDecimal(vw.PORC_QTDE_FALT),
                    FORNECEDOR = "-",
                    VALOR_TOT_FACCAO = vw.VALOR_TOT_FACCAO
                });
            }

            return mod;
        }
        private List<VW_CALPRODUCAO> ObterFaccaoFal(List<VW_CALPRODUCAO> mod, int codigoServico)
        {
            var m = desenvController.ObterCalProducaoFACCAOFAL(hidColecao.Value, codigoServico);
            foreach (var vw in m)
            {
                mod.Add(new VW_CALPRODUCAO
                {
                    LOCAL = vw.SERVICO,
                    COLECAO = vw.COLECAO,
                    CODIGO_ORIGEM = Convert.ToInt16(vw.CODIGO_ORIGEM),
                    ORIGEM = vw.ORIGEM,
                    PRODUTO = vw.PRODUTO,
                    HB = vw.HB.ToString(),
                    GRIFFE = vw.GRIFFE,
                    GRUPO_PRODUTO = vw.GRUPO_PRODUTO,
                    DESC_PRODUTO = vw.DESC_PRODUTO,
                    COR = vw.COR,
                    DESC_COR = vw.DESC_COR,
                    TECIDO = vw.TECIDO,
                    QTDE_VAREJO = Convert.ToInt32(vw.QTDE_VAREJO),
                    QTDE_ATACADO = Convert.ToInt32(vw.QTDE_ATACADO),
                    QTDE_TOTAL = Convert.ToInt32(vw.QTDE_TOTAL),
                    DATA_ENTRADA_MODULO = Convert.ToDateTime(vw.DATA_ENTRADA_MODULO),
                    TEMPO_MODULO_DIAS = Convert.ToInt32(vw.TEMPO_MODULO_DIAS),
                    DATA_PREVISAO = Convert.ToDateTime(vw.DATA_PREVISAO),
                    TEMPO_PREVISAO_DIAS = Convert.ToInt32(vw.TEMPO_PREVISAO_DIAS),
                    PORC_QTDE_FALT = Convert.ToDecimal(vw.PORC_QTDE_FALT),
                    FORNECEDOR = vw.FORNECEDOR_FACCAO,
                    VALOR_TOT_FACCAO = vw.PRECO_SERVICO
                });
            }

            return mod;
        }

        private void CarregarProdutos()
        {
            var produtos = ObterProdutos();


            gvProduto.DataSource = produtos.OrderBy("TEMPO_PREVISAO_DIAS ASC, TEMPO_MODULO_DIAS DESC");
            gvProduto.DataBind();
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    VW_CALPRODUCAO prod = e.Row.DataItem as VW_CALPRODUCAO;
                    if (prod != null)
                    {
                        var desenvProduto = desenvController.ObterProduto(prod.COLECAO, prod.PRODUTO, prod.COR);
                        if (desenvProduto != null)
                        {
                            prod.CODIGO_PRODUTO = desenvProduto.CODIGO;
                            prod.FOTO1 = "/Fotos/" + prod.PRODUTO.Trim() + prod.COR.Trim() + ".png";
                            prod.FOTO2 = "/Fotos/" + prod.PRODUTO.Trim() + prod.COR.Trim() + ".jpg";
                            prod.FOTO3 = desenvProduto.FOTO;
                        }

                        ImageButton _imgProduto = e.Row.FindControl("imgProduto") as ImageButton;
                        _imgProduto.CommandArgument = prod.CODIGO_PRODUTO.ToString();
                        if (File.Exists(Server.MapPath(prod.FOTO1)))
                            _imgProduto.ImageUrl = prod.FOTO1;
                        else if (File.Exists(Server.MapPath(prod.FOTO2)))
                            _imgProduto.ImageUrl = prod.FOTO2;
                        else if (File.Exists(Server.MapPath(prod.FOTO3)))
                            _imgProduto.ImageUrl = prod.FOTO3;
                        else
                            _imgProduto.ImageUrl = "/Fotos/sem_foto.png";

                        Literal litDataPrevisao = e.Row.FindControl("litDataPrevisao") as Literal;
                        litDataPrevisao.Text = Convert.ToDateTime(prod.DATA_PREVISAO).ToString("dd/MM/yyyy");

                        Literal litValorFaccaoTotal = e.Row.FindControl("litValorFaccaoTotal") as Literal;
                        litValorFaccaoTotal.Text = (prod.VALOR_TOT_FACCAO <= 0) ? "-" : prod.VALOR_TOT_FACCAO.ToString("###,###,###,##0.00");

                        if (prod.TEMPO_PREVISAO_DIAS < 0)
                            e.Row.Cells[0].BackColor = Color.LightCoral;

                        qtdeVarejo += prod.QTDE_VAREJO;
                        qtdeAtacado += prod.QTDE_ATACADO;
                        qtdeTotal += prod.QTDE_TOTAL;

                        valorFaccaoTotal += prod.VALOR_TOT_FACCAO;
                    }
                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProduto.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[9].Text = qtdeTotal.ToString();

                footer.Cells[14].Text = valorFaccaoTotal.ToString();

            }
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<VW_CALPRODUCAO> prods = ObterProdutos();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            prods = prods.OrderBy(e.SortExpression + sortDirection);
            gvProduto.DataSource = prods;
            gvProduto.DataBind();
        }
        protected void imgProduto_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    var codigoProduto = b.CommandArgument;
                    if (codigoProduto != "" && codigoProduto != "0")
                        _url = "fnAbrirTelaCadastroMaiorVert('../mod_desenvolvimento/desenv_tripa_view_foto.aspx?p=" + codigoProduto + "');";
                    else
                        return;

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

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarProdutos();
            }
            catch (Exception)
            {
            }
        }
    }
}


public class VW_CALPRODUCAO
{
    public int CODIGO_PRODUTO { get; set; }
    public string LOCAL { get; set; }
    public string COLECAO { get; set; }
    public int CODIGO_ORIGEM { get; set; }
    public string ORIGEM { get; set; }
    public string GRIFFE { get; set; }
    public string GRUPO_PRODUTO { get; set; }
    public string PRODUTO { get; set; }
    public string HB { get; set; }
    public string DESC_PRODUTO { get; set; }
    public string COR { get; set; }
    public string DESC_COR { get; set; }
    public string TECIDO { get; set; }
    public int QTDE_VAREJO { get; set; }
    public int QTDE_ATACADO { get; set; }
    public int QTDE_TOTAL { get; set; }
    public string FOTO1 { get; set; }
    public string FOTO2 { get; set; }
    public string FOTO3 { get; set; }
    public DateTime DATA_ENTRADA_MODULO { get; set; }
    public DateTime DATA_PREVISAO { get; set; }
    public int TEMPO_MODULO_DIAS { get; set; }
    public int TEMPO_PREVISAO_DIAS { get; set; }
    public decimal PORC_QTDE_FALT { get; set; }
    public string FORNECEDOR { get; set; }
    public decimal VALOR_TOT_FACCAO { get; set; }

}