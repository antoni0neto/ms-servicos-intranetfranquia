using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;

namespace Relatorios.mod_financeiro
{
    public partial class contabil_cad_param_folha : System.Web.UI.Page
    {
        ContabilidadeController contabilidadeController = new ContabilidadeController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                CarregaEventos();
                CarregaContasDebito();
                CarregaContasCredito();
                RecarregarTela();

            }
            btIncluir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btIncluir, null) + ";");
            btCancelar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btCancelar, null) + ";");

            CriaCabecalhoRodapeAtualizaScript();
        }

        #region "DADOS INICIAS"
        private void CarregaContasCredito()
        {
            ddlContaCredito.DataSource = contabilidadeController.ObterContas();
            ddlContaCredito.DataBind();
            ddlContaCredito.Items.Insert(0, new ListItem("Selecione", "0"));
        }
        private void CarregaContasDebito()
        {
            ddlContaDebito.DataSource = contabilidadeController.ObterContas();
            ddlContaDebito.DataBind();
            ddlContaDebito.Items.Insert(0, new ListItem("Selecione", "0"));
        }
        private void CarregaEventos()
        {
            ddlEvento.DataSource = contabilidadeController.ObterEventos();
            ddlEvento.DataBind();
            ddlEvento.Items.Insert(0, new ListItem("Selecione", "0"));
        }
        protected void ddlContaDebito_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlContaDebito.SelectedValue != "0")
            {
                CarregaContasCredito();
                ddlContaCredito.Items.Remove(ddlContaDebito.SelectedItem);
            }
        }
        #endregion

        private void AtualizaGrid()
        {
            Utils.WebControls.GetBoundFieldIndexByName(gvParamContab, " - >>");
            Utils.WebControls.GetBoundFieldIndexByName(gvParamContab, " - <<");

            gvParamContab.DataSource = contabilidadeController.ObterParametrosContabilidade();
            gvParamContab.DataBind();

            gvEventoSemContabil.DataSource = contabilidadeController.ObterEventoSemContabilizacao();
            gvEventoSemContabil.DataBind();

            CriaCabecalhoRodapeAtualizaScript();
        }

        #region "CRUD"
        private void Incluir(GER_PARAMETROS_CONTABILIDADE _param)
        {
            contabilidadeController.InserirParamContabilidade(_param);
        }
        private void Excluir(int COD_EVENTO, string CONTA_DEBITO, string CONTA_CREDITO)
        {
            contabilidadeController.ExcluirParamContabilidade(COD_EVENTO, CONTA_DEBITO, CONTA_CREDITO);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            btCancelar.Visible = (ddlEvento.SelectedValue != "0");

            try
            {
                List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
                list.Add(new Utils.WebControls.ControlesValidacao() { Label = labEvento, Controle = ddlEvento, CorPadrao = Color.Black, CorAviso = Color.Red, ValorPadrao = "0" });
                list.Add(new Utils.WebControls.ControlesValidacao() { Label = labContaDebito, Controle = ddlContaDebito, CorPadrao = Color.Black, CorAviso = Color.Red, ValorPadrao = "0" });
                list.Add(new Utils.WebControls.ControlesValidacao() { Label = labContaCredito, Controle = ddlContaCredito, CorPadrao = Color.Black, CorAviso = Color.Red, ValorPadrao = "0" });

                if (!Utils.WebControls.ValidarCampos(ref list))
                {
                    labErro.Text = "Preencha corretamente os campos em vermelho.";

                    CriaCabecalhoRodapeAtualizaScript();
                    return;
                }

                int COD_EVENTO = Convert.ToInt32(ddlEvento.SelectedValue);
                string CONTA_DEBITO = ddlContaDebito.SelectedValue;
                string CONTA_CREDITO = ddlContaCredito.SelectedValue;

                GER_PARAMETROS_CONTABILIDADE _novo = contabilidadeController.ObterParametrosContabilidade(COD_EVENTO, CONTA_DEBITO, CONTA_CREDITO);

                if (_novo == null)
                {
                    _novo = new GER_PARAMETROS_CONTABILIDADE();

                    _novo.COD_EVENTO = int.Parse(ddlEvento.SelectedValue);
                    _novo.CONTA_DEBITO = ddlContaDebito.SelectedValue;
                    _novo.CONTA_CREDITO = ddlContaCredito.SelectedValue;
                    _novo.ITEM = 1;
                    _novo.DATA_INICIO = DateTime.Now;
                    _novo.CNPJ = "";

                    if (((Button)sender).CommandArgument == "I")
                    {
                        Incluir(_novo);
                        RecarregarTela();
                    }
                }
                else if (((Button)sender).CommandArgument == "A")
                {
                    Excluir(COD_EVENTO, CONTA_DEBITO, CONTA_CREDITO);
                    Incluir(_novo);
                    RecarregarTela();
                }
                else
                {
                    labErro.Text = "Evento/Conta de Débito/Crédito já cadastrado na base de dados.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
            CriaCabecalhoRodapeAtualizaScript();
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
                        int COD_EVENTO = Convert.ToInt32(b.CommandArgument.Split('|')[0].Trim());
                        string CONTA_DEBITO = b.CommandArgument.Split('|')[1].Trim();
                        string CONTA_CREDITO = b.CommandArgument.Split('|')[2].Trim();

                        Excluir(COD_EVENTO, CONTA_DEBITO, CONTA_CREDITO);
                        RecarregarTela();
                    }
                    catch (Exception)
                    {
                        labErro.Text = "O Tipo de Processo não pode ser excluído. Este Tipo de Processo foi utilizado em um Processo.";
                    }

                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
            CriaCabecalhoRodapeAtualizaScript();
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            btCancelar.Visible = false;
            btIncluir.CommandArgument = "I";

            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labEvento, Controle = ddlEvento, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labContaDebito, Controle = ddlContaDebito, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labContaCredito, Controle = ddlContaCredito, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErro, CorPadrao = Color.Gray, ValorPadrao = "" });

            Utils.WebControls.LimparControles(ref list);
            CriaCabecalhoRodapeAtualizaScript();
        }
        #endregion

        private void RecarregarTela()
        {
            btCancelar.Visible = false;
            btIncluir.CommandArgument = "I";

            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labEvento, Controle = ddlEvento, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labContaDebito, Controle = ddlContaDebito, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labContaCredito, Controle = ddlContaCredito, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErro, CorPadrao = Color.Gray, ValorPadrao = "" });

            Utils.WebControls.LimparControles(ref list);
            AtualizaGrid();
        }
        protected void gvParamContab_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_PARAMETROS_CONTABILIDADEResult> listParam = contabilidadeController.ObterParametrosContabilidade();

            string sortExpression = e.SortExpression;
            SortDirection sort = SortDirection.Ascending;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            listParam = listParam.OrderBy(e.SortExpression + sortDirection);
            gvParamContab.DataSource = listParam;
            gvParamContab.DataBind();

            CriaCabecalhoRodapeAtualizaScript();

        }
        private void CriaCabecalhoRodapeAtualizaScript()
        {
            var gridView = gvParamContab;
            if (gridView.Rows.Count > 0)
            {
                gridView.UseAccessibleHeader = true;
                if (gridView.HeaderRow != null)
                    gridView.HeaderRow.TableSection = TableRowSection.TableHeader;

                if (gridView.FooterRow != null && gridView.ShowFooter)
                    gridView.FooterRow.TableSection = TableRowSection.TableFooter;

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { travaHeaderGrid(); });", true);
            }
        }
    }
}