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
    public partial class seg_seguro_cad : System.Web.UI.Page
    {
        SeguroController segController = new SeguroController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataVencimento.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

                int codigoSeguro = 0;
                if (Request.QueryString["s"] == null || Request.QueryString["s"] == "")
                    Response.Redirect("seg_menu.aspx");

                codigoSeguro = Convert.ToInt32(Request.QueryString["s"].ToString());
                SEG_SEGURO _seguro = segController.ObterSeguro(codigoSeguro);

                CarregarEmpresa();
                CarregarTipoSeguro();
                CarregarSeguradora();
                CarregarCorretor();
                CarregarTipoCobertura();

                if (_seguro == null)
                {
                    codigoSeguro = CriarSeguro();
                    hidCodigoSeguro.Value = codigoSeguro.ToString();
                }
                else
                {
                    hidCodigoSeguro.Value = codigoSeguro.ToString();

                    txtProposta.Text = _seguro.PROPOSTA;
                    txtApolice.Text = _seguro.APOLICE;
                    ddlEmpresa.SelectedValue = _seguro.EMPRESA.ToString();
                    ddlTipoSeguro.SelectedValue = _seguro.SEG_TIPO_SEGURO.ToString();
                    ddlSeguradora.SelectedValue = _seguro.SEG_SEGURADORA.ToString();
                    ddlCorretor.SelectedValue = _seguro.SEG_CORRETOR.ToString();
                    txtDataVencimento.Text = Convert.ToDateTime(_seguro.DATA_VENCIMENTO).ToString("dd/MM/yyyy");
                    txtValorPremio.Text = _seguro.VALOR_PREMIO.ToString();
                    txtObservacao.Text = _seguro.OBSERVACAO;

                    CarregarItem();
                    CarregarItemCobertura();

                    btExcluir.Visible = true;
                }

                dialogPai.Visible = false;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarEmpresa()
        {
            var empresa = new BaseController().ObterEmpresa();

            empresa.Insert(0, new EMPRESA { CODIGO_EMPRESA = 0, NOME = "" });
            ddlEmpresa.DataSource = empresa;
            ddlEmpresa.DataBind();
        }
        private void CarregarTipoSeguro()
        {
            var tipoSeguro = segController.ObterTipoSeguro();

            tipoSeguro.Insert(0, new SEG_TIPO_SEGURO { CODIGO = 0, DESCRICAO = "" });
            ddlTipoSeguro.DataSource = tipoSeguro;
            ddlTipoSeguro.DataBind();
        }
        private void CarregarSeguradora()
        {
            var seguradora = segController.ObterSeguradora();

            seguradora.Insert(0, new SEG_SEGURADORA { CODIGO = 0, DESCRICAO = "" });
            ddlSeguradora.DataSource = seguradora;
            ddlSeguradora.DataBind();
        }
        private void CarregarCorretor()
        {
            var corretor = segController.ObterCorretor();

            corretor.Insert(0, new SEG_CORRETOR { CODIGO = 0, DESCRICAO = "" });
            ddlCorretor.DataSource = corretor;
            ddlCorretor.DataBind();
        }
        private void CarregarTipoCobertura()
        {
            var tipoCobertura = segController.ObterTipoCobertura();

            tipoCobertura.Insert(0, new SEG_TIPO_COBERTURA { CODIGO = 0, DESCRICAO = "" });
            ddlTipoCobertura.DataSource = tipoCobertura;
            ddlTipoCobertura.DataBind();
        }

        private int CriarSeguro()
        {
            int codigoSeguro = 0;

            SEG_SEGURO seguro = new SEG_SEGURO();
            seguro.DATA_ALTERACAO = DateTime.Now;
            seguro.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            seguro.STATUS = 'X';

            codigoSeguro = segController.InserirSeguro(seguro);
            return codigoSeguro;
        }
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labProposta.ForeColor = _OK;
            labApolice.ForeColor = _OK;
            if (txtProposta.Text.Trim() == "" && txtApolice.Text.Trim() == "")
            {
                labProposta.ForeColor = _notOK;
                labApolice.ForeColor = _notOK;
                retorno = false;
            }

            labEmpresa.ForeColor = _OK;
            if (ddlEmpresa.SelectedValue == "0")
            {
                labEmpresa.ForeColor = _notOK;
                retorno = false;
            }

            labTipoSeguro.ForeColor = _OK;
            if (ddlTipoSeguro.SelectedValue == "0")
            {
                labTipoSeguro.ForeColor = _notOK;
                retorno = false;
            }

            labSeguradora.ForeColor = _OK;
            if (ddlSeguradora.SelectedValue == "0")
            {
                labSeguradora.ForeColor = _notOK;
                retorno = false;
            }

            labCorretor.ForeColor = _OK;
            if (ddlCorretor.SelectedValue == "0")
            {
                labCorretor.ForeColor = _notOK;
                retorno = false;
            }

            labDataVencimento.ForeColor = _OK;
            if (txtDataVencimento.Text.Trim() == "")
            {
                labDataVencimento.ForeColor = _notOK;
                retorno = false;
            }

            labValorPremio.ForeColor = _OK;
            if (txtValorPremio.Text.Trim() == "")
            {
                labValorPremio.ForeColor = _notOK;
                retorno = false;
            }

            labItens.ForeColor = _OK;
            if (gvItemCobertura.Rows.Count <= 0)
            {
                labItens.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if (bt != null)
            {
                try
                {
                    labErro.Text = "";

                    if (!ValidarCampos())
                    {
                        labErro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                        return;
                    }

                    SEG_SEGURO _seguro = segController.ObterSeguro(Convert.ToInt32(hidCodigoSeguro.Value));
                    if (_seguro == null)
                    {
                        labErro.Text = "Seguro não encontrado. Entre em contato com o TI.";
                        return;
                    }

                    _seguro.EMPRESA = Convert.ToInt32(ddlEmpresa.SelectedValue);
                    _seguro.SEG_TIPO_SEGURO = Convert.ToInt32(ddlTipoSeguro.SelectedValue);
                    _seguro.APOLICE = txtApolice.Text.Trim().ToUpper();
                    _seguro.PROPOSTA = txtProposta.Text.Trim().ToUpper();
                    _seguro.SEG_SEGURADORA = Convert.ToInt32(ddlSeguradora.SelectedValue);
                    _seguro.SEG_CORRETOR = Convert.ToInt32(ddlCorretor.SelectedValue);
                    _seguro.DATA_VENCIMENTO = Convert.ToDateTime(txtDataVencimento.Text.Trim());
                    _seguro.VALOR_PREMIO = Convert.ToDecimal(txtValorPremio.Text.Trim());
                    _seguro.OBSERVACAO = txtObservacao.Text.Trim().ToUpper();
                    _seguro.DATA_ALTERACAO = DateTime.Now;
                    _seguro.STATUS = 'A';

                    segController.AtualizarSeguro(_seguro);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('seg_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('seg_menu.aspx', '_self'); }, 'Seguro Novo': function () { window.open('seg_seguro_cad.aspx?s=0', '_self'); } } }); });", true);
                    dialogPai.Visible = true;

                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if (bt != null)
            {
                try
                {
                    labErro.Text = "";

                    int codigoSeguro = Convert.ToInt32(hidCodigoSeguro.Value);

                    segController.ExcluirSeguro(codigoSeguro);

                    labMensagem.Text = "EXCLUÍDO COM SUCESSO";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('seg_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('seg_menu.aspx', '_self'); }, 'Seguro Novo': function () { window.open('seg_seguro_cad.aspx?s=0', '_self'); } } }); });", true);
                    dialogPai.Visible = true;
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }

        #region "Cobertura"
        protected void CarregarItemCobertura()
        {
            List<SEG_ITEM_COB> _list = new List<SEG_ITEM_COB>();
            _list = segController.ObterItemCoberturaSeguro(Convert.ToInt32(hidCodigoSeguro.Value));
            if (_list != null)
            {
                gvTipoCobertura.DataSource = _list;
                gvTipoCobertura.DataBind();
            }
        }
        protected void gvTipoCobertura_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SEG_ITEM_COB _itemCob = e.Row.DataItem as SEG_ITEM_COB;

                    if (_itemCob != null)
                    {
                        Literal _litTipoCobertura = e.Row.FindControl("litTipoCobertura") as Literal;
                        if (_litTipoCobertura != null)
                            _litTipoCobertura.Text = _itemCob.SEG_TIPO_COBERTURA1.DESCRICAO;

                        Literal _litValorCobertura = e.Row.FindControl("litValorCobertura") as Literal;
                        if (_litValorCobertura != null)
                            _litValorCobertura.Text = "R$ " + _itemCob.VALOR.ToString("###,###,##0.00");

                        ImageButton _btExcluirTipoCobertura = e.Row.FindControl("btExcluirTipoCobertura") as ImageButton;
                        if (_btExcluirTipoCobertura != null)
                            _btExcluirTipoCobertura.CommandArgument = _itemCob.CODIGO.ToString();
                    }
                }
            }
        }
        protected void btExcluirTipoCobertura_Click(object sender, EventArgs e)
        {
            ImageButton _btExcluir = (ImageButton)sender;
            if (_btExcluir != null)
            {
                try
                {
                    string codigoItemCob = _btExcluir.CommandArgument.ToString();

                    segController.ExcluirItemCobertura(Convert.ToInt32(codigoItemCob));
                    CarregarItemCobertura();
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }
        protected void btIncluirTipoCobertura_Click(object sender, EventArgs e)
        {
            int tipoCobertura = 0;
            try
            {
                labErro.Text = "";

                if (ddlTipoCobertura.SelectedValue == "0")
                {
                    labErro.Text = "Selecione o Tipo de Cobertura.";
                    return;
                }

                if (txtValorCobertura.Text.Trim() == "")
                {
                    labErro.Text = "Informe o Valor da Cobertura.";
                    return;
                }

                tipoCobertura = Convert.ToInt32(ddlTipoCobertura.SelectedValue);

                SEG_ITEM_COB itemCob = new SEG_ITEM_COB();
                itemCob.SEG_SEGURO = Convert.ToInt32(hidCodigoSeguro.Value);
                itemCob.SEG_TIPO_COBERTURA = tipoCobertura;
                itemCob.VALOR = Convert.ToDecimal(txtValorCobertura.Text);

                segController.InserirItemCobertura(itemCob);

                ddlTipoCobertura.SelectedValue = "0";
                txtValorCobertura.Text = "";

                CarregarItemCobertura();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        protected void btIncluirItemCobertura_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                string item = "";

                if (txtItem.Text.Trim() == "")
                {
                    labErro.Text = "Informe a descrição do Item.";
                    return;
                }

                if (gvTipoCobertura.Rows.Count <= 0)
                {
                    labErro.Text = "Informe pelo menos um Tipo de Cobertura ";
                    return;
                }

                item = txtItem.Text.Trim().ToUpper();

                int codigoItem = 0;
                SEG_SEGURO_ITEM _item = new SEG_SEGURO_ITEM();
                _item.ITEM = item;
                _item.SEG_SEGURO = Convert.ToInt32(hidCodigoSeguro.Value);
                codigoItem = segController.InserirSeguroItem(_item);

                List<SEG_ITEM_COB> _list = new List<SEG_ITEM_COB>();
                _list = segController.ObterItemCoberturaSeguro(Convert.ToInt32(hidCodigoSeguro.Value));

                SEG_SEGURO_ITEM_COB _itemCobertura = null;
                foreach (SEG_ITEM_COB i in _list)
                {
                    _itemCobertura = new SEG_SEGURO_ITEM_COB();
                    _itemCobertura.SEG_SEGURO_ITEM = codigoItem;
                    _itemCobertura.SEG_ITEM_COB = i.CODIGO;
                    segController.InserirSeguroItemCobertura(_itemCobertura);
                }

                txtItem.Text = "";

                CarregarItem();
                CarregarItemCobertura();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion

        #region "ITEM"
        protected void CarregarItem()
        {
            List<SP_OBTER_SEGUROSResult> _list = new List<SP_OBTER_SEGUROSResult>();
            _list = segController.ObterRelSeguros(Convert.ToInt32(hidCodigoSeguro.Value), null, null, null, "", "", null, null, null, null);
            if (_list != null)
            {
                gvItemCobertura.DataSource = _list;
                gvItemCobertura.DataBind();
            }
        }
        protected void gvItemCobertura_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_SEGUROSResult _item = e.Row.DataItem as SP_OBTER_SEGUROSResult;

                    if (_item != null)
                    {
                        Literal _litItem = e.Row.FindControl("litItem") as Literal;
                        if (_litItem != null)
                            _litItem.Text = _item.ITEM;

                        Literal _litTipoCobertura = e.Row.FindControl("litTipoCobertura") as Literal;
                        if (_litTipoCobertura != null)
                            _litTipoCobertura.Text = _item.COBERTURA;

                        ImageButton _btExcluirItem = e.Row.FindControl("btExcluirItem") as ImageButton;
                        if (_btExcluirItem != null)
                            _btExcluirItem.CommandArgument = _item.SEG_SEGURO_ITEM.ToString();
                    }
                }
            }
        }
        protected void btExcluirItem_Click(object sender, EventArgs e)
        {
            ImageButton _btExcluir = (ImageButton)sender;
            if (_btExcluir != null)
            {
                try
                {
                    string codigoItem = _btExcluir.CommandArgument.ToString();

                    segController.ExcluirItemCoberturaSP(Convert.ToInt32(codigoItem));

                    CarregarItem();
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }


        }
        #endregion
    }
}
