using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.Text;

namespace Relatorios
{
    public partial class estoque_mercadoria_conferencia_detalhe : System.Web.UI.Page
    {
        EstoqueController estoqueController = new EstoqueController();
        BaseController baseController = new BaseController();
        int colunaRecebido = 0;
        int colunaEmTransito = 0;
        int colunaOK = 0;

        List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult> g_produtoRecebidoLoja = new List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "")
                {
                    // TO DO
                }
                else
                {
                    string codigo = Request.QueryString["p"].ToString();
                    hidProduto.Value = codigo;

                    if (hidProduto.Value.Trim() != "")
                    {
                        CarregarFilial();
                        CarregarGrids(codigo, "");

                        var produto = new BaseController().BuscaProduto(codigo.Trim());
                        if (produto != null)
                            labProduto.Text = produto.PRODUTO1.Trim() + " - " + produto.DESC_PRODUTO;
                    }
                }
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = baseController.BuscaFiliais(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").ToList();
                //lstFilial = baseController.BuscaFiliais();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                        ddlFilial.SelectedIndex = 1;
                }
            }
        }
        private List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult> ObterProdutoRecebido(string codigoProduto)
        {
            return estoqueController.ObterProdutoRecebidoLojaNota(codigoProduto, "");
        }
        #endregion

        private void CarregarGrids(string codigoProduto, string filial)
        {
            g_produtoRecebidoLoja = ObterProdutoRecebido(codigoProduto);

            if (filial.Trim() != "")
                g_produtoRecebidoLoja = g_produtoRecebidoLoja.Where(p => p.FILIAL.Trim() == filial).ToList();

            //Bind nos produtos recebidos com divergencia
            gvProdutoRecebido.DataSource = g_produtoRecebidoLoja.Where(p => p.CODIGO > 0 && (p.STATUS != "OK" && p.STATUS_CONFERENCIA != 'F'));
            gvProdutoRecebido.DataBind();

            //Bind nos produtos em transito
            gvProdutoEmTransito.DataSource = g_produtoRecebidoLoja.Where(p => p.CODIGO == 0);
            gvProdutoEmTransito.DataBind();

            //Bind nos produtos recebidos com OK ou Solucionados
            gvProdutoRecebidoOK.DataSource = g_produtoRecebidoLoja.Where(p => p.CODIGO > 0 && (p.STATUS == "OK" || p.STATUS_CONFERENCIA == 'F'));
            gvProdutoRecebidoOK.DataBind();
        }

        #region "FILTROS"
        protected void btFiltrar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            try
            {
                CarregarGrids(hidProduto.Value, ddlFilial.SelectedItem.Text.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion

        #region "PRODUTO RECEBIDO"
        protected void gvProdutoRecebido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult _produtoRecebido = e.Row.DataItem as SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult;

                    colunaRecebido += 1;
                    if (_produtoRecebido != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaRecebido.ToString();

                        DropDownList _ddlSolucao = e.Row.FindControl("ddlSolucao") as DropDownList;
                        if (_ddlSolucao != null)
                            _ddlSolucao.SelectedValue = _produtoRecebido.STATUS_CONFERENCIA.ToString();

                        GridView gvNotaMercadoriaRecebida = e.Row.FindControl("gvNotaMercadoriaRecebida") as GridView;
                        if (gvNotaMercadoriaRecebida != null)
                        {
                            List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult> _detalheNota = new List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult>();
                            //Obter grade recebida
                            _detalheNota = g_produtoRecebidoLoja.Where(p => p.CODIGO == _produtoRecebido.CODIGO).ToList();

                            var gradeNota = estoqueController.ObterGradeLojaEntradaLinha(_produtoRecebido.PRODUTO.Trim(), _produtoRecebido.COR_PRODUTO.Trim(), _produtoRecebido.FILIAL.Trim(), _produtoRecebido.ROMANEIO_PRODUTO.Trim());
                            if (gradeNota != null)
                            {
                                //Adicionar grade real da nota
                                _detalheNota.Add(new SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult
                                {
                                    NUMERO_NF_TRANSFERENCIA = "",
                                    SERIE_NF_ENTRADA = "",
                                    //EMISSAO = null,
                                    EXP_RECEBIDO = Convert.ToInt32(gradeNota.EXP_RECEBIDO),
                                    XP_RECEBIDO = Convert.ToInt32(gradeNota.XP_RECEBIDO),
                                    PP_RECEBIDO = Convert.ToInt32(gradeNota.PP_RECEBIDO),
                                    P_RECEBIDO = Convert.ToInt32(gradeNota.P_RECEBIDO),
                                    M_RECEBIDO = Convert.ToInt32(gradeNota.M_RECEBIDO),
                                    G_RECEBIDO = Convert.ToInt32(gradeNota.G_RECEBIDO),
                                    GG_RECEBIDO = Convert.ToInt32(gradeNota.GG_RECEBIDO)
                                });
                            }

                            gvNotaMercadoriaRecebida.DataSource = _detalheNota;
                            gvNotaMercadoriaRecebida.DataBind();
                        }
                    }
                }
            }
        }
        protected void gvNotaMercadoriaRecebida_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult _notaMercadoria = e.Row.DataItem as SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult;

                    if (_notaMercadoria != null)
                    {

                        Literal _litEmissao = e.Row.FindControl("litEmissao") as Literal;
                        if (_litEmissao != null)
                            if (_notaMercadoria.NUMERO_NF_TRANSFERENCIA.Trim() != "")
                                _litEmissao.Text = _notaMercadoria.EMISSAO.ToString("dd/MM/yyyy");

                        Literal _litTipoNota = e.Row.FindControl("litTipoNota") as Literal;
                        if (_litTipoNota != null)
                        {
                            if (_notaMercadoria.NUMERO_NF_TRANSFERENCIA.Trim() != "")
                                _litTipoNota.Text = "RECEBIDA";
                            else
                                _litTipoNota.Text = "NOTA";
                        }
                    }
                }
            }
        }
        protected void ddlSolucao_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl != null)
            {
                try
                {
                    GridViewRow row = (GridViewRow)ddl.NamingContainer;
                    if (row != null)
                    {
                        int codigo = Convert.ToInt32(gvProdutoRecebido.DataKeys[row.RowIndex].Value);

                        if (codigo > 0)
                        {
                            ESTOQUE_LOJA_NF_RECEB_PRODUTO produto = estoqueController.ObterEstoqueLojaRecebProduto(codigo);
                            if (produto != null)
                            {
                                produto.STATUS_CONFERENCIA = (ddl.SelectedValue == "") ? ' ' : Convert.ToChar(ddl.SelectedValue);
                                estoqueController.AtualizarEstoqueLojaRecebProduto(produto);

                                if (ddl.SelectedValue == "L")
                                    GerarDadosEmail(row, ddl.SelectedItem.Text.Trim());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected void btConferir_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if (bt != null)
            {
                try
                {
                    GridViewRow row = (GridViewRow)bt.NamingContainer;
                    if (row != null)
                    {
                        int codigo = Convert.ToInt32(gvProdutoRecebido.DataKeys[row.RowIndex].Value);

                        if (codigo > 0)
                        {

                            ESTOQUE_LOJA_NF_RECEB_PRODUTO produto = estoqueController.ObterEstoqueLojaRecebProduto(codigo);
                            if (produto != null)
                            {
                                produto.STATUS_CONFERENCIA = 'F';
                                TextBox txt = row.FindControl("txtObservacao") as TextBox;
                                produto.OBSERVACAO = (txt == null) ? "" : txt.Text.Trim();
                                estoqueController.AtualizarEstoqueLojaRecebProduto(produto);
                            }

                            CarregarGrids(hidProduto.Value, ddlFilial.SelectedItem.Text.Trim());

                            GerarDadosEmail(row, "CONCLUÍDO");
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void GerarDadosEmail(GridViewRow row, string status)
        {
            //Obter grid FILHO
            GridView gvNotaMercadoriaRecebida = row.FindControl("gvNotaMercadoriaRecebida") as GridView;
            List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult> lojaNotaAuxiliar = new List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult>();

            if (gvNotaMercadoriaRecebida != null)
            {
                //LISTA UTILIZADA PARA AUXILIO DO CORPO DE EMAIL
                SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult lojaNota = null;
                foreach (GridViewRow rowChild in gvNotaMercadoriaRecebida.Rows)
                {
                    lojaNota = new SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult();
                    lojaNota.NUMERO_NF_TRANSFERENCIA = rowChild.Cells[1].Text.Trim();
                    Literal _litEmissao = rowChild.FindControl("litEmissao") as Literal;
                    if (_litEmissao != null && _litEmissao.Text.Trim() != "")
                        lojaNota.EMISSAO = Convert.ToDateTime(_litEmissao.Text.Trim());
                    lojaNota.SERIE_NF_ENTRADA = rowChild.Cells[3].Text.Trim();
                    lojaNota.EXP_RECEBIDO = Convert.ToInt32(rowChild.Cells[5].Text.Trim());
                    lojaNota.XP_RECEBIDO = Convert.ToInt32(rowChild.Cells[6].Text.Trim());
                    lojaNota.PP_RECEBIDO = Convert.ToInt32(rowChild.Cells[7].Text.Trim());
                    lojaNota.P_RECEBIDO = Convert.ToInt32(rowChild.Cells[8].Text.Trim());
                    lojaNota.M_RECEBIDO = Convert.ToInt32(rowChild.Cells[9].Text.Trim());
                    lojaNota.G_RECEBIDO = Convert.ToInt32(rowChild.Cells[10].Text.Trim());
                    lojaNota.GG_RECEBIDO = Convert.ToInt32(rowChild.Cells[11].Text.Trim());

                    lojaNotaAuxiliar.Add(lojaNota);
                }
            }

            TextBox _txtObservacao = row.FindControl("txtObservacao") as TextBox;
            var _filial = baseController.BuscaFilial(row.Cells[2].Text.Trim());
            EnviarEmail(status, _txtObservacao.Text, _filial, lojaNotaAuxiliar);
        }
        private void EnviarEmail(string status, string obs, FILIAI _filial, List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult> lojaNotaAuxiliar)
        {
            string msg = "";
            email_envio email = new email_envio();
            email.ASSUNTO = "Mercadoria Conferida - " + _filial.FILIAL.Trim() + " - NF: " + lojaNotaAuxiliar[0].NUMERO_NF_TRANSFERENCIA;
            email.REMETENTE = (USUARIO)Session["USUARIO"];

            List<string> destinatario = new List<string>();
            List<USUARIO> usuarioDestinatario = new List<USUARIO>();
            msg = obs;
            usuarioDestinatario = new UsuarioController().ObterEmailUsuarioTela(1, 2).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (USUARIO usu in usuarioDestinatario)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);

            //OBTER EMAIL DO SUPERVISOR DA LOJA
            var _supervisorId = baseController.BuscaSupervisorLoja(_filial.COD_FILIAL);
            if (_supervisorId != null)
            {
                var _supervisorUsuario = baseController.BuscaUsuario(_supervisorId.codigo_usuario);
                if (_supervisorUsuario != null)
                    destinatario.Add(_supervisorUsuario.EMAIL);
            }

            //OBTER EMAIL DA LOJA
            var _filialIntranet = baseController.ObterFilialIntranet(_filial.COD_FILIAL);
            if (_filialIntranet != null)
                destinatario.Add(_filialIntranet.email);

            email.MENSAGEM = MontarCorpoEmail(status, msg, _filial.FILIAL.Trim(), lojaNotaAuxiliar);
            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(string status, string msg, string filial, List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult> lojaNotaAuxiliar)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            sb.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>NOTA MERCADORIA</title>");
            sb.Append("    <meta charset='UTF-8'>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divProduto' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                         <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            sb.Append("                         color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                         background: white; white-space: nowrap;'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 150px;'>");
            sb.Append("                                    Filial:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                " + filial);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    NF Transferência:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                   " + lojaNotaAuxiliar[0].NUMERO_NF_TRANSFERENCIA);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Série:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                  " + lojaNotaAuxiliar[0].SERIE_NF_ENTRADA);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Emissão:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                   " + lojaNotaAuxiliar[0].EMISSAO.ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Status:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                   " + status);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Observação:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + msg.ToUpper());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td colspan='2'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Produto:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + labProduto.Text);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        <table cellpadding='0' cellspacing='0' style='width: 600pt; padding: 0px; color: black;");
            sb.Append("                            font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
            sb.Append("                            border: 1px solid #ccc;'>");
            sb.Append("                            <tr style='background-color: #ccc;'>");
            sb.Append("                                <td>");
            sb.Append("                                    Grade");
            sb.Append("                                </td>");
            sb.Append("                                <td style='text-align: center;'>");
            sb.Append("                                    EXP");
            sb.Append("                                </td>");
            sb.Append("                                <td style='text-align: center;'>");
            sb.Append("                                    XP");
            sb.Append("                                </td>");
            sb.Append("                                <td style='text-align: center;'>");
            sb.Append("                                    PP");
            sb.Append("                                </td>");
            sb.Append("                                <td style='text-align: center;'>");
            sb.Append("                                    P");
            sb.Append("                                </td>");
            sb.Append("                                <td style='text-align: center;'>");
            sb.Append("                                    M");
            sb.Append("                                </td>");
            sb.Append("                                <td style='text-align: center;'>");
            sb.Append("                                    G");
            sb.Append("                                </td>");
            sb.Append("                                <td style='text-align: center;'>");
            sb.Append("                                    GG");
            sb.Append("                                </td>");
            sb.Append("                                <td style='text-align: center;'>");
            sb.Append("                                    Total");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            foreach (SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult lna in lojaNotaAuxiliar)
            {
                int total = lna.EXP_RECEBIDO + lna.XP_RECEBIDO + lna.PP_RECEBIDO + lna.P_RECEBIDO + lna.M_RECEBIDO + lna.G_RECEBIDO + lna.GG_RECEBIDO;

                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    " + ((lna.NUMERO_NF_TRANSFERENCIA == "" || lna.NUMERO_NF_TRANSFERENCIA.Contains("nbsp;")) ? "NOTA" : "RECEBIDA"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + lna.EXP_RECEBIDO.ToString());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + lna.XP_RECEBIDO.ToString());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + lna.PP_RECEBIDO.ToString());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + lna.P_RECEBIDO.ToString());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + lna.M_RECEBIDO.ToString());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + lna.G_RECEBIDO.ToString());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + lna.GG_RECEBIDO.ToString());
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 85px;'>");
                sb.Append("                                    " + total.ToString());
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Conferido por: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO.ToUpper() + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }

        #endregion

        #region "PRODUTO EM TRANSITO"
        protected void gvProdutoEmTransito_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult _produtoEmTransito = e.Row.DataItem as SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult;

                    colunaEmTransito += 1;
                    if (_produtoEmTransito != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaEmTransito.ToString();

                    }
                }
            }
        }
        #endregion

        #region "PRODUTO OK"
        protected void gvProdutoRecebidoOK_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string status_conferencia = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult _produtoOK = e.Row.DataItem as SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult;

                    colunaOK += 1;
                    if (_produtoOK != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaOK.ToString();

                        Literal _litSolucao = e.Row.FindControl("litSolucao") as Literal;
                        if (_litSolucao != null)
                        {
                            if (_produtoOK.STATUS_CONFERENCIA == 'E')
                                status_conferencia = "EM ANÁLISE";
                            else if (_produtoOK.STATUS_CONFERENCIA == 'F')
                                status_conferencia = "FINALIZADO";
                            else
                                status_conferencia = "";
                            _litSolucao.Text = status_conferencia;
                        }
                    }
                }
            }
        }
        #endregion


    }
}


