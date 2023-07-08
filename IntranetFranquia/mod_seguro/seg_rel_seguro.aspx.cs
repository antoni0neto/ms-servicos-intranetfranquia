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
    public partial class seg_rel_seguro : System.Web.UI.Page
    {
        SeguroController segController = new SeguroController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtVencimentoIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtVencimentoFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                CarregarItem();
                CarregarEmpresa();
                CarregarTipoSeguro();
                CarregarSeguradora();
                CarregarCorretor();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_SEGUROSResult> ObterSeguros()
        {
            List<SP_OBTER_SEGUROSResult> lstSeguros = new List<SP_OBTER_SEGUROSResult>();

            int? seguroItem = null;
            int? empresa = null;
            int? tipoSeguro = null;
            string apolice = "";
            string proposta = "";
            int? seguradora = null;
            int? corretor = null;
            DateTime? vencimentoIni = null;
            DateTime? vencimentoFim = null;

            if (ddlItem.SelectedValue != "0")
                seguroItem = Convert.ToInt32(ddlItem.SelectedValue);

            if (ddlEmpresa.SelectedValue != "0")
                empresa = Convert.ToInt32(ddlEmpresa.SelectedValue);

            if (ddlTipoSeguro.SelectedValue != "0")
                tipoSeguro = Convert.ToInt32(ddlTipoSeguro.SelectedValue);

            apolice = txtApoliceProposta.Text.Trim();

            if (ddlSeguradora.SelectedValue != "0")
                seguradora = Convert.ToInt32(ddlSeguradora.SelectedValue);

            if (ddlCorretor.SelectedValue != "0")
                corretor = Convert.ToInt32(ddlCorretor.SelectedValue);

            if (txtVencimentoIni.Text.Trim() != "")
                vencimentoIni = Convert.ToDateTime(txtVencimentoIni.Text.Trim());

            if (txtVencimentoFim.Text.Trim() != "")
                vencimentoFim = Convert.ToDateTime(txtVencimentoFim.Text.Trim());

            lstSeguros = segController.ObterRelSeguros(null, seguroItem, empresa, tipoSeguro, apolice, proposta, seguradora, corretor, vencimentoIni, vencimentoFim).Where(p => p.STATUS == 'A').OrderBy(p => p.DATA_VENCIMENTO).ToList();

            return lstSeguros;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var lstSeguros = ObterSeguros();

                gvSeguro.DataSource = lstSeguros;
                gvSeguro.DataBind();

                if (lstSeguros == null || lstSeguros.Count() <= 0)
                    labErro.Text = "Nenhuma Seguro Encontrado.";

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvSeguro_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_SEGUROSResult _seguro = e.Row.DataItem as SP_OBTER_SEGUROSResult;

                    if (_seguro != null)
                    {
                        Literal _litVencimento = e.Row.FindControl("litVencimento") as Literal;
                        if (_litVencimento != null)
                            _litVencimento.Text = Convert.ToDateTime(_seguro.DATA_VENCIMENTO).ToString("dd/MM/yyyy");

                        Literal _litCobertura = e.Row.FindControl("litCobertura") as Literal;
                        if (_litCobertura != null)
                        { _litCobertura.Text = _seguro.COBERTURA; }

                        Literal _litValor = e.Row.FindControl("litValor") as Literal;
                        if (_litValor != null)
                            _litValor.Text = "R$ " + Convert.ToDecimal(_seguro.VALOR_PREMIO).ToString("###,###,###,##0.00");

                        ImageButton _btAbrir = e.Row.FindControl("btAbrir") as ImageButton;
                        if (_btAbrir != null)
                            _btAbrir.CommandArgument = _seguro.CODIGO_SEGURO.ToString();
                    }
                }
            }
        }
        protected void btAbrir_Click(object sender, EventArgs e)
        {
            ImageButton _btAbrir = (ImageButton)sender;
            if (_btAbrir != null)
            {
                labErro.Text = "";

                try
                {
                    string codigoSeguro = _btAbrir.CommandArgument.ToString();

                    Response.Redirect("seg_seguro_cad.aspx?s=" + codigoSeguro, "_blank", "");

                    //labErro.Text = "SEGURO NÃO ENCONTRADO. ENTRE EM CONTATO COM TI.";
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarItem()
        {
            var item = segController.ObterSeguroItem();

            item.Insert(0, new SEG_SEGURO_ITEM { CODIGO = 0, ITEM = "" });
            ddlItem.DataSource = item;
            ddlItem.DataBind();
        }
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
        #endregion
    }
}
