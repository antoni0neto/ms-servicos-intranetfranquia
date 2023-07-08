using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class AlteraNel : System.Web.UI.Page
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
                CarregaDropDownListColecaoNel();
                CarregaDropDownListJanelaNel();
                CarregaDropDownListContainer();
            }
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecao();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListJanela()
        {
            ddlJanela.DataSource = baseController.BuscaJanela();
            ddlJanela.DataBind();
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlJanela_DataBound(object sender, EventArgs e)
        {
            ddlJanela.Items.Add(new ListItem("Selecione", "0"));
            ddlJanela.SelectedValue = "0";
        }

        private void CarregaDropDownListColecaoNel()
        {
            ddlColecaoNel.DataSource = baseController.BuscaColecao();
            ddlColecaoNel.DataBind();
        }

        private void CarregaDropDownListJanelaNel()
        {
            ddlJanelaNel.DataSource = baseController.BuscaJanela();
            ddlJanelaNel.DataBind();
        }

        protected void ddlColecaoNel_DataBound(object sender, EventArgs e)
        {
            ddlColecaoNel.Items.Add(new ListItem("Selecione", "0"));
            ddlColecaoNel.SelectedValue = "0";
        }

        protected void ddlJanelaNel_DataBound(object sender, EventArgs e)
        {
            ddlJanelaNel.Items.Add(new ListItem("Selecione", "0"));
            ddlJanelaNel.SelectedValue = "0";
        }

        protected void btBuscarNel_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals("") ||
                ddlJanela.SelectedValue.ToString().Equals("0") ||
                ddlJanela.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewNel();
        }

        private void CarregaNel(int codigoNel)
        {
            IMPORTACAO_NEL nel = usuarioController.BuscaPorCodigoNel(codigoNel);

            if (nel != null)
            {
                txtCodigo.Text = nel.CODIGO_NEL.ToString();
                txtDescricao.Text = nel.DESCRICAO_NEL;

                if (nel.DATA_PAGAMENTO != null)
                    txtDataPagamento.Text = Convert.ToDateTime(nel.DATA_PAGAMENTO).ToString("dd/MM/yyyy");

                if (nel.VALOR_PAGAMENTO != null)
                    txtValorPagamento.Text = Convert.ToDecimal(nel.VALOR_PAGAMENTO).ToString("###,###.##");

                if (nel.DATA_DRAFT != null)
                    txtDataDraft.Text = Convert.ToDateTime(nel.DATA_DRAFT).ToString("dd/MM/yyyy");

                if (nel.ORIGINAL != null)
                {
                    if (nel.ORIGINAL == true)
                        cbOriginal.Checked = true;
                    else
                        cbOriginal.Checked = false;
                }

                if (nel.DATA_RECEBIMENTO != null)
                    txtDataRecebimento.Text = Convert.ToDateTime(nel.DATA_RECEBIMENTO).ToString("dd/MM/yyyy");

                if (nel.DATA_DESPACHANTE != null)
                    txtDataDespachante.Text = Convert.ToDateTime(nel.DATA_DESPACHANTE).ToString("dd/MM/yyyy");

                if (nel.DATA_LI != null)
                    txtDataLI.Text = Convert.ToDateTime(nel.DATA_LI).ToString("dd/MM/yyyy");

                if (nel.DATA_BOOKING != null)
                    txtDataBooking.Text = Convert.ToDateTime(nel.DATA_BOOKING).ToString("dd/MM/yyyy");

                if (nel.DATA_EMBARQUE != null)
                    txtDataEmbarque.Text = Convert.ToDateTime(nel.DATA_EMBARQUE).ToString("dd/MM/yyyy");

                if (nel.DESCRICAO_DI != null)
                    txtDI.Text = nel.DESCRICAO_DI;

                if (nel.DESCRICAO_BL != null)
                    txtBL.Text = nel.DESCRICAO_BL;

                if (nel.PACK != null)
                {
                    if (nel.PACK == true)
                        cbPack.Checked = true;
                    else
                        cbPack.Checked = false;
                }

                if (nel.COMERCIAL_INVOCE != null)
                {
                    if (nel.COMERCIAL_INVOCE == true)
                        cbComercialInvoce.Checked = true;
                    else
                        cbComercialInvoce.Checked = false;
                }

                if (nel.DATA_PREVISAO_CHEGADA != null)
                    txtDataChegada.Text = Convert.ToDateTime(nel.DATA_PREVISAO_CHEGADA).ToString("dd/MM/yyyy");

                if (nel.DATA_DESEMBARACO != null)
                    txtDataDesembaraco.Text = Convert.ToDateTime(nel.DATA_DESEMBARACO).ToString("dd/MM/yyyy");

                if (nel.STATUS_LIBERACAO != null)
                    txtStatus.Text = nel.STATUS_LIBERACAO;

                if (nel.CODIGO_COLECAO != null)
                    ddlColecaoNel.SelectedValue = nel.CODIGO_COLECAO.ToString();

                if (nel.CODIGO_JANELA != null)
                    ddlJanelaNel.SelectedValue = nel.CODIGO_JANELA.ToString();
                
                if (nel.CODIGO_CONTAINER != null)
                    ddlContainer.SelectedValue = nel.CODIGO_CONTAINER.ToString();

                HiddenFieldCodigoNel.Value = nel.CODIGO_NEL.ToString();
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                IMPORTACAO_NEL nel;

                if (Convert.ToInt32(HiddenFieldCodigoNel.Value) > 0)
                    nel = usuarioController.BuscaPorCodigoNel(Convert.ToInt32(HiddenFieldCodigoNel.Value));
                else
                {
                    IMPORTACAO_NEL nelOld = usuarioController.BuscaNelPelaDescricao(txtDescricao.Text);

                    if (nelOld != null)
                        throw new Exception("NeL já existe!");

                    nel = new IMPORTACAO_NEL();
                }

                nel.CODIGO_NEL = Convert.ToInt32(txtCodigo.Text);
                nel.DESCRICAO_NEL = txtDescricao.Text;

                if (txtDataPagamento.Text != "")
                    nel.DATA_PAGAMENTO = Convert.ToDateTime(txtDataPagamento.Text);

                if (txtValorPagamento.Text != "")
                    nel.VALOR_PAGAMENTO = Convert.ToDecimal(txtValorPagamento.Text);

                if (txtDataDraft.Text != "")
                    nel.DATA_DRAFT = Convert.ToDateTime(txtDataDraft.Text);

                if (txtDataRecebimento.Text != "")
                    nel.DATA_RECEBIMENTO = Convert.ToDateTime(txtDataRecebimento.Text);

                if (txtDataDespachante.Text != "")
                    nel.DATA_DESPACHANTE = Convert.ToDateTime(txtDataDespachante.Text);

                if (txtDataLI.Text != "")
                    nel.DATA_LI = Convert.ToDateTime(txtDataLI.Text);

                if (txtDataBooking.Text != "")
                    nel.DATA_BOOKING = Convert.ToDateTime(txtDataBooking.Text);

                if (txtDataEmbarque.Text != "")
                    nel.DATA_EMBARQUE = Convert.ToDateTime(txtDataEmbarque.Text);

                if (txtDataChegada.Text != "")
                    nel.DATA_PREVISAO_CHEGADA = Convert.ToDateTime(txtDataChegada.Text);

                if (txtDataDesembaraco.Text != "")
                    nel.DATA_DESEMBARACO = Convert.ToDateTime(txtDataDesembaraco.Text);

                if (txtDI.Text != "")
                    nel.DESCRICAO_DI = txtDI.Text;

                if (txtBL.Text != "")
                    nel.DESCRICAO_BL = txtBL.Text;

                if (cbOriginal != null)
                {
                    if (cbOriginal.Checked)
                        nel.ORIGINAL = true;
                    else
                        nel.ORIGINAL = false;
                }

                if (cbPack != null)
                {
                    if (cbPack.Checked)
                        nel.PACK = true;
                    else
                        nel.PACK = false;
                }

                if (cbComercialInvoce != null)
                {
                    if (cbComercialInvoce.Checked)
                        nel.COMERCIAL_INVOCE = true;
                    else
                        nel.COMERCIAL_INVOCE = false;
                }

                if (txtStatus.Text != "")
                    nel.STATUS_LIBERACAO = txtStatus.Text;

                if (ddlColecaoNel != null)
                    nel.CODIGO_COLECAO = Convert.ToInt32(ddlColecaoNel.SelectedValue);
                
                if (ddlJanelaNel != null)
                    nel.CODIGO_JANELA = Convert.ToInt32(ddlJanelaNel.SelectedValue);

                if (ddlContainer != null)
                    nel.CODIGO_CONTAINER = Convert.ToInt32(ddlContainer.SelectedValue);

                if (Convert.ToInt32(HiddenFieldCodigoNel.Value) > 0)
                    usuarioController.AtualizaNel(nel);
                else
                    usuarioController.InsereNel(nel);

                LabelFeedBack.Text = "Gravado com sucesso!";

                CarregaGridViewNel();

                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewNel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_NEL nel = e.Row.DataItem as IMPORTACAO_NEL;

            if (nel != null)
            {
                Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;

                if (buttonEditar != null)
                    buttonEditar.CommandArgument = nel.CODIGO_NEL.ToString();

                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;

                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = nel.CODIGO_NEL.ToString();
            }
        }

        private void CarregaGridViewNel()
        {
            GridViewNel.DataSource = usuarioController.BuscaNels(Convert.ToInt32(ddlColecao.SelectedValue), Convert.ToInt32(ddlJanela.SelectedValue));
            GridViewNel.DataBind();
        }

        protected void ButtonExcluirNel_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                IMPORTACAO_NEL nel = usuarioController.BuscaPorCodigoNel(Convert.ToInt32(buttonExcluir.CommandArgument));

                if (nel != null)
                {
                    usuarioController.ExcluiNel(nel);

                    CarregaGridViewNel();

                    LimpaTela();
                }
            }
        }

        protected void ButtonEditarNel_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;

            if (buttonEditar != null)
            {
                CarregaNel(Convert.ToInt32(buttonEditar.CommandArgument));
            
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
            txtDataDraft.Text = string.Empty;
            cbOriginal.Checked = false;
            txtDataRecebimento.Text = string.Empty;
            txtDataDespachante.Text = string.Empty;
            txtDataLI.Text = string.Empty;
            txtDataBooking.Text = string.Empty;
            txtDataEmbarque.Text = string.Empty;
            txtDI.Text = string.Empty;
            txtBL.Text = string.Empty;
            cbPack.Checked = false;
            cbComercialInvoce.Checked = false;
            txtDataChegada.Text = string.Empty;
            txtDataDesembaraco.Text = string.Empty;
            txtStatus.Text = string.Empty;

            ddlColecaoNel.Items.Add(new ListItem("Selecione", "0"));
            ddlColecaoNel.SelectedValue = "0";

            ddlJanelaNel.Items.Add(new ListItem("Selecione", "0"));
            ddlJanelaNel.SelectedValue = "0";
            
            ddlContainer.Items.Add(new ListItem("Selecione", "0"));
            ddlContainer.SelectedValue = "0";

            HiddenFieldCodigoNel.Value = "0";
        }
        
        protected void ddlContainer_DataBound(object sender, EventArgs e)
        {
            ddlContainer.Items.Add(new ListItem("Selecione", "0"));
            ddlContainer.SelectedValue = "0";
        }

        private void CarregaDropDownListContainer()
        {
            ddlContainer.DataSource = usuarioController.BuscaContainers();
            ddlContainer.DataBind();
        }
    }
}