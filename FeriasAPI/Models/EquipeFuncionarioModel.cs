using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriasAPI.Models
{
    public class EquipeFuncionarioModel
    {
        public int Registro { get; set; }
        public int FUNC_ID { get; set; } 
        public string Descricao { get; set; }
        public int OperAdm { get; set; }
    }
}