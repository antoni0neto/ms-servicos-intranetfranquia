using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Relatorios.mod_ecom.mag;


namespace Relatorios
{
    public partial class ecom_dev_estoque : System.Web.UI.Page
    {
        EcomController eController = new EcomController();

        string headerTam1 = "";
        string headerTam2 = "";
        string headerTam3 = "";
        string headerTam4 = "";
        string headerTam5 = "";
        string headerTam6 = "";
        string headerTam7 = "";
        string headerTam8 = "";
        string headerTam9 = "";
        string headerTam10 = "";
        string headerTam11 = "";
        string headerTam12 = "";
        string headerTam13 = "";
        string headerTam14 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {
                CarregarProdutoDevolvidos();
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_ECOM_PRODUTO_DEVOLUCAOResult> ObterProdutoLinxDevolucao()
        {
            var produtos = eController.ObterProdutoLinxDevolucao(null, "").AsQueryable();

            if (txtDataIni.Text.Trim() != "")
                produtos = produtos.Where(x => x.RECEBIMENTO >= Convert.ToDateTime(txtDataIni.Text));

            if (txtDataFim.Text.Trim() != "")
                produtos = produtos.Where(x => x.RECEBIMENTO <= Convert.ToDateTime(txtDataFim.Text));

            return produtos.ToList();
        }
        private void CarregarProdutoDevolvidos()
        {
            gvProduto.DataSource = ObterProdutoLinxDevolucao();
            gvProduto.DataBind();
        }

        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PRODUTO_DEVOLUCAOResult dev = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_DEVOLUCAOResult;

                    System.Web.UI.WebControls.Image _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                    _imgProduto.ImageUrl = (dev.FOTO_FRENTE == null) ? "" : dev.FOTO_FRENTE;

                    Literal _litRecebimento = e.Row.FindControl("litRecebimento") as Literal;
                    if (_litRecebimento != null)
                        _litRecebimento.Text = dev.RECEBIMENTO.ToString("dd/MM/yyyy");

                    Button _btDevolverDef = e.Row.FindControl("btDevolverDef") as Button;
                    Button _btDevolver = e.Row.FindControl("btDevolver") as Button;
                    _btDevolver.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(_btDevolver, null) + ";");
                    _btDevolver.CommandArgument = dev.CODIGO.ToString() + "|" + dev.NF_ENTRADA.Trim();

                    _btDevolverDef.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(_btDevolverDef, null) + ";");
                    _btDevolverDef.CommandArgument = dev.CODIGO.ToString() + "|" + dev.NF_ENTRADA.Trim();

