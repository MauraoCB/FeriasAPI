using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriasAPI.Models
{
    public class EmpresaFilialModel
    {
        public int EMPR_ID { get; set; }
        public string EMPR_DESC { get; set; }
        public int FILI_ID { get; set; }
        public string FILI_DESC { get; set; }
    }
}