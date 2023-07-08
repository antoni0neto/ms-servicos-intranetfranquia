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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using DAL;
using System.Text;

namespace Relatorios
{
    public partial class prod_rel_produto_preco_aprovado : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

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
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "prod_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";

                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
                CarregarGrupos();
                CarregarSubGrupos("");
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_PRODUTO_PRECO_APROVADOResult> ObterPrecoAprovado()
        {
            List<SP_OBTER_PRODUTO_PRECO_APROVADOResult> precoAprovado = new List<SP_OBTER_PRODUTO_PRECO_APROVADOResult>();

            string grupo = "";
            char? tipo = null;

            if (ddlGrupo.SelectedValue.Trim() != "")
                grupo = ddlGrupo.SelectedValue.Trim();

            if (ddlTipo.SelectedValue.Trim() != "")
                tipo = Convert.ToChar(ddlTipo.SelectedValue.Trim());

            precoAprovado = prodController.ObterProdutoPrecoAprovado(ddlColecoesBuscar.SelectedValue.Trim(), txtProduto.Text.Trim(), grupo, ddlGriffe.SelectedValue.Trim(), ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim(), tipo);

            return precoAprovado.OrderByDescending(p => p.PRECO).ToList();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlColecoesBuscar.SelectedValue.Trim() == "")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                gvProduto.DataSource = ObterPrecoAprovado();
                gvProduto.DataBind();

                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;
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
                    SP_OBTER_PRODUTO_PRECO_APROVADOResult _precoAprovado = e.Row.DataItem as SP_OBTER_PRODUTO_PRECO_APROVADOResult;

