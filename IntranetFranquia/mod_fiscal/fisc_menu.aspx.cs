using DAL;
using System;
using System.Linq;
using System.Web.UI;

namespace Relatorios
{
    public partial class fisc_menu : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ContabilidadeController contController = new ContabilidadeController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var aliqICMSBAixa = contController.ObterNFAliqImposto(0);
                var icmsRegimeEsp = contController.ObterNFAliqImposto(1);
                mnuFiscal.Items[2].Text = "3. Diferencial Alíquota ICMS <font color='red'>(" + (aliqICMSBAixa.Count() + icmsRegimeEsp.Count()).ToString() + ")</font>";

                var certAVencer = contController.ObterCertificadoDigitalAVencer(30);
                if (certAVencer != null)
                    mnuCadastro.Items[2].Text = "3. Certificado Digital <font color='red'>(" + (certAVencer.Count()).ToString() + ")</font>";

                var nfDev = desenvController.ObterPedidoQtdeEmissaoNF();
                if (nfDev != null)
                    mnuFiscal.Items[8].Text = "9. Emissão NF Devolução de Tecido <font color='red'>(" + (nfDev.Count()).ToString() + ")</font>";

                var retiradaDev = desenvController.ObterPedidoQtdeRetiradaBaixa();
                if (retiradaDev != null)
                    mnuFiscal.Items[9].Text = "10. Baixa de Tecido Devolvido <font color='red'>(" + (retiradaDev.Count()).ToString() + ")</font>";



            }
        }

    }
}
