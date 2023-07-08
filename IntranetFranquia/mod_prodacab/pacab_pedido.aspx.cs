using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class pacab_pedido : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        const string PROD_ACAB_PEDCOMPRA = "PROD_ACAB_PEDCOMPRA";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
                CarregarFabricante();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

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
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

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
            List<DESENV_PRODUTO_ORIGEM> _origem = desenvController.ObterProdutoOrigem().Where(i => i.STATUS == 'A').ToList();
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
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        private void CarregarFabricante()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor().ToList();
            _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });
            _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'C') || p.STATUS == 'S').GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (_fornecedores != null)
            {
                ddlFabricante.DataSource = _fornecedores;
                ddlFabricante.DataBind();
            }
        }
        #endregion

        #region "PRODUTO"
        private void CarregarProdutoAcabado(string colecao, int? desenvOrigem, string grupo, string griffe, string produto, char mostruario)
        {
            var produtoAcabado = ObterProdutoAcabado(colecao, desenvOrigem, grupo, griffe, produto, mostruario);

            gvProdutoAcabado.DataSource = produtoAcabado;
            gvProdutoAcabado.DataBind();

            Session[PROD_ACAB_PEDCOMPRA] = produtoAcabado;
        }
        protected void gvProdutoAcabado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_ACABADOResult produtoAcabado = e.Row.DataItem as SP_OBTER_PRODUTO_ACABADOResult;

                    if (produtoAcabado != null)
                    {
                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(produtoAcabado.COLECAO)).DESC_COLECAO;

                        Button _btGerarPedido = e.Row.FindControl("btGerarPedido") as Button;
                        if (_btGerarPedido != null)
                            _btGerarPedido.CommandArgument = produtoAcabado.CODIGO.ToString();

                        ImageButton _btImprimir = e.Row.FindControl("btImprimir") as ImageButton;
                        if (_btImprimir != null)
                            _btImprimir.CommandArgument = produtoAcabado.CODIGO.ToString();


                        //Popular GRID VIEW FILHO
                        if (produtoAcabado.FOTO != null && produtoAcabado.FOTO.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<DESENV_PRODUTO> _fotoProduto = new List<DESENV_PRODUTO>();
                                _fotoProduto.Add(new DESENV_PRODUTO { CODIGO = produtoAcabado.CODIGO, FOTO = produtoAcabado.FOTO, FOTO2 = produtoAcabado.FOTO2 });
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
        protected void gvProdutoAcabado_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[PROD_ACAB_PEDCOMPRA] != null)
            {
                IEnumerable<SP_OBTER_PRODUTO_ACABADOResult> produtoAcabado = (IEnumerable<SP_OBTER_PRODUTO_ACABADOResult>)Session[PROD_ACAB_PEDCOMPRA];

                produtoAcabado = Filtrar(produtoAcabado);

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(gvProdutoAcabado, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(gvProdutoAcabado, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(gvProdutoAcabado, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                produtoAcabado = produtoAcabado.OrderBy(e.SortExpression + sortDirection);
                gvProdutoAcabado.DataSource = produtoAcabado;
                gvProdutoAcabado.DataBind();
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
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvProdutoAcabado.DataSource = Filtrar((IEnumerable<SP_OBTER_PRODUTO_ACABADOResult>)Session[PROD_ACAB_PEDCOMPRA]);
            gvProdutoAcabado.DataBind();
        }
        private IEnumerable<SP_OBTER_PRODUTO_ACABADOResult> Filtrar(IEnumerable<SP_OBTER_PRODUTO_ACABADOResult> lista)
        {
            if (lista != null)
            {
                if (ddlFiltroPedidoVarejo.SelectedValue != "")
                {
                    if (ddlFiltroPedidoVarejo.SelectedValue == "N")
                        lista = lista.Where(p => p.PED_VAREJO == "");
                    else
                        lista = lista.Where(p => p.PED_VAREJO != "");
                }

                if (ddlFiltroPedidoAtacado.SelectedValue != "")
                {
                    if (ddlFiltroPedidoAtacado.SelectedValue == "N")
                        lista = lista.Where(p => p.PED_ATACADO == "");
                    else
                        lista = lista.Where(p => p.PED_ATACADO != "");
                }

                if (ddlFiltroPedidoMostruario.SelectedValue != "")
                {
                    if (ddlFiltroPedidoMostruario.SelectedValue == "N")
                        lista = lista.Where(p => p.PED_MOSTRUARIO == "");
                    else
                        lista = lista.Where(p => p.PED_MOSTRUARIO != "");
                }

            }

            return lista;
        }
        #endregion

        #region "GERAR PEDIDO"
        protected void btGerarPedido_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    string desenvProduto = "";
                    string mostruario = "";
                    desenvProduto = b.CommandArgument;
                    mostruario = (chkMostruario.Checked) ? "S" : "N";

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('pacab_pedido_edit.aspx?d=" + desenvProduto + "&m=" + mostruario + "');";
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

        private List<SP_OBTER_PRODUTO_ACABADOResult> ObterProdutoAcabado(string colecao, int? desenvOrigem, string grupo, string griffe, string produto, char mostruario)
        {
            var produtoAcabado = desenvController.ObterProdutoAcabado(colecao, desenvOrigem, grupo, griffe, produto, mostruario);

            if (ddlFabricante.SelectedValue != "")
                produtoAcabado = produtoAcabado.Where(p => p.FORNECEDOR == ddlFabricante.SelectedValue.Trim()).ToList();

            return produtoAcabado;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            int? desenvOrigem = null;
            char mostruario = 'N';

            try
            {
                labMsg.Text = "";

                if (ddlColecoes.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Coleção.";
                    return;
                }


                if (ddlOrigem.SelectedValue != "0")
                    desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);


                if (chkMostruario.Checked)
                    mostruario = 'S';

                CarregarProdutoAcabado(ddlColecoes.SelectedValue.Trim(), desenvOrigem, ddlGrupo.SelectedValue.Trim(), ddlGriffe.SelectedValue.Trim(), txtProduto.Text.Trim().ToUpper(), mostruario);
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        protected void btImprimir_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";

            if (b != null)
            {
                try
                {
                    string desenvProduto = "";
                    string mostruario = "";
                    desenvProduto = b.CommandArgument;
                    mostruario = (chkMostruario.Checked) ? "S" : "N";

                    GerarRelatorio(Convert.ToInt32(desenvProduto), mostruario);

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

        #region "RELATORIO"
        private void GerarRelatorio(int codigoProduto, string mostruario)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "PROD_ACAB_" + mostruario + "_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(codigoProduto, mostruario));
                wr.Flush();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
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
        private StringBuilder MontarCabecalho(StringBuilder _texto)
        {
            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append("     <head>");
            _texto.Append("         <title>Produto Acabado - Ficha</title>   ");
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


            return _texto;
        }
        private StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.Append("</body>");
            _texto.Append("</html>");

            return _texto;
        }
        private StringBuilder MontarRelatorioHTML(int codigoProduto, string mostruario)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarLogisticaProdutoAcabado(_texto, codigoProduto, mostruario);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        public StringBuilder MontarLogisticaProdutoAcabado(StringBuilder _texto, int codigoProduto, string mostruario)
        {
            ProducaoController prodController = new ProducaoController();
            BaseController baseController = new BaseController();

            int contLinha = 0;
            int tLinhaVarejo = 6;
            int tLinhaAtacado = 5;
            int tLinhaControleQ = 8;

            var desenvProduto = desenvController.ObterProduto(codigoProduto);

            string pcVarejo = "-";
            string pcAtacado = "-";
            string pcMostruario = "-";

            string dataPedidoVarejo = "-";
            string dataPedidoAtacado = "-";
            string dataPedidoMostruario = "-";

            string dataEntregaVarejo = "-";
            string dataEntregaAtacado = "-";
            string dataEntregaMostruario = "-";

            string custo = "";
            string valorTotal = "";
            string fornecedor = desenvProduto.FORNECEDOR.Trim();

            int qtdeTotal = 0;

            SP_OBTER_PRODUTO_COMPRAResult atacadoQtde = null;
            SP_OBTER_PRODUTO_COMPRAResult varejoQtde = null;
            SP_OBTER_PRODUTO_COMPRAResult mostruarioQtde = null;

            var pedidoLinx = desenvController.ObterProdutoPedidoLinx(codigoProduto.ToString(), Convert.ToChar(mostruario));
            if (pedidoLinx != null && pedidoLinx.Count() > 0)
            {
                var m = pedidoLinx.Where(p => p.FILIAL.Trim().Contains("CD MOSTRUARIO")).FirstOrDefault();
                if (m != null)
                {
                    pcMostruario = m.PEDIDO;
                    dataPedidoMostruario = m.DATA_INCLUSAO.ToString("dd/MM/yyyy");
                    dataEntregaMostruario = (m.DATA_ENTREGA == null) ? "-" : Convert.ToDateTime(m.DATA_ENTREGA).ToString("dd/MM/yyyy");
                    custo = (m.CUSTO == null) ? "-" : Convert.ToDecimal(m.CUSTO).ToString("###,###,##0.00");
                    fornecedor = m.FORNECEDOR;
                }

                var a = pedidoLinx.Where(p => p.FILIAL.Trim().Contains("ATACADO HANDBOOK")).FirstOrDefault();
                if (a != null)
                {
                    pcAtacado = a.PEDIDO;
                    dataPedidoAtacado = a.DATA_INCLUSAO.ToString("dd/MM/yyyy");
                    dataEntregaAtacado = (a.DATA_ENTREGA == null) ? "-" : Convert.ToDateTime(a.DATA_ENTREGA).ToString("dd/MM/yyyy");
                    custo = (a.CUSTO == null) ? "-" : Convert.ToDecimal(a.CUSTO).ToString("###,###,##0.00");
                    fornecedor = a.FORNECEDOR;
                }

                var v = pedidoLinx.Where(p => p.FILIAL.Trim().Contains("CD - LUGZY")).FirstOrDefault();
                if (v != null)
                {
                    pcVarejo = v.PEDIDO;
                    dataPedidoVarejo = v.DATA_INCLUSAO.ToString("dd/MM/yyyy");
                    dataEntregaVarejo = (v.DATA_ENTREGA == null) ? "-" : Convert.ToDateTime(v.DATA_ENTREGA).ToString("dd/MM/yyyy");
                    custo = (v.CUSTO == null) ? "-" : Convert.ToDecimal(v.CUSTO).ToString("###,###,##0.00");
                    fornecedor = v.FORNECEDOR;
                }

                atacadoQtde = desenvController.ObterProdutoAcabado(desenvProduto.MODELO, desenvProduto.COR, pcAtacado);
                varejoQtde = desenvController.ObterProdutoAcabado(desenvProduto.MODELO, desenvProduto.COR, pcVarejo);
                mostruarioQtde = desenvController.ObterProdutoAcabado(desenvProduto.MODELO, desenvProduto.COR, pcMostruario);

                qtdeTotal = Convert.ToInt32(((atacadoQtde != null) ? atacadoQtde.QTDE_ORIGINAL : 0) + ((varejoQtde != null) ? varejoQtde.QTDE_ORIGINAL : 0) + ((mostruarioQtde != null) ? mostruarioQtde.QTDE_ORIGINAL : 0));

                valorTotal = "R$ " + Convert.ToDecimal((((((atacadoQtde != null) ? atacadoQtde.QTDE_ORIGINAL : 0) + ((varejoQtde != null) ? varejoQtde.QTDE_ORIGINAL : 0) + ((mostruarioQtde != null) ? mostruarioQtde.QTDE_ORIGINAL : 0)))) * Convert.ToDecimal(((custo == "-") ? "0" : custo))).ToString("###,###,###,##0.00");

            }

            _texto.AppendLine(" <br />");
            _texto.AppendLine("    <span>LOGÍSTICA - PRODUTO ACABADO</span>");
            _texto.AppendLine("<div id='divLog' align='center'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("            background: white; white-space: nowrap;'>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 20px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr style='text-align: left; border-top: 2px solid #000; border-bottom: none;'>");
            _texto.AppendLine("                            <td rowspan='7' style='border: 1px solid #000;'>");
            _texto.AppendLine("                                <img alt='Foto Peça' width='90' height='120' src='..\\.." + desenvProduto.FOTO.Replace("~", "").Replace("/", "\\") + "' />");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 184px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;" + ((mostruario == "S") ? "MOSTRUÁRIO" : "PRODUÇÃO"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 200px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;COLEÇÃO: " + baseController.BuscaColecaoAtual(desenvProduto.COLECAO).DESC_COLECAO.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
            _texto.AppendLine("                                &nbsp;PRODUTO ACABADO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;QTDE: " + qtdeTotal.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;NOME: " + desenvProduto.GRUPO + " " + desenvProduto.DESC_MODELO.Replace(desenvProduto.GRUPO, ""));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
            _texto.AppendLine("                                &nbsp;GRIFFE: " + desenvProduto.GRIFFE);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;PRODUTO: " + desenvProduto.MODELO);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;COR: " + desenvProduto.COR + " - " + prodController.ObterCoresBasicas(desenvProduto.COR).DESC_COR.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;CUSTO: " + custo);
            _texto.AppendLine("                            </td>");

            decimal preco = 0;
            var precoTO = baseController.BuscaPrecoOriginalProduto(desenvProduto.MODELO.Trim());
            if (precoTO != null && precoTO.PRECO1 > 0)
                preco = Convert.ToDecimal(precoTO.PRECO1);
            else
            {
                var precoTL = baseController.BuscaPrecoLojaProduto(desenvProduto.MODELO.Trim());
                if (precoTL != null && precoTL.PRECO1 > 0)
                    preco = Convert.ToDecimal(precoTL.PRECO1);
            }


            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
            _texto.AppendLine("                                 &nbsp;VENDA: R$ " + preco.ToString("###,###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;FORNECEDOR: " + desenvProduto.FORNECEDOR.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Total: " + valorTotal);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;P.C. Mostruário: " + pcMostruario);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;Data Pedido: " + dataPedidoMostruario);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Entrega: " + dataEntregaMostruario);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;P.C. Atacado: " + pcAtacado);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;Data Pedido: " + dataPedidoAtacado);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Entrega: " + dataEntregaAtacado);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;P.C. Varejo: " + pcVarejo);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;Data Pedido: " + dataPedidoVarejo);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Entrega: " + dataEntregaVarejo);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");


            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td colspan='4'>");
            _texto.AppendLine("                                <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse; width: 517pt;'>");
            _texto.AppendLine("                                    <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                                        <td style=' text-align: left; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            -");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "XP")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "PP")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "P")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "M")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "G")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "GG")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            TOTAL");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;MOSTRUÁRIO");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO1.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO2.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO3.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO4.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO5.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO6.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? (mostruarioQtde.CO1 + mostruarioQtde.CO2 + mostruarioQtde.CO3 + mostruarioQtde.CO4 + mostruarioQtde.CO5 + mostruarioQtde.CO6).ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;VAREJO");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO1.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO2.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO3.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO4.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO5.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO6.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? (varejoQtde.CO1 + varejoQtde.CO2 + varejoQtde.CO3 + varejoQtde.CO4 + varejoQtde.CO5 + varejoQtde.CO6).ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;ATACADO");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO1.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO2.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO3.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO4.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO5.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO6.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? (atacadoQtde.CO1 + atacadoQtde.CO2 + atacadoQtde.CO3 + atacadoQtde.CO4 + atacadoQtde.CO5 + atacadoQtde.CO6).ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;TOTAL");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO1 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO1 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO1 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO2 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO2 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO2 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO3 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO3 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO3 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO4 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO4 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO4 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO5 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO5 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO5 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO6 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO6 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO6 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " +
                                        (((varejoQtde != null) ? (varejoQtde.CO1 + varejoQtde.CO2 + varejoQtde.CO3 + varejoQtde.CO4 + varejoQtde.CO5 + varejoQtde.CO6) : 0)
                                        +
                                        ((mostruarioQtde != null) ? (mostruarioQtde.CO1 + mostruarioQtde.CO2 + mostruarioQtde.CO3 + mostruarioQtde.CO4 + mostruarioQtde.CO5 + mostruarioQtde.CO6) : 0)
                                        +
                                        ((atacadoQtde != null) ? (atacadoQtde.CO1 + atacadoQtde.CO2 + atacadoQtde.CO3 + atacadoQtde.CO4 + atacadoQtde.CO5 + atacadoQtde.CO6) : 0)).ToString()

                );
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                </table>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            //VAREJO
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                <span style='font-size: 11px;'>ESTOQUE VAREJO</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                -");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            //INICIO LOOP ESTOQUE VAREJO
            for (contLinha = 1; contLinha <= tLinhaVarejo; contLinha++)
            {
                _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }
            // FIM LOOP ESTOQUE VAREJO

            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL FINAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            //ATACADO
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                <span style='font-size: 11px;'>ESTOQUE ATACADO</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                ");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            //INICIO LOOP ESTOQUE ATACADO
            for (contLinha = 1; contLinha <= tLinhaAtacado; contLinha++)
            {
                _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }
            // FIM LOOP ESTOQUE ATACADO

            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL FINAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            //CONTROLE DE QUALIDADE
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                <span style='font-size: 11px;'>CONTROLE DE QUALIDADE</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                ");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            //INICIO LOOP CONTROLE DE QUALIDADE
            for (contLinha = 1; contLinha <= tLinhaControleQ; contLinha++)
            {
                _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }
            // FIM LOOP CONTROLE DE QUALIDADE

            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL FINAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            //RODAPE
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                PILOTO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                MOSTRUÁRIO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='8' style='text-align:left;'>");
            _texto.AppendLine("                                SCS - 2ª QUALIDADE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                -");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                PERDA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                AMOSTRA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 12px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%;'>");
            _texto.AppendLine("                        <tr style='border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='width: 100%; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                OBSERVAÇÕES:");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; border-top: 1px solid #FFF; line-height: 15px;'>");
            _texto.AppendLine("                            <td style='text-align: center;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("</div>");

            return _texto;
        }
        #endregion


    }
}

