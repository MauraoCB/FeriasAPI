using ECRUD;
using FeriasAPI.Helpers;
using FeriasAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Enums = FeriasAPI.Models.Enums;

namespace FeriasAPI.Repository
{
    public class FuncionarioRepository:RepositoryBase
    {
        public static FuncionarioModel GetFuncionario(string funcRegistro)
        {
            FuncionarioModel funcionario = new FuncionarioModel();

            try
            {
                using (var Oracle = new ECRUD.Oracle())
                {                   
                    QueryData queryData = new QueryData();

                    queryData = ReturnSqlQuery("Telemat.V_FUNCIONARIOS_ALL", Enums.OperationDML.SELECT, $"FUNC_REGISTRO = '{funcRegistro}'", "", "", funcionario);

                    DataTable tableFuncionario = Oracle.GetDataTableWithException(queryData.SqlStatment, Enums.Bancos.Telemat, out strException);

                    funcionario = ConvertDataTable<FuncionarioModel>(tableFuncionario).FirstOrDefault();

                    return funcionario;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal static List<FuncionarioModel> GetGestores()
        {
            using (var oracle = new ECRUD.Oracle())
            {
                string sqlStatment = "SELECT DISTINCT FUNC_REGISTRO, FUNC_NOME  FROM TELEMAT.V_FUNCIONARIOS_ALL WHERE FUNC_ATIVO = 1 AND FUNC_REGISTRO IN (SELECT FUNC_GESTOR FROM TELEMAT.V_FUNCIONARIOS_ALL) ORDER BY FUNC_NOME";
                DataTable tableGestores = oracle.GetDataTableWithException(sqlStatment, Enums.Bancos.Telemat, out strException);

                var gestores = ConvertDataTable<FuncionarioModel>(tableGestores);
                return gestores;
            }
        }
        internal static List<FuncionarioModel> GetFuncionariosByGestor(int gestor)
        {
            try
            {
                object[,] parameters = { { "FUNC_GESTOR", gestor } };
                if (gestor == 0)
                {
                    parameters = null;
                }
                return GetFuncionarios(parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static List<EquipeFuncionarioModel> GetEquipeFuncionarios()
        {
            List<EquipeFuncionarioModel> equipeFuncionarios = new List<EquipeFuncionarioModel>  ();
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(" SELECT DISTINCT S.Registro, F.FUNC_ID, E.EQUI_DESCRICAO AS Descricao, E.OPER_ADM AS OperAdm");
            sbQuery.Append(" FROM ESCALA.SUB_EQUIPES@CTIS_PROD S, TELEMAT.V_FUNCIONARIOS_ALL F, ESCALA.MV_EQUIPES_ALL E");
            sbQuery.Append(" WHERE S.FUNC_ID = F.FUNC_ID AND S.Registro = E.Registro");
            sbQuery.Append(" UNION ALL");
            sbQuery.Append(" SELECT DISTINCT S.Registro, F.FUNC_ID, E.EQUI_DESCRICAO AS Descricao, E.OPER_ADM AS OperAdm");
            sbQuery.Append(" FROM ESCALA.SUB_EQUIPES@CONVICON_prod S, TELEMAT.V_FUNCIONARIOS_ALL F, ESCALA.MV_EQUIPES_ALL E");
            sbQuery.Append(" WHERE S.FUNC_ID = F.FUNC_ID AND S.Registro = E.Registro");
            sbQuery.Append(" UNION ALL");
            sbQuery.Append(" SELECT DISTINCT S.Registro, F.FUNC_ID, E.EQUI_DESCRICAO AS Descricao, E.OPER_ADM AS OperAdm");
            sbQuery.Append(" FROM ESCALA.SUB_EQUIPES@PRODALE_PROD.SANTOSBRASIL.COM.BR S, TELEMAT.V_FUNCIONARIOS_ALL F, ESCALA.MV_EQUIPES_ALL E");
            sbQuery.Append(" WHERE S.FUNC_ID = F.FUNC_ID AND S.Registro = E.Registro");
            sbQuery.Append(" UNION ALL");
            sbQuery.Append(" SELECT DISTINCT S.Registro, F.FUNC_ID, E.EQUI_DESCRICAO AS Descricao, E.OPER_ADM AS OperAdm");
            sbQuery.Append(" FROM ESCALA.SUB_EQUIPES@IMBITUBA_PROD.SANTOSBRASIL.COM.BR S, TELEMAT.V_FUNCIONARIOS_ALL F, ESCALA.MV_EQUIPES_ALL E");
            sbQuery.Append(" WHERE S.FUNC_ID = F.FUNC_ID AND S.Registro = E.Registro");


            var dtRetorno = GetDataTable(sbQuery.ToString(), Enums.Bancos.Escala);

            equipeFuncionarios = ConvertDataTable<EquipeFuncionarioModel>(dtRetorno);
            return equipeFuncionarios;
        }



        internal static List<ListItem> GetCentroCusto()
        {
            var CentroCustoList = GetFuncionarios(null).GroupBy(c => new {Value = c.FUNC_CC.ToString(), Text = c.FUNC_CC_NOME }).Select(g => g.First()).ToList();

            List<ListItem> listRetorno = CentroCustoList.Select(l => new ListItem { Value = l.FUNC_CC.ToString(), Text = l.FUNC_CC_NOME }).ToList();
            listRetorno.Insert(0, new ListItem { Value = "0", Text = "(Selecione)" });

            return listRetorno;
        }

        internal static List<ListItem> GetCargos()
        {
            var CargosList = GetFuncionarios(null).GroupBy(c => new { Value = c.FUNC_COD_CARGO.ToString(), Text = c.FUNC_COD_CARGO }).Select(g => g.First()).ToList();

            List<ListItem> listRetorno = CargosList.Select(l => new ListItem { Value = l.FUNC_COD_CARGO.ToString(), Text = l.FUNC_CARGO }).ToList();
            listRetorno.Insert(0, new ListItem { Value = "0", Text = "(Selecione)" });

            return listRetorno;
        }

        internal static List<ListItem> GetEquipes()
        {
            try
            {
                using (var Oracle = new ECRUD.Oracle())
                {
                    string sql = "SELECT DISTINCT EQUI_DESCRICAO FROM Escala.MV_EQUIPES_ALL ORDER BY EQUI_DESCRICAO";
                

                    DataTable tableEquipes = Oracle.GetDataTableWithException(sql, Enums.Bancos.Escala, out strException);

                    var distinctRows = (from DataRow dRow in tableEquipes.Rows
                                        select dRow["EQUI_DESCRICAO"]).Distinct();

                    List<ListItem> listRetorno = new List<ListItem>();

                    foreach (var row in distinctRows)
                    {
                        listRetorno.Add(new ListItem { Value = row.ToString(), Text =row.ToString() });
                    };

                    listRetorno.Insert(0, new ListItem { Value = "0", Text = "(Selecione)" });
                    return listRetorno;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<FuncionarioModel> GetFuncionariosByCentroCusto(int centroCusto)
        {            
            try
            {
                object[,] parameters = { { "FUNC_CC", centroCusto} };
                return GetFuncionarios(parameters, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<FuncionarioModel> GetFuncionariosByCodigoCargo(int codigoCargo)
        {
            try
            {
                object[,] parameters = { { "FUNC_COD_CARGO", codigoCargo } };
                return GetFuncionarios(parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static List<FuncionarioModel> GetFuncionarios(object[,] Params, bool useLike = false)
        {
            using (var Oracle = new ECRUD.Oracle())
            {                
                QueryData queryData = new QueryData();

                string whereClause = "" ;

                if (Params != null)
                {
                    whereClause = $"{Params[0, 0]} = '{Params[0, 1]}'";
                    if (useLike)
                    {
                        whereClause = $"{Params[0, 0]} LIKE '%{Params[0, 1]}%'";
                    } 
                }

                queryData = ReturnSqlQuery("Telemat.V_FUNCIONARIOS_ALL", Enums.OperationDML.SELECT, whereClause, "", "", new FuncionarioModel());
                                
                var funcionariosRetorno = ConvertDataReader<FuncionarioModel>(Oracle.GetDataReaderWithException(queryData.SqlStatment, Enums.Bancos.Telemat, out strException));         

                return funcionariosRetorno;
            }
        }
    }
}