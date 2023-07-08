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
    public partial class prod_cad_fornecedor : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3" && tela != "4")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "prod_menu.aspx";

                if (tela == "3")
                    hrefVoltar.HRef = "../mod_fiscal/fisc_menu.aspx";

                if (tela == "4")
                    hrefVoltar.HRef = "../mod_contabilidade/contabil_menu.aspx";

                //Carregar combos
                CarregarFornecedor();

                //Carregar gv de Fornecedores
                CarregarFornecedores();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarFornecedor()
        {
            List<SP_OBTER_FORNECEDORESResult> _fornecedores = prodController.ObterFornecedores("T");
            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new SP_OBTER_FORNECEDORESResult { FORNECEDOR = "Selecione" });

                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }
        }
        private void CarregarFornecedores()
        {
            List<PROD_FORNECEDOR> _fornecedor = new List<PROD_FORNECEDOR>();

            try
            {
                _fornecedor = prodController.ObterFornecedor().OrderBy(p => p.FORNECEDOR).ToList();
                if (_fornecedor != null)
                {
                    gvFornecedor.DataSource = _fornecedor;
                    gvFornecedor.DataBind();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (gvFornecedor): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labFornecedor.ForeColor = Color.Black;
            labICMS.ForeColor = Color.Black;
            labTipo.ForeColor = Color.Black;
            labEmail.ForeColor = Color.Black;
            labStatus.ForeColor = Color.Black;
            btCancelar.Visible = false;
            ddlFornecedor.SelectedValue = "Selecione";
            txtICMS.Text = "";
            ddlTipo.SelectedValue = "";
            ddlStatus.SelectedValue = "A";
            txtEmail.Text = "";

            CarregarFornecedores();
        }
        #endregion

        #region "CRUD"
        private void Incluir(PROD_FORNECEDOR _fornecedor)
        {
            prodController.InserirFornecedor(_fornecedor);
        }
        private void Editar(PROD_FORNECEDOR _fornecedor)
        {
            prodController.AtualizarFornecedor(_fornecedor);
        }
        private void Excluir(int _codigo)
        {
            prodController.ExcluirFornecedor(_codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            labErro.Text = "";
            if (ddlFornecedor.SelectedValue.Trim() == "Selecione" || ddlFornecedor.SelectedValue.Trim() == "")
            {
                labErro.Text = "Selecione o Fornecedor.";
                return;
            }

            if (txtICMS.Text.Trim() == "")
            {
                labErro.Text = "Informe a % do ICMS.";
                return;
            }

            if (ddlTipo.SelectedValue.Trim() == "0" || ddlTipo.SelectedValue.Trim() == "")
            {
                labErro.Text = "Selecione o TIPO.";
                return;
            }

            if (ddlStatus.SelectedValue.Trim() == "0")
            {
                labErro.Text = "Selecione o Status.";
                return;
            }

            string email = txtEmail.Text;
            if (email != "")
            {
                email = email.Replace(",", ";");
                if (!Utils.WebControls.ValidarEmail(email))
                {
                    labErro.Text = "Informe E-Mail válido. (Exemplo: fornecedor@hbf.com.br)";
                    return;
                }
            }


            try
            {
                _Inclusao = true;
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    _Inclusao = false;

                PROD_FORNECEDOR _novo = new PROD_FORNECEDOR();
                _novo.FORNECEDOR = ddlFornecedor.SelectedValue.ToUpper();
                _novo.PORC_ICMS = Convert.ToDecimal(txtICMS.Text.Trim());
                _novo.TIPO = Convert.ToChar(ddlTipo.SelectedValue.Trim().ToString());
                _novo.STATUS = Convert.ToChar(ddlStatus.Text);
                _novo.EMAIL = txtEmail.Text.Trim();

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
                    PROD_FORNECEDOR _fornecedor = prodController.ObterFornecedor(Convert.ToInt32(b.CommandArgument));
                    if (_fornecedor != null)
                    {

                        hidCodigo.Value = _fornecedor.CODIGO.ToString();

                        ddlFornecedor.SelectedValue = _fornecedor.FORNECEDOR.ToString();
                        txtICMS.Text = _fornecedor.PORC_ICMS.ToString();
                        ddlTipo.SelectedValue = _fornecedor.TIPO.ToString();
                        ddlStatus.SelectedValue = _fornecedor.STATUS.ToString();
                        txtEmail.Text = _fornecedor.EMAIL;

                        labFornecedor.ForeColor = Color.Red;
                        labICMS.ForeColor = Color.Red;
                        labTipo.ForeColor = Color.Red;
                        labEmail.ForeColor = Color.Red;
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
                        labErro.Text = "O Fornecedor não pode ser excluído. Este Fornecedor já está cadastrado em um Produto.";
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
        protected void gvFornecedor_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_FORNECEDOR _fornecedor = e.Row.DataItem as PROD_FORNECEDOR;

                    coluna += 1;
                    if (_fornecedor != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litICMS = e.Row.FindControl("litICMS") as Literal;
                        if (_litICMS != null)
                            _litICMS.Text = _fornecedor.PORC_ICMS.ToString("###,##0.00") + "%";

                        Literal _litTipo = e.Row.FindControl("litTipo") as Literal;
                        if (_litTipo != null)
                        {
                            string tipo = "";

                            if (_fornecedor.TIPO == 'A')
                                tipo = "Aviamento";
                            else if (_fornecedor.TIPO == 'T')
                                tipo = "Tecido";
                            else if (_fornecedor.TIPO == 'S')
                                tipo = "Serviço";
                            else if (_fornecedor.TIPO == 'C')
                                tipo = "Acessório";

                            _litTipo.Text = tipo;
                        }

                        Literal _status = e.Row.FindControl("litStatus") as Literal;
                        if (_status != null)
                        {
                            if (_fornecedor.STATUS == 'A')
                                _status.Text = "Ativo";
                            else
                                _status.Text = "Inativo";
                        }

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _fornecedor.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _fornecedor.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
