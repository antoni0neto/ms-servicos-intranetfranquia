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

namespace Relatorios
{
    public partial class cont_resultado : System.Web.UI.Page
    {
        ContagemController contController = new ContagemController();
        BaseController baseController = new BaseController();

        const string cAbaTodos = "0";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarFilial();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);

            //Evitar duplo clique no botão
            ibtPesquisar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(ibtPesquisar, null) + ";");
            btIncluir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btIncluir, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais();
            filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        private void CarregarDataParaContagem(string codigoFilial)
        {
            var dataContagem = contController.ObterDataParaContagem(codigoFilial);

            List<ListItem> items = new List<ListItem>();
            foreach (var i in dataContagem)
                items.Add(new ListItem { Text = Convert.ToDateTime(i.DATA).ToString("dd/MM/yyyy"), Value = Convert.ToDateTime(i.DATA).ToString("dd/MM/yyyy") });

            items.Insert(0, new ListItem { Text = "Selecione", Value = "" });
            ddlDataContagem.DataSource = items;
            ddlDataContagem.DataBind();
        }
        #endregion

        #region "AÇÕES DA TELA"
        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                string codigoFilial = "";
                labErro.Text = "";

                ddlFilial.Enabled = true;
                if (ddlFilial.SelectedValue == "")
                {
                    labErro.Text = "Selecione uma Filial.";
                    return;
                }

                codigoFilial = ddlFilial.SelectedValue;
                ddlFilial.Enabled = false;

                CarregarDataParaContagem(codigoFilial);
                CarregarResultadoContagem(codigoFilial);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtLimpar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";

                ddlFilial.Enabled = true;
                ddlFilial.SelectedValue = "";

                ddlDataContagem.DataSource = new List<CONT_PROGRAMACAO>();
                ddlDataContagem.DataBind();
                txtContagem.Text = "";
                txtGerente.Text = "";

                gvResultadoContagem.DataSource = new List<SP_OBTER_CONT_RESULT_LOJAResult>();
                gvResultadoContagem.DataBind();
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

        #endregion

        #region "RESULTADO"
        private void CarregarResultadoContagem(string codigoFilial)
        {
            var resultadoLoja = contController.ObterResultadoPorLoja(codigoFilial);
            gvResultadoContagem.DataSource = resultadoLoja;
            gvResultadoContagem.DataBind();

            if (resultadoLoja == null || resultadoLoja.Count() <= 0)
            {
                labErro.Text = "Nenhum resultado de Contagem Encontrado.";
                return;
            }
        }
        protected void gvResultadoContagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CONT_RESULT_LOJAResult resultadoCont = e.Row.DataItem as SP_OBTER_CONT_RESULT_LOJAResult;

                    if (resultadoCont != null)
                    {
                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = resultadoCont.DATA_CONTAGEM.ToString("dd/MM/yyyy");

                        Literal _litDiaUltimaContagem = e.Row.FindControl("litDiaUltimaContagem") as Literal;
                        if (_litDiaUltimaContagem != null)
                            _litDiaUltimaContagem.Text = resultadoCont.DIAS_ULTIMA_CONTAGEM.ToString() + " dias";

                        if (resultadoCont.LEGENDA.ToLower() == "boa")
                            e.Row.BackColor = Color.LightGreen;
                        else if (resultadoCont.LEGENDA.ToLower() == "preocupante")
                            e.Row.BackColor = Color.Gold;
                        else if (resultadoCont.LEGENDA.ToLower() == "ruim")
                            e.Row.BackColor = Color.LightCoral;
                        else if (resultadoCont.LEGENDA.ToLower() == "crítica")
                        {
                            e.Row.BackColor = Color.MediumPurple;
                            e.Row.ForeColor = Color.White;
                        }

                        ImageButton _btExcluir = e.Row.FindControl("btExcluir") as ImageButton;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = resultadoCont.CODIGO_RESULTADO.ToString();
                            var dataFechamento = contController.ObterResultadoFechamento(Convert.ToDateTime(resultadoCont.DATA_CONTAGEM.Year + "-" + ((resultadoCont.DATA_CONTAGEM.Month.ToString().Length == 1) ? ("0" + resultadoCont.DATA_CONTAGEM.Month.ToString()) : resultadoCont.DATA_CONTAGEM.Month.ToString()) + "-01"));
                            if (dataFechamento != null)
                                _btExcluir.Visible = false;

                        }



                    }
                }
            }

        }
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlDataContagem.SelectedValue == "")
                {
                    labErro.Text = "Selecione uma data programada.";
                    return;
                }
                if (txtContagem.Text.Trim() == "")
                {
                    labErro.Text = "Informe o resultado da Contagem de Peças.";
                    return;
                }
                if (txtPorcValorAceitavel.Text.Trim() == "")
                {
                    labErro.Text = "Informe a % de Valor Aceitável.";
                    return;
                }

                if (txtGerente.Text.Trim() == "")
                {
                    labErro.Text = "Informe o Gerente que fez a contagem.";
                    return;
                }

                CONT_RESULTADO contResultado = new CONT_RESULTADO();
                contResultado.DATA = Convert.ToDateTime(ddlDataContagem.SelectedValue);
                contResultado.CODIGO_FILIAL = ddlFilial.SelectedValue;
                contResultado.VALOR_ACEITAVEL_PORC = Convert.ToDecimal(txtPorcValorAceitavel.Text);
                contResultado.CONTAGEM = Convert.ToInt32(txtContagem.Text) * -1;
                contResultado.GERENTE = txtGerente.Text.Trim().ToUpper();

                contController.InserirResultado(contResultado);

                CarregarDataParaContagem(ddlFilial.SelectedValue);
                CarregarResultadoContagem(ddlFilial.SelectedValue);

                ddlDataContagem.SelectedValue = "";
                txtContagem.Text = "";
                txtGerente.Text = "";

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }


        }
        protected void btExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton ibt = (ImageButton)sender;

                if (ibt != null)
                {
                    int codigoResultado = 0;
                    codigoResultado = Convert.ToInt32(ibt.CommandArgument);

                    ibt.Enabled = false;
                    contController.ExcluirResultado(codigoResultado);
                    CarregarDataParaContagem(ddlFilial.SelectedValue);
                    CarregarResultadoContagem(ddlFilial.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #endregion



        #region "VALIDACAO"
        #endregion



    }
}