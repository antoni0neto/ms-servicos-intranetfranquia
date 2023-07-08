using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using Microsoft.Reporting.WebForms;
using System.Net;

namespace Relatorios
{
    public partial class ImprimeMovimentoProduto : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ReportViewer1.ProcessingMode = ProcessingMode.Local;

                //ServerReport serverReport = ReportViewer1.ServerReport;

                ////CustomReportCredentials irsc = new CustomReportCredentials("Administrador", "@5kl1kmm", string.Empty);
                ////serverReport.ReportServerCredentials = irsc;

                //serverReport.ReportServerUrl = new Uri(@"http://187.8.234.228:8012/ReportServerTeste");
                ////serverReport.ReportServerUrl = new Uri(@"http://localhost:8012/ReportServerTeste");
                ////http://192.168.1.8:8012/ReportServerTeste/ReportService.asmx

                //serverReport.ReportPath = @"/Report1.rdlc";
                //serverReport.Refresh();

            }

            string[] datas = baseController.RetornaDatas();

            HiddenFieldData1.Value = datas[1];
            HiddenFieldData2.Value = datas[3];
            HiddenFieldData3.Value = datas[5];
            HiddenFieldData4.Value = datas[7];
            HiddenFieldData5.Value = datas[9];

            ReportViewer1.DataBind();
        }
    }

    public class CustomReportCredentials : IReportServerCredentials
    {
        private string _UserName;
        private string _PassWord;
        private string _DomainName;

        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }

        public ICredentials NetworkCredentials
        {
            //get { return new NetworkCredential(_UserName, _PassWord, _DomainName); }
            get { return new NetworkCredential(_UserName, _PassWord); }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user,
         out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}
