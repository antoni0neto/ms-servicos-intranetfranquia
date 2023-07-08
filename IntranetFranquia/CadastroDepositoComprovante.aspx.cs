using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class CadastroDepositoComprovante : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();
                CarregaGridViewComprovantes();
            }
        }

        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                ddlFilial.DataBind();
            }
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            txtDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            txtDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        private void CarregaComprovante(int codigoComprovante)
        {
            DEPOSITO_COMPROVANTE depositoComprovante = usuarioController.BuscaPorCodigoComprovante(codigoComprovante);

            if (depositoComprovante != null)
            {
                FILIAI filial = baseController.BuscaFilialCodigoInt(depositoComprovante.CODIGO_FILIAL);

                if (filial != null)
                    ddlFilial.SelectedValue = filial.COD_FILIAL;
                
                txtDataInicio.Text = depositoComprovante.DATA_INICIO.ToString("dd/MM/yyyy");
                txtDataFim.Text = depositoComprovante.DATA_FIM.ToString("dd/MM/yyyy");
                txtValor.Text = depositoComprovante.VALOR.ToString();
                txtObs.Text = depositoComprovante.OBS;
                HiddenFieldCodigoComprovante.Value = depositoComprovante.CODIGO.ToString();
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            if (txtDataInicio.Text.Equals("") ||
                txtDataInicio.Text == null ||
                txtDataFim.Text.Equals("") ||
                txtDataFim.Text == null ||
                ddlFilial.SelectedValue.ToString().Equals("0") ||
                ddlFilial.SelectedValue.ToString().Equals("") ||
                txtValor.Text.Equals("") ||
                txtValor.Text == null)
                return;

            try
            {
                DEPOSITO_COMPROVANTE depositoComprovante;

                if (Convert.ToInt32(HiddenFieldCodigoComprovante.Value) > 0)
                    depositoComprovante = usuarioController.BuscaPorCodigoComprovante(Convert.ToInt32(HiddenFieldCodigoComprovante.Value));
                else
                    depositoComprovante = new DEPOSITO_COMPROVANTE();

                depositoComprovante.CODIGO_FILIAL = Convert.ToInt32(ddlFilial.SelectedValue);
                depositoComprovante.DATA_INICIO = Convert.ToDateTime(txtDataInicio.Text);
                depositoComprovante.DATA_FIM = Convert.ToDateTime(txtDataFim.Text);
                depositoComprovante.VALOR = Convert.ToDecimal(txtValor.Text);
                depositoComprovante.OBS = txtObs.Text;

                if (Convert.ToInt32(HiddenFieldCodigoComprovante.Value) > 0)
                    usuarioController.AtualizaComprovante(depositoComprovante);
                else
                    usuarioController.InsereComprovante(depositoComprovante);

                LabelFeedBack.Text = "Gravado com sucesso!";

                CarregaGridViewComprovantes();

                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewComprovantes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DEPOSITO_COMPROVANTE depositoComprovante = e.Row.DataItem as DEPOSITO_COMPROVANTE;

            if (depositoComprovante != null)
            {
                Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;

                if (buttonEditar != null)
                    buttonEditar.CommandArgument = depositoComprovante.CODIGO.ToString();

                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;

                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = depositoComprovante.CODIGO.ToString();

                Button buttonAtualizar = e.Row.FindControl("ButtonAtualizar") as Button;

                if (buttonAtualizar != null)
                    buttonAtualizar.CommandArgument = depositoComprovante.CODIGO.ToString();

                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                {
                    FILIAI filial = baseController.BuscaFilialCodigoInt(depositoComprovante.CODIGO_FILIAL);

                    if (filial != null)
                        literalFilial.Text = filial.FILIAL;
                }
            }
        }

        private void CarregaGridViewComprovantes()
        {
            GridViewComprovantes.DataSource = usuarioController.BuscaComprovantes();
            GridViewComprovantes.DataBind();
        }

        protected void ButtonExcluirComprovante_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                usuarioController.ExcluiComprovante(Convert.ToInt32(buttonExcluir.CommandArgument));

                CarregaGridViewComprovantes();

                LimpaTela();

                LimpaFeedBack();
            }
        }

        protected void ButtonEditarComprovante_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;

            if (buttonEditar != null)
            {
                CarregaComprovante(Convert.ToInt32(buttonEditar.CommandArgument));

                LimpaFeedBack();
            }
        }

        protected void ButtonAtualizarComprovante_Click(object sender, EventArgs e)
        {
            Button buttonAtualizar = sender as Button;

            if (buttonAtualizar != null)
            {
                DEPOSITO_COMPROVANTE depositoComprovante = usuarioController.BuscaPorCodigoComprovante(Convert.ToInt32(buttonAtualizar.CommandArgument));

                if (depositoComprovante != null)
                {
                    List<DEPOSITO_HISTORICO> listaDepositoHistorico = baseController.BuscaDepositoSemana(depositoComprovante.CODIGO_FILIAL, depositoComprovante.DATA_INICIO, depositoComprovante.DATA_FIM);

                    if (listaDepositoHistorico != null)
                    {
                        /*
                        decimal valorDeposito = 0;
                        
                        foreach (DEPOSITO_HISTORICO depositoHistorico in listaDepositoHistorico)
                        {
                            valorDeposito += depositoHistorico.VALOR_DEPOSITO;
                        }
                        */
                        //if (depositoComprovante.VALOR == valorDeposito)
                        //{
                            foreach (DEPOSITO_HISTORICO depositoHistorico in listaDepositoHistorico)
                            {
                                depositoHistorico.CONFERIDO = true;

                                baseController.AtualizaHistoricoDeposito(depositoHistorico);
                            }

                            List<DEPOSITO_COMPROVANTE> listaDepositoComprovante = usuarioController.BuscaDepositosComprovantes(depositoComprovante.CODIGO_FILIAL, depositoComprovante.DATA_INICIO, depositoComprovante.DATA_FIM);

                            if (listaDepositoComprovante != null)
                            {
                                foreach (DEPOSITO_COMPROVANTE itemDepositoComprovante in listaDepositoComprovante)
                                {
                                    itemDepositoComprovante.CONFERIDO = true;

                                    baseController.AtualizaDepositoComprovante(itemDepositoComprovante);
                                }
                            }
                        //}
                    }
                }

                CarregaGridViewComprovantes();

                LimpaTela();

                LimpaFeedBack();
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            HiddenFieldCodigoComprovante.Value = "0";
            txtDataInicio.Text = string.Empty;
            txtDataFim.Text = string.Empty;
            txtValor.Text = string.Empty;
            txtObs.Text = string.Empty;
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }
    }
}