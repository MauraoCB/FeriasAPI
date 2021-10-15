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
    public class FeriasDelegaRepository: RepositoryBase
    {
        public static List<FeriasDelegaModel> GetFeriasDelega(int gestor)
        {
            List<FeriasDelegaModel> ferias = new List<FeriasDelegaModel>();
            QueryData queryData = new QueryData();

            string whereClause = "";

            if (gestor !=0)
            {
                whereClause = $"FUNC_GESTOR = {gestor}";
            }

            queryData = ReturnSqlQuery("Escala.Ferias_Delega", Enums.OperationDML.SELECT, whereClause, "", "", new FeriasDelegaModel());
            DataTable returnTable = GetDataTable(queryData.SqlStatment, Enums.Bancos.Escala);

            ferias = ConvertDataTable<FeriasDelegaModel>(returnTable);

            return ferias;
        }

        internal static void SaveFeriasDelegaList(List<FeriasDelegaModel> feriasDelegaList)
        {
            foreach (var item in feriasDelegaList)
            {
                SaveFeriasDelega(item);
            }
        }

        public static bool SaveFeriasDelega(FeriasDelegaModel feriasDelega)
        {
            try
            {
                StringBuilder sbQuery = new StringBuilder();

                QueryData queryData = new QueryData();
                if (feriasDelega.FRDL_ID != 0)
                {
                    queryData = ReturnSqlQuery("ESCALA.Ferias_Delega", Enums.OperationDML.UPDATE, $"FRDL_ID = {feriasDelega.FRDL_ID} ", "FRDL_ID", "FRDL_ID", feriasDelega);
                }
                else
                {
                    //Verifica se já tem delegação ativa para esse delegado / funcionário
                    string sqlStatment = $"SELECT FRDL_ID FROM ESCALA.Ferias_Delega WHERE FRDL_ATIVO = 1 AND FUNC_REGISTRO_DELEGADO = {feriasDelega.FUNC_REGISTRO_DELEGADO} AND FUNC_REGISTRO_FILHO = {feriasDelega.FUNC_REGISTRO_FILHO}";
                    DataTable dt = GetDataTable(sqlStatment, Enums.Bancos.Escala); 

                    if (dt.Rows.Count >0)
                    {
                        return true;
                    }

                    feriasDelega.FRDL_CRIACAO = DateTime.Now;
                    queryData = ReturnSqlQuery("ESCALA.Ferias_Delega", Enums.OperationDML.INSERT, "", "FRDL_ID", "FRDL_ID", feriasDelega);
                }
                ExecuteInsertUpdate(Enums.Bancos.Escala, queryData);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}