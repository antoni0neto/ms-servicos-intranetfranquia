using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class estoque_baixa_nf_entrada_prod : System.Web.UI.Page
    {
        EstoqueController estoqueController = new EstoqueController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarNFs();
            }

        }


        #region "NF"
        private List<SP_OBTER_NF_PRODUTO_ACABADOResult> ObterNFEntradaProdutoAcabado()
        {
            DateTime dataIni = DateTime.Today;
            DateTime dataFim = DateTime.Today;
            string filial = "";
            string nomeCliFor = "";

            var nfEntrada = estoqueController.ObterNFEntradaProdutoAcabado(dataIni, dataFim, filial, nomeCliFor);
            return nfEntrada;
        }
        private void CarregarNFs()
        {
            var nfEntrada = ObterNFEntradaProdutoAcabado();

            gvNFEntrada.DataSource = nfEntrada;
            gvNFEntrada.DataBind();


        }
        protected void gvNFEntrada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_NF_PRODUTO_ACABADOResult nfEntrada = e.Row.DataItem as SP_OBTER_NF_PRODUTO_ACABADOResult;

                    if (nfEntrada != null)
                    {
                        CheckBox cbBaixar = e.Row.FindControl("cbBaixar") as CheckBox;
                        if (nfEntrada.CODIGO > 0)
                        {
                            cbBaixar.Checked = true;
                            e.Row.BackColor = Color.PaleGreen;
                        }
                        else
                        {
                            cbBaixar.Checked = false;
                            e.Row.BackColor = Color.White;
                        }
                    }
                }
            }
        }

        protected void btBaixar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {

                        string nomeCliFor = ((Literal)row.FindControl("litNomeClifor")).Text;
                        string nfEntrada = ((Literal)row.FindControl("litNFEntrada")).Text;
                        string serieNFEntrada = ((Literal)row.FindControl("litSerieNFEntrada")).Text;
                        DateTime recebimento = Convert.ToDateTime(((Literal)row.FindControl("litRecebimento")).Text);

                        CTB_NF_ENTRADA_BAIXA nfEntradaBaixa = new CTB_NF_ENTRADA_BAIXA();
                        nfEntradaBaixa.NOME_CLIFOR = nomeCliFor;
                        nfEntradaBaixa.NF_ENTRADA = nfEntrada;
                        nfEntradaBaixa.SERIE_NF_ENTRADA = serieNFEntrada;
                        nfEntradaBaixa.RECEBIMENTO = recebimento;
                        nfEntradaBaixa.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        nfEntradaBaixa.DATA_INCLUSAO = DateTime.Now;
                        estoqueController.InserirNFEntrada(nfEntradaBaixa);

                        CarregarNFs();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion

        protected void cbBaixar_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb != null)
            {
                try
                {
                    GridViewRow row = (GridViewRow)cb.NamingContainer;
                    if (row != null)
                    {
                        string nomeCliFor = ((Literal)row.FindControl("litNomeClifor")).Text;
                        string nfEntrada = ((Literal)row.FindControl("litNFEntrada")).Text;
                        string serieNFEntrada = ((Literal)row.FindControl("litSerieNFEntrada")).Text;
                        DateTime recebimento = Convert.ToDateTime(((Literal)row.FindControl("litRecebimento")).Text);

                        if (cb.Checked)
                        {
                            CTB_NF_ENTRADA_BAIXA nfEntradaBaixa = new CTB_NF_ENTRADA_BAIXA();
                            nfEntradaBaixa.NOME_CLIFOR = nomeCliFor;
                            nfEntradaBaixa.NF_ENTRADA = nfEntrada;
                            nfEntradaBaixa.SERIE_NF_ENTRADA = serieNFEntrada;
                            nfEntradaBaixa.RECEBIMENTO = recebimento;
                            nfEntradaBaixa.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                            nfEntradaBaixa.DATA_INCLUSAO = DateTime.Now;
                            estoqueController.InserirNFEntrada(nfEntradaBaixa);
                        }
                        else
                        {
                            var nfEntradaBaixada = estoqueController.ObterNFEntrada(nomeCliFor, nfEntrada, serieNFEntrada, recebimento);
                            estoqueController.ExcluirNFEntrada(nfEntradaBaixada.CODIGO);
                        }

                        CarregarNFs();
                    }
                }
                catch (Exception)
                {
                }
            }
        }


    }
}

