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
    public partial class rh_ponto_batida_pendente : System.Web.UI.Page
    {
        RHController rhController = new RHController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtData.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {
                CarregarFilial();
                CarregarBatidasPendentes();
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            List<FILIAI> lstFilial = new List<FILIAI>();

            lstFilial = ObterFilial();

            ddlFilial.DataSource = lstFilial;
            ddlFilial.DataBind();
        }
        private List<FILIAI> ObterFilial()
        {
            List<FILIAI> lstFilial = new List<FILIAI>();

            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                lstFilial = new BaseController().BuscaFiliais_Intermediario(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA" || p.TIPO_FILIAL.Trim() == "INATIVA").ToList();
                var filialDePara = new BaseController().BuscaFilialDePara();
                if (lstFilial.Count > 0)
                {
                    lstFilial = lstFilial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();
                }
            }

            return lstFilial;
        }
        private List<RH_PONTO_BATIDA_TIPO> ObterTipoBatida()
        {
            var tipoBatida = rhController.ObterBatidaTipo().Where(p => p.STATUS == 'A').ToList();
            return tipoBatida;
        }
        private List<SP_OBTER_FUNCIONARIOResult> ObterFuncionario(string filial)
        {
            var funcionario = rhController.ObterFuncionario(null, "", filial, "", "").Where(p => p.BATE_PONTO == 'S').OrderBy(o => o.NOME).ToList();
            funcionario.Insert(0, new SP_OBTER_FUNCIONARIOResult { CODIGO = 0, NOME = "Selecione" });
            return funcionario;
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
        private bool ValidarPeriodo(string e1, string s1, string e2, string s2)
        {
            if (Convert.ToInt32(s1.Replace(":", "")) < Convert.ToInt32(e1.Replace(":", "")))
                return false;

            if (Convert.ToInt32(s2.Replace(":", "")) < Convert.ToInt32(e2.Replace(":", "")))
                return false;

            if (Convert.ToInt32(e2.Replace(":", "")) < Convert.ToInt32(s1.Replace(":", "")))
                return false;

            if (Convert.ToInt32(s2.Replace(":", "")) < Convert.ToInt32(s1.Replace(":", "")))
                return false;

            if (Convert.ToInt32(s2.Replace(":", "")) < Convert.ToInt32(e1.Replace(":", "")))
                return false;

            return true;
        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroGrid.Text = "";

                if (txtData.Text.Trim() != "" && !ValidarData(txtData.Text.Trim()))
                {
                    labErro.Text = "Informe a Data Referência Válida.";
                    return;
                }

                CarregarBatidasPendentes();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void CarregarBatidasPendentes()
        {
            List<RH_PONTO_BATIDA> _pontoBatida = new List<RH_PONTO_BATIDA>();

            _pontoBatida = rhController.ObterPontoBatida().Where(p => (p.STATUS == 'N' || p.RH_PONTO_BATIDA_TIPO == 99) && p.STATUS != 'E' && DateTime.Today > p.DATA_INCLUSAO.Date.AddHours(23).AddMinutes(59)).ToList();

            if (ddlFilial.SelectedValue.Trim() != "")
                _pontoBatida = _pontoBatida.Where(p => p.CODIGO_FILIAL.Trim() == ddlFilial.SelectedValue.Trim()).ToList();

            if (txtData.Text.Trim() != "")
                _pontoBatida = _pontoBatida.Where(p => p.DATA == Convert.ToDateTime(txtData.Text.Trim())).ToList();

            gvBatidaPendente.DataSource = _pontoBatida.OrderBy(p => p.CODIGO_FILIAL).ThenBy(p => (p.RH_FUNCIONARIO == null) ? p.FUNCIONARIO_NOME : p.RH_FUNCIONARIO1.NOME).ThenBy(p => p.DATA);
            gvBatidaPendente.DataBind();

        }
        private string CalcularPeriodo(DateTime? e1, DateTime? s1, DateTime? e2, DateTime? s2)
        {
            string periodoCalculado = "";

            TimeSpan periodo1 = TimeSpan.Zero;
            TimeSpan periodo2 = TimeSpan.Zero;
            double totalSegundos = 0;
            if (s1 != null && e1 != null)
                periodo1 = Convert.ToDateTime(s1).Subtract(Convert.ToDateTime(e1));

            if (s2 != null && e2 != null)
                periodo2 = Convert.ToDateTime(s2).Subtract(Convert.ToDateTime(e2));

            totalSegundos = periodo1.TotalSeconds + periodo2.TotalSeconds;

            TimeSpan time = TimeSpan.FromSeconds(totalSegundos);
            periodoCalculado = time.ToString(@"hh\:mm");

            return periodoCalculado;
        }

        #region "GRID BATIDA PENDENTE"
        protected void gvBatidaPendente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    RH_PONTO_BATIDA batida = e.Row.DataItem as RH_PONTO_BATIDA;

                    if (batida != null)
                    {
                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = new BaseController().BuscaFilialCodigo(Convert.ToInt32(batida.CODIGO_FILIAL)).FILIAL;

                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = batida.DATA.ToString("dd/MM/yyyy");

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                        {
                            if (batida.RH_FUNCIONARIO != null)
                                _litNome.Text = batida.RH_FUNCIONARIO1.VENDEDOR + " - " + batida.RH_FUNCIONARIO1.NOME.Trim();
                            else
                                _litNome.Text = batida.FUNCIONARIO_NOME.Trim();
                        }
                        Literal _litTipoBatida = e.Row.FindControl("litTipoBatida") as Literal;
                        if (_litTipoBatida != null)
                            if (batida.RH_PONTO_BATIDA_TIPO != null)
                                _litTipoBatida.Text = batida.RH_PONTO_BATIDA_TIPO1.DESCRICAO;

                        Literal _litEntrada1 = e.Row.FindControl("litEntrada1") as Literal;
                        if (_litEntrada1 != null)
                        {
                            if (batida.ENTRADA1 != null)
                                _litEntrada1.Text = Convert.ToDateTime(batida.ENTRADA1).ToString("HH:mm");
                            else
                                _litEntrada1.Text = "-";
                        }

                        Literal _litSaida1 = e.Row.FindControl("litSaida1") as Literal;
                        if (_litSaida1 != null)
                        {
                            if (batida.SAIDA1 != null)
                                _litSaida1.Text = Convert.ToDateTime(batida.SAIDA1).ToString("HH:mm");
                            else
                                _litSaida1.Text = "-";
                        }

                        Literal _litEntrada2 = e.Row.FindControl("litEntrada2") as Literal;
                        if (_litEntrada2 != null)
                        {
                            if (batida.ENTRADA2 != null)
                                _litEntrada2.Text = Convert.ToDateTime(batida.ENTRADA2).ToString("HH:mm");
                            else
                                _litEntrada2.Text = "-";
                        }

                        Literal _litSaida2 = e.Row.FindControl("litSaida2") as Literal;
                        if (_litSaida2 != null)
                        {
                            if (batida.SAIDA2 != null)
                                _litSaida2.Text = Convert.ToDateTime(batida.SAIDA2).ToString("HH:mm");
                            else
                                _litSaida2.Text = "-";
                        }
                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                            _litTotal.Text = CalcularPeriodo(batida.ENTRADA1, batida.SAIDA1, batida.ENTRADA2, batida.SAIDA2);

                        ImageButton _btExcluir = e.Row.FindControl("btExcluir") as ImageButton;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = batida.CODIGO.ToString();

                        ImageButton _btEditar = e.Row.FindControl("btEditar") as ImageButton;
                        if (_btEditar != null)
                            _btEditar.CommandArgument = batida.CODIGO.ToString();
                    }
                }
            }
        }
        protected void gvBatidaPendente_Sorting(object sender, GridViewSortEventArgs e)
        {
            /*IEnumerable<RH_PONTO_BATIDA> _pontoBatida = rhController.ObterPontoBatida().Where(p => (p.STATUS == 'N' || p.RH_PONTO_BATIDA_TIPO == 99) && p.STATUS != 'E' && DateTime.Today > p.DATA_INCLUSAO.Date.AddHours(23).AddMinutes(59)).ToList();

            if (ddlFilial.SelectedValue.Trim() != "")
                _pontoBatida = _pontoBatida.Where(p => p.CODIGO_FILIAL.Trim() == ddlFilial.SelectedValue.Trim()).ToList();

            if (txtData.Text.Trim() != "")
                _pontoBatida = _pontoBatida.Where(p => p.DATA == Convert.ToDateTime(txtData.Text.Trim())).ToList();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _pontoBatida = _pontoBatida.OrderBy(e.SortExpression + sortDirection);
            gvBatidaPendente.DataSource = _pontoBatida;
            gvBatidaPendente.DataBind();*/
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    GridViewRow row = (GridViewRow)bt.NamingContainer;
                    if (row != null)
                    {
                        int codigoPontoBatida = 0;

                        codigoPontoBatida = Convert.ToInt32(gvBatidaPendente.DataKeys[row.RowIndex].Value);

                        if (codigoPontoBatida > 0)
                        {
                            var pontoBatida = rhController.ObterPontoBatida(codigoPontoBatida);
                            if (pontoBatida != null)
                            {
                                pontoBatida.DATA_EXCLUSAO = DateTime.Now;
                                pontoBatida.USUARIO_EXCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                                pontoBatida.STATUS = 'E';
                                rhController.AtualizarPontoBatida(pontoBatida);

                                CarregarBatidasPendentes();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btEditar_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    string codigoBatida = b.CommandArgument;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('rh_ponto_batida_pendente_editar.aspx?p=" + codigoBatida + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }

        }

        /*protected void gvBatidaNova_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    RH_PONTO_BATIDA _pb = e.Row.DataItem as RH_PONTO_BATIDA;
                    coluna += 1;
                    if (_pb != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        DropDownList _ddlFilialNovo = e.Row.FindControl("ddlFilialNovo") as DropDownList;
                        if (_ddlFilialNovo != null)
                        {
                            _ddlFilialNovo.DataSource = ObterFilial();
                            _ddlFilialNovo.DataBind();

                            if (_pb.CODIGO_FILIAL != null)
                            {
                                _ddlFilialNovo.SelectedValue = _pb.CODIGO_FILIAL;

                                DropDownList _ddlFuncionarioNovo = e.Row.FindControl("ddlFuncionarioNovo") as DropDownList;
                                if (_ddlFuncionarioNovo != null)
                                {
                                    _ddlFuncionarioNovo.DataSource = ObterFuncionario(_pb.CODIGO_FILIAL);
                                    _ddlFuncionarioNovo.DataBind();

                                    if (_pb.RH_FUNCIONARIO != null)
                                        _ddlFuncionarioNovo.SelectedValue = _pb.RH_FUNCIONARIO.ToString();
                                }
                            }
                        }

                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = _pb.DATA.ToString("dd/MM/yyyy");


                        DropDownList _ddlTipoBatidaNovo = e.Row.FindControl("ddlTipoBatidaNovo") as DropDownList;
                        if (_ddlTipoBatidaNovo != null)
                        {
                            _ddlTipoBatidaNovo.DataSource = ObterTipoBatida();
                            _ddlTipoBatidaNovo.DataBind();

                            if (_pb.RH_PONTO_BATIDA_TIPO != null)
                            {
                                _ddlTipoBatidaNovo.SelectedValue = _pb.RH_PONTO_BATIDA_TIPO.ToString();
                                //ddlTipoBatida_SelectedIndexChanged(_ddlTipoBatidaNovo, null);
                            }
                        }

                        TextBox _txtEntrada1 = e.Row.FindControl("txtEntrada1") as TextBox;
                        if (_txtEntrada1 != null)
                        {
                            if (_pb.ENTRADA1 != null)
                                _txtEntrada1.Text = Convert.ToDateTime(_pb.ENTRADA1).ToString("HH:mm");
                        }

                        TextBox _txtSaida1 = e.Row.FindControl("txtSaida1") as TextBox;
                        if (_txtSaida1 != null)
                        {
                            if (_pb.SAIDA1 != null)
                                _txtSaida1.Text = Convert.ToDateTime(_pb.SAIDA1).ToString("HH:mm");
                        }

                        TextBox _txtEntrada2 = e.Row.FindControl("txtEntrada2") as TextBox;
                        if (_txtEntrada2 != null)
                        {
                            if (_pb.ENTRADA2 != null)
                                _txtEntrada2.Text = Convert.ToDateTime(_pb.ENTRADA2).ToString("HH:mm");
                        }

                        TextBox _txtSaida2 = e.Row.FindControl("txtSaida2") as TextBox;
                        if (_txtSaida2 != null)
                        {
                            if (_pb.SAIDA2 != null)
                                _txtSaida2.Text = Convert.ToDateTime(_pb.SAIDA2).ToString("HH:mm");
                        }

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                            _litTotal.Text = CalcularPeriodo(_pb.ENTRADA1, _pb.SAIDA1, _pb.ENTRADA2, _pb.SAIDA2);

                        ImageButton _btAprovar = e.Row.FindControl("btAprovar") as ImageButton;
                        if (_btAprovar != null)
                            _btAprovar.CommandArgument = _pb.CODIGO.ToString();

                        //Popular GRID VIEW FILHO
                        GridView gvBatidaPendente = e.Row.FindControl("gvBatidaPendente") as GridView;
                        if (gvBatidaPendente != null)
                        {
                            List<RH_PONTO_BATIDA> pontoBatidaFilho = new List<RH_PONTO_BATIDA>();
                            var _pontoBatida = rhController.ObterPontoBatida(_pb.CODIGO);
                            if (_pontoBatida != null)
                                pontoBatidaFilho.Add(_pb);

                            gvBatidaPendente.DataSource = pontoBatidaFilho;
                            gvBatidaPendente.DataBind();
                        }

                    }
                }
            }
        }
        protected void btAprovar_Click(object sender, EventArgs e)
        {
            int codigoBatida = 0;
            string msg = "";
            RH_PONTO_BATIDA _pontoBatida = null;
            try
            {
                labErroGrid.Text = "";

                ImageButton b = (ImageButton)sender;
                if (b != null)
                {
                    //Obter referencia para Obter o Codigo da Batida
                    codigoBatida = Convert.ToInt32(b.CommandArgument);
                    _pontoBatida = rhController.ObterPontoBatida(codigoBatida);

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        DropDownList _ddlFilial = row.FindControl("ddlFilialNovo") as DropDownList;
                        DropDownList _ddlFuncionario = row.FindControl("ddlFuncionarioNovo") as DropDownList;
                        DropDownList _ddlTipoBatida = row.FindControl("ddlTipoBatidaNovo") as DropDownList;
                        TextBox _txtEntrada1 = row.FindControl("txtEntrada1") as TextBox;
                        TextBox _txtSaida1 = row.FindControl("txtSaida1") as TextBox;
                        TextBox _txtEntrada2 = row.FindControl("txtEntrada2") as TextBox;
                        TextBox _txtSaida2 = row.FindControl("txtSaida2") as TextBox;
                        Literal _litData = row.FindControl("litData") as Literal;

                        if (_ddlFilial.SelectedValue == "")
                        {
                            msg = "Selecione a Filial.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            labErroGrid.Text = msg;
                            return;
                        }

                        if (_ddlFuncionario.SelectedValue == "0")
                        {
                            msg = "Selecione o Funcionário.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            labErroGrid.Text = msg;
                            return;
                        }

                        if (_ddlTipoBatida.SelectedValue == "0")
                        {
                            msg = "Selecione o Tipo de Batida.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            labErroGrid.Text = msg;
                            return;
                        }

                        if (_ddlTipoBatida.SelectedValue == "99")
                        {
                            msg = "Tipo de Batida não pode ser PENDENTE.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            labErroGrid.Text = msg;
                            return;
                        }

                        if (_ddlTipoBatida.SelectedValue == "1" || _ddlTipoBatida.SelectedValue == "6")
                        {
                            if (_txtEntrada1.Text == "" || _txtSaida1.Text == "" || _txtEntrada2.Text == "" || _txtSaida2.Text == "")
                            {
                                msg = "Informe o PERÍODO trabalhado corretamente!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                labErroGrid.Text = msg;
                                return;
                            }

                            if (_txtEntrada1.Text.Trim() != "")
                            {
                                if (!ValidarData(_txtEntrada1.Text.Trim()))
                                {
                                    msg = "Hora da ENTRADA 1 Inválida!";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                    labErroGrid.Text = msg;
                                    return;
                                }
                            }

                            if (_txtSaida1.Text.Trim() != "")
                            {
                                if (!ValidarData(_txtSaida1.Text.Trim()))
                                {
                                    msg = "Hora da SAÍDA 1 Inválida!";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                    labErroGrid.Text = msg;
                                    return;
                                }
                            }

                            if (_txtEntrada2.Text.Trim() != "")
                            {
                                if (!ValidarData(_txtEntrada2.Text.Trim()))
                                {
                                    msg = "Hora da ENTRADA 2 Inválida!";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                    labErroGrid.Text = msg;
                                    return;
                                }
                            }

                            if (_txtSaida2.Text.Trim() != "")
                            {
                                if (!ValidarData(_txtSaida2.Text.Trim()))
                                {
                                    msg = "Hora da SAÍDA 2 Inválida!";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                    labErroGrid.Text = msg;
                                    return;
                                }
                            }

                            if (!ValidarPeriodo(_txtEntrada1.Text, _txtSaida1.Text, _txtEntrada2.Text, _txtSaida2.Text))
                            {
                                msg = "O período da HORA está incorreto. Informe as horas em ORDEM.";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                labErroGrid.Text = msg;
                                return;
                            }
                        }

                        int? pontoPeriodoTrab = null;
                        int diaSemana = ((int)Convert.ToDateTime(_litData.Text.Trim()).DayOfWeek) + 1;
                        var perTrab = rhController.ObterFuncionarioPeriodoTrab(_ddlFuncionario.SelectedValue).Where(p => diaSemana >= p.RH_PONTO_PERIODO_TRAB1.DIA_INICIAL && diaSemana <= p.RH_PONTO_PERIODO_TRAB1.DIA_FINAL).ToList();
                        if (perTrab != null && perTrab.Count() > 0)
                            pontoPeriodoTrab = perTrab[0].RH_PONTO_PERIODO_TRAB;

                        if (pontoPeriodoTrab == null)
                        {
                            msg = "Funcionário não possui Período de Trabalho cadastrado.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            labErroGrid.Text = msg;
                            return;
                        }

                        var verificaBatida = rhController.ObterPontoBatida().Where(p => p.RH_FUNCIONARIO != null && p.RH_FUNCIONARIO == Convert.ToInt32(_ddlFuncionario.SelectedValue) && p.DATA == _pontoBatida.DATA && p.STATUS != 'E').SingleOrDefault();
                        if (verificaBatida != null && codigoBatida != verificaBatida.CODIGO)
                        {
                            msg = "Funcionário já possui batida neste dia.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            labErroGrid.Text = msg;
                            return;
                        }

                        _pontoBatida.CODIGO_FILIAL = _ddlFilial.SelectedValue;
                        _pontoBatida.RH_FUNCIONARIO = Convert.ToInt32(_ddlFuncionario.SelectedValue);
                        _pontoBatida.RH_PONTO_BATIDA_TIPO = Convert.ToInt32(_ddlTipoBatida.SelectedValue);
                        _pontoBatida.RH_PONTO_PERIODO_TRAB = pontoPeriodoTrab;
                        _pontoBatida.ENTRADA1 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + _txtEntrada1.Text.Trim());
                        _pontoBatida.SAIDA1 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + _txtSaida1.Text.Trim());
                        _pontoBatida.ENTRADA2 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + _txtEntrada2.Text.Trim());
                        _pontoBatida.SAIDA2 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + _txtSaida2.Text.Trim());
                        _pontoBatida.STATUS = 'B';
                        _pontoBatida.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        _pontoBatida.DATA_ALTERACAO = DateTime.Now;

                        rhController.AtualizarPontoBatida(_pontoBatida);

                        CarregarBatidasPendentes();
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ddlTipoBatida_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList _ddlTipoBatida = (DropDownList)sender;
                if (_ddlTipoBatida != null)
                {
                    GridViewRow row = (GridViewRow)_ddlTipoBatida.NamingContainer;
                    if (row != null)
                    {
                        labErroGrid.Text = "";

                        Literal _litTotal = row.FindControl("litTotal") as Literal;
                        Literal _litData = row.FindControl("litData") as Literal;
                        TextBox _txtEntrada1 = row.FindControl("txtEntrada1") as TextBox;
                        TextBox _txtSaida1 = row.FindControl("txtSaida1") as TextBox;
                        TextBox _txtEntrada2 = row.FindControl("txtEntrada2") as TextBox;
                        TextBox _txtSaida2 = row.FindControl("txtSaida2") as TextBox;
                        DropDownList _ddlFuncionarioNovo = row.FindControl("ddlFuncionarioNovo") as DropDownList;

                        _txtEntrada1.Text = "";
                        _txtSaida1.Text = "";
                        _txtEntrada2.Text = "";
                        _txtSaida2.Text = "";

                        _txtEntrada1.Enabled = true;
                        _txtSaida1.Enabled = true;
                        _txtEntrada2.Enabled = true;
                        _txtSaida2.Enabled = true;
                        if (_ddlTipoBatida.SelectedValue != "1" && _ddlTipoBatida.SelectedValue != "0" && _ddlTipoBatida.SelectedValue != "6")
                        {
                            _txtEntrada1.Enabled = false;
                            _txtSaida1.Enabled = false;
                            _txtEntrada2.Enabled = false;
                            _txtSaida2.Enabled = false;
                        }

                        if (_ddlFuncionarioNovo.SelectedValue == "0")
                        {
                            labErroGrid.Text = "Selecione o Funcionário.";
                            _ddlTipoBatida.SelectedValue = "0";
                            return;
                        }

                        if (_ddlTipoBatida.SelectedValue == "6" && _ddlFuncionarioNovo.SelectedValue != "0")
                        {
                            //TRAZER HORA CADASTRADA NO PERIODO
                            int diaSemana = (int)Convert.ToDateTime(_litData.Text.Trim()).DayOfWeek + 1;
                            var periodoTrabalho = rhController.ObterFuncionarioPeriodoTrab(_ddlFuncionarioNovo.SelectedValue).Where(p => diaSemana >= p.RH_PONTO_PERIODO_TRAB1.DIA_INICIAL && diaSemana <= p.RH_PONTO_PERIODO_TRAB1.DIA_FINAL).ToList();
                            if (periodoTrabalho == null || periodoTrabalho.Count <= 0)
                            {
                                labErroGrid.Text = "Funcionário não possui Período de Trabalho cadastrado.";
                                _ddlTipoBatida.SelectedValue = "0";
                                return;
                            }

                            _txtEntrada1.Text = periodoTrabalho[0].RH_PONTO_PERIODO_TRAB1.HORA_INICIAL.ToString("HH:mm");
                            _txtSaida2.Text = periodoTrabalho[0].RH_PONTO_PERIODO_TRAB1.HORA_FINAL.ToString("HH:mm");
                        }

                        if (_litTotal != null)
                            _litTotal.Text = CalcularPeriodo(null, null, null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                labErroGrid.Text = ex.Message;
            }
        }
        protected void txtHora_TextChanged(object sender, EventArgs e)
        {
            DateTime? entrada1 = null;
            DateTime? saida1 = null;
            DateTime? entrada2 = null;
            DateTime? saida2 = null;
            string msg = "";

            try
            {

                TextBox txt = (TextBox)sender;
                if (txt != null)
                {
                    GridViewRow row = (GridViewRow)txt.NamingContainer;
                    if (row != null)
                    {
                        TextBox _entrada1 = row.FindControl("txtEntrada1") as TextBox;
                        TextBox _saida1 = row.FindControl("txtSaida1") as TextBox;
                        TextBox _entrada2 = row.FindControl("txtEntrada2") as TextBox;
                        TextBox _saida2 = row.FindControl("txtSaida2") as TextBox;

                        Literal _litTotal = row.FindControl("litTotal") as Literal;

                        if (_entrada1.Text.Trim() != "")
                        {
                            if (!ValidarData(_entrada1.Text.Trim()))
                            {
                                txt.Text = "";
                                msg = "Hora da ENTRADA 1 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            entrada1 = Convert.ToDateTime(_entrada1.Text);
                        }

                        if (_saida1.Text.Trim() != "")
                        {
                            if (!ValidarData(_saida1.Text.Trim()))
                            {
                                txt.Text = "";
                                msg = "Hora da SAÍDA 1 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            saida1 = Convert.ToDateTime(_saida1.Text);
                        }

                        if (_entrada2.Text.Trim() != "")
                        {
                            if (!ValidarData(_entrada2.Text.Trim()))
                            {
                                txt.Text = "";
                                msg = "Hora da ENTRADA 2 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            entrada2 = Convert.ToDateTime(_entrada2.Text);
                        }

                        if (_saida2.Text.Trim() != "")
                        {
                            if (!ValidarData(_saida2.Text.Trim()))
                            {
                                txt.Text = "";
                                msg = "Hora da SAÍDA 2 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            saida2 = Convert.ToDateTime(_saida2.Text);
                        }

                        if (_litTotal != null)
                            _litTotal.Text = CalcularPeriodo(entrada1, saida1, entrada2, saida2);

                        if (txt.ID == "txtEntrada1" || txt.ID == "txtEntrada1F")
                            _saida1.Focus();
                        if (txt.ID == "txtSaida1" || txt.ID == "txtSaida1F")
                            _entrada2.Focus();
                        if (txt.ID == "txtEntrada2" || txt.ID == "txtEntrada2F")
                            _saida2.Focus();

                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }

        }
        protected void ddlFilialNovo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList _ddl = (DropDownList)sender;
            if (_ddl != null)
            {
                GridViewRow row = (GridViewRow)_ddl.NamingContainer;
                if (row != null)
                {
                    DropDownList _ddlFuncionarioNovo = row.FindControl("ddlFuncionarioNovo") as DropDownList;
                    if (_ddlFuncionarioNovo != null)
                    {
                        _ddlFuncionarioNovo.DataSource = ObterFuncionario(_ddl.SelectedValue);
                        _ddlFuncionarioNovo.DataBind();
                    }
                }
            }
        }*/

        #endregion
        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {



                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    gvBatidaPendente.HeaderRow.BackColor = Color.White;
                    foreach (TableCell cell in gvBatidaPendente.HeaderRow.Cells)
                    {
                        cell.BackColor = gvBatidaPendente.HeaderStyle.BackColor;
                    }
                    foreach (GridViewRow row in gvBatidaPendente.Rows)
                    {
                        row.BackColor = Color.White;
                        foreach (TableCell cell in row.Cells)
                        {
                            if (row.RowIndex % 2 == 0)
                            {
                                cell.BackColor = gvBatidaPendente.AlternatingRowStyle.BackColor;
                            }
                            else
                            {
                                cell.BackColor = gvBatidaPendente.RowStyle.BackColor;
                            }
                            cell.CssClass = "textmode";
                        }
                    }

                    gvBatidaPendente.RenderControl(hw);

                    //style to format numbers to string
                    string style = @"<style> .textmode { } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception)
            {
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

    }
}
