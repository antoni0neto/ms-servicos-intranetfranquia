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
    public partial class jur_tipo_processo_cad : System.Web.UI.Page
    {
        JuridicoController jurController = new JuridicoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Carregar gv gvTipoProcesso
                CarregarTiposProcessos();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarTiposProcessos()
        {
            List<JUR_TIPO_PROCESSO> _tipoProcesso = new List<JUR_TIPO_PROCESSO>();

            try
            {
                _tipoProcesso = jurController.ObterTipoProcesso().OrderBy(p => p.DESCRICAO).ToList();
                if (_tipoProcesso != null)
                {
                    gvTipoProcesso.DataSource = _tipoProcesso;
                    gvTipoProcesso.DataBind();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (gvTipoProcesso): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labDescricao.ForeColor = Color.Black;
            labStatus.ForeColor = Color.Black;
            btCancelar.Visible = false;
            txtDescricao.Text = "";
            ddlStatus.SelectedValue = "A";

            CarregarTiposProcessos();
        }
        #endregion

        #region "CRUD"
        private void Incluir(JUR_TIPO_PROCESSO _tpProcesso)
        {
            jurController.InserirTipoProcesso(_tpProcesso);
        }
        private void Editar(JUR_TIPO_PROCESSO _tpProcesso)
        {
            jurController.AtualizarTipoProcesso(_tpProcesso);
        }
        private void Excluir(int _codigo)
        {
            jurController.ExcluirTipoProcesso(_codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            labErro.Text = "";
            if (txtDescricao.Text.Trim() == "")
            {
                labErro.Text = "Informe a DESCRIÇÃO.";
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

                JUR_TIPO_PROCESSO _novo = new JUR_TIPO_PROCESSO();
                _novo.DESCRICAO = txtDescricao.Text.Trim();
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
                    JUR_TIPO_PROCESSO _tpProcesso = jurController.ObterTipoProcesso(Convert.ToInt32(b.CommandArgument));
                    if (_tpProcesso != null)
                    {

                        hidCodigo.Value = _tpProcesso.CODIGO.ToString();

                        txtDescricao.Text = _tpProcesso.DESCRICAO.Trim();
                        ddlStatus.SelectedValue = _tpProcesso.STATUS.ToString();

                        labDescricao.ForeColor = Color.Red;
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
                        labErro.Text = "O Tipo de Processo não pode ser excluído. Este Tipo de Processo foi utilizado em um Processo.";
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
        protected void gvTipoProcesso_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    JUR_TIPO_PROCESSO _tpProcesso = e.Row.DataItem as JUR_TIPO_PROCESSO;

                    coluna += 1;
                    if (_tpProcesso != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _status = e.Row.FindControl("litStatus") as Literal;
                        if (_status != null)
                        {
                            if (_tpProcesso.STATUS == 'A')
                                _status.Text = "Ativo";
                            else
                                _status.Text = "Inativo";
                        }

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _tpProcesso.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _tpProcesso.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
