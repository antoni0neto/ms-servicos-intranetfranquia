using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class CadastroNotaDefeito : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCodigoProduto.Attributes.Add("onKeyPress", "doClick('" + btBuscar.ClientID + "',event)");
                txtDescricaoProduto.Attributes.Add("onKeyPress", "doClick('" + ButtonSalvar.ClientID + "',event)");

                CarregaDropDownListOrigemDefeito();
                CarregaDropDownListDefeito();

                if (Session["USUARIO"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                {
                    USUARIO usuario = (USUARIO)Session["USUARIO"];

                    if (usuario != null)
                    {
                        ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                        ddlFilial.DataBind();
                    }
                }
            }
        }

        protected void btContinue_Click(object sender, EventArgs e)
        {
            if (ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewNotaRetiradaItem(Convert.ToInt32(ddlFilial.SelectedValue));
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        private void CarregaDropDownListOrigemDefeito()
        {
            ddlOrigemDefeito.DataSource = baseController.BuscaOrigensDefeito();
            ddlOrigemDefeito.DataBind();
        }

        private void CarregaDropDownListDefeito()
        {
            ddlDefeito.DataSource = baseController.BuscaDefeitos();
            ddlDefeito.DataBind();
        }

        private void CarregaDropDownListTamanho(string grade)
        {
            List<Tamanho> listaTamanho = MontaLista(grade);

            if (listaTamanho != null)
            {
                ddlTamanho.DataSource = listaTamanho;
                ddlTamanho.DataBind();
            }
        }

        private void CarregaDropDownListProdutoCor(string codigoProduto)
        {
            ddlProdutoCor.DataSource = baseController.BuscaProdutoCores(codigoProduto);
            ddlProdutoCor.DataBind();
        }

        private void CarregaNotaRetiradaItem(int codigoNotaRetiradaItem)
        {
            NOTA_RETIRADA_ITEM notaRetiradaItem = usuarioController.BuscaNotaRetiradaItem(codigoNotaRetiradaItem);

            if (notaRetiradaItem != null)
            {
                btBuscar.Enabled = false;
                ButtonSalvar.Enabled = true;

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

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            if (ddlTamanho.SelectedValue.ToString().Equals(""))
            {
                LabelFeedBack.Text = "Informar Tamanho !!!";

                return;
            }

            try
            {
                NOTA_RETIRADA_ITEM notaRetiradaItem;

                if (Convert.ToInt32(HiddenFieldCodigoNotaRetiradaItem.Value) > 0)
                    notaRetiradaItem = usuarioController.BuscaNotaRetiradaItem(Convert.ToInt32(HiddenFieldCodigoNotaRetiradaItem.Value));
                else
                    notaRetiradaItem = new NOTA_RETIRADA_ITEM();

                notaRetiradaItem.CODIGO_PRODUTO = Convert.ToInt32(txtCodigoProduto.Text);
                notaRetiradaItem.COR_PRODUTO = ddlProdutoCor.SelectedItem.ToString();
                notaRetiradaItem.DESCRICAO_PRODUTO = txtDescricaoProduto.Text;
                notaRetiradaItem.TAMANHO = ddlTamanho.SelectedItem.ToString();
                notaRetiradaItem.CODIGO_ORIGEM_DEFEITO = ddlOrigemDefeito.SelectedValue;
                notaRetiradaItem.CODIGO_DEFEITO = ddlDefeito.SelectedValue;
                notaRetiradaItem.DATA_LANCAMENTO = Convert.ToDateTime(baseController.BuscaDataBanco().data.ToString("dd/MM/yyyy"));

                if (Convert.ToInt32(HiddenFieldCodigoNotaRetiradaItem.Value) > 0)
                    usuarioController.AtualizaNotaRetiradaItem(notaRetiradaItem);
                else
                {
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
                }

                btBuscar.Enabled = true;
                txtCodigoProduto.Enabled = true;

                LabelFeedBack.Text = "Gravado com sucesso!";

                CarregaGridViewNotaRetiradaItem(Convert.ToInt32(ddlFilial.SelectedValue));

                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewNotaRetiradaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_RETIRADA_ITEM notaRetiradaItem = e.Row.DataItem as NOTA_RETIRADA_ITEM;

            if (notaRetiradaItem != null)
            {
                Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;

                if (buttonEditar != null)
                    buttonEditar.CommandArgument = notaRetiradaItem.CODIGO_NOTA_RETIRADA_ITEM.ToString();

                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;

                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = notaRetiradaItem.CODIGO_NOTA_RETIRADA_ITEM.ToString();

                Literal literalOrigemDefeito = e.Row.FindControl("LiteralOrigemDefeito") as Literal;

                if (literalOrigemDefeito != null)
                {
                    NOTA_RETIRADA_ORIGEM_DEFEITO origemDefeito = baseController.BuscaOrigemDefeito(notaRetiradaItem.CODIGO_ORIGEM_DEFEITO);

                    if (origemDefeito != null)
                        literalOrigemDefeito.Text = origemDefeito.DESCRICAO_ORIGEM_DEFEITO;
                }

                Literal literalDefeito = e.Row.FindControl("LiteralDefeito") as Literal;

                if (literalDefeito != null)
                {
                    NOTA_RETIRADA_DEFEITO defeito = baseController.BuscaDefeito(notaRetiradaItem.CODIGO_DEFEITO);

                    if (defeito != null)
                        literalDefeito.Text = defeito.DESCRICAO_DEFEITO;
                }
            }
        }

        private void CarregaGridViewNotaRetiradaItem(int filial)
        {
            GridViewNotaRetiradaItem.DataSource = usuarioController.BuscaFilialNotaRetiradaItem(filial);
            GridViewNotaRetiradaItem.DataBind();
        }

        protected void ButtonExcluirNotaRetiradaItem_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                int retorno = usuarioController.ExcluiNotaRetiradaItem(Convert.ToInt32(buttonExcluir.CommandArgument));

                if (retorno == 1)
                    LabelFeedBack.Text = "Erro na exclusão do Item de Produto !!!";

                CarregaGridViewNotaRetiradaItem(Convert.ToInt32(ddlFilial.SelectedValue));

                LimpaTela();
            }
        }

        protected void ButtonEditarNotaRetiradaItem_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;

            if (buttonEditar != null)
            {
                CarregaNotaRetiradaItem(Convert.ToInt32(buttonEditar.CommandArgument));

                LimpaFeedBack();
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
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
            
            ButtonSalvar.Enabled = false;

            HiddenFieldCodigoNotaRetiradaItem.Value = "0";
        }

        protected void ddlOrigemDefeito_DataBound(object sender, EventArgs e)
        {
            ddlOrigemDefeito.Items.Add(new ListItem("Selecione", "0"));
            ddlOrigemDefeito.SelectedValue = "0";
        }

        protected void ddlDefeito_DataBound(object sender, EventArgs e)
        {
            ddlDefeito.Items.Add(new ListItem("Selecione", "0"));
            ddlDefeito.SelectedValue = "0";
        }

        protected void ddlProdutoCor_DataBound(object sender, EventArgs e)
        {
            ddlProdutoCor.Items.Add(new ListItem("Selecione", "0"));
            ddlProdutoCor.SelectedValue = "0";
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            PRODUTO produto = baseController.BuscaProduto(txtCodigoProduto.Text);

            if (produto == null)
            {
                LabelFeedBack.Text = "Produto não Cadastrado !!!";

                return;
            }
            else
            {
                LabelFeedBack.Text = "";

                txtDescricaoProduto.Text = produto.DESC_PRODUTO;

                CarregaDropDownListProdutoCor(produto.PRODUTO1);

                CarregaDropDownListTamanho(produto.GRADE);

                ButtonSalvar.Enabled = true;
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
    }
}