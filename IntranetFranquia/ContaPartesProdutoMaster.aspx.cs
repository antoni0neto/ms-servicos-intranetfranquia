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
    public partial class ContaPartesProdutoMaster : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaGridViewKitNatal();
            }
        }

        private void CarregaGridViewKitNatal()
        {
            List<PartesProdutoMaster> lppm = new List<PartesProdutoMaster>();
            PartesProdutoMaster ppm = null;

            List<string> datas = new List<string>();

            datas.Add("20121123");
            datas.Add("20121124");
            datas.Add("20121125");
            datas.Add("20121126");
            datas.Add("20121127");
            datas.Add("20121128");
            datas.Add("20121129");
            datas.Add("20121130");
            datas.Add("20121201");
            datas.Add("20121202");
            datas.Add("20121203");
            datas.Add("20121204");
            datas.Add("20121205");
            datas.Add("20121206");
            datas.Add("20121207");
            datas.Add("20121208");
            datas.Add("20121209");
            datas.Add("20121210");
            datas.Add("20121211");
            datas.Add("20121212");
            datas.Add("20121213");
            datas.Add("20121214");
            datas.Add("20121215");
            datas.Add("20121216");
            datas.Add("20121217");
            datas.Add("20121218");
            datas.Add("20121219");
            datas.Add("20121220");
            datas.Add("20121221");
            datas.Add("20121222");
            datas.Add("20121223");
            datas.Add("20121224");
            datas.Add("20121225");
            datas.Add("20121226");
            datas.Add("20121227");
            datas.Add("20121228");
            datas.Add("20121229");
            datas.Add("20121230");

            List<USUARIO> usuarios = baseController.BuscaUsuarioPerfil(3);//Supervisor

            if (usuarios != null)
            {
                foreach (USUARIO usuario in usuarios)
                {
                    List<FILIAI> filiais = baseController.BuscaFiliais(usuario);

                    if (filiais != null)
                    {
                        int contaLinha = 0;
                        decimal contaPerc = 0;

                        foreach (FILIAI filial in filiais)
                        {
                            ppm = new PartesProdutoMaster();

                            foreach (string data in datas)
                            {
                                int contaKit = 0;

                                List<Sp_Busca_TicketsResult> listaVendas = baseController.BuscaTicketsLoja(data, filial.COD_FILIAL);

                                if (listaVendas != null && listaVendas.Count > 0)
                                {
                                    foreach (Sp_Busca_TicketsResult lv in listaVendas)
                                    {
                                        List<LOJA_VENDA_PRODUTO> listaKitNatal = baseController.BuscaKitNatalTicket(filial.COD_FILIAL, lv.ticket);

                                        if (listaKitNatal != null && listaKitNatal.Count > 0)
                                            contaKit++;
                                    }

                                    ppm.Supervisor = usuario.USUARIO1;
                                    ppm.Filial = baseController.BuscaFilialCodigo(Convert.ToInt32(filial.COD_FILIAL)).FILIAL;

                                    if (data.Equals("20121123"))
                                    {
                                        ppm.Perc_23_11 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100; 
                                    }
                                    if (data.Equals("20121124"))
                                    {
                                        ppm.Perc_24_11 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121125"))
                                    {
                                        ppm.Perc_25_11 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121126"))
                                    {
                                        ppm.Perc_26_11 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121127"))
                                    {
                                        ppm.Perc_27_11 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121128"))
                                    {
                                        ppm.Perc_28_11 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121129"))
                                    {
                                        ppm.Perc_29_11 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121130"))
                                    {
                                        ppm.Perc_30_11 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121201"))
                                    {
                                        ppm.Perc_01_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121202"))
                                    {
                                        ppm.Perc_02_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121203"))
                                    {
                                        ppm.Perc_03_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121204"))
                                    {
                                        ppm.Perc_04_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121205"))
                                    {
                                        ppm.Perc_05_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121206"))
                                    {
                                        ppm.Perc_06_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121207"))
                                    {
                                        ppm.Perc_07_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121208"))
                                    {
                                        ppm.Perc_08_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121209"))
                                    {
                                        ppm.Perc_09_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121210"))
                                    {
                                        ppm.Perc_10_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121211"))
                                    {
                                        ppm.Perc_11_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121212"))
                                    {
                                        ppm.Perc_12_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121213"))
                                    {
                                        ppm.Perc_13_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121214"))
                                    {
                                        ppm.Perc_14_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121215"))
                                    {
                                        ppm.Perc_15_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121216"))
                                    {
                                        ppm.Perc_16_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121217"))
                                    {
                                        ppm.Perc_17_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121218"))
                                    {
                                        ppm.Perc_18_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121219"))
                                    {
                                        ppm.Perc_19_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121220"))
                                    {
                                        ppm.Perc_20_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121221"))
                                    {
                                        ppm.Perc_21_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121222"))
                                    {
                                        ppm.Perc_22_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121223"))
                                    {
                                        ppm.Perc_23_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                    if (data.Equals("20121224"))
                                    {
                                        ppm.Perc_24_12 = ((Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100).ToString("N2") + "%";
                                        contaLinha++;
                                        contaPerc += (Convert.ToDecimal(contaKit) / Convert.ToDecimal(listaVendas.Count)) * 100;
                                    }
                                }
                            }

                            ppm.Perc_total = (contaPerc / Convert.ToDecimal(contaLinha)).ToString("N2") + "%";

                            lppm.Add(ppm);
                        }
                    }
                }

                GridViewKitNatal.DataSource = lppm;
                GridViewKitNatal.DataBind();
            }
        }
    }
}
