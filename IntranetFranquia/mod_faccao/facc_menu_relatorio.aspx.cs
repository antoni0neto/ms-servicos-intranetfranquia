using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class facc_menu_relatorio : System.Web.UI.Page
    {

        FaccaoController faccController = new FaccaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            USUARIO usuario = null;

            if (Session["USUARIO"] != null)
            {
                usuario = new USUARIO();
                usuario = (USUARIO)Session["USUARIO"];

                List<SP_OBTER_FACCAO_TIMEResult> _timeFaccao = null;
                List<SP_OBTER_FACCAO_TIME_ENCAIXEResult> _timeFaccaoEncaixe = null;

                if (usuario != null)
                {
                    _timeFaccao = new List<SP_OBTER_FACCAO_TIMEResult>();
                    _timeFaccao.AddRange(faccController.ObterFaccaoTime("", null, null, "", "", null, "AG. ENTRADA").Where(p => p.SUBSTATUS.Trim() == "CORTE INTEIRO" && p.STATUS_SEMANA != "OK"));

                    _timeFaccaoEncaixe = new List<SP_OBTER_FACCAO_TIME_ENCAIXEResult>();
                    _timeFaccaoEncaixe.AddRange(faccController.ObterFaccaoTimeEncaixe("", null, null, "", null, null, ""));
                    mnuRelatorio.Items[4].Text = "5. Time Facção <font color='red'>(" + (_timeFaccao.Count() + _timeFaccaoEncaixe.Count()).ToString() + ")</font>";
                }
            }
        }
    }
}