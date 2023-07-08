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
    public partial class jur_instancia_cad : System.Web.UI.Page
    {
        JuridicoController jurController = new JuridicoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Carregar gv gvTipoProcesso
                CarregarInstancias();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarInstancias()
        {
            List<JUR_TIPO_INSTANCIA> _tipoInstancia = new List<JUR_TIPO_INSTANCIA>();

            try
            {
                _tipoInstancia = jurController.ObterTipoInstancia().OrderBy(p => p.DESCRICAO).ToList();
                if (_tipoInstancia != null)
                {
                    gvTipoInstancia.DataSource = _tipoInstancia;
                    gvTipoInstancia.DataBind();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (gvInstancia): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labDescricao.ForeColor = Color.Black;
            labOrdem.ForeColor = Color.Black;
            labStatus.ForeColor = Color.Black;
            btCancelar.Visible = false;
            txtDescricao.Text = "";
            txtOrdem.Text = "";
            ddlStatus.SelectedValue = "A";

            CarregarInstancias();
        }
        #endregion

        #region "CRUD"
        private void Incluir(JUR_TIPO_INSTANCIA _tpInstancia)
        {
            jurController.InserirTipoInstancia(_tpInstancia);
        }
        private void Editar(JUR_TIPO_INSTANCIA _tpInstancia)
        {
            jurController.AtualizarTipoInstancia(_tpInstancia);
        }
        private void Excluir(int _codigo)
        {
            jurController.ExcluirTipoInstancia(_codigo);
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

            if (txtOrdem.Text.Trim() == "")
            {
                labErro.Text = "Informe a ORDEM.";
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

                JUR_TIPO_INSTANCIA _novo = new JUR_TIPO_INSTANCIA();
                _novo.DESCRICAO = txtDescricao.Text.Trim();
                _novo.ORDEM = Convert.ToInt32(txtOrdem.Text);
                _novo.STATUS = Convert.ToChar(ddlStatus.Text);

                var processoBase = jurController.ObterTipoInstancia().Where(p => p.ORDEM == Convert.ToInt32(txtOrdem.Text)).SingleOrDefault();
                if (_Inclusao)
                {
                    if (processoBase == null)
                    {
                        Incluir(_novo);
                    }
                    else
                    {
                        labErro.Text = "Já existe uma instância com esta ORDEM. Informe uma nova ORDEM.";
                        return;
                    }
                }
                else
                {
                    _novo.CODIGO = Convert.ToInt32(hidCodigo.Value);
                    if (processoBase != null)
                        if (processoBase.CODIGO != _novo.CODIGO)
                        {
                            labErro.Text = "Já existe uma instância com esta ORDEM. Informe uma nova ORDEM.";
                            return;
                        }

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
                    JUR_TIPO_INSTANCIA _tpInstancia = jurController.ObterTipoInstancia(Convert.ToInt32(b.CommandArgument));
                    if (_tpInstancia != null)
                    {

                        hidCodigo.Value = _tpInstancia.CODIGO.ToString();
                        txtDescricao.Text = _tpInstancia.DESCRICAO.Trim();
                        txtOrdem.Text = _tpInstancia.ORDEM.ToString();
                        ddlStatus.SelectedValue = _tpInstancia.STATUS.ToString();

                        labDescricao.ForeColor = Color.Red;
                        labOrdem.ForeColor = Color.Red;
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
                        labErro.Text = "A Instância não pode ser excluída. Esta Instância foi utilizada em um Processo.";
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
        protected void gvTipoInstancia_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    JUR_TIPO_INSTANCIA _tpInstancia = e.Row.DataItem as JUR_TIPO_INSTANCIA;

                    coluna += 1;
                    if (_tpInstancia != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _status = e.Row.FindControl("litStatus") as Literal;
                        if (_status != null)
                        {
                            if (_tpInstancia.STATUS == 'A')
                                _status.Text = "Ativo";
                            else
                                _status.Text = "Inativo";
                        }

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _tpInstancia.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _tpInstancia.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
