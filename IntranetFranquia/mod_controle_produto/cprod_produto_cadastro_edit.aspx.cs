using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;
using System.Text;

namespace Relatorios
{
    public partial class cprod_produto_cadastro_edit : System.Web.UI.Page
    {
        ControleProdutoController cprodController = new ControleProdutoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                string colecao = "";
                string produto = "";
                if (Request.QueryString["c"] == null || Request.QueryString["c"] == "" ||
                    Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("cprod_menu.aspx");

                colecao = Request.QueryString["c"].ToString();
                produto = Request.QueryString["p"].ToString();

                SP_OBTER_PRODUTO_SEM_CADASTROResult prodSemCad = cprodController.ObterProdutoSemCadastro(colecao, produto).FirstOrDefault();
                if (prodSemCad == null)
                    Response.Redirect("cprod_menu.aspx");

                //if (prodSemCad.DATA_CADASTRO_LIB != null)
                //{
                //    Response.Write("PRODUTO JÁ LIBERADO PARA CADASTRO.");
                //    Response.End();
                //}

                CarregarProdutosTamanho();
                CarregarLinhas();
                //CarregarGrupos();
                //CarregarNCM();

                hidColecao.Value = colecao;
                hidProduto.Value = produto;

                labColecao.Text = baseController.BuscaColecaoAtual(prodSemCad.COLECAO).DESC_COLECAO;
                labProduto.Text = prodSemCad.MODELO;
                labGrupo.Text = prodSemCad.GRUPO;
                labGriffe.Text = prodSemCad.GRIFFE;
                labCor.Text = prodSemCad.DESC_COR;
                labModelagem.Text = produto;
                labTecido.Text = prodSemCad.TECIDO_POCKET;
                labSigned.Text = prodSemCad.SIGNED_NOME;

                if (prodSemCad.LINHA != null)
                    ddlLinha.SelectedValue = prodSemCad.LINHA;

                if (prodSemCad.SUBGRUPO_PRODUTO != null)
                    ddlSubGrupo.SelectedValue = prodSemCad.SUBGRUPO_PRODUTO;

                //SE ja foi castrado, repete as informações
                var desenvProduto = desenvController.ObterProduto(colecao).Where(p => p.MODELO.Trim() == produto && p.DATA_CADASTRO_LIB != null).FirstOrDefault();
                if (desenvProduto != null)
                {
                    txtNome.Text = desenvProduto.DESC_MODELO;
                    ddlSubGrupo.SelectedValue = (desenvProduto.SUBGRUPO_PRODUTO == null) ? "" : desenvProduto.SUBGRUPO_PRODUTO.Trim();
                    if (desenvProduto.LINHA != null)
                    {
                        var l = baseController.BuscaLinhas().Where(p => p.LINHA.Trim() == desenvProduto.LINHA.Trim()).FirstOrDefault();
                        if (l != null)
                            ddlLinha.SelectedValue = l.LINHA;
                    }
                    ddlGrade.SelectedValue = (desenvProduto.GRADE == null) ? "" : desenvProduto.GRADE.Trim();

                }

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }


        #region "DADOS INICIAIS"
        private void CarregarLinhas()
        {
            var linhas = baseController.BuscaLinhas();

            linhas.Insert(0, new PRODUTOS_LINHA { LINHA = "Selecione" });
            ddlLinha.DataSource = linhas;
            ddlLinha.DataBind();
        }
        private void CarregarProdutosTamanho()
        {
            var prodTam = baseController.BuscaProdutosTamanho().Where(p =>
                p.GRADE.Trim().Contains("KIDS") ||
                p.GRADE.Trim() == "PQ-MD-GD" ||
                p.GRADE.Trim() == "PP-PQ-MD-GD-2G-3G" ||
                p.GRADE.Trim() == "PQ-MD-GD-2G-3G" ||
                p.GRADE.Trim() == "34-36-38-40-42-44" ||
                p.GRADE.Trim() == "34-36-38-40-42-44-46" ||
                p.GRADE.Trim() == "PP-PQ-MD-GD" ||
                p.GRADE.Trim() == "PP-PQ-MD-GD-GG" ||
                p.GRADE.Trim() == "PQ-MD-GD-GG" ||
                p.GRADE.Trim() == "XP-PP-PQ-MD-GD" ||
                p.GRADE.Trim() == "PQ-MD-GD-GG-XG" ||
                p.GRADE.Trim() == "UNICO" ||
                p.GRADE.Trim() == "38-40-42-44-46-48" ||
                p.GRADE.Trim() == "XP-PP-PQ-MD-GD-GG"
                ).OrderBy(p => p.GRADE).ToList();

            prodTam.Insert(0, new PRODUTOS_TAMANHO { GRADE = "Selecione" });
            ddlGrade.DataSource = prodTam;
            ddlGrade.DataBind();
        }
        //private void CarregarNCM()
        //{
        //    var ncm = cprodController.ObterProdutoNCM();

