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

namespace Relatorios
{
    public partial class prod_con_hb_filtro : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                //Valida queryString
                if (Request.QueryString["d"] == null || Request.QueryString["d"] == "" ||
                    Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("prod_menu.aspx");
                }

                string detalhe = Request.QueryString["d"].ToString();
                string tela = Request.QueryString["t"].ToString();

                if (detalhe != "1" && detalhe != "2")
                    Response.Redirect("prod_menu.aspx");

                if (detalhe == "1")
                {
                    labTitulo.Text = "HB Consulta";
                    labTituloMenu.Text = "HB Consulta";
                }
                else
                {
                    labTitulo.Text = "HB Consulta Detalhe";
                    labTituloMenu.Text = "HB Consulta Detalhe";
                }

                if (tela == "1")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";
                else if (tela == "3")
                    hrefVoltar.HRef = "../mod_faccao/facc_menu.aspx";
                else if (tela == "4")
                    hrefVoltar.HRef = "../mod_producao/prod_menu_cad.aspx";

                hidTela.Value = tela;
                hidDetalhe.Value = detalhe;
                CarregarColecoes();

                //Controle do botão voltar da tela de consulta detalhes
                string col = "";
                if (Request.QueryString["col"] != null && Request.QueryString["col"] != "")
                    col = Request.QueryString["col"].ToString();

                string h = "";
                if (Request.QueryString["h"] != null && Request.QueryString["h"] != "")
                    h = Request.QueryString["h"].ToString();

                //Se esta voltando da consulta de detalhes, recarrega o grid
                if (col != "" && h != "" && tela == "2")
                {
                    ddlColecoesBuscar.SelectedValue = col;
                    txtHBBuscar.Text = h;
                    btHBBuscar_Click(null, null);
                }
            }
        }

        protected void btHBBuscar_Click(object sender, EventArgs e)
        {
            labHBBuscar.Text = "";
            if (ddlColecoesBuscar.SelectedValue == "" || ddlColecoesBuscar.SelectedValue == "0")
            {
                labHBBuscar.Text = "Selecione a Coleção.";
                return;
            }

            if ((txtHBBuscar.Text == "" || Convert.ToInt32(txtHBBuscar.Text) <= 0) && txtNomeBuscar.Text.Trim() == "")
            {
                labHBBuscar.Text = "Informe o número do HB e/ou NOME.";
                return;
            }

            try
            {
                List<PROD_HB> lstProdHB = new List<PROD_HB>();
                if (txtHBBuscar.Text.Trim() != "")
                    lstProdHB = prodController.ObterNumeroHB(ddlColecoesBuscar.SelectedValue, Convert.ToInt32(txtHBBuscar.Text));
                else
                    lstProdHB = prodController.ObterHB().Where(p => p.COLECAO.Trim() == ddlColecoesBuscar.SelectedValue.Trim()
                        && p.NOME.Contains(txtNomeBuscar.Text.Trim().ToUpper())).ToList();

                PROD_HB prod_hb = null;
                prod_hb = lstProdHB.Where(p => p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).OrderBy(x => x.STATUS).Take(1).SingleOrDefault();

                if (prod_hb == null)
                {
                    labHBBuscar.Text = "Nenhum HB encontrato. Refaça a sua pesquisa.";
                }
                if (prod_hb != null && prod_hb.STATUS.ToString() == "X")
                {
                    labHBBuscar.Text = "Nenhum HB encontrato. Refaça a sua pesquisa.";
                }
                if (prod_hb != null && prod_hb.STATUS.ToString() == "E")
                {
                    labHBBuscar.Text = "Este HB foi EXCLUÍDO. Refaça a sua pesquisa.";
                }

                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;

                if (labHBBuscar.Text != "")
                {
                    gvHB.DataSource = null;
                }
                else
                {

                    List<PROD_HB> _listaHB = new List<PROD_HB>();
                    if (txtHBBuscar.Text.Trim() != "")
                        _listaHB = prodController.ObterNumeroHB(ddlColecoesBuscar.SelectedValue, Convert.ToInt32(txtHBBuscar.Text));
                    else
                        _listaHB = prodController.ObterHB().Where(p => p.COLECAO.Trim() == ddlColecoesBuscar.SelectedValue.Trim()
                        && p.NOME.Contains(txtNomeBuscar.Text.Trim().ToUpper())).ToList();

                    if (hidDetalhe.Value == "2")
                    {
                        List<PROD_HB> _listaHBDetalhe = new List<PROD_HB>();
                        foreach (PROD_HB p in _listaHB)
                            _listaHBDetalhe.AddRange(prodController.ObterDetalhesHB(p.CODIGO));

                        //Zera lista e adiciona os detalhes
                        _listaHB = new List<PROD_HB>();
                        _listaHB = _listaHBDetalhe.OrderBy(p => p.HB).ThenBy(l => l.PROD_DETALHE1.DESCRICAO).ThenBy(x => x.NOME).ToList();
                    }

                    gvHB.DataSource = _listaHB.Where(p => p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N'));
                }

                gvHB.DataBind();

            }
            catch (Exception ex)
            {
                labHBBuscar.Text = "ERRO: " + ex.Message;
            }
        }
        protected void btHB_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    if (hidDetalhe.Value == "1")
                        Response.Redirect("prod_con_hb.aspx?t=" + hidTela.Value + "&c=" + Convert.ToInt32(b.CommandArgument).ToString());
                    else
                        Response.Redirect("prod_con_hb_det.aspx?t=" + hidTela.Value + "&c=" + Convert.ToInt32(b.CommandArgument).ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        protected void gvHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _hb = e.Row.DataItem as PROD_HB;

                    coluna += 1;
                    if (_hb != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _det = e.Row.FindControl("litDetalhe") as Literal;
                        if (_det != null)
                            _det.Text = (_hb.PROD_DETALHE1 == null) ? "PRINCIPAL" : _hb.PROD_DETALHE1.DESCRICAO;

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                            _litNome.Text = _hb.NOME;

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(_hb.COR).DESC_COR;

                        Literal _litMostruario = e.Row.FindControl("litMostruario") as Literal;
                        if (_litMostruario != null)
                            _litMostruario.Text = (_hb.MOSTRUARIO == null || _hb.MOSTRUARIO == ' ') ? "-" : (_hb.MOSTRUARIO == 'S') ? "Sim" : "Não";

                        Button _btHB = e.Row.FindControl("btHB") as Button;
                        if (_btHB != null)
                            _btHB.CommandArgument = _hb.CODIGO.ToString();
                    }
                }
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoesBuscar.DataSource = _colecoes;
                ddlColecoesBuscar.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoesBuscar.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        #endregion
    }
}
