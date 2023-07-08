using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;

namespace Relatorios
{
    public partial class gerloja_visita_supervisor_add : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Session["USUARIO"] == null)
                    return;

                var ano = Request.QueryString["an"].ToString();
                var mes = Request.QueryString["me"].ToString();
                var dia = Request.QueryString["di"].ToString();
                var sup = Request.QueryString["sup"].ToString();

                hidCodigoUsuario.Value = sup.ToString();
                hidAno.Value = ano.ToString();
                hidMes.Value = mes.ToString();
                hidDia.Value = dia.ToString();

                txtData.Text = ((dia.Length == 1) ? ("0" + dia) : dia) + "/" + ((mes.Length == 1) ? ("0" + mes) : mes) + "/" + ano;

                CarregarFilial();
                CarregarAgendaDia();

                //Controle de Controles
                btCancelar.Visible = false;
                pnlErro.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarFilial()
        {
            var filial = baseController.ObterFiliaisLojaIntranet();

            filial = filial.OrderBy(p => p.FILIAL).ToList();
            filial.Insert(0, new FILIAI1 { COD_FILIAL = "", FILIAL = "Selecione" });
            filial.Insert(1, new FILIAI1 { COD_FILIAL = "000000", FILIAL = "ESCRITORIO" });
            filial.Insert(2, new FILIAI1 { COD_FILIAL = "900000", FILIAL = "EXTRA" });
            ddlFilial.DataSource = filial;
            ddlFilial.DataBind();

        }
        private void CarregarAgendaDia()
        {
            try
            {
                var agendaSemana = lojaController.ObterAgendaVisitaPorUsuario(Convert.ToInt32(hidCodigoUsuario.Value), Convert.ToInt32(hidAno.Value), Convert.ToInt32(hidMes.Value), Convert.ToInt32(hidDia.Value));

                gvAgenda.DataSource = agendaSemana;
                gvAgenda.DataBind();

                ddlCor.Enabled = true;
                if (agendaSemana.Count() > 0)
                {
                    ddlCor.SelectedValue = agendaSemana[0].COR;
                    ddlCor.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (AgendaSemana Add Filial): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                pnlErro.Visible = true;
                pnlErro.Height = Unit.Pixel(100);
            }
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labFilial.ForeColor = Color.Black;
            labData.ForeColor = Color.Black;
            labCor.ForeColor = Color.Black;
            btCancelar.Visible = false;

            txtDescritivo.Text = "";
            ddlFilial.SelectedValue = "";
            pnlErro.Visible = false;

            CarregarAgendaDia();
        }

        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool inc = false;

            pnlErro.Visible = false;
            if (txtData.Text.Trim() == "")
            {
                labErro.Text = "Informa a Data da Visita.";
                pnlErro.Visible = true;
                return;
            }
            if (ddlCor.SelectedValue == "")
            {
                labErro.Text = "Selecione a Cor do Dia.";
                pnlErro.Visible = true;
                return;
            }
            if (ddlFilial.SelectedValue == "")
            {
                labErro.Text = "Selecione a Filial.";
                pnlErro.Visible = true;
                return;
            }

            var usuarioComFilial = lojaController.ObterAgendaVisitaPorUsuarioFilial(Convert.ToInt32(hidCodigoUsuario.Value), Convert.ToInt32(hidAno.Value), Convert.ToInt32(hidMes.Value), Convert.ToInt32(hidDia.Value), ddlFilial.SelectedValue.Trim());
            if (usuarioComFilial != null)
            {
                labErro.Text = "Filial já inserida na agenda deste dia.";
                pnlErro.Visible = true;
                return;
            }

            try
            {
                LOJA_AGENDA_VISITA _novo = new LOJA_AGENDA_VISITA();

                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                {
                    _novo.CODIGO = Convert.ToInt32(hidCodigo.Value);
                    _novo.DATA_BAIXA = DateTime.Now;
                }
                else
                {
                    inc = true;
                }

                _novo.USUARIO = Convert.ToInt32(hidCodigoUsuario.Value);
                _novo.CODIGO_FILIAL = ddlFilial.SelectedValue.Trim();
                _novo.DATA_VISITA = Convert.ToDateTime(txtData.Text);
                _novo.OBS = txtDescritivo.Text.Trim();
                _novo.COR = ddlCor.SelectedValue;

                if (inc)
                    lojaController.InserirAgendaVisita(_novo);
                else
                    lojaController.AtualizarAgendaVisita(_novo);

                RecarregarTela();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                pnlErro.Visible = true;
                pnlErro.Height = Unit.Pixel(100);
            }
        }
        protected void btAlterar_Click(object sender, EventArgs e)
        {
            pnlErro.Visible = false;

            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    LOJA_AGENDA_VISITA agenda = lojaController.ObterAgendaVisitaPorCodigo(Convert.ToInt32(b.CommandArgument));
                    if (agenda != null)
                    {
                        hidCodigo.Value = agenda.CODIGO.ToString();
                        ddlFilial.SelectedValue = baseController.ObterFilialIntranet(agenda.CODIGO_FILIAL).COD_FILIAL;
                        txtDescritivo.Text = agenda.OBS;
                        labFilial.ForeColor = Color.Red;
                        txtDescritivo.ForeColor = Color.Red;
                        btCancelar.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btAlterar_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    pnlErro.Visible = true;
                    pnlErro.Height = Unit.Pixel(100);
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            pnlErro.Visible = false;
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    var codigo = Convert.ToInt32(b.CommandArgument);
                    lojaController.ExcluirAgendaVisita(codigo);

                    RecarregarTela();
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    pnlErro.Visible = true;
                    pnlErro.Height = Unit.Pixel(100);
                }
            }
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            RecarregarTela();
        }
        #endregion

        protected void gvAgenda_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    LOJA_AGENDA_VISITA agenda = e.Row.DataItem as LOJA_AGENDA_VISITA;

                    if (agenda != null)
                    {

                        Literal litFilial = e.Row.FindControl("litFilial") as Literal;
                        litFilial.Text = baseController.ObterFilialIntranet(agenda.CODIGO_FILIAL).FILIAL.Trim();

                        Literal litDataVisita = e.Row.FindControl("litDataVisita") as Literal;
                        litDataVisita.Text = agenda.DATA_VISITA.ToString("dd/MM/yyyy");

                        Literal litObs = e.Row.FindControl("litObs") as Literal;
                        litObs.Text = agenda.OBS;

                        Button btAlterar = e.Row.FindControl("btAlterar") as Button;
                        btAlterar.CommandArgument = agenda.CODIGO.ToString();

                        Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                        btExcluir.CommandArgument = agenda.CODIGO.ToString();

                    }
                }
            }
        }
    }
}
