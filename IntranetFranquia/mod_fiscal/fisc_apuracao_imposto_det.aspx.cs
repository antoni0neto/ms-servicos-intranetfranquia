using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;
using Relatorios.mod_desenvolvimento.modelo_pocket;

namespace Relatorios
{
    public partial class fisc_apuracao_imposto_det : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ContabilidadeController contabilController = new ContabilidadeController();

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["e"] == null || Request.QueryString["e"] == "" ||
                    Request.QueryString["i"] == null || Request.QueryString["i"] == "" ||
                    Session["USUARIO"] == null)
                {
                    Response.Write("SESSÃO ENCERRADA.");
                    Response.End();
                }

                string codigoApuracaoEmpresa = "";
                string tipoImposto = "";
                codigoApuracaoEmpresa = Request.QueryString["e"].ToString();
                tipoImposto = Request.QueryString["i"].ToString();

                hidCTBApuracaoEmpresa.Value = codigoApuracaoEmpresa;
                hidTipoImposto.Value = tipoImposto;

                //PREENCHER CABECALHO
                var empresaApuracao = contabilController.ObterApuracaoEmpresa(Convert.ToInt32(codigoApuracaoEmpresa));
                if (empresaApuracao != null)
                {
                    txtEmpresa.Text = empresaApuracao.EMPRESA1.NOME;
                    txtCompetencia.Text = empresaApuracao.COMPETENCIA.ToString("dd/MM/yyyy");
                    txtImposto.Text = tipoImposto;
                }

                CarregarCalculoApuracao();
            }
        }

        private void CarregarCalculoApuracao()
        {
            HtmlGenericControl divApuracaoImp;
            try
            {
                //Descarregar hMTL na DIV
                divApuracaoImp = new HtmlGenericControl();
                divApuracaoImp.InnerHtml = MontarApuracaoImposto(hidCTBApuracaoEmpresa.Value, hidTipoImposto.Value, true, false).ToString();

                pnlApuracaoImp.Controls.Add(divApuracaoImp);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                //IMPRESSAO
                StreamWriter wr = null;
                string nomeArquivo = "APU_IMPOSTO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarApuracaoImposto(hidCTBApuracaoEmpresa.Value, hidTipoImposto.Value, false, true).ToString());
                wr.Flush();
                wr.Close();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirPocket('" + nomeArquivo + "')", true);

                CarregarCalculoApuracao();

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }
        protected void btDARF_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                //IMPRESSAO
                StreamWriter wr = null;
                string nomeArquivo = "DARF_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarDARF(hidCTBApuracaoEmpresa.Value, hidTipoImposto.Value, true).ToString());
                wr.Flush();
                wr.Close();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirPocket('" + nomeArquivo + "')", true);

                CarregarCalculoApuracao();

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        #region "RELATORIO"
        private string MontarApuracaoImposto(string codigoApuracaoEmpresa, string tipoImposto, bool calcula, bool print)
        {
            RelatorioApuracaoImposto relApuracaoImposto = new RelatorioApuracaoImposto();
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            return relApuracaoImposto.MontarApuracaoImposto(Convert.ToInt32(codigoApuracaoEmpresa), tipoImposto, codigoUsuario, calcula, print).ToString();
        }
        private string MontarDARF(string codigoApuracaoEmpresa, string tipoImposto, bool print)
        {
            RelatorioApuracaoImposto relApuracaoImposto = new RelatorioApuracaoImposto();
            return relApuracaoImposto.MontarDARF(Convert.ToInt32(codigoApuracaoEmpresa), tipoImposto, print).ToString();
        }
        #endregion


    }
}
