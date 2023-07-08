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
    public partial class fre_cad_taxa_combustivel : System.Web.UI.Page
    {
        FreteController freController = new FreteController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregaAno();
                RecarregarTela();

            }
            btIncluir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btIncluir, null) + ";");
            btCancelar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btCancelar, null) + ";");

            CriaCabecalhoRodapeAtualizaScript();
        }

        private void CarregaAno()
        {
            // CARREGA TODOS OS ANOS CADASTRADOS PARA CARREGAMENTO DINÂMICO DOS ITEMS
            List<int> listAnosCadastrados = freController.ObterAnosCadastradosTaxaCombustivel();

            if (listAnosCadastrados == null)
            {
                listAnosCadastrados = new List<int>();
            }

            List<int> lisNewYear = new List<int>();

            lisNewYear.Add(DateTime.Now.Year);
            lisNewYear.Add(DateTime.Now.AddYears(1).Year);

            foreach (var ano in lisNewYear)
            {
                if (!listAnosCadastrados.Contains(ano))
                {
                    listAnosCadastrados.Add(ano);
                }
            }

            foreach (var ano in listAnosCadastrados)
            {
                ddlAno.Items.Add(new ListItem(ano.ToString(), ano.ToString()));
            }

            ddlAno.Items.Insert(0, new ListItem("Selecione", "0"));

        }

        #region "CRUD"
        private void Incluir(FRETE_TAXA_COMBUSTIVEL _param)
        {
            freController.InserirTaxaCombustivel(_param);
        }

        private void Atualizar(FRETE_TAXA_COMBUSTIVEL _param)
        {
            freController.AtualizarTaxaCombustivel(_param);
        }
        private void Excluir(int CODIGO)
        {
            freController.ExcluirTaxaCombustivel(CODIGO);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            btCancelar.Visible = (txtValorTaxa.Text != "0");

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

                decimal descontoPercentual = 0;

                Decimal.TryParse(txtValorTaxa.Text, out descontoPercentual);

                if (descontoPercentual > 100)
                {
                    labErro.Text = "O valor da taxa não pode ultrapassar 100%.";
                    CriaCabecalhoRodapeAtualizaScript();
                    return;
                }

                FRETE_TAXA_COMBUSTIVEL _novo;

                if (((Button)sender).CommandArgument == "I")
                    _novo = freController.ObterTaxaCombustivel(int.Parse(ddlMes.SelectedItem.Value), int.Parse(ddlAno.SelectedItem.Value));
                else
                    _novo = freController.ObterTaxaCombustivel(int.Parse(hidCodigo.Value));

                if (_novo == null)
                {
                    _novo = new FRETE_TAXA_COMBUSTIVEL();

                    _novo.VALOR_TAXA = 
                    _novo.MES = int.Parse(ddlMes.SelectedItem.Value);
                    _novo.ANO = int.Parse(ddlAno.SelectedItem.Value);

                    if (((Button)sender).CommandArgument == "I")
                    {
                        Incluir(_novo);
                        RecarregarTela();
                        labErro.Text = "Registro inserido com sucesso.";
                    }
                }
                else if (((Button)sender).CommandArgument == "A")
                {
                    _novo.VALOR_TAXA = Decimal.Parse(txtValorTaxa.Text.Trim());
                    _novo.MES = int.Parse(ddlMes.SelectedItem.Value);
                    _novo.ANO = int.Parse(ddlAno.SelectedItem.Value);

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

                FRETE_TAXA_COMBUSTIVEL _taxaCombustivel = freController.ObterTaxaCombustivel(CODIGO);

                txtValorTaxa.Text = _taxaCombustivel.VALOR_TAXA.ToString();
                ddlMes.SelectedValue = _taxaCombustivel.MES.ToString();
                ddlAno.SelectedValue = _taxaCombustivel.ANO.ToString();

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
            Utils.WebControls.GetBoundFieldIndexByName(gvCadTaxaCombustivel, " - >>");
            Utils.WebControls.GetBoundFieldIndexByName(gvCadTaxaCombustivel, " - <<");

            gvCadTaxaCombustivel.DataSource = freController.ObterTaxaCombustivel();
            gvCadTaxaCombustivel.DataBind();

            CriaCabecalhoRodapeAtualizaScript();
        }

        private void PreparaControlesTela(ref List<Utils.WebControls.ControlesValidacao> list)
        {
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labValorTaxa, Controle = txtValorTaxa, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labMes, Controle = ddlMes, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labAno, Controle = ddlAno, CorPadrao = Color.Gray, CorAviso = Color.Red, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Controle = labErro, CorPadrao = Color.Gray, ValorPadrao = "" });

        }

        private void CriaCabecalhoRodapeAtualizaScript()
        {
            var gridView = gvCadTaxaCombustivel;
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

        public string ObterNomeMes(int mes)
        {
            List<string> listMeses = new List<string>();

            listMeses.Add("");            
            listMeses.Add("Janeiro");
            listMeses.Add("Fevereiro");
            listMeses.Add("Março");
            listMeses.Add("Abril");
            listMeses.Add("Maio");
            listMeses.Add("Junho");
            listMeses.Add("Julho");
            listMeses.Add("Agosto");
            listMeses.Add("Setembro");
            listMeses.Add("Outubro");
            listMeses.Add("Novembro");
            listMeses.Add("Dezembro");

            return listMeses[mes];
        }
    }
}