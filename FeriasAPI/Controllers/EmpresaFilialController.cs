using FeriasAPI.Models;
using FeriasAPI.Repository;
using System.Collections.Generic;

using System.Web.Http;

namespace FeriasAPI.Controllers
{
    [RoutePrefix("api/EmpresaFilial")]
    public class EmpresaFilialController : ApiController
    {
        [HttpGet]
        [Route("GetEmpresas")]
        public List<EmpresaFilialModel> GetEmpresas()
        {
            return EmpresaFilialRepository.GetEmpresas();
        }

        [HttpGet]
        [Route("GetUnidades/{empresaId}")]
        public List<EmpresaFilialModel> GetUnidades(int empresaId)
        {
            return EmpresaFilialRepository.GetUnidades(empresaId);
        }
    }
}