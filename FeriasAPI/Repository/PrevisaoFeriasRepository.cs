using FeriasAPI.Helpers;
using FeriasAPI.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Enums = FeriasAPI.Models.Enums;

namespace FeriasAPI.Repository
{
    public class PrevisaoFeriasRepository : RepositoryBase
    {

        internal static void SavePrevisaoFerias(List<DistribuicaoFeriasOperModel> distribuicaoFerias)
        {
            foreach (var item in distribuicaoFerias)
            {
                QueryData queryData = new QueryData();
                queryData = ReturnSqlQuery("ESCALA.Distribuicao_Ferias_Oper", Enums.OperationDML.UPDATE, $"DFOP_ID = {item.DFOP_ID} ", "DFOP_ID", "DFOP_ID", item);

                ExecuteInsertUpdate(Enums.Bancos.Escala, queryData);
            }
        }

        public static List<DistribuicaoFeriasOperModel> GetPrevisaoFerias(object[,] parameters)
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
                    queryData = ReturnSqlQuery("Escala.DISTRIBUICAO_FERIAS_OPER", Enums.OperationDML.SELECT, whereClause, "", "", new DistribuicaoFeriasOperModel());

                    var distribuicaoFeriasRetorno = ConvertDataReader<DistribuicaoFeriasOperModel>(Oracle.GetDataReaderWithException(queryData.SqlStatment, Enums.Bancos.Escala, out strException));
                    return distribuicaoFeriasRetorno;
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }

        public static List<DistribuicaoFeriasOperModel> GetPrevisaoFerias(int dfopId)
        {
            object[,] parameters = { { "DFOP_ID", dfopId }};
            return GetPrevisaoFerias(parameters);
        }
        public static List<DistribuicaoFeriasOperModel> GetPrevisaoFerias(int matricula, string periodoAquisitivo)
        {
            object[,] parameters = { { "DFOP_MATRICULA", matricula }, { "DFOP_PERIODO_AQUISITIVO", periodoAquisitivo } };
            return GetPrevisaoFerias(parameters);
        }
    }
}