using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class fin_deposito_compimagem : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!IsPostBack)
            {
                CarregarFiliais();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private void CarregarFiliais()
        {
            var filial = baseController.BuscaFiliais();
            var filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();

            filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
            ddlFilial.DataSource = filial;
            ddlFilial.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            CarregarComprovantes();
        }

        private void CarregarComprovantes()
        {
            var comprovantes = baseController.BuscaComprovantes();

            if (ddlFilial.SelectedValue != "")
                comprovantes = comprovantes.Where(p => p.CODIGO_LOJA == Convert.ToInt32(ddlFilial.SelectedValue)).ToList();

            if (txtDataInicial.Text != "")
                comprovantes = comprovantes.Where(p => p.DATA_DIGITADA >= Convert.ToDateTime(txtDataInicial.Text)).ToList();

            if (txtDataFinal.Text != "")
                comprovantes = comprovantes.Where(p => p.DATA_DIGITADA <= Convert.ToDateTime(txtDataFinal.Text)).ToList();

            gvComprovante.DataSource = comprovantes;
            gvComprovante.DataBind();
        }
        protected void gvComprovante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMAGEM imagem = e.Row.DataItem as IMAGEM;

            if (imagem != null)
            {
                Label labFilial = e.Row.FindControl("labFilial") as Label;
                labFilial.Text = baseController.BuscaFilialCodigo(imagem.CODIGO_LOJA).FILIAL;

                Label labDataDigitada = e.Row.FindControl("labDataDigitada") as Label;
                labDataDigitada.Text = (imagem.DATA_DIGITADA == null) ? "-" : Convert.ToDateTime(imagem.DATA_DIGITADA).ToString("dd/MM/yyyy");

                HyperLink hlAbrirComp = e.Row.FindControl("hlAbrirComp") as HyperLink;
                hlAbrirComp.NavigateUrl = string.Format("~/Upload/{0}", imagem.LOCAL_IMAGEM);
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
    }
}