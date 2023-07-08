using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios.mod_frete
{
    public partial class fre_cad_tipo_pagamento : System.Web.UI.Page
    {
        FreteController freController = new FreteController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RecarregarTela();

            }
            btIncluir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btIncluir, null) + ";");
            btCancelar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btCancelar, null) + ";");

            CriaCabecalhoRodapeAtualizaScript();
        }

        #region "CRUD"
        private void Incluir(FRETE_TIPO_PAGAMENTO _param)
        {
            freController.InserirTipoPagamento(_param);
        }

        private void Atualizar(FRETE_TIPO_PAGAMENTO _param)
        {
            freController.AtualizarTipoPagamento(_param);
        }
        private void Excluir(int CODIGO)
        {
            freController.ExcluirTipoPagamento(CODIGO);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            btCancelar.Visible = (txtDescricao.Text != "");

            try
            {
                List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
                PreparaControlesTela(ref list);

                if (!Utils.WebControls.ValidarCampos(ref list))
                {
                    labErro.Text = "Preencha corretamente os campos em vermelho.";

                    CriaCabecalhoRodapeAtualizaScript();
                    return;
                }

                if (hidCodigo.Value == "1")
                {
                    labErro.Text = "Não é permitido a inclusão/alteração do Tipo de Pagamento 'PRÉ PAGO'.";
                    CriaCabecalhoRodapeAtualizaScript();
                    return;
                }                

                FRETE_TIPO_PAGAMENTO _novo;

                if (((Button)sender).CommandArgument == "I")
                    _novo = freController.ObterTipoPagamento(txtDescricao.Text.Trim());
                else
                    _novo = freController.ObterTipoPagamento(int.Parse(hidCodigo.Value));

                if (_novo == null)
                {
                    _novo = new FRETE_TIPO_PAGAMENTO();

                    _novo.DESCRICAO = txtDescricao.Text.Trim();
                    _novo.STATUS = Convert.ToChar(ddlStatus.SelectedValue);

                    if (((Button)sender).CommandArgument == "I")
                    {
                        Incluir(_novo);
                        RecarregarTela();
                        labErro.Text = "Registro inserido com sucesso.";
                    }
                }
                else if (((Button)sender).CommandArgument == "A")
                {
                    _novo.DESCRICAO = txtDescricao.Text.Trim();
                    _novo.STATUS = Convert.ToChar(ddlStatus.SelectedValue);

                    Atualizar(_novo);
                    RecarregarTela();
                    labErro.Text = "Registro atualizado com sucesso.";
                }
                else
                {
                    labErro.Text = "Registro já cadastrado na base de dados.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }

            CriaCabecalhoRodapeAtualizaScript();

        }

        protected void btAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                Button b = (Button)sender;

                int CODIGO = Convert.ToInt32(b.CommandArgument.Trim());

                if (CODIGO == 1)
                {
                    labErro.Text = "Não é permitido a alteração do Tipo de Pagamento 'PRÉ PAGO'.";
                }
                else
                {
                    FRETE_TIPO_PAGAMENTO _tipoPagamento = freController.ObterTipoPagamento(CODIGO);

                    ddlStatus.SelectedValue = _tipoPagamento.STATUS.ToString();
                    txtDescricao.Text = _tipoPagamento.DESCRICAO;

                    btCancelar.Visible = true;
                    btIncluir.CommandArgument = "A";
                    hidCodigo.Value = CODIGO.ToString();

                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

            CriaCabecalhoRodapeAtualizaScript();

        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            labErro.Text = "";
            try
            {
                try
                {
                    int CODIGO = int.Parse(b.CommandArgument.Trim());

                    if (CODIGO == 1)
                    {
                        labErro.Text = "Não é permitido a exclusão do Tipo de Pagamento 'PRÉ PAGO'.";
                    }
                    else
                    {
                        Excluir(CODIGO);
                        RecarregarTela();
                    }
                }
                catch (Exception)
                {
                    labErro.Text = "Este registro não pode ser excluído.";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }

            CriaCabecalhoRodapeAtualizaScript();
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            btCancelar.Visible = false;
            btIncluir.CommandArgument = "I";
            labErro.Text = "";

            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
            PreparaControlesTela(ref list);
            Utils.WebControls.LimparControles(ref list);
            CriaCabecalhoRodapeAtualizaScript();

        }
        #endregion

        private void RecarregarTela()
        {
            btCancelar.Visible = false;
            btIncluir.CommandArgument = "I";

            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
            PreparaControlesTela(ref list);
            Utils.WebControls.LimparControles(ref list);
            AtualizaGrid();

        }

        private void AtualizaGrid()
        {
            Utils.WebControls.GetBoundFieldIndexByName(gvCadTipoPagamento, " - >>");
            Utils.WebControls.GetBoundFieldIndexByName(gvCadTipoPagamento, " - <<");

            gvCadTipoPagamento.DataSource = freController.ObterTipoPagamento();
            gvCadTipoPagamento.DataBind();

            CriaCabecalhoRodapeAtualizaScript();

        }

        private void PreparaControlesTela(ref List<Utils.WebControls.ControlesValidacao> list)
        {
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labDescricao, Controle = txtDescricao, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labStatus, Controle = ddlStatus, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErro, CorPadrao = Color.Gray, ValorPadrao = "" });

        }

        private void CriaCabecalhoRodapeAtualizaScript()
        {
            var gridView = gvCadTipoPagamento;
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