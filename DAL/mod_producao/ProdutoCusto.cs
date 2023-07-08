using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace DAL
{
    public class ProdutoCusto
    {
        public string COLUNA_NOME { get; set; }
        public decimal COLUNA_A { get; set; }
        public decimal COLUNA_B { get; set; }
        public decimal COLUNA_C { get; set; }
        public decimal COLUNA_D { get; set; }
        public decimal COLUNA_E { get; set; }

        public PRODUTO_CUSTO_SIMULACAO CalcularCustoProduto2(decimal p_custoInicial, decimal p_ICMSCredito, decimal p_markUP, decimal p_precoProduto, decimal p_custoRotativo, decimal p_TT, decimal p_impostoTT, decimal p_ICMS, PRODUTO_CUSTO_SIMULACAO_PORC valCalculo)
        {
            decimal v_precoEntrada = p_precoProduto;

            // Se informado preço aprovado, calcular apenas uma vez
            p_precoProduto = (p_markUP == 0) ? p_precoProduto : (p_custoRotativo * p_markUP);
            p_TT = (p_precoProduto * (valCalculo.TT / 100));
            p_impostoTT = (p_TT * (valCalculo.IMPOSTO / 100));
            p_ICMS = ((p_TT * (valCalculo.ICMS_DEBITO / 100)) - p_ICMSCredito);
            p_custoRotativo = p_custoInicial + p_impostoTT + p_ICMS;

            if ((Math.Round((Math.Abs(p_precoProduto) - Math.Abs(v_precoEntrada)), 3) <= Convert.ToDecimal(0.010)) || p_markUP == 0)
            {
                PRODUTO_CUSTO_SIMULACAO pc = new PRODUTO_CUSTO_SIMULACAO();
                pc.PRECO_PRODUTO = Math.Round(p_precoProduto, 2);
                pc.TT_PORC = Math.Round(p_TT, 2);
                pc.TT_IMPOSTO = Math.Round(p_impostoTT, 2);
                pc.ICMS = Math.Round(p_ICMS, 2);
                pc.CUSTO_COM_IMPOSTO = Math.Round(p_custoRotativo, 2);
                pc.MARKUP = p_markUP;

                return pc;
            }

            return CalcularCustoProduto2(p_custoInicial, p_ICMSCredito, p_markUP, p_precoProduto, p_custoRotativo, p_TT, p_impostoTT, p_ICMS, valCalculo);
        }
        public PRODUTO_CUSTO_SIMULACAO CalcularLucro(PRODUTO_CUSTO_SIMULACAO p_produtoCusto, PRODUTO_CUSTO_SIMULACAO_PORC valCalculo)
        {
            PRODUTO_CUSTO_SIMULACAO produto_custo = null;

            if (p_produtoCusto == null)
                throw new Exception("CUSTO DO PRODUTO NÃO INFORMADO...");

            decimal custos = 0;
            decimal impostos = 0;
            decimal icmsCredito = 0;
            decimal icmsDebito = 0;
            decimal icms = 0;
            decimal margem = 0;
            decimal lucro = 0;

            custos = p_produtoCusto.PRECO_PRODUTO * (valCalculo.CUSTO_LOJA / 100);
            impostos = p_produtoCusto.PRECO_PRODUTO * (valCalculo.IMPOSTO / 100);
            icmsCredito = p_produtoCusto.TT_PORC * (valCalculo.ICMS_CREDITO_LOJA / 100);
            icmsDebito = p_produtoCusto.PRECO_PRODUTO * (valCalculo.ICMS_DEBITO_LOJA / 100);
            icms = (icmsDebito - icmsCredito);
            margem = p_produtoCusto.PRECO_PRODUTO - p_produtoCusto.CUSTO_COM_IMPOSTO - Math.Round(custos, 2) - Math.Round(impostos, 2) - Math.Round(icms, 2);
            lucro = (margem / p_produtoCusto.PRECO_PRODUTO);

            produto_custo = new PRODUTO_CUSTO_SIMULACAO();
            produto_custo = p_produtoCusto;
            //retirado 26/06 - solicitado por tata
            //produto_custo.LUCRO_PORC = Math.Round(lucro, 2) * 100;
            produto_custo.LUCRO_PORC = 0;

            if (produto_custo.MARKUP == 0)
                produto_custo.MARKUP = Math.Round((p_produtoCusto.PRECO_PRODUTO / p_produtoCusto.CUSTO_COM_IMPOSTO), 1);

            return produto_custo;
        }

        public PRODUTO_CUSTO_SIMULACAO CalcularCustoProdutoX(decimal custo1, decimal markUP, decimal precoProdutoValidado, decimal operacionalEEtiqueta)
        {
            decimal precoProduto = 0;

            if (markUP > 0)
                precoProduto = ((custo1 + operacionalEEtiqueta) * markUP);
            else
                precoProduto = precoProdutoValidado;

            var pc = new PRODUTO_CUSTO_SIMULACAO();
            pc.PRECO_PRODUTO = Math.Round(precoProduto, 2);
            pc.TT_PORC = 0;//Convert.ToDecimal(valCalculo.VALOR_TT25);
            pc.TT_IMPOSTO = 0;
            pc.ICMS = 0;
            pc.CUSTO_COM_IMPOSTO = 0;//Math.Round(custoComImposto, 2);

            if (markUP > 0)
                pc.MARKUP = markUP;
            else
                pc.MARKUP = ((precoProduto) / (custo1 + operacionalEEtiqueta));

            return pc;
        }



        public PRODUTO_CUSTO_SIMULACAO CalcularCustoProduto27(decimal custo, decimal markUP, decimal precoProdutoValidado)
        {
            decimal precoProduto = 0;

            if (markUP > 0)
                precoProduto = ((custo) * markUP);
            else
                precoProduto = precoProdutoValidado;

            var pc = new PRODUTO_CUSTO_SIMULACAO();
            pc.PRECO_PRODUTO = Math.Round(precoProduto, 2);
            pc.TT_PORC = 0;//Convert.ToDecimal(valCalculo.VALOR_TT25);
            pc.TT_IMPOSTO = 0;
            pc.ICMS = 0;
            pc.CUSTO_COM_IMPOSTO = 0;//Math.Round(custoComImposto, 2);

            if (markUP > 0)
                pc.MARKUP = markUP;
            else
                pc.MARKUP = ((precoProduto) / (custo));

            return pc;
        }


        public PRODUTO_CUSTO_SIMULACAO CalcularCustoProdutoTerceiro(decimal CMV, decimal markUP, decimal precoProdutoValidado, PRODUTO_CUSTO_SIMULACAO_PORC valCalculo)
        {

            var p = new PRODUTO_CUSTO_SIMULACAO();

            //decimal impostoAMAIS = (CMV * Convert.ToDecimal(valCalculo.VALOR_PORC_AMAIS25));
            //decimal custoComImposto = CMV + impostoAMAIS;

            //custoComImposto = (custoComImposto / Convert.ToDecimal(valCalculo.VALOR_TT25));

            decimal precoProduto = 0;
            if (markUP > 0)
                precoProduto = (CMV * markUP);
            else
                precoProduto = precoProdutoValidado;

            p.PRECO_PRODUTO = precoProduto;
            p.LUCRO_PORC = 0;

            if (markUP > 0)
                p.MARKUP = markUP;
            else
                p.MARKUP = Math.Round((p.PRECO_PRODUTO / CMV), 1);

            p.CUSTO_COM_IMPOSTO = CMV;

            return p;
        }
    }

}
