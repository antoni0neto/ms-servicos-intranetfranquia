using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace Relatorios
{
    public class PocketEntity
    {
        public int index { get; set; }
        public int qtdeCopia { get; set; }
        public DESENV_PRODUTO produto { get; set; }
        public DESENV_ACESSORIO acessorio { get; set; }
    }
}