using FeriasAPI.Models;
using FeriasAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace FeriasAPI.Controllers
{
    [RoutePrefix("api/Funcionario")]
    public class FuncionarioController:ApiController
    {
        [HttpGet]
        [Route("GetFuncionario/{funcRegistro}")]
        public FuncionarioModel GetFuncionario(string funcRegistro)
        {
            return FuncionarioRepository.GetFuncionario(funcRegistro);
        }

        [HttpGet]
        [Route("GetFuncionariosByCentroCusto/{funcRegistro}")]
        public List<FuncionarioModel> GetFuncionariosByCentroCusto(int centroCusto)
        {
            return FuncionarioRepository.GetFuncionariosByCentroCusto(centroCusto);
        }

        [HttpGet]
        [Route("GetFuncionariosByGestor/{gestor}")]
        public List<FuncionarioModel> GetFuncionariosByGestor(int gestor)
        {
            return FuncionarioRepository.GetFuncionariosByGestor(gestor);
        }

        [HttpGet]
        [Route("GetFuncionariosByCodigoCargo/{codigoCargo}")]
        public List<FuncionarioModel> GetFuncionariosByCodigoCargo(int codigoCargo)
        {
            return FuncionarioRepository.GetFuncionariosByCodigoCargo(codigoCargo);
        }

        [HttpGet]
        [Route("GetEquipeFuncionarios")]
        public List<EquipeFuncionarioModel> GetEquipeFuncionarios()
        {
            return FuncionarioRepository.GetEquipeFuncionarios();
        }

        [HttpGet]
        [Route("GetCentroCusto")]
        public List<ListItem> GetCentroCusto()
        {
            return FuncionarioRepository.GetCentroCusto();
        }

        [HttpGet]
        [Route("GetCargos")]
        public List<ListItem> GetCargos()
        {
            return FuncionarioRepository.GetCargos();
        }

        [HttpGet]
        [Route("GetEquipes")]
        public List<ListItem> GetEquipes()
        {
            return FuncionarioRepository.GetEquipes();
        }

        [HttpGet]
        [Route("GetGestores")]
        public List<FuncionarioModel> GetGestores()
        {
            return FuncionarioRepository.GetGestores();
        }
    }
}