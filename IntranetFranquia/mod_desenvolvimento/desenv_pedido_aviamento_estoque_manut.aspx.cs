using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;
using System.Data.OleDb;

namespace Relatorios
{
    public partial class desenv_pedido_aviamento_estoque_manut : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarGrupos();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private void CarregarGrupos()
        {
            List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "" });
                ddlMaterialGrupo.DataSource = _matGrupo;
                ddlMaterialGrupo.DataBind();

            }
        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim());

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarSubGrupos(string grupo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = null;
            if (grupo.Trim() != "")
                _matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();
            else
                _matSubGrupo = desenvController.ObterMaterialSubGrupo();

            if (_matSubGrupo != null)
            {
                _matSubGrupo = _matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "" });

                ddlMaterialSubGrupo.DataSource = _matSubGrupo;
                ddlMaterialSubGrupo.DataBind();
            }
        }
        protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCores(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim());
        }
        private void CarregarCores(string grupo, string subGrupo)
        {
            var materiais = desenvController.ObterMaterialCorV2(grupo, subGrupo, "");
            var coresBasicas = prodController.ObterCoresBasicas();

            coresBasicas = coresBasicas.Where(p => materiais.Any(x => x.COR_MATERIAL.Trim() == p.COR.Trim())).ToList();
            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

            ddlCor.DataSource = coresBasicas;
            ddlCor.DataBind();

            if (coresBasicas.Count == 2)
                ddlCor.SelectedIndex = 1;

        }

        protected void gvEstoqueAviamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_ESTOQUEResult estoque = e.Row.DataItem as SP_OBTER_MATERIAL_ESTOQUEResult;

                    if (estoque != null)
                    {


                    }
                }
            }
        }

        private void CarregarEstoque(string material, string grupo, string subgrupo, string cor, string corFornecedor)
        {

            var materialEstoque = desenvController.ObterMaterialEstoque(material, grupo, subgrupo, cor, "");

            gvEstoqueAviamento.DataSource = materialEstoque;
            gvEstoqueAviamento.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";


                var subGrupoMaterial = (ddlMaterialSubGrupo.SelectedItem != null) ? ddlMaterialSubGrupo.SelectedItem.Text.Trim() : "";
                CarregarEstoque(txtMaterial.Text, ddlMaterialGrupo.SelectedItem.Text.Trim(), subGrupoMaterial, ddlCor.SelectedValue.Trim(), "");

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }


        protected void btAjustarEstoque_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (Session["USUARIO"] != null)
                {


                    Button bt = (Button)sender;
                    GridViewRow row = (GridViewRow)bt.NamingContainer;

                    var material = gvEstoqueAviamento.DataKeys[row.RowIndex][0].ToString();
                    var corMaterial = gvEstoqueAviamento.DataKeys[row.RowIndex][1].ToString();
                    var qtde = ((TextBox)row.FindControl("txtQtdeAjuste")).Text;

                    if (qtde == "" || qtde == "0")
                        return;

                    var estoque = new DESENV_MATERIAL_ESTOQUE();
                    estoque.MATERIAL = material;
                    estoque.COR_MATERIAL = corMaterial;
                    estoque.DATA_ENTRADA = DateTime.Now;
                    estoque.QTDE_ESTOQUE = Convert.ToDecimal(qtde);
                    estoque.CODIGO_USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    desenvController.InserirMaterialEstoque(estoque);

                    var subGrupoMaterial = (ddlMaterialSubGrupo.SelectedItem != null) ? ddlMaterialSubGrupo.SelectedItem.Text.Trim() : "";
                    CarregarEstoque(txtMaterial.Text, ddlMaterialGrupo.SelectedItem.Text.Trim(), subGrupoMaterial, ddlCor.SelectedValue.Trim(), "");
                }
                else
                {
                    labErro.Text = "Sua sessão caiu. Faça login novamente.";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

    }

}
