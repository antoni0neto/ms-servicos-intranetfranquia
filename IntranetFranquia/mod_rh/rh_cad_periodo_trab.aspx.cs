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
    public partial class rh_cad_periodo_trab : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtData.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                //Carregar gv gvFeriado
                CarregarPeriodosTrabalho();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarPeriodosTrabalho()
        {
            List<RH_PONTO_PERIODO_TRAB> periodoTrab = new List<RH_PONTO_PERIODO_TRAB>();

            try
            {
                periodoTrab = rhController.ObterPeriodoTrabalho();
                if (periodoTrab != null)
                {
                    gvPeriodoTrab.DataSource = periodoTrab;
                    gvPeriodoTrab.DataBind();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (gvPeriodoTrab): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labDescricao.ForeColor = Color.Black;
            labSubDescricao.ForeColor = Color.Black;
            labDiaInicial.ForeColor = Color.Black;
            labDiaFinal.ForeColor = Color.Black;
            labHoraInicial.ForeColor = Color.Black;
            labHoraFinal.ForeColor = Color.Black;
            labStatus.ForeColor = Color.Black;

            btCancelar.Visible = false;
            txtDescricao.Text = "";
            txtSubDescricao.Text = "";
            ddlDiaInicial.SelectedValue = "";
            ddlDiaFinal.SelectedValue = "";
            txtHoraInicial.Text = "";
            txtHoraFinal.Text = "";
            ddlStatus.SelectedValue = "A";

            labErro.Text = "";

            CarregarPeriodosTrabalho();
        }
        private string RetornarDiaSemana(int dia)
        {
            string diaExtenso = "";

            if (dia == 1)
                diaExtenso = "Domingo";
            if (dia == 2)
                diaExtenso = "Segunda";
            if (dia == 3)
                diaExtenso = "Terça";
            if (dia == 4)
                diaExtenso = "Quarta";
            if (dia == 5)
                diaExtenso = "Quinta";
            if (dia == 6)
                diaExtenso = "Sexta";
            if (dia == 7)
                diaExtenso = "Sábado";

            return diaExtenso;
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

        #region "CRUD"
        private void Incluir(RH_PONTO_PERIODO_TRAB _periodoTrab)
        {
            rhController.InserirPeriodoTrabalho(_periodoTrab);
        }
        private void Editar(RH_PONTO_PERIODO_TRAB _periodoTrab)
        {
            rhController.AtualizarPeriodoTrabalho(_periodoTrab);
        }
        private void Excluir(int _codigo)
        {
            rhController.ExcluirPeriodoTrabalho(_codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            labErro.Text = "";
            if (txtDescricao.Text.Trim() == "")
            {
                labErro.Text = "Informe a Descrição do Período de Trabalho.";
                return;
            }

            if (txtSubDescricao.Text.Trim() == "")
            {
                labErro.Text = "Informe a Sub Descrição do Período de Trabalho.";
                return;
            }

            if (ddlDiaInicial.SelectedValue == "")
            {
                labErro.Text = "Seleciona o Dia Inicial.";
                return;
            }
            if (ddlDiaFinal.SelectedValue == "")
            {
                labErro.Text = "Seleciona o Dia Final.";
                return;
            }

            if (txtHoraInicial.Text.Trim() == "")
            {
                labErro.Text = "Informa a Hora Inicial.";
                return;
            }

            if (txtHoraFinal.Text.Trim() == "")
            {
                labErro.Text = "Informa a Hora Final.";
                return;
            }

            if (!ValidarData(txtHoraInicial.Text.Trim()))
            {
                labErro.Text = "Informa a Hora Inicial Válida.";
                return;
            }

            if (!ValidarData(txtHoraFinal.Text.Trim()))
            {
                labErro.Text = "Informa a Hora Final Válida.";
                return;
            }

            if (Convert.ToInt32(txtHoraFinal.Text.Trim().Replace(":", "")) <= Convert.ToInt32(txtHoraInicial.Text.Trim().Replace(":", "")))
            {
                labErro.Text = "Informa a Hora Final Maior que Hora Inicial.";
                return;
            }

            if (ddlStatus.SelectedValue.Trim() == "0")
            {
                labErro.Text = "Selecione o Status.";
                return;
            }

            try
            {
                _Inclusao = true;
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    _Inclusao = false;

                RH_PONTO_PERIODO_TRAB _novo = new RH_PONTO_PERIODO_TRAB();
                _novo.DESCRICAO = txtDescricao.Text.Trim().ToUpper();
                _novo.SUB_DESCRICAO = txtSubDescricao.Text.Trim().ToUpper();
                _novo.DIA_INICIAL = Convert.ToInt32(ddlDiaInicial.SelectedValue);
                _novo.DIA_FINAL = Convert.ToInt32(ddlDiaFinal.SelectedValue);
                _novo.HORA_INICIAL = Convert.ToDateTime("1900-01-01 " + txtHoraInicial.Text);
                _novo.HORA_FINAL = Convert.ToDateTime("1900-01-01 " + txtHoraFinal.Text);
                _novo.DATA_ALTERACAO = DateTime.Now;
                _novo.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _novo.STATUS = Convert.ToChar(ddlStatus.Text);

                if (_Inclusao)
                {
                    Incluir(_novo);
                }
                else
                {
                    _novo.CODIGO = Convert.ToInt32(hidCodigo.Value);
                    Editar(_novo);
                }

                RecarregarTela();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        protected void btAlterar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    RH_PONTO_PERIODO_TRAB _periodoTrab = rhController.ObterPeriodoTrabalho(Convert.ToInt32(b.CommandArgument));
                    if (_periodoTrab != null)
                    {

                        var batida = rhController.ObterPontoBatida().Where(p => p.RH_PONTO_PERIODO_TRAB == _periodoTrab.CODIGO);
                        if (batida != null && batida.Count() > 0)
                        {
                            labErro.Text = "Não é permitido alterar um Horário de Trabalho que já foi utilizado para Batida de Ponto.";
                            return;
                        }

                        hidCodigo.Value = _periodoTrab.CODIGO.ToString();
                        txtDescricao.Text = _periodoTrab.DESCRICAO.Trim().ToUpper();
                        if (_periodoTrab.SUB_DESCRICAO != null)
                            txtSubDescricao.Text = _periodoTrab.SUB_DESCRICAO.Trim().ToUpper();
                        ddlDiaInicial.SelectedValue = _periodoTrab.DIA_INICIAL.ToString();
                        ddlDiaFinal.SelectedValue = _periodoTrab.DIA_FINAL.ToString();
                        txtHoraInicial.Text = _periodoTrab.HORA_INICIAL.ToString("HH:mm");
                        txtHoraFinal.Text = _periodoTrab.HORA_FINAL.ToString("HH:mm");
                        ddlStatus.SelectedValue = _periodoTrab.STATUS.ToString();

                        labDescricao.ForeColor = Color.Red;
                        labSubDescricao.ForeColor = Color.Red;
                        labDiaInicial.ForeColor = Color.Red;
                        labDiaFinal.ForeColor = Color.Red;
                        labHoraInicial.ForeColor = Color.Red;
                        labHoraFinal.ForeColor = Color.Red;
                        labStatus.ForeColor = Color.Red;
                        btCancelar.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btAlterar_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
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
                        Excluir(Convert.ToInt32(b.CommandArgument));
                        RecarregarTela();
                    }
                    catch (Exception)
                    {
                        labErro.Text = "O Período de Trabalho não pode ser excluído. Este Período foi utilizado.";
                    }

                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            RecarregarTela();
        }
        #endregion
        protected void gvPeriodoTrab_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    RH_PONTO_PERIODO_TRAB _periodoTrab = e.Row.DataItem as RH_PONTO_PERIODO_TRAB;

                    coluna += 1;
                    if (_periodoTrab != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litDiaInicial = e.Row.FindControl("litDiaInicial") as Literal;
                        if (_litDiaInicial != null)
                            _litDiaInicial.Text = RetornarDiaSemana(_periodoTrab.DIA_INICIAL);

                        Literal _litDiaFinal = e.Row.FindControl("litDiaFinal") as Literal;
                        if (_litDiaFinal != null)
                            _litDiaFinal.Text = RetornarDiaSemana(_periodoTrab.DIA_FINAL);

                        Literal _litHoraInicial = e.Row.FindControl("litHoraInicial") as Literal;
                        if (_litHoraInicial != null)
                            _litHoraInicial.Text = _periodoTrab.HORA_INICIAL.ToString("HH:mm");

                        Literal _litHoraFinal = e.Row.FindControl("litHoraFinal") as Literal;
                        if (_litHoraFinal != null)
                            _litHoraFinal.Text = _periodoTrab.HORA_FINAL.ToString("HH:mm");

                        Literal _status = e.Row.FindControl("litStatus") as Literal;
                        if (_status != null)
                        {
                            if (_periodoTrab.STATUS == 'A')
                                _status.Text = "Ativo";
                            else
                                _status.Text = "Inativo";
                        }

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _periodoTrab.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _periodoTrab.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
