using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriasAPI.Models
{
    public class Enums
    {
        public enum Bancos
        {
            Escala,
            Telemat
        }

        public enum OperationDML
        {
            INSERT,
            UPDATE,
            SELECT,
            DELETE
        }
    }
}