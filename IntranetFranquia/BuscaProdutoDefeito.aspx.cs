using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class BuscaProdutoDefeito : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCodigoProduto.Attributes.Add("onKeyPress", "doClick('" + btBuscar.ClientID + "',event)");

                if (Session["USUARIO"] == null)
                    Response.Redirect("~/Login.aspx");
            }
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
/*
        private void CarregaNotaRetiradaItem(int codigoNotaRetiradaItem)
        {
            NOTA_RETIRADA_ITEM notaRetiradaItem = usuarioController.BuscaNotaRetiradaItem(codigoNotaRetiradaItem);

            if (notaRetiradaItem != null)
            {
                btBuscar.Enabled = false;

                txtCodigoProduto.Text = notaRetiradaItem.CODIGO_PRODUTO.ToString();
                txtCodigoProduto.Enabled = false;

                CarregaDropDownListProdutoCor(notaRetiradaItem.CODIGO_PRODUTO.ToString());

                PRODUTO produto = baseController.BuscaProduto(notaRetiradaItem.CODIGO_PRODUTO.ToString());

                if (produto != null)
                    CarregaDropDownListTamanho(produto.GRADE);
            }
        }
        */
        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            if (ddlTamanho.SelectedValue.ToString().Equals(""))
            {
                LabelFeedBack.Text = "Informar Tamanho !!!";

                return;
            }

            CarregaGridViewNotaRetiradaItem(txtCodigoProduto.Text, ddlProdutoCor.SelectedItem.ToString(), ddlTamanho.SelectedItem.ToString());
        }

        protected void GridViewNotaRetiradaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_RETIRADA_ITEM notaRetiradaItem = e.Row.DataItem as NOTA_RETIRADA_ITEM;

            if (notaRetiradaItem != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                {
                    NOTA_RETIRADA notaRetirada = baseController.BuscaNotaRetirada(notaRetiradaItem.CODIGO_NOTA_RETIRADA);

                    if (notaRetirada != null)
                    {
                        FILIAI filial = baseController.BuscaFilialCodigo(notaRetirada.CODIGO_FILIAL);

                        if (filial != null)
                            literalFilial.Text = filial.FILIAL;
                    }
                }
            }
        }

        private void CarregaGridViewNotaRetiradaItem(string codigoProduto, string cor, string tamanho)
        {
            GridViewNotaRetiradaItem.DataSource = usuarioController.BuscaProdutoDefeitoSegundaQualidade(codigoProduto, cor, tamanho);
            GridViewNotaRetiradaItem.DataBind();
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            txtCodigoProduto.Text = string.Empty;

            ddlTamanho.Items.Clear();
            ddlProdutoCor.Items.Clear();
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

                CarregaDropDownListProdutoCor(produto.PRODUTO1);

                CarregaDropDownListTamanho(produto.GRADE);
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

            item = new Tamanho();
            
            indice++;
            
            item.Codigo = indice;
            item.Descricao = texto;

            lista.Add(item);

            return lista;
        }
    }
}