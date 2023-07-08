using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class LancamentoContabilEntity
    {
        public LancamentoContabilEntity()
        { }

        public int Empresa { get; set; }
        public string Filial { get; set; }
        public DateTime Data_Lancamento { get; set; }
        public string Conta_Saida { get; set; }
        public string Tipo_Lancamento_Saida { get; set; }
        public string Conta_Entrada { get; set; }
        public string Tipo_Lancamento_Entrada { get; set; }
        public decimal Valor_Deposito { get; set; }
        public string Desc_Despesa { get; set; }
        public string Historico_Lancamento { get; set; }
        public string Lancamento_Numero { get; set; }
        
    }
}
