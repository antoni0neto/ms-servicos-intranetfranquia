using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class rh_menu : System.Web.UI.Page
    {

        RHController rhController = new RHController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                USUARIO usuario = null;
                pnlLoja.Visible = false;
                pnlEscritorio.Visible = false;

                if (Session["USUARIO"] != null)
                {
                    usuario = new USUARIO();
                    usuario = (USUARIO)Session["USUARIO"];

                    if (usuario != null)
                    {
                        int codigoPerfil = usuario.CODIGO_PERFIL;

                        if (codigoPerfil == 2 || codigoPerfil == 11 || codigoPerfil == 1 || codigoPerfil == 3)
                            pnlLoja.Visible = true;
                        if (codigoPerfil == 11 || codigoPerfil == 1)
                            pnlEscritorio.Visible = true;

                        /*CONTAGEM DE SOLICITACOES PENDENTES*/
                        List<USUARIOLOJA> lojaUsuario = new BaseController().BuscaUsuarioLoja(usuario);
                        var painelSol = rhController.ObterPainelSolicitacao("", null, null);
                        painelSol = painelSol.Where(p => lojaUsuario.Select(x => x.CODIGO_LOJA).Contains(p.CODIGO_FILIAL)).ToList();

                        if (codigoPerfil == 2 || codigoPerfil == 3)
                            painelSol = painelSol.Where(p => p.RH_SOL_STATUS == 1 || p.RH_SOL_STATUS == 2).ToList();
                        else
                            painelSol = painelSol.Where(p => p.RH_SOL_STATUS == 2 || p.RH_SOL_STATUS == 3).ToList();
                        if (painelSol != null)
                            mnuSolicitacao.Items[7].Text = "8. Solicitações Pendentes <font color='red'>(" + painelSol.Count.ToString() + ")</font>";
                        /************************************/

                        /*CONTAGEM DE BATIDAS PENDENTES******/
                        //var pontoBatida = rhController.ObterPontoBatida().Where(p => (p.STATUS == 'N' || p.RH_PONTO_BATIDA_TIPO == 99) && p.STATUS != 'E' && DateTime.Today > p.DATA_INCLUSAO.Date.AddHours(23).AddMinutes(59)).ToList();
                        //if (pontoBatida != null)
                        //    mnuControlePontoEscritorio.Items[1].Text = "2. Batidas Pendentes <font color='red'>(" + pontoBatida.Count.ToString() + ")</font>";
                        /************************************/
                    }
                }
            }
        }
    }
}