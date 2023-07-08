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
    public partial class cont_entrada60mais_resultado : System.Web.UI.Page
    {
        ContagemController contController = new ContagemController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarFilial();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (ddlFilial.SelectedValue == "" || ddlFilial.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Filial.";
                    return;
                }

                CarregarConferenciaProduto(ddlFilial.SelectedValue);

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        #region "GRID INICIAL"
        private void CarregarConferenciaProduto(string codigoFilial)
        {
            var confProduto60Mais = contController.Obter60MaisResultado(codigoFilial);
            gvProdutoConferencia.DataSource = confProduto60Mais;
            gvProdutoConferencia.DataBind();
        }
        protected void gvProdutoConferencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int qtdeEstoqueRET = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_60MAIS_RESULTADOResult _confe60Mais = e.Row.DataItem as SP_OBTER_60MAIS_RESULTADOResult;

                    if (_confe60Mais != null)
                    {

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                            _litNome.Text = baseController.BuscaProduto(_confe60Mais.PRODUTO).DESC_PRODUTO.Trim();

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(_confe60Mais.COR_PRODUTO).DESC_COR;

                        Literal _litEstoqueLoja = e.Row.FindControl("litEstoqueLoja") as Literal;
                        if (_litEstoqueLoja != null)
                            _litEstoqueLoja.Text = (_confe60Mais.QTDE_ESTOQUE_LOJA).ToString();

                        Literal _litEstoqueRet = e.Row.FindControl("litEstoqueRet") as Literal;
                        if (_litEstoqueRet != null)
                        {
                            var estProduto = contController.ObterEstoqueProdutoPorLoja(_confe60Mais.FILIAL, _confe60Mais.PRODUTO, _confe60Mais.COR_PRODUTO);
                            if (estProduto != null)
                            {
                                qtdeEstoqueRET = Convert.ToInt32(estProduto.ESTOQUE);
                                _litEstoqueRet.Text = qtdeEstoqueRET.ToString();
                            }
                        }

                        Literal _litResultado = e.Row.FindControl("litResultado") as Literal;
                        if (_litResultado != null)
                        {
                            _litResultado.Text = ((_confe60Mais.QTDE_ESTOQUE_LOJA + _confe60Mais.QTDE_TRANSITO) - (qtdeEstoqueRET + _confe60Mais.QTDE_LIQ_POS_BAIXA)).ToString();
                        }

                        ImageButton _btBaixar = e.Row.FindControl("btBaixar") as ImageButton;
                        if (_btBaixar != null)
                            _btBaixar.CommandArgument = _confe60Mais.CODIGO.ToString();
                    }
                }
            }
        }
        #endregion

        #region "BAIXA"
        protected void btBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton btBaixar = (ImageButton)sender;
                if (btBaixar != null)
                {
                    GridViewRow row = (GridViewRow)btBaixar.NamingContainer;
                    if (row != null)
                    {
                        int codigo = 0;

                        codigo = Convert.ToInt32(btBaixar.CommandArgument);
                        string obs = ((TextBox)row.FindControl("txtObs")).Text.Trim().ToUpper();

                        var l5mais = contController.ObterLoja60Mais(codigo);
                        if (l5mais != null)
                        {
                            l5mais.USUARIO_BAIXA = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                            l5mais.OBSERVACAO = obs;
                            l5mais.STATUS = 'F';
                            contController.AtualizarLoja60Mais(l5mais);

                            CarregarConferenciaProduto(ddlFilial.SelectedValue);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = baseController.BuscaFiliais(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").ToList();

                var filialDePara = baseController.BuscaFilialDePara().ToList();
                lstFilial = lstFilial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                        btBuscar_Click(null, null);
                    }
                }
            }
        }
        #endregion

    }
}
