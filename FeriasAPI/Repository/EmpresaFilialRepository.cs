using FeriasAPI.Helpers;
using FeriasAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FeriasAPI.Repository
{
    public class EmpresaFilialRepository : RepositoryBase
    {
        public static List<EmpresaFilialModel> GetEmpresas()
        {
            List<EmpresaFilialModel> empresas = new List<EmpresaFilialModel>();
            QueryData queryData = new QueryData();

            queryData = ReturnSqlQuery("Escala.Empresa_Filial", Enums.OperationDML.SELECT, "", "", "", new EmpresaFilialModel());
            DataTable returnTable = GetDataTable(queryData.SqlStatment, Enums.Bancos.Escala);

            empresas = ConvertDataTable<EmpresaFilialModel>(returnTable);

            return empresas;
        }
        public static List<EmpresaFilialModel> GetUnidades(int empresaId)
        {
            List<EmpresaFilialModel> unidades = new List<EmpresaFilialModel>();
            QueryData queryData = new QueryData() { SqlStatment = $"SELECT FILI_ID, FILI_DESC ESCALA.Empresa_Filial WHERE EMPR_ID = {empresaId}" };

          
            DataTable returnTable = GetDataTable(queryData.SqlStatment, Enums.Bancos.Escala);

            unidades = ConvertDataTable<EmpresaFilialModel>(returnTable);

            return unidades;
        }
    }
}