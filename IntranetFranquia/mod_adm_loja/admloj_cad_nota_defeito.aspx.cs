using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class admloj_cad_nota_defeito : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCodigoProduto.Attributes.Add("onKeyPress", "doClick('" + btBuscar.ClientID + "',event)");
                txtDescricaoProduto.Attributes.Add("onKeyPress", "doClick('" + btSalvar.ClientID + "',event)");

                CarregarFiliais();
                CarregaDropDownListOrigemDefeito();
                CarregaDropDownListDefeito();


            }
        }

        #region "DADOS INICIAIS"

        private void CarregarFiliais()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                if (usuario != null)
                {

                    var f = baseController.BuscaFiliais(usuario);

                    f.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    ddlFilial.DataSource = f;
                    ddlFilial.DataBind();
                }
            }
        }
        private void CarregaDropDownListOrigemDefeito()
        {
            var odef = baseController.BuscaOrigensDefeito();

            odef.Insert(0, new NOTA_RETIRADA_ORIGEM_DEFEITO { CODIGO = 0, DESCRICAO_ORIGEM_DEFEITO = "Selecione" });
            ddlOrigemDefeito.DataSource = odef;
            ddlOrigemDefeito.DataBind();
        }
        private void CarregaDropDownListDefeito()
        {
            var def = baseController.BuscaDefeitos();

            def.Insert(0, new NOTA_RETIRADA_DEFEITO { CODIGO = 0, DESCRICAO_DEFEITO = "Selecione" });
            ddlDefeito.DataSource = def;
            ddlDefeito.DataBind();
        }
        private void CarregaDropDownListTamanho(string grade)
        {
            List<Tamanho> listaTamanho = MontaLista(grade);

            if (listaTamanho != null)
            {
                ddlTamanho.DataSource = listaTamanho;
                ddlTamanho.DataBind();

                if (listaTamanho.Count() == 2)
                    ddlTamanho.SelectedIndex = 1;
            }
        }
        public List<Tamanho> MontaLista(string texto)
        {
            List<Tamanho> lista = new List<Tamanho>();

            Tamanho item = new Tamanho();

            item.Codigo = 0;
            item.Descricao = "Selecione";

            lista.Add(item);

            int indice = 0;

            texto = texto.Trim();

            if (texto.Contains("-"))
            {
                string descricao = "";

                for (int i = 0; i < texto.Length; i++)
                {
                    if (texto.Substring(i, 1).Equals("-"))
                    {
                        item = new Tamanho();

                        indice++;

                        item.Codigo = indice;
                        item.Descricao = descricao;

                        lista.Add(item);

                        descricao = "";
                    }
                    else
                        descricao += texto.Substring(i, 1);
                }

                item = new Tamanho();

                indice++;

                item.Codigo = indice;
                item.Descricao = descricao;

                lista.Add(item);

                return lista;
            }

            if (texto.Substring(0, 1).Equals("1") ||
                texto.Substring(0, 1).Equals("2") ||
                texto.Substring(0, 1).Equals("3"))
            {
                if (texto.Length > 8)
                {
                    item = new Tamanho();

                    indice++;

                    item.Codigo = indice;
                    item.Descricao = "33/34";

                    lista.Add(item);

                    item = new Tamanho();

                    indice++;

                    item.Codigo = indice;
                    item.Descricao = "34/35";

                    lista.Add(item);

                    item = new Tamanho();

                    indice++;

                    item.Codigo = indice;
                    item.Descricao = "35/36";

                    lista.Add(item);

                    item = new Tamanho();

                    indice++;

                    item.Codigo = indice;
                    item.Descricao = "36/37";

                    lista.Add(item);

                    item = new Tamanho();

                    indice++;

                    item.Codigo = indice;
                    item.Descricao = "37/38";

                    lista.Add(item);

                    item = new Tamanho();

                    indice++;

                    item.Codigo = indice;
                    item.Descricao = "38/39";

                    lista.Add(item);

                    return lista;
                }
                else
                {
                    int tamanhoMenor = Convert.ToInt32(texto.Substring(0, 2));
                    int tamanhoMaior = Convert.ToInt32(texto.Substring(5, 2));

                    for (int i = 0; tamanhoMenor <= tamanhoMaior; i++)
                    {
                        item = new Tamanho();

                        indice++;

                        item.Codigo = indice;
                        item.Descricao = tamanhoMenor.ToString();

                        lista.Add(item);

                        tamanhoMenor++;
                    }

                    return lista;
                }
            }
            else
            {
                if (texto.Length == 5)
                {
                    item = new Tamanho();

                    indice++;

                    item.Codigo = indice;
                    item.Descricao = "UN";

                    lista.Add(item);

                    return lista;
                }
            }

            item = new Tamanho();

            indice++;

            item.Codigo = indice;
            item.Descricao = texto;

            lista.Add(item);

            return lista;
        }
        private void CarregaDropDownListProdutoCor(string codigoProduto)
        {
            var cores = baseController.BuscaProdutoCores(codigoProduto);

            cores.Insert(0, new PRODUTO_CORE { COR_PRODUTO = "0", DESC_COR_PRODUTO = "Selecione" });
            ddlProdutoCor.DataSource = cores;
            ddlProdutoCor.DataBind();

            if (cores.Count() == 2)
                ddlProdutoCor.SelectedIndex = 1;

        }

        private void LimpaFeedBack()
        {
            labErro.Text = string.Empty;
        }
        private void LimpaTela()
        {
            txtCodigoProduto.Text = string.Empty;
            txtDescricaoProduto.Text = string.Empty;

            ddlTamanho.Items.Clear();
            ddlProdutoCor.Items.Clear();
            ddlOrigemDefeito.Items.Clear();
            ddlDefeito.Items.Clear();

            CarregaDropDownListOrigemDefeito();
            CarregaDropDownListDefeito();

            btSalvar.Enabled = false;
            btBuscar.Enabled = true;

            HiddenFieldCodigoNotaRetiradaItem.Value = "0";
        }

        protected void btContinue_Click(object sender, EventArgs e)
        {
            if (ddlFilial.SelectedValue == "")
            {
                labErro.Text = "Selecione a Filial.";
                return;
            }

            LimpaTela();
            LimpaFeedBack();

            CarregaGridViewNotaRetiradaItem(Convert.ToInt32(ddlFilial.SelectedValue));
        }
        #endregion

        private void CarregaGridViewNotaRetiradaItem(int filial)
        {
            GridViewNotaRetiradaItem.DataSource = usuarioController.BuscaFilialNotaRetiradaItem(filial);
            GridViewNotaRetiradaItem.DataBind();
        }
        private void CarregaNotaRetiradaItem(int codigoNotaRetiradaItem)
        {
            NOTA_RETIRADA_ITEM notaRetiradaItem = usuarioController.BuscaNotaRetiradaItem(codigoNotaRetiradaItem);

            if (notaRetiradaItem != null)
            {
                btBuscar.Enabled = false;
                btSalvar.Enabled = true;

                txtCodigoProduto.Text = notaRetiradaItem.CODIGO_PRODUTO.ToString();
                txtCodigoProduto.Enabled = false;

                txtDescricaoProduto.Text = notaRetiradaItem.DESCRICAO_PRODUTO;

                CarregaDropDownListProdutoCor(notaRetiradaItem.CODIGO_PRODUTO.ToString());

                PRODUTO produto = baseController.BuscaProduto(notaRetiradaItem.CODIGO_PRODUTO.ToString());

                if (produto != null)
                    CarregaDropDownListTamanho(produto.GRADE);

                HiddenFieldCodigoNotaRetiradaItem.Value = notaRetiradaItem.CODIGO_NOTA_RETIRADA_ITEM.ToString();
            }
        }
        protected void GridViewNotaRetiradaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_RETIRADA_ITEM notaRetiradaItem = e.Row.DataItem as NOTA_RETIRADA_ITEM;

            if (notaRetiradaItem != null)
            {

                Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                if (btExcluir != null)
                    btExcluir.CommandArgument = notaRetiradaItem.CODIGO_NOTA_RETIRADA_ITEM.ToString();

                Literal literalOrigemDefeito = e.Row.FindControl("LiteralOrigemDefeito") as Literal;
                if (literalOrigemDefeito != null)
                {
                    var origemDefeito = baseController.BuscaOrigemDefeito(notaRetiradaItem.CODIGO_ORIGEM_DEFEITO);
                    if (origemDefeito != null)
                        literalOrigemDefeito.Text = origemDefeito.DESCRICAO_ORIGEM_DEFEITO;
                }

                Literal literalDefeito = e.Row.FindControl("LiteralDefeito") as Literal;
                if (literalDefeito != null)
                {
                    var defeito = baseController.BuscaDefeito(notaRetiradaItem.CODIGO_DEFEITO);
                    if (defeito != null)
                        literalDefeito.Text = defeito.DESCRICAO_DEFEITO;
                }

                Literal litDataLancamento = e.Row.FindControl("litDataLancamento") as Literal;
                if (litDataLancamento != null)
                    litDataLancamento.Text = (notaRetiradaItem.DATA_LANCAMENTO == null) ? "" : Convert.ToDateTime(notaRetiradaItem.DATA_LANCAMENTO).ToString("dd/MM/yyyy");
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {

            if (txtCodigoProduto.Text.Trim() == "")
            {
                labErro.Text = "Por favor, informe o Código do Produto.";
                return;
            }

            if (ddlProdutoCor.SelectedValue == "0")
            {
                labErro.Text = "Por favor, informe a Cor.";
                return;
            }

            if (ddlTamanho.SelectedValue == "0")
            {
                labErro.Text = "Por favor, informe o Tamanho.";
                return;
            }

            if (ddlOrigemDefeito.SelectedValue == "0")
            {
                labErro.Text = "Por favor, informe a Origem do Defeito.";
                return;
            }

            if (ddlDefeito.SelectedValue == "0")
            {
                labErro.Text = "Por favor, informe o Defeito.";
                return;
            }

            if (ddlFilial.SelectedValue == "")
            {
                labErro.Text = "Selecione a Filial.";
                return;
            }

            try
            {
                var produto = txtCodigoProduto.Text.Trim();
                var cor = ddlProdutoCor.SelectedValue.Trim();
                var tamanho = ddlTamanho.SelectedItem.Text.ToString();

                var produtoBarra = baseController.BuscaProdutoBarra(produto, cor, tamanho);
                var codigoFilial = ddlFilial.SelectedValue.Trim();

                if (produtoBarra != null)
                {
                    var notaRetiradaItem = new NOTA_RETIRADA_ITEM();

                    notaRetiradaItem.CODIGO_PRODUTO = Convert.ToInt32(produto);
                    notaRetiradaItem.COR_PRODUTO = ddlProdutoCor.SelectedItem.ToString();
                    notaRetiradaItem.COR = cor;
                    notaRetiradaItem.DESCRICAO_PRODUTO = txtDescricaoProduto.Text;
                    notaRetiradaItem.TAMANHO = ddlTamanho.SelectedItem.ToString();
                    notaRetiradaItem.CODIGO_ORIGEM_DEFEITO = ddlOrigemDefeito.SelectedValue;
                    notaRetiradaItem.CODIGO_DEFEITO = ddlDefeito.SelectedValue;
                    notaRetiradaItem.DATA_LANCAMENTO = Convert.ToDateTime(baseController.BuscaDataBanco().data.ToString("dd/MM/yyyy"));


                    var geraRomaneio = baseController.ObterFilialIntranet(codigoFilial);
                    if (
                        (geraRomaneio.GERA_ROMANEIO_CMAX && produtoBarra.PRODUTO.Substring(0, 1) == "6") ||
                        (geraRomaneio.GERA_ROMANEIO_HANDBOOK && produtoBarra.PRODUTO.Substring(0, 1) != "6")
                    )
                    {
                        //SALDO POR DEFEITO
                        var saldo = baseController.ObterSaldoLojaDevDefeito(produtoBarra.PRODUTO, produtoBarra.COR_PRODUTO, Convert.ToInt32(produtoBarra.TAMANHO), ddlFilial.SelectedItem.Text.Trim());
                        if (saldo == null)
                        {
                            //labErro.Text = "O produto informado não tem saldo de devolução. Entre em contato com a Logística.";
                            //return;
                            notaRetiradaItem.FILIAL = ddlFilial.SelectedItem.Text.Trim();
                            notaRetiradaItem.FILIAL_ORIGEM = "SEM SALDO";
                            notaRetiradaItem.NF_ENTRADA = "999999";
                            notaRetiradaItem.SERIE_NF_ENTRADA = "99";
                            notaRetiradaItem.ITEM_IMPRESSAO = "";
                            notaRetiradaItem.CUSTO = 0;
                            notaRetiradaItem.PEDIDO = "";
                        }
                        else
                        {
                            notaRetiradaItem.FILIAL = saldo.FILIAL;
                            notaRetiradaItem.FILIAL_ORIGEM = saldo.FILIAL_ORIGEM;
                            notaRetiradaItem.NF_ENTRADA = saldo.NF_ENTRADA;
                            notaRetiradaItem.SERIE_NF_ENTRADA = saldo.SERIE_NF_ENTRADA;
                            notaRetiradaItem.ITEM_IMPRESSAO = saldo.ITEM_IMPRESSAO;
                            notaRetiradaItem.CUSTO = saldo.CUSTO;
                            notaRetiradaItem.PEDIDO = saldo.PEDIDO;
                        }

                        notaRetiradaItem.ENL1 = (produtoBarra.TAMANHO == 1) ? 1 : 0;
                        notaRetiradaItem.ENL2 = (produtoBarra.TAMANHO == 2) ? 1 : 0;
                        notaRetiradaItem.ENL3 = (produtoBarra.TAMANHO == 3) ? 1 : 0;
                        notaRetiradaItem.ENL4 = (produtoBarra.TAMANHO == 4) ? 1 : 0;
                        notaRetiradaItem.ENL5 = (produtoBarra.TAMANHO == 5) ? 1 : 0;
                        notaRetiradaItem.ENL6 = (produtoBarra.TAMANHO == 6) ? 1 : 0;
                        notaRetiradaItem.ENL7 = (produtoBarra.TAMANHO == 7) ? 1 : 0;
                        notaRetiradaItem.ENL8 = (produtoBarra.TAMANHO == 8) ? 1 : 0;
                        notaRetiradaItem.ENL9 = (produtoBarra.TAMANHO == 9) ? 1 : 0;
                        notaRetiradaItem.ENL10 = (produtoBarra.TAMANHO == 10) ? 1 : 0;
                        notaRetiradaItem.ENL11 = (produtoBarra.TAMANHO == 11) ? 1 : 0;
                        notaRetiradaItem.ENL12 = (produtoBarra.TAMANHO == 12) ? 1 : 0;
                        notaRetiradaItem.ENL13 = (produtoBarra.TAMANHO == 13) ? 1 : 0;
                        notaRetiradaItem.ENL14 = (produtoBarra.TAMANHO == 14) ? 1 : 0;

                    }

                    // Codigo Antigo
                    NOTA_RETIRADA wNotaRetirada = usuarioController.BuscaNotaRetiradaEmAberto(Convert.ToInt32(ddlFilial.SelectedValue));
                    if (wNotaRetirada == null)
                    {
                        NOTA_RETIRADA notaRetirada = new NOTA_RETIRADA();

                        notaRetirada.CODIGO_FILIAL = Convert.ToInt32(Convert.ToInt32(ddlFilial.SelectedValue));
                        notaRetirada.BAIXADO = false;

                        usuarioController.InsereNotaRetirada(notaRetirada);

                        NOTA_RETIRADA ultimaNotaRetirada = usuarioController.BuscaNotaRetiradaEmAberto(Convert.ToInt32(ddlFilial.SelectedValue));

                        if (ultimaNotaRetirada != null)
                        {
                            notaRetiradaItem.CODIGO_NOTA_RETIRADA = ultimaNotaRetirada.CODIGO_NOTA_RETIRADA;

                            if (baseController.BuscaUltimoNotaRetiradaItem(ultimaNotaRetirada.CODIGO_NOTA_RETIRADA) == null)
                                notaRetiradaItem.ITEM_PRODUTO = 1;
                            else
                                notaRetiradaItem.ITEM_PRODUTO = baseController.BuscaUltimoNotaRetiradaItem(ultimaNotaRetirada.CODIGO_NOTA_RETIRADA) + 1;

                            usuarioController.InsereNotaRetiradaItem(notaRetiradaItem);
                        }
                    }
                    else
                    {
                        notaRetiradaItem.CODIGO_NOTA_RETIRADA = wNotaRetirada.CODIGO_NOTA_RETIRADA;

                        if (baseController.BuscaUltimoNotaRetiradaItem(wNotaRetirada.CODIGO_NOTA_RETIRADA) == null)
                            notaRetiradaItem.ITEM_PRODUTO = 1;
                        else
                            notaRetiradaItem.ITEM_PRODUTO = baseController.BuscaUltimoNotaRetiradaItem(wNotaRetirada.CODIGO_NOTA_RETIRADA) + 1;

                        usuarioController.InsereNotaRetiradaItem(notaRetiradaItem);
                    }

                    btBuscar.Enabled = true;
                    txtCodigoProduto.Enabled = true;

                    labErro.Text = "Produto incluído com Sucesso.";

                }


                CarregaGridViewNotaRetiradaItem(Convert.ToInt32(ddlFilial.SelectedValue));
                LimpaTela();

            }
            catch (Exception ex)
            {
                labErro.Text = string.Format("Erro: {0}", ex.Message);
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button bt = sender as Button;

            if (bt != null)
            {
                int retorno = usuarioController.ExcluiNotaRetiradaItem(Convert.ToInt32(bt.CommandArgument));

                if (retorno == 1)
                    labErro.Text = "Erro ao excluir Produto.";

                CarregaGridViewNotaRetiradaItem(Convert.ToInt32(ddlFilial.SelectedValue));

                LimpaTela();
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            PRODUTO produto = baseController.BuscaProduto(txtCodigoProduto.Text);

            if (produto == null)
            {
                labErro.Text = "Produto não Cadastrado !!!";
                return;
            }
            else
            {
                labErro.Text = "";

                txtDescricaoProduto.Text = produto.DESC_PRODUTO;

                CarregaDropDownListProdutoCor(produto.PRODUTO1);

                CarregaDropDownListTamanho(produto.GRADE);

                btSalvar.Enabled = true;
            }
        }

    }
}