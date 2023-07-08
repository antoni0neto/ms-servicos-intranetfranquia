using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using DAL;

namespace Relatorios.wcfMobileApp
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class VendaOnline : IVendaOnline
    {
        UsuarioController _usuarioController = new UsuarioController();
        DesempenhoController _desempenhoController = new DesempenhoController();
        BaseController _baseController = new BaseController();
        SaidaProdutoController _saidaController = new SaidaProdutoController();

        //OK
        public bool helloApp()
        {
            return true;
        }
        //OK
        public int getUser(string user, string password)
        {
            if (user.Trim() == "" || password.Trim() == "")
                return 0;

            USUARIO _user = null;
            _user = _usuarioController.ValidaUsuario(user.Trim(), Criptografia.Encrypt(password.Trim()));
            //Encontrou?
            if (_user == null)
                return 0;

            //Tem perfil de ADM?
            //if (_user.CODIGO_PERFIL != 1)
            //return 0;

            return _user.CODIGO_USUARIO;
        }
        //FILTRAR POR USUARIO
        public IList<FILIAI1> getBranch(string usercode)
        {
            int usuarioId = 0;

            if (usercode.Trim() == "")
                throw new Exception("WCF: User Code is EMPTY.");

            if (!Int32.TryParse(usercode.Trim(), out usuarioId))
                throw new Exception("WCF: User Code is not INTEGER.");

            List<FILIAI1> _list = new List<FILIAI1>();
            _list = _baseController.ObterFiliaisIntranet(usuarioId);
            return _list;
        }
        //OK
        public SP_APP_OBTER_DESEMPENHOResult getBranchInformation(string _branch, string _dataIni, string _dataFim)
        {
            //Declaração de variáveis
            SP_APP_OBTER_DESEMPENHOResult _d;

            //Obter desempenho da filial
            _d = new SP_APP_OBTER_DESEMPENHOResult();
            _d = _baseController.WS_ObterDesempenhoFilial(_dataIni, _dataFim, _branch);

            return _d;
        }
        //OK
        public IList<SP_APP_OBTER_DESEMPENHOResult> getBranchGoal(string _dataIni, string _dataFim)
        {
            List<SP_APP_OBTER_DESEMPENHOResult> _list = null;

            _list = new List<SP_APP_OBTER_DESEMPENHOResult>();
            _list = _baseController.WS_ObterDesempenhoFilial(_dataIni, _dataFim);

            return _list;
        }
        //OK
        public IList<SP_APP_OBTER_DESEMPENHOResult> getBranchSupervisor(string _dataIni, string _dataFim)
        {
            //Declaração de variaveis
            List<SP_APP_OBTER_DESEMPENHOResult> _list = null;
            //List<SP_APP_OBTER_DESEMPENHOResult> _listHeader = null;
            List<SP_APP_OBTER_DESEMPENHOResult> _listFooter = null;
            //string _super = "";

            //Inicialização de variaveis
            _list = new List<SP_APP_OBTER_DESEMPENHOResult>();
            //_listHeader = new List<SP_APP_OBTER_DESEMPENHOResult>();
            _listFooter = new List<SP_APP_OBTER_DESEMPENHOResult>();

            //Criar HEADER
            _list = _baseController.WS_ObterDesempenhoFilial(_dataIni, _dataFim);
            /*
            foreach (SP_APP_OBTER_DESEMPENHOResult d in _list)
            {
                if (_super != d.NOME_USUARIO.Trim().ToUpper())
                {
                    _listHeader.Add(new SP_APP_OBTER_DESEMPENHOResult
                    {
                        COD_FILIAL = "0",
                        FILIAL = "HEADER",
                        NOME_USUARIO = d.NOME_USUARIO.Trim().ToUpper(),
                        VENDA = 0,
                        COTA = 0,
                        TICKETS = 0,
                        ULTIMA_VENDA = "00:00",
                        ITEM = 0
                    });
                }

                //Para controle de troca o usuário
                _super = d.NOME_USUARIO.Trim().ToUpper();
            }*/

            //Criar HEADER
            _listFooter = _list.GroupBy(p =>
                            new { NOME_USUARIO = p.NOME_USUARIO }).Select(i =>
                                    new SP_APP_OBTER_DESEMPENHOResult
                                    {
                                        POSICAO = 0,
                                        COD_FILIAL = "0",
                                        FILIAL = "FOOTER",
                                        NOME_USUARIO = i.Key.NOME_USUARIO,
                                        VENDA = i.Sum(w => w.VENDA),
                                        COTA = i.Sum(y => y.COTA),
                                        TICKETS = i.Sum(f => f.TICKETS),
                                        ULTIMA_VENDA = "00:00",
                                        ITEM = 2
                                    }).ToList();

            //Adicionar Header e Footer
            //_list = _list.Union(_listHeader).ToList();
            _list = _list.Union(_listFooter).ToList();

            _list.OrderBy(p => p.NOME_USUARIO).ThenBy(i => i.ITEM).ToList();

            return _list;
        }
        //OK
        public SP_APP_OBTER_DESEMPENHOResult getBranchTotal(string _dataIni, string _dataFim)
        {
            IList<SP_APP_OBTER_DESEMPENHOResult> _list = new List<SP_APP_OBTER_DESEMPENHOResult>();
            SP_APP_OBTER_DESEMPENHOResult _goal = new SP_APP_OBTER_DESEMPENHOResult();
            decimal venda = 0;
            decimal cota = 0;
            string atingido = "";
            int ticket = 0;
            int qtde_total_venda = 0;

            _list = getBranchGoal(_dataIni, _dataFim);

            if (_list != null)
            {
                venda = _list.Sum(p => p.VENDA);
                cota = _list.Sum(p => p.COTA);
                ticket = _list.Sum(p => p.TICKETS);
                atingido = ((venda / ((cota <= 0) ? 1 : cota)) * 100).ToString("##0.00");
                qtde_total_venda = _list.Sum(p => p.QTDE_TOTAL_VENDA);

                _goal.POSICAO = 0;
                _goal.COTA = (cota <= 0) ? 1 : cota;
                _goal.FILIAL = "Rede";
                _goal.ULTIMA_VENDA = "00:00";
                _goal.TICKETS = ticket;
                _goal.VENDA = venda;
                _goal.QTDE_TOTAL_VENDA = qtde_total_venda;
            }

            return _goal;
        }
        //OK
        public IList<SP_Vendas_VendedorResult> getSalesManInformation(string _branch, string _dataIni, string _dataFim)
        {
            DateTime ini = DateTime.Now;
            DateTime fim = DateTime.Now;

            DateTime.TryParse(_dataIni, out ini);
            DateTime.TryParse(_dataFim, out fim);

            return _saidaController.BuscaVendasVendedor(ini.ToString("dd/MM/yyyy"), fim.ToString("dd/MM/yyyy"), _branch);
        }

    }
}

