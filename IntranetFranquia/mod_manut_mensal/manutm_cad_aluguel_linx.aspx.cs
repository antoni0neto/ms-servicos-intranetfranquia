using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;

namespace Relatorios
{
    public partial class manutm_cad_aluguel_linx : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        decimal totalBoleto = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarFilial();
                CarregarMesVencimento();

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btLimpar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btLimpar, null) + ";");
            btEnviar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviar, null) + ";");
            btExcluir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btExcluir, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            var filial = baseController.BuscaFiliaisAtivaInativa();
            var filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();

            filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
            ddlFilial.DataSource = filial;
            ddlFilial.DataBind();

        }
        private void CarregarMesVencimento()
        {
            var v = baseController.BuscaReferencias().OrderByDescending(p => p.ANO).ThenByDescending(i => i.MES).ToList();

            v.Insert(0, new ACOMPANHAMENTO_ALUGUEL_MESANO { CODIGO_ACOMPANHAMENTO_MESANO = 0, DESCRICAO = "Selecione" });
            ddlVencimento.DataSource = v;
            ddlVencimento.DataBind();
        }

        private bool ValidarCampos(bool inclusao)
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue.Trim() == "")
            {
                labFilial.ForeColor = _notOK;
                retorno = false;
            }

            labVencimento.ForeColor = _OK;
            if (ddlVencimento.SelectedValue.Trim() == "0")
            {
                labVencimento.ForeColor = _notOK;
                retorno = false;
            }

            labDia.ForeColor = _OK;
            if (txtDia.Text.Trim() == "")
            {
                labDia.ForeColor = _notOK;
                retorno = false;
            }

            labLancamento.ForeColor = _OK;
            if (txtLancamento.Text.Trim() == "")
            {
                labLancamento.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarContasIntranet()
        {
            bool valido = true;

            foreach (GridViewRow r in gvBoletoDespesa.Rows)
            {
                var ddlContaIntranet = ((DropDownList)r.FindControl("ddlContaIntranet"));
                if (ddlContaIntranet.SelectedValue == "0")
                    return false;
            }

            return valido;
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErroBusca.Text = "";
                labErro.Text = "";

                if (!ValidarCampos(true))
                {
                    labErroBusca.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                //var lancamentoValido = lojaController.ObterBoletoLancamento2(Convert.ToInt32(txtLancamento.Text.Trim()));
                //if (lancamentoValido == null)
                //{
                //    labErroBusca.Text = "Lançamento NÃO encontrado.";
                //    return;
                //}

                //if (lancamentoValido.COD_FILIAL.Trim() != ddlFilial.SelectedValue.Trim())
                //{
                //    labErroBusca.Text = "Filial do lançamento NÃO é a mesma da Filial selecionada.";
                //    return;
                //}

                //DateTime vencimentoInf = Convert.ToDateTime(txtDia.Text.Trim() + "/" + ddlVencimento.SelectedItem.Text.Trim());
                //if (lancamentoValido.VENCIMENTO != vencimentoInf)
                //{
                //    labErroBusca.Text = "Vencimento do lançamento NÃO é a mesmo do Vencimento informado.";
                //    return;
                //}

                hidTotalBoleto.Value = "0";
                CarregarBoleto();
                CarregarDespesas();

                ddlFilial.Enabled = false;
                ddlVencimento.Enabled = false;
                txtDia.Enabled = false;
                txtLancamento.Enabled = false;

            }
            catch (Exception ex)
            {
                labErroBusca.Text = ex.Message;
            }
        }
        private void CarregarBoleto()
        {
            //verifica se ja existe
            var boleto = lojaController.ObterBoletoAluguel(ddlFilial.SelectedValue, Convert.ToInt32(txtLancamento.Text.Trim()));
            if (boleto != null)
            {
                hidCodigoBoleto.Value = boleto.CODIGO.ToString();
                txtObs.Text = boleto.OBSERVACAO;
                btExcluir.Visible = true;

                if (boleto.DIRETORIO_BOLETO != null && boleto.DIRETORIO_BOLETO != "")
                {
                    imgBoleto.ImageUrl = "~/Image/pdf-download.jpg";
                    hidArquivoPDF.Value = boleto.DIRETORIO_BOLETO;

                    linkNomePDF.NavigateUrl = hidArquivoPDF.Value;
                    linkNomePDF.Text = "<font size='2' face='Calibri'>" + hidArquivoPDF.Value.Replace("~/PDF_BOLETO/", "") + "</font>";
                }

            }
        }
        private void CarregarDespesas()
        {
            var boletoDespesas = lojaController.ObterBoletoLancamentoDespesas(Convert.ToInt32(txtLancamento.Text.Trim()));

            gvBoletoDespesa.DataSource = boletoDespesas;
            gvBoletoDespesa.DataBind();
        }

        #region "INCLUSAO"
        protected void btEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroBusca.Text = "";
                if (!ValidarCampos(true))
                {
                    labErroBusca.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                //var lancamentoValido = lojaController.ObterBoletoLancamento2(Convert.ToInt32(txtLancamento.Text.Trim()));
                //if (lancamentoValido == null)
                //{
                //    labErroBusca.Text = "Lançamento NÃO encontrado.";
                //    return;
                //}

                //if (lancamentoValido.COD_FILIAL.Trim() != ddlFilial.SelectedValue.Trim())
                //{
                //    labErroBusca.Text = "Filial do lançamento NÃO é a mesma da Filial selecionada.";
                //    return;
                //}

                //DateTime vencimentoInf = Convert.ToDateTime(txtDia.Text.Trim() + "/" + ddlVencimento.SelectedItem.Text.Trim());
                //if (lancamentoValido.VENCIMENTO != vencimentoInf)
                //{
                //    labErroBusca.Text = "Vencimento do lançamento NÃO é a mesmo do Vencimento informado.";
                //    return;
                //}

                if (!ValidarContasIntranet())
                {
                    labErro.Text = "Preencha corretamente as <strong>Contas da Intranet</strong>.";
                    return;
                }

                var inc = true;
                var boleto = new ACOMPANHAMENTO_ALUGUEL_BOLETO();
                if (hidCodigoBoleto.Value != "")
                {
                    boleto = lojaController.ObterBoletoAluguel(Convert.ToInt32(hidCodigoBoleto.Value));
                    inc = false;
                }

                boleto.CODIGO_FILIAL = ddlFilial.SelectedValue;
                boleto.DATA_VENCIMENTO = Convert.ToDateTime(txtDia.Text + "/" + ddlVencimento.SelectedItem.Text.Trim());
                boleto.VALOR = Convert.ToDecimal(hidTotalBoleto.Value);
                boleto.DIRETORIO_BOLETO = hidArquivoPDF.Value;
                boleto.DATA_INCLUSAO = DateTime.Now;
                boleto.OBSERVACAO = txtObs.Text;
                boleto.LANCAMENTO = Convert.ToInt32(txtLancamento.Text);

                int codigoBoleto = 0;
                if (inc)
                    codigoBoleto = lojaController.InserirBoletoAluguel(boleto);
                else
                    codigoBoleto = lojaController.AtualizarBoletoAluguel(boleto);

                //Inserir despesas
                if (codigoBoleto > 0)
                {
                    foreach (GridViewRow r in gvBoletoDespesa.Rows)
                    {
                        int codigoDespesa = Convert.ToInt32(((DropDownList)r.FindControl("ddlContaIntranet")).SelectedValue);
                        decimal valor = Convert.ToDecimal(((Literal)r.FindControl("litValor")).Text.Replace("R$", "").Trim());
                        int item = Convert.ToInt32(((Literal)r.FindControl("litItem")).Text.Replace("R$", "").Trim());

                        int codigoBoletoDespesa = 0;
                        codigoBoletoDespesa = Convert.ToInt32(gvBoletoDespesa.DataKeys[r.RowIndex].Value);

                        if (codigoBoletoDespesa == 0)
                        {
                            var acompAlu = new ACOMPANHAMENTO_ALUGUEL();
                            acompAlu.CODIGO_ALUGUEL_DESPESA = codigoDespesa;
                            acompAlu.VALOR = valor;
                            acompAlu.CODIGO_FILIAL = Convert.ToInt32(ddlFilial.SelectedValue);
                            acompAlu.CODIGO_MESANO = Convert.ToInt32(ddlVencimento.SelectedValue);
                            acompAlu.DATA_VENCIMENTO = Convert.ToDateTime(boleto.DATA_VENCIMENTO);
                            acompAlu.STATUS = 0;
                            acompAlu.VALOR_BOLETO = 'S';
                            acompAlu.ACOMPANHAMENTO_ALUGUEL_BOLETO = codigoBoleto;
                            acompAlu.LANCAMENTO = Convert.ToInt32(txtLancamento.Text);
                            acompAlu.ITEM = item;

                            lojaController.InserirBoletoAluguelDespesa(acompAlu);
                        }
                        else
                        {
                            var acompAlu = lojaController.ObterBoletoAluguelDespesa(codigoBoletoDespesa);
                            if (acompAlu != null)
                            {
                                acompAlu.CODIGO_ALUGUEL_DESPESA = codigoDespesa;
                                acompAlu.VALOR = valor;
                                acompAlu.DATA_VENCIMENTO = Convert.ToDateTime(boleto.DATA_VENCIMENTO);

                                lojaController.AtualizarBoletoAluguelDespesa(acompAlu);
                            }
                        }

                    }
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('manutm_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('manutm_menu.aspx', '_self'); }, Cadastro: function () { window.open('manutm_cad_aluguel_linx.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void btPDFBoleto_Click(object sender, EventArgs e)
        {
            try
            {
                var retorno = SalvarPDF();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: (btPDFBoleto_Click) \n\n" + ex.Message;
            }

        }
        private string SalvarPDF()
        {
            string path = string.Empty;
            string fileName = string.Empty;
            string ext = string.Empty;

            try
            {

                if (!uploadBoleto.HasFile)
                    return "Selecione um arquivo para upload...";

                if (uploadBoleto.FileContent.Length == 0)
                    return "Selecione um arquivo com conteúdo...";

                //As the input is external, always do case-insensitive comparison unless you actually care about the case.
                if (!uploadBoleto.PostedFile.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                    return "Selecione apenas arquivos PDF...";

                //Obter variaveis do arquivo
                ext = System.IO.Path.GetExtension(uploadBoleto.PostedFile.FileName);
                fileName = ddlFilial.SelectedValue.Trim() + "_" + ddlVencimento.SelectedItem.Text.Trim().Replace("/", "-") + "_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm") + ext;
                path = Server.MapPath("~/PDF_BOLETO/") + fileName;

                uploadBoleto.PostedFile.SaveAs(path);

                imgBoleto.ImageUrl = "~/Image/pdf-download.jpg";
                hidArquivoPDF.Value = "~/PDF_BOLETO/" + fileName;
                linkNomePDF.NavigateUrl = hidArquivoPDF.Value;
                linkNomePDF.Text = "<font size='2' face='Calibri'>" + fileName + "</font>";


                return fileName;
            }
            catch (Exception)
            {
            }

            return "Nenhuma ação realizada...";
        }
        #endregion

        #region "GRID DESPESAS"
        protected void gvBoletoDespesa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_BOLETO_LANCAMENTO_DESPESASResult bol = e.Row.DataItem as SP_OBTER_BOLETO_LANCAMENTO_DESPESASResult;

                    if (bol != null)
                    {
                        Literal litValor = e.Row.FindControl("litValor") as Literal;
                        if (litValor != null)
                            litValor.Text = "R$ " + Convert.ToDecimal(bol.DEBITO).ToString("###,###,###,##0.00");

                        DropDownList ddlContaIntranet = e.Row.FindControl("ddlContaIntranet") as DropDownList;
                        ddlContaIntranet.DataSource = ObterContasIntranet();
                        ddlContaIntranet.DataBind();

                        if (bol.CODIGO_ALUGUEL_DESPESA <= 0)
                        {
                            var dePara = baseController.ObterAluguelDespesaPorConta(bol.CONTA_CONTABIL);
                            if (dePara != null)
                                ddlContaIntranet.SelectedValue = dePara.CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA.ToString();
                        }
                        else
                        {
                            ddlContaIntranet.SelectedValue = bol.CODIGO_ALUGUEL_DESPESA.ToString();
                        }

                        totalBoleto += Convert.ToDecimal(bol.DEBITO);

                    }
                }
            }

        }
        protected void gvBoletoDespesa_DataBound(object sender, EventArgs e)
        {

            GridViewRow footer = gvBoletoDespesa.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[4].Text = "R$ " + totalBoleto.ToString("###,###,###,##0.00");

                hidTotalBoleto.Value = totalBoleto.ToString();
            }
        }

        private List<ACOMPANHAMENTO_ALUGUEL_DESPESA> ObterContasIntranet()
        {
            var c = baseController.BuscaAluguelDespesas().OrderBy(p => p.DESCRICAO).ToList();
            c.Insert(0, new ACOMPANHAMENTO_ALUGUEL_DESPESA { CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA = 0, DESCRICAO = "Selecione" });
            return c;
        }
        #endregion

        protected void btLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void LimparCampos()
        {
            ddlFilial.SelectedValue = "";
            ddlVencimento.SelectedValue = "0";
            txtDia.Text = "";
            txtLancamento.Text = "";

            txtObs.Text = "";

            imgBoleto.ImageUrl = "";
            linkNomePDF.Text = "";
            hidArquivoPDF.Value = "";
            hidCodigoBoleto.Value = "";
            hidTotalBoleto.Value = "0";

            gvBoletoDespesa.DataSource = new List<SP_OBTER_BOLETO_LANCAMENTO_DESPESASResult>();
            gvBoletoDespesa.DataBind();

            ddlFilial.Enabled = true;
            ddlVencimento.Enabled = true;
            txtDia.Enabled = true;
            txtLancamento.Enabled = true;

            btExcluir.Visible = false;

            labErro.Text = "";
            labErroBusca.Text = "";
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            if (hidCodigoBoleto.Value != "")
            {
                //excluir despesas
                foreach (GridViewRow r in gvBoletoDespesa.Rows)
                {
                    int codigoBoletoDespesa = 0;
                    codigoBoletoDespesa = Convert.ToInt32(gvBoletoDespesa.DataKeys[r.RowIndex].Value);

                    if (codigoBoletoDespesa > 0)
                        lojaController.ExcluirBoletoAluguelDespesa(codigoBoletoDespesa);
                }

                //excluir boleto
                int codigoBoleto = Convert.ToInt32(hidCodigoBoleto.Value);
                lojaController.ExcluirBoletoAluguel(codigoBoleto);

                LimparCampos();
            }
        }

    }
}
