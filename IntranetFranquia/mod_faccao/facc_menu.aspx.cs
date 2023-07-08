using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class facc_menu : System.Web.UI.Page
    {

        FaccaoController faccController = new FaccaoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                USUARIO usuario = null;

                if (Session["USUARIO"] != null)
                {
                    usuario = new USUARIO();
                    usuario = (USUARIO)Session["USUARIO"];

                    //List<SP_OBTER_LISTA_SAIDAResult> _saida = null;
                    //List<SP_OBTER_LISTA_ENTRADAResult> _entrada = null;
                    //List<SP_OBTER_FACCAO_TIMEResult> _timeFaccao = null;
                    //List<SP_OBTER_FACCAO_TIME_ENCAIXEResult> _timeFaccaoEncaixe = null;
                    //List<SP_OBTER_HB_CORTE_BAIXA_HOJEResult> _hbCorteHoje = null;

                    if (usuario != null)
                    {
                        if (usuario.CODIGO_PERFIL == 44)
                        {
                            pnlAcabamentoInterno.Visible = true;
                        }
                        else if (usuario.CODIGO_PERFIL == 45)
                        {
                            pnlLogistica.Visible = true;
                        }
                        else if (usuario.CODIGO_PERFIL == 46)
                        {
                            pnlAcabamentoExterno.Visible = true;
                        }
                        else
                        {
                            pnlMenuGeral.Visible = true;
                        }

                        ////contadores
                        //_entrada = new List<SP_OBTER_LISTA_ENTRADAResult>();
                        //_entrada.AddRange(faccController.ObterListaEntrada(21, "", ' ').Where(p => p.GRADE_TOTAL_SAIDA > 0));
                        //var countInterno = _entrada.Where(p => p.FORNECEDOR.Contains("HANDBOOK") && (p.STATUS == 'A' || p.STATUS == 'P')).Count();
                        //var countExterno = _entrada.Where(p => !p.FORNECEDOR.Contains("HANDBOOK") && (p.STATUS == 'A' || p.STATUS == 'P')).Count();
                        //var countOutros = _entrada.Where(p => (p.STATUS == 'V' || p.STATUS == 'L' || p.STATUS == 'C') && p.DATA_DISTRIBUICAO == null).Count();
                        //var countLogistica = faccController.ObterListaEntrada(22, "", ' ').Where(p => p.GRADE_TOTAL_SAIDA > 0).Count();

                        //// PRIMEIRO MENU
                        //_saida = new List<SP_OBTER_LISTA_SAIDAResult>();
                        //_saida.AddRange(faccController.ObterListaSaida(20, 1));
                        //mnuFaccao.Items[0].Text = "1. Encaixe <font color='red'>(" + _saida.Count().ToString() + ")</font>";

                        //_saida = new List<SP_OBTER_LISTA_SAIDAResult>();
                        //_saida.AddRange(faccController.ObterListaSaida(20, 2));
                        //mnuFaccao.Items[1].Text = "2. Emissão Nota Fiscal <font color='red'>(" + _saida.Count().ToString() + ")</font>";

                        //_entrada = new List<SP_OBTER_LISTA_ENTRADAResult>();
                        //_entrada.AddRange(faccController.ObterListaEntrada(20, "", ' ').Where(p => p.GRADE_TOTAL_SAIDA > 0));
                        //mnuFaccao.Items[2].Text = "3. Entrada de Produto <font color='red'>(" + _entrada.Count().ToString() + ")</font>";
                        //mnuFaccaoExterno.Items[0].Text = "1. Entrada de Produto Facção <font color='red'>(" + _entrada.Count().ToString() + ")</font>";

                        //_entrada = new List<SP_OBTER_LISTA_ENTRADAResult>();
                        //_entrada.AddRange(faccController.ObterListaEntrada(20, "", ' ').Where(p => p.GRADE_TOTAL_SAIDA > 0 && (p.STATUS == 'P')));
                        //mnuFaccao.Items[3].Text = "4. Pendentes <font color='red'>(" + _entrada.Count().ToString() + ")</font>";
                        //mnuFaccaoExterno.Items[1].Text = "2. Pendentes <font color='red'>(" + _entrada.Count().ToString() + ")</font>";

                        //var aval = faccController.ObterFaccaoAvaliacaoRel("", null, "", "").Where(p => p.NOTA1 == null && p.GRADE_RECEBIDA > 0).Count();
                        //mnuFaccao.Items[4].Text = "5. Avaliação de Produção <font color='red'>(" + aval.ToString() + ")</font>";

                        if (usuario.CODIGO_USUARIO != 18 && usuario.CODIGO_USUARIO != 1144)
                        {
                            if (mnuFaccao.Items.Count > 5)
                                mnuFaccao.Items.RemoveAt(5);
                        }


                        ////SEGUNDO MENU
                        //_saida = new List<SP_OBTER_LISTA_SAIDAResult>();
                        //_saida.AddRange(faccController.ObterListaSaida(21, 1));
                        //mnuFaccaoAcabamento.Items[0].Text = "1. Encaixe Acabamento <font color='red'>(" + _saida.Count().ToString() + ")</font>";

                        //_saida = new List<SP_OBTER_LISTA_SAIDAResult>();
                        //_saida.AddRange(faccController.ObterListaSaida(21, 2));
                        //mnuFaccaoAcabamento.Items[1].Text = "2. Emissão Nota Fiscal Acabamento <font color='red'>(" + _saida.Count().ToString() + ")</font>";

                        //mnuFaccaoAcabamento.Items[2].Text = "3. Entrada Produto Acabado Interno <font color='red'>(" + (countInterno).ToString() + ")</font>";
                        //mnuFaccaoAcabamento.Items[3].Text = "4. Entrada Produto Acabado Externo <font color='red'>(" + (countExterno).ToString() + ")</font>";
                        //mnuFaccaoAcabamento.Items[4].Text = "5. Entrada Produto Acabado Outros <font color='red'>(" + (countOutros).ToString() + ")</font>";
                        //mnuFaccaoAcabamento.Items[5].Text = "6. Entrada Produto Logística <font color='red'>(" + (countLogistica).ToString() + ")</font>";


                        ////TERCEIRO MENU
                        //_timeFaccao = new List<SP_OBTER_FACCAO_TIMEResult>();
                        //_timeFaccao.AddRange(faccController.ObterFaccaoTime("", null, null, "", "", null, "AG. ENTRADA").Where(p => p.SUBSTATUS.Trim() == "CORTE INTEIRO" && p.STATUS_SEMANA != "OK"));

                        //_timeFaccaoEncaixe = new List<SP_OBTER_FACCAO_TIME_ENCAIXEResult>();
                        //_timeFaccaoEncaixe.AddRange(faccController.ObterFaccaoTimeEncaixe("", null, null, "", null, null, ""));
                        //mnuRelatorio1.Items[5].Text = "6. Time Facção <font color='red'>(" + (_timeFaccao.Count() + _timeFaccaoEncaixe.Count()).ToString() + ")</font>";
                        //mnuRelatorio2.Items[5].Text = "6. Time Facção <font color='red'>(" + (_timeFaccao.Count() + _timeFaccaoEncaixe.Count()).ToString() + ")</font>";
                        //mnuRelatorio3.Items[5].Text = "6. Time Facção <font color='red'>(" + (_timeFaccao.Count() + _timeFaccaoEncaixe.Count()).ToString() + ")</font>";
                        //mnuRelatorio4.Items[5].Text = "6. Time Facção <font color='red'>(" + (_timeFaccao.Count() + _timeFaccaoEncaixe.Count()).ToString() + ")</font>";

                        //_hbCorteHoje = new List<SP_OBTER_HB_CORTE_BAIXA_HOJEResult>();
                        //_hbCorteHoje.AddRange(faccController.ObterHBCorteHoje());
                        //mnuRelatorio1.Items[13].Text = "14. Status de Detalhes <font color='red'>(" + (_hbCorteHoje[0].BAIXADO_HOJE).ToString() + ")</font>";
                        //mnuRelatorio2.Items[13].Text = "14. Status de Detalhes <font color='red'>(" + (_hbCorteHoje[0].BAIXADO_HOJE).ToString() + ")</font>";
                        //mnuRelatorio3.Items[13].Text = "14. Status de Detalhes <font color='red'>(" + (_hbCorteHoje[0].BAIXADO_HOJE).ToString() + ")</font>";
                        //mnuRelatorio4.Items[13].Text = "14. Status de Detalhes <font color='red'>(" + (_hbCorteHoje[0].BAIXADO_HOJE).ToString() + ")</font>";

                        //// Menus por perfil
                        //mnuAcabamentoInterno.Items[1].Text = "2. Entrada Produto Acabado Interno <font color='red'>(" + (countInterno).ToString() + ")</font>";
                        //mnuAcabamentoExterno.Items[0].Text = "1. Entrada Produto Acabado Externo <font color='red'>(" + (countExterno).ToString() + ")</font>";
                        //mnuLogistica.Items[0].Text = "1. Entrada Produto Logística <font color='red'>(" + (countLogistica).ToString() + ")</font>";

                    }
                }
            }
        }
    }
}