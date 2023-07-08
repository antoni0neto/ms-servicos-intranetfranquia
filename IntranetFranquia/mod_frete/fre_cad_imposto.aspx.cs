using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios.mod_frete
{
    public partial class fre_cad_imposto : System.Web.UI.Page
    {
        private const decimal multiplicadorValorII = 0.6M;
        private const decimal divisorValorICMS = 0.82M;
        private const decimal multiplicadorPercentualICMS = 0.18M;
        private const decimal multiplicadorReembInfra = 0.68M;
        private const decimal multiplicadorDesembaraco = 15M;

        FreteController freController = new FreteController();

        #region "Page_Load"
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregaTelaInicial();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);

        }

        private void CarregaTelaInicial()
        {
            ibtNovo_Click(null, null);
            VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;
        }

        #endregion

        #region "Propriedades"
        /// <summary>
        /// Define a visão de visualização atual dos botões de ação
        /// </summary>
        /// <param name="visaoAtual"></param>
        private EnumVisaoBotoes VisaoAtualBotoesAcao
        {
            get
            {
                if (Utils.SessionManager.Obter<EnumVisaoBotoes>("VisaoAtualBotoesAcao") == EnumVisaoBotoes.NAO_DEFINIDO)
                    return EnumVisaoBotoes.BUSCAR_NOVO;
                else
                    return Utils.SessionManager.Obter<EnumVisaoBotoes>("VisaoAtualBotoesAcao");
            }
            set
            {
                switch (value)
                {
                    case EnumVisaoBotoes.BUSCAR_NOVO:
                        ibtNovo.Visible = true;
                        ibtPesquisar.Visible = true;

                        ibtSalvar.Visible = false;
                        ibtCancelar.Visible = false;

                        break;
                    case EnumVisaoBotoes.SALVAR_CANCELAR:
                        ibtNovo.Visible = false;
                        ibtPesquisar.Visible = false;

                        ibtSalvar.Visible = true;
                        ibtCancelar.Visible = true;

                        break;
                }
                Utils.SessionManager.Armazenar<EnumVisaoBotoes>(value, "VisaoAtualBotoesAcao");
            }
        }

        #endregion

        #region "Enumerações"

        public enum EnumVisaoAbas
        {
            IMPOSTO_CADASTRO = 0,
            IMPOSTO_LISTAGEM = 1
        }

        public enum EnumVisaoBotoes
        {
            NAO_DEFINIDO = -1,
            BUSCAR_NOVO = 0,
            SALVAR_CANCELAR = 1
        }


        #endregion

        #region "Carrega Tela Inicial"
        /// <summary>
        /// Carrega Formulários e botões para primeiro início
        /// </summary>
        private void RecarregaGridListagemImpostos()
        {
            VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;
            odsGridImposto.SelectParameters["CODIGO"].DefaultValue = "0";
            odsGridImposto.SelectParameters["AWB"].DefaultValue = "";
            odsGridImposto.SelectParameters["STATUS"].DefaultValue = "";
            odsGridImposto.DataBind();

        }
        private void RecarregaFormularioImpostos()
        {
            VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;
            odsImposto.SelectParameters["CODIGO"].DefaultValue = "0";
            odsImposto.SelectParameters["AWB"].DefaultValue = "";
            odsImposto.SelectParameters["STATUS"].DefaultValue = "";
            odsImposto.DataBind();

        }

        #endregion

        #region "Carrega controles para validação na tela"

        /// <summary>
        /// Carrega Validação para os campos do Item da Nota e Labels relacionados
        /// </summary>
        /// <param name="list"></param>
        private void CarregaControleValidacaoImposto(ref List<Utils.WebControls.ControlesValidacao> list)
        {
            Label labAWB = frvCadImposto.FindControl("labAWB") as Label;
            Label labTaxaDolar = frvCadImposto.FindControl("labTaxaDolar") as Label;
            Label labPeso = frvCadImposto.FindControl("labPeso") as Label;           
            Label labStatus = frvCadImposto.FindControl("labStatus") as Label;

            TextBox txtAWB = frvCadImposto.FindControl("txtAWB") as TextBox;
            TextBox txtTaxaDolar = frvCadImposto.FindControl("txtTaxaDolar") as TextBox;
            TextBox txtPeso = frvCadImposto.FindControl("txtPeso") as TextBox;

            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labAWB, Controle = txtAWB, TamanhoMinimo = 12, TamanhoMaximo = 12, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labTaxaDolar, Controle = txtTaxaDolar, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labPeso, Controle = txtPeso, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            
            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErroGeral, CorPadrao = Color.Gray, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErroImposto, CorPadrao = Color.Gray, ValorPadrao = "" });

        }

        #endregion

        #region "Ações do Usuário"

        /// <summary>
        /// Atualiza/Salva um Imposto.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtSalvar_Click(object sender, ImageClickEventArgs e)
        {
            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
            CarregaControleValidacaoImposto(ref list);

            if (!Utils.WebControls.ValidarCampos(ref list))
            {
                labErroImposto.Text = "Preencha corretamente os campos em vermelho.";
                return;
            }

            if (frvCadImposto.CurrentMode == FormViewMode.Insert)
                frvCadImposto.InsertItem(true);

            else if (frvCadImposto.CurrentMode == FormViewMode.Edit)
                frvCadImposto.UpdateItem(true);
        }

        /// <summary>
        /// Muda tela para a visão de Busca/Novo Produto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtCancelar_Click(object sender, ImageClickEventArgs e)
        {
            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
            CarregaControleValidacaoImposto(ref list);
            Utils.WebControls.LimparControles(ref list);

            ibtSalvar.CommandArgument = "I";
            ibtExcluir.Visible = false;
            frvCadImposto.ChangeMode(FormViewMode.ReadOnly);

            RecarregaFormularioImpostos();

            VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;

            labErroGeral.Text = "";
            labErroImposto.Text = "";


        }

        /// <summary>
        /// Limpa a tela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtLimpar_Click(object sender, ImageClickEventArgs e)
        {
            ibtNovo_Click(null, null);
            VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;

        }

        /// <summary>
        /// Realiza  pesquisa de um Imposto cadastrado, utilizando filtragem por AWB e Status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            labErroGeral.Text = "";
            labErroImposto.Text = "";

            TextBox txtAWB = frvCadImposto.FindControl("txtAWB") as TextBox;

            List<FRETE_IMPOSTO> listImpostos = freController.ObterFreteImposto(0, txtAWB.Text, DateTime.MinValue, null);

            if (listImpostos.Count == 1)
            {
                FRETE_IMPOSTO imposto = listImpostos.FirstOrDefault();

                odsImposto.SelectParameters["CODIGO"].DefaultValue = imposto.CODIGO.ToString();
                odsImposto.SelectParameters["AWB"].DefaultValue = imposto.FRETE_RECEBIMENTO_NOTA1.AWB;
                odsImposto.SelectParameters["STATUS"].DefaultValue = "";
                odsImposto.DataBind();
                frvCadImposto.ChangeMode(FormViewMode.Edit);
                frvCadImposto.DataBind();

                VisaoAtualBotoesAcao = EnumVisaoBotoes.SALVAR_CANCELAR;
                ibtExcluir.Visible = true;

                SelecionarAba(EnumVisaoAbas.IMPOSTO_CADASTRO);

            }
            else
            {
                SelecionarAba(EnumVisaoAbas.IMPOSTO_LISTAGEM);

            }

            odsGridImposto.SelectParameters["CODIGO"].DefaultValue = "0";
            odsGridImposto.SelectParameters["AWB"].DefaultValue = txtAWB.Text;
            odsGridImposto.SelectParameters["STATUS"].DefaultValue = "";
            gvFreteImposto.DataBind();


        }

        protected void btExcluirImposto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int CODIGO = int.Parse(((ImageButton)sender).CommandArgument);

                freController.ExcluirFreteImposto(CODIGO);

                frvCadImposto.ChangeMode(FormViewMode.Insert);
                RecarregaFormularioImpostos();

                odsGridImposto.SelectParameters["CODIGO"].DefaultValue = "0";
                odsGridImposto.SelectParameters["AWB"].DefaultValue = "";
                odsGridImposto.SelectParameters["STATUS"].DefaultValue = "";
                gvFreteImposto.DataBind();

                VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;
                SelecionarAba(EnumVisaoAbas.IMPOSTO_LISTAGEM);

                labErroGeral.Text = "Imposto excluído com sucesso.";


            }
            catch (Exception)
            {
                labErroGeral.Text = "Este registro não pode ser excluído.";
            }
        }

        /// <summary>
        /// Muda tela para a visão Salvar/Cancelar Nota
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtNovo_Click(object sender, ImageClickEventArgs e)
        {
            ibtSalvar.CommandArgument = "I";
            ibtExcluir.Visible = false;

            frvCadImposto.ChangeMode(FormViewMode.Insert);

            TextBox txtValorReembInfra = frvCadImposto.FindControl("txtValorReembInfra") as TextBox;
            TextBox txtTotalFaturaEmDolar = frvCadImposto.FindControl("txtTotalFaturaEmDolar") as TextBox;

            TextBox txtValorICMS = frvCadImposto.FindControl("txtValorICMS") as TextBox;
            TextBox txtValorII = frvCadImposto.FindControl("txtValorII") as TextBox;

            TextBox txtValorDesembaraco = frvCadImposto.FindControl("txtValorDesembaraco") as TextBox;

            TextBox txtValorTotalFaturaReal = frvCadImposto.FindControl("txtValorTotalFaturaReal") as TextBox;
            TextBox txtValorTotalImposto = frvCadImposto.FindControl("txtValorTotalImposto") as TextBox;
            TextBox txtFreteTaxaCombustivel = frvCadImposto.FindControl("txtFreteTaxaCombustivel") as TextBox;
            TextBox txtValorFrete = frvCadImposto.FindControl("txtValorFrete") as TextBox;

            txtTotalFaturaEmDolar.Text = "";
            txtValorReembInfra.Text = "";
            txtValorICMS.Text = "";
            txtValorII.Text = "";
            txtValorDesembaraco.Text = "";
            txtValorTotalFaturaReal.Text = "";
            txtValorTotalImposto.Text = "";
            txtFreteTaxaCombustivel.Text = "";
            txtValorFrete.Text = "";

            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
            CarregaControleValidacaoImposto(ref list);
            Utils.WebControls.LimparControles(ref list);

            VisaoAtualBotoesAcao = EnumVisaoBotoes.SALVAR_CANCELAR;

            labErroGeral.Text = "";
            labErroImposto.Text = "";

        }

        protected void ibtExcluirImpostoFormulario_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (frvCadImposto.CurrentMode == FormViewMode.Edit)
                {
                    int CODIGO = int.Parse(frvCadImposto.SelectedValue.ToString());

                    freController.ExcluirFreteImposto(CODIGO);

                    frvCadImposto.ChangeMode(FormViewMode.Insert);
                    RecarregaFormularioImpostos();

                    odsGridImposto.SelectParameters["CODIGO"].DefaultValue = "0";
                    odsGridImposto.SelectParameters["AWB"].DefaultValue = "";
                    odsGridImposto.SelectParameters["STATUS"].DefaultValue = "";
                    gvFreteImposto.DataBind();

                    VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;
                    SelecionarAba(EnumVisaoAbas.IMPOSTO_LISTAGEM);

                    labErroGeral.Text = "Imposto excluído com sucesso.";

                }
            }
            catch (Exception)
            {
                labErroGeral.Text = "Este registro não pode ser excluído.";
            }
        }

        protected void txtAWB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtAWB = ((TextBox)sender) as TextBox;

                if (txtAWB.Text != "" &&
                    (frvCadImposto.CurrentMode == FormViewMode.Insert || frvCadImposto.CurrentMode == FormViewMode.Edit))
                {
                    labErroImposto.Text = "";

                    if (freController.ObterFreteImposto(0, txtAWB.Text.Trim(), DateTime.MinValue, "").Count > 0)
                    {
                        labErroImposto.Text = "Imposto já cadastrado na base de dados.";
                        return;
                    }

                    TextBox txtTaxaDolar = frvCadImposto.FindControl("txtTaxaDolar") as TextBox;

                    FRETE_RECEBIMENTO_NOTA nota = freController.ObterRecebimentoNota(txtAWB.Text);

                    if (nota != null)
                    {
                        FRETE_TAXA_COMBUSTIVEL combustivel = freController.ObterTaxaCombustivel(nota.DATA_RECEBIMENTO);

                        //ARMAZENA A TAXA DO COMBUSTÍVEL (%)
                        if (combustivel != null)
                        {
                            TextBox txtFreteTaxaCombustivel = frvCadImposto.FindControl("txtFreteTaxaCombustivel") as TextBox;

                            txtFreteTaxaCombustivel.Text = combustivel.VALOR_TAXA.ToString();
                            Page.SetFocus(txtTaxaDolar.ClientID);

                        }
                        else
                        {
                            labErroImposto.Text = "Taxa de Combustível não localizada para o mês e ano do recebimento desta nota.";
                            Page.SetFocus(txtAWB.ClientID);
                        }
                    }
                    else
                    {
                        labErroImposto.Text = "Nota não localizada.";
                        txtAWB.Text = "";
                        Page.SetFocus(txtAWB.ClientID);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region "Eventos disparados pelo FormView"

        protected void frvCadImposto_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            try
            {
                labErroGeral.Text = "";
                labErroImposto.Text = "";

                TextBox txtAWB = frvCadImposto.FindControl("txtAWB") as TextBox;

                if (freController.ObterFreteImposto(0, txtAWB.Text.Trim(), DateTime.MinValue, "").Count > 0)
                {
                    labErroImposto.Text = "Imposto já cadastrado na base de dados.";
                    e.Cancel = true;
                    return;
                }

                FRETE_RECEBIMENTO_NOTA nota = freController.ObterRecebimentoNota(txtAWB.Text);

                if (nota == null)
                {
                    labErroImposto.Text = "Nota não localizada.";
                    e.Cancel = true;
                    return;
                }

                if (freController.ObterRecebimentoItemsPorNota(nota.CODIGO).Count == 0)
                {
                    labErroImposto.Text = "Não é possível salvar, pois esta Nota não possui nenhum item vinculado.";
                    e.Cancel = true;
                    return;
                }

                FRETE_TAXA_COMBUSTIVEL combustivel = freController.ObterTaxaCombustivel(nota.DATA_RECEBIMENTO);

                if (combustivel == null)
                {
                    labErroImposto.Text = "Taxa de Combustível não localizada para o mês e ano do recebimento desta nota.";
                    e.Cancel = true;
                    return;
                }

                TextBox txtTaxaDolar = frvCadImposto.FindControl("txtTaxaDolar") as TextBox;
                TextBox txtPeso = frvCadImposto.FindControl("txtPeso") as TextBox;
                TextBox txtValorFrete = frvCadImposto.FindControl("txtValorFrete") as TextBox;
                TextBox txtValorII = frvCadImposto.FindControl("txtValorII") as TextBox;
                TextBox txtValorICMS = frvCadImposto.FindControl("txtValorICMS") as TextBox;
                TextBox txtValorDesembaraco = frvCadImposto.FindControl("txtValorDesembaraco") as TextBox;
                TextBox txtValorReembInfra = frvCadImposto.FindControl("txtValorReembInfra") as TextBox;
                TextBox txtTotalFaturaEmDolar = frvCadImposto.FindControl("txtTotalFaturaEmDolar") as TextBox;
               
                CheckBox chkBoletoOK = frvCadImposto.FindControl("chkBoletoOK") as CheckBox;

                decimal taxaDolar = 0;
                decimal peso = 0;
                decimal valorFaturaEmDolar = 0;
                decimal valorFrete = 0;
                decimal valorII = 0;
                decimal valorICMS = 0;

                Decimal.TryParse(txtTaxaDolar.Text, out taxaDolar);
                Decimal.TryParse(txtPeso.Text, out peso);

                Decimal.TryParse(txtTotalFaturaEmDolar.Text, out valorFaturaEmDolar);
                Decimal.TryParse(txtValorFrete.Text, out valorFrete);
                Decimal.TryParse(txtTaxaDolar.Text, out taxaDolar);

                e.Values["TAXA_DOLAR"] = txtTaxaDolar.Text.Replace(",", ".");

                e.Values["PESO"] = txtPeso.Text.Replace(",", ".");

                e.Values["TAXA_DOLAR"] = txtTaxaDolar.Text.Replace(",", ".");

                e.Values["PESO"] = peso.ToString().Replace(",", ".");

                valorII = freController.CalculaValorII(valorFrete, valorFaturaEmDolar, taxaDolar, multiplicadorValorII);
                e.Values["VALOR_II"] = valorII.ToString().Replace(",", ".");

                valorFrete = freController.CalculaValorFrete(txtAWB.Text, peso, taxaDolar);
                e.Values["VALOR_FRETE"] = valorFrete.ToString().Replace(",", ".");

                e.Values["VALOR_DESEMBARACO"] = freController.CalculaValorDesembaraco(taxaDolar, multiplicadorDesembaraco).ToString().Replace(",", ".");

                e.Values["VALOR_REEMB_INFRA"] = freController.CalculaValorReembolsoInfra(peso, taxaDolar, multiplicadorReembInfra).ToString().Replace(",", ".");

                e.Values["VALOR_FATURA_DOLAR"] = freController.CalculaTotalNotaDolar(nota).ToString().Replace(",", ".");

                valorICMS = freController.CalculaValorICMS(valorII, valorFrete, valorFaturaEmDolar, taxaDolar, divisorValorICMS, multiplicadorPercentualICMS);
                e.Values["VALOR_ICMS"] = valorICMS.ToString().Replace(",", ".");

                e.Values["FRETE_RECEBIMENTO_NOTA"] = nota.CODIGO;
                e.Values["FRETE_TAXA_COMBUSTIVEL"] = combustivel.CODIGO;
                e.Values["DATA_INCLUSAO"] = DateTime.Now;
                e.Values["USUARIO_INCLUSAO"] = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                e.Values["STATUS"] = "1";
                e.Values["BOLETO"] = chkBoletoOK.Checked;


            }
            catch
            {

            }
        }

        protected void frvCadImposto_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            try
            {
                labErroGeral.Text = "";
                labErroImposto.Text = "";

                TextBox txtAWB = frvCadImposto.FindControl("txtAWB") as TextBox;

                FRETE_RECEBIMENTO_NOTA nota = freController.ObterRecebimentoNota(txtAWB.Text);

                if (nota == null)
                {
                    labErroImposto.Text = "Nota não localizada.";
                    e.Cancel = true;
                    return;
                }

                if (freController.ObterRecebimentoItemsPorNota(nota.CODIGO).Count == 0)
                {
                    labErroImposto.Text = "Não é possível salvar, pois esta Nota não possui nenhum item vinculado.";
                    e.Cancel = true;
                    return;
                }

                FRETE_IMPOSTO imposto = freController.ObterFreteImposto(int.Parse(frvCadImposto.SelectedValue.ToString()));

                if (imposto.FRETE_TAXA_COMBUSTIVEL1 == null)
                {
                    labErroImposto.Text = "Taxa de Combustível não localizada para o mês e ano do recebimento desta nota.";
                    e.Cancel = true;
                    return;
                }

                TextBox txtTaxaDolar = frvCadImposto.FindControl("txtTaxaDolar") as TextBox;
                TextBox txtPeso = frvCadImposto.FindControl("txtPeso") as TextBox;
                TextBox txtValorFrete = frvCadImposto.FindControl("txtValorFrete") as TextBox;
                TextBox txtValorII = frvCadImposto.FindControl("txtValorII") as TextBox;
                TextBox txtValorICMS = frvCadImposto.FindControl("txtValorICMS") as TextBox;
                TextBox txtValorDesembaraco = frvCadImposto.FindControl("txtValorDesembaraco") as TextBox;
                TextBox txtValorReembInfra = frvCadImposto.FindControl("txtValorReembInfra") as TextBox;
                TextBox txtTotalFaturaEmDolar = frvCadImposto.FindControl("txtTotalFaturaEmDolar") as TextBox;
               
                CheckBox chkBoletoOK = frvCadImposto.FindControl("chkBoletoOK") as CheckBox;

                decimal taxaDolar = 0;
                decimal peso = 0;
                decimal valorFaturaEmDolar = 0;
                decimal valorFrete = 0;
                decimal valorII = 0;
                decimal valorICMS = 0;

                Decimal.TryParse(txtTaxaDolar.Text, out taxaDolar);
                Decimal.TryParse(txtPeso.Text, out peso);

                Decimal.TryParse(txtTotalFaturaEmDolar.Text, out valorFaturaEmDolar);
                Decimal.TryParse(txtValorFrete.Text, out valorFrete);

                Decimal.TryParse(txtTaxaDolar.Text, out taxaDolar);
                Decimal.TryParse(txtPeso.Text, out peso);

                Decimal.TryParse(txtTotalFaturaEmDolar.Text, out valorFaturaEmDolar);
                Decimal.TryParse(txtValorFrete.Text, out valorFrete);
                Decimal.TryParse(txtTaxaDolar.Text, out taxaDolar);

                e.NewValues["FRETE_RECEBIMENTO_NOTA"] = imposto.FRETE_RECEBIMENTO_NOTA1.CODIGO;
                e.NewValues["FRETE_TAXA_COMBUSTIVEL"] = imposto.FRETE_TAXA_COMBUSTIVEL1.CODIGO;

                e.NewValues["TAXA_DOLAR"] = txtTaxaDolar.Text.Replace(",", ".");

                e.NewValues["PESO"] = peso.ToString().Replace(",", ".");

                valorII = freController.CalculaValorII(valorFrete, valorFaturaEmDolar, taxaDolar, multiplicadorValorII);
                e.NewValues["VALOR_II"] = valorII.ToString().Replace(",", ".");

                valorFrete = freController.CalculaValorFrete(txtAWB.Text, peso, taxaDolar);
                e.NewValues["VALOR_FRETE"] = valorFrete.ToString().Replace(",", ".");

                e.NewValues["VALOR_DESEMBARACO"] = freController.CalculaValorDesembaraco(taxaDolar, multiplicadorDesembaraco).ToString().Replace(",", ".");

                e.NewValues["VALOR_REEMB_INFRA"] = freController.CalculaValorReembolsoInfra(peso, taxaDolar, multiplicadorReembInfra).ToString().Replace(",", ".");

                e.NewValues["VALOR_FATURA_DOLAR"] = freController.CalculaTotalNotaDolar(imposto.FRETE_RECEBIMENTO_NOTA1).ToString().Replace(",", ".");

                valorICMS = freController.CalculaValorICMS(valorII, valorFrete, valorFaturaEmDolar, taxaDolar, divisorValorICMS, multiplicadorPercentualICMS);
                e.NewValues["VALOR_ICMS"] = valorICMS.ToString().Replace(",", ".");

                e.NewValues["STATUS"] = "1";
                e.NewValues["BOLETO"] = chkBoletoOK.Checked;


            }
            catch (Exception ex)
            {
                e.Cancel = true;
                labErroGeral.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }

        }

        protected void gvFreteImposto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    labErroGeral.Text = "";
                    labErroImposto.Text = "";

                    int CODIGO = int.Parse(((GridView)sender).DataKeys[int.Parse(e.CommandArgument.ToString())][0].ToString());

                    FRETE_IMPOSTO imposto = freController.ObterFreteImposto(CODIGO);

                    odsImposto.SelectParameters["CODIGO"].DefaultValue = CODIGO.ToString();
                    odsImposto.SelectParameters["AWB"].DefaultValue = imposto.FRETE_RECEBIMENTO_NOTA1.AWB;
                    odsImposto.SelectParameters["STATUS"].DefaultValue = "";

                    frvCadImposto.ChangeMode(FormViewMode.Edit);
                    odsImposto.DataBind();

                    VisaoAtualBotoesAcao = EnumVisaoBotoes.SALVAR_CANCELAR;

                    ibtSalvar.Visible = true;
                    ibtCancelar.Visible = true;
                    ibtExcluir.Visible = true;
                    ibtLimpar.Visible = true;
                }
            }
            catch
            {

            }
        }

        protected void frvCadImposto_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {            
            RecarregaGridListagemImpostos();           

            frvCadImposto.ChangeMode(FormViewMode.Insert);
            RecarregaFormularioImpostos();

            odsGridImposto.SelectParameters["CODIGO"].DefaultValue = "0";
            odsGridImposto.SelectParameters["AWB"].DefaultValue = "";
            odsGridImposto.SelectParameters["STATUS"].DefaultValue = "";
            gvFreteImposto.DataBind();

            VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;
            
            labErroGeral.Text = "Imposto inserido com sucesso.";

        }

        protected void frvCadImposto_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {            
            RecarregaGridListagemImpostos();
            frvCadImposto.ChangeMode(FormViewMode.Insert);
            RecarregaFormularioImpostos();

            odsGridImposto.SelectParameters["CODIGO"].DefaultValue = "0";
            odsGridImposto.SelectParameters["AWB"].DefaultValue = "";
            odsGridImposto.SelectParameters["STATUS"].DefaultValue = "";
            gvFreteImposto.DataBind();

            VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;
            
            labErroGeral.Text = "Imposto atualizado com sucesso.";

        }

        #endregion

        private void SelecionarAba(EnumVisaoAbas abaSelecionada)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((int)abaSelecionada).ToString() + "');", true);
            hidTabSelected.Value = ((int)abaSelecionada).ToString();

        }

        /// <summary>
        /// Atualiza o valor total do Desembaraço ao ser digitada a taxa do dólar da Nota.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtTaxaDolar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal taxaDolar = 0;
                TextBox txtAWB = frvCadImposto.FindControl("txtAWB") as TextBox;

                TextBox txtValorReembInfra = frvCadImposto.FindControl("txtValorReembInfra") as TextBox;
                TextBox txtTotalFaturaEmDolar = frvCadImposto.FindControl("txtTotalFaturaEmDolar") as TextBox;

                TextBox txtValorICMS = frvCadImposto.FindControl("txtValorICMS") as TextBox;
                TextBox txtValorII = frvCadImposto.FindControl("txtValorII") as TextBox;

                TextBox txtValorDesembaraco = frvCadImposto.FindControl("txtValorDesembaraco") as TextBox;

                TextBox txtValorTotalFaturaReal = frvCadImposto.FindControl("txtValorTotalFaturaReal") as TextBox;
                TextBox txtValorTotalImposto = frvCadImposto.FindControl("txtValorTotalImposto") as TextBox;
                TextBox txtFreteTaxaCombustivel = frvCadImposto.FindControl("txtFreteTaxaCombustivel") as TextBox;

                TextBox txtValorFrete = frvCadImposto.FindControl("txtValorFrete") as TextBox;
                
                FRETE_RECEBIMENTO_NOTA nota = freController.ObterRecebimentoNota(txtAWB.Text);

                if (nota != null)
                {
                    Decimal.TryParse(((TextBox)sender).Text, out taxaDolar);                    
                    TextBox txtPeso = frvCadImposto.FindControl("txtPeso") as TextBox;

                    txtValorDesembaraco.Text = freController.CalculaValorDesembaraco(taxaDolar, multiplicadorDesembaraco).ToString();
                    Page.SetFocus(txtPeso.ClientID);

                }
                else
                {
                    labErroImposto.Text = "Nota não localizada.";

                    txtTotalFaturaEmDolar.Text = "";
                    txtValorReembInfra.Text = "";
                    txtValorICMS.Text = "";
                    txtValorII.Text = "";
                    txtValorDesembaraco.Text = "";
                    txtValorTotalFaturaReal.Text = "";
                    txtValorTotalImposto.Text = "";
                    txtFreteTaxaCombustivel.Text = "";
                    txtValorFrete.Text = "";
                }
            }
            catch (Exception ex)
            {
                labErroImposto.Text = ex.Message;
            }
        }

        /// <summary>
        /// Atualiza o valor total do Reembolso ao ser digitado o peso da Nota.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtPeso_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labErroImposto.Text = "";

                decimal peso = 0;               
                decimal valorFaturaEmDolar = 0;
                decimal valorDesembaraco = 0;
                decimal valorReembInfra = 0;
                decimal valorFrete = 0;
                decimal taxaDolar = 0;
                decimal valorII = 0;
                decimal valorICMS = 0;

                TextBox txtAWB = frvCadImposto.FindControl("txtAWB") as TextBox;

                TextBox txtTaxaDolar = frvCadImposto.FindControl("txtTaxaDolar") as TextBox;
                TextBox txtValorFrete = frvCadImposto.FindControl("txtValorFrete") as TextBox;

                Decimal.TryParse(((TextBox)sender).Text, out peso);
                Decimal.TryParse(txtTaxaDolar.Text, out taxaDolar);

                TextBox txtValorReembInfra = frvCadImposto.FindControl("txtValorReembInfra") as TextBox;
                TextBox txtTotalFaturaEmDolar = frvCadImposto.FindControl("txtTotalFaturaEmDolar") as TextBox;
               
                TextBox txtValorICMS = frvCadImposto.FindControl("txtValorICMS") as TextBox;
                TextBox txtValorII = frvCadImposto.FindControl("txtValorII") as TextBox;
                
                TextBox txtValorDesembaraco = frvCadImposto.FindControl("txtValorDesembaraco") as TextBox;

                TextBox txtValorTotalFaturaReal = frvCadImposto.FindControl("txtValorTotalFaturaReal") as TextBox;
                TextBox txtValorTotalImposto = frvCadImposto.FindControl("txtValorTotalImposto") as TextBox;
                TextBox txtFreteTaxaCombustivel = frvCadImposto.FindControl("txtFreteTaxaCombustivel") as TextBox;

                CheckBox chkBoletoOK = frvCadImposto.FindControl("chkBoletoOK") as CheckBox;

                FRETE_RECEBIMENTO_NOTA nota = freController.ObterRecebimentoNota(txtAWB.Text);               

                if (nota != null)
                {
                    Decimal.TryParse(txtTaxaDolar.Text, out taxaDolar);
                    Decimal.TryParse(txtValorDesembaraco.Text, out valorDesembaraco);

                    valorReembInfra = freController.CalculaValorReembolsoInfra(peso, taxaDolar, multiplicadorReembInfra);
                    txtValorReembInfra.Text = valorReembInfra.ToString();

                    valorFrete = freController.CalculaValorFrete(txtAWB.Text, peso, taxaDolar);
                    txtValorFrete.Text = valorFrete.ToString();
                    valorFaturaEmDolar = freController.CalculaTotalNotaDolar(nota);
                    txtTotalFaturaEmDolar.Text = valorFaturaEmDolar.ToString();

                    if (txtValorTotalFaturaReal != null)
                        txtValorTotalFaturaReal.Text = freController.CalculaTotalNota(nota, taxaDolar).ToString();

                    if (frvCadImposto.CurrentMode == FormViewMode.Insert && freController.ObterFreteImposto(0, nota.AWB, DateTime.MinValue, "").Count > 0)
                    {
                        labErroImposto.Text = "Imposto já cadastrado na base de dados.";
                    }

                    FRETE_TAXA_COMBUSTIVEL combustivel = freController.ObterTaxaCombustivel(nota.DATA_RECEBIMENTO);

                    if (combustivel != null)
                    {
                        txtFreteTaxaCombustivel.Text = combustivel.VALOR_TAXA.ToString();
                    }
                    else
                    {
                        labErroImposto.Text = "Taxa de Combustível não localizada para o mês e ano do recebimento desta nota.";
                    }

                    //CALCULA VALOR II
                    valorII = freController.CalculaValorII(valorFrete, valorFaturaEmDolar, taxaDolar, multiplicadorValorII);

                    txtValorII.Text = valorII.ToString();

                    //CALCULA VALOR ICMS
                    valorICMS = freController.CalculaValorICMS(valorII, valorFrete, valorFaturaEmDolar, taxaDolar, divisorValorICMS, multiplicadorPercentualICMS);
                    txtValorICMS.Text = valorICMS.ToString();

                    if (txtValorTotalImposto != null)
                        txtValorTotalImposto.Text = freController.CalculaTotalImposto(valorII, valorICMS, valorDesembaraco, valorReembInfra).ToString();

                    Page.SetFocus(chkBoletoOK.ClientID);

                }
                else
                {
                    labErroImposto.Text = "Nota não localizada.";

                    txtTotalFaturaEmDolar.Text = "";
                    txtValorReembInfra.Text = "";
                    txtValorICMS.Text = "";
                    txtValorII.Text = "";
                    txtValorDesembaraco.Text = "";
                    txtValorTotalFaturaReal.Text = "";
                    txtValorTotalImposto.Text = "";
                    txtFreteTaxaCombustivel.Text = "";
                    txtValorFrete.Text = "";
                }

            }
            catch (Exception ex)
            {
                labErroImposto.Text = ex.Message;
            }
        }        

        protected void frvCadImposto_DataBound(object sender, EventArgs e)
        {
            TextBox txtAWB = frvCadImposto.FindControl("txtAWB") as TextBox;
            TextBox txtTotalFaturaEmDolar = frvCadImposto.FindControl("txtTotalFaturaEmDolar") as TextBox;
            CheckBox chkBoletoOK = frvCadImposto.FindControl("chkBoletoOK") as CheckBox;
            TextBox txtFreteTaxaCombustivel = frvCadImposto.FindControl("txtFreteTaxaCombustivel") as TextBox;

            if (txtAWB != null && txtAWB.Text != "")
            {
                FRETE_IMPOSTO imposto = freController.ObterFreteImposto(0, txtAWB.Text, DateTime.MinValue, null).FirstOrDefault();

                TextBox txtValorTotalFaturaReal = frvCadImposto.FindControl("txtValorTotalFaturaReal") as TextBox;
                TextBox txtValorTotalImposto = frvCadImposto.FindControl("txtValorTotalImposto") as TextBox;

                if (txtValorTotalImposto != null)
                    txtValorTotalImposto.Text = freController.CalculaTotalImposto(imposto.VALOR_II, imposto.VALOR_ICMS, imposto.VALOR_DESEMBARACO, imposto.VALOR_REEMB_INFRA).ToString();
                
                txtTotalFaturaEmDolar.Text = freController.CalculaTotalNotaDolar(imposto.FRETE_RECEBIMENTO_NOTA1).ToString();

                if (txtValorTotalFaturaReal != null)
                    txtValorTotalFaturaReal.Text = freController.CalculaTotalNota(imposto.FRETE_RECEBIMENTO_NOTA1, imposto.TAXA_DOLAR).ToString();

                FRETE_TAXA_COMBUSTIVEL combustivel = freController.ObterTaxaCombustivel(imposto.FRETE_RECEBIMENTO_NOTA1.DATA_RECEBIMENTO);

                if (combustivel != null)
                {
                    txtFreteTaxaCombustivel.Text = combustivel.VALOR_TAXA.ToString();
                }
                else
                {
                    labErroImposto.Text = "Taxa de Combustível não localizada para o mês e ano do recebimento desta nota.";
                }

                chkBoletoOK.Checked = imposto.BOLETO;

            }
        }
    }
}