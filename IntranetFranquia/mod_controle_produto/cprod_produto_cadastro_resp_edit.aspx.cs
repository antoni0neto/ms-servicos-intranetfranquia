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
    public partial class cprod_produto_cadastro_resp_edit : System.Web.UI.Page
    {
        ControleProdutoController cprodController = new ControleProdutoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        PROD_GRADE gradeNome = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                int codigoHB = 0;
                if (
                    Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("cprod_menu.aspx");

                codigoHB = Convert.ToInt32(Request.QueryString["p"].ToString());

                SP_OBTER_PRODUTO_CORTADOResult prodCortado = cprodController.ObterProdutoCortado(codigoHB, null, "", "", "").FirstOrDefault();
                if (prodCortado == null || codigoHB == 0)
                    Response.Redirect("cprod_menu.aspx");

                hidMaterial.Value = prodCortado.LAVAGEM_MATERIAL;
                hidCodigoHB.Value = prodCortado.PROD_HB.ToString();

                labColecao.Text = baseController.BuscaColecaoAtual(prodCortado.COLECAO).DESC_COLECAO;
                labProduto.Text = prodCortado.PRODUTO;
                labGrupo.Text = prodCortado.GRUPO;
                labGriffe.Text = prodCortado.GRIFFE;
                labNome.Text = prodCortado.DESC_MODELO;
                labHB.Text = prodCortado.HB.ToString();
                labCor.Text = prodCortado.DESC_COR;
                labKids.Text = (prodCortado.KIDS == "S") ? "SIM" : "NÃO";

                CarregarNCM();

                var hb = prodController.ObterHB(Convert.ToInt32(hidCodigoHB.Value));
                if (hb != null)
                {
                    var materialList = desenvController.ObterMaterial(hb.GRUPO_TECIDO, hb.TECIDO);
                    foreach (var m in materialList)
                    {
                        var matCor = desenvController.ObterMaterialCor(m.MATERIAL, hb.COR, hb.COR_FORNECEDOR);
                        if (matCor != null)
                        {
                            var matComp = desenvController.ObterMaterialComposicao(m.MATERIAL);
                            if (matComp != null && matComp.Count() > 0)
                            {
                                hidMaterial.Value = m.MATERIAL;
                                break;
                            }
                        }
                    }
                }

                var comp = prodController.ObterListaComposicaoHB(Convert.ToInt32(prodCortado.PROD_HB)).Where(p => p.ETIQUETA_COMPOSICAO == 'S');
                var compAviamento = prodController.ObterListaComposicaoHBAviamento(Convert.ToInt32(prodCortado.PROD_HB)).Where(p => p.ETIQUETA_COMPOSICAO == 'S');

                List<SP_OBTER_COMPOSICAO_HBResult> compList = new List<SP_OBTER_COMPOSICAO_HBResult>();
                compList.AddRange(comp);
                foreach (var a in compAviamento)
                    compList.Add(new SP_OBTER_COMPOSICAO_HBResult { COMPOSICAO = a.COMPOSICAO, DESCRICAO = a.DESCRICAO, ETIQUETA_COMPOSICAO = a.ETIQUETA_COMPOSICAO });

                gvComposicao.DataSource = compList;
                gvComposicao.DataBind();

                List<PROD_HB_GRADE> _grade = new List<PROD_HB_GRADE>();
                PROD_HB_GRADE grade = prodController.ObterGradeHB(codigoHB, 3);
                if (grade != null)
                {

                    if (prodCortado.PROD_GRADE != null)
                        gradeNome = prodController.ObterGradeNome(Convert.ToInt32(prodCortado.PROD_GRADE));

                    _grade.Add(grade);
                    gvGrade.DataSource = _grade;
                    gvGrade.DataBind();
                }


                //CONTROLE DE NCM
                ObterNCM(prodCortado.SUBGRUPO_PRODUTO, prodCortado.GRIFFE, prodCortado.GRUPO, hidMaterial.Value);

                //VEM DEPOIS
                var lav = desenvController.ObterMaterialLavagem(hidMaterial.Value);
                gvLavagem.DataSource = lav;
                gvLavagem.DataBind();

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarNCM()
        {
            var ncm = cprodController.ObterProdutoNCM();

            ncm.Insert(0, new PRODUTO_NCM { CLASSIF_FISCAL = "Selecione" });
            ddlNCM.DataSource = ncm;
            ddlNCM.DataBind();

        }


        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = false;

            labComposicao.ForeColor = _notOK;
            foreach (GridViewRow row in gvComposicao.Rows)
                if (((CheckBox)row.FindControl("cbMarcarComp")).Checked)
                {
                    retorno = true;
                    labComposicao.ForeColor = _OK;
                    break;
                }

            labNCM.ForeColor = _OK;
            if (ddlNCM.SelectedValue.Trim() == "Selecione" && ddlNCM.Enabled)
            {
                labNCM.ForeColor = _notOK;
                retorno = false;
            }

            labNCMOutro.ForeColor = _OK;
            if (ddlNCM.SelectedValue.Trim() == "Selecione" && !ddlNCM.Enabled && txtNCM.Text.Trim() == "")
            {
                labNCMOutro.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        private void ObterNCM(string subGrupoProduto, string griffe, string grupoProduto, string material)
        {

            string composicao = "";

            var compNCM = cprodController.ObterComposicaoNCM(material);
            if (compNCM != null)
                composicao = compNCM.COMPOSICAO;

            var ncm = cprodController.ObterProdutoNCM(subGrupoProduto, griffe, composicao, grupoProduto).Select(p => new PRODUTO_NCM
            {
                CLASSIF_FISCAL = p.CLASSIF_FISCAL.Trim()
            }).Distinct().ToList();

            ddlNCM.Enabled = true;
            txtNCM.Enabled = false;
            txtNCM.Text = "";
            if (ncm == null || ncm.Count() <= 0)
            {
                ddlNCM.Enabled = false;
                txtNCM.Enabled = true;
            }

            ncm.Insert(0, new PRODUTO_NCM { CLASSIF_FISCAL = "Selecione" });
            ddlNCM.DataSource = ncm;
            ddlNCM.DataBind();

            if (ncm != null && ncm.Count() == 2)
            {
                ddlNCM.SelectedIndex = 1;
                ddlNCM.Enabled = false;
            }

        }

        protected void gvGrade_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (gradeNome != null)
                {
                    e.Row.Cells[1].Text = gradeNome.GRADE_EXP;    //EXP
                    e.Row.Cells[2].Text = gradeNome.GRADE_XP;     //XP
                    e.Row.Cells[3].Text = gradeNome.GRADE_PP;     //PP
                    e.Row.Cells[4].Text = gradeNome.GRADE_P;      //P
                    e.Row.Cells[5].Text = gradeNome.GRADE_M;      //M
                    e.Row.Cells[6].Text = gradeNome.GRADE_G;      //G
                    e.Row.Cells[7].Text = gradeNome.GRADE_GG;     //GG
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_GRADE _HB = e.Row.DataItem as PROD_HB_GRADE;

                    if (_HB != null)
                    {
                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = _HB.PROD_HB1.COR.Trim() + " - " + prodController.ObterCoresBasicas(_HB.PROD_HB1.COR).DESC_COR.Trim();

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                            _litTotal.Text = (_HB.GRADE_EXP + _HB.GRADE_XP + _HB.GRADE_PP + _HB.GRADE_P + _HB.GRADE_M + _HB.GRADE_G + _HB.GRADE_GG).ToString();
                    }
                }
            }
        }
        protected void gvLavagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    MATERIAL_LAVAGEM _matLav = e.Row.DataItem as MATERIAL_LAVAGEM;

                    if (_matLav != null)
                    {
                        System.Web.UI.WebControls.Image _imgLavagem = e.Row.FindControl("imgLavagem") as System.Web.UI.WebControls.Image;
                        if (_imgLavagem != null)
                            _imgLavagem.ImageUrl = _matLav.IMAGEM;
                    }
                }
            }

        }

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

                var hb = prodController.ObterHB(Convert.ToInt32(hidCodigoHB.Value));
                if (hb == null)
                {
                    labErro.Text = "HB não encontrado. Entre em contato com TI.";
                    return;
                }

                hb.DATA_ENVIO_ETI_COMP = DateTime.Now;
                prodController.AtualizarHB(hb);

                PROD_HB_ETIQUETA_COMP etiquetaComp = null;
                prodController.ExcluirEtiquetaComposicao(Convert.ToInt32(hidCodigoHB.Value));
                foreach (GridViewRow row in gvComposicao.Rows)
                {
                    if (((CheckBox)row.FindControl("cbMarcarComp")).Checked)
                    {
                        etiquetaComp = new PROD_HB_ETIQUETA_COMP();
                        etiquetaComp.PROD_HB = Convert.ToInt32(hidCodigoHB.Value);
                        etiquetaComp.DETALHE = ((Literal)row.FindControl("litDescricao")).Text.Trim();
                        etiquetaComp.COMPOSICAO = ((Literal)row.FindControl("litComposicao")).Text.Trim();

                        prodController.InserirEtiquetaComposicao(etiquetaComp);
                    }
                }

                if (Constante.enviarEmail)
                    EnviarEmail(hb);

                labErro.Text = "HB enviado com sucesso para produção de Etiqueta de Composição.";
                labErro.Font.Bold = true;
                btSalvar.Enabled = false;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "EMAIL"
        private void EnviarEmail(PROD_HB hb)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Etiqueta de Composição - " + hb.CODIGO_PRODUTO_LINX;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(hb);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(10, 2).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(PROD_HB hb)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var _gradeNome = prodController.ObterGradeNome(Convert.ToInt32(hb.PROD_GRADE));

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Etiqueta de Composicao</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divEtiqueta' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:left;'>");
            sb.Append("                        <h3>Etiqueta de Composição " + " - " + ((hb.MOSTRUARIO == 'S') ? "MOSTRUÁRIO" : "PRODUÇÃO") + " - Módulo de Controle de Produto</h3>");
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
            sb.Append("                            <tr>");
            sb.Append("                                <td style='width: 165px;'>");
            sb.Append("                                    Produto:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + hb.CODIGO_PRODUTO_LINX);
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
            sb.Append("                                    " + hb.NOME);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + hb.COR.Trim() + " - " + prodController.ObterCoresBasicas(hb.COR).DESC_COR);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    KIDS:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ((hb.KIDS == 'S') ? "SIM" : "NÃO"));
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
            sb.Append("                                    " + ((txtNCM.Text.Trim() != "") ? txtNCM.Text.Trim() : ddlNCM.SelectedValue.Trim()));
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            var comp = prodController.ObterListaComposicaoHB(Convert.ToInt32(hb.CODIGO)).Where(p => p.ETIQUETA_COMPOSICAO == 'S');
            var compAviamento = prodController.ObterListaComposicaoHBAviamento(Convert.ToInt32(hb.CODIGO)).Where(p => p.ETIQUETA_COMPOSICAO == 'S');

            List<SP_OBTER_COMPOSICAO_HBResult> compList = new List<SP_OBTER_COMPOSICAO_HBResult>();
            compList.AddRange(comp);
            foreach (var a in compAviamento)
                compList.Add(new SP_OBTER_COMPOSICAO_HBResult { COMPOSICAO = a.COMPOSICAO, DESCRICAO = a.DESCRICAO, ETIQUETA_COMPOSICAO = a.ETIQUETA_COMPOSICAO });

            if (compList != null && compList.Count() > 0)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Composição:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + compList[0].DESCRICAO + " - " + compList[0].COMPOSICAO);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
                foreach (var c in compList.Where(p => p.DESCRICAO != compList[0].DESCRICAO))
                {
                    sb.Append("                            <tr>");
                    sb.Append("                                <td>");
                    sb.Append("                                    &nbsp;");
                    sb.Append("                                </td>");
                    sb.Append("                                <td>");

                    if (c.DESCRICAO.Trim().ToUpper() == "COR 2" || c.DESCRICAO.Trim().ToUpper() == "COR 3")
                        sb.Append("                                    DETALHE - " + c.COMPOSICAO);
                    else
                        sb.Append("                                    " + c.DESCRICAO + " - " + c.COMPOSICAO);

                    sb.Append("                                </td>");
                    sb.Append("                                <td>");
                    sb.Append("                                    &nbsp;");
                    sb.Append("                                </td>");
                    sb.Append("                            </tr>");
                }
            }
            sb.Append("                            <tr>");
            sb.Append("                                <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                             <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Grade:");
            sb.Append("                                </td>");
            sb.Append("                                 <td colspan='2'>");
            sb.Append("                                     <table cellpadding='0' cellspacing='0' style='width: 400pt; padding: 0px; color: black;");
            sb.Append("                                         font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
            sb.Append("                                         border: 1px solid #ccc;'>");
            sb.Append("                                         <tr style='background-color: #ccc;'>");
            sb.Append("                                             <td>");
            sb.Append("                                                 &nbsp;");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  &nbsp;");
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");

            var _grade = prodController.ObterGradeHB(hb.CODIGO, 3);
            sb.Append("                                         <tr>");
            sb.Append("                                             <td>");
            sb.Append("                                                 " + prodController.ObterCoresBasicas(hb.COR.Trim()).DESC_COR.Trim());
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_EXP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_XP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_PP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_P)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_M)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_G)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_GG)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : (_grade.GRADE_EXP + _grade.GRADE_XP + _grade.GRADE_PP + _grade.GRADE_P + _grade.GRADE_M + _grade.GRADE_G + _grade.GRADE_GG))));
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");
            sb.Append("                                     </table>");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string mat = "";
            var material = desenvController.ObterMaterial(hidMaterial.Value);
            if (material != null)
                mat = "TECIDO: " + material.GRUPO + " / " + material.SUBGRUPO + " - FORNECEDOR: " + material.FABRICANTE;

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Lavagem:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                     " + mat);
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
            sb.Append("                             <tr>");
            sb.Append("                                 <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
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
