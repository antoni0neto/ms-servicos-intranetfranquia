using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.Services;

namespace Relatorios
{
    public partial class ecom_dragdrop_ordenacao : System.Web.UI.Page
    {

        const string PRO = "PRO";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "ecom_menu.aspx";


                CarregarProdutoIni();

            }
        }

        private void CarregarProdutoIni()
        {
            Session[PRO] = null;

            var ord = new List<ORD>();
            ord.Add(new ORD
            {
                Id1 = 1,
                Location1 = "Brazil",
                Preference1 = 1,
                Id2 = 2,
                Location2 = "New Zeland",
                Preference2 = 2,
                Id3 = 3,
                Location3 = "USA",
                Preference3 = 3
            });

            ord.Add(new ORD
            {
                Id1 = 4,
                Location1 = "Chile",
                Preference1 = 4,
                Id2 = 5,
                Location2 = "Argentina",
                Preference2 = 5,
                Id3 = 6,
                Location3 = "Australia",
                Preference3 = 6
            });

            ord.Add(new ORD
            {
                Id1 = 7,
                Location1 = "Africa",
                Preference1 = 7,
                Id2 = 8,
                Location2 = "Nigeria",
                Preference2 = 8,
                Id3 = 9,
                Location3 = "Egito",
                Preference3 = 9
            });





            Session[PRO] = ord;
            gvLocations.DataSource = ord;
            gvLocations.DataBind();
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            int[] locationIds1 = (from p in Request.Form["LocationId1"].Split(',')
                                  select int.Parse(p)).ToArray();
            int[] locationIds2 = (from p in Request.Form["LocationId2"].Split(',')
                                  select int.Parse(p)).ToArray();
            int[] locationIds3 = (from p in Request.Form["LocationId3"].Split(',')
                                  select int.Parse(p)).ToArray();

            int preference = 1;
            foreach (int locationId in locationIds1)
            {
                this.UpdatePreference(locationId, preference);
                preference += 1;
            }

            Response.Redirect(Request.Url.AbsoluteUri);
        }

        private void UpdatePreference(int locationId, int preference)
        {
            var t = locationId;

            t = locationId;
        }


    }
}

public class ORD
{
    public int Id1 { get; set; }
    public int Id2 { get; set; }
    public int Id3 { get; set; }

    public string Location1 { get; set; }
    public string Location2 { get; set; }
    public string Location3 { get; set; }

    public int Preference1 { get; set; }
    public int Preference2 { get; set; }
    public int Preference3 { get; set; }
}