                    if (!headerTam1.Contains(dev.TAMANHO_1))
                        headerTam1 = headerTam1 + dev.TAMANHO_1 + "-";
                    if (!headerTam2.Contains(dev.TAMANHO_2))
                        headerTam2 = headerTam2 + dev.TAMANHO_2 + "-";
                    if (!headerTam3.Contains(dev.TAMANHO_3))
                        headerTam3 = headerTam3 + dev.TAMANHO_3 + "-";
                    if (!headerTam4.Contains(dev.TAMANHO_4))
                        headerTam4 = headerTam4 + dev.TAMANHO_4 + "-";
                    if (!headerTam5.Contains(dev.TAMANHO_5))
                        headerTam5 = headerTam5 + dev.TAMANHO_5 + "-";
                    if (!headerTam6.Contains(dev.TAMANHO_6))
                        headerTam6 = headerTam6 + dev.TAMANHO_6 + "-";
                    if (!headerTam7.Contains(dev.TAMANHO_7))
                        headerTam7 = headerTam7 + dev.TAMANHO_7 + "-";
                    if (!headerTam8.Contains(dev.TAMANHO_8))
                        headerTam8 = headerTam8 + dev.TAMANHO_8 + "-";
                    if (!headerTam9.Contains(dev.TAMANHO_9))
                        headerTam9 = headerTam9 + dev.TAMANHO_9 + "-";
                    if (!headerTam10.Contains(dev.TAMANHO_10))
                        headerTam10 = headerTam10 + dev.TAMANHO_10 + "-";
                    if (!headerTam11.Contains(dev.TAMANHO_11))
                        headerTam11 = headerTam11 + dev.TAMANHO_11 + "-";
                    if (!headerTam12.Contains(dev.TAMANHO_12))
                        headerTam12 = headerTam7 + dev.TAMANHO_12 + "-";
                    if (!headerTam13.Contains(dev.TAMANHO_13))
                        headerTam13 = headerTam13 + dev.TAMANHO_13 + "-";
                    if (!headerTam14.Contains(dev.TAMANHO_14))
                        headerTam14 = headerTam7 + dev.TAMANHO_14 + "-";


                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }


        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GerarHeaderTamanho();
        }
        private void GerarHeaderTamanho()
        {
            GridViewRow _header = gvProduto.HeaderRow;
            if (_header != null)
            {
                headerTam1 = headerTam1 + "-";
                headerTam1 = headerTam1.Replace("--", "");
                headerTam2 = headerTam2 + "-";
                headerTam2 = headerTam2.Replace("--", "");
                headerTam3 = headerTam3 + "-";
                headerTam3 = headerTam3.Replace("--", "");
                headerTam4 = headerTam4 + "-";
                headerTam4 = headerTam4.Replace("--", "");
                headerTam5 = headerTam5 + "-";
                headerTam5 = headerTam5.Replace("--", "");
                headerTam6 = headerTam6 + "-";
                headerTam6 = headerTam6.Replace("--", "");
                headerTam7 = headerTam7 + "-";
                headerTam7 = headerTam7.Replace("--", "");
                headerTam8 = headerTam8 + "-";
                headerTam8 = headerTam8.Replace("--", "");
                headerTam9 = headerTam9 + "-";
                headerTam9 = headerTam9.Replace("--", "");
                headerTam10 = headerTam10 + "-";
                headerTam10 = headerTam10.Replace("--", "");
                headerTam11 = headerTam11 + "-";
                headerTam11 = headerTam11.Replace("--", "");
                headerTam12 = headerTam12 + "-";
                headerTam12 = headerTam12.Replace("--", "");
                headerTam13 = headerTam13 + "-";
                headerTam13 = headerTam13.Replace("--", "");
                headerTam14 = headerTam14 + "-";
                headerTam14 = headerTam14.Replace("--", "");

                _header.Cells[7].Text = headerTam1 ?? "-";
                _header.Cells[8].Text = headerTam2 ?? "-";
                _header.Cells[9].Text = headerTam3 ?? "-";
                _header.Cells[10].Text = headerTam4 ?? "-";
                _header.Cells[11].Text = headerTam5 ?? "-";
                _header.Cells[12].Text = headerTam6 ?? "-";
                _header.Cells[13].Text = headerTam7 ?? "-";
                _header.Cells[14].Text = headerTam8 ?? "-";
                _header.Cells[15].Text = headerTam9 ?? "-";
                _header.Cells[16].Text = headerTam10 ?? "-";
                _header.Cells[17].Text = headerTam11 ?? "-";
                _header.Cells[18].Text = headerTam12 ?? "-";
                _header.Cells[19].Text = headerTam13 ?? "-";
                _header.Cells[20].Text = headerTam14 ?? "-";
            }
        }

        protected void btDevolver_Click(object sender, EventArgs e)
        {

            try
            {
                Button bt = (Button)sender;
                if (bt != null)
                {
                    var vals = bt.CommandArgument.Split('|');

                    int codigo = Convert.ToInt32(vals[0]);
                    var nfEntrada = vals[1].ToString();

                    var dev = eController.ObterProdutoLinxDevolucao(codigo, nfEntrada).FirstOrDefault();
                    if (dev != null)
                    {
                        USUARIO usuario = (USUARIO)Session["USUARIO"];
                        string mapPath = Server.MapPath("..");

                        Magento mag = new Magento(usuario.CODIGO_USUARIO, mapPath);
                        mag.SalvarProdutoMagentoDevolucao(codigo, nfEntrada);
                    }
                }

                CarregarProdutoDevolvidos();
            }
            catch (Exception ex)
            {
            }
        }
        protected void btDevolverDef_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                if (bt != null)
                {
                    var vals = bt.CommandArgument.Split('|');

                    int codigo = Convert.ToInt32(vals[0]);
                    var nfEntrada = vals[1].ToString();

                    var dev = eController.ObterProdutoLinxDevolucao(codigo, nfEntrada).FirstOrDefault();
                    if (dev != null)
                    {
                        USUARIO usuario = (USUARIO)Session["USUARIO"];

                        //inserir devolucao estoque deste produto
                        var devIntra = new ECOM_ESTOQUE_DEV();
                        devIntra.PRODUTO = dev.PRODUTO;
                        devIntra.COR = dev.COR;
                        devIntra.NOME_CLIFOR = dev.NOME_CLIFOR;
                        devIntra.FILIAL = dev.FILIAL;
                        devIntra.NF_ENTRADA = dev.NF_ENTRADA;
                        devIntra.SERIE_NF_ENTRADA = dev.SERIE_NF_ENTRADA;
                        devIntra.RECEBIMENTO = dev.RECEBIMENTO;
                        devIntra.DATA_ENTRADA = DateTime.Now;
                        devIntra.USUARIO_ENTRADA = usuario.CODIGO_USUARIO;
                        eController.InserirMagentoEstoqueDevolvido(devIntra);
                    }
                }

                CarregarProdutoDevolvidos();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarProdutoDevolvidos();
            }
            catch (Exception)
            {
            }
        }

    }
}

