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
    public partial class rh_ponto_batida_abrir : System.Web.UI.Page
    {
        RHController rhController = new RHController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataBatida.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate()-1) });});", true);

            if (!Page.IsPostBack)
            {
                CarregarFilial();

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btAbrirBatida.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAbrirBatida, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = new BaseController().BuscaFiliais_Intermediario(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA" || p.TIPO_FILIAL.Trim() == "INATIVA").ToList();

                var filialDePara = new BaseController().BuscaFilialDePara();
                if (lstFilial.Count > 0)
                {
                    lstFilial = lstFilial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                        ddlFilial.SelectedIndex = 1;
                }
            }
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue.Trim() == "")
            {
                labFilial.ForeColor = _notOK;
                retorno = false;
            }

            labDataBatida.ForeColor = _OK;
            if (txtDataBatida.Text.Trim() == "")
            {
                labDataBatida.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarData(string strHora)
        {
            try
            {
                if (strHora.Length == 5)
                    strHora = "1900-01-01 " + strHora;

                Convert.ToDateTime(strHora);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region "INCLUSAO"

        #endregion

        protected void btAbrirBatida_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlFilial.SelectedValue.Trim() == "")
                {
                    labErro.Text = "Selecione a Filial.";
                    return;
                }

                if (txtDataBatida.Text.Trim() == "")
                {
                    labErro.Text = "Informe a Data Referência.";
                    return;
                }

                if (!ValidarData(txtDataBatida.Text.Trim()))
                {
                    labErro.Text = "Informe a Data Referência Válida.";
                    return;
                }

                var lstBatida = rhController.ObterPontoBatida(ddlFilial.SelectedValue, Convert.ToDateTime(txtDataBatida.Text));

                if (lstBatida == null || lstBatida.Count() <= 0)
                {
                    labErro.Text = "Dia da Batida não encontrado. Verifique se o dia já foi gerado pela Loja.";
                    return;
                }

                RH_PONTO_BATIDA batida = null;
                foreach (var b in lstBatida)
                {
                    batida = rhController.ObterPontoBatida(b.CODIGO);
                    if (batida != null)
                    {
                        batida.DATA_INCLUSAO = DateTime.Now;
                        batida.USUARIO_ABERTURA_DIA = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        batida.DATA_ABERTURA_DIA = DateTime.Now;
                        rhController.AtualizarPontoBatida(batida);
                    }
                }

                labErro.Text = "Dia da Batida aberto com Sucesso. A Filial <b>" + ddlFilial.SelectedItem.Text.Trim() + "</b> tem até às <b>23h59</b> de hoje para preencher a Batida.";

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }


    }
}
