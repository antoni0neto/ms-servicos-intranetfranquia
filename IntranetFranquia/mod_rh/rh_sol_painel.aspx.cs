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
    public partial class rh_sol_painel : System.Web.UI.Page
    {
        RHController rhController = new RHController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarFilial();
                CarregarStatus();
                CarregarTipoSolicitacao();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_SOL_PAINELResult> ObterPainelSolicitacao(USUARIO usuario)
        {
            List<SP_OBTER_SOL_PAINELResult> painelSol = new List<SP_OBTER_SOL_PAINELResult>();

            int? codigoTipoSolicitacao = null;
            int? codigoStatus = null;

            if (ddlTipoSolicitacao.SelectedValue.Trim() != "" && ddlTipoSolicitacao.SelectedValue.Trim() != "0")
                codigoTipoSolicitacao = Convert.ToInt32(ddlTipoSolicitacao.SelectedValue.Trim());

            if (ddlStatus.SelectedValue.Trim() != "" && ddlStatus.SelectedValue.Trim() != "0")
                codigoStatus = Convert.ToInt32(ddlStatus.SelectedValue.Trim());

            //OBTER TUDO
            painelSol = rhController.ObterPainelSolicitacao(ddlFilial.SelectedValue.Trim(), codigoTipoSolicitacao, codigoStatus);

            //FILTRAR POR PERMISSAO EM LOJA
            List<USUARIOLOJA> lojaUsuario = new BaseController().BuscaUsuarioLoja(usuario);
            painelSol = painelSol.Where(p => lojaUsuario.Select(x => x.CODIGO_LOJA).Contains(p.CODIGO_FILIAL)).ToList();

            if (txtNumeroSolicitacao.Text.Trim() != "")
                painelSol = painelSol.Where(p => Convert.ToInt32(p.NUMERO_SOLICITACAO) == Convert.ToInt32(txtNumeroSolicitacao.Text.Trim())).ToList();

            return painelSol;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                USUARIO usuario = new USUARIO();
                usuario = (USUARIO)Session["USUARIO"];

                var painelSol = ObterPainelSolicitacao(usuario);

                gvPainel.DataSource = painelSol;
                gvPainel.DataBind();

                if (painelSol == null || painelSol.Count() <= 0)
                    labErro.Text = "Nenhuma Solicitação Encontrada.";

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvPainel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_SOL_PAINELResult _painelSol = e.Row.DataItem as SP_OBTER_SOL_PAINELResult;

                    if (_painelSol != null)
                    {
                        ImageButton _btAbrir = e.Row.FindControl("btAbrir") as ImageButton;
                        if (_btAbrir != null)
                            _btAbrir.CommandArgument = _painelSol.NUMERO_SOLICITACAO.ToString() + "|" + _painelSol.CODIGO_TIPO_SOL.ToString();
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
                    string codigoSolicitacao = "0";
                    string tipoSolicitacao = "";
                    string[] commandArgument = { "", "" };

                    commandArgument = _btAbrir.CommandArgument.ToString().Split('|');

                    codigoSolicitacao = commandArgument[0];
                    tipoSolicitacao = commandArgument[1];

                    if (tipoSolicitacao == "1") // TRANFERENCIA TEMPORARIA DE VENDEDOR
                        Response.Redirect("rh_sol_transf_temp_vendedor.aspx?t=2&s=" + codigoSolicitacao.ToString());
                    else if (tipoSolicitacao == "2") // TRANFERENCIA DE VENDEDOR
                        Response.Redirect("rh_sol_transferencia_vendedor.aspx?t=2&s=" + codigoSolicitacao.ToString());
                    else if (tipoSolicitacao == "3") // ADMISSÃO
                        Response.Redirect("rh_sol_admissao.aspx?t=2&s=" + codigoSolicitacao.ToString());
                    else if (tipoSolicitacao == "4") // DEMISSÃO
                        Response.Redirect("rh_sol_demissao.aspx?t=2&s=" + codigoSolicitacao.ToString());
                    else if (tipoSolicitacao == "5") // FÉRIAS
                        Response.Redirect("rh_sol_ferias.aspx?t=2&s=" + codigoSolicitacao.ToString());
                    else if (tipoSolicitacao == "6") // ALTERACAO CARGO/ SALARIO/ PERIODO DE TRABALHO
                        Response.Redirect("rh_sol_alteracao_cargo_sal_periodo.aspx?t=2&s=" + codigoSolicitacao.ToString());
                    else if (tipoSolicitacao == "7") // ALTERACAO DE VENDA
                        Response.Redirect("rh_sol_alteracao_venda.aspx?t=2&s=" + codigoSolicitacao.ToString());


                    labErro.Text = "SOLICITAÇÃO NÃO ENCONTRADA. ENTRE EM CONTATO COM TI.";
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
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
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                        ddlFilial.Enabled = false;
                    }
                }
            }
        }
        private void CarregarStatus()
        {
            var status = rhController.ObterStatusSolicitacao();

            status.Insert(0, new RH_SOL_STATUS { CODIGO = 0, DESCRICAO = "" });
            ddlStatus.DataSource = status;
            ddlStatus.DataBind();
            ddlStatus.SelectedValue = "4";
        }
        private void CarregarTipoSolicitacao()
        {
            var tipoSolicitacao = rhController.ObterTipoSolicitacao();

            tipoSolicitacao.Insert(0, new RH_SOL_TIPO { CODIGO = 0, DESCRICAO = "" });
            ddlTipoSolicitacao.DataSource = tipoSolicitacao;
            ddlTipoSolicitacao.DataBind();
        }
        #endregion
    }
}
