using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class desenv_produto_necessidade_compra : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        int coluna = 0;
        decimal qtdeTotalConsumo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarGrupos();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);

            //Evitar duplo clique no botão
            ibtPesquisar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(ibtPesquisar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupos()
        {
            //Obter Grupo da ficha tecnica
            var _grupo = desenvController.ObterFichaTecnica().Where(p => p.GRUPO != null && p.GRUPO.Trim() != "").ToList();

            var cf1 = _grupo.GroupBy(p => new { GRUPO = p.GRUPO.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { GRUPO = k.Key.GRUPO }).ToList();

            cf1.Insert(0, new DESENV_PRODUTO_FICTEC { GRUPO = "" });
            ddlMaterialGrupo.DataSource = cf1;
            ddlMaterialGrupo.DataBind();

        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "ddlMaterialGrupo")
                CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim());
        }
        private void CarregarSubGrupos(string grupo)
        {
            //Obter subGrupo da ficha tecnica
            var _subGrupo = desenvController.ObterFichaTecnica().Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                            p.SUBGRUPO != null && p.SUBGRUPO.Trim() != "").ToList();

            var cf1 = _subGrupo.GroupBy(p => new { SUBGRUPO = p.SUBGRUPO.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { SUBGRUPO = k.Key.SUBGRUPO }).ToList();

            cf1.Insert(0, new DESENV_PRODUTO_FICTEC { SUBGRUPO = "" });
            ddlMaterialSubGrupo.DataSource = cf1;
            ddlMaterialSubGrupo.DataBind();

            if (cf1.Count == 2)
            {
                ddlMaterialSubGrupo.SelectedIndex = 1;
                ddlMaterialSubGrupo_SelectedIndexChanged(ddlMaterialSubGrupo, null);
            }
        }
        protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "ddlMaterialSubGrupo")
                CarregarCores(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedValue.Trim());
        }
        private void CarregarCores(string grupo, string subGrupo)
        {
            //Obter cores da ficha tecnica
            var _cor = desenvController.ObterFichaTecnica().Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                    p.SUBGRUPO.Trim() == subGrupo.Trim() &&
                                    p.COR_MATERIAL != null && p.COR_MATERIAL.Trim() != "").ToList();

            var cf1 = _cor.GroupBy(p => new { COR_MATERIAL = p.COR_MATERIAL.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_MATERIAL = k.Key.COR_MATERIAL }).ToList();

            var _coresBasicas = ObterCoresBasicas();
            _coresBasicas = _coresBasicas.Where(p => cf1.Any(x => x.COR_MATERIAL.Trim() == p.COR.Trim())).ToList();

            _coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
            ddlCor.DataSource = _coresBasicas;
            ddlCor.DataBind();

            if (_coresBasicas.Count == 2)
            {
                ddlCor.SelectedIndex = 1;
                ddlCor_SelectedIndexChanged(ddlCor, null);
            }
        }
        protected void ddlCor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "ddlCor")
                CarregarCorFornecedor(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedValue.Trim(), ddlCor.SelectedValue.Trim());
        }
        private void CarregarCorFornecedor(string grupo, string subGrupo, string cor)
        {
            //Obter cores do fornecedor da ficha tecnica

            var _corFornecedor = desenvController.ObterFichaTecnica().Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                                        p.SUBGRUPO.Trim() == subGrupo.Trim() &&
                                                        p.COR_MATERIAL.Trim() == cor.Trim() &&
                                                        p.COR_FORNECEDOR != null && p.COR_FORNECEDOR.Trim() != "").ToList();
            var cf1 = _corFornecedor.GroupBy(p => new { COR_FORNECEDOR = p.COR_FORNECEDOR.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_FORNECEDOR = k.Key.COR_FORNECEDOR }).ToList();

            cf1.Insert(0, new DESENV_PRODUTO_FICTEC { COR_FORNECEDOR = "" });
            ddlCorFornecedor.DataSource = cf1;
            ddlCorFornecedor.DataBind();

            if (cf1.Count == 2)
                ddlCorFornecedor.SelectedIndex = 1;

        }
        private List<CORES_BASICA> ObterCoresBasicas()
        {
            List<CORES_BASICA> _cores = new List<CORES_BASICA>();
            _cores = new ProducaoController().ObterCoresBasicas();
            return _cores;
        }
        #endregion

        #region "AÇÕES DA TELA"
        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        #endregion

        #region "MATERIAIS"
        private List<SP_OBTER_NECESSIDADE_COMPRAResult> ObterNecessidadeCompraMaterial()
        {
            List<SP_OBTER_NECESSIDADE_COMPRAResult> _necessidadeCompra = new List<SP_OBTER_NECESSIDADE_COMPRAResult>();

            _necessidadeCompra = desenvController.ObterNecessidadeCompraMaterial(
                ddlMaterialGrupo.SelectedValue.Trim(),
                ddlMaterialSubGrupo.SelectedValue.Trim(),
                ddlCor.SelectedValue.Trim(),
                ddlCorFornecedor.SelectedValue.Trim());

            return _necessidadeCompra;
        }
        private void CarregarMateriais()
        {
            var _necessidadeCompra = ObterNecessidadeCompraMaterial();

            if (_necessidadeCompra == null || _necessidadeCompra.Count() <= 0)
                labErro.Text = "Nenhuma Necessidade de Compra encontrada. Refaça sua pesquisa.";

            gvMateriais.DataSource = _necessidadeCompra;
            gvMateriais.DataBind();
        }
        protected void gvMateriais_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_NECESSIDADE_COMPRAResult _necessidadeCompra = e.Row.DataItem as SP_OBTER_NECESSIDADE_COMPRAResult;

                    coluna += 1;
                    if (_necessidadeCompra != null)
                    {

                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = coluna.ToString();
                        //SOMATORIO
                        qtdeTotalConsumo += Convert.ToDecimal(_necessidadeCompra.NECESSIDADE);
                    }
                }
            }
        }
        protected void gvMateriais_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvMateriais.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                footer.Cells[5].Text = qtdeTotalConsumo.ToString("###,###,###,##0.000");
            }
        }
        protected void gvMateriais_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_NECESSIDADE_COMPRAResult> listaNecessidadeCompra = ObterNecessidadeCompraMaterial();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            listaNecessidadeCompra = listaNecessidadeCompra.OrderBy(e.SortExpression + sortDirection);
            gvMateriais.DataSource = listaNecessidadeCompra;
            gvMateriais.DataBind();
        }
        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                /*if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamento os campos em Vermelho.";
                    return;
                }*/

                //Limpar cabecalho
                Utils.WebControls.GetBoundFieldIndexByName(gvMateriais, " - >>");
                Utils.WebControls.GetBoundFieldIndexByName(gvMateriais, " - <<");

                CarregarMateriais();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #endregion

        #region "VALIDACAO"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labGrupo.ForeColor = _OK;
            if (ddlMaterialGrupo.SelectedValue.Trim() == "")
            {
                labGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labSubGrupo.ForeColor = _OK;
            if (ddlMaterialSubGrupo.SelectedValue.Trim() == "")
            {
                labSubGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labCor.ForeColor = _OK;
            if (ddlCor.SelectedValue.Trim() == "")
            {
                labCor.ForeColor = _notOK;
                retorno = false;
            }

            labCorFornecedor.ForeColor = _OK;
            if (ddlCorFornecedor.SelectedValue.Trim() == "Selecione")
            {
                labCorFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private void LimparCampos(bool _cabecalho)
        {
            labErro.Text = "";
        }
        #endregion
    }
}
