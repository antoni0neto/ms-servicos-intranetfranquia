using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class CadastroProforma : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        PerfilController perfilController = new PerfilController();
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListColecao();
                CarregaDropDownListJanela();
                CarregaGridViewProforma();
            }
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = usuarioController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        protected void ddlJanela_DataBound(object sender, EventArgs e)
        {
            ddlJanela.Items.Add(new ListItem("Selecione", "0"));
            ddlJanela.SelectedValue = "0";
        }

        private void CarregaDropDownListJanela()
        {
            ddlJanela.DataSource = baseController.BuscaJanela();
            ddlJanela.DataBind();
        }

        private void CarregaProforma(int codigoProforma)
        {
            IMPORTACAO_PROFORMA proforma = usuarioController.BuscaPorCodigoProforma(codigoProforma);

            if (proforma != null)
            {
                txtCodigo.Text = proforma.CODIGO_PROFORMA.ToString();
                txtDescricao.Text = proforma.DESCRICAO_PROFORMA;

                if (proforma.DATA_PAGAMENTO != null)
                    txtDataPagamento.Text = Convert.ToDateTime(proforma.DATA_PAGAMENTO).ToString("dd/MM/yyyy");

                if (proforma.VALOR_PAGAMENTO != null)
                    txtValorPagamento.Text = Convert.ToDecimal(proforma.VALOR_PAGAMENTO).ToString("###,###.##");

                if (proforma.CODIGO_COLECAO != null)
                    ddlColecao.SelectedValue = proforma.CODIGO_COLECAO.ToString();

                if (proforma.CODIGO_JANELA != null)
                    ddlJanela.SelectedValue = proforma.CODIGO_JANELA.ToString();

                HiddenFieldCodigoProforma.Value = proforma.CODIGO_PROFORMA.ToString();
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                IMPORTACAO_PROFORMA proforma;

                if (Convert.ToInt32(HiddenFieldCodigoProforma.Value) > 0)
                    proforma = usuarioController.BuscaPorCodigoProforma(Convert.ToInt32(HiddenFieldCodigoProforma.Value));
                else
                {
                    IMPORTACAO_PROFORMA proformaOld = usuarioController.BuscaProformaPelaDescricao(txtDescricao.Text);

                    if (proformaOld != null)
                        throw new Exception("Proforma já existe!");

                    proforma = new IMPORTACAO_PROFORMA();
                }

                proforma.CODIGO_PROFORMA = Convert.ToInt32(txtCodigo.Text);
                proforma.DESCRICAO_PROFORMA = txtDescricao.Text;

                if (txtDataPagamento.Text != "")
                    proforma.DATA_PAGAMENTO = Convert.ToDateTime(txtDataPagamento.Text);

                if (txtValorPagamento.Text != "")
                    proforma.VALOR_PAGAMENTO = Convert.ToDecimal(txtValorPagamento.Text);

                if (ddlColecao != null)
                    proforma.CODIGO_COLECAO = Convert.ToInt32(ddlColecao.SelectedValue);

                if (ddlJanela != null)
                    proforma.CODIGO_JANELA = Convert.ToInt32(ddlJanela.SelectedValue);

                if (Convert.ToInt32(HiddenFieldCodigoProforma.Value) > 0)
                    usuarioController.AtualizaProforma(proforma);
                else
                    usuarioController.InsereProforma(proforma);

                LabelFeedBack.Text = "Gravado com sucesso!";

                CarregaGridViewProforma();

                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewProforma_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_PROFORMA proforma = e.Row.DataItem as IMPORTACAO_PROFORMA;

            if (proforma != null)
            {
                Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;

                if (buttonEditar != null)
                    buttonEditar.CommandArgument = proforma.CODIGO_PROFORMA.ToString();

                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;

                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = proforma.CODIGO_PROFORMA.ToString();
            }
        }

        private void CarregaGridViewProforma()
        {
            GridViewProforma.DataSource = usuarioController.BuscaProformas();
            GridViewProforma.DataBind();
        }

        protected void ButtonExcluirProforma_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                IMPORTACAO_PROFORMA proforma = usuarioController.BuscaPorCodigoProforma(Convert.ToInt32(buttonExcluir.CommandArgument));

                if (proforma != null)
                {
                    usuarioController.ExcluiProforma(proforma);

                    CarregaGridViewProforma();

                    LimpaTela();
                }
            }
        }

        protected void ButtonEditarProforma_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;

            if (buttonEditar != null)
            {
                CarregaProforma(Convert.ToInt32(buttonEditar.CommandArgument));
            
                LimpaFeedBack();
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            txtCodigo.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            txtDataPagamento.Text = string.Empty;
            txtValorPagamento.Text = string.Empty;

            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";

            ddlJanela.Items.Add(new ListItem("Selecione", "0"));
            ddlJanela.SelectedValue = "0";

            HiddenFieldCodigoProforma.Value = "0";
        }
        
        protected void CalendarDataPagamento_SelectionChanged(object sender, EventArgs e)
        {
            txtDataPagamento.Text = CalendarDataPagamento.SelectedDate.ToString("dd/MM/yyyy");
        }
    }
}