        //    ncm.Insert(0, new PRODUTO_NCM { CLASSIF_FISCAL = "Selecione" });
        //    ddlNCM.DataSource = ncm;
        //    ddlNCM.DataBind();
        //}

        //private void CarregarGrupos()
        //{
        //    List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo().Where(p => p.CODIGO_GRUPO.Trim() != "01").OrderBy(o => o.GRUPO.Trim()).ToList();
        //    if (_matGrupo != null)
        //    {
        //        _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "Selecione" });
        //        ddlMaterialGrupo.DataSource = _matGrupo;
        //        ddlMaterialGrupo.DataBind();
        //    }
        //}
        //protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim());
        //    ObterNCM(ddlSubGrupo.SelectedValue.Trim(), labGriffe.Text.Trim(), labGrupo.Text.Trim(), hidMaterial.Value);
        //}
        //private void CarregarSubGrupos(string grupo)
        //{
        //    List<MATERIAIS_SUBGRUPO> _matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).OrderBy(o => o.SUBGRUPO.Trim()).ToList();
        //    if (_matSubGrupo != null)
        //    {
        //        _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "Selecione" });
        //        ddlMaterialSubGrupo.DataSource = _matSubGrupo;
        //        ddlMaterialSubGrupo.DataBind();

        //        if (_matSubGrupo.Count() == 2)
        //        {
        //            ddlMaterialSubGrupo.SelectedIndex = 1;
        //            ddlMaterialSubGrupo_SelectedIndexChanged(null, null);
        //        }
        //    }
        //}
        //protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //List<MATERIAL_LAVAGEM> matLav = new List<MATERIAL_LAVAGEM>();
        //    List<MATERIAL_COMPOSICAO> matComp = new List<MATERIAL_COMPOSICAO>();

        //    hidMaterial.Value = "";
        //    MATERIAI _material = desenvController.ObterMaterial(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim()).FirstOrDefault();
        //    if (_material != null)
        //    {
        //        hidMaterial.Value = _material.MATERIAL.Trim();
        //        //matLav = desenvController.ObterMaterialLavagem(_material.MATERIAL);
        //        matComp = desenvController.ObterMaterialComposicao(_material.MATERIAL);
        //    }

        //    //gvLavagem.DataSource = matLav;
        //    //gvLavagem.DataBind();

        //    gvComposicao.DataSource = matComp;
        //    gvComposicao.DataBind();

        //    ObterNCM(ddlSubGrupo.SelectedValue.Trim(), labGriffe.Text.Trim(), labGrupo.Text.Trim(), hidMaterial.Value);
        //}

        protected void ddlSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ObterNCM(ddlSubGrupo.SelectedValue.Trim(), labGriffe.Text.Trim(), labGrupo.Text.Trim(), hidMaterial.Value);
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labNome.ForeColor = _OK;
            if (txtNome.Text.Trim() == "")
            {
                labNome.ForeColor = _notOK;
                retorno = false;
            }

