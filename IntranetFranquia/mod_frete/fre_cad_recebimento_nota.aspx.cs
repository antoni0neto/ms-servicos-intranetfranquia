using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;

namespace Relatorios.mod_frete
{
    public partial class fre_cad_recebimento_nota : System.Web.UI.Page
    {
        FreteController freController = new FreteController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregaDropDownListTipoEnvio();
                CarregaDropDownListTipoPagamento();
                CarregaDropDownListTipoAmostra();
                CarregaDropDownListGrupoProduto();
                CarregaDropDownListGriffe();
                CarregaTelaInicial();

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataRecebimento.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            //SelecionarAba(EnumVisaoAbas.NOTA_CADASTRO);
            gvFreteRecebimentoNota.RowCommand += gvFreteRecebimentoNota_RowCommand;
            gvFreteRecebimentoItem.RowCommand += gvFreteRecebimentoItem_RowCommand;

        }


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
            NOTA_CADASTRO = 0,
            NOTA_LISTAGEM = 1
        }

        public enum EnumVisaoBotoes
        {
            NAO_DEFINIDO = -1,
            BUSCAR_NOVO = 0,
            SALVAR_CANCELAR = 1
        }


        #endregion

        #region "Carrega Dropdownlist"
        private void CarregaDropDownListTipoEnvio()
        {
            ddlTipoEnvio.DataSource = freController.ObterTipoEnvioAtivo();
            ddlTipoEnvio.DataBind();
            ddlTipoEnvio.Items.Insert(0, new ListItem("Selecione", "0"));
        }
        private void CarregaDropDownListTipoAmostra()
        {
            ddlTipoAmostra.DataSource = freController.ObterTipoAmostraAtivo();
            ddlTipoAmostra.DataBind();
            ddlTipoAmostra.Items.Insert(0, new ListItem("Selecione", "0"));
        }
        private void CarregaDropDownListGriffe()
        {
            ddlGriffe.DataSource = baseController.BuscaGriffes();
            ddlGriffe.DataBind();
            ddlGriffe.Items.Insert(0, new ListItem("Selecione", "0"));
        }
        private void CarregaDropDownListGrupoProduto()
        {
            ddlGrupoProduto.DataSource = baseController.BuscaGrupos();
            ddlGrupoProduto.DataBind();
            ddlGrupoProduto.Items.Insert(0, new ListItem("Selecione", "0"));
        }
        private void CarregaDropDownListTipoPagamento()
        {
            ddlTipoPagamento.DataSource = freController.ObterTipoPagamentoAtivo();
            ddlTipoPagamento.DataBind();
            ddlTipoPagamento.Items.Insert(0, new ListItem("Selecione", "0"));
        }
        #endregion

        #region "Carrega Tela Inicial"
        /// <summary>
        /// Carrega Formulários e botões para primeiro início
        /// </summary>
        private void CarregaTelaInicial()
        {
            ibtNovo_Click(null, null);
            VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;
            CarregaGridFreteRecebimentoNota("", DateTime.MinValue, 0, 0);
        }

        #endregion

        #region "Carrega controles para validação na tela"

        /// <summary>
        /// Carrega Validação para os campos da Nota e Labels relacionados
        /// </summary>
        /// <param name="list"></param>
        private void CarregaControleValidacaoNota(ref List<Utils.WebControls.ControlesValidacao> list)
        {
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labAWB, Controle = txtAWB, TamanhoMinimo = 12, TamanhoMaximo = 12, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labDataRecebimento, Controle = txtDataRecebimento, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labTipoEnvio, Controle = ddlTipoEnvio, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labTipoPagamento, Controle = ddlTipoPagamento, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });

            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErroGeral, CorPadrao = Color.Gray, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErroNota, CorPadrao = Color.Gray, ValorPadrao = "" });

        }

        /// <summary>
        /// Carrega Validação para os campos do Item da Nota e Labels relacionados
        /// </summary>
        /// <param name="list"></param>
        private void CarregaControleValidacaoItem(ref List<Utils.WebControls.ControlesValidacao> list)
        {
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labTipoAmostra, Controle = ddlTipoAmostra, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labGrupoProduto, Controle = ddlGrupoProduto, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labGriffe, Controle = ddlGriffe, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labQtdePecas, Controle = txtQtdePecas, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labValorUnitario, Controle = txtValorUnitario, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labStatus, Controle = ddlStatus, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labProduto, Controle = txtProduto, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });

            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErroGeral, CorPadrao = Color.Gray, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErroItem, CorPadrao = Color.Gray, ValorPadrao = "" });

        }

        #endregion

        #region "CRUD"
        private int IncluirRecebimentoNota(FRETE_RECEBIMENTO_NOTA _param)
        {
            return freController.InserirRecebimentoNota(_param);
        }
        private void AtualizarRecebimentoNota(FRETE_RECEBIMENTO_NOTA _param)
        {
            freController.AtualizarRecebimentoNota(_param);
        }
        private void ExcluirRecebimentoNota(int CODIGO)
        {
            freController.ExcluirRecebimentoNota(CODIGO);
        }
        private int IncluirRecebimentoItem(FRETE_RECEBIMENTO_ITEM _param)
        {
            return freController.InserirRecebimentoNota(_param);
        }
        private void AtualizarRecebimentoItem(FRETE_RECEBIMENTO_ITEM _param)
        {
            freController.AtualizarRecebimenItem(_param);
        }
        private void ExcluirRecebimentoItem(int CODIGO)
        {
            freController.ExcluirRecebimentoItem(CODIGO);
        }
        #endregion

        #region "Ações do Usuário"
        protected void ibtSalvar_Click(object sender, ImageClickEventArgs e)
        {
            labErroNota.Text = "";

            try
            {
                List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
                CarregaControleValidacaoNota(ref list);

                if (!Utils.WebControls.ValidarCampos(ref list))
                {
                    labErroNota.Text = "Preencha corretamente os campos em vermelho.";
                    return;
                }

                FRETE_RECEBIMENTO_NOTA _novo;

                if (((ImageButton)sender).CommandArgument == "I")
                    _novo = freController.ObterRecebimentoNota(txtAWB.Text);
                else
                    _novo = freController.ObterRecebimentoNota(int.Parse(hidPKNota.Value));

                if (_novo == null)
                {
                    _novo = new FRETE_RECEBIMENTO_NOTA();

                    _novo.AWB = txtAWB.Text;
                    _novo.DATA_RECEBIMENTO = DateTime.Parse(txtDataRecebimento.Text.Trim());
                    _novo.FRETE_TIPO_ENVIO = int.Parse(ddlTipoEnvio.SelectedValue);
                    _novo.FRETE_TIPO_PAGAMENTO = int.Parse(ddlTipoPagamento.SelectedValue);
                    _novo.OBS = txtObsNota.Text.Trim();
                    _novo.DATA_INCLUSAO = DateTime.Now;
                    _novo.USUARIO_RECEBIMENTO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO; //Utils.SessionManager.Obter<USUARIO>("USUARIO_LOGADO").CODIGO_USUARIO;

                    if (((ImageButton)sender).CommandArgument == "I")
                    {
                        IncluirRecebimentoNota(_novo);
                        CarregaTelaInicial();
                        labErroGeral.Text = "Registro inserido com sucesso.";

                    }
                }
                else if (((ImageButton)sender).CommandArgument == "A")
                {
                    //NOTAS QUE POSSUEM IMPOSTO JÁ CALCULADO NÃO PODEM SER EDITADAS
                    if (freController.ObterFreteImposto(0, _novo.AWB, DateTime.MinValue, "").Count > 0)
                    {
                        labErroNota.Text = "Não é permitida alteração, pois já existe um imposto calculado para esta nota.";
                        return;
                    }

                    _novo.AWB = txtAWB.Text;
                    _novo.DATA_RECEBIMENTO = DateTime.Parse(txtDataRecebimento.Text.Trim());
                    _novo.FRETE_TIPO_ENVIO = int.Parse(ddlTipoEnvio.SelectedValue);
                    _novo.FRETE_TIPO_PAGAMENTO = int.Parse(ddlTipoPagamento.SelectedValue);
                    _novo.OBS = txtObsNota.Text.Trim();

                    AtualizarRecebimentoNota(_novo);
                    CarregaTelaInicial();
                    labErroGeral.Text = "Registro atualizado com sucesso.";

                }
                else
                {
                    labErroNota.Text = "Nota já cadastrada na base de dados.";
                    return;
                }


            }
            catch (Exception ex)
            {
                labErroGeral.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }

        /// <summary>
        /// Muda tela para a visão de Busca/Novo Produto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtCancelar_Click(object sender, ImageClickEventArgs e)
        {
            ibtSalvar.CommandArgument = "I";
            ibtExcluir.Visible = false;
            hidPKNota.Value = "0";

            VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;

            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();

            CarregaControleValidacaoNota(ref list);
            CarregaControleValidacaoItem(ref list);
            Utils.WebControls.LimparControles(ref list);

            ddlTipoAmostra.SelectedIndex = -1;
            ddlGrupoProduto.SelectedIndex = -1;
            ddlGriffe.SelectedIndex = -1;
            txtQtdePecas.Text = "";
            txtValorUnitario.Text = "";
            ddlStatus.SelectedIndex = -1;
            labDescricaoProduto.Text = "";
            txtProduto.Text = "";
            txtObsNota.Text = "";

            labErroNota.Text = "";
            labErroGeral.Text = "";

            gvFreteRecebimentoItem.DataSource = new List<FRETE_RECEBIMENTO_ITEM>();
            gvFreteRecebimentoItem.DataBind();

        }

        /// <summary>
        /// Exclui uma Nota diretamente no formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtExcluirNotaFormulario(object sender, ImageClickEventArgs e)
        {
            labErroNota.Text = "";
            try
            {
                try
                {
                    int CODIGO = int.Parse(hidPKNota.Value);

                    if (hidPKNota.Value == "0")
                    {
                        labErroNota.Text = "Selecione uma nota antes de excluir.";
                        return;
                    }

                    FRETE_RECEBIMENTO_NOTA nota = freController.ObterRecebimentoNota(CODIGO);

                    if (freController.ObterFreteImposto(0, nota.AWB, DateTime.MinValue, "").Count > 0)
                    {
                        labErroNota.Text = "Não é permitida exclusão, pois já existe um imposto calculado para esta nota.";
                        return;
                    }

                    ExcluirRecebimentoNota(CODIGO);
                    CarregaTelaInicial();
                    ibtExcluir.Visible = false;
                    VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;
                    labErroNota.Text = "Nota excluída com sucesso.";

                }
                catch (Exception)
                {
                    labErroGeral.Text = "Este registro não pode ser excluído.";
                }

            }
            catch (Exception ex)
            {
                labErroGeral.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }

        protected void ibtLimpar_Click(object sender, ImageClickEventArgs e)
        {
            ibtNovo_Click(null, null);
            VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;

        }

        protected void ibtPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            DateTime DATA_RECEBIMENTO = DateTime.MinValue;
            int FRETE_TIPO_ENVIO = 0;
            int FRETE_TIPO_PAGAMENTO = 0;

            DateTime.TryParse(txtDataRecebimento.Text.Trim(), out DATA_RECEBIMENTO);
            int.TryParse(ddlTipoEnvio.SelectedValue, out FRETE_TIPO_ENVIO);
            int.TryParse(ddlTipoPagamento.SelectedValue, out FRETE_TIPO_PAGAMENTO);

            List<FRETE_RECEBIMENTO_NOTA> listNotas = freController.ObterRecebimentoNota(txtAWB.Text, DATA_RECEBIMENTO, FRETE_TIPO_ENVIO, FRETE_TIPO_PAGAMENTO);

            if (listNotas.Count == 1)
            {
                FRETE_RECEBIMENTO_NOTA nota = listNotas.FirstOrDefault();

                hidPKNota.Value = nota.CODIGO.ToString();

                txtAWB.Text = nota.AWB;
                txtDataRecebimento.Text = nota.DATA_RECEBIMENTO.ToString("dd/MM/yyyy");

                if (nota.FRETE_TIPO_ENVIO1.STATUS == '0')
                {
                    ddlTipoEnvio.DataSource = freController.ObterTipoEnvio();
                    ddlTipoEnvio.DataBind();
                    ddlTipoEnvio.Items.Insert(0, new ListItem("Selecione", "0"));
                }

                ddlTipoEnvio.SelectedValue = nota.FRETE_TIPO_ENVIO.ToString();

                if (nota.FRETE_TIPO_PAGAMENTO1.STATUS == '0')
                {
                    ddlTipoPagamento.DataSource = freController.ObterTipoPagamento();
                    ddlTipoPagamento.DataBind();
                    ddlTipoPagamento.Items.Insert(0, new ListItem("Selecione", "0"));
                }

                ddlTipoPagamento.SelectedValue = nota.FRETE_TIPO_PAGAMENTO.ToString();
                txtObsNota.Text = nota.OBS;

                ibtSalvar.CommandArgument = "A";

                List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
                CarregaControleValidacaoNota(ref list);
                Utils.WebControls.ValidarCampos(ref list);
                CarregaGridRecebimentoItemsNota(nota.CODIGO);

                SelecionarAba(EnumVisaoAbas.NOTA_CADASTRO);
                VisaoAtualBotoesAcao = EnumVisaoBotoes.SALVAR_CANCELAR;

                ibtSalvar.Visible = true;
                ibtCancelar.Visible = true;
                ibtExcluir.Visible = true;

            }
            else
            {
                SelecionarAba(EnumVisaoAbas.NOTA_LISTAGEM);
            }

            gvFreteRecebimentoNota.DataSource = listNotas;
            gvFreteRecebimentoNota.DataBind();

        }

        /// <summary>
        /// Exibe o nome do produto após inserção de código caixa de texto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtProduto_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt != null)
            {
                try
                {
                    PRODUTO produto = new PRODUTO();
                    produto = baseController.BuscaProduto(txt.Text.Trim());

                    if (produto != null)
                    {
                        labDescricaoProduto.Text = produto.DESC_PRODUTO.Trim();
                    }
                    else
                    {
                        labDescricaoProduto.Text = "Produto não encontrado.";
                        txt.Text = "";
                    }
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Limpa os campos relacionados ao Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCancelarItem_Click(object sender, ImageClickEventArgs e)
        {
            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();

            CarregaControleValidacaoItem(ref list);

            Utils.WebControls.LimparControles(ref list);

            //hidPKItem.Value = "0";
            ddlTipoAmostra.SelectedIndex = -1;
            ddlGrupoProduto.SelectedIndex = -1;
            ddlGriffe.SelectedIndex = -1;
            txtQtdePecas.Text = "";
            txtValorUnitario.Text = "";
            ddlStatus.SelectedIndex = -1;
            labDescricaoProduto.Text = "";
            txtProduto.Text = "";
            txtObsItem.Text = "";

            labErroGeral.Text = "";
            labErroItem.Text = "";


        }

        /// <summary>
        /// Armazena um item em memória e atualiza o Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btSalvarAtualizarItem_Click(object sender, ImageClickEventArgs e)
        {
            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();

            CarregaControleValidacaoNota(ref list);
            CarregaControleValidacaoItem(ref list);

            string AWB = txtAWB.Text;
            string PRODUTO = txtProduto.Text.Trim();
            string valorUnitario = txtValorUnitario.Text;
            string qtdePecas = txtQtdePecas.Text;
            string status = ddlStatus.SelectedItem.Value;
            string griffe = ddlGriffe.SelectedValue;
            string grupoProduto = ddlGrupoProduto.SelectedValue;
            string tipoAmostra = ddlTipoAmostra.SelectedValue;
            string obsItem = txtObsItem.Text.Trim();

            if (!Utils.WebControls.ValidarCampos(ref list))
            {
                labErroItem.Text = "Preencha corretamente os campos em vermelho.";
                return;
            }

            if (hidPKNota.Value == "0")
            {
                ibtSalvar_Click(ibtSalvar, null);

            }

            FRETE_RECEBIMENTO_NOTA _novo;

            if (ibtSalvar.CommandArgument == "I")
                _novo = freController.ObterRecebimentoNota(AWB);
            else
                _novo = freController.ObterRecebimentoNota(int.Parse(hidPKNota.Value));

            if (_novo == null)
                return;

            if (freController.ObterFreteImposto(0, _novo.AWB, DateTime.MinValue, "").Count > 0)
            {
                labErroItem.Text = "Não é permitida alteração deste item, pois já existe um imposto calculado para esta nota.";
                return;
            }

            FRETE_RECEBIMENTO_ITEM item = freController.ObterRecebimentoItem(int.Parse(hidPKNota.Value), PRODUTO);

            if (item == null)
                item = new FRETE_RECEBIMENTO_ITEM();

            if (hidPKNota.Value == "0")
            {
                hidPKNota.Value = _novo.CODIGO.ToString();

                txtAWB.Text = _novo.AWB;
                txtDataRecebimento.Text = _novo.DATA_RECEBIMENTO.ToString("dd/MM/yyyy");

                if (_novo.FRETE_TIPO_ENVIO1.STATUS == '0')
                {
                    ddlTipoEnvio.DataSource = freController.ObterTipoEnvio();
                    ddlTipoEnvio.DataBind();
                    ddlTipoEnvio.Items.Insert(0, new ListItem("Selecione", "0"));
                }

                ddlTipoEnvio.SelectedValue = _novo.FRETE_TIPO_ENVIO.ToString();

                if (_novo.FRETE_TIPO_PAGAMENTO1.STATUS == '0')
                {
                    ddlTipoPagamento.DataSource = freController.ObterTipoPagamento();
                    ddlTipoPagamento.DataBind();
                    ddlTipoPagamento.Items.Insert(0, new ListItem("Selecione", "0"));
                }

                ddlTipoPagamento.SelectedValue = _novo.FRETE_TIPO_PAGAMENTO.ToString();
                txtObsNota.Text = _novo.OBS;

                ibtSalvar.CommandArgument = "A";

                SelecionarAba(EnumVisaoAbas.NOTA_CADASTRO);
                VisaoAtualBotoesAcao = EnumVisaoBotoes.SALVAR_CANCELAR;

            }

            item.FRETE_RECEBIMENTO_NOTA = _novo.CODIGO;
            item.FRETE_TIPO_AMOSTRA = int.Parse(tipoAmostra);
            item.GRUPO_PRODUTO = grupoProduto;
            item.PRODUTO = PRODUTO;
            item.GRIFFE = griffe;
            item.VALOR_UNITARIO = Decimal.Parse(valorUnitario);
            item.QTDE_PECAS = int.Parse(qtdePecas);
            item.STATUS = Char.Parse(status);
            item.OBS = obsItem;

            btCancelarItem_Click(null, null);

            if (item.CODIGO > 0)
            {
                AtualizarRecebimentoItem(item);
                labErroItem.Text = "Item atualizado com sucesso.";

            }
            else
            {
                IncluirRecebimentoItem(item);
                labErroItem.Text = "Item inserido com sucesso.";

            }
            gvFreteRecebimentoItem.DataSource = freController.ObterRecebimentoItemsPorNota(_novo.CODIGO);
            gvFreteRecebimentoItem.DataBind();

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
            VisaoAtualBotoesAcao = EnumVisaoBotoes.SALVAR_CANCELAR;

            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();

            CarregaControleValidacaoNota(ref list);
            CarregaControleValidacaoItem(ref list);

            Utils.WebControls.LimparControles(ref list);

            hidPKNota.Value = "0";

            ddlTipoAmostra.SelectedIndex = -1;
            ddlGrupoProduto.SelectedIndex = -1;
            ddlGriffe.SelectedIndex = -1;
            txtQtdePecas.Text = "";
            txtValorUnitario.Text = "";
            ddlStatus.SelectedIndex = -1;
            labDescricaoProduto.Text = "";
            txtProduto.Text = "";
            txtObsNota.Text = "";

            ddlTipoAmostra.SelectedIndex = -1;
            ddlGrupoProduto.SelectedIndex = -1;
            ddlGriffe.SelectedIndex = -1;
            txtQtdePecas.Text = "";
            txtValorUnitario.Text = "";
            ddlStatus.SelectedIndex = -1;
            txtObsItem.Text = "";

            labErroGeral.Text = "";
            labErroNota.Text = "";
            labErroItem.Text = "";

            gvFreteRecebimentoItem.DataSource = new List<FRETE_RECEBIMENTO_ITEM>();
            gvFreteRecebimentoItem.DataBind();

        }

        /// <summary>
        /// Excluir um Item de Nota
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btExcluirItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int CODIGO = int.Parse(((ImageButton)sender).CommandArgument);

                FRETE_RECEBIMENTO_NOTA nota = freController.ObterRecebimentoNota(int.Parse(hidPKNota.Value));

                if (freController.ObterFreteImposto(0, nota.AWB, DateTime.MinValue, "").Count > 0)
                {
                    labErroItem.Text = "Não é permitido a exclusão deste item, pois já existe um imposto calculado para esta nota.";
                    return;
                }

                FRETE_RECEBIMENTO_ITEM item = freController.ObterRecebimentoItem(CODIGO);

                if (item != null)
                {
                    ExcluirRecebimentoItem(item.CODIGO);

                    gvFreteRecebimentoItem.DataSource = freController.ObterRecebimentoItemsPorNota(int.Parse(hidPKNota.Value));
                    gvFreteRecebimentoItem.DataBind();

                    labErroItem.Text = "Item excluído com sucesso.";
                }
            }
            catch (Exception)
            {
                labErroNota.Text = "Este registro não pode ser excluído.";
            }
        }

        /// <summary>
        /// Exclui uma Nota diretamente no GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btExcluirNotaGridView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int CODIGO = int.Parse(((ImageButton)sender).CommandArgument);

                FRETE_RECEBIMENTO_NOTA nota = freController.ObterRecebimentoNota(CODIGO);

                if (nota != null)
                {
                    ExcluirRecebimentoNota(nota.CODIGO);
                    gvFreteRecebimentoNota.DataSource = freController.ObterRecebimentoNota();
                    gvFreteRecebimentoNota.DataBind();

                    VisaoAtualBotoesAcao = EnumVisaoBotoes.BUSCAR_NOVO;
                    ibtExcluir.Visible = false;
                    SelecionarAba(EnumVisaoAbas.NOTA_LISTAGEM);

                    labErroGeral.Text = "Nota excluída com sucesso.";

                }

            }
            catch (Exception)
            {
                SelecionarAba(EnumVisaoAbas.NOTA_LISTAGEM);
                labErroGeral.Text = "Este registro não pode ser excluído.";
            }


        }

        #endregion

        public void CarregaGridFreteRecebimentoNota(string AWB, DateTime DATA_RECEBIMENTO, int FRETE_TIPO_ENVIO, int FRETE_TIPO_PAGAMENTO)
        {
            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
            CarregaControleValidacaoNota(ref list);
            Utils.WebControls.LimparControles(ref list);

            gvFreteRecebimentoNota.DataSource = freController.ObterRecebimentoNota(AWB, DATA_RECEBIMENTO, FRETE_TIPO_ENVIO, FRETE_TIPO_PAGAMENTO);
            gvFreteRecebimentoNota.DataBind();

            SelecionarAba(EnumVisaoAbas.NOTA_LISTAGEM);

        }
        public void CarregaGridRecebimentoItemsNota(int FRETE_RECEBIMENTO_NOTA)
        {
            List<FRETE_RECEBIMENTO_ITEM> listItemsNota = freController.ObterRecebimentoItemsPorNota(FRETE_RECEBIMENTO_NOTA);

            gvFreteRecebimentoItem.DataSource = freController.ObterRecebimentoItemsPorNota(FRETE_RECEBIMENTO_NOTA);
            gvFreteRecebimentoItem.DataBind();

            btCancelarItem_Click(null, null);

        }

        #region "Ações do GridView"

        /// <summary>
        /// Executa uma ação no Grid de Notas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gvFreteRecebimentoNota_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    labErroGeral.Text = "";
                    labErroNota.Text = "";

                    int CODIGO = int.Parse(((GridView)sender).DataKeys[int.Parse(e.CommandArgument.ToString())][0].ToString());

                    FRETE_RECEBIMENTO_NOTA nota = freController.ObterRecebimentoNota(CODIGO);

                    if (nota != null)
                    {
                        if (freController.ObterFreteImposto(0, nota.AWB, DateTime.MinValue, "").Count > 0)
                        {
                            labErroNota.Text = "Não é permitida alteração, pois já existe um imposto calculado para esta nota.";

                        }

                        hidPKNota.Value = CODIGO.ToString();

                        txtAWB.Text = nota.AWB;
                        txtDataRecebimento.Text = nota.DATA_RECEBIMENTO.ToString("dd/MM/yyyy");

                        if (nota.FRETE_TIPO_ENVIO1.STATUS == '0')
                        {
                            ddlTipoEnvio.DataSource = freController.ObterTipoEnvio();
                            ddlTipoEnvio.DataBind();
                            ddlTipoEnvio.Items.Insert(0, new ListItem("Selecione", "0"));
                        }

                        ddlTipoEnvio.SelectedValue = nota.FRETE_TIPO_ENVIO.ToString();

                        if (nota.FRETE_TIPO_PAGAMENTO1.STATUS == '0')
                        {
                            ddlTipoPagamento.DataSource = freController.ObterTipoPagamento();
                            ddlTipoPagamento.DataBind();
                            ddlTipoPagamento.Items.Insert(0, new ListItem("Selecione", "0"));
                        }

                        ddlTipoPagamento.SelectedValue = nota.FRETE_TIPO_PAGAMENTO.ToString();
                        txtObsNota.Text = nota.OBS;

                        ibtSalvar.CommandArgument = "A";

                        List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
                        CarregaControleValidacaoNota(ref list);
                        Utils.WebControls.ValidarCampos(ref list);
                        CarregaGridRecebimentoItemsNota(CODIGO);

                        SelecionarAba(EnumVisaoAbas.NOTA_CADASTRO);
                        VisaoAtualBotoesAcao = EnumVisaoBotoes.SALVAR_CANCELAR;

                        ibtSalvar.Visible = true;
                        ibtCancelar.Visible = true;
                        ibtExcluir.Visible = true;
                        ibtLimpar.Visible = true;

                    }
                }

            }
            catch
            {

            }

        }

        /// <summary>
        /// Executa uma ação no Grid de Items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gvFreteRecebimentoItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = 0;

            try
            {
                if (e.CommandName == "Select")
                {
                    int CODIGO = int.Parse(((GridView)sender).DataKeys[int.Parse(e.CommandArgument.ToString())].Values[index].ToString());

                    FRETE_RECEBIMENTO_ITEM item = freController.ObterRecebimentoItem(CODIGO);

                    if (item != null)
                    {
                        if (freController.ObterFreteImposto(0, item.FRETE_RECEBIMENTO_NOTA1.AWB, DateTime.MinValue, "").Count > 0)
                        {
                            labErroItem.Text = "Não é permitida alteração deste item, pois já existe um imposto calculado para esta nota.";

                        }

                        ddlGrupoProduto.SelectedValue = item.GRUPO_PRODUTO;

                        if (item.FRETE_TIPO_AMOSTRA1.STATUS == '0')
                        {
                            ddlTipoAmostra.DataSource = freController.ObterTipoAmostra();
                            ddlTipoAmostra.DataBind();
                            ddlTipoAmostra.Items.Insert(0, new ListItem("Selecione", "0"));
                        }
                        ddlTipoAmostra.SelectedValue = item.FRETE_TIPO_AMOSTRA.ToString();
                        ddlGriffe.SelectedValue = item.GRIFFE;
                        txtQtdePecas.Text = item.QTDE_PECAS.ToString();
                        ddlStatus.SelectedValue = item.STATUS.ToString();
                        txtValorUnitario.Text = item.VALOR_UNITARIO.ToString();
                        txtProduto.Text = item.PRODUTO;
                        txtObsItem.Text = item.OBS;
                    }
                }
            }
            catch (Exception ex)
            {
                labErroGeral.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }

        #endregion

        private void SelecionarAba(EnumVisaoAbas abaSelecionada)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((int)abaSelecionada).ToString() + "');", true);
            hidTabSelected.Value = ((int)abaSelecionada).ToString();

        }
    }

}