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
    public partial class atac_cad_rep_comissao : System.Web.UI.Page
    {
        AtacadoController atacController = new AtacadoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarRepresentante();
                CarregarComissaoRepresentantes();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarRepresentante()
        {
            ddlRepresentante.DataSource = new BaseController().BuscaRepresentanteAtacado().OrderBy(p => p.REPRESENTANTE);
            ddlRepresentante.DataBind();
            ddlRepresentante.Items.Insert(0, new ListItem("Selecione", "Selecione"));
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labRepresentante.ForeColor = Color.Black;
            labFilial.ForeColor = Color.Black;
            labComissão.ForeColor = Color.Black;

            btCancelar.Visible = false;
            ddlRepresentante.SelectedValue = "Selecione";
            ddlFilial.SelectedValue = "Selecione";
            txtComissao.Text = "";

            CarregarComissaoRepresentantes();
        }
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = new BaseController().BuscaFiliais(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").ToList();
                //lstFilial = baseController.BuscaFiliais();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                    }
                }
            }
        }
        #endregion

        #region "CRUD"
        private void Incluir(ATACADO_REP_COMISSAO _comissaoREP)
        {
            atacController.InserirComissaoRep(_comissaoREP);
        }
        private void Editar(ATACADO_REP_COMISSAO _comissaoREP)
        {
            atacController.AtualizarComissaoRep(_comissaoREP);
        }
        private void Excluir(int _codigo)
        {
            atacController.ExcluirComissaoRep(_codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            labErro.Text = "";
            if (ddlRepresentante.SelectedValue == "Selecione")
            {
                labErro.Text = "Selecione o Representante.";
                return;
            }

            if (ddlFilial.SelectedValue.Trim() == "Selecione")
            {
                labErro.Text = "Selecione a Filial.";
                return;
            }

            if (txtComissao.Text.Trim() == "")
            {
                labErro.Text = "Informe a Comissão.";
                return;
            }

            try
            {
                _Inclusao = true;
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    _Inclusao = false;

                ATACADO_REP_COMISSAO _novo = new ATACADO_REP_COMISSAO();
                _novo.REPRESENTANTE = ddlRepresentante.SelectedValue;
                _novo.FILIAL = ddlFilial.SelectedValue;
                _novo.COMISSAO = Convert.ToDecimal(txtComissao.Text);
                _novo.DATA_INCLUSAO = DateTime.Now;
                _novo.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

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
                    ATACADO_REP_COMISSAO _comissaoREP = atacController.ObterComissaoRep(Convert.ToInt32(b.CommandArgument));
                    if (_comissaoREP != null)
                    {

                        hidCodigo.Value = _comissaoREP.CODIGO.ToString();

                        ddlRepresentante.SelectedValue = _comissaoREP.REPRESENTANTE;
                        ddlFilial.SelectedValue = _comissaoREP.FILIAL;
                        txtComissao.Text = _comissaoREP.COMISSAO.ToString();

                        labRepresentante.ForeColor = Color.Red;
                        labFilial.ForeColor = Color.Red;
                        labComissão.ForeColor = Color.Red;
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
                        labErro.Text = "A Comissão não pode ser excluída. Esta Comissão foi utilizada.";
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

        private void CarregarComissaoRepresentantes()
        {
            List<ATACADO_REP_COMISSAO> _comissaoREP = new List<ATACADO_REP_COMISSAO>();

            try
            {
                _comissaoREP = atacController.ObterComissaoRep();

                gvComissao.DataSource = _comissaoREP;
                gvComissao.DataBind();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void gvComissao_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ATACADO_REP_COMISSAO _atacadoREP = e.Row.DataItem as ATACADO_REP_COMISSAO;

                    coluna += 1;
                    if (_atacadoREP != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = _atacadoREP.FILIAL;

                        Literal _litComissao = e.Row.FindControl("litComissao") as Literal;
                        if (_litComissao != null)
                            _litComissao.Text = _atacadoREP.COMISSAO.ToString("###,###,###,##0.000");

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _atacadoREP.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _atacadoREP.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