            labSubGrupo.ForeColor = _OK;
            if (ddlSubGrupo.SelectedValue == "Selecione")
            {
                labSubGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labLinha.ForeColor = _OK;
            if (ddlLinha.SelectedValue == "Selecione")
            {
                labLinha.ForeColor = _notOK;
                retorno = false;
            }

            labGrade.ForeColor = _OK;
            if (ddlGrade.SelectedValue == "Selecione")
            {
                labGrade.ForeColor = _notOK;
                retorno = false;
            }

            //labTecidoGrupo.ForeColor = _OK;
            //if (ddlMaterialGrupo.SelectedValue.Trim() == "Selecione")
            //{
            //    labTecidoGrupo.ForeColor = _notOK;
            //    retorno = false;
            //}

            //labTecidoSubGrupo.ForeColor = _OK;
            //if (ddlMaterialSubGrupo.SelectedValue.Trim() == "Selecione")
            //{
            //    labTecidoSubGrupo.ForeColor = _notOK;
            //    retorno = false;
            //}

            //labNCM.ForeColor = _OK;
            //if (ddlNCM.SelectedValue.Trim() == "Selecione" && ddlNCM.Enabled)
            //{
            //    labNCM.ForeColor = _notOK;
            //    retorno = false;
            //}

            //labNCMOutro.ForeColor = _OK;
            //if (ddlNCM.SelectedValue.Trim() == "Selecione" && !ddlNCM.Enabled && txtNCM.Text.Trim() == "")
            //{
            //    labNCMOutro.ForeColor = _notOK;
            //    retorno = false;
            //}

            return retorno;
        }
        #endregion

        //private void ObterNCM(string subGrupoProduto, string griffe, string grupoProduto, string material)
        //{
        //    string composicao = "";

        //    var compNCM = cprodController.ObterComposicaoNCM(material);
        //    if (compNCM != null)
        //        composicao = compNCM.COMPOSICAO;

        //    var ncm = cprodController.ObterProdutoNCM(subGrupoProduto, griffe, composicao, grupoProduto).Select(p => new PRODUTO_NCM
        //    {
        //        CLASSIF_FISCAL = p.CLASSIF_FISCAL.Trim()
        //    }).Distinct().ToList();

        //    ddlNCM.Enabled = true;
        //    txtNCM.Enabled = false;
        //    txtNCM.Text = "";
        //    if (ncm == null || ncm.Count() <= 0)
        //    {
        //        ddlNCM.Enabled = false;
        //        txtNCM.Enabled = true;
        //    }




        //    ncm.Insert(0, new PRODUTO_NCM { CLASSIF_FISCAL = "Selecione" });
        //    ddlNCM.DataSource = ncm;
        //    ddlNCM.DataBind();

        //    if (ncm != null && ncm.Count() == 2)
        //    {
        //        ddlNCM.SelectedIndex = 1;
        //        ddlNCM.Enabled = false;
        //    }
        //}

        //protected void gvLavagem_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if (e.Row.DataItem != null)
        //        {
        //            MATERIAL_LAVAGEM _matLav = e.Row.DataItem as MATERIAL_LAVAGEM;

        //            if (_matLav != null)
        //            {
        //                System.Web.UI.WebControls.Image _imgLavagem = e.Row.FindControl("imgLavagem") as System.Web.UI.WebControls.Image;
        //                if (_imgLavagem != null)
        //                    _imgLavagem.ImageUrl = _matLav.IMAGEM;
        //            }
        //        }
        //    }

        //}
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                List<DESENV_PRODUTO> produtos = new List<DESENV_PRODUTO>();
                produtos = desenvController.ObterProduto(hidColecao.Value).Where(p =>
                    p.MODELO.Trim() == hidProduto.Value &&
                    p.STATUS == 'A' &&
                    //p.DATA_CADASTRO_LIB == null &&
                    p.DATA_APROVACAO != null).ToList();
                if (produtos == null || produtos.Count() <= 0)
                {
                    labErro.Text = "Produto não encontrado. Entre em contato com TI.";
                    return;
                }

                DESENV_PRODUTO prod = null;
                List<DESENV_PRODUTO> lstProdutosN = new List<DESENV_PRODUTO>();
                foreach (var p in produtos)
                {
                    prod = desenvController.ObterProduto(p.CODIGO);
                    if (prod != null)
                    {
                        prod.DESC_MODELO = txtNome.Text.Trim().ToUpper();
                        prod.SUBGRUPO_PRODUTO = ddlSubGrupo.SelectedValue.Trim().ToUpper();
                        prod.LINHA = ddlLinha.SelectedValue.ToUpper();
                        prod.GRADE = ddlGrade.SelectedValue.Trim().ToUpper();
                        //prod.NCM = (ddlNCM.SelectedValue != "Selecione") ? ddlNCM.SelectedValue : txtNCM.Text.Trim();
                        prod.DATA_CADASTRO_LIB = DateTime.Now;
                        //prod.LAVAGEM_MATERIAL = hidMaterial.Value;
                        desenvController.AtualizarProduto(prod);

                        lstProdutosN.Add(prod);
                    }
                }

