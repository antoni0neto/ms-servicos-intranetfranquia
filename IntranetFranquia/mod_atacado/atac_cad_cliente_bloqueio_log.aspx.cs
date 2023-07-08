using DAL;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class atac_cad_cliente_bloqueio_log : System.Web.UI.Page
    {
        AtacadoController atacController = new AtacadoController();
        BaseController baseController = new BaseController();

        decimal totFatAtraso = 0;

        decimal totFatValor = 0;
        decimal totOrigValor = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                string colecao = "";
                string clifor = "";
                if (Request.QueryString["col"] == null || Request.QueryString["col"] == "" ||
                    Request.QueryString["clifor"] == null || Request.QueryString["clifor"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("atac_menu.aspx");

                USUARIO usuario = ((USUARIO)Session["USUARIO"]);

                colecao = Request.QueryString["col"].ToString();
                clifor = Request.QueryString["clifor"].ToString();

                hidClifor.Value = clifor;
                hidColecao.Value = colecao;

                CarregarCliente(colecao, clifor);
                CarregarCondPgtos();

                if (usuario.CODIGO_PERFIL == 4)
                {
                    btBloquearFaturamento.Enabled = false;
                    btSemCredito.Enabled = false;
                    btSalvar.Enabled = false;
                }

            }

            //Evitar duplo clique no botão
            btSemCredito.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSemCredito, null) + ";");
            btBloquearFaturamento.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBloquearFaturamento, null) + ";");
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            //labFornecedor.ForeColor = _OK;
            //if (ddlFornecedor.SelectedValue.Trim() == "Selecione")
            //{
            //    labFornecedor.ForeColor = _notOK;
            //    retorno = false;
            //}

            return retorno;
        }

        private void CarregarCondPgtos()
        {
            var c = atacController.ObterAtacadoCondPgto();

            ddlCondPgto.DataSource = c;
            ddlCondPgto.DataBind();
        }
        #endregion

        private void CarregarCliente(string colecao, string clifor)
        {
            var cli = atacController.ObterClienteAtacadoBloq(colecao, clifor, "", "", "", null, null).FirstOrDefault();
            if (cli != null)
            {
                txtCliente.Text = cli.CLIENTE_ATACADO;
                txtRazaoSocial.Text = cli.RAZAO_SOCIAL;
                txtClifor.Text = cli.CLIFOR;
                txtDataFundacao.Text = cli.DATA_FUNDACAO;
                txtCNPJ.Text = cli.CGC_CPF;
                txtEmail.Text = cli.EMAIL;
                txtDDD.Text = cli.DDD1;
                txtTelefone.Text = cli.TELEFONE1;
                txtQtdeProtesto.Text = cli.QTDE_PROTESTO;
                txtRepresentante.Text = cli.REPRESENTANTE;
                txtDataSerasa.Text = cli.DATA_SERASA;
                txtLimiteCredito.Text = cli.LIMITE_CREDITO.ToString();
                ddlTipoBloqueio.SelectedValue = cli.TIPO_BLOQUEIO;
                txtInsEstadual.Text = cli.RG_IE;

                if (cli.SEM_CREDITO)
                    ddlSemCredito.SelectedValue = "S";
                if (cli.BLOQUEIO_FATURAMENTO != null)
                    ddlBloquearFaturamento.SelectedValue = "S";

                txtObsGeral.Text = cli.OBS;
                txtObsFaturamento.Text = cli.OBS_DE_FATURAMENTO;

                if (cli.ARQUIVO_PDF != null && cli.ARQUIVO_PDF != "")
                {
                    imgArquivoSerasa.ImageUrl = "~/Image/pdf-download.jpg";
                    hidArquivoPDF.Value = cli.ARQUIVO_PDF;
                    btExcluirArquivo.Visible = true;

                    linkNomePDF.NavigateUrl = hidArquivoPDF.Value;
                    linkNomePDF.Text = "<font size='2' face='Calibri'>" + hidArquivoPDF.Value.Replace("~/PDF_SERASA/", "") + "</font>";
                }

                totOrigValor = 0;
                var cliPedido = atacController.ObterPedidoClienteAtacado(cli.COLECAO_PESQUISA, cli.CLIENTE_ATACADO);
                gvColecaoPedido.DataSource = cliPedido;
                gvColecaoPedido.DataBind();

                totOrigValor = 0;
                var cliHistVenda = atacController.ObterHistVendaClienteAtacado(cli.CLIENTE_ATACADO);
                gvHistVenda.DataSource = cliHistVenda;
                gvHistVenda.DataBind();

                totOrigValor = 0;
                var cliFaturaAtraso = atacController.ObterClientesFaturaAtraso("", cli.CLIFOR);
                gvFaturaAtraso.DataSource = cliFaturaAtraso;
                gvFaturaAtraso.DataBind();
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlTipoBloqueio.SelectedValue == "")
                {
                    labErro.Text = "Selecione o Tipo Bloqueio.";
                    return;
                }

                SalvarDados(0);

                btSalvar.Enabled = false;

                labErro.Text = "Cliente Atacado alterado com sucesso.";

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btBloquearFaturamento_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlTipoBloqueio.SelectedValue == "")
                {
                    labErro.Text = "Selecione o Tipo Bloqueio.";
                    return;
                }

                SalvarDados(1);

                btBloquearFaturamento.Enabled = false;

                labErro.Text = "Cliente Atacado BLOQUEADO com sucesso.";

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btSemCredito_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlTipoBloqueio.SelectedValue == "")
                {
                    labErro.Text = "Selecione o Tipo Bloqueio.";
                    return;
                }

                SalvarDados(2);

                btSemCredito.Enabled = false;

                labErro.Text = "Cliente Atacado alterado para SEM CRÉDITO com sucesso.";

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void SalvarDados(int origem)
        {

            ATACADO_BLOQ_CLIENTE cli = new ATACADO_BLOQ_CLIENTE();
            cli.COLECAO = hidColecao.Value;
            cli.CLIFOR = hidClifor.Value;
            cli.CLIENTE_ATACADO = txtCliente.Text;
            cli.CGC_CPF = txtCNPJ.Text;
            cli.TIPO_BLOQUEIO = ddlTipoBloqueio.SelectedValue;

            // BOTAO BLOQUEAR FATURAMENTO
            if (origem == 1)
            {
                cli.BLOQUEIO_FATURAMENTO = DateTime.Now;
            }
            else
            {
                if (ddlBloquearFaturamento.SelectedValue == "S")
                    cli.BLOQUEIO_FATURAMENTO = DateTime.Now;
            }

            // BOTAO SEM CRÉDITO
            if (origem == 2)
                cli.SEM_CREDITO = true;
            else
                cli.SEM_CREDITO = (ddlSemCredito.SelectedValue == "S") ? true : false;


            cli.LIMITE_CREDITO = Convert.ToDecimal(txtLimiteCredito.Text);
            cli.DATA_FUNDACAO = txtDataFundacao.Text;
            cli.DATA_SERASA = txtDataSerasa.Text;
            cli.QTDE_PROTESTO = txtQtdeProtesto.Text;
            cli.OBS = txtObsGeral.Text;
            cli.OBS_DE_FATURAMENTO = txtObsFaturamento.Text;
            cli.ARQUIVO_PDF = hidArquivoPDF.Value;
            cli.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            cli.DATA_INCLUSAO = DateTime.Now;

            atacController.InserirHistClienteBloqueado(cli);

            var ret = atacController.AtualizarClienteAtacado(cli.CLIENTE_ATACADO, txtEmail.Text.Trim().ToUpper(), txtDDD.Text.Trim(), txtTelefone.Text.Trim(), cli.LIMITE_CREDITO, cli.BLOQUEIO_FATURAMENTO, cli.SEM_CREDITO, cli.OBS, cli.OBS_DE_FATURAMENTO, cli.DATA_SERASA, cli.DATA_FUNDACAO, cli.QTDE_PROTESTO, cli.TIPO_BLOQUEIO, txtInsEstadual.Text.Trim());
        }

        protected void btArquivoSerasaIncluir_Click(object sender, EventArgs e)
        {
            var retorno = SalvarPDF();
        }
        private string SalvarPDF()
        {
            string path = string.Empty;
            string fileName = string.Empty;
            string ext = string.Empty;

            try
            {

                if (!uploadArquivoSerasa.HasFile)
                    return "Selecione um arquivo para upload...";

                if (uploadArquivoSerasa.FileContent.Length == 0)
                    return "Selecione um arquivo com conteúdo...";

                //As the input is external, always do case-insensitive comparison unless you actually care about the case.
                if (!uploadArquivoSerasa.PostedFile.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                    return "Selecione apenas arquivos PDF...";

                //Obter variaveis do arquivo
                ext = System.IO.Path.GetExtension(uploadArquivoSerasa.PostedFile.FileName);
                fileName = hidClifor.Value.Trim() + "_" + hidColecao.Value.Trim() + "_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm") + ext;
                path = Server.MapPath("~/PDF_SERASA/") + fileName;

                uploadArquivoSerasa.PostedFile.SaveAs(path);

                imgArquivoSerasa.ImageUrl = "~/Image/pdf-download.jpg";
                hidArquivoPDF.Value = "~/PDF_SERASA/" + fileName;
                linkNomePDF.NavigateUrl = hidArquivoPDF.Value;
                linkNomePDF.Text = "<font size='2' face='Calibri'>" + fileName + "</font>";


                return fileName;
            }
            catch (Exception)
            {
            }

            return "Nenhuma ação realizada...";
        }

        protected void gvColecaoPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PEDIDO_CLIENTEResult pedido = e.Row.DataItem as SP_OBTER_PEDIDO_CLIENTEResult;

                    if (pedido != null)
                    {
                        Literal _litValorOriginal = e.Row.FindControl("litValorOriginal") as Literal;
                        if (_litValorOriginal != null)
                            _litValorOriginal.Text = "R$ " + Convert.ToDecimal(pedido.TOT_VALOR_ORIGINAL).ToString("###,###,###,##0.00");

                        totOrigValor += Convert.ToDecimal(pedido.TOT_VALOR_ORIGINAL);
                    }
                }
            }
        }
        protected void gvColecaoPedido_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvColecaoPedido.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[3].Text = "R$ " + totOrigValor.ToString("###,###,###,##0.00");
            }
        }

        protected void gvHistVenda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HIST_VENDA_CLIENTE_ATACResult histVenda = e.Row.DataItem as SP_OBTER_HIST_VENDA_CLIENTE_ATACResult;

                    if (histVenda != null)
                    {
                        Literal _litValorFaturado = e.Row.FindControl("litValorFaturado") as Literal;
                        if (_litValorFaturado != null)
                            _litValorFaturado.Text = "R$ " + Convert.ToDecimal(histVenda.VALOR_FATURADO).ToString("###,###,###,##0.00");

                        Literal _litValorOriginal = e.Row.FindControl("litValorOriginal") as Literal;
                        if (_litValorOriginal != null)
                            _litValorOriginal.Text = "R$ " + Convert.ToDecimal(histVenda.VALOR_ORIGINAL).ToString("###,###,###,##0.00");


                        totFatValor += Convert.ToDecimal(histVenda.VALOR_FATURADO);
                        totOrigValor += Convert.ToDecimal(histVenda.VALOR_ORIGINAL);
                    }
                }
            }
        }
        protected void gvHistVenda_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvHistVenda.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";

                _footer.Cells[3].Text = "R$ " + totOrigValor.ToString("###,###,###,##0.00");
                _footer.Cells[4].Text = "R$ " + totFatValor.ToString("###,###,###,##0.00");

            }
        }

        protected void gvFaturaAtraso_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CLIENTES_FATURA_ATRASOResult cliFat = e.Row.DataItem as SP_OBTER_CLIENTES_FATURA_ATRASOResult;

                    if (cliFat != null)
                    {

                        Literal _litDataVencimento = e.Row.FindControl("litDataVencimento") as Literal;
                        if (_litDataVencimento != null)
                            _litDataVencimento.Text = Convert.ToDateTime(cliFat.VENCIMENTO_REAL).ToString("dd/MM/yyyy");

                        Literal _litValorAtraso = e.Row.FindControl("litValorAtraso") as Literal;
                        if (_litValorAtraso != null)
                            _litValorAtraso.Text = "R$ " + Convert.ToDecimal(cliFat.VALOR_EM_ATRASO).ToString("###,###,###,##0.00");


                        totFatAtraso += Convert.ToDecimal(cliFat.VALOR_EM_ATRASO);
                    }
                }
            }
        }
        protected void gvFaturaAtraso_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvFaturaAtraso.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";

                _footer.Cells[5].Text = "R$ " + totFatAtraso.ToString("###,###,###,##0.00");
            }
        }

        protected void ddlTipoBloqueio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoBloqueio.SelectedValue != "")
            {

                if (ddlTipoBloqueio.SelectedValue == "JURIDICO" ||
                    ddlTipoBloqueio.SelectedValue == "OBSERVACAO" ||
                    ddlTipoBloqueio.SelectedValue == "SEM CREDITO" ||
                    ddlTipoBloqueio.SelectedValue == "PAGAMENTO CARTAO")
                    ddlSemCredito.SelectedValue = "S";
                else
                    ddlSemCredito.SelectedValue = "N";

            }
        }

        protected void btExcluirArquivo_Click(object sender, EventArgs e)
        {
            if (hidArquivoPDF.Value != "")
            {
                atacController.ExcluirArquivoPDF(hidClifor.Value, hidColecao.Value, hidArquivoPDF.Value);
                linkNomePDF.Text = "";
                hidArquivoPDF.Value = "";
            }
        }

        protected void btAlterarCondPgto_Click(object sender, EventArgs e)
        {
            // enviar NOMECLIFOR e nao CODIGO
            var nomeCliFor = new BaseController().ObterCadastroCLIFORCodigo(hidClifor.Value);

            // atualizar pedidos
            var pedidos = atacController.ObterPedidoClienteAtacado(hidColecao.Value, nomeCliFor.NOME_CLIFOR.Trim());
            foreach (var pe in pedidos)
            {
                var vendaPedido = baseController.ObterPedido(pe.PEDIDO);
                vendaPedido.CONDICAO_PGTO = ddlCondPgto.SelectedValue;

                baseController.AtualizarPedidoCondPagamento(vendaPedido);
            }

            totOrigValor = 0;
            var cliPedido = atacController.ObterPedidoClienteAtacado(hidColecao.Value, nomeCliFor.NOME_CLIFOR.Trim());
            gvColecaoPedido.DataSource = cliPedido;
            gvColecaoPedido.DataBind();

        }

    }
}
