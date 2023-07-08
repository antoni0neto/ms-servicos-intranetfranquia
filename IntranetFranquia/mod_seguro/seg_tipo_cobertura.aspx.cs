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
    public partial class seg_tipo_cobertura : System.Web.UI.Page
    {
        SeguroController segController = new SeguroController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarTiposCoberturas();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarTiposCoberturas()
        {
            List<SEG_TIPO_COBERTURA> _tipoCobertura = new List<SEG_TIPO_COBERTURA>();

            try
            {
                _tipoCobertura = segController.ObterTipoCobertura();
                if (_tipoCobertura != null)
                {
                    gvTipoCobertura.DataSource = _tipoCobertura;
                    gvTipoCobertura.DataBind();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (gvTipoSeguro): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
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

            CarregarTiposCoberturas();
        }
        #endregion

        #region "CRUD"
        private void Incluir(SEG_TIPO_COBERTURA _tpCobertura)
        {
            segController.InserirTipoCobertura(_tpCobertura);
        }
        private void Editar(SEG_TIPO_COBERTURA _tpCobertura)
        {
            segController.AtualizarTipoCobertura(_tpCobertura);
        }
        private void Excluir(int _codigo)
        {
            segController.ExcluirTipoCobertura(_codigo);
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

                SEG_TIPO_COBERTURA _novo = new SEG_TIPO_COBERTURA();
                _novo.DESCRICAO = txtDescricao.Text.Trim().ToUpper();
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
                    SEG_TIPO_COBERTURA _tpCobertura = segController.ObterTipoCobertura(Convert.ToInt32(b.CommandArgument));
                    if (_tpCobertura != null)
                    {

                        hidCodigo.Value = _tpCobertura.CODIGO.ToString();
                        txtDescricao.Text = _tpCobertura.DESCRICAO.Trim();
                        ddlStatus.SelectedValue = _tpCobertura.STATUS.ToString();

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
                        labErro.Text = "O Tipo de Cobertura não pode ser excluído. Este Tipo foi utilizado em um Seguro.";
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
        protected void gvTipoCobertura_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SEG_TIPO_COBERTURA _tpCobertura = e.Row.DataItem as SEG_TIPO_COBERTURA;

                    if (_tpCobertura != null)
                    {
                        Literal _status = e.Row.FindControl("litStatus") as Literal;
                        if (_status != null)
                        {
                            if (_tpCobertura.STATUS == 'A')
                                _status.Text = "Ativo";
                            else
                                _status.Text = "Inativo";
                        }

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _tpCobertura.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _tpCobertura.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
