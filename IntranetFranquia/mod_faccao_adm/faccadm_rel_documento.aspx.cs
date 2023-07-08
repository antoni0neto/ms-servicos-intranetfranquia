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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using DAL;
using System.Text;

namespace Relatorios
{
    public partial class faccadm_rel_documento : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataVigenciaIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataVigenciaFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                CarregarFaccao();
                CarregarTipoDocumento();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_FACCAO_DOCUMENTOResult> ObterDocumentos()
        {
            List<SP_OBTER_FACCAO_DOCUMENTOResult> listaFaccaoDocumento = new List<SP_OBTER_FACCAO_DOCUMENTOResult>();
            int? codigoTipoDocumento = null;
            DateTime? vigenciaIni = null;
            DateTime? vigenciaFim = null;
            char? docFaltante = null;

            if (ddlTipoDocumento.SelectedValue != "0")
                codigoTipoDocumento = Convert.ToInt32(ddlTipoDocumento.SelectedValue);

            if (txtDataVigenciaIni.Text.Trim() != "")
                vigenciaIni = Convert.ToDateTime(txtDataVigenciaIni.Text.Trim());

            if (txtDataVigenciaFim.Text.Trim() != "")
                vigenciaFim = Convert.ToDateTime(txtDataVigenciaFim.Text.Trim());

            if (ddlStatusDocumento.SelectedValue != "")
                docFaltante = Convert.ToChar(ddlStatusDocumento.SelectedValue);

            listaFaccaoDocumento = faccController.ObterFaccaoDocumento(ddlFaccao.SelectedValue, codigoTipoDocumento, vigenciaIni, vigenciaFim, docFaltante);

            return listaFaccaoDocumento;
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gvDocumento.DataSource = ObterDocumentos();
                gvDocumento.DataBind();

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvDocumento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_DOCUMENTOResult _faccaoDocumento = e.Row.DataItem as SP_OBTER_FACCAO_DOCUMENTOResult;

                    if (_faccaoDocumento != null)
                    {
                        if (_faccaoDocumento.DATA_VIGENCIA_FIM != null)
                        {
                            if (_faccaoDocumento.DATA_VIGENCIA_FIM.Value.Date < DateTime.Now.Date)
                                e.Row.ForeColor = Color.Red;
                        }

                        Literal _litVigenciaIni = e.Row.FindControl("litVigenciaIni") as Literal;
                        if (_litVigenciaIni != null)
                            _litVigenciaIni.Text = (_faccaoDocumento.DATA_VIGENCIA_INI == null) ? "-" : Convert.ToDateTime(_faccaoDocumento.DATA_VIGENCIA_INI).ToString("dd/MM/yyyy");

                        Literal _litVigenciaFim = e.Row.FindControl("litVigenciaFim") as Literal;
                        if (_litVigenciaFim != null)
                            _litVigenciaFim.Text = (_faccaoDocumento.DATA_VIGENCIA_FIM == null) ? "-" : Convert.ToDateTime(_faccaoDocumento.DATA_VIGENCIA_FIM).ToString("dd/MM/yyyy");

                        Literal _litStatusDocumento = e.Row.FindControl("litStatusDocumento") as Literal;
                        if (_litStatusDocumento != null)
                        {
                            string statusDocumento = "";
                            if (_faccaoDocumento.DOC_FALTANTE == "F")
                                statusDocumento = "FALTANTE";
                            else if (_faccaoDocumento.DOC_FALTANTE == "V")
                                statusDocumento = "VENCIDO";
                            else
                                statusDocumento = "ENTREGUE";

                            _litStatusDocumento.Text = statusDocumento;
                        }

                        ImageButton _ibtPesquisar = e.Row.FindControl("ibtPesquisar") as ImageButton;
                        ImageButton _ibtImprimir = e.Row.FindControl("ibtImprimir") as ImageButton;
                        _ibtPesquisar.Visible = false;
                        _ibtImprimir.Visible = false;
                        if (_faccaoDocumento.COD_FACCAO_DOCUMENTO != null && _faccaoDocumento.DOCUMENTO != null)
                        {
                            _ibtPesquisar.CommandArgument = _faccaoDocumento.COD_FACCAO_DOCUMENTO.ToString();
                            _ibtImprimir.CommandArgument = _faccaoDocumento.COD_FACCAO_DOCUMENTO.ToString();
                            _ibtPesquisar.Visible = true;
                            _ibtImprimir.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvDocumento_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_FACCAO_DOCUMENTOResult> listDocumentos = ObterDocumentos();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            listDocumentos = listDocumentos.OrderBy(e.SortExpression + sortDirection);
            gvDocumento.DataSource = listDocumentos;
            gvDocumento.DataBind();

        }

        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton b = (ImageButton)sender;
                if (b != null)
                {
                    int codigoFaccaoDocumento = 0;
                    if (b.CommandArgument != "")
                    {
                        codigoFaccaoDocumento = Convert.ToInt32(b.CommandArgument);
                        var faccaoDocumento = faccController.ObterFaccaoDocumento(codigoFaccaoDocumento);
                        if (faccaoDocumento != null)
                        {
                            Response.Redirect(("faccadm_manutencao.aspx?d=" + codigoFaccaoDocumento.ToString()), "_blank", "");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton b = (ImageButton)sender;
                if (b != null)
                {
                    int codigoFaccaoDocumento = 0;
                    if (b.CommandArgument != "")
                    {
                        codigoFaccaoDocumento = Convert.ToInt32(b.CommandArgument);
                        var faccaoDocumento = faccController.ObterFaccaoDocumento(codigoFaccaoDocumento);
                        if (faccaoDocumento != null)
                            Response.Redirect(faccaoDocumento.DOCUMENTO, "_blank", "");
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFaccao()
        {
            var faccaoCadastro = faccController.ObterFaccaoCadastro("", "");

            faccaoCadastro.Insert(0, new SP_OBTER_FACCAO_CADASTROResult { NOME_CLIFOR = "" });
            ddlFaccao.DataSource = faccaoCadastro;
            ddlFaccao.DataBind();
        }
        private void CarregarTipoDocumento()
        {
            var tipoDocumento = faccController.ObterTipoDocumento().OrderBy(p => p.DOCUMENTO).ToList();

            tipoDocumento.Insert(0, new FACC_TIPO_DOCUMENTO { CODIGO = 0, DOCUMENTO = "" });
            ddlTipoDocumento.DataSource = tipoDocumento;
            ddlTipoDocumento.DataBind();
        }

        #endregion
    }
}
