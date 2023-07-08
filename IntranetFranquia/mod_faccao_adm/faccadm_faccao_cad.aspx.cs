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

namespace Relatorios.mod_desenvolvimento
{
    public partial class faccadm_faccao_cad : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();

        const string cAbaTodos = "2";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;
                ibtEditar.Visible = false;
                ibtLimpar.Visible = false;

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);
        }

        #region "DADOS INICIAIS"

        #endregion

        #region "AÇÕES DA TELA"
        protected void ibtEditar_Click(object sender, EventArgs e)
        {
            try
            {
                //controle de visibilidade dos botões da tela
                ibtEditar.Visible = false;
                ibtPesquisar.Visible = false;
                ibtLimpar.Visible = false;

                ibtSalvar.Visible = true;
                ibtCancelar.Visible = true;

                labErro.Text = "";
                HabilitarCampos(true);
                txtFaccao.Enabled = false;
                hidAcao.Value = "A";
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

                string nomeCliFor = txtFaccao.Text.ToUpper().Trim();
                string cgcCPF = txtCNPJCPF.Text.Trim();

                var faccaoCadastro = faccController.ObterFaccaoCadastro(nomeCliFor, "");
                if (faccaoCadastro != null && faccaoCadastro.Count() == 1)
                {

                    CarregarFaccao(faccaoCadastro[0]);
                    CarregarTipoDocumento(nomeCliFor);
                    CarregarSocios(nomeCliFor);

                    ibtPesquisar.Visible = false;
                    ibtSalvar.Visible = false;
                    ibtCancelar.Visible = false;
                    ibtEditar.Visible = true;
                    ibtLimpar.Visible = true;

                    hidAcao.Value = "C";

                    HabilitarCampos(false);

                    MoverAba("0");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + cAbaTodos + "');", true);
                    hidTabSelected.Value = cAbaTodos;
                    CarregarTodos(nomeCliFor, cgcCPF);
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
                string nomeCliFor = txtFaccao.Text.Trim().ToUpper();

                labErro.Text = "";

                ibtPesquisar.Visible = true;
                ibtEditar.Visible = false;
                ibtLimpar.Visible = false;
                hidAcao.Value = "";

                HabilitarCampos(true);
                LimparCampos();

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

                //controle de visibilidade dos botões da tela
                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;

                System.Drawing.Color _OK = System.Drawing.Color.Gray;
                //labMaterialDescricao.ForeColor = _OK;

                if (hidAcao.Value == "I")
                {
                    ibtLimpar_Click(null, null);
                    txtFaccao.Text = "";
                    txtFaccao.Enabled = true;

                    ibtPesquisar.Visible = true;
                }

                if (hidAcao.Value == "A")
                {
                    ibtPesquisar_Click(null, null);
                    ibtPesquisar.Visible = false;
                    ibtEditar.Visible = true;
                    ibtLimpar.Visible = true;
                }

                hidAcao.Value = "";
                LimparCamposSocio();
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
        private void CarregarFaccao(SP_OBTER_FACCAO_CADASTROResult faccaoCadastro)
        {
            txtRazaoSocial.Text = faccaoCadastro.RAZAO_SOCIAL.Trim();
            txtCNPJCPF.Text = faccaoCadastro.CGC_CPF.Trim();
            txtOrgSocietaria.Text = faccaoCadastro.COMP_SOCIETARIA;
            txtInscricaoEstadual.Text = faccaoCadastro.RG_IE;
            txtQtdeFuncionario.Text = faccaoCadastro.QTDE_FUN;
            txtEmail.Text = faccaoCadastro.EMAIL.Trim();
            txtEndereco.Text = faccaoCadastro.ENDERECO.Trim();
            txtComplemento.Text = faccaoCadastro.COMPLEMENTO.Trim();
            txtCidade.Text = faccaoCadastro.CIDADE.Trim();
            txtUF.Text = faccaoCadastro.UF;
            txtCEP.Text = faccaoCadastro.CEP.Trim();
        }

        #endregion

        #region "TIPOS DE DOCUMENTO"
        private void CarregarTipoDocumento(string nomeCliFor)
        {
            var faccaoCadastroDoc = faccController.ObterFaccaoCadastroDoc(nomeCliFor);

            gvTipoDocumento.DataSource = faccaoCadastroDoc;
            gvTipoDocumento.DataBind();
        }
        protected void gvTipoDocumento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_CADASTRO_DOCResult _faccaoCadastroDoc = e.Row.DataItem as SP_OBTER_FACCAO_CADASTRO_DOCResult;

                    if (_faccaoCadastroDoc != null)
                    {
                        CheckBox _cbObrigatorio = e.Row.FindControl("cbObrigatorio") as CheckBox;
                        if (_cbObrigatorio != null)
                        {
                            if (_faccaoCadastroDoc.OBRIGATORIO == 'S')
                                _cbObrigatorio.Checked = true;
                        }

                    }
                }
            }

        }

        protected void cbObrigatorio_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _cbObrigatorio = (CheckBox)sender;
            if (_cbObrigatorio != null)
            {
                string nomeCliFor = txtFaccao.Text.Trim().ToUpper();
                GridViewRow row = (GridViewRow)_cbObrigatorio.NamingContainer;
                if (row != null)
                {
                    int codigoTipoDocumento = Convert.ToInt32(gvTipoDocumento.DataKeys[row.RowIndex].Value.ToString());

                    var tipoDocumentoFaccao = faccController.ObterFaccaoTipoDocumento(nomeCliFor, codigoTipoDocumento);
                    if (tipoDocumentoFaccao == null)
                    {
                        FACC_FACCAO_TIPO_DOCUMENTO faccaoTipoDocumento = new FACC_FACCAO_TIPO_DOCUMENTO();
                        faccaoTipoDocumento.NOME_CLIFOR = nomeCliFor;
                        faccaoTipoDocumento.FACC_TIPO_DOCUMENTO = codigoTipoDocumento;
                        faccaoTipoDocumento.OBRIGATORIO = (_cbObrigatorio.Checked) ? 'S' : 'N';
                        faccController.InserirFaccaoTipoDocumento(faccaoTipoDocumento);
                    }
                    else
                    {
                        tipoDocumentoFaccao.OBRIGATORIO = (_cbObrigatorio.Checked) ? 'S' : 'N';
                        faccController.AtualizarFaccaoTipoDocumento(tipoDocumentoFaccao);
                    }

                }
            }
        }
        #endregion

        #region "SÓCIOS"
        private void CarregarSocios(string nomeCliFor)
        {
            var faccaoSocio = faccController.ObterFaccaoSocio(nomeCliFor);
            gvSocio.DataSource = faccaoSocio;
            gvSocio.DataBind();
        }
        protected void btSocioIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                string nomeCliFor = txtFaccao.Text.Trim().ToUpper();
                bool inserir = false;

                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos do Sócio corretamente.";
                    return;
                }

                if (!Utils.WebControls.ValidarCPF(txtSocioCPF.Text.Trim()))
                {
                    labErro.Text = "CPF do Sócio informado está Incorreto. Informe um CPF válido.";
                    return;
                }

                FACC_SOCIO _faccaoSocio = null;
                _faccaoSocio = faccController.ObterFaccaoSocio(Convert.ToInt32(hidCodigoSocio.Value));
                if (_faccaoSocio == null)
                {
                    inserir = true;
                    _faccaoSocio = new FACC_SOCIO();
                }

                _faccaoSocio.NOME_CLIFOR = nomeCliFor;
                _faccaoSocio.NOME = txtSocioNome.Text.Trim().ToUpper();
                _faccaoSocio.CNPJCPF = txtSocioCPF.Text.Trim().ToUpper();
                _faccaoSocio.TELEFONE = txtSocioTelefone.Text.Trim();
                _faccaoSocio.EMAIL = txtSocioEmail.Text.Trim().ToUpper();
                _faccaoSocio.DATA_INCLUSAO = DateTime.Now;
                _faccaoSocio.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _faccaoSocio.STATUS = 'A';

                if (inserir)
                    faccController.InserirFaccaoSocio(_faccaoSocio);
                else
                    faccController.AtualizarFaccaoSocio(_faccaoSocio);

                CarregarSocios(nomeCliFor);

                LimparCamposSocio();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluirSocio_Click). \n\n" + ex.Message;
            }
        }
        protected void btSocioEditar_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            if (b != null)
            {
                try
                {
                    int codigoSocio = Convert.ToInt32(b.CommandArgument);

                    hidCodigoSocio.Value = codigoSocio.ToString();

                    var faccaoSocio = faccController.ObterFaccaoSocio(codigoSocio);
                    if (faccaoSocio != null)
                    {
                        txtSocioNome.Text = faccaoSocio.NOME;
                        txtSocioCPF.Text = faccaoSocio.CNPJCPF;
                        txtSocioTelefone.Text = faccaoSocio.TELEFONE;
                        txtSocioEmail.Text = faccaoSocio.EMAIL;
                    }

                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btSocioEditar_Click). \n\n" + ex.Message;
                }
            }
        }
        protected void btSocioExcluir_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            if (b != null)
            {
                try
                {
                    int codigoSocio = Convert.ToInt32(b.CommandArgument);
                    faccController.ExcluirFaccaoSocio(codigoSocio);

                    CarregarSocios(txtFaccao.Text.Trim().ToUpper());
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btSocioExcluir_Click). \n\n" + ex.Message;
                }
            }
        }
        protected void gvSocio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    FACC_SOCIO _socio = e.Row.DataItem as FACC_SOCIO;

                    if (_socio != null)
                    {

                        Literal _litCPF = e.Row.FindControl("litCPF") as Literal;
                        if (_socio.CNPJCPF != null && _socio.CNPJCPF.Trim() != "")
                        {
                            if (_socio.CNPJCPF.Trim().Length == 11)
                                _litCPF.Text = Convert.ToUInt64(_socio.CNPJCPF.Trim()).ToString(@"000\.000\.000\-00");
                            else
                                _litCPF.Text = Convert.ToUInt64(_socio.CNPJCPF.Trim()).ToString(@"00\.000\.000\/0000\-00");
                        }

                        ImageButton _btSocioEditar = e.Row.FindControl("btSocioEditar") as ImageButton;
                        if (_btSocioEditar != null)
                            _btSocioEditar.CommandArgument = _socio.CODIGO.ToString();

                        ImageButton _btSocioExcluir = e.Row.FindControl("btSocioExcluir") as ImageButton;
                        if (_btSocioExcluir != null)
                            _btSocioExcluir.CommandArgument = _socio.CODIGO.ToString();

                    }
                }
            }
        }
        #endregion

        #region "TODOS"
        private void CarregarTodos(string nomeCLIFOR, string cgcCPF)
        {
            List<SP_OBTER_FACCAO_CADASTROResult> faccaoCadastro = new List<SP_OBTER_FACCAO_CADASTROResult>();

            faccaoCadastro = faccController.ObterFaccaoCadastro("", "");

            if (txtFaccao.Text.Trim() != "")
                faccaoCadastro = faccaoCadastro.Where(p => p.NOME_CLIFOR.Trim().ToUpper().Contains(txtFaccao.Text.Trim().ToUpper())).OrderBy(p => p.NOME_CLIFOR).ToList();

            if (txtCNPJCPF.Text.Trim() != "")
                faccaoCadastro = faccaoCadastro.Where(p => p.CGC_CPF.Trim().ToUpper().Contains(txtCNPJCPF.Text.Trim().ToUpper())).OrderBy(p => p.NOME_CLIFOR).ToList();

            gvTodos.DataSource = faccaoCadastro;
            gvTodos.DataBind();
        }
        protected void gvTodos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_CADASTROResult _facCAD = e.Row.DataItem as SP_OBTER_FACCAO_CADASTROResult;
                    if (_facCAD != null)
                    {
                        Label _labCGCCPF = e.Row.FindControl("labCGCCPF") as Label;
                        if (_labCGCCPF != null)
                        {
                            if (_facCAD.CGC_CPF != null && _facCAD.CGC_CPF.Trim() != "")
                            {
                                if (_facCAD.CGC_CPF.Trim().Length == 11)
                                    _labCGCCPF.Text = Convert.ToUInt64(_facCAD.CGC_CPF.Trim()).ToString(@"000\.000\.000\-00");
                                else
                                    _labCGCCPF.Text = Convert.ToUInt64(_facCAD.CGC_CPF.Trim()).ToString(@"00\.000\.000\/0000\-00");
                            }
                        }

                        ImageButton _ibtPesquisar = e.Row.FindControl("btPesquisar") as ImageButton;
                        if (_ibtPesquisar != null)
                            _ibtPesquisar.CommandArgument = _facCAD.NOME_CLIFOR;
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
                    string nomeCliFor = ibt.CommandArgument;

                    txtFaccao.Text = nomeCliFor;
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

            labSocioNome.ForeColor = _OK;
            if (txtSocioNome.Text.Trim() == "")
            {
                labSocioNome.ForeColor = _notOK;
                retorno = false;
            }

            labSocioCPF.ForeColor = _OK;
            if (txtSocioCPF.Text.Trim() == "")
            {
                labSocioCPF.ForeColor = _notOK;
                retorno = false;
            }

            labSocioTelefone.ForeColor = _OK;
            if (txtSocioTelefone.Text.Trim() == "")
            {
                labSocioTelefone.ForeColor = _notOK;
                retorno = false;
            }

            labSocioEmail.ForeColor = _OK;
            if (txtSocioEmail.Text.Trim() == "")
            {
                labSocioEmail.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private void HabilitarCampos(bool enable)
        {
            txtFaccao.Enabled = enable;

            txtSocioNome.Enabled = enable;
            txtSocioCPF.Enabled = enable;
            txtSocioTelefone.Enabled = enable;
            txtSocioEmail.Enabled = enable;

            gvTipoDocumento.Enabled = enable;
            gvSocio.Enabled = enable;

            btSocioIncluir.Visible = enable;

        }
        private void LimparCampos()
        {
            labErro.Text = "";

            txtFaccao.Text = "";
            txtRazaoSocial.Text = "";
            txtCNPJCPF.Text = "";
            txtOrgSocietaria.Text = "";
            txtInscricaoEstadual.Text = "";
            txtQtdeFuncionario.Text = "";
            txtEmail.Text = "";
            txtEndereco.Text = "";
            txtComplemento.Text = "";
            txtCidade.Text = "";
            txtUF.Text = "";
            txtCEP.Text = "";

            gvTipoDocumento.DataSource = new List<SP_OBTER_FACCAO_CADASTRO_DOCResult>();
            gvTipoDocumento.DataBind();

            hidCodigoSocio.Value = "0";
            btSocioIncluir.Visible = false;
            gvSocio.DataSource = new List<FACC_SOCIO>();
            gvSocio.DataBind();

        }
        private void LimparCamposSocio()
        {
            hidCodigoSocio.Value = "0";
            txtSocioNome.Text = "";
            txtSocioCPF.Text = "";
            txtSocioTelefone.Text = "";
            txtSocioEmail.Text = "";
        }

        #endregion



    }
}