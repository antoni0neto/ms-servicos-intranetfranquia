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

namespace Relatorios
{
    public partial class prod_rel_produto_acortar : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int coluna, qtde_total = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_PRODUTO_ACORTARResult> ObterProdutoACortar()
        {
            int? origem = null;

            if (ddlOrigem.SelectedValue != "0" && ddlOrigem.SelectedValue != "")
                origem = Convert.ToInt32(ddlOrigem.SelectedValue);

            var produtoACortar = prodController.ObterProdutoACortar(ddlColecoes.SelectedValue.Trim(), origem, ddlGrupo.SelectedValue.Trim(), txtModelo.Text.Trim(), txtNome.Text.Trim());
            Session["PRODUTOACORTAR"] = produtoACortar;

            return produtoACortar;
        }

        #region "PRODUTOS"

        private void CarregarProdutos()
        {
            gvProduto.DataSource = ObterProdutoACortar();
            gvProduto.DataBind();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (ddlColecoes.SelectedValue.Trim() == "" && (ddlOrigem.SelectedValue == "0" || ddlOrigem.SelectedValue == "") && ddlGrupo.SelectedValue.Trim() == "" && txtModelo.Text.Trim() == "" && txtNome.Text.Trim() == "")
                {
                    labErro.Text = "Informe pelo menos um Filtro.";
                    return;
                }

                CarregarProdutos();

                if (ddlColecoes.SelectedValue.Trim() != "")
                    Session["COLECAO"] = ddlColecoes.SelectedValue;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_ACORTARResult _produtoACortar = e.Row.DataItem as SP_OBTER_PRODUTO_ACORTARResult;

                    coluna += 1;
                    if (_produtoACortar != null)
                    {

                        qtde_total += Convert.ToInt32(_produtoACortar.QTDE_MOSTRUARIO);

                        //Popular GRID VIEW FILHO
                        if (_produtoACortar.FOTO != null && _produtoACortar.FOTO.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<DESENV_PRODUTO> _fotoProduto = new List<DESENV_PRODUTO>();
                                _fotoProduto.Add(new DESENV_PRODUTO { CODIGO = _produtoACortar.CODIGO, FOTO = _produtoACortar.FOTO, FOTO2 = _produtoACortar.FOTO2 });
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
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProduto.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
            }
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["PRODUTOACORTAR"] != null)
            {
                IEnumerable<SP_OBTER_PRODUTO_ACORTARResult> listaProdutoACortar = (List<SP_OBTER_PRODUTO_ACORTARResult>)Session["PRODUTOACORTAR"];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                listaProdutoACortar = listaProdutoACortar.OrderBy(e.SortExpression + sortDirection);
                gvProduto.DataSource = listaProdutoACortar;
                gvProduto.DataBind();
            }
        }
        protected void gvFoto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;
                    if (_produto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _produto.FOTO;

                        System.Web.UI.WebControls.Image _imgFotoPeca2 = e.Row.FindControl("imgFotoPeca2") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca2 != null)
                            _imgFotoPeca2.ImageUrl = _produto.FOTO2;
                    }
                }
            }
        }
        #endregion

        #region "RELATORIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (ddlColecoes.SelectedValue.Trim() == "" && (ddlOrigem.SelectedValue == "0" || ddlOrigem.SelectedValue == "") && ddlGrupo.SelectedValue.Trim() == "" && txtModelo.Text.Trim() == "" && txtNome.Text.Trim() == "")
                {
                    labErro.Text = "Informe pelo menos um Filtro.";
                    return;
                }

                GerarRelatorio(ObterProdutoACortar());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void GerarRelatorio(List<SP_OBTER_PRODUTO_ACORTARResult> _produtoACortar)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "PRODUTO_A_CORTAR_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_produtoACortar));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        private StringBuilder MontarRelatorioHTML(List<SP_OBTER_PRODUTO_ACORTARResult> _produtoACortar)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarProdutoACortar(_texto, _produtoACortar);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        private StringBuilder MontarCabecalho(StringBuilder _texto)
        {
            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append("     <head>");
            _texto.Append("         <title>Produtos A Cortar</title>   ");
            _texto.Append("         <meta charset='UTF-8'>          ");
            _texto.Append("         <style type='text/css'>");
            _texto.Append("             @media print");
            _texto.Append("             {");
            _texto.Append("                 .tdback");
            _texto.Append("                 {");
            _texto.Append("                     background-color: WindowFrame !important;");
            _texto.Append("                     -webkit-print-color-adjust: exact;");
            _texto.Append("                 }");
            _texto.Append("             }");
            _texto.Append("         </style>");
            _texto.Append("     </head>");
            _texto.Append("");
            _texto.Append("<body onLoad='window.print();'>");
            _texto.Append("     <div id='fichaProdutoACortar' align='center' style='border: 0px solid #000;'>");
            _texto.Append("        <br />");
            _texto.Append("        <br />");
            _texto.Append("        <div align='center' style='border: 2px solid #000; background-color: transparent;");
            _texto.Append("            width: 517pt;'>");
            _texto.Append("            <h4>");
            _texto.Append("                Produtos liberados sem corte em " + DateTime.Now.ToString("dd/MM/yyyy HH:MM") + " </h4>");
            _texto.Append("            <table cellpadding='0' cellspacing='0' style='width: 518pt; padding: 0px; color: black;");
            _texto.Append("                font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; background: white;");
            _texto.Append("                white-space: nowrap; border: 1px solid #000; border-top: 2px solid #000;'>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");

            return _texto;
        }
        private StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.Append("                <tr>");
            _texto.Append("                    <td>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("            </table>");
            _texto.Append("        </div>");
            _texto.Append("    </div>");
            _texto.Append("</body>");
            _texto.Append("</html>");

            return _texto;
        }
        private StringBuilder MontarProdutoACortar(StringBuilder _texto, List<SP_OBTER_PRODUTO_ACORTARResult> _produtoACortar)
        {
            if (_produtoACortar.Count > 0)
            {
                _texto.Append("<tr style='line-height: 19px;'>");
                _texto.Append("    <td style='padding: 0px 0px 0px 10px; border:0px solid #000;'>");
                _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
                _texto.Append("            color: black; font-size: 8.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                _texto.Append("            background: white; white-space: nowrap;'>");
                _texto.Append("            <tr style='line-height: 5px;'>");
                _texto.Append("                <td colspan='6'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr>");
                _texto.Append("                <td style='width: 110px'>");
                _texto.Append("                    Coleção");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 120px'>");
                _texto.Append("                    Origem");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 130px'>");
                _texto.Append("                    Grupo");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 85px'>");
                _texto.Append("                    Modelo");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 110px'>");
                _texto.Append("                    Nome");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 100px'>");
                _texto.Append("                    Cor");
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align:center; width: 100px;'>");
                _texto.Append("                    Mostruário");
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align:center; width: 100px;'>");
                _texto.Append("                    Atacado");
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align:center; width: 100px;'>");
                _texto.Append("                    Varejo");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");

                foreach (var pac in _produtoACortar)
                {
                    _texto.Append("            <tr>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + pac.DESC_COLECAO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + pac.ORIGEM);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + pac.GRUPO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align:center;'>");
                    _texto.Append("                    " + pac.MODELO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + pac.NOME);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + pac.DESC_COR);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align:center;'>");
                    _texto.Append("                    " + pac.QTDE_MOSTRUARIO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align:center;'>");
                    _texto.Append("                    " + pac.QTDE_ATACADO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align:center;'>");
                    _texto.Append("                    " + pac.QTDE_VAREJO);
                    _texto.Append("                </td>");
                    _texto.Append("            </tr>");
                }

                _texto.Append("        </table>");
                _texto.Append("    </td>");
                _texto.Append("</tr>");
            }
            return _texto;
        }

        #endregion

        #region "DADOS INICIAIS"
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                Session["COLECAO"] = ddlColecoes.SelectedValue;
                CarregarOrigem(colecao);
            }
        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
                    CarregarOrigem(Session["COLECAO"].ToString().Trim());
                    ddlColecoes_SelectedIndexChanged(null, null);
                }
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = (new ProducaoController().ObterGrupoProduto("01"));
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = new DesenvolvimentoController().ObterProdutoOrigem().Where(i => i.STATUS == 'A').ToList();
            _origem = _origem.Where(p => p.COLECAO.Trim() == col.Trim()).OrderBy(i => i.DESCRICAO).ToList();
            if (_origem != null)
            {
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigem.DataSource = _origem;
                ddlOrigem.DataBind();

                if (_origem.Count == 2)
                    ddlOrigem.SelectedValue = _origem[1].CODIGO.ToString();

            }
        }
        #endregion

    }
}
