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
    public partial class prod_cad_produto_custo_novo : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        ProdutoCusto pc = new ProdutoCusto();
        int colTecido = 0;
        int colAviamento = 0;
        int colServico = 0;

        decimal totalTecidoCusto = 0;
        decimal totalTecidoPreco = 0;
        decimal totalAviamento = 0;
        decimal totalServicoCusto = 0;
        decimal totalServicoPreco = 0;

        List<PROD_FORNECEDOR> lstFornecedorTecido = new List<PROD_FORNECEDOR>();
        List<PROD_FORNECEDOR> lstFornecedorAviamento = new List<PROD_FORNECEDOR>();
        List<PROD_FORNECEDOR> lstFornecedorServico = new List<PROD_FORNECEDOR>();
        List<PROD_SERVICO> lstServicoProducao = new List<PROD_SERVICO>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();


                string colecao = "";
                string produto = "";
                string mostruario = "";
                if (Request.QueryString["c"] != null && Request.QueryString["c"].ToString() != "")
                    colecao = Request.QueryString["c"].ToString();

                if (Request.QueryString["p"] != null && Request.QueryString["p"].ToString() != "")
                    produto = Request.QueryString["p"].ToString();

                if (Request.QueryString["m"] != null && Request.QueryString["m"].ToString() != "")
                    mostruario = Request.QueryString["m"].ToString();

                if (colecao != "0" && produto != "0" && mostruario != "0")
                {
                    ddlColecoes.SelectedValue = colecao;
                    txtProdutoLinxFiltro.Text = produto;
                    chkMostruario.Checked = (mostruario == "S") ? true : false;
                    btBuscar_Click(null, null);
                }

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btGerarCusto.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGerarCusto, null) + ";");
        }

        #region "INICIALIZAR DADOS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private List<PROD_FORNECEDOR> CarregarFornecedores(char tipo)
        {
            List<PROD_FORNECEDOR> _fornecedores = new List<PROD_FORNECEDOR>();
            _fornecedores = prodController.ObterFornecedor();
            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });
                return _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == tipo) || p.STATUS == 'S').ToList();
            }
            return null;
        }
        private void CarregarServicosProducao()
        {
            List<PROD_SERVICO> _servico = new List<PROD_SERVICO>();
            _servico = prodController.ObterServicoProducao().Where(p => p.STATUS == 'A').ToList();
            if (_servico != null)
            {
                _servico.Insert(0, new PROD_SERVICO { DESCRICAO = "Selecione", STATUS = 'A' });
                lstServicoProducao = _servico;
            }
        }
        private PROD_HB ObterProduto()
        {
            var produto = new PROD_HB();
            produto = prodController.ObterHB().Where(p => p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim() && p.CODIGO_PRODUTO_LINX.Trim() == txtProdutoLinxFiltro.Text.Trim() && p.STATUS == 'B' && p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).Take(1).SingleOrDefault();
            if (produto == null)
                produto = prodController.ObterHB().Where(p => p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim() && p.NOME.Trim().ToUpper() == txtNomeFiltro.Text.Trim().ToUpper() && p.STATUS == 'B' && p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).Take(1).SingleOrDefault();

            return produto;
        }
        #endregion

        #region "AÇÕES"
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            try
            {
                hidCodigoHB.Value = "";
                pnlCusto.Visible = false;
                if (ddlColecoes.SelectedValue.Trim() == "" || ddlColecoes.SelectedValue.Trim() == "0")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }
                if (txtProdutoLinxFiltro.Text.Trim() == "" && txtNomeFiltro.Text.Trim() == "")
                {
                    labErro.Text = "Informe Código e/ou Nome do Produto.";
                    return;
                }
                if (!ValidarCodigoLinx())
                {
                    labErro.Text = "Produto informado INVÁLIDO para geração de CUSTO. Não existe cadastro no LINX.";
                    return;
                }

                //var colecao = ddlColecoes.SelectedValue.Trim();
                //if (colecao == "22" || colecao == "23" || colecao == "24" || colecao == "25" || colecao == "26")
                //{
                //    labErro.Text = "Utilize esta tela para aprovar preços da Coleção 27+.";
                //    return;
                //}


                PROD_HB produto = ObterProduto();

                if (produto == null)
                    throw new Exception("PRODUTO NÃO POSSUI HB.");

                if (!ValidarProdutoCortado(produto))
                    throw new Exception("PRODUTO ainda não foi CORTADO.");

                if (!ValidarProdutoPrecoAprovado(produto))
                    throw new Exception("PRODUTO já possui preço APROVADO.");


                PROD_HB_ROTA rotaHB = null;
                if (produto.ORDEM_PRODUCAO != null)
                {
                    rotaHB = prodController.ObterRotaOP(produto.ORDEM_PRODUCAO);
                    //VERIFICAR ROTAS
                    if (rotaHB == null)
                        throw new Exception("HB " + produto.HB.ToString() + " não possui ROTA. Entre em contato com TI.");
                }
                else
                {
                    throw new Exception("HB " + produto.HB.ToString() + " não possui OP. Entre em contato com TI.");
                }

                var desenvProduto = desenvController.ObterProduto(produto.COLECAO, produto.CODIGO_PRODUTO_LINX, produto.COR);

                txtHB.Text = produto.HB.ToString();
                txtProduto.Text = produto.CODIGO_PRODUTO_LINX;
                txtGrupo.Text = produto.GRUPO.Trim();
                txtNome.Text = produto.NOME.Trim();
                txtGriffe.Text = (desenvProduto == null) ? "DesenvProduto Não Encontrado" : desenvProduto.GRIFFE;
                txtMostruario.Text = (produto.MOSTRUARIO == 'N') ? "Não" : "Sim";
                imgFotoPeca.ImageUrl = produto.FOTO_PECA;

                CarregarTecidos(produto);
                CarregarAviamentos(produto);
                CarregarServicos(produto);
                CalcularCustosVariavelCol(produto.COLECAO.Trim());

                hidCodigoHB.Value = produto.CODIGO.ToString();

                pnlCusto.Visible = true;
                Session["COLECAO"] = ddlColecoes.SelectedValue;

                gvTecidoNovo.Visible = true;
                gvAviamentoNovo.Visible = true;
                if (produto.MOSTRUARIO == 'N')
                {
                    gvTecidoNovo.Visible = false;
                    gvAviamentoNovo.Visible = false;
                }

                var simulacao = prodController.ObterCustoSimulacao(produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(produto.MOSTRUARIO), 'S');
                chkFinalizado.Checked = false;
                if (simulacao != null && simulacao.Count() > 0)
                    chkFinalizado.Checked = true;

            }
            catch (Exception ex)
            {
                labErro.Text = "" + ex.Message;
            }
        }
        #endregion

        #region "GRIDS"
        //TECIDO
        private void CarregarTecidos(PROD_HB produto)
        {
            List<PROD_HB> lstTecidos = new List<PROD_HB>();

            if (produto != null)
            {
                //Adiciona pai na lista
                lstTecidos.Add(produto);

                List<PROD_HB> tecidoDetalhe = new List<PROD_HB>();
                tecidoDetalhe = prodController.ObterDetalhesHB(produto.CODIGO);
                foreach (PROD_HB det in tecidoDetalhe)
                {
                    if (det != null)
                        lstTecidos.Add(det);
                }

                if (lstTecidos != null && lstTecidos.Count > 0)
                {
                    gvTecido.DataSource = lstTecidos;
                    gvTecido.DataBind();
                }

                lstFornecedorTecido = CarregarFornecedores('T');
                var materialExtra = prodController.ObterMaterialExtra(produto.CODIGO, 'T');

                if (materialExtra == null || materialExtra.Count() <= 0)
                    materialExtra.Insert(0, new PROD_HB_MATERIAL_EXTRA { CODIGO = 0 });

                gvTecidoNovo.DataSource = materialExtra;
                gvTecidoNovo.DataBind();
            }
        }
        protected void gvTecido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal precoTecido = 0;
            decimal consumoTecidoCusto = 0;
            decimal consumoTecidoPreco = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _tecido = e.Row.DataItem as PROD_HB;

                    colTecido += 1;
                    if (_tecido != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colTecido.ToString();

                        int totalGrade = 0;
                        totalGrade = prodController.ObterQtdeGradeHB(_tecido.CODIGO, 3);

                        Literal _litFornecedor = e.Row.FindControl("litFornecedor") as Literal;
                        if (_litFornecedor != null)
                            _litFornecedor.Text = _tecido.FORNECEDOR.Trim().Substring(0, ((_tecido.FORNECEDOR.Trim().Length <= 17) ? (_tecido.FORNECEDOR.Trim().Length - 1) : 17));

                        Literal _litTecido = e.Row.FindControl("litTecido") as Literal;
                        if (_litTecido != null)
                            _litTecido.Text = _tecido.TECIDO.Trim() + " -  " + ((_tecido.UNIDADE_MEDIDA == null) ? "" : (_tecido.UNIDADE_MEDIDA1.DESCRICAO + "s"));

                        Literal _litDetalhe = e.Row.FindControl("litDetalhe") as Literal;
                        if (_litDetalhe != null)
                            _litDetalhe.Text = (_tecido.PROD_DETALHE == null) ? "PRINCIPAL" : _tecido.PROD_DETALHE1.DESCRICAO;

                        Literal _grade = e.Row.FindControl("litGrade") as Literal;
                        if (_grade != null)
                            _grade.Text = totalGrade.ToString();

                        Literal _litICMS = e.Row.FindControl("litICMS") as Literal;
                        if (_litICMS != null)
                            _litICMS.Text = prodController.ObterFornecedor().Where(p =>
                                                                                    p.FORNECEDOR.Trim().ToUpper() == _tecido.FORNECEDOR.Trim().ToUpper() &&
                                                                                    p.TIPO == 'T').SingleOrDefault().PORC_ICMS.ToString();

                        Literal _precoTecido = e.Row.FindControl("litPrecoTecido") as Literal;
                        if (_precoTecido != null)
                        {
                            precoTecido = Convert.ToDecimal(_tecido.CUSTO_TECIDO);
                            _precoTecido.Text = precoTecido.ToString("###,###,##0.00");
                        }

                        Literal _litConsumoCusto = e.Row.FindControl("litConsumoCusto") as Literal;
                        if (_litConsumoCusto != null)
                        {
                            decimal gastoPorCorte = 0;
                            gastoPorCorte = Convert.ToDecimal(_tecido.GASTO_FOLHA * totalGrade);
                            consumoTecidoCusto = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte + _tecido.RETALHOS) / totalGrade));

                            _litConsumoCusto.Text = consumoTecidoCusto.ToString("###,###,##0.00");
                        }

                        Literal _litConsumoPreco = e.Row.FindControl("litConsumoPreco") as Literal;
                        if (_litConsumoPreco != null)
                        {
                            decimal gastoPorCorte = 0;
                            if (_tecido.GASTO_PECA_CUSTO == null)
                            {
                                gastoPorCorte = Convert.ToDecimal(_tecido.GASTO_FOLHA * totalGrade);
                                consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte + _tecido.RETALHOS) / totalGrade));
                            }
                            else
                            {
                                gastoPorCorte = Convert.ToDecimal(_tecido.GASTO_PECA_CUSTO * totalGrade);
                                consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte) / totalGrade));
                            }

                            _litConsumoPreco.Text = consumoTecidoPreco.ToString("###,###,##0.00");
                        }

                        Literal _litTotalCusto = e.Row.FindControl("litTotalCusto") as Literal;
                        if (_litTotalCusto != null)
                        {
                            decimal total = 0;
                            total = (precoTecido * consumoTecidoCusto);

                            if (chkMostruario.Checked)
                                total = total + (total * Convert.ToDecimal(10.000 / 100.000));

                            _litTotalCusto.Text = total.ToString("###,###,##0.00");

                            //somatorio
                            totalTecidoCusto += total;
                        }

                        Literal _litTotalPreco = e.Row.FindControl("litTotalPreco") as Literal;
                        if (_litTotalPreco != null)
                        {
                            decimal total = 0;
                            total = (precoTecido * consumoTecidoPreco);

                            if (chkMostruario.Checked)
                                total = total + (total * Convert.ToDecimal(10.000 / 100.000));

                            _litTotalPreco.Text = total.ToString("###,###,##0.00");

                            //somatorio
                            totalTecidoPreco += total;
                        }
                    }
                }
            }
        }
        protected void gvTecido_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvTecido.FooterRow;
            if (footer != null)
            {
                /*footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[10].Text = totalTecidoCusto.ToString("###,###,###,##0.000");
                footer.Cells[11].Text = totalTecidoPreco.ToString("###,###,###,##0.000");*/
            }
        }

        protected void gvTecidoNovo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_MATERIAL_EXTRA _tecido = e.Row.DataItem as PROD_HB_MATERIAL_EXTRA;

                    colTecido += 1;
                    if (_tecido != null)
                    {

                        if (_tecido.CODIGO == 0)
                        {
                            e.Row.Visible = false;
                            return;
                        }

                        Literal _litDetalhe = e.Row.FindControl("litDetalhe") as Literal;
                        if (_litDetalhe != null)
                            _litDetalhe.Text = "";

                        Literal _medida = e.Row.FindControl("litMedida") as Literal;
                        if (_medida != null)
                            _medida.Text = "";

                        int totalGrade = 0;
                        totalGrade = prodController.ObterQtdeGradeHB(_tecido.PROD_HB, 3);
                        Literal _grade = e.Row.FindControl("litGrade") as Literal;
                        if (_grade != null)
                            _grade.Text = totalGrade.ToString();

                        Literal _litICMS = e.Row.FindControl("litICMS") as Literal;
                        if (_litICMS != null)
                            _litICMS.Text = prodController.ObterFornecedor().Where(p =>
                                                                                    p.FORNECEDOR.Trim().ToUpper() == _tecido.FORNECEDOR.Trim().ToUpper() &&
                                                                                    p.TIPO == 'T').SingleOrDefault().PORC_ICMS.ToString();

                        Literal _precoTecido = e.Row.FindControl("litPrecoTecido") as Literal;
                        if (_precoTecido != null)
                            _precoTecido.Text = _tecido.PRECO.ToString("###,###,##0.00");

                        Literal _litConsumoCusto = e.Row.FindControl("litConsumoCusto") as Literal;
                        if (_litConsumoCusto != null)
                            _litConsumoCusto.Text = _tecido.CONSUMO_CUSTO.ToString("###,###,##0.00");

                        Literal _litConsumoPreco = e.Row.FindControl("litConsumoPreco") as Literal;
                        if (_litConsumoPreco != null)
                            _litConsumoPreco.Text = _tecido.CONSUMO_PRECO.ToString("###,###,##0.00");

                        Literal _litTotalCusto = e.Row.FindControl("litTotalCusto") as Literal;
                        if (_litTotalCusto != null)
                            _litTotalCusto.Text = (_tecido.PRECO * _tecido.CONSUMO_CUSTO).ToString("###,###,##0.00");

                        Literal _litTotalPreco = e.Row.FindControl("litTotalPreco") as Literal;
                        if (_litTotalPreco != null)
                            _litTotalPreco.Text = (_tecido.PRECO * _tecido.CONSUMO_PRECO).ToString("###,###,##0.00");
                    }
                }
            }
        }
        protected void gvTecidoNovo_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvTecidoNovo.FooterRow;
            if (footer != null)
            {
                DropDownList _ddlFornecedor = footer.FindControl("ddlFornecedor") as DropDownList;
                if (_ddlFornecedor != null)
                {
                    _ddlFornecedor.DataSource = lstFornecedorTecido;
                    _ddlFornecedor.DataBind();
                }
            }
        }
        protected void btIncluirTecido_Click(object sender, EventArgs e)
        {
            labErroTecido.Text = "";
            try
            {

                GridViewRow row = gvTecidoNovo.FooterRow;
                if (row != null)
                {
                    DropDownList _ddlFornecedor = row.FindControl("ddlFornecedor") as DropDownList;
                    TextBox _txtTecido = row.FindControl("txtTecido") as TextBox;
                    TextBox _txtPreco = row.FindControl("txtPreco") as TextBox;
                    TextBox _txtConsumoCusto = row.FindControl("txtConsumoCusto") as TextBox;
                    TextBox _txtConsumoPreco = row.FindControl("txtConsumoPreco") as TextBox;

                    if (_ddlFornecedor.SelectedValue == "Selecione")
                    {
                        labErroTecido.Text = "Selecione o Fornecedor";
                        return;
                    }

                    if (_txtTecido.Text.Trim() == "")
                    {
                        labErroTecido.Text = "Informe o Tecido.";
                        return;
                    }

                    if (_txtPreco.Text.Trim() == "")
                    {
                        labErroTecido.Text = "Informe o Preço do Tecido.";
                        return;
                    }

                    if (_txtConsumoCusto.Text.Trim() == "")
                    {
                        labErroTecido.Text = "Informe o Consumo para CUSTO.";
                        return;
                    }

                    if (_txtConsumoPreco.Text.Trim() == "")
                    {
                        labErroTecido.Text = "Informe o Consumo para PREÇO.";
                        return;
                    }

                    PROD_HB_MATERIAL_EXTRA matExtra = new PROD_HB_MATERIAL_EXTRA();
                    matExtra.PROD_HB = Convert.ToInt32(hidCodigoHB.Value);
                    matExtra.FORNECEDOR = _ddlFornecedor.SelectedValue;
                    matExtra.MATERIAL = _txtTecido.Text.Trim().ToUpper();

                    decimal icms = 0;
                    var f = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == _ddlFornecedor.SelectedValue).ToList();
                    if (f != null && f.Count() > 0)
                        icms = f[0].PORC_ICMS;
                    else
                        icms = 12;

                    matExtra.ICMS = icms;
                    matExtra.PRECO = Convert.ToDecimal(_txtPreco.Text);
                    matExtra.CONSUMO_CUSTO = Convert.ToDecimal(_txtConsumoCusto.Text);
                    matExtra.CONSUMO_PRECO = Convert.ToDecimal(_txtConsumoPreco.Text);
                    matExtra.TIPO_MATERIAL = 'T';
                    matExtra.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    matExtra.DATA_INCLUSAO = DateTime.Now;

                    prodController.InserirMaterialExtra(matExtra);

                    var prodHB = prodController.ObterHB(Convert.ToInt32(hidCodigoHB.Value));
                    CarregarTecidos(prodHB);

                }
            }
            catch (Exception ex)
            {
                labErroTecido.Text = ex.Message;
            }
        }
        protected void btExcluirTecido_Click(object sender, EventArgs e)
        {
            try
            {

                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    GridViewRow row = (GridViewRow)bt.NamingContainer;
                    if (row != null)
                    {
                        int codigoMaterial = Convert.ToInt32(gvTecidoNovo.DataKeys[row.RowIndex].Value);
                        if (codigoMaterial > 0)
                        {
                            prodController.ExcluirMaterialExtra(codigoMaterial);

                            var prodHB = prodController.ObterHB(Convert.ToInt32(hidCodigoHB.Value));
                            CarregarTecidos(prodHB);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labErroTecido.Text = ex.Message;
            }
        }

        //AVIAMENTO
        private void GravarValoresAviamento()
        {
            int codigo = 0;

            PROD_HB_PROD_AVIAMENTO aviamento = null;
            foreach (GridViewRow row in gvAviamento.Rows)
            {
                if (row != null)
                {
                    TextBox _txtConsumo = row.FindControl("txtConsumo") as TextBox;
                    TextBox _txtPreco = row.FindControl("txtPreco") as TextBox;

                    if (_txtConsumo != null && _txtConsumo.Text.Trim() != "" && _txtPreco != null && _txtPreco.Text.Trim() != "")
                    {

                        aviamento = new PROD_HB_PROD_AVIAMENTO();

                        //Obter codigo tabela PROD_HB_CUSTO_SERVICO
                        codigo = Convert.ToInt32(gvAviamento.DataKeys[row.RowIndex].Value);
                        aviamento.CODIGO = codigo;
                        aviamento.CONSUMO = Convert.ToDecimal(_txtConsumo.Text.Trim());
                        aviamento.CUSTO_UNITARIO = Convert.ToDecimal(_txtPreco.Text.Trim());

                        prodController.AtualizarAviamentoCusto(aviamento);
                    }
                }
            }
        }
        private void SomarTotalAviamento()
        {
            decimal valorAviamentoColuna = 0;
            Literal litTotal = null;

            foreach (GridViewRow row in gvAviamento.Rows)
            {
                if (row != null)
                {
                    litTotal = row.FindControl("litTotal") as Literal;
                    if (litTotal != null)
                    {
                        valorAviamentoColuna = (litTotal.Text.Trim() == "") ? 0 : Convert.ToDecimal(litTotal.Text.Trim());
                        totalAviamento += valorAviamentoColuna;
                    }
                }
            }

            GridViewRow footer = gvAviamento.FooterRow;
            if (footer != null)
                footer.Cells[6].Text = totalAviamento.ToString("###,###,###,##0.00");
        }
        private void CarregarAviamentos(PROD_HB produto)
        {
            List<SP_OBTER_HB_AVIAMENTO_CUSTOResult> lstAviamentos = new List<SP_OBTER_HB_AVIAMENTO_CUSTOResult>();

            if (produto != null)
            {
                lstAviamentos = prodController.ObterAviamentoCusto(produto.CODIGO);

                gvAviamento.DataSource = lstAviamentos;
                gvAviamento.DataBind();

                gvAviamentoNovo.ShowHeader = false;
                if (lstAviamentos == null || lstAviamentos.Count <= 0)
                    gvAviamentoNovo.ShowHeader = true;

                lstFornecedorAviamento = CarregarFornecedores('A');
                var materialExtra = prodController.ObterMaterialExtra(produto.CODIGO, 'A');

                if (materialExtra == null || materialExtra.Count() <= 0)
                    materialExtra.Insert(0, new PROD_HB_MATERIAL_EXTRA { CODIGO = 0 });

                gvAviamentoNovo.DataSource = materialExtra;
                gvAviamentoNovo.DataBind();
            }
        }
        protected void gvAviamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HB_AVIAMENTO_CUSTOResult _aviamento = e.Row.DataItem as SP_OBTER_HB_AVIAMENTO_CUSTOResult;

                    colAviamento += 1;
                    if (_aviamento != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colAviamento.ToString();

                        Literal _litICMS = e.Row.FindControl("litICMS") as Literal;
                        if (_litICMS != null)
                            _litICMS.Text = _aviamento.PORC_ICMS.ToString();

                        TextBox _txtPreco = e.Row.FindControl("txtPreco") as TextBox;
                        if (_txtPreco != null)
                            _txtPreco.Text = Convert.ToDecimal(_aviamento.PRECO).ToString("###,###,##0.00");

                        TextBox _txtConsumo = e.Row.FindControl("txtConsumo") as TextBox;
                        if (_txtConsumo != null)
                            _txtConsumo.Text = (_aviamento.CONSUMO == null) ? "0" : Convert.ToDecimal(_aviamento.CONSUMO).ToString("#########0.00");

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                            _litTotal.Text = ((Convert.ToDecimal(_aviamento.PRECO) * ((_aviamento.CONSUMO == null) ? 0 : Convert.ToDecimal(_aviamento.CONSUMO)))).ToString("#########0.00");


                        if (_aviamento.PRECO == null || _aviamento.PRECO <= 0)
                            _txtPreco.BackColor = Color.LightBlue;
                    }
                }
            }
        }
        protected void gvAviamento_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvAviamento.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[6].Text = totalAviamento.ToString("###,###,###,##0.00");

                SomarTotalAviamento();
            }
        }
        protected void txtConsumo_TextChanged(object sender, EventArgs e)
        {
            TextBox txtConsumo = (TextBox)sender;
            try
            {
                if (txtConsumo != null)
                {
                    if (txtConsumo.Text.Trim() != "" && Convert.ToDecimal(txtConsumo.Text) >= 0)
                    {
                        GridViewRow row = (GridViewRow)txtConsumo.NamingContainer;
                        if (row != null)
                        {
                            // Total = Preço X Consumo

                            //Obter PREÇO
                            decimal preco = 0;
                            TextBox _txtPreco = row.FindControl("txtPreco") as TextBox;
                            if (_txtPreco != null)
                                preco = (_txtPreco.Text.Trim() == "") ? 0 : Convert.ToDecimal(_txtPreco.Text.Trim());

                            decimal total = 0;
                            total = (preco * Convert.ToDecimal(txtConsumo.Text.Trim()));

                            Literal litTotal = row.FindControl("litTotal") as Literal;
                            if (litTotal != null)
                                litTotal.Text = total.ToString("###,###,##0.00");

                            //Realizar a somatoria do grid de aviamentos
                            SomarTotalAviamento();
                            GravarValoresAviamento();
                        }
                        else
                        {
                            //MSG - LINHA NAO ENCONTRADA, ENTRE EM CONTATO COM TI
                        }
                    }
                    else
                    {
                        //MSG - CONSUMO INVALIDO, CONSUMO DEVE SER MAIOR QUE ZERO
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void txtPreco_TextChanged(object sender, EventArgs e)
        {
            TextBox txtPreco = (TextBox)sender;
            try
            {
                if (txtPreco != null)
                {
                    if (txtPreco.Text.Trim() != "" && Convert.ToDecimal(txtPreco.Text) >= 0)
                    {
                        GridViewRow row = (GridViewRow)txtPreco.NamingContainer;
                        if (row != null)
                        {
                            // Total = Preço X Consumo

                            //Obter Consumo
                            decimal consumo = 0;
                            TextBox _txtConsumo = row.FindControl("txtConsumo") as TextBox;
                            if (_txtConsumo != null)
                                consumo = (_txtConsumo.Text.Trim() == "") ? 0 : Convert.ToDecimal(_txtConsumo.Text.Trim());

                            decimal total = 0;
                            total = (consumo * Convert.ToDecimal(txtPreco.Text.Trim()));

                            Literal litTotal = row.FindControl("litTotal") as Literal;
                            if (litTotal != null)
                                litTotal.Text = total.ToString("###,###,##0.00");

                            //Realizar a somatoria do grid de aviamentos
                            SomarTotalAviamento();
                            GravarValoresAviamento();
                        }
                        else
                        {
                            //MSG - LINHA NAO ENCONTRADA, ENTRE EM CONTATO COM TI
                        }
                    }
                    else
                    {
                        //MSG - CONSUMO INVALIDO, CONSUMO DEVE SER MAIOR QUE ZERO
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void gvAviamentoNovo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_MATERIAL_EXTRA _aviamento = e.Row.DataItem as PROD_HB_MATERIAL_EXTRA;

                    colAviamento += 1;
                    if (_aviamento != null)
                    {
                        if (_aviamento.CODIGO == 0)
                        {
                            e.Row.Visible = false;
                            return;
                        }

                        Literal _litICMS = e.Row.FindControl("litICMS") as Literal;
                        if (_litICMS != null)
                            _litICMS.Text = prodController.ObterFornecedor().Where(p =>
                                                                                    p.FORNECEDOR.Trim().ToUpper() == _aviamento.FORNECEDOR.Trim().ToUpper() &&
                                                                                    p.TIPO == 'A').SingleOrDefault().PORC_ICMS.ToString();

                        Literal _litPreco = e.Row.FindControl("litPreco") as Literal;
                        if (_litPreco != null)
                            _litPreco.Text = _aviamento.PRECO.ToString("#########0.00");

                        Literal _litConsumo = e.Row.FindControl("litConsumo") as Literal;
                        if (_litConsumo != null)
                            _litConsumo.Text = _aviamento.CONSUMO_PRECO.ToString("###,###,##0.00");

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                            _litTotal.Text = (_aviamento.PRECO * _aviamento.CONSUMO_PRECO).ToString("#########0.00");
                    }
                }
            }
        }
        protected void gvAviamentoNovo_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvAviamentoNovo.FooterRow;
            if (footer != null)
            {
                DropDownList _ddlFornecedor = footer.FindControl("ddlFornecedor") as DropDownList;
                if (_ddlFornecedor != null)
                {
                    _ddlFornecedor.DataSource = lstFornecedorAviamento;
                    _ddlFornecedor.DataBind();
                }
            }
        }
        protected void btIncluirAviamento_Click(object sender, EventArgs e)
        {
            labErroAviamento.Text = "";
            try
            {
                GridViewRow row = gvAviamentoNovo.FooterRow;
                if (row != null)
                {
                    DropDownList _ddlFornecedor = row.FindControl("ddlFornecedor") as DropDownList;
                    TextBox _txtAviamento = row.FindControl("txtAviamento") as TextBox;
                    TextBox _txtPreco = row.FindControl("txtPreco") as TextBox;
                    TextBox _txtConsumo = row.FindControl("txtConsumo") as TextBox;

                    if (_ddlFornecedor.SelectedValue == "Selecione")
                    {
                        labErroAviamento.Text = "Selecione o Fornecedor";
                        return;
                    }

                    if (_txtAviamento.Text.Trim() == "")
                    {
                        labErroAviamento.Text = "Informe o Aviamento.";
                        return;
                    }

                    if (_txtPreco.Text.Trim() == "")
                    {
                        labErroAviamento.Text = "Informe o Preço do Tecido.";
                        return;
                    }

                    if (_txtConsumo.Text.Trim() == "")
                    {
                        labErroAviamento.Text = "Informe o Consumo.";
                        return;
                    }

                    PROD_HB_MATERIAL_EXTRA matExtra = new PROD_HB_MATERIAL_EXTRA();
                    matExtra.PROD_HB = Convert.ToInt32(hidCodigoHB.Value);
                    matExtra.FORNECEDOR = _ddlFornecedor.SelectedValue;
                    matExtra.MATERIAL = _txtAviamento.Text.Trim().ToUpper();

                    decimal icms = 0;
                    var f = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == _ddlFornecedor.SelectedValue).ToList();
                    if (f != null && f.Count() > 0)
                        icms = f[0].PORC_ICMS;
                    else
                        icms = 12;

                    matExtra.ICMS = icms;
                    matExtra.PRECO = Convert.ToDecimal(_txtPreco.Text);
                    matExtra.CONSUMO_CUSTO = Convert.ToDecimal(_txtConsumo.Text);
                    matExtra.CONSUMO_PRECO = Convert.ToDecimal(_txtConsumo.Text);
                    matExtra.TIPO_MATERIAL = 'A';
                    matExtra.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    matExtra.DATA_INCLUSAO = DateTime.Now;

                    prodController.InserirMaterialExtra(matExtra);

                    var prodHB = prodController.ObterHB(Convert.ToInt32(hidCodigoHB.Value));
                    CarregarAviamentos(prodHB);

                }
            }
            catch (Exception ex)
            {
                labErroAviamento.Text = ex.Message;
            }
        }
        protected void btExcluirAviamento_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    GridViewRow row = (GridViewRow)bt.NamingContainer;
                    if (row != null)
                    {
                        int codigoMaterial = Convert.ToInt32(gvAviamentoNovo.DataKeys[row.RowIndex].Value);
                        if (codigoMaterial > 0)
                        {
                            prodController.ExcluirMaterialExtra(codigoMaterial);

                            var prodHB = prodController.ObterHB(Convert.ToInt32(hidCodigoHB.Value));
                            CarregarAviamentos(prodHB);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labErroAviamento.Text = ex.Message;
            }
        }

        //SERVIÇOS
        private void GravarCustoServico()
        {
            int codigo = 0;

            PROD_HB_CUSTO_SERVICO custoServico = null;
            foreach (GridViewRow row in gvServico.Rows)
            {
                if (row != null)
                {
                    DropDownList ddlFornecedor = row.FindControl("ddlFornecedor") as DropDownList;
                    DropDownList ddlServico = row.FindControl("ddlServico") as DropDownList;
                    TextBox txtValorServico = row.FindControl("txtValorServico") as TextBox;
                    TextBox txtValorPreco = row.FindControl("txtValorPreco") as TextBox;

                    if (ddlFornecedor.SelectedValue.Trim() != "Selecione" && ddlServico.SelectedValue != "" && txtValorServico.Text != "" && txtValorPreco.Text != "")
                    {

                        custoServico = new PROD_HB_CUSTO_SERVICO();

                        //Obter codigo tabela PROD_HB_CUSTO_SERVICO
                        codigo = Convert.ToInt32(gvServico.DataKeys[row.RowIndex].Value);
                        custoServico.CODIGO = codigo;
                        custoServico.PRODUTO = txtProduto.Text.Trim();
                        custoServico.FORNECEDOR = ddlFornecedor.SelectedValue;
                        custoServico.SERVICO = Convert.ToInt32(ddlServico.SelectedValue);
                        custoServico.CUSTO = Convert.ToDecimal(txtValorServico.Text);
                        custoServico.CUSTO_PECA = Convert.ToDecimal(txtValorPreco.Text);
                        custoServico.MOSTRUARIO = (txtMostruario.Text.Trim().ToUpper() == "SIM") ? 'S' : 'N';
                        custoServico.DATA_INCLUSAO = DateTime.Now;
                        custoServico.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                        if (codigo <= 0) //Insert
                            prodController.InserirCustoServico(custoServico);
                        else
                            prodController.AtualizarCustoServico(custoServico);
                    }
                }
            }
        }
        private void SomarTotalServico()
        {
            decimal valorServicoColuna = 0;
            decimal valorPrecoColuna = 0;
            TextBox _txtValorServico = null;
            TextBox _txtValorPreco = null;

            foreach (GridViewRow row in gvServico.Rows)
            {
                if (row != null)
                {
                    _txtValorServico = row.FindControl("txtValorServico") as TextBox;
                    _txtValorPreco = row.FindControl("txtValorPreco") as TextBox;

                    if (_txtValorServico != null)
                    {
                        valorServicoColuna = (_txtValorServico.Text.Trim() == "") ? 0 : Convert.ToDecimal(_txtValorServico.Text.Trim());
                        totalServicoCusto += valorServicoColuna;
                    }

                    if (_txtValorPreco != null)
                    {
                        valorPrecoColuna = (_txtValorPreco.Text.Trim() == "") ? 0 : Convert.ToDecimal(_txtValorPreco.Text.Trim());
                        totalServicoPreco += valorPrecoColuna;
                    }
                }
            }

            GridViewRow footer = gvServico.FooterRow;
            if (footer != null)
            {
                footer.Cells[3].Text = totalServicoCusto.ToString("###,###,###,##0.00");
                footer.Cells[4].Text = totalServicoPreco.ToString("###,###,###,##0.00");
            }
        }
        private void CarregarServicos(PROD_HB produto)
        {
            List<PROD_HB_CUSTO_SERVICO> lstServicosFinal = new List<PROD_HB_CUSTO_SERVICO>();
            List<PROD_HB_CUSTO_SERVICO> lstServicos = new List<PROD_HB_CUSTO_SERVICO>();
            lstServicos = prodController.ObterCustoServico().Where(p => p.PRODUTO.Trim() == produto.CODIGO_PRODUTO_LINX.ToString() && p.MOSTRUARIO == produto.MOSTRUARIO).ToList();

            char mostruario = 'N';
            if (txtMostruario.Text.Trim().ToUpper() == "SIM")
                mostruario = 'S';

            labLavanderia.Text = "NÃO";
            labEstamparia.Text = "NÃO";
            labFaccao.Text = "NÃO";
            labAcabamento.Text = "NÃO";

            //obter rota do produto
            var rotaHB = prodController.ObterRotaHB(produto.CODIGO);

            if (rotaHB != null)
            {
                int i = 1;
                for (; i <= 4; i++)
                {
                    var f = lstServicos.Where(p => p.SERVICO == ((p.MOSTRUARIO == 'S' && i == 1) ? 6 : i)).FirstOrDefault();
                    if (f != null)
                        lstServicosFinal.Add(f);
                    else if ((rotaHB.FACCAO && (i == 1 || i == 6)) || (rotaHB.ESTAMPARIA && i == 2) || (rotaHB.LAVANDERIA && i == 3))
                        //else if (rotaHB.FACCAO && (i == 1 || i == 6))
                        lstServicosFinal.Add(new PROD_HB_CUSTO_SERVICO
                        {
                            CODIGO = 0,
                            MOSTRUARIO = mostruario,
                            CUSTO_PECA = ((mostruario == 'S' && i == 1) ? produto.PRECO_FACC_MOSTRUARIO : 0),
                            SERVICO = ((mostruario == 'S' && i == 1) ? 6 : i)
                        });
                }

                for (i = lstServicosFinal.Count; i < 6; i++)
                    lstServicosFinal.Add(new PROD_HB_CUSTO_SERVICO { CODIGO = 0, MOSTRUARIO = mostruario });

                if (rotaHB.LAVANDERIA)
                    labLavanderia.Text = "SIM";
                if (rotaHB.ESTAMPARIA)
                    labEstamparia.Text = "SIM";
                if (rotaHB.FACCAO)
                    labFaccao.Text = "SIM";
                if (rotaHB.ACABAMENTO)
                    labAcabamento.Text = "SIM";
            }

            if (produto != null)
            {
                lstFornecedorServico = CarregarFornecedores('S');
                CarregarServicosProducao();

                gvServico.DataSource = lstServicosFinal;
                gvServico.DataBind();
            }
        }
        protected void gvServico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_CUSTO_SERVICO _servico = e.Row.DataItem as PROD_HB_CUSTO_SERVICO;

                    colServico += 1;
                    if (_servico != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colServico.ToString();

                        DropDownList _ddlFornecedor = e.Row.FindControl("ddlFornecedor") as DropDownList;
                        if (_ddlFornecedor != null)
                        {
                            _ddlFornecedor.DataSource = lstFornecedorServico;
                            _ddlFornecedor.DataBind();

                            if (_servico.FORNECEDOR != null && _servico.FORNECEDOR.Trim() != "")
                                _ddlFornecedor.SelectedValue = _servico.FORNECEDOR;
                        }

                        DropDownList _ddlServico = e.Row.FindControl("ddlServico") as DropDownList;
                        if (_ddlServico != null)
                        {
                            if (_servico.MOSTRUARIO == 'N')
                                lstServicoProducao = lstServicoProducao.Where(p => p.CODIGO != 6).ToList();
                            else
                                lstServicoProducao = lstServicoProducao.Where(p => p.CODIGO != 1).ToList(); ;

                            _ddlServico.DataSource = lstServicoProducao;
                            _ddlServico.DataBind();

                            if (_servico.SERVICO > 0)
                                _ddlServico.SelectedValue = _servico.SERVICO.ToString();
                        }
                        //CUSTO
                        TextBox _txtValorServico = e.Row.FindControl("txtValorServico") as TextBox;
                        if (_txtValorServico != null)
                            if (_servico.CUSTO > 0)
                            {
                                _txtValorServico.Text = _servico.CUSTO.ToString("########0.00");
                                totalServicoCusto += _servico.CUSTO;
                            }

                        //PRECO
                        TextBox _txtValorPreco = e.Row.FindControl("txtValorPreco") as TextBox;
                        if (_txtValorPreco != null)
                            if (_servico.CUSTO_PECA > 0)
                            {
                                _txtValorPreco.Text = Convert.ToDecimal(_servico.CUSTO_PECA).ToString("########0.00");
                                totalServicoPreco += Convert.ToDecimal(_servico.CUSTO_PECA);
                            }

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _servico.CODIGO.ToString();
                    }
                }
            }
        }
        protected void gvServico_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvServico.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[3].Text = totalServicoCusto.ToString("###,###,###,##0.00");
                footer.Cells[4].Text = totalServicoPreco.ToString("###,###,###,##0.00");
            }
        }
        protected void txtValorServico_TextChanged(object sender, EventArgs e)
        {
            TextBox txtValorServico = (TextBox)sender;
            try
            {
                if (txtValorServico != null)
                {
                    if (txtValorServico.Text.Trim() != "" && Convert.ToDecimal(txtValorServico.Text) >= 0)
                    {
                        GridViewRow row = (GridViewRow)txtValorServico.NamingContainer;
                        if (row != null)
                        {
                            //Realizar a somatoria do grid de servicos
                            SomarTotalServico();
                        }
                        else
                        {
                            //MSG - LINHA NAO ENCONTRADA, ENTRE EM CONTATO COM TI
                        }
                    }
                    else
                    {
                        //MSG - CONSUMO INVALIDO, CONSUMO DEVE SER MAIOR QUE ZERO
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    if (b.CommandArgument.Trim().ToString() != "")
                    {
                        prodController.ExcluirCustoServico(Convert.ToInt32(b.CommandArgument));

                        //Recarregar serviço
                        PROD_HB produto = ObterProduto();
                        CarregarServicos(produto);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion



        #region "GERAR CUSTO COLECAO 27 >="
        private decimal CalcularICMSCredito(decimal tipoCusto)
        {
            decimal imcsCredito = 0;
            decimal valorTotal = 0;
            decimal valorICMS = 0;
            Literal litTotalCusto = null;
            Literal litTotalPreco = null;
            Literal litTotal = null;
            Literal litICMS = null;

            //Calcular IMCS Credito de Tecido
            decimal imcsCreditoTecido = 0;
            foreach (GridViewRow row in gvTecido.Rows)
            {
                if (row != null)
                {
                    litTotalCusto = row.FindControl("litTotalCusto") as Literal;
                    litTotalPreco = row.FindControl("litTotalPreco") as Literal;

                    litICMS = row.FindControl("litICMS") as Literal;
                    if (tipoCusto == 'S')
                    {
                        if (litTotalCusto != null)
                            valorTotal = (litTotalCusto.Text.Trim() == "") ? 0 : Convert.ToDecimal(litTotalCusto.Text.Trim());
                    }
                    else
                    {
                        if (litTotalPreco != null)
                            valorTotal = (litTotalPreco.Text.Trim() == "") ? 0 : Convert.ToDecimal(litTotalPreco.Text.Trim());
                    }

                    valorICMS = (litICMS.Text.Trim() == "") ? 0 : (Convert.ToDecimal(litICMS.Text.Trim()) / 100);
                    imcsCreditoTecido += (valorTotal * valorICMS);
                }
            }
            foreach (GridViewRow row in gvTecidoNovo.Rows)
            {
                if (row != null)
                {
                    litTotalCusto = row.FindControl("litTotalCusto") as Literal;
                    litTotalPreco = row.FindControl("litTotalPreco") as Literal;

                    litICMS = row.FindControl("litICMS") as Literal;
                    if (tipoCusto == 'S')
                    {
                        if (litTotalCusto != null)
                            valorTotal = (litTotalCusto.Text.Trim() == "") ? 0 : Convert.ToDecimal(litTotalCusto.Text.Trim());
                    }
                    else
                    {
                        if (litTotalPreco != null)
                            valorTotal = (litTotalPreco.Text.Trim() == "") ? 0 : Convert.ToDecimal(litTotalPreco.Text.Trim());
                    }
                    valorICMS = (litICMS.Text.Trim() == "") ? 0 : (Convert.ToDecimal(litICMS.Text.Trim()) / 100);
                    imcsCreditoTecido += (valorTotal * valorICMS);
                }
            }

            valorTotal = 0;
            valorICMS = 0;
            //Calcular ICMS Creidto de Aviamento
            decimal imcsCreditoAviamento = 0;
            foreach (GridViewRow row in gvAviamento.Rows)
            {
                if (row != null)
                {
                    litTotal = row.FindControl("litTotal") as Literal;
                    litICMS = row.FindControl("litICMS") as Literal;
                    if (litTotal != null && litICMS != null)
                    {
                        valorTotal = (litTotal.Text.Trim() == "") ? 0 : Convert.ToDecimal(litTotal.Text.Trim());
                        valorICMS = (litICMS.Text.Trim() == "") ? 0 : (Convert.ToDecimal(litICMS.Text.Trim()) / 100);
                        imcsCreditoAviamento += (valorTotal * valorICMS);
                    }
                }
            }
            foreach (GridViewRow row in gvAviamentoNovo.Rows)
            {
                if (row != null)
                {
                    litTotal = row.FindControl("litTotal") as Literal;
                    litICMS = row.FindControl("litICMS") as Literal;
                    if (litTotal != null && litICMS != null)
                    {
                        valorTotal = (litTotal.Text.Trim() == "") ? 0 : Convert.ToDecimal(litTotal.Text.Trim());
                        valorICMS = (litICMS.Text.Trim() == "") ? 0 : (Convert.ToDecimal(litICMS.Text.Trim()) / 100);
                        imcsCreditoAviamento += (valorTotal * valorICMS);
                    }
                }
            }

            imcsCredito = imcsCreditoTecido + imcsCreditoAviamento;
            return imcsCredito;
        }
        private bool GerarCustoSimulacao()
        {
            char mostruario = 'N';
            if (txtMostruario.Text.Trim().ToUpper() == "SIM")
                mostruario = 'S';

            //Declaração Custo Total por ORIGEM
            decimal vTecidoCusto = 0;
            decimal vTecidoPreco = 0;
            decimal vAviamento = 0;
            decimal vServicoCusto = 0;
            decimal vServicoPreco = 0;
            decimal vEtiqueta = 0;
            decimal vTAG = 0;

            var grupoProduto = txtGrupo.Text.Trim().ToUpper();

            //Inicializando variaveis
            foreach (GridViewRow rowT in gvTecido.Rows)
            {
                vTecidoCusto += (((Literal)rowT.FindControl("litTotalCusto")).Text == "") ? 0 : Convert.ToDecimal(((Literal)rowT.FindControl("litTotalCusto")).Text);
                vTecidoPreco += (((Literal)rowT.FindControl("litTotalPreco")).Text == "") ? 0 : Convert.ToDecimal(((Literal)rowT.FindControl("litTotalPreco")).Text);
            }
            foreach (GridViewRow rowT in gvTecidoNovo.Rows)
            {
                vTecidoCusto += (((Literal)rowT.FindControl("litTotalCusto")).Text == "") ? 0 : Convert.ToDecimal(((Literal)rowT.FindControl("litTotalCusto")).Text);
                vTecidoPreco += (((Literal)rowT.FindControl("litTotalPreco")).Text == "") ? 0 : Convert.ToDecimal(((Literal)rowT.FindControl("litTotalPreco")).Text);
            }

            foreach (GridViewRow rowA in gvAviamento.Rows)
                vAviamento += (((Literal)rowA.FindControl("litTotal")).Text == "") ? 0 : Convert.ToDecimal(((Literal)rowA.FindControl("litTotal")).Text);
            foreach (GridViewRow rowA in gvAviamentoNovo.Rows)
                vAviamento += (((Literal)rowA.FindControl("litTotal")).Text == "") ? 0 : Convert.ToDecimal(((Literal)rowA.FindControl("litTotal")).Text);

            vServicoCusto = (gvServico.FooterRow == null) ? 0 : Convert.ToDecimal(gvServico.FooterRow.Cells[3].Text);
            vServicoPreco = (gvServico.FooterRow == null) ? 0 : Convert.ToDecimal(gvServico.FooterRow.Cells[4].Text);
            vEtiqueta = (txtValorEtiqueta.Text == "") ? 0 : Convert.ToDecimal(txtValorEtiqueta.Text);
            vTAG = (txtTAG.Text == "") ? 0 : Convert.ToDecimal(txtTAG.Text);

            var varColecao = prodController.ObterCustoVariavelPorColecao(ddlColecoes.SelectedValue.Trim());
            if (varColecao == null)
            {
                labCusto.Text = "As váriáveis da coleção não foram definidas na tabela PRODUTO_CUSTO_COLECAO_VAR.";
                return false;
            }

            var vServicoFaccao = 0M;
            // se for basic e ter estamparia, remover o valor da estamparia do servico
            var custoFaccao = prodController.ObterCustoServico(txtProduto.Text.Trim(), mostruario).Where(p => p.SERVICO == 1).FirstOrDefault();
            if (custoFaccao == null)
            {
                labCusto.Text = "Informe o custo da Facção...";
                return false;
            }

            vServicoFaccao = Convert.ToDecimal(custoFaccao.CUSTO_PECA);

            var vOperacional = 0M;
            var vOperacionalTot = 0M;
            var custoUnidade = 0M;
            var custoUnidadeMargem = 0M;
            var vMargem = 0M;
            var vImposto = 0M;


            var operacionalPorc = 0M;
            operacionalPorc = Convert.ToDecimal(txtOperacionalPorc.Text.Replace("%", "").Replace(".", ","));

            if (operacionalPorc > 0)
            {
                vOperacional = vServicoFaccao + (vServicoFaccao * operacionalPorc / 100.00M);
                vOperacionalTot = vServicoPreco + (vServicoPreco * operacionalPorc / 100.00M);
            }


            custoUnidade = vTecidoCusto + vAviamento + vServicoFaccao + vOperacional;
            var custoUnidadeTot = vTecidoCusto + vAviamento + vServicoPreco + vOperacionalTot;

            vMargem = (custoUnidade / varColecao.MARGEM_VAL) - custoUnidade;
            var vMargemTot = (custoUnidadeTot / varColecao.MARGEM_VAL) - custoUnidadeTot;

            custoUnidadeMargem = custoUnidadeTot + vMargemTot;
            vImposto = (custoUnidadeMargem / varColecao.IMPOSTO_VAL) - custoUnidadeMargem;

            if (mostruario == 'S')
            {
                InserirCustoSimulacao('S', 'S', vTecidoCusto, vAviamento, vServicoCusto, vOperacional, vEtiqueta, vTAG, vMargem, vImposto);
                InserirCustoSimulacao('S', 'N', vTecidoCusto, vAviamento, vServicoPreco, vOperacional, vEtiqueta, vTAG, vMargem, vImposto);
            }
            else
            {
                InserirCustoSimulacao('N', 'N', vTecidoCusto, vAviamento, vServicoPreco, vOperacional, vEtiqueta, vTAG, vMargem, vImposto);
            }

            return true;
        }
        private bool InserirCustoSimulacao(char mostruario, char tipoCusto, decimal vTecido, decimal vAviamento, decimal vServico, decimal vOperacional, decimal vEtiqueta, decimal vTAG, decimal vMargem, decimal vImposto)
        {

            List<PRODUTO_CUSTO_SIMULACAO> lstProdutoCusto = new List<PRODUTO_CUSTO_SIMULACAO>();
            //Primeiro realizar a simulação de todos os MARKUPS informado: (2, 2.5, 3, 3.5, 4)
            decimal mkup = Convert.ToDecimal(2.0);
            int codigo = 0;
            PRODUTO_CUSTO_SIMULACAO novoCusto = null;

            var custoTX = vTecido + vAviamento + vServico + vOperacional + vEtiqueta + vTAG + vMargem + vImposto;
            for (int i = 0; i < 5; i++)
            {
                novoCusto = new PRODUTO_CUSTO_SIMULACAO();
                try
                {
                    codigo = prodController.ObterCustoSimulacao(txtProduto.Text.Trim(), mostruario, mkup, tipoCusto).CODIGO;
                }
                catch (Exception) { codigo = 0; }

                novoCusto = pc.CalcularCustoProduto27(custoTX, mkup, 0);
                //novoCusto = pc.CalcularLucro(novoCusto, valoresCalculo);
                novoCusto.CODIGO = codigo;
                lstProdutoCusto.Add(novoCusto);

                mkup += Convert.ToDecimal(0.5);
            }

            char finalizado = 'N';
            if (chkFinalizado.Checked)
                finalizado = 'S';

            //Se calculado com com sucesso, insere na tabela
            foreach (PRODUTO_CUSTO_SIMULACAO pc in lstProdutoCusto)
            {
                pc.PRODUTO = txtProduto.Text.Trim();
                pc.MOSTRUARIO = mostruario;
                pc.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                pc.DATA_INCLUSAO = DateTime.Now;
                pc.CUSTO_MOSTRUARIO = tipoCusto;
                pc.SIMULACAO = 'S';
                pc.FINALIZADO = finalizado;

                if (pc.CODIGO <= 0)
                    prodController.InserirCustoSimulacao(pc);
                else
                    prodController.AtualizarCustoSimulacao(pc);
            }

            var produto = ObterProduto();
            if (produto != null)
            {
                InserirCustoOrigem(produto.CODIGO_PRODUTO_LINX.ToString(), produto.COLECAO.Trim(), 1, vTecido, mostruario, tipoCusto); //Tecido
                InserirCustoOrigem(produto.CODIGO_PRODUTO_LINX.ToString(), produto.COLECAO.Trim(), 2, vAviamento, mostruario, tipoCusto); //Aviamento
                InserirCustoOrigem(produto.CODIGO_PRODUTO_LINX.ToString(), produto.COLECAO.Trim(), 3, vServico, mostruario, tipoCusto); //Serviço
                InserirCustoOrigem(produto.CODIGO_PRODUTO_LINX.ToString(), produto.COLECAO.Trim(), 4, vOperacional, mostruario, tipoCusto); //Operacional
                InserirCustoOrigem(produto.CODIGO_PRODUTO_LINX.ToString(), produto.COLECAO.Trim(), 5, vImposto, mostruario, tipoCusto); //Imposto
                InserirCustoOrigem(produto.CODIGO_PRODUTO_LINX.ToString(), produto.COLECAO.Trim(), 6, vEtiqueta, mostruario, tipoCusto); //Etiqueta
                InserirCustoOrigem(produto.CODIGO_PRODUTO_LINX.ToString(), produto.COLECAO.Trim(), 14, vTAG, mostruario, tipoCusto); //TAG
                InserirCustoOrigem(produto.CODIGO_PRODUTO_LINX.ToString(), produto.COLECAO.Trim(), 15, vMargem, mostruario, tipoCusto); //Margem
            }

            return true;
        }
        private bool InserirCustoOrigem(string codigoProduto, string colecao, int codOrigem, decimal custoTotal, char mostruario, char tipoCusto)
        {
            int codigo = 0;

            try
            {
                codigo = prodController.ObterCustoOrigem(codigoProduto, colecao, codOrigem, mostruario, tipoCusto).CODIGO;
            }
            catch (Exception)
            {
                codigo = 0;
            }

            prodController.ExcluirCustoOrigem(codigo);

            PRODUTO_CUSTO_ORIGEM custoOrigem = new PRODUTO_CUSTO_ORIGEM();
            custoOrigem.CODIGO = codigo;
            custoOrigem.PRODUTO = codigoProduto;
            custoOrigem.COLECAO = colecao;
            custoOrigem.COD_CUSTO_ORIGEM = codOrigem;
            custoOrigem.CUSTO_TOTAL = Math.Round(custoTotal, 2);
            custoOrigem.MOSTRUARIO = mostruario;
            custoOrigem.CUSTO_MOSTRUARIO = tipoCusto;

            prodController.InserirCustoOrigem(custoOrigem);

            return true;
        }
        #endregion

        protected void btGerarCusto_Click(object sender, EventArgs e)
        {
            try
            {
                labCusto.Text = "";
                if (!ValidarServicos())
                {
                    labCusto.Text = "Selecione as informações para o Custo do Serviço corretamente.";
                    return;
                }

                int colecao = 0;
                if (!int.TryParse(ddlColecoes.SelectedValue, out colecao))
                {
                    labCusto.Text = "Erro ao obter Coleção. Entre em contato com TI.";
                    return;
                }

                //Gravar consumo do aviamento
                //GravarConsumoAviamento();
                //Gravar custo do servico
                GravarCustoServico();
                //Realizar simulação de Preço e grava na tabela de Simulação

                //if (colecao > 24)
                //    GerarCustoSimulacao25();
                //else
                GerarCustoSimulacao();

                labProduto.Text = txtProduto.Text.Trim();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('prod_menu.aspx', '_self'); }, 'Custo/Preço': function () { window.open('prod_cad_produto_custo_novo.aspx', '_self'); }, 'Aprovação Preço': function () { window.open('prod_cad_produto_custo_aprov_novo.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {
                labCusto.Text = ex.Message;
            }
        }

        #region "VALIDAÇÃO"
        private bool ValidarCodigoLinx()
        {
            PRODUTO _produto = null;
            string _codigoProduto = "";

            //CODIGO LINX EXISTE?
            _produto = (new BaseController().BuscaProduto(txtProdutoLinxFiltro.Text.Trim()));
            if (_produto == null)
            {
                var _produtoNome = (new BaseController().BuscaProdutosDescricao(txtNomeFiltro.Text.Trim().ToUpper()));
                if (_produtoNome == null || _produtoNome.Count() <= 0)
                    return false;

                _produto = _produtoNome[0];
            }

            ////CODIGO LINX CATEGORIA = "01" ?
            //if (_produto.COD_CATEGORIA != "01")
            //    return false;

            //CODIGO LINX INICIA COM NUMERO PAR? //PRODUTO NACIONAL
            _codigoProduto = _produto.PRODUTO1.Substring(0, 1);
            if (_codigoProduto != "" && Convert.ToInt32(_codigoProduto) > 0)
            {
                if ((Convert.ToInt32(_codigoProduto) % 2) != 0)
                    return false;
            }
            else
                return false;

            return true;
        }
        private bool ValidarServicos()
        {
            bool retorno = true;

            foreach (GridViewRow row in gvServico.Rows)
            {
                if (row != null)
                {
                    DropDownList ddlFornecedor = row.FindControl("ddlFornecedor") as DropDownList;
                    DropDownList ddlServico = row.FindControl("ddlServico") as DropDownList;
                    TextBox txtValorServico = row.FindControl("txtValorServico") as TextBox;
                    TextBox txtValorPreco = row.FindControl("txtValorPreco") as TextBox;

                    if (ddlFornecedor.SelectedValue.Trim() != "Selecione" || ddlServico.SelectedValue != "0" || txtValorServico.Text.Trim() != "0,00" || txtValorPreco.Text.Trim() != "0,00")
                    {
                        if (ddlFornecedor.SelectedValue.Trim() != "Selecione")
                        {
                            if (ddlServico.SelectedValue != "0")
                            {
                                if ((txtValorServico.Text.Trim() != "" && txtValorServico.Text.Trim() != "0,00") || (txtValorPreco.Text.Trim() != "" && txtValorPreco.Text.Trim() != "0,00"))
                                    retorno = true;
                                else
                                    return false;
                            }
                            else
                                return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return retorno;
        }
        private bool ValidarProdutoPrecoAprovado(PROD_HB produto)
        {
            string codigoProduto = "";
            codigoProduto = produto.CODIGO_PRODUTO_LINX.Trim();

            if (codigoProduto != "")
            {
                try
                {
                    if (prodController.ObterCustoSimulacao(codigoProduto, Convert.ToChar(produto.MOSTRUARIO), 'N', 'N', 'S').Count() > 0)
                        return false;
                }
                catch (Exception)
                {
                }
            }

            return true;
        }
        private bool ValidarProdutoCortado(PROD_HB produto)
        {
            if (produto != null)
            {
                //Validar  se pai esta cortado
                if (produto.STATUS != 'B')
                    return false;

                List<PROD_HB> detalhe = new List<PROD_HB>();
                detalhe = prodController.ObterDetalhesHB(produto.CODIGO);
                foreach (PROD_HB det in detalhe)
                {
                    //Validar se filhos estao cortados
                    if (det != null && det.STATUS != 'B')
                        return false;
                }
                return true;
            }
            return false;
        }
        private bool ValidarAviamentoComprado()
        {
            int totalAviamento = 0;
            int totalAviamentoComprado = 0;

            var prod_hb = prodController.ObterHB().Where(p => p.CODIGO_PRODUTO_LINX.ToString() == txtProduto.Text.Trim()).Take(1).SingleOrDefault();
            if (prod_hb != null)
            {
                var prod_hb_prod_aviamento = prodController.ObterAviamentoHB(prod_hb.CODIGO);
                if (prod_hb_prod_aviamento != null && prod_hb_prod_aviamento.Count() > 0)
                {
                    var prod_hb_comprado = prodController.ObterAviamentoComprado();

                    foreach (PROD_HB_PROD_AVIAMENTO aviamento in prod_hb_prod_aviamento)
                    {
                        totalAviamento += 1;
                        var aviamentoComprado = prod_hb_comprado.Where(p => p.PROD_HB_PROD_AVIAMENTO == aviamento.CODIGO);
                        if (aviamentoComprado != null && aviamentoComprado.Count() > 0)
                            totalAviamentoComprado += 1;
                    }
                }
            }

            if (totalAviamento == totalAviamentoComprado)
                return true;

            return false;
        }
        #endregion


        private void CalcularCustosVariavelCol(string colecao)
        {
            var varColecao = prodController.ObterCustoVariavelPorColecao(colecao);

            if (varColecao == null)
            {
                labErro.Text = "Variáveis de custo não foram definidas na tabela PRODUTO_CUSTO_COLECAO_VAR.";
                return;
            }

            txtValorEtiqueta.Text = varColecao.ETIQUETA_VAL.ToString();
            txtTAG.Text = varColecao.TAG_VAL.ToString();
            txtOperacionalPorc.Text = varColecao.OPERACIONAL_PORC.ToString() + "%";

            var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            if (codigoUsuario == 18 || codigoUsuario == 1144)
                txtOperacionalPorc.Enabled = true;

        }


    }
}
