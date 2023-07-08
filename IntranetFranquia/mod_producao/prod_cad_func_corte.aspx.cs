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
    public partial class prod_cad_func_corte : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Carregar gvCorteFuncionario
                CarregarCorteFuncionario();

                //Controle de Controles
                btCancelar.Visible = false;
                pnlErro.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarCorteFuncionario()
        {
            List<PROD_CORTE_FUNCIONARIO> _corteFunc = new List<PROD_CORTE_FUNCIONARIO>();

            try
            {
                _corteFunc = prodController.ObterCorteFuncionario();
                if (_corteFunc != null)
                {
                    gvCorteFuncionario.DataSource = _corteFunc;
                    gvCorteFuncionario.DataBind();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (CorteFuncionario): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                pnlErro.Visible = true;
                pnlErro.Height = Unit.Pixel(100);
            }
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labNome.ForeColor = Color.Black;
            labStatus.ForeColor = Color.Black;
            btCancelar.Visible = false;
            txtNome.Text = "";
            ddlStatus.SelectedValue = "A";
            pnlErro.Visible = false;

            CarregarCorteFuncionario();
        }

        #endregion

        #region "CRUD"

        private void Incluir(PROD_CORTE_FUNCIONARIO _corteFunc)
        {
            prodController.InserirCorteFuncionario(_corteFunc);
        }
        private void Editar(PROD_CORTE_FUNCIONARIO _corteFunc, string _codigo)
        {
            _corteFunc.CODIGO = Convert.ToInt32(_codigo);
            prodController.AtualizarCorteFuncionario(_corteFunc);
        }
        private void Excluir(int _Codigo)
        {
            prodController.ExcluirCorteFuncionario(_Codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            pnlErro.Visible = false;
            if (txtNome.Text.Trim() == "")
            {
                labErro.Text = "Informe o Nome do Funcionário.";
                pnlErro.Visible = true;
                return;
            }
            if (ddlStatus.Text == "0")
            {
                labErro.Text = "Selecione o Status.";
                pnlErro.Visible = true;
                return;
            }

            try
            {
                _Inclusao = true;
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    _Inclusao = false;

                PROD_CORTE_FUNCIONARIO _novo = new PROD_CORTE_FUNCIONARIO();

                _novo.NOME = txtNome.Text.Trim().ToUpper();
                _novo.STATUS = Convert.ToChar(ddlStatus.Text);

                if (_Inclusao)
                    Incluir(_novo);
                else
                    Editar(_novo, hidCodigo.Value);

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
                    PROD_CORTE_FUNCIONARIO _corteFunc = prodController.ObterCorteFuncionario(Convert.ToInt32(b.CommandArgument));
                    if (_corteFunc != null)
                    {
                        hidCodigo.Value = _corteFunc.CODIGO.ToString();
                        txtNome.Text = _corteFunc.NOME.Trim().ToUpper();
                        ddlStatus.SelectedValue = _corteFunc.STATUS.ToString();
                        labNome.ForeColor = Color.Red;
                        labStatus.ForeColor = Color.Red;
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
                    /*
                    // Validar exclusão
                    if (prodController.ObterCorteFuncionario(Convert.ToInt32(b.CommandArgument)))
                    {
                        labErro.Text = "O Funcionário não pode ser excluído. Este funcionário já está cadastrado em um HB.";
                        pnlErro.Visible = true;
                        return;
                    }
                    */

                    Excluir(Convert.ToInt32(b.CommandArgument));
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
        protected void gvCorteFuncionario_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_CORTE_FUNCIONARIO _corteFunc = e.Row.DataItem as PROD_CORTE_FUNCIONARIO;

                    coluna += 1;
                    if (_corteFunc != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _status = e.Row.FindControl("litStatus") as Literal;
                        if (_status != null)
                        {
                            if (_corteFunc.STATUS == 'A')
                                _status.Text = "Ativo";
                            else
                                _status.Text = "Inativo";
                        }

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                        {
                            _btAlerar.CommandArgument = _corteFunc.CODIGO.ToString();
                        }

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _corteFunc.CODIGO.ToString();
                        }

                    }
                }
            }
        }
    }
}
