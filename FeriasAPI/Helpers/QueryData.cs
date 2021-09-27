using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriasAPI.Helpers
{
    /// <summary>
    /// Classe auxiliar para manipulação de registros no banco de dados, contém a instrução SQL e os parâmetros
    /// </summary>
    public class QueryData
    {
        public string SqlStatment { get; set; }
        public object[,] Parameters { get; set; }
    }
}