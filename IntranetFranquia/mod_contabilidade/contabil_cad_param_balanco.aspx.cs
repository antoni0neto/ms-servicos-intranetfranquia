using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;

namespace Relatorios.mod_financeiro
{
    public partial class contabil_cad_param_balanco : System.Web.UI.Page
    {
        ContabilidadeController contabilidadeController = new ContabilidadeController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarBalancoConta();
                CarregarContas();
            }

            btIncluir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btIncluir, null) + ";");

        }

        private List<CTB_BALANCO_CONTA> ObterBalancoConta()
        {
            return contabilidadeController.ObterBalancoConta();
        }
        private void CarregarBalancoConta()
        {
            gvParamBalanco.DataSource = ObterBalancoConta();
            gvParamBalanco.DataBind();
        }

        #region "DADOS INICIAS"
        private void CarregarSubGrupo(string grupo)
        {
            List<ListItem> l = new List<ListItem>();

            if (grupo != "")
            {
                l.Add(new ListItem { Value = "", Text = "Selecione" });
                l.Add(new ListItem { Value = "CIRCULANTE", Text = "CIRCULANTE" });
                l.Add(new ListItem { Value = "NÃO CIRCULANTE", Text = "NÃO CIRCULANTE" });

                if (grupo == "PASSIVO")
                    l.Add(new ListItem { Value = "PATRIMÔNIO LÍQUIDO", Text = "PATRIMÔNIO LÍQUIDO" });
            }
            ddlSubGrupo.DataSource = l;
            ddlSubGrupo.DataBind();
        }
        private void CarregarClassificacao(string grupo, string subgrupo)
        {
            List<ListItem> l = new List<ListItem>();

            if (grupo != "" && subgrupo != "")
            {
                l.Add(new ListItem { Value = "", Text = "Selecione" });

                if (grupo == "ATIVO")
                {
                    if (subgrupo == "CIRCULANTE")
                    {
                        l.Add(new ListItem { Value = "DISPONÍVEL", Text = "DISPONÍVEL" });
                        l.Add(new ListItem { Value = "CRÉDITOS REALIZÁVEIS", Text = "CRÉDITOS REALIZÁVEIS" });
                    }
                    else if (subgrupo == "NÃO CIRCULANTE")
                    {
                        l.Add(new ListItem { Value = "REALIZÁVEL A LONGO PRAZO", Text = "REALIZÁVEL A LONGO PRAZO" });
                        l.Add(new ListItem { Value = "PERMANENTE", Text = "PERMANENTE" });
                        l.Add(new ListItem { Value = "INTANGÍVEL", Text = "INTANGÍVEL" });
                    }
                }
                else // PASSIVO
                {
                    if (subgrupo == "CIRCULANTE")
                    {
                        l.Add(new ListItem { Value = "OBRIGAÇÕES A PAGAR", Text = "OBRIGAÇÕES A PAGAR" });
                        l.Add(new ListItem { Value = "OBRIGAÇÕES FISCAIS", Text = "OBRIGAÇÕES FISCAIS" });
                    }
                    else if (subgrupo == "NÃO CIRCULANTE")
                    {
                        l.Add(new ListItem { Value = "EXIGÍVEL A LONGO PRAZO", Text = "EXIGÍVEL A LONGO PRAZO" });
                    }
                    else if (subgrupo == "PATRIMÔNIO LÍQUIDO")
                    {
                        l.Add(new ListItem { Value = "PATRIMÔNIO LÍQUIDO", Text = "PATRIMÔNIO LÍQUIDO" });
                    }
                }
            }

            ddlClassificacao.DataSource = l;
            ddlClassificacao.DataBind();

        }
        private void CarregarContas()
        {
            var c = contabilidadeController.ObterVisaoConta();

            c.Insert(0, new SP_OBTER_VISAO_CONTAResult { CLASSIFICACAO = "Selecione", DESCR_CONTA = "Selecione" });
            ddlConta.DataSource = c;
            ddlConta.DataBind();
        }

        #endregion

        #region "CRUD"
        private void Incluir(CTB_BALANCO_CONTA balancoConta)
        {
            contabilidadeController.InserirBalancoConta(balancoConta);
        }
        private void Excluir(int codigoBalancoConta)
        {
            contabilidadeController.ExcluirBalancoConta(codigoBalancoConta);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlGrupo.SelectedValue == "")
                {
                    labErro.Text = "Selecione o Grupo.";
                    return;
                }

                if (ddlSubGrupo.SelectedValue == "Selecione")
                {
                    labErro.Text = "Selecione o SubGrupo.";
                    return;
                }

                if (ddlClassificacao.SelectedValue == "Selecione")
                {
                    labErro.Text = "Selecione a Classificação.";
                    return;
                }

                if (ddlConta.SelectedValue == "Selecione")
                {
                    labErro.Text = "Selecione a Conta.";
                    return;
                }

                var x = contabilidadeController.ObterBalancoConta(ddlConta.SelectedValue);
                if (x != null)
                {
                    labErro.Text = "Conta já cadastrada - " + ddlGrupo.SelectedValue + " / " + ddlSubGrupo.SelectedValue + " / " + ddlClassificacao.SelectedValue + " / " + ddlConta.SelectedValue;
                    return;
                }


                CTB_BALANCO_CONTA novo = new CTB_BALANCO_CONTA();
                novo.GRUPO = ddlGrupo.SelectedValue.Trim();
                novo.SUBGRUPO = ddlSubGrupo.SelectedValue.Trim();
                novo.CLASSIFICACAO = ddlClassificacao.SelectedValue.Trim();
                novo.CONTA = ddlConta.SelectedValue.Trim();
                novo.DATA_ALTERACAO = DateTime.Now;
                novo.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                contabilidadeController.InserirBalancoConta(novo);

                CarregarBalancoConta();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }

        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (b != null)
            {
                labErro.Text = "";
                try
                {
                    int codigoBalancoConta = Convert.ToInt32(b.CommandArgument);
                    Excluir(codigoBalancoConta);
                    CarregarBalancoConta();

                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }

        }

        #endregion

        protected void gvParamBalanco_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<CTB_BALANCO_CONTA> paramBalanco = ObterBalancoConta();

            string sortExpression = e.SortExpression;
            SortDirection sort = SortDirection.Ascending;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            paramBalanco = paramBalanco.OrderBy(e.SortExpression + sortDirection);
            gvParamBalanco.DataSource = paramBalanco;
            gvParamBalanco.DataBind();

        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarSubGrupo(ddlGrupo.SelectedValue);
        }

        protected void ddlSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarClassificacao(ddlGrupo.SelectedValue.Trim(), ddlSubGrupo.SelectedValue.Trim());
        }

        protected void gvParamBalanco_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CTB_BALANCO_CONTA bc = e.Row.DataItem as CTB_BALANCO_CONTA;

                    if (bc != null)
                    {
                        Literal _litConta = e.Row.FindControl("litConta") as Literal;
                        if (_litConta != null)
                        {
                            _litConta.Text = contabilidadeController.ObterVisaoConta(bc.CONTA).DESCR_CONTA.Trim();
                        }
                    }
                }
            }
        }


    }
}