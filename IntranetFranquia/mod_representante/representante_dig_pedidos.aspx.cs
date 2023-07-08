using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Relatorios
{
    public partial class representante_dig_pedidos : System.Web.UI.Page
    {
        protected string ActiveTab { get; private set; }

        BaseController basec = new BaseController();
        RepresentanteController repr = new RepresentanteController();
        List<ItensVenda> itensVenda = new List<ItensVenda>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.ActiveTab = "0";

                if (Request.QueryString["s"] == null || Request.QueryString["s"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;
                ibtEditar.Visible = false;
                ibtExcluir.Visible = false;
                ibtLimpar.Visible = false;
                //btSalvarMaterial.Visible = false;
                //btCancelarMaterial.Visible = false;
                //ibtAprovarFichaTecnica.Visible = false;

                List<FormasPagamento> formas = new List<FormasPagamento>();
                formas.Add(new FormasPagamento("04", "ATACADO 30/60/90"));
                formas.Add(new FormasPagamento("12", "ATACADO 30/60/90/120"));
                formas.Add(new FormasPagamento("15", "30/60/90/120/150"));
                formas.Add(new FormasPagamento("27", "30/60/90/120/150/165 DD"));

                ViewState["FormasPagto"] = formas;

                Session["ItensVenda"] = itensVenda;
            }
            else
            {
                this.ActiveTab = hidTabSelected.Value;
                if (string.IsNullOrWhiteSpace(this.ActiveTab))
                {
                    this.ActiveTab = "0";
                }
            }
        }

        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            List<CADASTRO_CLI_FOR> clientes = basec.ObterCadastroClientes(txtNomeCliente.Text, txtCodigoCliente.Text, txtCnpjCliente.Text.Replace(".", "").Replace("-", "").Replace("/", ""));
            cmbClientes.DataSource = clientes;
            cmbClientes.DataBind();

            if (clientes.Count > 0)
            {
                trClientes.Visible = true;
            }
            else
            {
                trClientes.Visible = false;
                labErro.Text = "Cliente não encontrado";
            }
        }

        protected void cmbClientes_DataBinding(object sender, EventArgs e)
        {
            cmbClientes.Items.Add(new ListItem("Selecione", "0"));
            cmbClientes.SelectedValue = "0";
        }

        protected void cmbClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbClientes.SelectedValue != "0")
            {
                CADASTRO_CLI_FOR cliente = basec.ObterCadastroCLIFORCodigo(cmbClientes.SelectedValue);
                CarregaDadosIniciais(cliente);
                TravaTela();
                hdfCondPgto.Value = "04";

                trClientes.Visible = false;
                ibtPesquisar.Visible = false;
                ibtSalvar.Visible = true;
                ibtCancelar.Visible = false;
                ibtEditar.Visible = false;
                ibtLimpar.Visible = true;

                txtProdutoFiltro.Enabled = true;
            }
            else
            {
                LimparTela();
            }
        }

        private void CarregaDadosIniciais(CADASTRO_CLI_FOR cliente)
        {
            txtNomeCliente.Text = cliente.NOME_CLIFOR.Trim();
            txtCodigoCliente.Text = cliente.CLIFOR.Trim();
            txtCnpjCliente.Text = Convert.ToUInt64(cliente.CGC_CPF.Trim()).ToString(@"00\.000\.000\/0000\-00");
            txtInscricaoEstadual.Text = cliente.RG_IE.Trim();
            txtEndereco.Text = cliente.ENDERECO.Trim();
            txtCidade.Text = cliente.CIDADE.Trim();
            txtEstado.Text = cliente.UF.Trim();
            txtTelefone1.Text = "(" + cliente.DDD1.Trim() + ") " + cliente.TELEFONE1.Trim();
            txtTelefone2.Text = "(" + cliente.DDD2.Trim() + ") " + cliente.TELEFONE2.Trim();
            txtRepresentante.Text = repr.ObterRepresentante(cliente.NOME_CLIFOR).REPRESENTANTE.Trim();
            txtEmissao.Text = DateTime.Now.ToString("dd/MM/yyyy");
            string colecaoAtual = basec.BuscarParametro("COLECAO_PADRAO").VALOR_ATUAL.Trim();
            txtColecao.Text = basec.BuscaColecaoAtual(colecaoAtual).DESC_COLECAO;
            txtTabPreco.Text = "TA - TABELA ATACADO";
            txtCondPgto.Text = "ATACADO 30/60/90";
            txtTransportadora.Text = repr.ObterClienteAtacado(cliente.CLIFOR).TRANSPORTADORA.Trim();
            txtEmail.Text = cliente.EMAIL.Trim();
        }

        private void TravaTela()
        {
            txtNomeCliente.ReadOnly = true;
            txtCodigoCliente.ReadOnly = true;
            txtCnpjCliente.ReadOnly = true;
        }

        private void LimparTela()
        {
            txtNomeCliente.Text = string.Empty;
            txtCodigoCliente.Text = string.Empty;
            txtCnpjCliente.Text = string.Empty;
            txtNomeCliente.ReadOnly = false;
            txtCodigoCliente.ReadOnly = false;
            txtCnpjCliente.ReadOnly = false;
            txtInscricaoEstadual.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtCidade.Text = string.Empty;
            txtEstado.Text = string.Empty;
            txtTelefone1.Text = string.Empty;
            txtTelefone2.Text = string.Empty;
            txtRepresentante.Text = string.Empty;
            txtEmissao.Text = string.Empty;
            txtColecao.Text = string.Empty;
            txtTabPreco.Text = string.Empty;
            txtCondPgto.Text = string.Empty;
            txtTransportadora.Text = string.Empty;
            txtEmail.Text = string.Empty;

            txtProdutoFiltro.Text = string.Empty;
            lblGrupo.Text = string.Empty;
            lblSubgrupo.Text = string.Empty;
            lblTipo.Text = string.Empty;
            lblGriffe.Text = string.Empty;
            lblLinha.Text = string.Empty;
            lblColecao.Text = string.Empty;
            lblModelagem.Text = string.Empty;
            lblCartela.Text = string.Empty;
            lblPrecoVenda.Text = string.Empty;
            lblGrade.Text = string.Empty;

            txtProdutoFiltro.Enabled = false;

            itensVenda = new List<ItensVenda>();
            Session["ItensVenda"] = itensVenda;
            grdItens.DataSource = itensVenda;
            rptProdutos.DataSource = null;

            hidTabSelected.Value = "0";
        }

        protected void ibtSalvar_Click(object sender, ImageClickEventArgs e)
        {
            itensVenda = (List<ItensVenda>)Session["ItensVenda"];

            USUARIO usuario = (USUARIO)Session["USUARIO"];

            if (itensVenda.Count > 0)
            {
                try
                {
                    BaseController bas = new BaseController();
                    VENDAS_LOTE venda = new VENDAS_LOTE();
                    SP_OBTER_CLIENTE_ATACADOResult clienteAtacado = repr.ObterClienteAtacado(cmbClientes.SelectedItem.Value);

                    venda.PEDIDO_EXTERNO = bas.ObterSequencialTabelaLinx("VENDAS_LOTE.PEDIDO_EXTERNO");
                    venda.TIPO_DO_DIGITADOR = null;
                    venda.MOEDA = "R$";
                    venda.CODIGO_TAB_PRECO = "TA";
                    venda.TIPO = "MULTIMARCAS";
                    venda.CONDICAO_PGTO = hidCondPgto.Value;
                    venda.COLECAO = basec.BuscarParametro("COLECAO_PADRAO").VALOR_ATUAL.Trim();
                    venda.CLIENTE_ATACADO = clienteAtacado.CLIENTE_ATACADO; //cmbClientes.SelectedItem.Text;
                    venda.REPRESENTANTE = repr.ObterNomeRepresentante(usuario.REPRESENTANTE_CLIFOR);
                    venda.PEDIDO = null;
                    venda.EMISSAO = DateTime.Now;
                    venda.DATA_ENVIO = null;
                    venda.DATA_RECEBIMENTO = null;
                    venda.PEDIDO_CLIENTE = "";
                    venda.DESCONTO = 0;
                    venda.ENCARGO = 0;
                    venda.TOT_QTDE_ORIGINAL = itensVenda.Sum(x => x.QTDE_ORIGINAL);
                    venda.TOT_VALOR_ORIGINAL = itensVenda.Sum(x => x.VALOR_ORIGINAL);
                    venda.VALOR_IPI = 0;
                    venda.CTRL_MULT_ENTREGAS = true;
                    venda.ENTREGA_CIF = 0;
                    venda.ENTREGA_ACEITAVEL = 0;
                    venda.DESCONTO_SOBRE_1 = 0;
                    venda.DESCONTO_SOBRE_2 = 0;
                    venda.DESCONTO_SOBRE_3 = 0;
                    venda.DESCONTO_SOBRE_4 = 0;
                    venda.FILIAL = "ATACADO HANDBOOK";
                    venda.TRANSPORTADORA = clienteAtacado.TRANSPORTADORA;
                    venda.GERENTE = "ATACADO";
                    venda.APROVACAO = 'I';
                    venda.APROVADO_POR = "INTERNET";
                    venda.CONFERIDO = false;
                    venda.CONFERIDO_POR = "";
                    venda.COMISSAO = repr.ObterRepresentante(venda.REPRESENTANTE).COMISSAO ?? 0;
                    venda.COMISSAO_GERENTE = 0;
                    venda.PORCENTAGEM_ACERTO = 0;
                    venda.PRIORIDADE = 0;
                    venda.ACEITA_JUNTAR_PED = false;
                    venda.STATUS = null;
                    venda.DATA_PARA_TRANSFERENCIA = DateTime.Now;
                    venda.OBS = null;
                    venda.TIPO_FRETE = "";
                    venda.TABELA_FILHA = "VENDAS_LOTE_PROD";
                    venda.MULTI_DESCONTO_ACUMULAR = 0;
                    venda.RECARGO = 0;
                    venda.OBS_TRANSPORTE = null;
                    venda.ACEITA_PECAS_COM_CORTE = 0;
                    venda.ACEITA_PECAS_PEQUENAS = 0;
                    venda.DATA_FATURAMENTO_RELATIVO = null;
                    venda.EXPEDICAO_COMPLETO_CARTELA = false;
                    venda.EXPEDICAO_COMPLETO_COORDENADO = false;
                    venda.EXPEDICAO_COMPLETO_COR = false;
                    venda.EXPEDICAO_COMPLETO_PACK = false;
                    venda.EXPEDICAO_COMPLETO_PEDIDO = false;
                    venda.EXPEDICAO_COMPLETO_TAMANHOS = false;
                    venda.EXPEDICAO_NAO_JUNTAR_PRODUTO_CAIXA = false;
                    venda.EXPEDICAO_PORCENTAGEM_MAIOR = 0;
                    venda.EXPEDICAO_PORCENTAGEM_MINIMA = 0;
                    venda.EXPEDICAO_PORCENTAGEM_TIPO = 0;
                    venda.FILIAL_DIGITACAO = "ATACADO HANDBOOK";
                    venda.FRETE_CORTESIA = false;
                    venda.INDICA_LOCAL_SEPARACAO = 0;
                    venda.NOME_CLIFOR_ENTREGA = clienteAtacado.CLIENTE_ATACADO;
                    venda.NUMERO_ENTREGA = null;
                    venda.PEDIDO_EXTERNO_ORIGEM = null;
                    venda.PERIODO_PCP = null;
                    venda.TIPO_CAIXA = "01";
                    venda.TIPO_RATEIO = 0;
                    venda.TRANSP_REDESPACHO = null;
                    venda.PORC_DESCONTO = 0;
                    venda.NATUREZA_SAIDA = "100.01";
                    venda.COMISSAO_VALOR_GERENTE = 0;
                    venda.COMISSAO_VALOR = venda.TOT_VALOR_ORIGINAL * (venda.COMISSAO / 100);
                    venda.PORC_DESCONTO_COND_PGTO = null;
                    venda.DESCONTO_COND_PGTO = null;
                    venda.PORC_ENCARGO = null;

                    repr.InserirVenda(venda);

                    List<VENDAS_LOTE_PROD> itens = new List<VENDAS_LOTE_PROD>();
                    foreach (ItensVenda i in itensVenda)
                    {
                        VENDAS_LOTE_PROD item = new VENDAS_LOTE_PROD();
                        item.PRODUTO = i.PRODUTO;
                        item.COR_PRODUTO = i.COR_PRODUTO;
                        item.ENTREGA = DateTime.Now;
                        item.PEDIDO_EXTERNO = venda.PEDIDO_EXTERNO;
                        item.SEQUENCIAL_DIGITACAO = 0;
                        item.PACKS = "";
                        item.NUMERO_CONJUNTO = 0;
                        item.NUMERO_ENTREGA = null;
                        item.LIMITE_ENTREGA = item.ENTREGA.AddDays(20);
                        item.STATUS_VENDA_ATUAL = null;
                        item.QTDE_ORIGINAL = i.QTDE_ORIGINAL;
                        item.VALOR_ORIGINAL = i.VALOR_ORIGINAL;
                        item.PRECO1 = i.PRECO1;
                        item.PRECO2 = 0;
                        item.PRECO3 = 0;
                        item.PRECO4 = 0;
                        item.IPI = 0;
                        item.VO1 = i.VO1;
                        item.VO2 = i.VO2;
                        item.VO3 = i.VO3;
                        item.VO4 = i.VO4;
                        item.VO5 = i.VO5;
                        item.VO6 = i.VO6;
                        item.VO7 = 0;
                        item.VO8 = 0;
                        item.VO9 = 0;
                        item.VO10 = 0;
                        item.VO11 = 0;
                        item.VO12 = 0;
                        item.VO13 = 0;
                        item.VO14 = 0;
                        item.VO15 = 0;
                        item.VO16 = 0;
                        item.VO17 = 0;
                        item.VO18 = 0;
                        item.VO19 = 0;
                        item.VO20 = 0;
                        item.VO21 = 0;
                        item.VO22 = 0;
                        item.VO23 = 0;
                        item.VO24 = 0;
                        item.VO25 = 0;
                        item.VO26 = 0;
                        item.VO27 = 0;
                        item.VO28 = 0;
                        item.VO29 = 0;
                        item.VO30 = 0;
                        item.VO31 = 0;
                        item.VO32 = 0;
                        item.VO33 = 0;
                        item.VO34 = 0;
                        item.VO35 = 0;
                        item.VO36 = 0;
                        item.VO37 = 0;
                        item.VO38 = 0;
                        item.VO39 = 0;
                        item.VO40 = 0;
                        item.VO41 = 0;
                        item.VO42 = 0;
                        item.VO43 = 0;
                        item.VO44 = 0;
                        item.VO45 = 0;
                        item.VO46 = 0;
                        item.VO47 = 0;
                        item.VO48 = 0;
                        item.DATA_PARA_TRANSFERENCIA = DateTime.Now;
                        item.CODIGO_LOCAL_ENTREGA = null;
                        item.DESC_VENDA_CLIENTE = null;
                        item.ITEM_PEDIDO = "0000";
                        item.NUMERO_CAIXAS = 1;
                        item.TIPO_CAIXA = "01";
                        item.COMISSAO_ITEM = i.COMISSAO_ITEM;
                        item.COMISSAO_ITEM_GERENTE = 0;
                        item.ID_MODIFICACAO = null;
                        item.COMISSAO_VALOR_GERENTE = 0;
                        item.COMISSAO_VALOR = 0;

                        repr.InserirVendaProduto(item);
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }

        protected void ibtCancelar_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ibtEditar_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ibtExcluir_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ibtLimpar_Click(object sender, ImageClickEventArgs e)
        {
            LimparTela();
            trClientes.Visible = false;
        }

        protected void txtProdutoFiltro_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt != null)
            {
                try
                {
                    labErro.Text = "";
                    SP_OBTER_PRODUTO_PRECO_INFOResult produtoPrecoInfo = repr.ObterProdutoPrecoInfo(txtProdutoFiltro.Text.Trim());

                    if (produtoPrecoInfo != null)
                    {
                        DESENV_PRODUTO prod = (new DesenvolvimentoController()).ObterProduto(produtoPrecoInfo.COLECAO.Trim(), txtProdutoFiltro.Text.Trim()).FirstOrDefault();

                        if (prod != null)
                        {
                            CarregarProdutoFoto(prod);
                        }

                        lblNomeProduto.Text = produtoPrecoInfo.DESC_PRODUTO.Trim();
                        lblGrupo.Text = produtoPrecoInfo.GRUPO_PRODUTO.Trim();
                        lblSubgrupo.Text = produtoPrecoInfo.SUBGRUPO_PRODUTO.Trim();
                        lblTipo.Text = produtoPrecoInfo.TIPO_PRODUTO.Trim();
                        lblGriffe.Text = produtoPrecoInfo.GRIFFE.Trim();
                        lblLinha.Text = produtoPrecoInfo.LINHA.Trim();
                        lblColecao.Text = produtoPrecoInfo.COLECAO.Trim();
                        lblModelagem.Text = produtoPrecoInfo.MODELAGEM.Trim();
                        lblCartela.Text = produtoPrecoInfo.CARTELA.Trim();
                        lblPrecoVenda.Text = produtoPrecoInfo.PRECO1.ToString().Replace(".", ",");
                        lblGrade.Text = produtoPrecoInfo.GRADE.Trim();

                        List<SP_OBTER_GRADE_ESTOQUE_CORResult> listaItens = repr.ObterGradeEstoqueCor(produtoPrecoInfo.PRODUTO.Trim());
                        rptProdutos.DataSource = listaItens;
                        rptProdutos.DataBind();

                        divTableAdd.Visible = true;
                    }
                    else
                    {
                        divTableAdd.Visible = false;
                        labErro.Text = "Produto não encontrado";
                    }

                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }

        private void CarregarProdutoFoto(DESENV_PRODUTO _produto)
        {
            if (_produto != null)
            {
                imgFotoPeca.ImageUrl = _produto.FOTO;
                imgFotoPeca2.ImageUrl = _produto.FOTO2;
            }
        }

        protected void rptProdutos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (((SP_OBTER_GRADE_ESTOQUE_CORResult)e.Item.DataItem).CO1 == 0)
                ((TextBox)e.Item.FindControl("txtAddXP")).ReadOnly = true;
            else
                ((TextBox)e.Item.FindControl("txtAddXP")).ReadOnly = false;

            if (((SP_OBTER_GRADE_ESTOQUE_CORResult)e.Item.DataItem).CO2 == 0)
                ((TextBox)e.Item.FindControl("txtAddPP")).ReadOnly = true;
            else
                ((TextBox)e.Item.FindControl("txtAddPP")).ReadOnly = false;

            if (((SP_OBTER_GRADE_ESTOQUE_CORResult)e.Item.DataItem).CO3 == 0)
                ((TextBox)e.Item.FindControl("txtAddPQ")).ReadOnly = true;
            else
                ((TextBox)e.Item.FindControl("txtAddPQ")).ReadOnly = false;

            if (((SP_OBTER_GRADE_ESTOQUE_CORResult)e.Item.DataItem).CO4 == 0)
                ((TextBox)e.Item.FindControl("txtAddMD")).ReadOnly = true;
            else
                ((TextBox)e.Item.FindControl("txtAddMD")).ReadOnly = false;

            if (((SP_OBTER_GRADE_ESTOQUE_CORResult)e.Item.DataItem).CO5 == 0)
                ((TextBox)e.Item.FindControl("txtAddGD")).ReadOnly = true;
            else
                ((TextBox)e.Item.FindControl("txtAddGD")).ReadOnly = false;

            if (((SP_OBTER_GRADE_ESTOQUE_CORResult)e.Item.DataItem).CO6 == 0)
                ((TextBox)e.Item.FindControl("txtAddGG")).ReadOnly = true;
            else
                ((TextBox)e.Item.FindControl("txtAddGG")).ReadOnly = false;
        }

        protected void btnAdicionarProduto_Click(object sender, EventArgs e)
        {
            itensVenda = (List<ItensVenda>)Session["ItensVenda"];

            foreach (RepeaterItem i in rptProdutos.Items)
            {
                Label lblId = i.FindControl("lblIdentificador") as Label;
                string produto; string cor; string[] identificador;

                identificador = lblId.Text.Split('|');
                produto = identificador[0].Trim();
                cor = identificador[1].Trim();

                ItensVenda item = new ItensVenda();
                item.PRODUTO = produto;
                item.PRODUTO_DESC = lblNomeProduto.Text.Trim();
                item.COR_PRODUTO = cor;
                item.COR_PRODUTO_DESC = (i.FindControl("lblAddCorDesc") as Label).Text.Trim();
                item.ENTREGA = DateTime.ParseExact((i.FindControl("lblEntrega") as Label).Text.Trim(), "dd/MM/yyyy hh:mmm:ss", null);
                item.LIMITE_ENTREGA = DateTime.ParseExact((i.FindControl("lblLimiteEntrega") as Label).Text.Trim(), "dd/MM/yyyy hh:mmm:ss", null);
                item.PRECO1 = Decimal.Parse((i.FindControl("lblAddPreco") as Label).Text.Trim());
                item.VO1 = Int32.Parse((i.FindControl("txtAddXP") as TextBox).Text.Trim());
                item.VO2 = Int32.Parse((i.FindControl("txtAddPP") as TextBox).Text.Trim());
                item.VO3 = Int32.Parse((i.FindControl("txtAddPQ") as TextBox).Text.Trim());
                item.VO4 = Int32.Parse((i.FindControl("txtAddMD") as TextBox).Text.Trim());
                item.VO5 = Int32.Parse((i.FindControl("txtAddGD") as TextBox).Text.Trim());
                item.VO6 = Int32.Parse((i.FindControl("txtAddGG") as TextBox).Text.Trim());
                item.QTDE_ORIGINAL = item.VO1 + item.VO2 + item.VO3 + item.VO4 + item.VO5 + item.VO6;
                item.VALOR_ORIGINAL = item.PRECO1 * item.QTDE_ORIGINAL;
                item.COMISSAO_ITEM = repr.ObterRepresentante(cmbClientes.SelectedItem.Text).COMISSAO ?? 0;

                if (item.QTDE_ORIGINAL > 0)
                    itensVenda.Add(item);
            }

            grdItens.DataSource = itensVenda;
            grdItens.DataBind();

            Session["ItensVenda"] = itensVenda;

            List<FormasPagamento> formas = (List<FormasPagamento>)ViewState["FormasPagto"];
            //formas.Add(new FormasPagamento("04", "ATACADO 30/60/90"));
            //formas.Add(new FormasPagamento("12", "ATACADO 30/60/90/120"));
            //formas.Add(new FormasPagamento("15", "30/60/90/120/150"));
            //formas.Add(new FormasPagamento("27", "30/60/90/120/150/165 DD"));

            decimal valorTotal = itensVenda.Sum(x => x.VALOR_ORIGINAL);

            FormasPagamento forma;
            if (valorTotal >= 30000)
            {
                forma = formas.Where(f => f.Codigo == "37").ToList().FirstOrDefault();
            }
            else if (valorTotal >= 20000)
            {
                forma = formas.Where(f => f.Codigo == "15").ToList().FirstOrDefault();
            }
            else if (valorTotal >= 12000)
            {
                forma = formas.Where(f => f.Codigo == "12").ToList().FirstOrDefault();
            }
            else
            {
                forma = formas.Where(f => f.Codigo == "04").ToList().FirstOrDefault();
            }

            hidCondPgto.Value = forma.Codigo;
            txtCondPgto.Text = forma.Nome;
        }

        protected void btEditarItem_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btExcluirMaterial_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void grdItens_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int i = 0;
            i++;
        }
    }

    [Serializable]
    public class FormasPagamento
    {
        public String Codigo { get; set; }
        public String Nome { get; set; }

        public FormasPagamento(string codigo, string nome)
        {
            Codigo = codigo;
            Nome = nome;
        }
    }

    [Serializable]
    public class ItensVenda
    {
        public string PRODUTO { get; set; }
        public string PRODUTO_DESC { get; set; }
        public string COR_PRODUTO { get; set; }
        public string COR_PRODUTO_DESC { get; set; }
        public DateTime ENTREGA { get; set; }
        public DateTime LIMITE_ENTREGA { get; set; }
        public int QTDE_ORIGINAL { get; set; }
        public decimal VALOR_ORIGINAL { get; set; }
        public decimal PRECO1 { get; set; }
        public int VO1 { get; set; }
        public int VO2 { get; set; }
        public int VO3 { get; set; }
        public int VO4 { get; set; }
        public int VO5 { get; set; }
        public int VO6 { get; set; }
        public decimal COMISSAO_ITEM { get; set; }
    }
}