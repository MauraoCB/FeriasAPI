using FeriasAPI.Helpers;
using FeriasAPI.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using static FeriasAPI.Models.Enums;

namespace FeriasAPI.Repository
{
    public class RepositoryBase 
    {
        protected static string strException = "";
        protected static OracleDataReader reader;
        /// <summary>
        /// Retorna uma string SQL e os parâmetros para inserção, atualização, seleção e exclusão em uma determinada tabela
        /// </summary>
        /// <param name="tableName">Nome da tabela que sofrerá a ação</param>
        /// <param name="operationDML">Valores válidos I, U, S, D</param>
        /// <param name="objectModel">Objeto de classe cujas propriedades tenham o mesmo nome e tipo das respectivas colunas da tabela recebida no parâmetro tableName </param>
        /// <returns>Instrução SQL pronta para ser executada pela classe que este método</returns>
        public static QueryData ReturnSqlQuery(string tableName, OperationDML operationDML, string whereClause, string excludColumns, string identityColumn, object objectModel)
        {
            try
            {
                QueryData queryData = new QueryData();
                object[,] Params = { };

                StringBuilder sqlStatement = new StringBuilder();
                StringBuilder values = new StringBuilder();

                var fields = objectModel.GetType();

                Params = new object[fields.GetProperties().Length, 2];

                int index = 0;
                switch (operationDML)
                {
                    case OperationDML.INSERT:
                        if (identityColumn !="")
                        {
                            identityColumn += ",";
                        }

                        sqlStatement.Append($"INSERT INTO {tableName} ({identityColumn}");
                        foreach (var property in fields.GetProperties())
                        {
                            string columnName = property.Name;

                            if (excludColumns != "" && excludColumns.Contains(columnName))
                            {
                                continue;
                            }
                            sqlStatement.Append($"{columnName},");

                            Params[index, 0] = $":{columnName}";


                            if (property.PropertyType.Name == "DateTime")
                            {
                                values.Append($"TO_DATE(:{columnName}, 'YYYY-MM-DD'),");
                                Params[index, 1] = String.Format("{0:yyyy-MM-dd}", property.GetValue(objectModel));
                            }
                            else
                            {
                                values.Append($":{columnName},");
                                Params[index, 1] = property.GetValue(objectModel);

                                if (property.PropertyType.Name == "Boolean")
                                {
                                    Params[index, 1] = 0;
                                    if (Convert.ToBoolean(property.GetValue(objectModel)))
                                    {
                                        Params[index, 1] = 1;
                                    }
                                      
                                }
                            }

                            index++;
                        }
                        sqlStatement[sqlStatement.Length - 1] = Convert.ToChar(')');
                        values[values.Length - 1] = Convert.ToChar(')');

                        string idendtityNewValue = "";
                        if (identityColumn != "")
                        {
                            idendtityNewValue = GetNextIdentyValue (tableName, identityColumn.Replace(",", "")) + ",";
                        }

                        sqlStatement.Append($" VALUES({idendtityNewValue}");
                        sqlStatement.Append(values.ToString());

                        break;
                    case OperationDML.UPDATE:
                        sqlStatement.Append($"UPDATE {tableName} SET ");
                        foreach (var property in fields.GetProperties())
                        {
                            string columnName = property.Name;

                            if (excludColumns != "" && excludColumns.Contains(columnName))
                            {
                                continue;
                            }

                            

                            Params[index, 0] = $":{columnName}";


                            if (property.PropertyType.Name == "DateTime")
                            {
                                sqlStatement.Append($"{columnName} = TO_DATE(:{columnName}, 'YYYY-MM-DD'),");
                                Params[index, 1] = String.Format("{0:yyyy-MM-dd}", property.GetValue(objectModel));
                            }
                            else
                            {
                                sqlStatement.Append($"{columnName} = :{columnName},");
                                Params[index, 1] = property.GetValue(objectModel);

                                if (property.PropertyType.Name == "Boolean")
                                {
                                    Params[index, 1] = 0;
                                    if (Convert.ToBoolean(property.GetValue(objectModel)))
                                    {
                                        Params[index, 1] = 1;
                                    }

                                }
                            }

                            index++;
                        }
                        //Remove last comma
                        sqlStatement.Remove(sqlStatement.Length - 1, 1);

                        if (whereClause != "")
                        {
                            sqlStatement.Append($" WHERE {whereClause}");
                        }
                        break;
                    case OperationDML.SELECT:
                        sqlStatement.Append("SELECT ");
                        foreach (var property in fields.GetProperties())
                        {
                            sqlStatement.Append($"{property.Name},");
                        }

                        //Remove last comma
                        sqlStatement.Remove(sqlStatement.Length - 1, 1);

                        sqlStatement.Append($" FROM {tableName} ");
                        if (whereClause != "")
                        {
                            sqlStatement.Append($" WHERE {whereClause}");
                        }
                        break;
                    default:
                        break;
                }

                queryData.SqlStatment = sqlStatement.ToString();
                queryData.Parameters = Params;

                return queryData;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static List<T> ConvertDataReader<T>(OracleDataReader dr)
        {
            List<T> data = new List<T>();

            Type temp = typeof(T);


            while (dr.Read())
            {
                T obj = Activator.CreateInstance<T>();

                foreach (var prop in temp.GetProperties())
                {
                    if (dr[prop.Name].GetType().Name != "DBNull")
                    {
                        //   prop.SetValue(obj, Convert.ChangeType(dr[prop.Name], prop.PropertyType), null); 
                        if (prop.PropertyType.FullName.ToLower().Contains("nullable"))    
                        {
                            prop.SetValue(obj, dr[prop.Name]);
                        }
                        else
                        {
                            prop.SetValue(obj, Convert.ChangeType(dr[prop.Name], prop.PropertyType), null);
                        }
                    }
                }

                data.Add(obj);
            }

            return data;
        }

        public static void ExecuteInsertUpdate(Enums.Bancos banco, QueryData queryData)
        {
            using (OracleConnection connection = new OracleConnection(GetConnectionString(banco)))
            {
                connection.Open();

                OracleCommand cmd = new OracleCommand(queryData.SqlStatment, connection);              

                if (queryData.Parameters != null)
                {
                    for (int i = 0; i < queryData.Parameters.Length / 2; i++)
                    {
                        if (queryData.Parameters[i, 0] != null)
                        {
                            cmd.Parameters.Add(new OracleParameter(queryData.Parameters[i, 0].ToString(), queryData.Parameters[i, 1]));
                        }
                    } 
                }
                try
                {
                    cmd.Transaction = connection.BeginTransaction();
                    cmd.ExecuteNonQuery();
                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {

                    cmd.Transaction.Rollback();
                }
                connection.Close();
                connection.Dispose();
                GC.SuppressFinalize(connection);
            }
        }

        public static DataTable GetDataTable(string sqlStatment, Enums.Bancos banco, object[,] parameters)
        {
            using (OracleConnection connection = new OracleConnection(GetConnectionString(banco)))
            {
                DataTable dtResult = new DataTable();
                connection.Open();

                OracleCommand cmd = new OracleCommand(sqlStatment, connection);
               
                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length / 2; i++)
                    {
                        if (parameters[i, 0] != null)
                        {
                            cmd.Parameters.Add(new OracleParameter(parameters[i, 0].ToString(), parameters[i, 1]));
                        }
                    }
                }
                try
                {
                    using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
                    {
                        dataAdapter.SelectCommand = cmd;
                        dataAdapter.Fill(dtResult);
                    }
                }
                catch (Exception ex)
                {

                    
                }
                connection.Close();
                connection.Dispose();
                GC.SuppressFinalize(connection);
                return dtResult;
            }
        }
        public static DataTable GetDataTable(string sqlStatment, Enums.Bancos banco)
        {

            using (OracleConnection connection = new OracleConnection(GetConnectionString(banco)))
            {
                DataTable dtResult = new DataTable();
                connection.Open();

                OracleCommand cmd = new OracleCommand(sqlStatment, connection);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    object[] arrayValues = new object[reader.FieldCount];

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (!dtResult.Columns.Contains(reader.GetName(i)))
                        {
                            DataColumn column = new DataColumn() { ColumnName = reader.GetName(i), DataType = reader[i].GetType() };
                            dtResult.Columns.Add(column); 
                        }

                       arrayValues[i] = reader[i];
                    }
                    dtResult.LoadDataRow(arrayValues, true);

                }

                return dtResult;
            }
        }

        #region Private methods
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (var pro in temp.GetProperties())
                {
                    if (pro.Name.ToUpper() == column.ColumnName.ToUpper() && dr[column.ColumnName].GetType().Name != "DBNull")
                        pro.SetValue(obj, Convert.ChangeType(dr[column.ColumnName], pro.PropertyType), null);
                    else
                        continue;
                }
            }
            return obj;
        } 

        private static string GetConnectionString(Enums.Bancos banco)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[banco.ToString()].ConnectionString;
        }

        private static string GetNextIdentyValue(string tableName, string identityColumn)
        {
            string newValue = "0";
            using (var oracle = new ECRUD.Oracle())
            {

                OracleDataReader reader = oracle.GetDataReaderWithException($"SELECT nvl(max({identityColumn}), 0) + 1 as NewValue FROM {tableName}", Enums.Bancos.Escala, out strException);

                if (reader.Read())
                {
                    newValue =reader["NewValue"].ToString();
                }

                reader.Dispose();
                oracle.Dispose();
            }

            return newValue;
        }
        #endregion
    }
}