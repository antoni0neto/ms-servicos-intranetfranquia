using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DAL
{
    public class Farol
    {
        public Farol()
        {
        }

        private string loja;

        public string Loja
        {
            get { return loja; }
            set { loja = value; }
        }
        //Dia
        private string vl_cota_dia;

        public string Vl_cota_dia
        {
            get { return vl_cota_dia; }
            set { vl_cota_dia = value; }
        }

        private int qt_cota_dia;

        public int Qt_cota_dia
        {
            get { return qt_cota_dia; }
            set { qt_cota_dia = value; }
        }

        private string vl_venda_dia;

        public string Vl_venda_dia
        {
            get { return vl_venda_dia; }
            set { vl_venda_dia = value; }
        }

        private int qt_venda_dia;

        public int Qt_venda_dia
        {
            get { return qt_venda_dia; }
            set { qt_venda_dia = value; }
        }

        private string vl_medio_dia;

        public string Vl_medio_dia
        {
            get { return vl_medio_dia; }
            set { vl_medio_dia = value; }
        }

        private string perc_at_dia;

        public string Perc_at_dia
        {
            get { return perc_at_dia; }
            set { perc_at_dia = value; }
        }

        private string vl_cota_dia_ano_ant;

        public string Vl_cota_dia_ano_ant
        {
            get { return vl_cota_dia_ano_ant; }
            set { vl_cota_dia_ano_ant = value; }
        }

        private int qt_cota_dia_ano_ant;

        public int Qt_cota_dia_ano_ant
        {
            get { return qt_cota_dia_ano_ant; }
            set { qt_cota_dia_ano_ant = value; }
        }

        private string vl_venda_dia_ano_ant;

        public string Vl_venda_dia_ano_ant
        {
            get { return vl_venda_dia_ano_ant; }
            set { vl_venda_dia_ano_ant = value; }
        }

        private int qt_venda_dia_ano_ant;

        public int Qt_venda_dia_ano_ant
        {
            get { return qt_venda_dia_ano_ant; }
            set { qt_venda_dia_ano_ant = value; }
        }

        private string vl_medio_dia_ano_ant;

        public string Vl_medio_dia_ano_ant
        {
            get { return vl_medio_dia_ano_ant; }
            set { vl_medio_dia_ano_ant = value; }
        }

        private string perc_at_dia_ano_ant;

        public string Perc_at_dia_ano_ant
        {
            get { return perc_at_dia_ano_ant; }
            set { perc_at_dia_ano_ant = value; }
        }

        private string perc_at_dia_cota;

        public string Perc_at_dia_cota
        {
            get { return perc_at_dia_cota; }
            set { perc_at_dia_cota = value; }
        }

        private string perc_at_dia_venda;

        public string Perc_at_dia_venda
        {
            get { return perc_at_dia_venda; }
            set { perc_at_dia_venda = value; }
        }
        //Sem
        private string vl_cota_sem;

        public string Vl_cota_sem
        {
            get { return vl_cota_sem; }
            set { vl_cota_sem = value; }
        }

        private int qt_cota_sem;

        public int Qt_cota_sem
        {
            get { return qt_cota_sem; }
            set { qt_cota_sem = value; }
        }

        private string vl_venda_sem;

        public string Vl_venda_sem
        {
            get { return vl_venda_sem; }
            set { vl_venda_sem = value; }
        }

        private int qt_venda_sem;

        public int Qt_venda_sem
        {
            get { return qt_venda_sem; }
            set { qt_venda_sem = value; }
        }

        private string vl_medio_sem;

        public string Vl_medio_sem
        {
            get { return vl_medio_sem; }
            set { vl_medio_sem = value; }
        }

        private string perc_at_sem;

        public string Perc_at_sem
        {
            get { return perc_at_sem; }
            set { perc_at_sem = value; }
        }

        private string vl_cota_sem_ano_ant;

        public string Vl_cota_sem_ano_ant
        {
            get { return vl_cota_sem_ano_ant; }
            set { vl_cota_sem_ano_ant = value; }
        }

        private int qt_cota_sem_ano_ant;

        public int Qt_cota_sem_ano_ant
        {
            get { return qt_cota_sem_ano_ant; }
            set { qt_cota_sem_ano_ant = value; }
        }

        private string vl_venda_sem_ano_ant;

        public string Vl_venda_sem_ano_ant
        {
            get { return vl_venda_sem_ano_ant; }
            set { vl_venda_sem_ano_ant = value; }
        }

        private int qt_venda_sem_ano_ant;

        public int Qt_venda_sem_ano_ant
        {
            get { return qt_venda_sem_ano_ant; }
            set { qt_venda_sem_ano_ant = value; }
        }

        private string vl_medio_sem_ano_ant;

        public string Vl_medio_sem_ano_ant
        {
            get { return vl_medio_sem_ano_ant; }
            set { vl_medio_sem_ano_ant = value; }
        }

        private string perc_at_sem_ano_ant;

        public string Perc_at_sem_ano_ant
        {
            get { return perc_at_sem_ano_ant; }
            set { perc_at_sem_ano_ant = value; }
        }

        private string perc_at_sem_cota;

        public string Perc_at_sem_cota
        {
            get { return perc_at_sem_cota; }
            set { perc_at_sem_cota = value; }
        }

        private string perc_at_sem_venda;

        public string Perc_at_sem_venda
        {
            get { return perc_at_sem_venda; }
            set { perc_at_sem_venda = value; }
        }
        //Mês
        private string vl_cota_mes;

        public string Vl_cota_mes
        {
            get { return vl_cota_mes; }
            set { vl_cota_mes = value; }
        }

        private int qt_cota_mes;

        public int Qt_cota_mes
        {
            get { return qt_cota_mes; }
            set { qt_cota_mes = value; }
        }

        private string vl_venda_mes;

        public string Vl_venda_mes
        {
            get { return vl_venda_mes; }
            set { vl_venda_mes = value; }
        }

        private int qt_venda_mes;

        public int Qt_venda_mes
        {
            get { return qt_venda_mes; }
            set { qt_venda_mes = value; }
        }

        private string vl_medio_mes;

        public string Vl_medio_mes
        {
            get { return vl_medio_mes; }
            set { vl_medio_mes = value; }
        }

        private string perc_at_mes;

        public string Perc_at_mes
        {
            get { return perc_at_mes; }
            set { perc_at_mes = value; }
        }

        private string vl_cota_mes_ano_ant;

        public string Vl_cota_mes_ano_ant
        {
            get { return vl_cota_mes_ano_ant; }
            set { vl_cota_mes_ano_ant = value; }
        }

        private int qt_cota_mes_ano_ant;

        public int Qt_cota_mes_ano_ant
        {
            get { return qt_cota_mes_ano_ant; }
            set { qt_cota_mes_ano_ant = value; }
        }

        private string vl_venda_mes_ano_ant;

        public string Vl_venda_mes_ano_ant
        {
            get { return vl_venda_mes_ano_ant; }
            set { vl_venda_mes_ano_ant = value; }
        }

        private int qt_venda_mes_ano_ant;

        public int Qt_venda_mes_ano_ant
        {
            get { return qt_venda_mes_ano_ant; }
            set { qt_venda_mes_ano_ant = value; }
        }

        private string vl_medio_mes_ano_ant;

        public string Vl_medio_mes_ano_ant
        {
            get { return vl_medio_mes_ano_ant; }
            set { vl_medio_mes_ano_ant = value; }
        }

        private string perc_at_mes_ano_ant;

        public string Perc_at_mes_ano_ant
        {
            get { return perc_at_mes_ano_ant; }
            set { perc_at_mes_ano_ant = value; }
        }

        private string perc_at_mes_cota;

        public string Perc_at_mes_cota
        {
            get { return perc_at_mes_cota; }
            set { perc_at_mes_cota = value; }
        }

        private string perc_at_mes_venda;

        public string Perc_at_mes_venda
        {
            get { return perc_at_mes_venda; }
            set { perc_at_mes_venda = value; }
        }
        //Tri
        private string vl_venda_tri;

        public string Vl_venda_tri
        {
            get { return vl_venda_tri; }
            set { vl_venda_tri = value; }
        }

        private string vl_venda_tri_ano_ant;

        public string Vl_venda_tri_ano_ant
        {
            get { return vl_venda_tri_ano_ant; }
            set { vl_venda_tri_ano_ant = value; }
        }

        private string perc_venda_tri_at_ano;

        public string Perc_venda_tri_at_ano
        {
            get { return perc_venda_tri_at_ano; }
            set { perc_venda_tri_at_ano = value; }
        }

        private string vl_cota_tri;

        public string Vl_cota_tri
        {
            get { return vl_cota_tri; }
            set { vl_cota_tri = value; }
        }

        private string vl_cota_tri_ano_ant;

        public string Vl_cota_tri_ano_ant
        {
            get { return vl_cota_tri_ano_ant; }
            set { vl_cota_tri_ano_ant = value; }
        }

        private string perc_cota_tri_at_ano;

        public string Perc_cota_tri_at_ano
        {
            get { return perc_cota_tri_at_ano; }
            set { perc_cota_tri_at_ano = value; }
        }

        private string perc_venda_cota_tri;

        public string Perc_venda_cota_tri
        {
            get { return perc_venda_cota_tri; }
            set { perc_venda_cota_tri = value; }
        }

        private string perc_venda_cota_tri_ano_ant;

        public string Perc_venda_cota_tri_ano_ant
        {
            get { return perc_venda_cota_tri_ano_ant; }
            set { perc_venda_cota_tri_ano_ant = value; }
        }

        private string perc_qt_atendido_dia;

        public string Perc_qt_atendido_dia
        {
            get { return perc_qt_atendido_dia; }
            set { perc_qt_atendido_dia = value; }
        }

        private string perc_qt_atendido_sem;

        public string Perc_qt_atendido_sem
        {
            get { return perc_qt_atendido_sem; }
            set { perc_qt_atendido_sem = value; }
        }

        private string perc_qt_atendido_mes;

        public string Perc_qt_atendido_mes
        {
            get { return perc_qt_atendido_mes; }
            set { perc_qt_atendido_mes = value; }
        }

        private string perc_vl_medio_atendido_dia;

        public string Perc_vl_medio_atendido_dia
        {
            get { return perc_vl_medio_atendido_dia; }
            set { perc_vl_medio_atendido_dia = value; }
        }

        private string perc_vl_medio_atendido_sem;

        public string Perc_vl_medio_atendido_sem
        {
            get { return perc_vl_medio_atendido_sem; }
            set { perc_vl_medio_atendido_sem = value; }
        }

        private string perc_vl_medio_atendido_mes;

        public string Perc_vl_medio_atendido_mes
        {
            get { return perc_vl_medio_atendido_mes; }
            set { perc_vl_medio_atendido_mes = value; }
        }
    }
}