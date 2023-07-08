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
    public partial class fre_cad_grupo_precomin : System.Web.UI.Page
    {
        FreteController freController = new FreteController();
        ProducaoController proController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregaGrupoProdutos();                
                RecarregarTela();

            }
            btIncluir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btIncluir, null) + ";");
            btCancelar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btCancelar, null) + ";");

            CriaCabecalhoRodapeAtualizaScript();
        }

        private void CarregaGrupoProdutos()
        {
            ddlGrupoProduto.DataSource = (from g in proController.ObterGrupoProduto("01") select new { GRUPO_PRODUTO = g.GRUPO_PRODUTO.Trim() }).ToList();
            ddlGrupoProduto.DataBind();
            ddlGrupoProduto.Items.Insert(0, new ListItem("Selecione", "0"));
        }

        #region "CRUD"
        private void Incluir(FRETE_GRUPO_PRECOMIN _param)
        {
            freController.InserirGrupoPrecoMin(_param);
        }

        private void Atualizar(FRETE_GRUPO_PRECOMIN _param)
        {
            freController.AtualizarGrupoPrecoMin(_param);
        }
        private void Excluir(int CODIGO)
        {
            freController.ExcluirGrupoPrecoMin(CODIGO);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            btCancelar.Visible = (ddlGrupoProduto.SelectedValue != "0");

            try
            {
                List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
                list.Add(new Utils.WebControls.ControlesValidacao() { Label = labGrupoProduto, Controle = ddlGrupoProduto, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
                list.Add(new Utils.WebControls.ControlesValidacao() { Label = labValorUnitario, Controle = txtValorUnitario, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
                list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErro, CorPadrao = Color.Gray, ValorPadrao = "" });

                if (!Utils.WebControls.ValidarCampos(ref list))
                {
                    labErro.Text = "Preencha corretamente os campos em vermelho.";

                    CriaCabecalhoRodapeAtualizaScript();
                    return;
                }

                FRETE_GRUPO_PRECOMIN _novo;

                if (((Button)sender).CommandArgument == "I")
                    _novo = freController.ObterGrupoPrecoMin(ddlGrupoProduto.SelectedValue);
                else
                    _novo = freController.ObterGrupoPrecoMin(int.Parse(hidCodigo.Value));

                if (_novo == null)
                {
                    _novo = new FRETE_GRUPO_PRECOMIN();

                    _novo.GRUPO_PRODUTO = ddlGrupoProduto.SelectedValue;
                    _novo.VALOR_UNITARIO = Decimal.Parse(txtValorUnitario.Text.Trim());                    

                    if (((Button)sender).CommandArgument == "I")
                    {
                        Incluir(_novo);
                        RecarregarTela();
                        labErro.Text = "Registro inserido com sucesso.";
                    }                    
                }
                else if (((Button)sender).CommandArgument == "A")
                {
                    _novo.GRUPO_PRODUTO = ddlGrupoProduto.SelectedValue;
                    _novo.VALOR_UNITARIO = Decimal.Parse(txtValorUnitario.Text.Trim()); 

                    Atualizar(_novo);
                    RecarregarTela();
                    labErro.Text = "Registro atualizado com sucesso.";
                }
                else
                {
                    labErro.Text = "Grupo já cadastrado na base de dados.";
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

                string GRUPO_PRODUTO = b.CommandArgument.Trim();

                FRETE_GRUPO_PRECOMIN _grupoPrecoMin = freController.ObterGrupoPrecoMin(GRUPO_PRODUTO);

                ddlGrupoProduto.SelectedValue = _grupoPrecoMin.GRUPO_PRODUTO;
                txtValorUnitario.Text = _grupoPrecoMin.VALOR_UNITARIO.ToString();

                btCancelar.Visible = true;
                btIncluir.CommandArgument = "A";
                hidCodigo.Value = _grupoPrecoMin.CODIGO.ToString();
                
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
                    labErro.Text = "O Grupo não pode ser excluído. Este Grupo foi utilizado em um Processo.";
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
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labGrupoProduto, Controle = ddlGrupoProduto, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labValorUnitario, Controle = txtValorUnitario, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
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
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labGrupoProduto, Controle = ddlGrupoProduto, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labValorUnitario, Controle = txtValorUnitario, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErro, CorPadrao = Color.Gray, ValorPadrao = "" });

            Utils.WebControls.LimparControles(ref list);
            AtualizaGrid();
        }

        private void AtualizaGrid()
        {
            Utils.WebControls.GetBoundFieldIndexByName(gvCadGrupoPrecoMin, " - >>");
            Utils.WebControls.GetBoundFieldIndexByName(gvCadGrupoPrecoMin, " - <<");

            gvCadGrupoPrecoMin.DataSource = freController.ObterGrupoPrecoMin();
            gvCadGrupoPrecoMin.DataBind();

            CriaCabecalhoRodapeAtualizaScript();
        }      

        private void CriaCabecalhoRodapeAtualizaScript()
        {
            var gridView = gvCadGrupoPrecoMin;
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