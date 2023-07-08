using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using Relatorios.mod_rh.email;

namespace Relatorios.mod_desenvolvimento
{
    public partial class rh_cad_funcionario : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        BaseController baseController = new BaseController();

        int colunaPeriodo = 0, colunaTodos = 0;

        const string cAbaTodos = "1";

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataAdmissao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataDemissao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                Session["PERIODO_TRABALHO"] = null;

                CarregarFilial();
                CarregarPeriodoExp();
                CarregarPeriodoTrabalho(0);
                carregaCargos();

                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;
                ibtEditar.Visible = false;
                ibtExcluir.Visible = false;
                ibtLimpar.Visible = false;

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {

                var lstFilial = baseController.ListarFiliais().Where(p => p.INDICA_LOJA == true).ToList();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI1 { COD_FILIAL = "", FILIAL = "" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                    }
                }
            }
        }
        private void CarregarPeriodoExp()
        {

            var periodoExp = rhController.ObterPeriodoExperiencia();

            periodoExp.Insert(0, new RH_FUNCIONARIO_PERIODO_EXP { CODIGO = 0, DESCRICAO = "Selecione" });

            ddlPeriodoExp.DataSource = periodoExp;
            ddlPeriodoExp.DataBind();

        }

        private List<RH_PONTO_PERIODO_TRAB> ObterPeriodoTrabalho()
        {
            List<RH_PONTO_PERIODO_TRAB> _periodoTrab = new List<RH_PONTO_PERIODO_TRAB>();
            _periodoTrab = rhController.ObterPeriodoTrabalho();
            return _periodoTrab;
        }

        private void carregaCargos()
        {
            List<RH_CARGO> cargos = new List<RH_CARGO>();
            cargos = baseController.BuscarCargos('L');

            if (cargos != null)
            {
                cargos.Insert(0, new RH_CARGO { DESC_CARGO = "Selecione" });
                ddlCargo.DataSource = cargos;
                ddlCargo.DataBind();
            }
        }
        #endregion

        #region "AÇÕES DA TELA"
        protected void ibtIncluir_Click(object sender, EventArgs e)
        {
            //controle de visibilidade dos botões da tela
            ibtIncluir.Visible = false;
            ibtEditar.Visible = false;
            ibtExcluir.Visible = false;
            ibtPesquisar.Visible = false;
            ibtLimpar.Visible = false;

            //Controle de campos
            HabilitarCampos(true);
            LimparCampos();

            hidAcao.Value = "I";
            txtVendedorCodigo.Enabled = false;
            ibtSalvar.Visible = true;
            ibtCancelar.Visible = true;

            int codigoFuncionario = 0;
            if (hidCodigoFuncionario.Value != "")
                codigoFuncionario = Convert.ToInt32(hidCodigoFuncionario.Value);
            CarregarPeriodoTrabalho(codigoFuncionario);

            MoverAba("0");
            Session["PERIODO_TRABALHO"] = null;
        }
        protected void ibtEditar_Click(object sender, EventArgs e)
        {
            try
            {
                Session["PERIODO_TRABALHO"] = null;

                //controle de visibilidade dos botões da tela
                ibtIncluir.Visible = false;
                ibtEditar.Visible = false;
                ibtExcluir.Visible = false;
                ibtPesquisar.Visible = false;
                ibtLimpar.Visible = false;

                ibtSalvar.Visible = true;
                ibtCancelar.Visible = true;

                labErro.Text = "";
                HabilitarCampos(true);
                hidAcao.Value = "A";
                txtVendedorCodigo.Enabled = false;

                gvPeriodoTrabalho_DataBound(null, null);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {

            labErro.Text = "";
            try
            {
                Session["PERIODO_TRABALHO"] = null;

                string codigoFuncionario = hidCodigoFuncionario.Value;

                if (codigoFuncionario != "")
                {
                    var _funcionario = rhController.ObterFuncionario(Convert.ToInt32(codigoFuncionario), "", "", "", "");
                    if (_funcionario != null && _funcionario.Count() > 0)
                    {

                        CarregarFuncionario(_funcionario[0]);
                        CarregarPeriodoTrabalho(_funcionario[0].CODIGO);

                        ibtIncluir.Visible = true;
                        ibtPesquisar.Visible = false;
                        ibtSalvar.Visible = false;
                        ibtCancelar.Visible = false;
                        ibtEditar.Visible = true;
                        ibtLimpar.Visible = true;
                        ibtExcluir.Visible = true;
                        hidAcao.Value = "C";

                        HabilitarCampos(false);
                        gvPeriodoTrabalho_DataBound(null, null);

                        MoverAba("0");
                    }
                }
                else
                {
                    // USAR PELO MENOS ALGUM FILTRO
                    if (ddlFilial.SelectedValue.Trim() == "" && txtNome.Text.Trim() == "" && txtCPF.Text.Trim() == "")
                    {
                        labErro.Text = "Informe pelo menos um Filtro (Filial, Nome ou CPF).";
                        return;
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + cAbaTodos + "');", true);
                    hidTabSelected.Value = cAbaTodos;
                    CarregarFuncionarioTodos();
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtLimpar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                ibtPesquisar.Visible = true;
                ibtEditar.Visible = false;
                ibtLimpar.Visible = false;
                ibtExcluir.Visible = false;
                hidAcao.Value = "";

                HabilitarCampos(true);
                LimparCampos();

                Session["PERIODO_TRABALHO"] = null;
                CarregarPeriodoTrabalho(0);
                //CarregarFuncionarioTodos();
                MoverAba("0");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtSalvar_Click(object sender, EventArgs e)
        {
            //Validar campos obrigatórios
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho";
                    return;
                }

                if (!Utils.WebControls.ValidarCPF(txtCPF.Text.Trim()))
                {
                    labErro.Text = "CPF INVÁLIDO.";
                    return;
                }

                var funcionarios = rhController.ObterFuncionario().Where(p => p.CPF.Trim() == txtCPF.Text.Trim()).ToList();
                if (funcionarios != null && funcionarios.Count() > 0)
                {
                    if (hidAcao.Value == "I")
                    {
                        labErro.Text = "Este CPF já está cadastrado para o Funcionário: " + funcionarios[0].NOME.ToUpper() + " na Filial: " + funcionarios[0].CODIGO_FILIAL + ".";
                        return;
                    }
                    else
                    {
                        if (Convert.ToInt32(hidCodigoFuncionario.Value) != funcionarios[0].CODIGO)
                        {
                            labErro.Text = "Este CPF já está cadastrado para o Funcionário: " + funcionarios[0].NOME.ToUpper();
                            return;
                        }
                    }
                }

                DateTime? dataDemissao = null;

                if (txtDataDemissao.Text.Trim() != "")
                    dataDemissao = Convert.ToDateTime(txtDataDemissao.Text);

                //INCLUIR FUNCIONARIO NO LINX
                var funLINX = rhController.CriarFuncionarioLINX(
                    txtVendedorCodigo.Text.Trim(),
                    "",
                    ddlFilial.SelectedValue,
                    txtVendedorApelido.Text.Trim().ToUpper(),
                    txtNome.Text.Trim().ToUpper(),
                    Convert.ToDecimal(txtComissao.Text),
                    txtRG.Text.ToUpper(),
                    txtEndereco.Text.Trim().ToUpper(),
                    txtComplemento.Text.Trim().ToUpper(),
                    txtCidade.Text.Trim().ToUpper(),
                    txtCEP.Text,
                    txtTelefone.Text,
                    txtUF.Text.Trim().ToUpper(),
                    txtCPF.Text.Trim(),
                    ((cbGerenteLoja.Checked) ? '1' : '0'),
                    cbOperaCaixa.Checked,
                    cbAcessoGerencial.Checked,
                    Convert.ToDateTime(txtDataAdmissao.Text.Trim()),
                    dataDemissao,
                    ddlCargo.SelectedValue,
                    //txtCargo.Text.Trim().ToUpper(),
                    Convert.ToChar(hidAcao.Value));


                if (funLINX == null || funLINX.VENDEDOR == "")
                {
                    labErro.Text = "Erro ao inserir funcionário no LINX. Entre em contato com TI.";
                    return;
                }

                //INCLUIR FUNCIONARIO NA INTRANET
                var codigoFuncionario = 0;
                if (hidCodigoFuncionario.Value != "")
                    codigoFuncionario = Convert.ToInt32(hidCodigoFuncionario.Value);
                var funcionario = rhController.ObterFuncionario(codigoFuncionario);
                if (funcionario == null)
                {
                    funcionario = new RH_FUNCIONARIO();
                }
                else
                {
                    // se inseriu data de demissao
                    if (txtDataDemissao.Text.Trim() != "")
                    {
                        var dataDemissaoAux = Convert.ToDateTime(txtDataDemissao.Text.Trim());
                        if (dataDemissaoAux != funcionario.DATA_DEMISSAO)
                            funcionario.WAPP_BAIXA = null;
                    }
                    else
                    {
                        if (funcionario.DATA_DEMISSAO != null)
                            funcionario.WAPP_BAIXA = null;
                    }
                }

                funcionario.CODIGO_FILIAL = ddlFilial.SelectedValue;
                funcionario.NOME = txtNome.Text.Trim().ToUpper();
                funcionario.CPF = txtCPF.Text.Trim();
                funcionario.RH_FUNCIONARIO_PERIODO_EXP = Convert.ToInt32(ddlPeriodoExp.SelectedValue);
                funcionario.DATA_ADMISSAO = Convert.ToDateTime(txtDataAdmissao.Text.Trim());
                if (txtDataDemissao.Text.Trim() != "")
                    funcionario.DATA_DEMISSAO = Convert.ToDateTime(txtDataDemissao.Text.Trim());
                else
                    funcionario.DATA_DEMISSAO = null;

                funcionario.FOTO = "";
                funcionario.BATE_PONTO = (cbBatePonto.Checked) ? 'S' : 'N';
                funcionario.DATA_INCLUSAO = DateTime.Now;
                funcionario.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                funcionario.STATUS = 'A';
                funcionario.SEXO = Convert.ToChar(ddlSexo.SelectedValue);
                funcionario.DESC_CARGO = ddlCargo.SelectedItem.Text.Trim().ToUpper();
                funcionario.VENDEDOR = funLINX.VENDEDOR;

                if (hidAcao.Value == "I")
                    codigoFuncionario = rhController.InserirFuncionario(funcionario);
                else
                    rhController.AtualizarFuncionario(funcionario);

                //PERIODO DE TRABALHO
                if (Session["PERIODO_TRABALHO"] != null)
                {
                    RH_FUNCIONARIO_PONTO_PERIODO_TRAB _funcPeriodo = null;

                    //LOOP NA SESSION PARA INSERIR APENAS OS NOVOS//
                    List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> listaPeriodoTrabalho = new List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>();
                    listaPeriodoTrabalho = (List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>)Session["PERIODO_TRABALHO"];
                    foreach (RH_FUNCIONARIO_PONTO_PERIODO_TRAB per in listaPeriodoTrabalho)
                    {

                        if (per != null)
                        {
                            _funcPeriodo = new RH_FUNCIONARIO_PONTO_PERIODO_TRAB();
                            _funcPeriodo.RH_FUNCIONARIO = codigoFuncionario;
                            _funcPeriodo.RH_PONTO_PERIODO_TRAB = per.RH_PONTO_PERIODO_TRAB;
                            _funcPeriodo.DATA_INCLUSAO = DateTime.Now;
                            _funcPeriodo.STATUS = 'A';

                            rhController.InserirFuncionarioPeriodoTrab(_funcPeriodo);
                        }
                    }
                }

                var usuario = ((USUARIO)Session["USUARIO"]);
                EmailFuncionario.EnviarEmailAlteracaoFun(funcionario, ((hidAcao.Value == "I") ? true : false), usuario, txtTelefone.Text.Trim(), txtUF.Text.Trim());

                hidCodigoFuncionario.Value = codigoFuncionario.ToString();
                //Entrar em modo de CONSULTA
                ibtPesquisar_Click(null, null);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (!ValidarCampos() && hidAcao.Value == "A")
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho";
                    return;
                }

                //controle de visibilidade dos botões da tela
                ibtIncluir.Visible = true;
                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;

                System.Drawing.Color _OK = System.Drawing.Color.Gray;
                labFilial.ForeColor = _OK;
                labNome.ForeColor = _OK;
                labCPF.ForeColor = _OK;
                labCodigoVendedor.ForeColor = _OK;
                labVendedorApelido.ForeColor = _OK;
                labCargo.ForeColor = _OK;
                labComissao.ForeColor = _OK;
                labRG.ForeColor = _OK;
                labSexo.ForeColor = _OK;
                labPeriodoExp.ForeColor = _OK;
                labDataAdmissao.ForeColor = _OK;
                labDataDemissao.ForeColor = _OK;
                labTelefone.ForeColor = _OK;
                labEndereco.ForeColor = _OK;
                labComplemento.ForeColor = _OK;
                labCidade.ForeColor = _OK;
                labUF.ForeColor = _OK;
                labCEP.ForeColor = _OK;
                labBatePonto.ForeColor = _OK;
                labOperaCaixa.ForeColor = _OK;
                labAcessoGerencial.ForeColor = _OK;
                labGerenteLoja.ForeColor = _OK;
                labPeriodoTrabalho.ForeColor = _OK;

                if (hidAcao.Value == "I")
                {
                    ibtLimpar_Click(null, null);

                    ibtPesquisar.Visible = true;

                }

                if (hidAcao.Value == "A")
                {
                    ibtPesquisar_Click(null, null);
                    ibtPesquisar.Visible = false;
                    ibtExcluir.Visible = true;
                    ibtEditar.Visible = true;
                    ibtLimpar.Visible = true;

                    gvPeriodoTrabalho_DataBound(null, null);
                }

                hidAcao.Value = "";
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (hidCodigoFuncionario.Value == "")
                {
                    labErro.Text = "Funcionário não informado. Refaça sua pesquisa.";
                }
                else
                {
                    SP_OBTER_FUNCIONARIOResult _funcionario = null;
                    _funcionario = rhController.ObterFuncionario(Convert.ToInt32(hidCodigoFuncionario.Value), "", "", "", "").SingleOrDefault();

                    /*List<FUNCIONARIO_PONTO_PERIODO_TRAB> _periodoTrabFuncionario = rhController.ObterFuncionarioPeriodoTrab(hidCodigoFuncionario.Value);
                    foreach (FUNCIONARIO_PONTO_PERIODO_TRAB per in _periodoTrabFuncionario)
                        if (per != null)
                            rhController.ExcluirFuncionarioPeriodoTrab(per.CODIGO);*/

                    //rhController.ExcluirFuncionario(Convert.ToInt32(hidCodigoFuncionario.Value));

                    ibtLimpar_Click(null, null);
                    MoverAba("0");
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        private void CarregarFuncionario(SP_OBTER_FUNCIONARIOResult _funcionario)
        {
            ddlFilial.SelectedValue = _funcionario.CODIGO_FILIAL;
            txtNome.Text = _funcionario.NOME.Trim();
            txtCPF.Text = _funcionario.CPF;
            txtVendedorCodigo.Text = _funcionario.VENDEDOR;

            ddlStatus.SelectedValue = (_funcionario.DATA_DEMISSAO == null) ? "A" : "I";

            //FUNCIONARIO
            txtVendedorApelido.Text = _funcionario.VENDEDOR_APELIDO;
            //txtCargo.Text = _funcionario.DESC_CARGO;

            if (_funcionario.DESC_CARGO_INTRA != null)
            {
                ddlCargo.SelectedValue = _funcionario.DESC_CARGO_INTRA.Trim();
            }
            else
            {
                ddlCargo.SelectedValue = "Selecione";
            }

            txtComissao.Text = _funcionario.COMISSAO.ToString();
            txtRG.Text = _funcionario.RG;
            ddlSexo.SelectedValue = _funcionario.SEXO.ToString();
            ddlPeriodoExp.SelectedValue = _funcionario.RH_FUNCIONARIO_PERIODO_EXP.ToString();
            if (_funcionario.DATA_ADMISSAO != null)
                txtDataAdmissao.Text = Convert.ToDateTime(_funcionario.DATA_ADMISSAO).ToString("dd/MM/yyyy");
            if (_funcionario.DATA_DEMISSAO != null)
                txtDataDemissao.Text = Convert.ToDateTime(_funcionario.DATA_DEMISSAO).ToString("dd/MM/yyyy");
            txtTelefone.Text = _funcionario.TELEFONE;
            txtEndereco.Text = _funcionario.ENDERECO;
            txtComplemento.Text = _funcionario.COMPLEMENTO;
            txtCidade.Text = _funcionario.CIDADE;
            txtUF.Text = _funcionario.UF;
            txtCEP.Text = _funcionario.CEP;

            imgFoto.ImageUrl = (_funcionario.FOTO == null || _funcionario.FOTO == "") ? "~/Image/fun_sfoto.jpg" : _funcionario.FOTO;

            cbBatePonto.Checked = (_funcionario.BATE_PONTO == 'S') ? true : false;
            cbOperaCaixa.Checked = (Convert.ToBoolean(_funcionario.OPERA_CAIXA)) ? true : false;
            cbAcessoGerencial.Checked = (Convert.ToBoolean(_funcionario.ACESSO_GERENCIAL)) ? true : false;
            cbGerenteLoja.Checked = (_funcionario.GERENTE == '1') ? true : false;

        }
        #endregion

        #region "PERIODO DE TRABALHO"
        private void CarregarPeriodoTrabalho(int codigoFuncionario)
        {
            List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> _perFunc = rhController.ObterFuncionarioPeriodoTrab(codigoFuncionario.ToString());

            if (Session["PERIODO_TRABALHO"] != null)
                _perFunc.AddRange((List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>)Session["PERIODO_TRABALHO"]);

            if (_perFunc == null || _perFunc.Count <= 0)
                _perFunc.Add(new RH_FUNCIONARIO_PONTO_PERIODO_TRAB { RH_FUNCIONARIO = 0, RH_PONTO_PERIODO_TRAB = 0 });

            gvPeriodoTrabalho.DataSource = _perFunc;
            gvPeriodoTrabalho.DataBind();
        }
        protected void gvPeriodoTrabalho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    RH_FUNCIONARIO_PONTO_PERIODO_TRAB _perFunc = e.Row.DataItem as RH_FUNCIONARIO_PONTO_PERIODO_TRAB;

                    colunaPeriodo += 1;
                    if (_perFunc != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaPeriodo.ToString();

                        DropDownList _ddlPeriodo = e.Row.FindControl("ddlPeriodo") as DropDownList;
                        if (_ddlPeriodo != null)
                        {
                            _ddlPeriodo.DataSource = ObterPeriodoTrabalho();
                            _ddlPeriodo.DataBind();

                            _ddlPeriodo.SelectedValue = rhController.ObterPeriodoTrabalho(_perFunc.RH_PONTO_PERIODO_TRAB).CODIGO.ToString();
                        }

                        Label _labPeriodo = e.Row.FindControl("labPeriodo") as Label;
                        if (_labPeriodo != null)
                            if (_perFunc.RH_PONTO_PERIODO_TRAB1 != null)
                                _labPeriodo.Text = _perFunc.RH_PONTO_PERIODO_TRAB1.DESCRICAO;

                        ImageButton _ibtEditar = e.Row.FindControl("btEditarPeriodo") as ImageButton;
                        ImageButton _ibtSalvarPeriodo = e.Row.FindControl("btSalvarPeriodo") as ImageButton;
                        ImageButton _ibtSair = e.Row.FindControl("btSairPeriodo") as ImageButton;
                        ImageButton _ibtExcluir = e.Row.FindControl("btExcluirPeriodo") as ImageButton;
                        _ibtEditar.Visible = false;
                        _ibtSalvarPeriodo.Visible = false;
                        _ibtSair.Visible = false;
                        _ibtExcluir.Visible = false;
                        if (_perFunc.RH_PONTO_PERIODO_TRAB > 0)
                        {
                            _ibtExcluir.Visible = true;
                            _ibtExcluir.CommandArgument = _perFunc.CODIGO.ToString();

                            if (e.Row.RowState.ToString().ToUpper().Contains("EDIT"))
                            {
                                _ibtSalvarPeriodo.Visible = true;
                                _ibtSair.Visible = true;
                            }
                            else
                            {
                                _ibtEditar.Visible = true;
                            }
                        }
                    }
                }
            }

        }
        protected void gvPeriodoTrabalho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvPeriodoTrabalho.FooterRow;
            if (footer != null)
            {
                DropDownList _ddlPeriodoFooter = footer.FindControl("ddlPeriodoFooter") as DropDownList;
                if (_ddlPeriodoFooter != null)
                {
                    _ddlPeriodoFooter.DataSource = ObterPeriodoTrabalho();
                    _ddlPeriodoFooter.DataBind();
                }

                ImageButton _btIncluirPeriodo = footer.FindControl("btIncluirPeriodo") as ImageButton;
                if (_btIncluirPeriodo != null)
                {
                    _btIncluirPeriodo.Visible = false;
                    if (hidAcao.Value == "I" || hidAcao.Value == "A")
                        _btIncluirPeriodo.Visible = true;
                }
            }
        }
        protected void btEditarPeriodo_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    try
                    {

                        //Obter código da COR
                        string codigoPeriodoTrabalhoFuncionario = gvPeriodoTrabalho.DataKeys[row.RowIndex].Value.ToString();
                        /*var _perFunc = rhController.ObterFuncionarioPeriodoTrab(codigoPeriodoTrabalhoFuncionario);
                        if (_perFunc != null && _perFunc.Count() > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR já possui movimento. Não será possível Alterar.', { header: 'Aviso', life: 3000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }*/

                        gvPeriodoTrabalho.EditIndex = row.RowIndex;

                        int codigoFuncionario = 0;
                        if (hidCodigoFuncionario.Value != "")
                            codigoFuncionario = Convert.ToInt32(hidCodigoFuncionario.Value);
                        CarregarPeriodoTrabalho(codigoFuncionario);

                        //Botão acionado é escondido
                        bt.Visible = false;

                        ImageButton _btSairPeriodo = row.FindControl("btSairPeriodo") as ImageButton;
                        if (_btSairPeriodo != null)
                            _btSairPeriodo.Visible = true;

                        ImageButton _btSalvarPeriodo = row.FindControl("btSalvarPeriodo") as ImageButton;
                        if (_btSalvarPeriodo != null)
                            _btSalvarPeriodo.Visible = true;

                        Label _labPeriodo = row.FindControl("labPeriodo") as Label;

                        hidDescricaoPeriodo.Value = _labPeriodo.Text.Trim();

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btEditarPeriodo_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btSalvarPeriodo_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {

                        int codigoFuncionario = 0;
                        if (hidCodigoFuncionario.Value != "")
                            codigoFuncionario = Convert.ToInt32(hidCodigoFuncionario.Value);

                        string codigoPerTrabFuncionario = "";
                        DropDownList _ddlPeriodo = row.FindControl("ddlPeriodo") as DropDownList;

                        if (!ValidarPeriodoTrabalho(_ddlPeriodo.SelectedItem.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Este PERÍODO Já foi inserido.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> _perTrabFun = new List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>();
                        if (Session["PERIODO_TRABALHO"] != null)
                            _perTrabFun = (List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>)Session["PERIODO_TRABALHO"];

                        //Obter código
                        codigoPerTrabFuncionario = gvPeriodoTrabalho.DataKeys[row.RowIndex].Value.ToString();

                        RH_FUNCIONARIO_PONTO_PERIODO_TRAB perTrab = new RH_FUNCIONARIO_PONTO_PERIODO_TRAB();
                        perTrab = rhController.ObterFuncionarioPeriodoTrab(Convert.ToInt32(codigoPerTrabFuncionario));
                        if (perTrab == null)
                        {
                            int _index = _perTrabFun.FindIndex(p => p.RH_PONTO_PERIODO_TRAB1.DESCRICAO.Trim() == hidDescricaoPeriodo.Value.Trim());
                            _perTrabFun.RemoveAt(_index);
                        }
                        else
                        {
                            rhController.ExcluirFuncionarioPeriodoTrab(codigoPerTrabFuncionario);
                        }

                        perTrab = new RH_FUNCIONARIO_PONTO_PERIODO_TRAB();
                        perTrab.RH_FUNCIONARIO = codigoFuncionario;
                        perTrab.RH_PONTO_PERIODO_TRAB = Convert.ToInt32(_ddlPeriodo.SelectedValue.Trim());
                        perTrab.RH_PONTO_PERIODO_TRAB1 = rhController.ObterPeriodoTrabalho(Convert.ToInt32(_ddlPeriodo.SelectedValue.Trim()));
                        _perTrabFun.Add(perTrab);

                        Session["PERIODO_TRABALHO"] = _perTrabFun;
                        CarregarPeriodoTrabalho(codigoFuncionario);

                        ImageButton ibtSair = row.FindControl("btSairPeriodo") as ImageButton;
                        btSairPeriodo_Click(ibtSair, null);
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btSalvarPeriodo_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btSairPeriodo_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {
                        gvPeriodoTrabalho.EditIndex = -1;

                        int codigoFuncionario = 0;

                        if (hidCodigoFuncionario.Value != "")
                            codigoFuncionario = Convert.ToInt32(hidCodigoFuncionario.Value);
                        CarregarPeriodoTrabalho(codigoFuncionario);

                        //Botão acionado é escondido
                        bt.Visible = false;

                        hidDescricaoPeriodo.Value = "";

                        ImageButton _btEditarPeriodo = row.FindControl("btEditarPeriodo") as ImageButton;
                        if (_btEditarPeriodo != null)
                            _btEditarPeriodo.Visible = true;

                        ImageButton _btSalvarPeriodo = row.FindControl("btSalvarPeriodo") as ImageButton;
                        if (_btSalvarPeriodo != null)
                            _btSalvarPeriodo.Visible = false;

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btSairPeriodo_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btExcluirPeriodo_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {
                        int codigoFuncionario = 0;
                        string codigoPerTrabFuncionario = "";
                        string periodoTrabalho = "";

                        #region "Validação de Campos"
                        Label _labPeriodo = row.FindControl("labPeriodo") as Label;
                        if (_labPeriodo == null)
                        {
                            DropDownList _ddlPeriodo = row.FindControl("_ddlPeriodo") as DropDownList;
                            periodoTrabalho = _ddlPeriodo.SelectedItem.Text.Trim();
                        }
                        else
                        {
                            periodoTrabalho = _labPeriodo.Text.Trim();
                        }
                        #endregion

                        RH_FUNCIONARIO_PONTO_PERIODO_TRAB perFunc = null;

                        if (hidCodigoFuncionario.Value != "")
                            codigoFuncionario = Convert.ToInt32(hidCodigoFuncionario.Value);

                        codigoPerTrabFuncionario = gvPeriodoTrabalho.DataKeys[row.RowIndex].Value.ToString();

                        if (codigoFuncionario > 0 && codigoPerTrabFuncionario != "")
                            perFunc = rhController.ObterFuncionarioPeriodoTrab(Convert.ToInt32(codigoPerTrabFuncionario));
                        if (perFunc == null)
                        {
                            List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> _listaPeriodo = new List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>();
                            if (Session["PERIODO_TRABALHO"] != null)
                                _listaPeriodo = (List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>)Session["PERIODO_TRABALHO"];

                            int _index = _listaPeriodo.FindIndex(p => p.RH_PONTO_PERIODO_TRAB1.DESCRICAO.Trim() == periodoTrabalho);
                            _listaPeriodo.RemoveAt(_index);

                            Session["PERIODO_TRABALHO"] = _listaPeriodo;
                        }
                        else
                        {
                            rhController.ExcluirFuncionarioPeriodoTrab(perFunc.CODIGO);
                        }

                        gvPeriodoTrabalho.EditIndex = -1;
                        CarregarPeriodoTrabalho(codigoFuncionario);

                        hidDescricaoPeriodo.Value = "";
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btExcluirPeriodo_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }

        }
        protected void btIncluirPeriodo_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    try
                    {
                        int codigoFuncionario = 0;

                        if (hidCodigoFuncionario.Value != "")
                            codigoFuncionario = Convert.ToInt32(hidCodigoFuncionario.Value);

                        string PONTO_PERIODO_TRAB = "";
                        string descPeriodo = "";
                        DropDownList _ddlPeriodoFooter = row.FindControl("ddlPeriodoFooter") as DropDownList;

                        if (_ddlPeriodoFooter != null && _ddlPeriodoFooter.SelectedValue.Trim() != "")
                        {
                            PONTO_PERIODO_TRAB = _ddlPeriodoFooter.SelectedValue;
                            descPeriodo = _ddlPeriodoFooter.SelectedItem.Text.Trim();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Selecione um PERÍODO.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if (!ValidarPeriodoTrabalho(descPeriodo))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Este PERÍODO Já foi inserido.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> periodos = new List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>();
                        RH_FUNCIONARIO_PONTO_PERIODO_TRAB perFunc = new RH_FUNCIONARIO_PONTO_PERIODO_TRAB();
                        perFunc.RH_PONTO_PERIODO_TRAB = Convert.ToInt32(PONTO_PERIODO_TRAB);
                        perFunc.RH_PONTO_PERIODO_TRAB1 = rhController.ObterPeriodoTrabalho(Convert.ToInt32(PONTO_PERIODO_TRAB));

                        if (Session["PERIODO_TRABALHO"] != null)
                            periodos = (List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>)Session["PERIODO_TRABALHO"];

                        periodos.Add(perFunc);
                        Session["PERIODO_TRABALHO"] = periodos;

                        //Carregar Periodos
                        CarregarPeriodoTrabalho(codigoFuncionario);

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btIncluirPeriodo_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        #endregion

        #region "TODOS"
        private void CarregarFuncionarioTodos()
        {
            var _funcionarios = rhController.ObterFuncionario(null, txtVendedorCodigo.Text.Trim(), ddlFilial.SelectedValue, txtCPF.Text.Trim(), txtNome.Text.Trim());

            if (ddlFilial.SelectedValue.Trim() != "")
                _funcionarios = _funcionarios.Where(p => p.CODIGO_FILIAL.Trim() == ddlFilial.SelectedValue.Trim()).ToList();

            if (txtNome.Text.Trim() != "")
                _funcionarios = _funcionarios.Where(p => p.NOME.Trim().ToUpper().Contains(txtNome.Text.Trim().ToUpper())).ToList();

            if (txtVendedorApelido.Text.Trim() != "")
                _funcionarios = _funcionarios.Where(p => p.VENDEDOR_APELIDO.Trim().ToUpper().Contains(txtVendedorApelido.Text.Trim().ToUpper())).ToList();

            if (txtCPF.Text.Trim() != "")
                _funcionarios = _funcionarios.Where(p => p.CPF.Trim().ToUpper().Contains(txtCPF.Text.Trim().ToUpper())).ToList();

            if (ddlPeriodoExp.SelectedValue.Trim() != "" && ddlPeriodoExp.SelectedValue.Trim() != "0")
                _funcionarios = _funcionarios.Where(p => p.RH_FUNCIONARIO_PERIODO_EXP.ToString() == ddlPeriodoExp.SelectedValue).ToList();

            if (ddlStatus.SelectedValue.Trim() != "")
            {
                if (ddlStatus.SelectedValue.Trim() == "A") //ATIVOS
                    _funcionarios = _funcionarios.Where(p => p.DATA_DEMISSAO == null).ToList();
                else //INATIVOS
                    _funcionarios = _funcionarios.Where(p => p.DATA_DEMISSAO != null).ToList();

            }

            gvTodos.DataSource = _funcionarios.OrderBy(o => o.NOME);
            gvTodos.DataBind();
        }
        protected void gvTodos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FUNCIONARIOResult _funcionario = e.Row.DataItem as SP_OBTER_FUNCIONARIOResult;

                    colunaTodos += 1;
                    if (_funcionario != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaTodos.ToString();

                        Label _labFilial = e.Row.FindControl("labFilial") as Label;
                        if (_labFilial != null)
                            _labFilial.Text = new BaseController().ObterFilialIntranet(_funcionario.CODIGO_FILIAL).FILIAL;

                        Label _labCPF = e.Row.FindControl("labCPF") as Label;
                        if (_labCPF != null)
                        {
                            if (_funcionario.CPF != null && _funcionario.CPF.Trim() != "")
                            {
                                if (_funcionario.CPF.Trim().Length == 11)
                                    _labCPF.Text = Convert.ToUInt64(_funcionario.CPF.Trim()).ToString(@"000\.000\.000\-00");
                                else
                                    _labCPF.Text = Convert.ToUInt64(_funcionario.CPF.Trim()).ToString(@"00\.000\.000\/0000\-00");
                            }
                        }

                        Label _labCARGO = e.Row.FindControl("labCARGO") as Label;
                        if (_labCARGO != null)
                            if (_funcionario.DESC_CARGO_INTRA != null)
                                _labCARGO.Text = _funcionario.DESC_CARGO_INTRA.Trim();

                        ImageButton _ibtPesquisar = e.Row.FindControl("btPesquisar") as ImageButton;
                        if (_ibtPesquisar != null)
                        {
                            _ibtPesquisar.CommandArgument = _funcionario.CODIGO.ToString();
                        }
                    }
                }
            }
        }
        protected void btPesquisarTodos_Click(object sender, EventArgs e)
        {
            ImageButton ibt = (ImageButton)sender;
            if (ibt != null)
            {
                try
                {
                    string codigoFuncionario = ibt.CommandArgument;
                    hidCodigoFuncionario.Value = codigoFuncionario;

                    ibtPesquisar_Click(null, null);

                    MoverAba("0");
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }
        #endregion

        #region "VALIDACAO"
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

            labNome.ForeColor = _OK;
            if (txtNome.Text.Trim() == "")
            {
                labNome.ForeColor = _notOK;
                retorno = false;
            }

            labCPF.ForeColor = _OK;
            if (txtCPF.Text.Trim() == "")
            {
                labCPF.ForeColor = _notOK;
                retorno = false;
            }

            labVendedorApelido.ForeColor = _OK;
            if (txtVendedorApelido.Text.Trim() == "")
            {
                labVendedorApelido.ForeColor = _notOK;
                retorno = false;
            }

            labCargo.ForeColor = _OK;
            if (ddlCargo.SelectedValue.Trim() == "Selecione")
            {
                labCargo.ForeColor = _notOK;
                retorno = false;
            }

            labComissao.ForeColor = _OK;
            if (txtComissao.Text.Trim() == "")
            {
                labComissao.ForeColor = _notOK;
                retorno = false;
            }

            labSexo.ForeColor = _OK;
            if (ddlSexo.SelectedValue.Trim() == "")
            {
                labSexo.ForeColor = _notOK;
                retorno = false;
            }

            labPeriodoExp.ForeColor = _OK;
            if (ddlPeriodoExp.SelectedValue.Trim() == "0")
            {
                labPeriodoExp.ForeColor = _notOK;
                retorno = false;
            }

            labDataAdmissao.ForeColor = _OK;
            if (txtDataAdmissao.Text.Trim() == "")
            {
                labDataAdmissao.ForeColor = _notOK;
                retorno = false;
            }

            /*labDataDemissao.ForeColor = _OK;
            if (txtDataDemissao.Text.Trim() == "")
            {
                labDataDemissao.ForeColor = _notOK;
                retorno = false;
            }*/

            labCidade.ForeColor = _OK;
            if (txtCidade.Text.Trim() == "")
            {
                labCidade.ForeColor = _notOK;
                retorno = false;
            }

            labUF.ForeColor = _OK;
            if (txtUF.Text.Trim() == "")
            {
                labUF.ForeColor = _notOK;
                retorno = false;
            }

            labPeriodoTrabalho.ForeColor = _OK;
            if (gvPeriodoTrabalho.Rows.Count > 0)
            {
                Label _labPeriodo = gvPeriodoTrabalho.Rows[0].FindControl("labPeriodo") as Label;
                if (_labPeriodo.Text.Trim() == "")
                {
                    labPeriodoTrabalho.ForeColor = _notOK;
                    retorno = false;
                }
            }

            return retorno;
        }
        private bool ValidarPeriodoTrabalho(string descPeriodo)
        {
            bool retorno = true;

            foreach (GridViewRow row in gvPeriodoTrabalho.Rows)
            {
                if (row != null)
                {
                    Label _labPeriodo = row.FindControl("labPeriodo") as Label;
                    if (_labPeriodo != null)
                    {
                        if (_labPeriodo.Text.Trim() == descPeriodo.Trim())
                        {
                            retorno = false;
                            break;
                        }
                    }
                }
            }

            return retorno;
        }
        private void HabilitarCampos(bool enable)
        {
            ddlFilial.Enabled = enable;
            txtNome.Enabled = enable;
            txtCPF.Enabled = enable;
            txtVendedorCodigo.Enabled = enable;
            ddlStatus.Enabled = enable;

            txtVendedorApelido.Enabled = enable;
            ddlCargo.Enabled = enable;
            txtComissao.Enabled = enable;
            txtRG.Enabled = enable;
            ddlSexo.Enabled = enable;
            ddlPeriodoExp.Enabled = enable;
            txtDataAdmissao.Enabled = enable;
            txtDataDemissao.Enabled = enable;
            txtTelefone.Enabled = enable;
            txtEndereco.Enabled = enable;
            txtComplemento.Enabled = enable;
            txtCidade.Enabled = enable;
            txtUF.Enabled = enable;
            txtCEP.Enabled = enable;
            cbBatePonto.Enabled = enable;
            cbOperaCaixa.Enabled = enable;
            cbAcessoGerencial.Enabled = enable;
            cbGerenteLoja.Enabled = enable;
            cbBatePonto.Enabled = enable;

            foreach (GridViewRow row in gvPeriodoTrabalho.Rows)
                if (row != null)
                {
                    ImageButton _btEditarPeriodo = row.FindControl("btEditarPeriodo") as ImageButton;
                    if (_btEditarPeriodo != null)
                        _btEditarPeriodo.Visible = enable;
                    ImageButton _btExcluirPeriodo = row.FindControl("btExcluirPeriodo") as ImageButton;
                    if (_btExcluirPeriodo != null)
                        _btExcluirPeriodo.Visible = enable;
                }
        }
        private void LimparCampos()
        {
            hidCodigoFuncionario.Value = "";

            ddlFilial.SelectedValue = "";
            txtNome.Text = "";
            txtCPF.Text = "";
            txtVendedorCodigo.Text = "";
            ddlStatus.SelectedValue = "A";
            labErro.Text = "";

            //FUNCIONARIO
            txtVendedorApelido.Text = "";
            ddlCargo.SelectedValue = "Selecione";
            txtComissao.Text = "";
            txtRG.Text = "";
            ddlSexo.SelectedValue = "";
            ddlPeriodoExp.SelectedValue = "0";
            txtDataAdmissao.Text = "";
            txtDataDemissao.Text = "";
            txtTelefone.Text = "";
            txtEndereco.Text = "";
            txtComplemento.Text = "";
            txtCidade.Text = "";
            txtUF.Text = "";
            txtCEP.Text = "";
            imgFoto.ImageUrl = "~/Image/fun_sfoto.jpg";
            cbBatePonto.Checked = false;
            cbOperaCaixa.Checked = false;
            cbAcessoGerencial.Checked = false;
            cbGerenteLoja.Checked = false;
            cbBatePonto.Checked = false;
        }
        #endregion

        protected void ddlCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCargo.SelectedValue.Trim() == "VENDEDOR" || ddlCargo.SelectedValue.Trim() == "VR") //ATIVOS
                txtComissao.Text = "4,00000";
            else //INATIVOS
                txtComissao.Text = "0,00000";

        }

        protected void imgFoto_Click(object sender, ImageClickEventArgs e)
        {
            string url = "";

            try
            {
                if (hidCodigoFuncionario.Value != "" && hidCodigoFuncionario.Value != "0")
                {
                    var codigoFuncionario = hidCodigoFuncionario.Value;

                    //Abrir pop-up
                    url = "fnAbrirTelaCadastro('rh_cad_funcionario_foto.aspx?fot=" + codigoFuncionario + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
                }
            }
            catch (Exception ex)
            {
                //labErro.Text = ex.Message;
            }
        }
        protected void imgFotoAtualizar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (hidCodigoFuncionario.Value != "")
                {
                    ibtPesquisar_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                //labErro.Text = ex.Message;
            }
        }
    }
}