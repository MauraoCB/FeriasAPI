using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriasAPI.Models
{
    public class DistribuicaoFeriasOperModel
    {
        public DateTime DFOP_ADMISSAO { get; set; }
        public DateTime DFOP_DATA_LIMITE { get; set; }
        public DateTime DFOP_PROGRAMACAO { get; set; }
        public DateTime DFOP_DT_LEITURA { get; set; }
        public DateTime DFOP_ULT_LEITURA { get; set; }
        public DateTime DFOP_DT_LANC_PERIODO { get; set; }
        public bool DFOP_13 { get; set; }
        public bool DFOP_ATIVO { get; set; }
        public int DFOP_MATRICULA { get; set; }
        public int DFOP_MATRICULA_CHEFIA { get; set; }
        public int USUA_ID { get; set; }
        public int DFOP_ID { get; set; }
        public int DFOP_QTDE_DIAS { get; set; }
        public int DFOP_MESES { get; set; }
        public int DFOP_SALDO { get; set; }
        public int DFOP_DIAS_GOZADO { get; set; }
        public int DFOP_DIAS { get; set; }
        public int DFOP_ABONO { get; set; }
        public int DFOP_CODIGO_EMPRESA { get; set; }
        public int DFOP_CODIGO_ESTABELECIMENTO { get; set; }
        public int? TPPG_ID { get; set; }
        public int DFOP_CODIGO_CENTRO_RESULTADO { get; set; }
        public string DFOP_NOME_EMPRESA { get; set; }
        public string DFOP_NOME_ESTABELECIMENTO { get; set; }
        public string DFOP_PERIODO_AQUISITIVO { get; set; }
        public string DFOP_NOME_RESULTADO { get; set; }
        public string DFOP_ESCALA_TURMA { get; set; }
        public string DFOP_SITUACAO { get; set; }
        public string DFOP_NOME { get; set; }
        public string DFOP_NOME_CHEFIA { get; set; }
        public string DFOP_FERIAS_COMPULSORIAS { get; set; }
    }
}