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
    public partial class admloj_cad_vitrine : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "admloj_menu.aspx";

                CarregarFilial();
                CarregarSemanas();

            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            var usuario = (USUARIO)Session["USUARIO"];

            if (usuario.CODIGO_PERFIL == 3 || usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_PERFIL == 51 || usuario.CODIGO_PERFIL == 50)
                filial = baseController.BuscaFiliais();
            else
                filial = baseController.BuscaFiliais(usuario);

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();

                ddlFilial.Enabled = true;
                if (filial.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }
        }
        private void CarregarSemanas()
        {
            var semana = lojaController.ObterSemanaVenda();

            semana.Insert(0, new LOJA_VENDA_SEMANA { CODIGO = 0, SEMANA = "Selecione" });

            ddlSemana.DataSource = semana;
            ddlSemana.DataBind();
        }

        #endregion


        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlSemana.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Semana...";
                    return;
                }

                CarregarVitrineFilial();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private List<SP_OBTER_VITRINE_FILIALResult> ObterVitrineFilial()
        {

            var semanaSel = lojaController.ObterSemanaVenda(Convert.ToInt32(ddlSemana.SelectedValue));

            var vitrine = lojaController.ObterVitrineFilial(semanaSel.DATA_INI, semanaSel.DATA_FIM, ddlFilial.SelectedValue.Trim(), null, null);

            return vitrine;
        }
        private void CarregarVitrineFilial()
        {
            var vitrine = ObterVitrineFilial();

            gvVitrine.DataSource = vitrine;
            gvVitrine.DataBind();
        }

        protected void gvVitrine_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VITRINE_FILIALResult vitrine = e.Row.DataItem as SP_OBTER_VITRINE_FILIALResult;

                    if (vitrine != null)
                    {
                        Literal litDataFinalizado = e.Row.FindControl("litDataFinalizado") as Literal;
                        litDataFinalizado.Text = (vitrine.DATA_FINALIZADO == null) ? "-" : Convert.ToDateTime(vitrine.DATA_FINALIZADO).ToString("dd/MM/yyyy");

                        Literal litDataAnalisado = e.Row.FindControl("litDataAnalisado") as Literal;
                        litDataAnalisado.Text = (vitrine.DATA_ANALISADO == null) ? "-" : Convert.ToDateTime(vitrine.DATA_ANALISADO).ToString("dd/MM/yyyy");

                        if (vitrine.SUGESTAO == 'S')
                            e.Row.BackColor = Color.PaleGreen;

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }

        protected void btAbrir_Click(object sender, EventArgs e)
        {
            string url = "";

            try
            {
                labErro.Text = "";

                Button bt = (Button)sender;
                if (bt != null)
                {
                    GridViewRow row = (GridViewRow)bt.NamingContainer;

                    string codigoFilial = gvVitrine.DataKeys[row.RowIndex].Values[0].ToString().Trim();
                    string codigoVitrine = gvVitrine.DataKeys[row.RowIndex].Values[1].ToString().Trim();
                    string dataIni = gvVitrine.DataKeys[row.RowIndex].Values[2].ToString().Trim();
                    string dataFim = gvVitrine.DataKeys[row.RowIndex].Values[3].ToString().Trim();

                    //Abrir pop-up
                    url = "fnAbrirTelaCadastroMaior('admloj_cad_vitrine_fotomundo.aspx?codf=" + codigoFilial + "&codv=" + codigoVitrine + "&di=" + dataIni + "&df=" + dataFim + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
                }


            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }


    }
}
