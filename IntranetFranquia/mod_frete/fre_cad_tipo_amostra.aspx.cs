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
    public partial class fre_cad_tipo_amostra : System.Web.UI.Page
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
        private void Incluir(FRETE_TIPO_AMOSTRA _param)
        {
            freController.InserirTipoAmostra(_param);
        }

        private void Atualizar(FRETE_TIPO_AMOSTRA _param)
        {
            freController.AtualizarTipoAmostra(_param);
        }
        private void Excluir(int CODIGO)
        {
            freController.ExcluirTipoAmostra(CODIGO);
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

                FRETE_TIPO_AMOSTRA _novo;

                if (((Button)sender).CommandArgument == "I")
                    _novo = freController.ObterTipoAmostra(txtDescricao.Text.Trim());
                else
                    _novo = freController.ObterTipoAmostra(int.Parse(hidCodigo.Value));

                if (_novo == null)
                {
                    _novo = new FRETE_TIPO_AMOSTRA();

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
                Button b = (Button)sender;

                int CODIGO = Convert.ToInt32(b.CommandArgument.Trim());

                FRETE_TIPO_AMOSTRA _tipoAmostra = freController.ObterTipoAmostra(CODIGO);

                ddlStatus.SelectedValue = _tipoAmostra.STATUS.ToString();
                txtDescricao.Text = _tipoAmostra.DESCRICAO;

                btCancelar.Visible = true;
                btIncluir.CommandArgument = "A";
                hidCodigo.Value = CODIGO.ToString();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
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
                    Excluir(CODIGO);
                    RecarregarTela();
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
            Utils.WebControls.GetBoundFieldIndexByName(gvCadTipoAmostra, " - >>");
            Utils.WebControls.GetBoundFieldIndexByName(gvCadTipoAmostra, " - <<");

            gvCadTipoAmostra.DataSource = freController.ObterTipoAmostra();
            gvCadTipoAmostra.DataBind();

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
            var gridView = gvCadTipoAmostra;
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