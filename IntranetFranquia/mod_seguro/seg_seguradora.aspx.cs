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
    public partial class seg_seguradora : System.Web.UI.Page
    {
        SeguroController segController = new SeguroController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarSeguradoras();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarSeguradoras()
        {
            List<SEG_SEGURADORA> _seguradora = new List<SEG_SEGURADORA>();

            try
            {
                _seguradora = segController.ObterSeguradora();
                if (_seguradora != null)
                {
                    gvSeguradora.DataSource = _seguradora;
                    gvSeguradora.DataBind();
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

            CarregarSeguradoras();
        }
        #endregion

        #region "CRUD"
        private void Incluir(SEG_SEGURADORA _seguradora)
        {
            segController.InserirSeguradora(_seguradora);
        }
        private void Editar(SEG_SEGURADORA _seguradora)
        {
            segController.AtualizarSeguradora(_seguradora);
        }
        private void Excluir(int _codigo)
        {
            segController.ExcluirSeguradora(_codigo);
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

                SEG_SEGURADORA _novo = new SEG_SEGURADORA();
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
                    SEG_SEGURADORA _seguradora = segController.ObterSeguradora(Convert.ToInt32(b.CommandArgument));
                    if (_seguradora != null)
                    {

                        hidCodigo.Value = _seguradora.CODIGO.ToString();
                        txtDescricao.Text = _seguradora.DESCRICAO.Trim();
                        ddlStatus.SelectedValue = _seguradora.STATUS.ToString();

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
                        labErro.Text = "A Seguradora não pode ser excluída. Esta Seguradora foi utilizada em um Seguro.";
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
        protected void gvSeguradora_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SEG_SEGURADORA _seguradora = e.Row.DataItem as SEG_SEGURADORA;

                    if (_seguradora != null)
                    {
                        Literal _status = e.Row.FindControl("litStatus") as Literal;
                        if (_status != null)
                        {
                            if (_seguradora.STATUS == 'A')
                                _status.Text = "Ativo";
                            else
                                _status.Text = "Inativo";
                        }

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _seguradora.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _seguradora.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
