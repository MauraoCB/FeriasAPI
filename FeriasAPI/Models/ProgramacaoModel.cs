using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriasAPI.Models
{
    public class ProgramacaoModel
    {
        public int PGFR_ID { get; set; }
        public int PGFR_CODIGO_EMPRESA { get; set; }
        public string PGFR_NOME_EMPRESA { get; set; }
        public int PGFR_CODIGO_ESTABELECIMENTO { get; set; }
        public string PGFR_NOME_ESTABELECIMENTO { get; set; }
        public int PGFR_MATRICULA { get; set; }
        public string PGFR_NOME { get; set; }
        public string PGFR_ESCALA_TURMA { get; set; }
        public DateTime PGFR_ADMISSAO { get; set; }
        public string PGFR_SITUACAO { get; set; }
        public string PGFR_PERIODO_AQUISITIVO { get; set; }
        public DateTime PGFR_DATA_LIMITE { get; set; }
        public int PGFR_QTDE_DIAS { get; set; }
        public int PGFR_MESES { get; set; }
        public int PGFR_SALDO { get; set; }
        public int PGFR_DIAS_GOZADO { get; set; }
        public DateTime PGFR_PROGRAMACAO_1 { get; set; }
        public int PGFR_DIAS_1 { get; set; }
        public DateTime PGFR_PROGRAMACAO_2 { get; set; }
        public int PGFR_DIAS_2 { get; set; }
        public int PGFR_ABONO { get; set; }
        public int? TPPG_ID { get; set; }
        public int PGFR_13 { get; set; }
        public int PGFR_CODIGO_CENTRO_RESULTADO { get; set; }
        public string PGFR_NOME_RESULTADO { get; set; }
        public int PGFR_MATRICULA_CHEFIA { get; set; }
        public string PGFR_NOME_CHEFIA { get; set; }
        public string PGFR_FERIAS_COMPULSORIAS { get; set; }
        public DateTime PGFR_DT_LEITURA { get; set; }
        public int USUA_ID { get; set; }
        public bool PGFR_ATIVO { get; set; }

        public DateTime PGFR_ULT_LEITURA { get; set; }
        public DateTime PGFR_DT_LANC_PERIODO { get; set; }
        public bool PGFR_AUTORIZADO { get; set; }

    }
}