                    if (_precoAprovado != null)
                    {
                        Literal _litTL = e.Row.FindControl("litTL") as Literal;
                        if (_litTL != null)
                            if (_precoAprovado.PRECO != null)
                                _litTL.Text = "R$ " + Convert.ToDecimal(_precoAprovado.PRECO).ToString("###,###,###,##0.00");


                        System.Web.UI.WebControls.Image _imgFoto = e.Row.FindControl("imgFoto") as System.Web.UI.WebControls.Image;
                        if (_imgFoto != null)
                            _imgFoto.ImageUrl = _precoAprovado.FOTO_PECA;

                        ImageButton _ibtPesquisar = e.Row.FindControl("ibtPesquisar") as ImageButton;
                        ImageButton _ibtImprimir = e.Row.FindControl("ibtImprimir") as ImageButton;

                        if (((Convert.ToInt32(_precoAprovado.PRODUTO.Substring(0, 1)) % 2) == 0)) // PRODUTO NACIONAL
                        {
                            if (_precoAprovado != null && _precoAprovado.SIMULACAO == 'S')
                            {
                                _ibtPesquisar.Visible = false;
                                if (!_precoAprovado.REVENDA)
                                    _ibtImprimir.Visible = false;
                            }
                        }
                        else //PRODUTO IMPORTADO
                        {
                            _ibtPesquisar.Visible = false;
                        }

                        if (_precoAprovado.PRODUCAO != "")
                            e.Row.BackColor = Color.PaleGreen;

                    }
                }
            }
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {

            if (ddlColecoesBuscar.SelectedValue.Trim() == "")
            {
                labErro.Text = "Selecione a Coleção.";
                return;
            }

            IEnumerable<SP_OBTER_PRODUTO_PRECO_APROVADOResult> listPrecoAprovado = ObterPrecoAprovado();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            listPrecoAprovado = listPrecoAprovado.OrderBy(e.SortExpression + sortDirection);
            gvProduto.DataSource = listPrecoAprovado;
            gvProduto.DataBind();

        }

        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton b = (ImageButton)sender;

                if (b != null)
                {
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        var colecao = Convert.ToInt32(ddlColecoesBuscar.SelectedValue.Trim());
                        string produto = row.Cells[1].Text.Trim();
                        string mostruario = "";

                        HttpResponse resp = new HttpResponse(null);
                        var simulacaoProducao = prodController.ObterCustoSimulacao(produto, 'N').Where(p => p.SIMULACAO == 'N');
                        var simulacaoMostruario = prodController.ObterCustoSimulacao(produto, 'S').Where(p => p.SIMULACAO == 'N');
                        if (simulacaoProducao != null && simulacaoProducao.Count() > 0)//PRODUTO APROVADO PRODUCAO
                        {
                            mostruario = "N";
                            if (colecao >= 27 || colecao <= 90)
                                Response.Redirect(("prod_cad_produto_custo_aprov_novo.aspx?c=" + colecao + "&p=" + produto + "&m=" + mostruario), "_blank", "");
                            else
                                Response.Redirect(("prod_cad_produto_custo_aprov.aspx?c=" + colecao + "&p=" + produto + "&m=" + mostruario), "_blank", "");
                        }
                        else if (simulacaoMostruario != null && simulacaoMostruario.Count() > 0)//PRODUTO APROVADO MOSTRUARIO
                        {
                            mostruario = "S";
                            if (colecao >= 27 || colecao <= 90)
                                Response.Redirect(("prod_cad_produto_custo_aprov_novo.aspx?c=" + colecao + "&p=" + produto + "&m=" + mostruario), "_blank", "");
                            else
                                Response.Redirect(("prod_cad_produto_custo_aprov.aspx?c=" + colecao + "&p=" + produto + "&m=" + mostruario), "_blank", "");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton b = (ImageButton)sender;
                if (b != null)
                {
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        string colecao = ddlColecoesBuscar.SelectedValue.Trim();
                        string produto = row.Cells[1].Text.Trim();

                        bool produtoNacional = false;
                        if ((Convert.ToInt32(produto.Substring(0, 1)) % 2) == 0)
                            produtoNacional = true;

                        bool revenda = false;
                        var produtoLinx = baseController.BuscaProduto(produto);
                        if (produtoLinx != null)
                            revenda = produtoLinx.REVENDA;

                        GerarRelatorio(colecao, produto, produtoNacional, revenda);

                        //_url = "fnAbrirTelaCadastroMaior('prod_rel_produto_preco_aprovado_imp.aspx?c=" + colecao + "&p=" + produto + "&m=" + mostruario + "')";
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "IMPRESSAO"
        private PROD_HB ObterProduto(string colecao, string produto, char mostruario)
        {
            PROD_HB _produto = new PROD_HB();
            _produto = prodController.ObterHB().Where(p => p.COLECAO.Trim() == colecao.Trim() && p.CODIGO_PRODUTO_LINX.Trim() == produto.Trim() && p.MOSTRUARIO == mostruario).Take(1).SingleOrDefault();

            return _produto;
        }
        private void GerarRelatorio(string colecao, string produto, bool produtoNacional, bool revenda)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "PROD_PRECO_" + produto + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(colecao, produto, produtoNacional, revenda));
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
        private StringBuilder MontarRelatorioHTML(string colecao, string produto, bool produtoNacional, bool revenda)
        {
            StringBuilder _texto = new StringBuilder();

            if (produtoNacional && !revenda)
            {
                _texto = MontarRelatorioNacional(_texto, colecao, produto);
            }
            else
            {
                _texto = MontarRelatorioImportado(_texto, produto);
            }

            return _texto;
        }

        private StringBuilder MontarRelatorioNacional(StringBuilder _texto, string colecao, string produto)
        {
            char mostruario = ' ';

            var simulacaoProducao = prodController.ObterCustoSimulacao(produto, 'N').Where(p => p.SIMULACAO == 'N');
            var simulacaoMostruario = prodController.ObterCustoSimulacao(produto, 'S').Where(p => p.SIMULACAO == 'N');
            if (simulacaoProducao != null && simulacaoProducao.Count() > 0)//PRODUTO APROVADO PRODUCAO
                mostruario = 'N';
            else if (simulacaoMostruario != null && simulacaoMostruario.Count() > 0)//PRODUTO APROVADO MOSTRUARIO
                mostruario = 'S';

            var _produto = ObterProduto(colecao, produto, mostruario);

            var griffe = "-";
            var desenvProduto = desenvController.ObterProduto(_produto.COLECAO.Trim(), _produto.CODIGO_PRODUTO_LINX.Trim()).FirstOrDefault();
            if (desenvProduto != null)
                griffe = desenvProduto.GRIFFE.Trim();

            _texto.AppendLine("");
            _texto.AppendLine("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine("<html>");
            _texto.AppendLine("<head>");
            _texto.AppendLine("    <title>Custo/Preço Nacional</title>");
            _texto.AppendLine("    <meta charset='UTF-8' />");
            _texto.AppendLine("    <style type='text/css'>");
            _texto.AppendLine("        @media print {");
            _texto.AppendLine("            .background-force {");
            _texto.AppendLine("                -webkit-print-color-adjust: exact;");
            _texto.AppendLine("            }");
            _texto.AppendLine("        }");
            _texto.AppendLine("    </style>");
            _texto.AppendLine("</head>");
            _texto.AppendLine("<body onload=''>");
            _texto.AppendLine("    <br />");
            _texto.AppendLine("    <div id='divNacional' align='center'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' width='517' style='width: 517pt;");
            _texto.AppendLine("            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("            background: white; white-space: nowrap;'>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px;' class='background-force'>");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Coleção");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;" + baseController.BuscaColecaoAtual(_produto.COLECAO).DESC_COLECAO.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;HB");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center; width:60px;'>");
            _texto.AppendLine("                                " + _produto.HB.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Produto");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + _produto.CODIGO_PRODUTO_LINX);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Cor");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + prodController.ObterCoresBasicas(_produto.COR).DESC_COR.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Griffe");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;" + griffe);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Nome");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:left;' colspan='3'>");
            _texto.AppendLine("                                &nbsp;&nbsp;" + _produto.NOME);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Data");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + _produto.DATA_INCLUSAO.ToString("dd/MM/yyyy"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding:10px;' class='background-force'>");
            _texto.AppendLine("                    Tecidos");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Fornecedor");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Tecido");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Mts/Kgs");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Quantidade");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Preço");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Consumo");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Total");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            var lstTecidos = new List<PROD_HB>();
            //Adiciona pai na lista
            lstTecidos.Add(_produto);
            var tecidoDetalhe = new List<PROD_HB>();
            tecidoDetalhe = prodController.ObterDetalhesHB(_produto.CODIGO);
            foreach (PROD_HB det in tecidoDetalhe)
                if (det != null)
                    lstTecidos.Add(det);

            int totalGrade = 0;
            decimal gastoPorCorte = 0;
            decimal consumoTecidoPreco = 0;
            decimal totalTecido = 0;
            decimal valorTotalTecido = 0;
            decimal valorTotalCusto = 0;

            foreach (var tec in lstTecidos)
            {
                totalGrade = prodController.ObterQtdeGradeHB(tec.CODIGO, 3);

                if (tec.GASTO_PECA_CUSTO == null)
                {
                    gastoPorCorte = Convert.ToDecimal(tec.GASTO_FOLHA * totalGrade);
                    consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte + tec.RETALHOS) / totalGrade));
                }
                else
                {
                    gastoPorCorte = Convert.ToDecimal(tec.GASTO_PECA_CUSTO * totalGrade);
                    consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte) / totalGrade));
                }

                totalTecido = totalGrade * consumoTecidoPreco;
                valorTotalTecido = consumoTecidoPreco * Convert.ToDecimal(tec.CUSTO_TECIDO);

                if (mostruario == 'S')
                    valorTotalTecido = valorTotalTecido + (valorTotalTecido * Convert.ToDecimal(10.000 / 100.000));


                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + tec.FORNECEDOR);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + tec.TECIDO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + totalTecido.ToString("###,###,##0.000"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + totalGrade.ToString());
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + Convert.ToDecimal(tec.CUSTO_TECIDO).ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + consumoTecidoPreco.ToString("###,###,##0.000"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + valorTotalTecido.ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");

                valorTotalCusto += valorTotalTecido;
            }


            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px;' class='background-force'>");
            _texto.AppendLine("                    Aviamentos");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Fornecedor");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Aviamento");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Consumo");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Preço");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Total");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");


            var lstAviamentos = new List<SP_OBTER_HB_AVIAMENTO_CUSTOResult>();
            lstAviamentos = prodController.ObterAviamentoCusto(_produto.CODIGO);
            //var aviamentoExtra = prodController.ObterMaterialExtra(produto.CODIGO, 'A');

            valorTotalCusto = 0;
            decimal valorTotalAviamento = 0;
            foreach (var avi in lstAviamentos)
            {

                valorTotalAviamento = Convert.ToDecimal(avi.CONSUMO * avi.PRECO);

                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + avi.FORNECEDOR);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + avi.AVIAMENTO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + avi.CONSUMO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + avi.PRECO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + valorTotalAviamento.ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");

                valorTotalCusto += valorTotalAviamento;
            }
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px;' class='background-force'>");
            _texto.AppendLine("                    Serviços");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Fornecedor");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Serviço");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center; width: 130px;'>");
            _texto.AppendLine("                                Total");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            var lstServicos = prodController.ObterCustoServico(_produto.CODIGO_PRODUTO_LINX, Convert.ToChar(_produto.MOSTRUARIO));

            valorTotalCusto = 0;
            decimal valorTotalServico = 0;
            foreach (var ser in lstServicos)
            {

                valorTotalServico = ((ser.CUSTO_PECA != null) ? Convert.ToDecimal(ser.CUSTO_PECA) : Convert.ToDecimal(ser.CUSTO));

                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + ser.FORNECEDOR);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + prodController.ObterServicoProducao(ser.SERVICO).DESCRICAO.ToUpper());
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + valorTotalServico.ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");

                valorTotalCusto += valorTotalServico;
            }
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px;' class='background-force'>");
            _texto.AppendLine("                    Outros");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Item");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center; width: 130px;'>");
            _texto.AppendLine("                                Total");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            var custoOrigem = prodController.ObterCustoOrigem(_produto.CODIGO_PRODUTO_LINX, mostruario, 'N').Where(p => p.COD_CUSTO_ORIGEM == 4 ||
                                                                                                                  p.COD_CUSTO_ORIGEM == 5 ||
                                                                                                                  p.COD_CUSTO_ORIGEM == 6 ||
                                                                                                                  p.COD_CUSTO_ORIGEM == 14 ||
                                                                                                                  p.COD_CUSTO_ORIGEM == 15).OrderBy(p => p.PRODUTO_CUSTO_COD_ORIGEM.DESCRICAO).ToList();

            valorTotalCusto = 0;
            foreach (var c in custoOrigem)
            {

                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + c.PRODUTO_CUSTO_COD_ORIGEM.DESCRICAO.ToString());
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + Convert.ToDecimal(c.CUSTO_TOTAL).ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");

                valorTotalCusto += Convert.ToDecimal(c.CUSTO_TOTAL);
            }



            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px; text-align:center;' class='background-force'>");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                <img alt='Foto Peça' Width='180px' src='..\\.." + _produto.FOTO_PECA.Replace("~", "").Replace("/", "\\") + "' />");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td valign='top'>");
            _texto.AppendLine("                                <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                        <td colspan='5'>Formação MARK-UP</td>");
            _texto.AppendLine("                                    </tr>");


            var lstCustoSimulacao = prodController.ObterCustoSimulacao(_produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(_produto.MOSTRUARIO)).Where(p => p.CUSTO_MOSTRUARIO == 'N').ToList();

            List<decimal> precoProduto = new List<decimal>();
            List<decimal> markUp = new List<decimal>();
            List<decimal> lucro = new List<decimal>();
            foreach (PRODUTO_CUSTO_SIMULACAO custoS in lstCustoSimulacao)
            {
                if (custoS != null)
                {
                    precoProduto.Add(custoS.PRECO_PRODUTO);
                    markUp.Add(custoS.MARKUP);
                    lucro.Add(custoS.LUCRO_PORC);
                }
            }

            _texto.AppendLine("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[0].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[1].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[2].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[3].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[4].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[0].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[1].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[2].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[3].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[4].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");


            var CMV = prodController.ObterCustoOrigem(_produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(_produto.MOSTRUARIO), 'N').Where(p =>
                                                                                                                        p.COD_CUSTO_ORIGEM == 1 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 2 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 3 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 4 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 5 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 6 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 14 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 15
                                                                                                                        ).Sum(p => p.CUSTO_TOTAL);


            var mkp = 0.00M;
            var precoProdutoFinal = 0.00M;
            var precoAprovadoProducao = prodController.ObterCustoSimulacaoPrecoAprovado(_produto.CODIGO_PRODUTO_LINX.Trim(), 'N');
            if (precoAprovadoProducao != null)
            {
                mkp = precoAprovadoProducao.MARKUP;
                precoProdutoFinal = precoAprovadoProducao.PRECO_PRODUTO;
            }
            else
            {
                var precoAprovadoMostruario = prodController.ObterCustoSimulacaoPrecoAprovado(_produto.CODIGO_PRODUTO_LINX.Trim(), 'S');
                if (precoAprovadoMostruario != null)
                {
                    mkp = precoAprovadoMostruario.MARKUP;
                    precoProdutoFinal = precoAprovadoMostruario.PRECO_PRODUTO;
                }
            }

            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1; text-align:left;'>&nbsp; Custo</td>");
            _texto.AppendLine("                                        <td colspan='2' style='text-align: left;'>&nbsp;&nbsp;R$ " + Convert.ToDecimal(CMV).ToString("###,###,##0.00") + "</td>");
            _texto.AppendLine("                                        <td colspan='2'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1; text-align:left;'>&nbsp; MKUP</td>");
            _texto.AppendLine("                                        <td colspan='2' style='text-align: left;'>&nbsp;&nbsp;" + (mkp.ToString("0.0")) + "</td>");
            _texto.AppendLine("                                        <td colspan='2'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");

            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1; text-align:left;'>&nbsp; Preço</td>");
            _texto.AppendLine("                                        <td colspan='2' style='text-align: left;'>&nbsp;&nbsp;R$ " + (precoProdutoFinal.ToString("###,###,##0.00")) + "</td>");
            _texto.AppendLine("                                        <td colspan='2'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                </table>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("    </div>");
            _texto.AppendLine("</body>");
            _texto.AppendLine("</html>");


            return _texto;
        }

        private StringBuilder MontarRelatorioNacionalXXXX(StringBuilder _texto, string colecao, string produto)
        {
            char mostruario = ' ';

            var simulacaoProducao = prodController.ObterCustoSimulacao(produto, 'N').Where(p => p.SIMULACAO == 'N');
            var simulacaoMostruario = prodController.ObterCustoSimulacao(produto, 'S').Where(p => p.SIMULACAO == 'N');
            if (simulacaoProducao != null && simulacaoProducao.Count() > 0)//PRODUTO APROVADO PRODUCAO
                mostruario = 'N';
            else if (simulacaoMostruario != null && simulacaoMostruario.Count() > 0)//PRODUTO APROVADO MOSTRUARIO
                mostruario = 'S';

            PROD_HB _produto = ObterProduto(colecao, produto, mostruario);

            _texto.Append("");
            _texto.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append("<html>");
            _texto.Append("<head>");
            _texto.Append("    <title>Custo/Preço Nacional</title>");
            _texto.Append("    <meta charset='UTF-8' />");
            _texto.Append("    <style type='text/css'>");
            _texto.Append("        @media print");
            _texto.Append("        {");
            _texto.Append("            .background-force");
            _texto.Append("            {");
            _texto.Append("                -webkit-print-color-adjust: exact;");
            _texto.Append("            }");
            _texto.Append("        }");
            _texto.Append("    </style>");
            _texto.Append("</head>");
            _texto.Append("<body onload='window.print();'>");
            _texto.Append("    <br />");
            _texto.Append("    <div id='divNacional' align='center'>");
            _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='517' style='width: 517pt;");
            _texto.Append("            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("            background: white; white-space: nowrap;'>");
            _texto.Append("            <tr>");
            _texto.Append("                <td style='padding: 10px;' class='background-force'>");
            _texto.Append("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td style='background-color:#FFE4E1;'>");
            _texto.Append("                                &nbsp;Grupo");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                " + _produto.GRUPO);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-bottom: none; border-top: none;'>&nbsp;</td>");
            _texto.Append("                            <td style='background-color:#FFE4E1;'>");
            _texto.Append("                                &nbsp;HB");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                " + _produto.HB);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-bottom: none; border-top: none;'>&nbsp;</td>");
            _texto.Append("                            <td style='background-color:#FFE4E1;'>");
            _texto.Append("                                &nbsp;Mostruário");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                " + ((mostruario == 'S') ? "Sim" : "Não"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='8'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td style='background-color:#FFE4E1;'>");
            _texto.Append("                                &nbsp;Nome");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                " + _produto.NOME);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-bottom: none; border-top: none;'>&nbsp;</td>");
            _texto.Append("                            <td style='background-color:#FFE4E1;'>");
            _texto.Append("                                &nbsp;Data");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                " + _produto.DATA_INCLUSAO.ToString("dd/MM/yyyy"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-bottom: none; border-top: none;'>&nbsp;</td>");
            _texto.Append("                            <td style='background-color:#FFE4E1;'>");
            _texto.Append("                                &nbsp;Código");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                " + _produto.CODIGO_PRODUTO_LINX);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("            <tr>");
            _texto.Append("                <td style='padding:10px;' class='background-force'>");
            _texto.Append("                    Tecidos");
            _texto.Append("                     <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.Append("                        <tr style='background-color:#FFE4E1;'>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;Fornecedor");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;Tecido");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                Mts/Kgs");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                Quantidade");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                Preço");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                Consumo");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                Total");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");

            List<PROD_HB> lstTecidos = new List<PROD_HB>();
            //Adiciona pai na lista
            lstTecidos.Add(_produto);
            List<PROD_HB> tecidoDetalhe = new List<PROD_HB>();
            tecidoDetalhe = prodController.ObterDetalhesHB(_produto.CODIGO);
            foreach (PROD_HB det in tecidoDetalhe)
                if (det != null)
                    lstTecidos.Add(det);
            //var tecidoExtra = prodController.ObterMaterialExtra(_produto.CODIGO, 'T');

            int totalGrade = 0;
            decimal gastoPorCorte = 0;
            decimal consumoTecidoPreco = 0;
            decimal totalTecido = 0;
            decimal valorTotalTecido = 0;
            decimal valorTotalCusto = 0;
            foreach (var tec in lstTecidos)
            {
                totalGrade = prodController.ObterQtdeGradeHB(tec.CODIGO, 3);

                if (tec.GASTO_PECA_CUSTO == null)
                {
                    gastoPorCorte = Convert.ToDecimal(tec.GASTO_FOLHA * totalGrade);
                    consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte + tec.RETALHOS) / totalGrade));
                }
                else
                {
                    gastoPorCorte = Convert.ToDecimal(tec.GASTO_PECA_CUSTO * totalGrade);
                    consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte) / totalGrade));
                }

                totalTecido = totalGrade * consumoTecidoPreco;

                valorTotalTecido = consumoTecidoPreco * Convert.ToDecimal(tec.CUSTO_TECIDO);

                _texto.Append("                        <tr>");
                _texto.Append("                            <td>");
                _texto.Append("                                &nbsp;" + tec.FORNECEDOR);
                _texto.Append("                            </td>");
                _texto.Append("                            <td>");
                _texto.Append("                                &nbsp;" + tec.TECIDO);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                " + totalTecido.ToString("###,###,##0.000"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                " + totalGrade.ToString());
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                R$ " + Convert.ToDecimal(tec.CUSTO_TECIDO).ToString("###,###,##0.00"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                " + consumoTecidoPreco.ToString("###,###,##0.000"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                R$ " + valorTotalTecido.ToString("###,###,##0.00"));
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");

                valorTotalCusto += valorTotalTecido;
            }
            _texto.Append("                        <tr style='background-color:#FFE4E1;'>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");

            _texto.Append("            <tr>");
            _texto.Append("                <td style='padding: 10px;' class='background-force'>");
            _texto.Append("                    Aviamentos");
            _texto.Append("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.Append("                        <tr style='background-color:#FFE4E1;'>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;Fornecedor");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;Aviamento");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                Consumo");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                Preço");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                Total");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");

            List<SP_OBTER_HB_AVIAMENTO_CUSTOResult> lstAviamentos = new List<SP_OBTER_HB_AVIAMENTO_CUSTOResult>();
            lstAviamentos = prodController.ObterAviamentoCusto(_produto.CODIGO);
            //var aviamentoExtra = prodController.ObterMaterialExtra(produto.CODIGO, 'A');

            valorTotalCusto = 0;
            decimal valorTotalAviamento = 0;
            foreach (var avi in lstAviamentos)
            {

                valorTotalAviamento = Convert.ToDecimal(avi.CONSUMO * avi.PRECO);

                _texto.Append("                        <tr>");
                _texto.Append("                            <td>");
                _texto.Append("                                &nbsp;" + avi.FORNECEDOR);
                _texto.Append("                            </td>");
                _texto.Append("                            <td>");
                _texto.Append("                                &nbsp;" + avi.AVIAMENTO);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                " + avi.CONSUMO);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                R$ " + avi.PRECO);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                R$ " + valorTotalAviamento.ToString("###,###,##0.00"));
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");

                valorTotalCusto += valorTotalAviamento;
            }
            _texto.Append("                        <tr style='background-color:#FFE4E1;'>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");

            _texto.Append("            <tr>");
            _texto.Append("                <td style='padding: 10px;' class='background-force'>");
            _texto.Append("                    Serviços");
            _texto.Append("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.Append("                        <tr style='background-color:#FFE4E1;'>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;Fornecedor");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;Serviço");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                Total");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");

            var lstServicos = prodController.ObterCustoServico(_produto.CODIGO_PRODUTO_LINX, Convert.ToChar(_produto.MOSTRUARIO));

            valorTotalCusto = 0;
            decimal valorTotalServico = 0;
            foreach (var ser in lstServicos)
            {

                valorTotalServico = ((ser.CUSTO_PECA != null) ? Convert.ToDecimal(ser.CUSTO_PECA) : Convert.ToDecimal(ser.CUSTO));

                _texto.Append("                        <tr>");
                _texto.Append("                            <td>");
                _texto.Append("                                &nbsp;" + ser.FORNECEDOR);
                _texto.Append("                            </td>");
                _texto.Append("                            <td>");
                _texto.Append("                                &nbsp;" + prodController.ObterServicoProducao(ser.SERVICO).DESCRICAO.ToUpper());
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                R$ " + valorTotalServico.ToString("###,###,##0.00"));
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");

                valorTotalCusto += valorTotalServico;
            }
            _texto.Append("                        <tr style='background-color:#FFE4E1;'>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center;'>");
            _texto.Append("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");

            _texto.Append("            <tr>");
            _texto.Append("                <td style='padding: 10px; text-align:center;' class='background-force'>");
            _texto.Append("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td>");
            _texto.Append("                                <img alt='Foto Peça' Width='180px' src='..\\.." + _produto.FOTO_PECA.Replace("~", "").Replace("/", "\\") + "' />");
            _texto.Append("                            </td>");
            _texto.Append("                            <td valign='top'>");
            _texto.Append("                                <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");

            //List<PRODUTO_CUSTO_SIMULACAO> lstCustoSimulacao = new List<PRODUTO_CUSTO_SIMULACAO>();
            //lstCustoSimulacao = prodController.ObterCustoSimulacao().Where(p =>
            //                                                            p.PRODUTO.Trim() == _produto.CODIGO_PRODUTO_LINX.Trim() &&
            //                                                            p.MOSTRUARIO == _produto.MOSTRUARIO &&
            //                                                            p.CUSTO_MOSTRUARIO == 'N').ToList();
            var lstCustoSimulacao = prodController.ObterCustoSimulacao(_produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(_produto.MOSTRUARIO)).Where(p => p.CUSTO_MOSTRUARIO == 'N').ToList();

            List<decimal> precoProduto = new List<decimal>();
            List<decimal> markUp = new List<decimal>();
            List<decimal> lucro = new List<decimal>();
            foreach (PRODUTO_CUSTO_SIMULACAO custoS in lstCustoSimulacao)
            {
                if (custoS != null)
                {
                    precoProduto.Add(custoS.PRECO_PRODUTO);
                    markUp.Add(custoS.MARKUP);
                    lucro.Add(custoS.LUCRO_PORC);
                }
            }

            _texto.Append("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            " + markUp[0].ToString("0.0"));
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            " + markUp[1].ToString("0.0"));
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            " + markUp[2].ToString("0.0"));
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            " + markUp[3].ToString("0.0"));
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            " + markUp[4].ToString("0.0"));
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            " + lucro[0].ToString("###") + "%");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            " + lucro[1].ToString("###") + "%");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            " + lucro[2].ToString("###") + "%");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            " + lucro[3].ToString("###") + "%");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            " + lucro[4].ToString("###") + "%");
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            R$ " + precoProduto[0].ToString("###,###,##0.00"));
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            R$ " + precoProduto[1].ToString("###,###,##0.00"));
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            R$ " + precoProduto[2].ToString("###,###,##0.00"));
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            R$ " + precoProduto[3].ToString("###,###,##0.00"));
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            R$ " + precoProduto[4].ToString("###,###,##0.00"));
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='5'>&nbsp;</td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='4'>&nbsp;</td>");
            _texto.Append("                                        <td style='background-color:#FFE4E1;'>Custo TX</td>");
            _texto.Append("                                    </tr>");

            var CMV = new List<PRODUTO_CUSTO_ORIGEM>();
            if (colecao == "27")
            {
                CMV = prodController.ObterCustoOrigem(_produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(_produto.MOSTRUARIO), 'N').Where(p =>
                                                                                                                            p.COD_CUSTO_ORIGEM == 1 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 2 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 3 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 4 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 5 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 6 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 14 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 15
                                                                                                                            ).ToList();
            }
            else
            {
                CMV = prodController.ObterCustoOrigem(_produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(_produto.MOSTRUARIO), 'N').Where(p =>
                                                                                                                            p.COD_CUSTO_ORIGEM == 1 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 2 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 3 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 4 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 6
                                                                                                                            ).ToList();
            }


            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='4'>&nbsp;</td>");
            _texto.Append("                                        <td>R$ " + Convert.ToDecimal(CMV.Sum(p => p.CUSTO_TOTAL)).ToString("###,###,##0.00") + "</td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='5'>&nbsp;</td>");
            _texto.Append("                                    </tr>");


            //var precoAprovado = prodController.ObterCustoSimulacao().Where(p =>
            //                                                                p.PRODUTO.Trim() == _produto.CODIGO_PRODUTO_LINX.Trim() &&
            //                                                                    //p.MOSTRUARIO == 'S' &&
            //                                                                p.SIMULACAO == 'N' &&
            //                                                                p.CUSTO_MOSTRUARIO == 'N').OrderBy(x => x.DATA_INCLUSAO).ToList();
            var precoAprovado = prodController.ObterCustoSimulacaoPrecoAprovado(_produto.CODIGO_PRODUTO_LINX.Trim());

            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='3'>&nbsp;</td>");
            _texto.Append("                                        <td style='background-color:#FFE4E1;'> Aprovado</td>");
            _texto.Append("                                        <td> R$ " + (precoAprovado[precoAprovado.Count() - 1].PRECO_PRODUTO.ToString("###,###,##0.00")) + "</td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='3'>&nbsp;</td>");
            _texto.Append("                                        <td style='background-color:#FFE4E1;'> Lucro %</td>");
            _texto.Append("                                        <td> R$ " + (precoAprovado[precoAprovado.Count() - 1].LUCRO_PORC.ToString("###,###,##0.00")) + "%</td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='3'>&nbsp;</td>");
            _texto.Append("                                        <td style='background-color:#FFE4E1;'> MKUP</td>");
            _texto.Append("                                        <td> R$ " + (precoAprovado[precoAprovado.Count() - 1].MARKUP.ToString("0.0")) + "</td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='5'>&nbsp;</td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='4'>&nbsp;</td>");
            _texto.Append("                                        <td style='background-color:#FFE4E1;'>-</td>");
            _texto.Append("                                    </tr>");

            //var tt = CMV.Where(p => p.COD_CUSTO_ORIGEM != 5);
            decimal valorTT = 0;
            //if (tt != null && tt.Count() > 0)
            //{
            //    valorTT = (tt.Sum(p => p.CUSTO_TOTAL).Value);
            //    valorTT = valorTT + (valorTT * Convert.ToDecimal(0.10));
            //    valorTT = Math.Round(valorTT, 2);
            //}

            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='4'>&nbsp;</td>");
            _texto.Append("                                        <td>R$ " + valorTT.ToString("###,###,##0.00") + "</td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='5'>&nbsp;</td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='4'>&nbsp;</td>");
            _texto.Append("                                        <td style='background-color:#FFE4E1;'>&nbsp;</td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='4'>&nbsp;</td>");
            _texto.Append("                                        <td>&nbsp;</td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td colspan='4'>&nbsp;</td>");
            _texto.Append("                                        <td>&nbsp;</td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                </table>");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("        </table>");
            _texto.Append("    </div>");
            _texto.Append("</body>");
            _texto.Append("</html>");



            return _texto;
        }
        private StringBuilder MontarRelatorioImportado(StringBuilder _texto, string produto)
        {

            var imp = prodController.ObterProdutoImportadoPreco(produto);

            _texto.Append("");
            _texto.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append("<html>");
            _texto.Append("<head>");
            _texto.Append("    <title>Custo/Preço Importado</title>");
            _texto.Append("    <meta charset='UTF-8' />");
            _texto.Append("</head>");
            _texto.Append("<body onload='window.print();'>");
            _texto.Append("    <br />");
            _texto.Append("    <div id='divImportado' align='center'>");
            _texto.Append("        <table border='1' cellpadding='0' cellspacing='0' width='517' style='width: 517pt;");
            _texto.Append("            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("            background: white; white-space: nowrap;'>");
            _texto.Append("            <tr>");
            _texto.Append("                <td style='line-height:23px; width:290px; padding:30px;'>");
            _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='2' style='text-align:center'>");
            _texto.Append("                                <font size='3' color='red'>" + imp[0].DESC_PRODUTO + "</font>");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='2' style='text-align:center'>");
            _texto.Append("                                " + imp[0].PRODUTO);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td>");
            _texto.Append("                                R$ " + imp[0].CUSTO);
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                " + imp[0].INFO);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td>");
            _texto.Append("                                FOB");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                " + imp[0].FOB);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td>");
            _texto.Append("                                QUANT");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");

            int cont = 0;
            foreach (var i in imp)
            {
                _texto.Append("                        <tr>");
                _texto.Append("                            <td>" + ((cont == 0) ? "COR" : "") + "</td>");
                _texto.Append("                            <td>" + i.DESC_COR_PRODUTO + "</td>");
                _texto.Append("                        </tr>");
                cont += 1;
            }
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("                <td style=' line-height:50px;' valign='top'>");
            _texto.Append("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td style='width:110px;'>");
            _texto.Append("                                &nbsp;&nbsp;INICIAL");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: none'>");
            _texto.Append("                                &nbsp;&nbsp;" + imp[0].INICIAL);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:right; border-left: none'>");
            _texto.Append("                                " + (Math.Round(Convert.ToDouble(imp[0].MARKUP), 2).ToString()) + "&nbsp;&nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("            <tr>");
            _texto.Append("                <td colspan='2'>");
            _texto.Append("                    <table border='1' width='100%'>");
            cont = 0;

            _texto.Append("                        <tr>");
            foreach (var i in imp)
            {
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                " + ((i.NUMERO_FOTO != null) ? "<img alt='Foto' src='" + i.PATH_FOTO + "' width='200' />" : "&nbsp;") + "");
                _texto.Append("                            </td>");

                if (cont == 2)
                {
                    _texto.Append("                        </tr>");
                    _texto.Append("                        <tr>");
                }

                cont += 1;
            }
            _texto.Append("                        </tr>");

            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("        </table>");
            _texto.Append("    </div>");
            _texto.Append("</body>");
            _texto.Append("</html>");


            return _texto;
        }
        #endregion

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecoesBuscar.DataSource = _colecoes;
                ddlColecoesBuscar.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoesBuscar.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("01");
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = (new BaseController().BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        private void CarregarGrupos()
        {
            List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "" });
                ddlMaterialGrupo.DataSource = _matGrupo;
                ddlMaterialGrupo.DataBind();
            }
        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarSubGrupos(string grupo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = null;
            if (grupo.Trim() != "")
                _matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();
            else
                _matSubGrupo = desenvController.ObterMaterialSubGrupo();

            if (_matSubGrupo != null)
            {
                _matSubGrupo = _matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "" });
                ddlMaterialSubGrupo.DataSource = _matSubGrupo;
                ddlMaterialSubGrupo.DataBind();
            }
        }
        #endregion
    }
}
