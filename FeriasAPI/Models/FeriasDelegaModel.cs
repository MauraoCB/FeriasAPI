using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriasAPI.Models
{
    public class FeriasDelegaModel
    {
        public int FRDL_ID { get; set; }
        public int FUNC_REGISTRO_DELEGADO { get; set; }
        public int FUNC_REGISTRO_FILHO { get; set; }
        public int FUNC_GESTOR { get; set; }
        public DateTime FRDL_CRIACAO { get; set; }
        public int  FRDL_ATIVO { get; set; }
        public DateTime FRDL_CANCEL { get; set; }
        public int FRDL_FLOW_GESTOR { get; set; }
    }
}