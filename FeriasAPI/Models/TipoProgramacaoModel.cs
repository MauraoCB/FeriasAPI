using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriasAPI.Models
{
    public class TipoProgramacaoModel
    {
        public int TPPG_ID { get; set; }
        public string TPPG_DESCRICAO { get; set; }
        public int  TPPG_DIAS_1_PER { get; set; }
        public int  TPPG_DIAS_2_PER { get; set; }
        public int  TPPG_ABONO { get; set; }
        public int TPVZ_ID { get; set; }
        public bool  TPPG_ATIVO { get; set; }
    }
}