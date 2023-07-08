using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class ContaPartesProdutoSuper : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListSupervisor();
            }
        }

        private void CarregaDropDownListSupervisor()
        {
            ddlSupervisor.DataSource = baseController.BuscaUsuarioPerfil(3);
            ddlSupervisor.DataBind();
        }

        private void CarregaGridViewProdutos()
        {
            USUARIO usuario = baseController.BuscaUsuario(Convert.ToInt32(ddlSupervisor.SelectedValue));

            List<FILIAI> filiais = baseController.BuscaFiliais(usuario);

            if (filiais != null)
            {
                List<PartesProduto> lpp = new List<PartesProduto>();

                foreach (FILIAI filial in filiais)
                {
                    List<Sp_Partes_ProdutoResult> listaPartesProduto = baseController.BuscaPartesProduto(filial.COD_FILIAL, 
                                                                                                         baseController.AjustaData(TextBoxDataInicio.Text), 
                                                                                                         baseController.AjustaData(TextBoxDataFim.Text));

                    Sp_Partes_ProdutoResult itemAnterior = new Sp_Partes_ProdutoResult();

                    int contCima = 0;
                    int contBaixo = 0;
                    int contVestido = 0;
                    int contKitNatal = 0;
                    int contTicket = 0;
                    int contBaixo_1 = 0;
                    int contBaixo_2 = 0;
                    int contBaixo_3 = 0;
                    int contCima_1 = 0;
                    int contCima_2 = 0;
                    int contCima_3 = 0;
                    int totKitNatal = 0;

                    if (listaPartesProduto != null)
                    {
                        foreach (Sp_Partes_ProdutoResult item in listaPartesProduto)
                        {
                            if (item.ticket.Equals(itemAnterior.ticket) || itemAnterior == null)
                            {
                                if (item.grupo_produto.Equals("BAIXO") && item.acessorios.Equals("PEÇAS"))
                                    contBaixo += Convert.ToInt32(item.qtde);
                                if (item.grupo_produto.Equals("CIMA") && item.acessorios.Equals("PEÇAS"))
                                    contCima += Convert.ToInt32(item.qtde);
                                if (item.grupo_produto.Equals("VESTIDO") && item.acessorios.Equals("PEÇAS"))
                                    contVestido += Convert.ToInt32(item.qtde);
                                if (item.grupo_produto.Equals("KIT NATAL") && item.acessorios.Equals("PEÇAS"))
                                {
                                    contKitNatal += Convert.ToInt32(item.qtde);

                                    totKitNatal++;
                                }
                                if (itemAnterior == null)
                                {
                                    itemAnterior = item;

                                    contTicket++;
                                }
                            }
                            else
                            {
                                contTicket++;

                                if (contCima > (contBaixo + contVestido))
                                {
                                    if ((contCima - (contBaixo + contVestido)) == 1)
                                        contBaixo_1 += contCima - (contBaixo + contVestido);
                                    if ((contCima - (contBaixo + contVestido)) == 2)
                                        contBaixo_2 += contCima - (contBaixo + contVestido);
                                    if ((contCima - (contBaixo + contVestido)) > 2)
                                        contBaixo_3 += contCima - (contBaixo + contVestido);
                                }

                                if (contBaixo > (contCima + contVestido))
                                {
                                    if ((contBaixo - (contCima + contVestido)) == 1)
                                        contCima_1 += contBaixo - (contCima + contVestido);
                                    if ((contBaixo - (contCima + contVestido)) == 2)
                                        contCima_2 += contBaixo - (contCima + contVestido);
                                    if ((contBaixo - (contCima + contVestido)) > 2)
                                        contCima_3 += contBaixo - (contCima + contVestido);
                                }

                                if (contVestido > (contCima + contBaixo))
                                {
                                    if ((contVestido - (contCima + contBaixo)) == 1)
                                        contCima_1 += contVestido - (contCima + contBaixo);
                                    if ((contVestido - (contCima + contBaixo)) == 2)
                                        contCima_2 += contVestido - (contCima + contBaixo);
                                    if ((contVestido - (contCima + contBaixo)) > 2)
                                        contCima_3 += contVestido - (contCima + contBaixo);
                                }

                                contCima = 0;
                                contBaixo = 0;
                                contVestido = 0;
                                contKitNatal = 0;

                                if (item.grupo_produto.Equals("BAIXO") && item.acessorios.Equals("PEÇAS"))
                                    contBaixo += Convert.ToInt32(item.qtde);
                                if (item.grupo_produto.Equals("CIMA") && item.acessorios.Equals("PEÇAS"))
                                    contCima += Convert.ToInt32(item.qtde);
                                if (item.grupo_produto.Equals("VESTIDO") && item.acessorios.Equals("PEÇAS"))
                                    contVestido += Convert.ToInt32(item.qtde);
                                if (item.grupo_produto.Equals("KIT NATAL") && item.acessorios.Equals("PEÇAS"))
                                {
                                    contKitNatal += Convert.ToInt32(item.qtde);

                                    totKitNatal++;
                                }

                                itemAnterior = item;
                            }
                        }

                        PartesProduto pp = new PartesProduto();

                        pp.Filial = baseController.BuscaFilialCodigo(Convert.ToInt32(filial.COD_FILIAL)).FILIAL;
                        pp.Ticket = contTicket.ToString();
                        pp.Kit_natal = totKitNatal.ToString();

                        if (contTicket > 0)
                            pp.Perc_atingido = ((Convert.ToDecimal(totKitNatal) / Convert.ToDecimal(contTicket)) * 100).ToString("N2") + "%";

                        pp.Cima_1 = contCima_1.ToString();
                        pp.Cima_2 = contCima_2.ToString();
                        pp.Cima_3 = contCima_3.ToString();
                        pp.Baixo_1 = contBaixo_1.ToString();
                        pp.Baixo_2 = contBaixo_2.ToString();
                        pp.Baixo_3 = contBaixo_3.ToString();

                        lpp.Add(pp);
                    }
                }

                GridViewProdutos.DataSource = lpp;
                GridViewProdutos.DataBind();
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            if (ddlSupervisor.SelectedValue.ToString().Equals("0") || 
                ddlSupervisor.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewProdutos();
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }
        
        protected void CalendarDataInicio_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime value = new DateTime(2012, 11, 23);

            if (e.Day.Date < value)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
            }
        } 

        protected void GridViewProdutos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewProdutos.PageIndex = e.NewPageIndex;
            CarregaGridViewProdutos();
        }

        protected void ddlSupervisor_DataBound(object sender, EventArgs e)
        {
            ddlSupervisor.Items.Add(new ListItem("Selecione", "0"));
            ddlSupervisor.SelectedValue = "0";
        }
    }
}
