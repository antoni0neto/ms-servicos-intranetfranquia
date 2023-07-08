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
    public partial class contabil_cad_fornecedor_conta : System.Web.UI.Page
    {
        ContabilidadeController contabilController = new ContabilidadeController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarFornecedores();
                CarregarContaContabil();
                CarregarFornecedoresConta();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarFornecedores()
        {
            List<SP_OBTER_FORNECEDORESResult> _fornecedores = new ProducaoController().ObterFornecedores("D");
            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new SP_OBTER_FORNECEDORESResult { CLIFOR = "Selecione", FORNECEDOR = "Selecione" });

                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }
        }
        private void CarregarContaContabil()
        {
            ddlContaContabil.DataSource = contabilController.ObterContasFiltro("3.%", "1.1.5.01%", "1.2.3.01%", "1.2.4.01%", "1.2.3.05%", "1.1.5.03%");

            ddlContaContabil.DataBind();
            ddlContaContabil.Items.Insert(0, new ListItem("Selecione", "Selecione"));
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labFornecedor.ForeColor = Color.Black;
            labContaContabil.ForeColor = Color.Black;

            btCancelar.Visible = false;
            ddlFornecedor.SelectedValue = "Selecione";
            ddlContaContabil.SelectedValue = "Selecione";

            labErro.Text = "";

            CarregarFornecedoresConta();
        }
        #endregion

        #region "CRUD"
        private void Incluir(CTB_FORNECEDOR_CONTA _fornecedorConta)
        {
            contabilController.InserirFornecedorConta(_fornecedorConta);
        }
        private void Editar(CTB_FORNECEDOR_CONTA _fornecedorConta)
        {
            contabilController.AtualizarFornecedorConta(_fornecedorConta);
        }
        private void Excluir(int _codigo)
        {
            contabilController.ExcluirFornecedorConta(_codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            labErro.Text = "";
            if (ddlFornecedor.SelectedValue == "Selecione")
            {
                labErro.Text = "Selecione o Fornecedor.";
                return;
            }

            if (ddlContaContabil.SelectedValue.Trim() == "Selecione")
            {
                labErro.Text = "Selecione a Conta Contábil.";
                return;
            }

            var _valida = contabilController.ObterFornecedorConta(ddlFornecedor.SelectedValue).Where(p =>
                                                                                    p.CONTA_CONTABIL.Trim() == ddlContaContabil.SelectedValue.Trim() &&
                                                                                    p.DATA_EXCLUSAO == null).SingleOrDefault();

            if (_valida != null)
            {
                if ((hidCodigo.Value == "") || (_valida.CODIGO != Convert.ToInt32(hidCodigo.Value)))
                {
                    labErro.Text = "Este Fornecedor já está cadastrado para esta Conta.";
                    return;
                }
            }

            try
            {
                _Inclusao = true;
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    _Inclusao = false;

                CTB_FORNECEDOR_CONTA _novo = new CTB_FORNECEDOR_CONTA();
                _novo.CLIFOR = ddlFornecedor.SelectedValue;
                _novo.FORNECEDOR = ddlFornecedor.SelectedItem.Text.ToUpper();
                _novo.CONTA_CONTABIL = ddlContaContabil.SelectedValue;
                _novo.DATA_ALTERACAO = DateTime.Now;
                _novo.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

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

                    CTB_FORNECEDOR_CONTA _fornecedorConta = contabilController.ObterFornecedorConta(Convert.ToInt32(b.CommandArgument));
                    if (_fornecedorConta != null)
                    {

                        hidCodigo.Value = _fornecedorConta.CODIGO.ToString();

                        ddlFornecedor.SelectedValue = _fornecedorConta.CLIFOR;
                        ddlContaContabil.SelectedValue = _fornecedorConta.CONTA_CONTABIL;

                        labFornecedor.ForeColor = Color.Red;
                        labContaContabil.ForeColor = Color.Red;
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
                        CTB_FORNECEDOR_CONTA _fornecedorConta = contabilController.ObterFornecedorConta(Convert.ToInt32(b.CommandArgument));
                        if (_fornecedorConta != null)
                        {

                            _fornecedorConta.DATA_EXCLUSAO = DateTime.Now;
                            _fornecedorConta.USUARIO_EXCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                            Editar(_fornecedorConta);
                            //Excluir(Convert.ToInt32(b.CommandArgument));
                            RecarregarTela();
                        }
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
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            RecarregarTela();
        }
        #endregion

        private void CarregarFornecedoresConta()
        {
            try
            {
                var fornecedorConta = contabilController.ObterFornecedorConta().Where(p => p.DATA_EXCLUSAO == null).OrderBy(p => p.FORNECEDOR).ThenBy(o => o.CONTA_CONTABIL).ToList();

                gvFornecedorConta.DataSource = fornecedorConta;
                gvFornecedorConta.DataBind();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void gvFornecedorConta_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CTB_FORNECEDOR_CONTA fornecedorConta = e.Row.DataItem as CTB_FORNECEDOR_CONTA;

                    if (fornecedorConta != null)
                    {

                        Literal litContaContabil = e.Row.FindControl("litContaContabil") as Literal;
                        var cc = contabilController.ObterContaDireto(fornecedorConta.CONTA_CONTABIL);
                        litContaContabil.Text = cc.CONTA_CONTABIL.Trim() + " - " + cc.DESC_CONTA.Trim();


                        Button btAlerar = e.Row.FindControl("btAlterar") as Button;
                        btAlerar.CommandArgument = fornecedorConta.CODIGO.ToString();

                        Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                        btExcluir.CommandArgument = fornecedorConta.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
