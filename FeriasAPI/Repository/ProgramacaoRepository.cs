using ECRUD;
using FeriasAPI.Helpers;
using FeriasAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Enums = FeriasAPI.Models.Enums;

namespace FeriasAPI.Repository
{
    public class ProgramacaoRepository: RepositoryBase
    {
        public static bool SaveImportedSheet(List<ProgramacaoModel> programacao)
        {
            try
            {

                foreach (var item in programacao)
                {

                    StringBuilder sbQuery = new StringBuilder();

                    sbQuery.Append(" SELECT PGFR_ID, TPPG_ID, PGFR_13, PGFR_PROGRAMACAO_1, PGFR_PROGRAMACAO_2 ");
                    sbQuery.Append("   FROM Escala.PROGRAMACAO_FERIAS ");
                    sbQuery.Append("  WHERE PGFR_MATRICULA = :PGFR_MATRICULA ");
                    sbQuery.Append("    AND PGFR_PERIODO_AQUISITIVO = :PGFR_PERIODO_AQUISITIVO ");

                    object[,] Params =
                    {
                        { ":PGFR_MATRICULA", item.PGFR_MATRICULA },
                        { ":PGFR_PERIODO_AQUISITIVO", item.PGFR_PERIODO_AQUISITIVO }
                        };

                    item.PGFR_ULT_LEITURA = DateTime.Now;
                    item.PGFR_DT_LEITURA = DateTime.Now;

                    DataTable dt = GetDataTable(sbQuery.ToString(), Enums.Bancos.Escala, Params);
                    QueryData queryData = new QueryData();
                    if (dt.Rows.Count != 0)
                    {
                        item.PGFR_ID = Convert.ToInt16(dt.Rows[0]["PGFR_ID"]);
                        if (dt.Rows[0]["TPPG_ID"].GetType().Name != "DBNull")
                        {
                            item.TPPG_ID = Convert.ToInt32(dt.Rows[0]["TPPG_ID"]);
                        }

                          if (dt.Rows[0]["PGFR_13"].ToString() != "0")
                          {
                              item.PGFR_13 = Convert.ToInt32(dt.Rows[0]["PGFR_13"]);
                          }
                                           
                        if  (Convert.ToDateTime( dt.Rows[0]["PGFR_PROGRAMACAO_1"]) != DateTime.MinValue)
                        {
                            item.PGFR_PROGRAMACAO_1 = Convert.ToDateTime(dt.Rows[0]["PGFR_PROGRAMACAO_1"]);
                        }

                        if (Convert.ToDateTime(dt.Rows[0]["PGFR_PROGRAMACAO_2"]) != DateTime.MinValue)
                        {
                            item.PGFR_PROGRAMACAO_2 = Convert.ToDateTime(dt.Rows[0]["PGFR_PROGRAMACAO_2"]);
                        }
                        //
                        queryData = ReturnSqlQuery("ESCALA.PROGRAMACAO_FERIAS", Enums.OperationDML.UPDATE, $"PGFR_MATRICULA = {item.PGFR_MATRICULA} AND PGFR_PERIODO_AQUISITIVO = '{item.PGFR_PERIODO_AQUISITIVO}'", "PGFR_ID,PGFR_DIAS_1,PGFR_DIAS_2,PGFR_ABONO,PGFR_DT_LEITURA", "", item);
                    }
                    else
                    {
                        //30 DIAS CONSECUTIVOS ou 20 DIAS COM 10 DE ABONO
                        if (item.TPPG_ID <= 2 && item.PGFR_13 > 0)
                        {
                            item.PGFR_13 = 1;
                        }
                        queryData = ReturnSqlQuery("ESCALA.PROGRAMACAO_FERIAS", Enums.OperationDML.INSERT, "", "PGFR_ID", "", item);
                    }

                    ExecuteInsertUpdate(Enums.Bancos.Escala, queryData);

                    //Replica a leitura para a tabela DISTRIBUCAO_FERIAS_OPER
                    sbQuery.Clear();
                    sbQuery.Append(" SELECT DFOP_ID");
                    sbQuery.Append("   FROM Escala.DISTRIBUICAO_FERIAS_OPER ");
                    sbQuery.Append($"  WHERE DFOP_MATRICULA = {item.PGFR_MATRICULA} ");
                    sbQuery.Append($"    AND DFOP_PERIODO_AQUISITIVO = '{item.PGFR_PERIODO_AQUISITIVO}' ");

                    dt = GetDataTable(sbQuery.ToString(), Enums.Bancos.Escala);
                    if (dt.Rows.Count > 0)
                    {
                        string dfopId = dt.Rows[0]["DFOP_ID"].ToString();

                        queryData = new QueryData() { SqlStatment = $"UPDATE Escala.DISTRIBUICAO_FERIAS_OPER SET DFOP_ULT_LEITURA = TO_DATE(sysdate) WHERE DFOP_ID = {dfopId} " };

                    }
                    else
                    {
                        var distribuicaoFerias = LoadDistribucaoFeriasOper(item);

                        queryData = ReturnSqlQuery("ESCALA.DISTRIBUICAO_FERIAS_OPER", Enums.OperationDML.INSERT, "", "DFOP_ID", "DFOP_ID", distribuicaoFerias);
                    }

                    ExecuteInsertUpdate(Enums.Bancos.Escala, queryData);
                }
                 
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal static void AprovarProgramacoes(List<dynamic> progamacoesAprovar)
        {
            QueryData queryData = new QueryData();
            foreach (var item in progamacoesAprovar)
            {
                queryData.SqlStatment = $"UPDATE Escala.PROGRAMACAO_FERIAS SET PGFR_AUTORIZADO = {item.Autorizado}, PGFR_STATUS = '{item.Status}', PGFR_MOTIVO_STATUS = '{item.MotivoStatus}' WHERE PGFR_ID = {item.PGFR_ID}";
                ExecuteInsertUpdate(Enums.Bancos.Escala, queryData);
            }
        }

        internal static void SaveProgramacao(ProgramacaoModel programacao)
        {
            try
            {
                programacao.PGFR_DT_LANC_PERIODO = DateTime.Now;
                QueryData queryData = new QueryData();
                queryData = ReturnSqlQuery("ESCALA.PROGRAMACAO_FERIAS", Enums.OperationDML.UPDATE, $"PGFR_ID = {programacao.PGFR_ID} ", "PGFR_ID", "", programacao);
                    
                    ExecuteInsertUpdate(Enums.Bancos.Escala, queryData);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<ProgramacaoModel> GetProgramacaoFerias(object[,] parameters)
        {
            using (var Oracle = new ECRUD.Oracle())
            {
                QueryData queryData = new QueryData();

                string whereClause = "";

                if (parameters != null)
                {
                    whereClause = " WHERE ";
                    for (int i = 0; i < parameters.Length / 2; i++)
                    {
                        if (parameters[i, 0] != null)
                        {
                            whereClause += $" {parameters[i, 0]} = '{parameters[i, 1]} AND";

                        }
                    }

                    whereClause = whereClause.Remove(whereClause.Length - 3);
                }

                try
                {
                    queryData = ReturnSqlQuery("Escala.PROGRAMACAO_FERIAS", Enums.OperationDML.SELECT, whereClause, "", "", new ProgramacaoModel());

                    var programacaoFeriasRetorno = ConvertDataReader<ProgramacaoModel>(Oracle.GetDataReaderWithException(queryData.SqlStatment, Enums.Bancos.Escala, out strException));
                    return programacaoFeriasRetorno;
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }

        private static DistribuicaoFeriasOperModel LoadDistribucaoFeriasOper(ProgramacaoModel programacao)
        {
            DistribuicaoFeriasOperModel distribuicaoFeriasOper = new DistribuicaoFeriasOperModel();

            distribuicaoFeriasOper.DFOP_13 = programacao.PGFR_13 != 0;
            distribuicaoFeriasOper.DFOP_ABONO = programacao.PGFR_ABONO;
            distribuicaoFeriasOper.DFOP_ADMISSAO = programacao.PGFR_ADMISSAO;
            distribuicaoFeriasOper.DFOP_ATIVO = programacao.PGFR_ATIVO;
            distribuicaoFeriasOper.DFOP_CODIGO_CENTRO_RESULTADO = programacao.PGFR_CODIGO_CENTRO_RESULTADO;
            distribuicaoFeriasOper.DFOP_CODIGO_EMPRESA = programacao.PGFR_CODIGO_EMPRESA;
            distribuicaoFeriasOper.DFOP_CODIGO_ESTABELECIMENTO = programacao.PGFR_CODIGO_ESTABELECIMENTO;
            distribuicaoFeriasOper.DFOP_DATA_LIMITE = programacao.PGFR_DATA_LIMITE;
            distribuicaoFeriasOper.DFOP_DIAS_GOZADO = programacao.PGFR_DIAS_GOZADO;
            distribuicaoFeriasOper.DFOP_DT_LANC_PERIODO = programacao.PGFR_DT_LANC_PERIODO;
            distribuicaoFeriasOper.DFOP_DT_LEITURA = programacao.PGFR_DT_LEITURA;
            distribuicaoFeriasOper.DFOP_ESCALA_TURMA = programacao.PGFR_ESCALA_TURMA;
            distribuicaoFeriasOper.DFOP_FERIAS_COMPULSORIAS = programacao.PGFR_FERIAS_COMPULSORIAS;
            distribuicaoFeriasOper.DFOP_MATRICULA = programacao.PGFR_MATRICULA;
            distribuicaoFeriasOper.DFOP_MATRICULA_CHEFIA = programacao.PGFR_MATRICULA_CHEFIA;
            distribuicaoFeriasOper.DFOP_MESES = programacao.PGFR_MESES;
            distribuicaoFeriasOper.DFOP_NOME = programacao.PGFR_NOME;
            distribuicaoFeriasOper.DFOP_NOME_CHEFIA = programacao.PGFR_NOME_CHEFIA;
            distribuicaoFeriasOper.DFOP_NOME_EMPRESA = programacao.PGFR_NOME_EMPRESA;
            distribuicaoFeriasOper.DFOP_NOME_ESTABELECIMENTO = programacao.PGFR_NOME_ESTABELECIMENTO;
            distribuicaoFeriasOper.DFOP_NOME_RESULTADO = programacao.PGFR_NOME_RESULTADO;
            distribuicaoFeriasOper.DFOP_PERIODO_AQUISITIVO = programacao.PGFR_PERIODO_AQUISITIVO;
            distribuicaoFeriasOper.DFOP_PROGRAMACAO = programacao.PGFR_PROGRAMACAO_1;
            distribuicaoFeriasOper.DFOP_QTDE_DIAS = programacao.PGFR_QTDE_DIAS;
            distribuicaoFeriasOper.DFOP_SALDO = programacao.PGFR_SALDO;
            distribuicaoFeriasOper.DFOP_SITUACAO = programacao.PGFR_SITUACAO;
            distribuicaoFeriasOper.DFOP_ULT_LEITURA = programacao.PGFR_ULT_LEITURA;
            distribuicaoFeriasOper.TPPG_ID = programacao.TPPG_ID;
            distribuicaoFeriasOper.USUA_ID = programacao.USUA_ID;

            return distribuicaoFeriasOper;
        }
    }
}