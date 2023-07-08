using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_rel_config_cad : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "ecom_menu.aspx";

                var codigoConfigHeader = "";
                var codigoConfig = 0;

                var queryGriffe = "";
                var queryGrupoProduto = "";

                codigoConfigHeader = Request.QueryString["coc"];
                if (codigoConfigHeader != null && codigoConfigHeader != "")
                    codigoConfig = Convert.ToInt32(codigoConfigHeader);

                queryGriffe = Request.QueryString["gg"];
                queryGrupoProduto = Request.QueryString["gp"];


                CarregarFiltro();
                CarregarOperacao();
                CarregarGrupo();
                CarregarGriffe();

                if (queryGriffe != "")
                    ddlGriffe.SelectedValue = baseController.BuscaGriffeProduto(queryGriffe).GRIFFE;
                if (queryGrupoProduto != "")
                    ddlGrupo.SelectedValue = baseController.BuscaGrupoProduto(queryGrupoProduto).GRUPO_PRODUTO;

                var config = ecomController.ObterEcomRelConfigPorCodigo(codigoConfig);
                if (config != null)
                {
                    var configCampo = ecomController.ObterEcomRelConfigCampoPorRelConfig(codigoConfig);
                    foreach (var campo in configCampo)
                    {
                        if (campo.ECOM_REL_CONFIG_FILTRO == 2)
                        {
                            var configGriffe = ecomController.ObterEcomRelConfigCampoValorPorConfigCampo(campo.CODIGO).SingleOrDefault();
                            ddlGriffe.SelectedValue = configGriffe.VALOR;
                            hidGriffe.Value = ddlGriffe.SelectedValue.Trim();

                        }
                        else if (campo.ECOM_REL_CONFIG_FILTRO == 3)
                        {
                            var configGrupoProduto = ecomController.ObterEcomRelConfigCampoValorPorConfigCampo(campo.CODIGO).SingleOrDefault();
                            ddlGrupo.SelectedValue = configGrupoProduto.VALOR;
                            hidGrupoProduto.Value = ddlGrupo.SelectedValue.Trim();
                        }
                    }

                    btSalvar.Text = "Alterar";
                    txtNome.Text = config.NOME;
                    ddlTipo.SelectedValue = config.ECOM_REL_CONFIG_TIPO.ToString();
                    ddlTipo.Enabled = false;
                    hidCodigoConfig.Value = codigoConfig.ToString();
                    pnlFiltro.Visible = true;

                    CarregarConfigCampos(codigoConfig);
                }

            }

            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
            btAddFiltro.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAddFiltro, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            var grupo = prodController.ObterGrupoProduto("");
            if (grupo != null)
            {
                grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "Selecione" });
                ddlGrupo.DataSource = grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "Selecione" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        private void CarregarFiltro()
        {
            var filtro = ecomController.ObterEcomRelFiltros().Where(p => p.CODIGO != 2 && p.CODIGO != 3).OrderBy(p => p.CAMPO_WHERE).ToList();

            filtro.Insert(0, new ECOM_REL_CONFIG_FILTRO { CODIGO = 0, CAMPO_WHERE = "Selecione" });
            ddlFiltro.DataSource = filtro;
            ddlFiltro.DataBind();
        }
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {

            var codigoFiltro = Convert.ToInt32(ddlFiltro.SelectedValue);
            var lstValores = new List<ListItem>();

            string griffe = hidGriffe.Value;
            string grupoProduto = hidGrupoProduto.Value;
            var valoresFiltro = ecomController.ObterEcomRelConfigFiltroValores(codigoFiltro, griffe, grupoProduto);
            foreach (var v in valoresFiltro)
                lstValores.Add(new ListItem { Value = v.VALOR, Text = v.TEXTO });

            if (valoresFiltro == null || valoresFiltro.Count() <= 0)
                lstValores.Insert(0, new ListItem { Value = "0", Text = "SEM VALOR" });
            else
                lstValores.Insert(0, new ListItem { Value = "0", Text = "Selecione" });
            ddlValores.DataSource = lstValores;
            ddlValores.DataBind();

        }
        private void CarregarOperacao()
        {
            var operacoes = ecomController.ObterEcomRelOperacoes();

            operacoes.Insert(0, new ECOM_REL_CONFIG_OPERACOE { CODIGO = 0, OPERACAO = "Selecione" });
            ddlOperacao.DataSource = operacoes;
            ddlOperacao.DataBind();
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroFiltro.Text = "";

                if (ddlGriffe.SelectedValue == "Selecione")
                {
                    labErro.Text = "Selecione a Griffe.";
                    return;
                }

                if (ddlGrupo.SelectedValue == "Selecione")
                {
                    labErro.Text = "Selecione o Grupo de Produto.";
                    return;
                }

                if (txtNome.Text.Trim() == "")
                {
                    labErro.Text = "Informe o nome da configuração.";
                    return;
                }

                if (ddlTipo.SelectedValue == "")
                {
                    labErro.Text = "Selecione o tipo de regra.";
                    return;
                }

                var config = new ECOM_REL_CONFIG();
                if (hidCodigoConfig.Value != "")
                    config = ecomController.ObterEcomRelConfigPorCodigo(Convert.ToInt32(hidCodigoConfig.Value));

                config.NOME = txtNome.Text.Trim().ToUpper();
                config.STATUS = 'A';
                config.DATA_INCLUSAO = DateTime.Now;
                config.ECOM_REL_CONFIG_TIPO = Convert.ToInt32(ddlTipo.SelectedValue);

                var codigoConfig = 0;

                if (hidCodigoConfig.Value == "")
                {
                    codigoConfig = ecomController.InserirEcomRelConfig(config);
                    AtualizarDadosGriffeGrupoProduto(codigoConfig, true);
                }
                else
                {
                    ecomController.AtualizarEcomRelConfig(config);
                    codigoConfig = config.CODIGO;
                    AtualizarDadosGriffeGrupoProduto(codigoConfig, false);
                }

                ddlTipo.Enabled = false;
                hidCodigoConfig.Value = codigoConfig.ToString();

                pnlFiltro.Visible = true;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }


        }
        private void AtualizarDadosGriffeGrupoProduto(int codigoConfig, bool inc)
        {
            if (inc)
            {
                var configCampos = new ECOM_REL_CONFIG_CAMPO();
                configCampos.ECOM_REL_CONFIG = codigoConfig;
                configCampos.ECOM_REL_CONFIG_FILTRO = 2;
                configCampos.ECOM_REL_CONFIG_OPERACOES = 1;
                var codigoConfigCampo = ecomController.InserirEcomRelConfigCampo(configCampos);

                var configCamposVal = new ECOM_REL_CONFIG_CAMPO_VAL();
                configCamposVal.ECOM_REL_CONFIG_CAMPO = codigoConfigCampo;
                configCamposVal.VALOR = ddlGriffe.SelectedValue;
                configCamposVal.VALOR_DESC = ddlGriffe.SelectedValue;
                ecomController.InserirEcomRelConfigCampoValor(configCamposVal);

                configCampos = new ECOM_REL_CONFIG_CAMPO();
                configCampos.ECOM_REL_CONFIG = codigoConfig;
                configCampos.ECOM_REL_CONFIG_FILTRO = 3;
                configCampos.ECOM_REL_CONFIG_OPERACOES = 1;
                codigoConfigCampo = ecomController.InserirEcomRelConfigCampo(configCampos);

                configCamposVal = new ECOM_REL_CONFIG_CAMPO_VAL();
                configCamposVal.ECOM_REL_CONFIG_CAMPO = codigoConfigCampo;
                configCamposVal.VALOR = ddlGrupo.SelectedValue;
                configCamposVal.VALOR_DESC = ddlGrupo.SelectedValue;
                ecomController.InserirEcomRelConfigCampoValor(configCamposVal);
            }
            else
            {
                var configCampo = ecomController.ObterEcomRelConfigCampoPorRelConfig(codigoConfig);
                foreach (var campo in configCampo)
                {
                    if (campo.ECOM_REL_CONFIG_FILTRO == 2)
                    {
                        var configGriffe = ecomController.ObterEcomRelConfigCampoValorPorConfigCampo(campo.CODIGO).SingleOrDefault();
                        configGriffe.VALOR = ddlGriffe.SelectedValue;
                        configGriffe.VALOR_DESC = ddlGriffe.SelectedValue;
                        ecomController.AtualizarEcomRelConfigCampoValor(configGriffe);
                    }
                    else if (campo.ECOM_REL_CONFIG_FILTRO == 3)
                    {
                        var configGrupoProduto = ecomController.ObterEcomRelConfigCampoValorPorConfigCampo(campo.CODIGO).SingleOrDefault();
                        configGrupoProduto.VALOR = ddlGrupo.SelectedValue;
                        configGrupoProduto.VALOR_DESC = ddlGrupo.SelectedValue;
                        ecomController.AtualizarEcomRelConfigCampoValor(configGrupoProduto);
                    }
                }
            }

            hidGriffe.Value = ddlGriffe.SelectedValue.Trim();
            hidGrupoProduto.Value = ddlGrupo.SelectedValue.Trim();

            CarregarConfigCampos(codigoConfig);
        }

        protected void btAddFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroFiltro.Text = "";

                if (ddlFiltro.SelectedValue == "0")
                {
                    labErroFiltro.Text = "Selecione o Campo de Filtro";
                    return;
                }
                if (ddlOperacao.SelectedValue == "0")
                {
                    labErroFiltro.Text = "Selecione a Operação de Filtro";
                    return;
                }

                if (ddlValores.SelectedValue == "0")
                {
                    labErroFiltro.Text = "Selecione o Valor do Filtro";
                    return;
                }

                var codigoConfig = Convert.ToInt32(hidCodigoConfig.Value);

                //verificar se filtro ja foi inserido
                var configCampo = ecomController.ObterEcomRelConfigCampoPorFiltro(codigoConfig, Convert.ToInt32(ddlFiltro.SelectedValue));
                //se nao existe, insere
                if (configCampo == null)
                {
                    var configCampos = new ECOM_REL_CONFIG_CAMPO();
                    configCampos.ECOM_REL_CONFIG = codigoConfig;
                    configCampos.ECOM_REL_CONFIG_FILTRO = Convert.ToInt32(ddlFiltro.SelectedValue);
                    configCampos.ECOM_REL_CONFIG_OPERACOES = Convert.ToInt32(ddlOperacao.SelectedValue);
                    var codigoConfigCampo = ecomController.InserirEcomRelConfigCampo(configCampos);

                    var configCamposVal = new ECOM_REL_CONFIG_CAMPO_VAL();
                    configCamposVal.ECOM_REL_CONFIG_CAMPO = codigoConfigCampo;
                    configCamposVal.VALOR = ddlValores.SelectedValue.Trim();
                    configCamposVal.VALOR_DESC = ddlValores.SelectedItem.Text.Trim();
                    var codigoConfigCampoVal = ecomController.InserirEcomRelConfigCampoValor(configCamposVal);
                }
                else
                {
                    //existe, entao
                    //operacao permite inserir mais de um valor ?
                    if (configCampo.ECOM_REL_CONFIG_OPERACOE.PERMITE_VALORES)
                    {

                        var valorExiste = ecomController.ObterEcomRelConfigCampoValorPorConfigCampo(configCampo.CODIGO).Where(p => p.VALOR.Trim() == ddlValores.SelectedValue.Trim());
                        if (valorExiste != null && valorExiste.Count() > 0)
                        {
                            labErroFiltro.Text = "Este valor do filtro já foi inserido.";
                            return;
                        }

                        var configCamposVal = new ECOM_REL_CONFIG_CAMPO_VAL();
                        configCamposVal.ECOM_REL_CONFIG_CAMPO = configCampo.CODIGO;
                        configCamposVal.VALOR = ddlValores.SelectedValue.Trim();
                        configCamposVal.VALOR_DESC = ddlValores.SelectedItem.Text.Trim();
                        var codigoConfigCampoVal = ecomController.InserirEcomRelConfigCampoValor(configCamposVal);
                    }
                    else
                    {
                        labErroFiltro.Text = "Este filtro já foi inserido. Para altera-lo, por favor, excluir o valor atual.";
                    }
                }

                //obter operacao
                var operacao = ecomController.ObterEcomRelOperacoes(Convert.ToInt32(ddlOperacao.SelectedValue));
                if (!operacao.PERMITE_VALORES)
                {
                    ddlOperacao.SelectedValue = "0";
                    ddlFiltro.SelectedValue = "0";
                }
                ddlValores.SelectedValue = "0";

                CarregarConfigCampos(codigoConfig);
            }
            catch (Exception ex)
            {
                labErroFiltro.Text = ex.Message;
            }
        }

        private void CarregarConfigCampos(int codigoConfig)
        {
            var configCampos = ecomController.ObterEcomRelConfigCampoPorRelConfig(codigoConfig);
            gvCampos.DataSource = configCampos;
            gvCampos.DataBind();
        }

        protected void gvCampos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_REL_CONFIG_CAMPO configCampo = e.Row.DataItem as ECOM_REL_CONFIG_CAMPO;

                    Literal litCampo = e.Row.FindControl("litCampo") as Literal;
                    litCampo.Text = configCampo.ECOM_REL_CONFIG_FILTRO1.CAMPO_WHERE;

                    Literal litOperacao = e.Row.FindControl("litOperacao") as Literal;
                    litOperacao.Text = configCampo.ECOM_REL_CONFIG_OPERACOE.OPERACAO;

                    ListBox lboxValores = e.Row.FindControl("lboxValores") as ListBox;
                    CarregarListaValores(configCampo.CODIGO, lboxValores);

                    Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                    btExcluir.CommandArgument = configCampo.CODIGO.ToString();

                    if (configCampo.ECOM_REL_CONFIG_FILTRO == 2 || configCampo.ECOM_REL_CONFIG_FILTRO == 3)
                        btExcluir.Visible = false;
                }
            }
        }
        protected void gvCampos_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvCampos.FooterRow;
            if (_footer != null)
            {
            }
        }
        private void CarregarListaValores(int codigoConfigCampo, ListBox lboxValores)
        {
            var valoresCampos = ecomController.ObterEcomRelConfigCampoValorPorConfigCampo(codigoConfigCampo);

            // limpar lista
            lboxValores.Items.Clear();

            ListItem item = null;
            foreach (var val in valoresCampos)
            {
                item = lboxValores.Items.FindByValue(val.CODIGO.ToString());
                if (item == null)
                {
                    item = new ListItem();
                    item.Value = val.CODIGO.ToString();
                    item.Text = val.VALOR_DESC;
                    lboxValores.Items.Add(item);
                }
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            try
            {
                labErro.Text = "";

                var codigoConfigCampo = Convert.ToInt32(bt.CommandArgument);
                ecomController.ExcluirEcomRelConfigCampo(codigoConfigCampo);
                CarregarConfigCampos(Convert.ToInt32(hidCodigoConfig.Value));

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btLimpar_Click(object sender, EventArgs e)
        {
            hidCodigoConfig.Value = "";
            pnlFiltro.Visible = false;
            ddlTipo.Enabled = true;
            txtNome.Text = "";
            btSalvar.Text = "Incluir";
            ddlTipo.SelectedValue = "";
            ddlGriffe.SelectedValue = "Selecione";
            hidGriffe.Value = "";
            ddlGrupo.SelectedValue = "Selecione";
            hidGrupoProduto.Value = "";
        }


    }
}
