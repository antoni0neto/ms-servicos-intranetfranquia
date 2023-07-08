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
    public partial class faccadm_tipo_doc_cad : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarTiposDocumento();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarTiposDocumento()
        {
            List<FACC_TIPO_DOCUMENTO> _tipoDocumento = new List<FACC_TIPO_DOCUMENTO>();

            try
            {
                _tipoDocumento = faccController.ObterTipoDocumento();
                if (_tipoDocumento != null)
                {
                    gvTipoDocumento.DataSource = _tipoDocumento;
                    gvTipoDocumento.DataBind();
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
            labDocumento.ForeColor = Color.Black;
            labTipoVigencia.ForeColor = Color.Black;
            labDescricao.ForeColor = Color.Black;
            labStatus.ForeColor = Color.Black;
            btCancelar.Visible = false;
            txtDocumento.Text = "";
            txtDescricao.Text = "";
            ddlTipoVigencia.SelectedValue = "";
            ddlStatus.SelectedValue = "A";

            CarregarTiposDocumento();
        }
        #endregion

        #region "CRUD"
        private void Incluir(FACC_TIPO_DOCUMENTO _tpDocumento)
        {
            faccController.InserirTipoDocumento(_tpDocumento);
        }
        private void Editar(FACC_TIPO_DOCUMENTO _tpDocumento)
        {
            faccController.AtualizarTipoDocumento(_tpDocumento);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            labErro.Text = "";
            if (txtDocumento.Text.Trim() == "")
            {
                labErro.Text = "Informe o Documento.";
                return;
            }

            if (ddlTipoVigencia.SelectedValue.Trim() == "")
            {
                labErro.Text = "Selecione o Tipo de Vigência.";
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

                FACC_TIPO_DOCUMENTO _novo = new FACC_TIPO_DOCUMENTO();
                _novo.DOCUMENTO = txtDocumento.Text.Trim().ToUpper();
                _novo.TIPO_VIGENCIA = Convert.ToChar(ddlTipoVigencia.SelectedValue);
                _novo.DESCRICAO = txtDescricao.Text.Trim();
                _novo.DATA_INCLUSAO = DateTime.Now;
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
                    FACC_TIPO_DOCUMENTO _tpDocumento = faccController.ObterTipoDocumento(Convert.ToInt32(b.CommandArgument));
                    if (_tpDocumento != null)
                    {

                        hidCodigo.Value = _tpDocumento.CODIGO.ToString();
                        txtDescricao.Text = _tpDocumento.DESCRICAO.Trim();
                        txtDocumento.Text = _tpDocumento.DOCUMENTO;
                        ddlTipoVigencia.SelectedValue = _tpDocumento.TIPO_VIGENCIA.ToString();
                        ddlStatus.SelectedValue = _tpDocumento.STATUS.ToString();

                        labDocumento.ForeColor = Color.Red;
                        labDescricao.ForeColor = Color.Red;
                        labTipoVigencia.ForeColor = Color.Red;
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
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            RecarregarTela();
        }
        #endregion
        protected void gvTipoDocumento_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    FACC_TIPO_DOCUMENTO _tpDocumento = e.Row.DataItem as FACC_TIPO_DOCUMENTO;

                    if (_tpDocumento != null)
                    {

                        Literal _litTipoVigencia = e.Row.FindControl("litTipoVigencia") as Literal;
                        if (_litTipoVigencia != null)
                        {
                            if (_tpDocumento.TIPO_VIGENCIA == 'A')
                                _litTipoVigencia.Text = "Anual";
                            else if (_tpDocumento.TIPO_VIGENCIA == 'M')
                                _litTipoVigencia.Text = "Mensal";
                            else if (_tpDocumento.TIPO_VIGENCIA == 'R')
                                _litTipoVigencia.Text = "Vitalício";
                            else
                                _litTipoVigencia.Text = "Inválido";
                        }

                        Literal _litStatus = e.Row.FindControl("litStatus") as Literal;
                        if (_litStatus != null)
                        {
                            if (_tpDocumento.STATUS == 'A')
                                _litStatus.Text = "Ativo";
                            else
                                _litStatus.Text = "Inativo";
                        }

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _tpDocumento.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
