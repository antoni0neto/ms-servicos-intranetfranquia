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
using System.Data.OleDb;

namespace Relatorios
{
    public partial class desenv_produto_origem : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy' });});", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy' });});", true);

            if (!Page.IsPostBack)
            {

                //Carregar combo de unidade e gv de aviamento
                CarregarColecoes();
                CarregarProdutoOrigem();

                //Controle de Controles
                btCancelar.Visible = false;
            }
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
        private void CarregarProdutoOrigem()
        {
            List<DESENV_PRODUTO_ORIGEM> _produtoOrigem = new List<DESENV_PRODUTO_ORIGEM>();

            try
            {
                _produtoOrigem = desenvController.ObterProdutoOrigem();
                if (_produtoOrigem != null)
                {
                    gvProdutoOrigem.DataSource = _produtoOrigem;
                    gvProdutoOrigem.DataBind();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (gvProdutoOrigem): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labDescricao.ForeColor = Color.Black;
            labColecao.ForeColor = Color.Black;
            labDataInicial.ForeColor = Color.Black;
            labDataFinal.ForeColor = Color.Black;
            labStatus.ForeColor = Color.Black;
            btCancelar.Visible = false;
            txtDescricao.Text = "";
            ddlColecoes.SelectedValue = "0";
            txtDataInicial.Text = "";
            txtDataFinal.Text = "";
            ddlStatus.SelectedValue = "A";

            CarregarProdutoOrigem();
        }
        #endregion

        #region "CRUD"
        private void Incluir(DESENV_PRODUTO_ORIGEM _origem)
        {
            desenvController.InserirProdutoOrigem(_origem);
        }
        private void Editar(DESENV_PRODUTO_ORIGEM _origem)
        {
            desenvController.AtualizarProdutoOrigem(_origem);
        }
        private void Excluir(int _codigo)
        {
            desenvController.ExcluirProdutoOrigem(_codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            labErro.Text = "";
            if (ddlColecoes.SelectedValue.Trim() == "0" || ddlColecoes.SelectedValue.Trim() == "")
            {
                labErro.Text = "Selecione a Coleção.";
                return;
            }

            if (txtDescricao.Text.Trim() == "")
            {
                labErro.Text = "Informe a descrição da Origem.";
                return;
            }

            if (txtDataInicial.Text.Trim() == "")
            {
                labErro.Text = "Informe a Data Inicial.";
                return;
            }

            if (txtDataFinal.Text.Trim() == "")
            {
                labErro.Text = "Informe a Data Final.";
                return;
            }

            if (ddlStatus.SelectedValue.Trim() == "0")
            {
                labErro.Text = "Selecione o Status.";
                return;
            }

            try
            {
                _Inclusao = true;
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    _Inclusao = false;

                DESENV_PRODUTO_ORIGEM _novo = new DESENV_PRODUTO_ORIGEM();
                _novo.COLECAO = ddlColecoes.SelectedValue.Trim();
                _novo.DESCRICAO = txtDescricao.Text.Trim().ToUpper();
                _novo.DATA_INICIAL = Convert.ToDateTime(txtDataInicial.Text);
                _novo.DATA_FINAL = Convert.ToDateTime(txtDataFinal.Text);
                _novo.STATUS = Convert.ToChar(ddlStatus.Text);

                if (_Inclusao)
                {
                    Incluir(_novo);
                }
                else
                {
                    _novo.CODIGO = Convert.ToInt32(hidCodigo.Value);
                    Editar(_novo);
                }

                RecarregarTela();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        protected void btAlterar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    DESENV_PRODUTO_ORIGEM _origem = desenvController.ObterProdutoOrigem(Convert.ToInt32(b.CommandArgument));
                    if (_origem != null)
                    {
                        string colecao = "";
                        colecao = (new BaseController().BuscaColecaoAtual(_origem.COLECAO)).COLECAO;

                        hidCodigo.Value = _origem.CODIGO.ToString();
                        txtDescricao.Text = _origem.DESCRICAO.Trim().ToUpper();
                        ddlColecoes.SelectedValue = colecao;
                        txtDataInicial.Text = (_origem.DATA_INICIAL == null) ? "" : Convert.ToDateTime(_origem.DATA_INICIAL).ToString("dd/MM/yyyy");
                        txtDataFinal.Text = (_origem.DATA_FINAL == null) ? "" : Convert.ToDateTime(_origem.DATA_FINAL).ToString("dd/MM/yyyy");
                        ddlStatus.SelectedValue = _origem.STATUS.ToString();
                        labDescricao.ForeColor = Color.Red;
                        labColecao.ForeColor = Color.Red;
                        labDataInicial.ForeColor = Color.Red;
                        labDataFinal.ForeColor = Color.Red;
                        labStatus.ForeColor = Color.Red;
                        btCancelar.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btAlterar_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {

            Button b = (Button)sender;
            if (b != null)
            {
                labErro.Text = "";
                try
                {
                    try
                    {
                        Excluir(Convert.ToInt32(b.CommandArgument));
                        RecarregarTela();
                    }
                    catch (Exception)
                    {
                        labErro.Text = "A origem não pode ser excluída. Esta Origem já está cadastrada em um Produto.";
                    }

                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            RecarregarTela();
        }
        #endregion

        protected void gvProdutoOrigem_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO_ORIGEM _origem = e.Row.DataItem as DESENV_PRODUTO_ORIGEM;

                    coluna += 1;
                    if (_origem != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(_origem.COLECAO)).DESC_COLECAO;

                        Literal _litDataInicial = e.Row.FindControl("litDataInicial") as Literal;
                        if (_litDataInicial != null)
                            _litDataInicial.Text = (_origem.DATA_INICIAL == null) ? "" : Convert.ToDateTime(_origem.DATA_INICIAL).ToString("dd/MM/yyyy");

                        Literal _litDataFinal = e.Row.FindControl("litDataFinal") as Literal;
                        if (_litDataFinal != null)
                            _litDataFinal.Text = (_origem.DATA_FINAL == null) ? "" : Convert.ToDateTime(_origem.DATA_FINAL).ToString("dd/MM/yyyy");

                        Literal _status = e.Row.FindControl("litStatus") as Literal;
                        if (_status != null)
                        {
                            if (_origem.STATUS == 'A')
                                _status.Text = "Ativo";
                            else
                                _status.Text = "Inativo";
                        }

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _origem.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _origem.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
