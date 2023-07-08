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
    public partial class ListaMovimentoGrupo : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Sp_Pega_Data_BancoResult dt = baseController.BuscaDataBanco();

                string data = dt.data.Year.ToString() + dt.data.Month.ToString("00") + dt.data.Day.ToString("00");

                Sp_Busca_Semana_454Result bs = baseController.BuscaSemana454(data);

                string semana = bs.ano_semana.ToString();

                if (semana.Substring(4, 2).Equals("01"))
                    semana = (dt.data.Year - 1).ToString() + "52";
                else
                    semana = (bs.ano_semana - 1).ToString();

                List<string> anoSemanasPassada = new List<string>();

                if (Convert.ToInt32(semana.Substring(4, 2)) > 4)
                {
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 4).ToString());
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 3).ToString());
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 2).ToString());
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                    anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                }
                else
                {
                    if (semana.Substring(4, 2).Equals("04"))
                    {
                        anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                        anoSemanasPassada.Add((Convert.ToInt32(semana) - 3).ToString());
                        anoSemanasPassada.Add((Convert.ToInt32(semana) - 2).ToString());
                        anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                        anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                    }
                    if (semana.Substring(4, 2).Equals("03"))
                    {
                        anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "51");
                        anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                        anoSemanasPassada.Add((Convert.ToInt32(semana) - 2).ToString());
                        anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                        anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                    }
                    if (semana.Substring(4, 2).Equals("02"))
                    {
                        anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "50");
                        anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "51");
                        anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                        anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                        anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                    }
                    if (semana.Substring(4, 2).Equals("01"))
                    {
                        anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "49");
                        anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "50");
                        anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "51");
                        anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                        anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                    }
                }

                string[] datasInicio = new string[5];
                string[] datasFim = new string[5];
                string[] textoProduto = new string[6];
                int[] qtdeVenda = new int[6];
                int[] qtdeVenda1 = new int[6];
                int[] qtdeVenda2 = new int[6];
                int[] qtdeVenda3 = new int[6];
                int[] qtdeVenda4 = new int[6];

                int[] qtdeCota1 = new int[6];
                int[] qtdeCota2 = new int[6];
                int[] qtdeCota3 = new int[6];
                int[] qtdeCota4 = new int[6];

                int i = 0;

                foreach (string item in anoSemanasPassada)
                {
                    datasInicio[i] = baseController.AjustaData(baseController.BuscaDataInicio(item));
                    datasFim[i] = baseController.AjustaData(baseController.BuscaDataFim(item));

                    i++;
                }

                List<Grupo> listGrupoFem = new List<Grupo>();

                Grupo itemGrupo = null;

                List<PRODUTO_GRUPO_PRODUTO> gpFem = baseController.BuscaQtdeGrupoProduto("FEMININO");

                List<COTAS_SEMANA_GRUPO> csg = baseController.BuscaCotasSemanaGrupo();

                string lojaAnterior = "";

                FILIAI filial = null;

                foreach (COTAS_SEMANA_GRUPO itemCota in csg)
                {
                    if (!itemCota.LOJA.Equals(lojaAnterior))
                    {
                        itemGrupo = new Grupo();

                        filial = baseController.BuscaFilial(itemCota.LOJA);

                        if (filial != null)
                        {
                            lojaAnterior = itemCota.LOJA;

                            itemGrupo.Loja = itemCota.LOJA;
                            itemGrupo.Cota = itemCota.COTA.ToString() + "%";

                            int y = 0;

                            foreach (PRODUTO_GRUPO_PRODUTO grupoProduto in gpFem)
                            {
                                Sp_Vendas_PeriodoResult vp = baseController.BuscaVendasPeriodo("01", "15", grupoProduto.GRIFFE, grupoProduto.GRUPO, filial.COD_FILIAL, datasInicio[1], datasFim[1], datasInicio[2], datasFim[2], datasInicio[3], datasFim[3], datasInicio[4], datasFim[4]);

                                qtdeVenda1[y] = Convert.ToInt32(vp.qt_venda_semana_1);
                                qtdeVenda2[y] = Convert.ToInt32(vp.qt_venda_semana_2);
                                qtdeVenda3[y] = Convert.ToInt32(vp.qt_venda_semana_3);
                                qtdeVenda4[y] = Convert.ToInt32(vp.qt_venda_semana_4);

                                qtdeCota1[y] = Convert.ToInt32(grupoProduto.SEMANA_1);
                                qtdeCota2[y] = Convert.ToInt32(grupoProduto.SEMANA_2);
                                qtdeCota3[y] = Convert.ToInt32(grupoProduto.SEMANA_3);
                                qtdeCota4[y] = Convert.ToInt32(grupoProduto.SEMANA_4);

                                textoProduto[y] = grupoProduto.GRIFFE + " / " + grupoProduto.GRUPO;

                                y++;
                            }

                            itemGrupo.Qtde_prod_1_sem_1 = qtdeVenda1[0];
                            itemGrupo.Qtde_prod_1_sem_2 = qtdeVenda2[0];
                            itemGrupo.Qtde_prod_1_sem_3 = qtdeVenda3[0];
                            itemGrupo.Qtde_prod_1_sem_4 = qtdeVenda4[0];
                            itemGrupo.Qtde_prod_1_total = qtdeVenda1[0] + qtdeVenda2[0] + qtdeVenda3[0] + qtdeVenda4[0];

                            itemGrupo.Qtde_prod_2_sem_1 = qtdeVenda1[1];
                            itemGrupo.Qtde_prod_2_sem_2 = qtdeVenda2[1];
                            itemGrupo.Qtde_prod_2_sem_3 = qtdeVenda3[1];
                            itemGrupo.Qtde_prod_2_sem_4 = qtdeVenda4[1];
                            itemGrupo.Qtde_prod_2_total = qtdeVenda1[1] + qtdeVenda2[1] + qtdeVenda3[1] + qtdeVenda4[1];

                            itemGrupo.Qtde_prod_3_sem_1 = qtdeVenda1[2];
                            itemGrupo.Qtde_prod_3_sem_2 = qtdeVenda2[2];
                            itemGrupo.Qtde_prod_3_sem_3 = qtdeVenda3[2];
                            itemGrupo.Qtde_prod_3_sem_4 = qtdeVenda4[2];
                            itemGrupo.Qtde_prod_3_total = qtdeVenda1[2] + qtdeVenda2[2] + qtdeVenda3[2] + qtdeVenda4[2];

                            itemGrupo.Qtde_prod_4_sem_1 = qtdeVenda1[3];
                            itemGrupo.Qtde_prod_4_sem_2 = qtdeVenda2[3];
                            itemGrupo.Qtde_prod_4_sem_3 = qtdeVenda3[3];
                            itemGrupo.Qtde_prod_4_sem_4 = qtdeVenda4[3];
                            itemGrupo.Qtde_prod_4_total = qtdeVenda1[3] + qtdeVenda2[3] + qtdeVenda3[3] + qtdeVenda4[3];

                            itemGrupo.Qtde_prod_5_sem_1 = qtdeVenda1[4];
                            itemGrupo.Qtde_prod_5_sem_2 = qtdeVenda2[4];
                            itemGrupo.Qtde_prod_5_sem_3 = qtdeVenda3[4];
                            itemGrupo.Qtde_prod_5_sem_4 = qtdeVenda4[4];
                            itemGrupo.Qtde_prod_5_total = qtdeVenda1[4] + qtdeVenda2[4] + qtdeVenda3[4] + qtdeVenda4[4];

                            itemGrupo.Qtde_prod_6_sem_1 = qtdeVenda1[5];
                            itemGrupo.Qtde_prod_6_sem_2 = qtdeVenda2[5];
                            itemGrupo.Qtde_prod_6_sem_3 = qtdeVenda3[5];
                            itemGrupo.Qtde_prod_6_sem_4 = qtdeVenda4[5];
                            itemGrupo.Qtde_prod_6_total = qtdeVenda1[5] + qtdeVenda2[5] + qtdeVenda3[5] + qtdeVenda4[5];

                            listGrupoFem.Add(itemGrupo);
                        }
                    }
                }

                txtProdutoFem_1.Text = textoProduto[0];
                txtProdutoFem_2.Text = textoProduto[1];
                txtProdutoFem_3.Text = textoProduto[2];
                txtProdutoFem_4.Text = textoProduto[3];
                txtProdutoFem_5.Text = textoProduto[4];
                txtProdutoFem_6.Text = textoProduto[5];

                itemGrupo = new Grupo();

                itemGrupo.Loja = "Total";

                itemGrupo.Qtde_prod_1_sem_1 = qtdeCota1[0];
                itemGrupo.Qtde_prod_1_sem_2 = qtdeCota2[0];
                itemGrupo.Qtde_prod_1_sem_3 = qtdeCota3[0];
                itemGrupo.Qtde_prod_1_sem_4 = qtdeCota4[0];
                itemGrupo.Qtde_prod_1_total = qtdeCota1[0] + qtdeCota2[0] + qtdeCota3[0] + qtdeCota4[0];

                itemGrupo.Qtde_prod_2_sem_1 = qtdeCota1[1];
                itemGrupo.Qtde_prod_2_sem_2 = qtdeCota2[1];
                itemGrupo.Qtde_prod_2_sem_3 = qtdeCota3[1];
                itemGrupo.Qtde_prod_2_sem_4 = qtdeCota4[1];
                itemGrupo.Qtde_prod_2_total = qtdeCota1[1] + qtdeCota2[1] + qtdeCota3[1] + qtdeCota4[1];

                itemGrupo.Qtde_prod_3_sem_1 = qtdeCota1[2];
                itemGrupo.Qtde_prod_3_sem_2 = qtdeCota2[2];
                itemGrupo.Qtde_prod_3_sem_3 = qtdeCota3[2];
                itemGrupo.Qtde_prod_3_sem_4 = qtdeCota4[2];
                itemGrupo.Qtde_prod_3_total = qtdeCota1[2] + qtdeCota2[2] + qtdeCota3[2] + qtdeCota4[2];

                itemGrupo.Qtde_prod_4_sem_1 = qtdeCota1[3];
                itemGrupo.Qtde_prod_4_sem_2 = qtdeCota2[3];
                itemGrupo.Qtde_prod_4_sem_3 = qtdeCota3[3];
                itemGrupo.Qtde_prod_4_sem_4 = qtdeCota4[3];
                itemGrupo.Qtde_prod_4_total = qtdeCota1[3] + qtdeCota2[3] + qtdeCota3[3] + qtdeCota4[3];

                itemGrupo.Qtde_prod_5_sem_1 = qtdeCota1[4];
                itemGrupo.Qtde_prod_5_sem_2 = qtdeCota2[4];
                itemGrupo.Qtde_prod_5_sem_3 = qtdeCota3[4];
                itemGrupo.Qtde_prod_5_sem_4 = qtdeCota4[4];
                itemGrupo.Qtde_prod_5_total = qtdeCota1[4] + qtdeCota2[4] + qtdeCota3[4] + qtdeCota4[4];

                itemGrupo.Qtde_prod_6_sem_1 = qtdeCota1[5];
                itemGrupo.Qtde_prod_6_sem_2 = qtdeCota2[5];
                itemGrupo.Qtde_prod_6_sem_3 = qtdeCota3[5];
                itemGrupo.Qtde_prod_6_sem_4 = qtdeCota4[5];
                itemGrupo.Qtde_prod_6_total = qtdeCota1[5] + qtdeCota2[5] + qtdeCota3[5] + qtdeCota4[5];

                listGrupoFem.Add(itemGrupo);

                GridViewGrupoFeminino.DataSource = listGrupoFem;
                GridViewGrupoFeminino.DataBind();

                List<Grupo> listGrupoMas = new List<Grupo>();

                List<PRODUTO_GRUPO_PRODUTO> gpMas = baseController.BuscaQtdeGrupoProduto("MASCULINO");

                lojaAnterior = "";

                foreach (COTAS_SEMANA_GRUPO itemCota in csg)
                {
                    if (!itemCota.LOJA.Equals(lojaAnterior))
                    {
                        itemGrupo = new Grupo();

                        filial = baseController.BuscaFilial(itemCota.LOJA);

                        if (filial != null)
                        {
                            itemGrupo = new Grupo();

                            lojaAnterior = itemCota.LOJA;

                            itemGrupo.Loja = itemCota.LOJA;
                            itemGrupo.Cota = itemCota.COTA.ToString() + "%";

                            int y = 0;

                            string categoria = "";

                            foreach (PRODUTO_GRUPO_PRODUTO grupoProduto in gpMas)
                            {
                                if (grupoProduto.GRUPO.Equals("CINTO"))
                                    categoria = "02";
                                else
                                    categoria = "01";

                                Sp_Vendas_PeriodoResult vp = baseController.BuscaVendasPeriodo(categoria, "15", grupoProduto.GRIFFE, grupoProduto.GRUPO, filial.COD_FILIAL, datasInicio[1], datasFim[1], datasInicio[2], datasFim[2], datasInicio[3], datasFim[3], datasInicio[4], datasFim[4]);

                                qtdeVenda1[y] = Convert.ToInt32(vp.qt_venda_semana_1);
                                qtdeVenda2[y] = Convert.ToInt32(vp.qt_venda_semana_2);
                                qtdeVenda3[y] = Convert.ToInt32(vp.qt_venda_semana_3);
                                qtdeVenda4[y] = Convert.ToInt32(vp.qt_venda_semana_4);

                                qtdeCota1[y] = Convert.ToInt32(grupoProduto.SEMANA_1);
                                qtdeCota2[y] = Convert.ToInt32(grupoProduto.SEMANA_2);
                                qtdeCota3[y] = Convert.ToInt32(grupoProduto.SEMANA_3);
                                qtdeCota4[y] = Convert.ToInt32(grupoProduto.SEMANA_4);

                                textoProduto[y] = grupoProduto.GRIFFE + " / " + grupoProduto.GRUPO;

                                y++;
                            }

                            itemGrupo.Qtde_prod_1_sem_1 = qtdeVenda1[0];
                            itemGrupo.Qtde_prod_1_sem_2 = qtdeVenda2[0];
                            itemGrupo.Qtde_prod_1_sem_3 = qtdeVenda3[0];
                            itemGrupo.Qtde_prod_1_sem_4 = qtdeVenda4[0];
                            itemGrupo.Qtde_prod_1_total = qtdeVenda1[0] + qtdeVenda2[0] + qtdeVenda3[0] + qtdeVenda4[0];

                            itemGrupo.Qtde_prod_2_sem_1 = qtdeVenda1[1];
                            itemGrupo.Qtde_prod_2_sem_2 = qtdeVenda2[1];
                            itemGrupo.Qtde_prod_2_sem_3 = qtdeVenda3[1];
                            itemGrupo.Qtde_prod_2_sem_4 = qtdeVenda4[1];
                            itemGrupo.Qtde_prod_2_total = qtdeVenda1[1] + qtdeVenda2[1] + qtdeVenda3[1] + qtdeVenda4[1];

                            itemGrupo.Qtde_prod_3_sem_1 = qtdeVenda1[2];
                            itemGrupo.Qtde_prod_3_sem_2 = qtdeVenda2[2];
                            itemGrupo.Qtde_prod_3_sem_3 = qtdeVenda3[2];
                            itemGrupo.Qtde_prod_3_sem_4 = qtdeVenda4[2];
                            itemGrupo.Qtde_prod_3_total = qtdeVenda1[2] + qtdeVenda2[2] + qtdeVenda3[2] + qtdeVenda4[2];

                            itemGrupo.Qtde_prod_4_sem_1 = qtdeVenda1[3];
                            itemGrupo.Qtde_prod_4_sem_2 = qtdeVenda2[3];
                            itemGrupo.Qtde_prod_4_sem_3 = qtdeVenda3[3];
                            itemGrupo.Qtde_prod_4_sem_4 = qtdeVenda4[3];
                            itemGrupo.Qtde_prod_4_total = qtdeVenda1[3] + qtdeVenda2[3] + qtdeVenda3[3] + qtdeVenda4[3];

                            listGrupoMas.Add(itemGrupo);
                        }
                    }
                }

                txtProdutoMas_1.Text = textoProduto[0];
                txtProdutoMas_2.Text = textoProduto[1];
                txtProdutoMas_3.Text = textoProduto[2];
                txtProdutoMas_4.Text = textoProduto[3];

                itemGrupo = new Grupo();

                itemGrupo.Loja = "Total";

                itemGrupo.Qtde_prod_1_sem_1 = qtdeCota1[0];
                itemGrupo.Qtde_prod_1_sem_2 = qtdeCota2[0];
                itemGrupo.Qtde_prod_1_sem_3 = qtdeCota3[0];
                itemGrupo.Qtde_prod_1_sem_4 = qtdeCota4[0];
                itemGrupo.Qtde_prod_1_total = qtdeCota1[0] + qtdeCota2[0] + qtdeCota3[0] + qtdeCota4[0];

                itemGrupo.Qtde_prod_2_sem_1 = qtdeCota1[1];
                itemGrupo.Qtde_prod_2_sem_2 = qtdeCota2[1];
                itemGrupo.Qtde_prod_2_sem_3 = qtdeCota3[1];
                itemGrupo.Qtde_prod_2_sem_4 = qtdeCota4[1];
                itemGrupo.Qtde_prod_2_total = qtdeCota1[1] + qtdeCota2[1] + qtdeCota3[1] + qtdeCota4[1];

                itemGrupo.Qtde_prod_3_sem_1 = qtdeCota1[2];
                itemGrupo.Qtde_prod_3_sem_2 = qtdeCota2[2];
                itemGrupo.Qtde_prod_3_sem_3 = qtdeCota3[2];
                itemGrupo.Qtde_prod_3_sem_4 = qtdeCota4[2];
                itemGrupo.Qtde_prod_3_total = qtdeCota1[2] + qtdeCota2[2] + qtdeCota3[2] + qtdeCota4[2];

                itemGrupo.Qtde_prod_4_sem_1 = qtdeCota1[3];
                itemGrupo.Qtde_prod_4_sem_2 = qtdeCota2[3];
                itemGrupo.Qtde_prod_4_sem_3 = qtdeCota3[3];
                itemGrupo.Qtde_prod_4_sem_4 = qtdeCota4[3];
                itemGrupo.Qtde_prod_4_total = qtdeCota1[3] + qtdeCota2[3] + qtdeCota3[3] + qtdeCota4[3];

                listGrupoMas.Add(itemGrupo);

                GridViewGrupoMasculino.DataSource = listGrupoMas;
                GridViewGrupoMasculino.DataBind();
            }
        }

        protected void GridViewGrupoFeminino_DataBound(object sender, EventArgs e)
        {
            int[] qtdeLinhaTotal = new int[30];

            foreach (GridViewRow item in GridViewGrupoFeminino.Rows)
            {
                if (item.Cells[0].Text.Equals("Total"))
                {
                    qtdeLinhaTotal[0] = Convert.ToInt32(item.Cells[2].Text);
                    qtdeLinhaTotal[1] = Convert.ToInt32(item.Cells[3].Text);
                    qtdeLinhaTotal[2] = Convert.ToInt32(item.Cells[4].Text);
                    qtdeLinhaTotal[3] = Convert.ToInt32(item.Cells[5].Text);
                    qtdeLinhaTotal[4] = Convert.ToInt32(item.Cells[6].Text);
                    qtdeLinhaTotal[5] = Convert.ToInt32(item.Cells[7].Text);
                    qtdeLinhaTotal[6] = Convert.ToInt32(item.Cells[8].Text);
                    qtdeLinhaTotal[7] = Convert.ToInt32(item.Cells[9].Text);
                    qtdeLinhaTotal[8] = Convert.ToInt32(item.Cells[10].Text);
                    qtdeLinhaTotal[9] = Convert.ToInt32(item.Cells[11].Text);
                    qtdeLinhaTotal[10] = Convert.ToInt32(item.Cells[12].Text);
                    qtdeLinhaTotal[11] = Convert.ToInt32(item.Cells[13].Text);
                    qtdeLinhaTotal[12] = Convert.ToInt32(item.Cells[14].Text);
                    qtdeLinhaTotal[13] = Convert.ToInt32(item.Cells[15].Text);
                    qtdeLinhaTotal[14] = Convert.ToInt32(item.Cells[16].Text);
                    qtdeLinhaTotal[15] = Convert.ToInt32(item.Cells[17].Text);
                    qtdeLinhaTotal[16] = Convert.ToInt32(item.Cells[18].Text);
                    qtdeLinhaTotal[17] = Convert.ToInt32(item.Cells[19].Text);
                    qtdeLinhaTotal[18] = Convert.ToInt32(item.Cells[20].Text);
                    qtdeLinhaTotal[19] = Convert.ToInt32(item.Cells[21].Text);
                    qtdeLinhaTotal[20] = Convert.ToInt32(item.Cells[22].Text);
                    qtdeLinhaTotal[21] = Convert.ToInt32(item.Cells[23].Text);
                    qtdeLinhaTotal[22] = Convert.ToInt32(item.Cells[24].Text);
                    qtdeLinhaTotal[23] = Convert.ToInt32(item.Cells[25].Text);
                    qtdeLinhaTotal[24] = Convert.ToInt32(item.Cells[26].Text);
                    qtdeLinhaTotal[25] = Convert.ToInt32(item.Cells[27].Text);
                    qtdeLinhaTotal[26] = Convert.ToInt32(item.Cells[28].Text);
                    qtdeLinhaTotal[27] = Convert.ToInt32(item.Cells[29].Text);
                    qtdeLinhaTotal[28] = Convert.ToInt32(item.Cells[30].Text);
                    qtdeLinhaTotal[29] = Convert.ToInt32(item.Cells[31].Text);
                }
            }

            foreach (GridViewRow item in GridViewGrupoFeminino.Rows)
            {
                if (!item.Cells[0].Text.Equals("Total"))
                {
                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[0])) > Convert.ToDouble(item.Cells[2].Text))
                        item.Cells[2].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[2].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[1])) > Convert.ToDouble(item.Cells[3].Text))
                        item.Cells[3].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[3].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[2])) > Convert.ToDouble(item.Cells[4].Text))
                        item.Cells[4].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[4].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[3])) > Convert.ToDouble(item.Cells[5].Text))
                        item.Cells[5].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[5].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[4])) > Convert.ToDouble(item.Cells[6].Text))
                        item.Cells[6].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[6].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[5])) > Convert.ToDouble(item.Cells[7].Text))
                        item.Cells[7].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[7].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[6])) > Convert.ToDouble(item.Cells[8].Text))
                        item.Cells[8].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[8].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[7])) > Convert.ToDouble(item.Cells[9].Text))
                        item.Cells[9].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[9].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[8])) > Convert.ToDouble(item.Cells[10].Text))
                        item.Cells[10].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[10].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[9])) > Convert.ToDouble(item.Cells[11].Text))
                        item.Cells[11].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[11].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[10])) > Convert.ToDouble(item.Cells[12].Text))
                        item.Cells[12].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[12].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[11])) > Convert.ToDouble(item.Cells[13].Text))
                        item.Cells[13].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[13].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[12])) > Convert.ToDouble(item.Cells[14].Text))
                        item.Cells[14].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[14].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[13])) > Convert.ToDouble(item.Cells[15].Text))
                        item.Cells[15].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[15].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[14])) > Convert.ToDouble(item.Cells[16].Text))
                        item.Cells[16].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[16].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[15])) > Convert.ToDouble(item.Cells[17].Text))
                        item.Cells[17].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[17].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[16])) > Convert.ToDouble(item.Cells[18].Text))
                        item.Cells[18].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[18].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[17])) > Convert.ToDouble(item.Cells[19].Text))
                        item.Cells[19].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[19].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[18])) > Convert.ToDouble(item.Cells[20].Text))
                        item.Cells[20].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[20].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[19])) > Convert.ToDouble(item.Cells[21].Text))
                        item.Cells[21].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[21].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[20])) > Convert.ToDouble(item.Cells[22].Text))
                        item.Cells[22].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[22].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[21])) > Convert.ToDouble(item.Cells[23].Text))
                        item.Cells[23].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[23].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[22])) > Convert.ToDouble(item.Cells[24].Text))
                        item.Cells[24].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[24].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[23])) > Convert.ToDouble(item.Cells[25].Text))
                        item.Cells[25].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[25].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[24])) > Convert.ToDouble(item.Cells[26].Text))
                        item.Cells[26].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[26].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[25])) > Convert.ToDouble(item.Cells[27].Text))
                        item.Cells[27].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[27].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[26])) > Convert.ToDouble(item.Cells[28].Text))
                        item.Cells[28].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[28].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[27])) > Convert.ToDouble(item.Cells[29].Text))
                        item.Cells[29].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[29].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[28])) > Convert.ToDouble(item.Cells[30].Text))
                        item.Cells[30].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[30].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[29])) > Convert.ToDouble(item.Cells[31].Text))
                        item.Cells[31].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[31].BackColor = System.Drawing.Color.GreenYellow;
                }
            }
        }

        protected void GridViewGrupoMasculino_DataBound(object sender, EventArgs e)
        {
            int[] qtdeLinhaTotal = new int[20];

            foreach (GridViewRow item in GridViewGrupoMasculino.Rows)
            {
                if (item.Cells[0].Text.Equals("Total"))
                {
                    qtdeLinhaTotal[0] = Convert.ToInt32(item.Cells[2].Text);
                    qtdeLinhaTotal[1] = Convert.ToInt32(item.Cells[3].Text);
                    qtdeLinhaTotal[2] = Convert.ToInt32(item.Cells[4].Text);
                    qtdeLinhaTotal[3] = Convert.ToInt32(item.Cells[5].Text);
                    qtdeLinhaTotal[4] = Convert.ToInt32(item.Cells[6].Text);
                    qtdeLinhaTotal[5] = Convert.ToInt32(item.Cells[7].Text);
                    qtdeLinhaTotal[6] = Convert.ToInt32(item.Cells[8].Text);
                    qtdeLinhaTotal[7] = Convert.ToInt32(item.Cells[9].Text);
                    qtdeLinhaTotal[8] = Convert.ToInt32(item.Cells[10].Text);
                    qtdeLinhaTotal[9] = Convert.ToInt32(item.Cells[11].Text);
                    qtdeLinhaTotal[10] = Convert.ToInt32(item.Cells[12].Text);
                    qtdeLinhaTotal[11] = Convert.ToInt32(item.Cells[13].Text);
                    qtdeLinhaTotal[12] = Convert.ToInt32(item.Cells[14].Text);
                    qtdeLinhaTotal[13] = Convert.ToInt32(item.Cells[15].Text);
                    qtdeLinhaTotal[14] = Convert.ToInt32(item.Cells[16].Text);
                    qtdeLinhaTotal[15] = Convert.ToInt32(item.Cells[17].Text);
                    qtdeLinhaTotal[16] = Convert.ToInt32(item.Cells[18].Text);
                    qtdeLinhaTotal[17] = Convert.ToInt32(item.Cells[19].Text);
                    qtdeLinhaTotal[18] = Convert.ToInt32(item.Cells[20].Text);
                    qtdeLinhaTotal[19] = Convert.ToInt32(item.Cells[21].Text);
                }
            }

            foreach (GridViewRow item in GridViewGrupoMasculino.Rows)
            {
                if (!item.Cells[0].Text.Equals("Total"))
                {
                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[0])) > Convert.ToDouble(item.Cells[2].Text))
                        item.Cells[2].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[2].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[1])) > Convert.ToDouble(item.Cells[3].Text))
                        item.Cells[3].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[3].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[2])) > Convert.ToDouble(item.Cells[4].Text))
                        item.Cells[4].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[4].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[3])) > Convert.ToDouble(item.Cells[5].Text))
                        item.Cells[5].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[5].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[4])) > Convert.ToDouble(item.Cells[6].Text))
                        item.Cells[6].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[6].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[5])) > Convert.ToDouble(item.Cells[7].Text))
                        item.Cells[7].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[7].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[6])) > Convert.ToDouble(item.Cells[8].Text))
                        item.Cells[8].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[8].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[7])) > Convert.ToDouble(item.Cells[9].Text))
                        item.Cells[9].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[9].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[8])) > Convert.ToDouble(item.Cells[10].Text))
                        item.Cells[10].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[10].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[9])) > Convert.ToDouble(item.Cells[11].Text))
                        item.Cells[11].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[11].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[10])) > Convert.ToDouble(item.Cells[12].Text))
                        item.Cells[12].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[12].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[11])) > Convert.ToDouble(item.Cells[13].Text))
                        item.Cells[13].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[13].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[12])) > Convert.ToDouble(item.Cells[14].Text))
                        item.Cells[14].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[14].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[13])) > Convert.ToDouble(item.Cells[15].Text))
                        item.Cells[15].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[15].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[14])) > Convert.ToDouble(item.Cells[16].Text))
                        item.Cells[16].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[16].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[15])) > Convert.ToDouble(item.Cells[17].Text))
                        item.Cells[17].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[17].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[16])) > Convert.ToDouble(item.Cells[18].Text))
                        item.Cells[18].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[18].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[17])) > Convert.ToDouble(item.Cells[19].Text))
                        item.Cells[19].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[19].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[18])) > Convert.ToDouble(item.Cells[20].Text))
                        item.Cells[20].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[20].BackColor = System.Drawing.Color.GreenYellow;

                    if (((Convert.ToDouble(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1)) / 100) * Convert.ToDouble(qtdeLinhaTotal[19])) > Convert.ToDouble(item.Cells[21].Text))
                        item.Cells[21].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[21].BackColor = System.Drawing.Color.GreenYellow;
                }
            }
        }
    }
}
