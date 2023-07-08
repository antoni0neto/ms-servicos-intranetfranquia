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
    public partial class fre_cad_valor_peso : System.Web.UI.Page
    {
        FreteController freController = new FreteController();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregaTipoEnvio();
                RecarregarTela();

            }
            btIncluir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btIncluir, null) + ";");
            btCancelar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btCancelar, null) + ";");

            CriaCabecalhoRodapeAtualizaScript();
        }

        private void CarregaTipoEnvio()
        {
            ddlTipoEnvio.DataSource = freController.ObterTipoEnvioAtivo();
            ddlTipoEnvio.DataBind();
            ddlTipoEnvio.Items.Insert(0, new ListItem("Selecione", "0"));
        }

        #region "CRUD"
        private void Incluir(FRETE_VALOR_PESO _param)
        {
            freController.InserirValorPeso(_param);
        }

        private void Atualizar(FRETE_VALOR_PESO _param)
        {
            freController.AtualizarValorPeso(_param);
        }
        private void Excluir(int CODIGO)
        {
            freController.ExcluirValorPeso(CODIGO);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            labErro.Text = "";            
            btCancelar.Visible = (ddlTipoEnvio.SelectedValue != "0");

            try
            {
                List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
                PreparaControlesTela(ref list);
                decimal peso = 0;

                if (!Utils.WebControls.ValidarCampos(ref list))
                {
                    labErro.Text = "Preencha corretamente os campos em vermelho.";

                    CriaCabecalhoRodapeAtualizaScript();
                    return;
                }
                              
                FRETE_VALOR_PESO _novo;
                decimal descontoPercentual = 0;
                Decimal.TryParse(txtDescontoPorcentagem.Text, out descontoPercentual);

                if (descontoPercentual > 100)
                {
                    labErro.Text = "O percentual não pode ultrapassar 100%.";
                    CriaCabecalhoRodapeAtualizaScript();
                    return;
                }

                decimal.TryParse(txtPeso.Text, out peso);

                if (((Button)sender).CommandArgument == "I")
                    _novo = freController.ObterValorPeso(int.Parse(ddlTipoEnvio.SelectedValue), peso);
                else
                    _novo = freController.ObterValorPeso(int.Parse(hidCodigo.Value));

                if (_novo == null)
                {
                    _novo = new FRETE_VALOR_PESO();

                    _novo.FRETE_TIPO_ENVIO = int.Parse(ddlTipoEnvio.SelectedValue);
                    _novo.DESCONTO_PORC = descontoPercentual;
                    _novo.PESO = Decimal.Parse(txtPeso.Text);
                    _novo.VALOR = Decimal.Parse(txtValor.Text);
                    
                    if (((Button)sender).CommandArgument == "I")
                    {
                        Incluir(_novo);
                        RecarregarTela();
                        labErro.Text = "Registro inserido com sucesso.";
                    }
                }
                else if (((Button)sender).CommandArgument == "A")
                {
                    _novo.FRETE_TIPO_ENVIO = int.Parse(ddlTipoEnvio.SelectedValue);
                    _novo.DESCONTO_PORC =descontoPercentual;
                    _novo.PESO = Decimal.Parse(txtPeso.Text);
                    _novo.VALOR = Decimal.Parse(txtValor.Text);

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

                FRETE_VALOR_PESO _valorPeso = freController.ObterValorPeso(CODIGO);

                ddlTipoEnvio.SelectedValue = _valorPeso.FRETE_TIPO_ENVIO.ToString();
                txtValor.Text = _valorPeso.VALOR.ToString();
                txtPeso.Text = _valorPeso.PESO.ToString();
                txtDescontoPorcentagem.Text = _valorPeso.DESCONTO_PORC.ToString();

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
            Utils.WebControls.GetBoundFieldIndexByName(gvCadValorPeso, " - >>");
            Utils.WebControls.GetBoundFieldIndexByName(gvCadValorPeso, " - <<");

            gvCadValorPeso.DataSource = freController.ObterValorPeso();
            gvCadValorPeso.DataBind();

            CriaCabecalhoRodapeAtualizaScript();
        }

        private void PreparaControlesTela(ref List<Utils.WebControls.ControlesValidacao> list)
        {
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labTipoEnvio, Controle = ddlTipoEnvio, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labPeso, Controle = txtPeso, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labValor, Controle = txtValor, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labDescontoPorcentagem, Controle = txtDescontoPorcentagem, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErro, CorPadrao = Color.Gray, ValorPadrao = "" });

        }

        private void CriaCabecalhoRodapeAtualizaScript()
        {
            var gridView = gvCadValorPeso;
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

        public FRETE_TIPO_ENVIO ObterTipoEnvio(int CODIGO)
        {
            var result = freController.ObterTipoEnvio(CODIGO);
            if (result == null)
                return new FRETE_TIPO_ENVIO();
            else
                return result;
        }
    }
}