                if (Constante.enviarEmail)
                    EnviarEmail(lstProdutosN);

                labErro.Text = "Produto liberado para cadastro com sucesso.";
                labErro.Font.Bold = true;
                btSalvar.Enabled = false;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "EMAIL"
        private void EnviarEmail(List<DESENV_PRODUTO> produtos)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Cadastro de Produto - " + produtos[0].MODELO;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(produtos);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(10, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(List<DESENV_PRODUTO> produtos)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Cadastro de Produto</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divPreco' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 600pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:left;'>");
            sb.Append("                        <h3>Cadastro de Produto - " + produtos[0].MODELO + "</h3>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 600pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    Coleção:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + baseController.BuscaColecaoAtual(produtos[0].COLECAO).DESC_COLECAO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Produto:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos[0].MODELO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Nome:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos[0].DESC_MODELO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Grupo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos[0].GRUPO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    SubGrupo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos[0].SUBGRUPO_PRODUTO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Griffe:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos[0].GRIFFE);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Signed/Estilista:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos[0].SIGNED_NOME);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Linha:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos[0].LINHA);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Tipo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ((produtos[0].PRODUTO_ACABADO == 'N') ? "NACIONAL" : "TERCEIROS - " + produtos[0].FORNECEDOR));
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor:");
            sb.Append("                                </td>");

            string cores = "";
            ProducaoController prodController = new ProducaoController();
            foreach (var p in produtos)
                cores = cores + p.COR + " - " + prodController.ObterCoresBasicas(p.COR).DESC_COR.Trim() + ", ";

            cores = cores.Trim() + ",";
            cores = cores.Replace(",,", "");
            sb.Append("                                <td>");
            sb.Append("                                    " + cores);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Modelagem:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos[0].MODELO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Tamanho:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos[0].GRADE);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    NCM:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ((produtos[0].NCM == null) ? "Não Informado" : produtos[0].NCM));
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            //var comp = desenvController.ObterMaterialComposicao(hidMaterial.Value);

            //if (comp != null && comp.Count() > 0)
            //{
            //    sb.Append("                            <tr>");
            //    sb.Append("                                <td>");
            //    sb.Append("                                    Composição:");
            //    sb.Append("                                </td>");
            //    sb.Append("                                <td>");
            //    sb.Append("                                    " + comp[0].QTDE + "% " + comp[0].DESCRICAO);
            //    sb.Append("                                </td>");
            //    sb.Append("                                <td>");
            //    sb.Append("                                    &nbsp;");
            //    sb.Append("                                </td>");
            //    sb.Append("                            </tr>");
            //    foreach (var c in comp.Where(p => p.CODIGO != comp[0].CODIGO))
            //    {
            //        sb.Append("                            <tr>");
            //        sb.Append("                                <td>");
            //        sb.Append("                                    &nbsp;");
            //        sb.Append("                                </td>");
            //        sb.Append("                                <td>");
            //        sb.Append("                                    " + c.QTDE + "% " + c.DESCRICAO);
            //        sb.Append("                                </td>");
            //        sb.Append("                                <td>");
            //        sb.Append("                                    &nbsp;");
            //        sb.Append("                                </td>");
            //        sb.Append("                            </tr>");
            //    }
            //}
            /*
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Lavagem:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            foreach (var lav in desenvController.ObterMaterialLavagem(hidMaterial.Value))
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                     <img alt='' Width='30px' Height='30px' src='http://cmaxweb.dnsalias.com:8585" + lav.IMAGEM.Replace("~", "") + "' />&nbsp;");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            */
            sb.Append("                            <tr>");
            sb.Append("                                <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + (new BaseController().BuscaUsuario(codigoUsuario).NOME_USUARIO.ToUpper()) + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion


    }
}
