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
    public partial class fisc_cad_resp_filial : System.Web.UI.Page
    {
        ContabilidadeController contController = new ContabilidadeController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarResponsavel();
                CarregarFilial();

                CarregarResponsavelFilial();

            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarResponsavel()
        {
            var respFiscal = contController.ObterResponsavelFiscal();

            respFiscal.Insert(0, new SP_OBTER_RESPONSAVEL_FISCALResult { CODIGO_USUARIO = 0, NOME_USUARIO = "Selecione" });
            ddlResponsavel.DataSource = respFiscal;
            ddlResponsavel.DataBind();
        }
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais();
            filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "0", FILIAL = "Selecione" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        #endregion

        private void CarregarResponsavelFilial()
        {
            var respFilial = contController.ObterRespFilialFiscal();

            gvRespFilial.DataSource = respFilial;
            gvRespFilial.DataBind();
        }


        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {

            labErro.Text = "";
            if (ddlResponsavel.SelectedValue == "0")
            {
                labErro.Text = "Selecione o Responsável.";
                return;
            }

            if (ddlFilial.SelectedValue == "0")
            {
                labErro.Text = "Selecione a Filial.";
                return;
            }

            try
            {

                var respFilialUsu = contController.ObterRespFilialFiscal(ddlFilial.SelectedValue, Convert.ToInt32(ddlResponsavel.SelectedValue));
                if (respFilialUsu != null)
                {
                    labErro.Text = "Filial já possui Responsável.";
                    return;
                }

                CTB_FILIAL_RESPONSAVEL _novo = new CTB_FILIAL_RESPONSAVEL();
                _novo.CODIGO_FILIAL = ddlFilial.SelectedValue;
                _novo.USUARIO_RESP = Convert.ToInt32(ddlResponsavel.SelectedValue);
                _novo.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _novo.DATA_ALTERACAO = DateTime.Now;

                contController.InserirRespFilialFiscal(_novo);

                CarregarResponsavelFilial();

                ddlFilial.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
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
                        contController.ExcluirRespFilialFiscal(Convert.ToInt32(b.CommandArgument));
                        CarregarResponsavelFilial();
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = ex.Message;
                    }

                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }

        #endregion
        protected void gvRespFilial_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CTB_FILIAL_RESPONSAVEL respFilial = e.Row.DataItem as CTB_FILIAL_RESPONSAVEL;

                    if (respFilial != null)
                    {
                        Literal _litResponsavel = e.Row.FindControl("litResponsavel") as Literal;
                        if (_litResponsavel != null)
                            _litResponsavel.Text = baseController.BuscaUsuario(respFilial.USUARIO_RESP).NOME_USUARIO.ToUpper();

                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = baseController.BuscaFilialCodigo(Convert.ToInt32(respFilial.CODIGO_FILIAL)).FILIAL.Trim();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = respFilial.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
