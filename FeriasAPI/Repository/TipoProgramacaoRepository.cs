using FeriasAPI.Helpers;
using FeriasAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace FeriasAPI.Repository
{
    public class TipoProgramacaoRepository: RepositoryBase
    {
        public static List<TipoProgramacaoModel> GetTipoProgramacao(int tipoProgramacaoId = 0)
        {
            List<TipoProgramacaoModel> tiposProgramacao = new List<TipoProgramacaoModel>();

            using (var Oracle = new ECRUD.Oracle())
            {
                QueryData queryData = new QueryData();

                string whereClause = "";

                if (tipoProgramacaoId != 0)
                {
                    whereClause = $"TPPG_ID = {tipoProgramacaoId}";
                }

                queryData = ReturnSqlQuery("Escala.Tipo_Programacao", Enums.OperationDML.SELECT, whereClause, "", "", new TipoProgramacaoModel());

                DataTable returnTable = Oracle.GetDataTableWithException(queryData.SqlStatment, Enums.Bancos.Escala, out strException);
            
                tiposProgramacao = ConvertDataTable<TipoProgramacaoModel>(returnTable);
                
                return tiposProgramacao;
            }
        }

        public static bool SaveTipoProgramacao(TipoProgramacaoModel tipoProgramacao)
        {
            try
            {
                using (var Oracle = new ECRUD.Oracle())
                {
                    StringBuilder sbQuery = new StringBuilder();

                    QueryData queryData = new QueryData();
                    if (tipoProgramacao.TPPG_ID != 0)
                    {
                        queryData = ReturnSqlQuery("ESCALA.TIPO_PROGRAMACAO", Enums.OperationDML.UPDATE, $"TPPG_ID = {tipoProgramacao.TPPG_ID}", "TPPG_ID", "", tipoProgramacao);
                    }
                    else
                    {
                        queryData = ReturnSqlQuery("ESCALA.TIPO_PROGRAMACAO", Enums.OperationDML.INSERT, "", "TPPG_ID", "TPPG_ID", tipoProgramacao);
                    }
                    //Oracle.InsertUpdateDeleteBind(queryData.SqlStatment, Enums.Bancos.Escala, queryData.Parameters);

                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
            
        public static List<TipoVisualizacaoModel> GetTipoVisualizacao()
        {
            List<TipoVisualizacaoModel> tipoVisualizacao = new List<TipoVisualizacaoModel>();

            using (var Oracle = new ECRUD.Oracle())
            {
                QueryData queryData = new QueryData();

                queryData = ReturnSqlQuery("Escala.Tipo_Visualizacao", Enums.OperationDML.SELECT, "", "", "", new TipoVisualizacaoModel());

                DataTable returnTable = Oracle.GetDataTable(queryData.SqlStatment, Enums.Bancos.Escala);

                tipoVisualizacao = ConvertDataTable<TipoVisualizacaoModel>(returnTable);

                return tipoVisualizacao;
            }
        }

    }
}