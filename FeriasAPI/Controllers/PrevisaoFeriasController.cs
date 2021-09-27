
using FeriasAPI.Models;
using FeriasAPI.Repository;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace FeriasAPI.Controllers
{
    [RoutePrefix("api/PrevisaoFerias")]
    public class PrevisaoFeriasController : ApiController
    {
      
        [HttpPost]
        [Route("SavePrevisaoFerias")]
        public void SavePrevisaoFerias([FromBody] List<DistribuicaoFeriasOperModel> distribuicaoFerias)
        {
            PrevisaoFeriasRepository.SavePrevisaoFerias(distribuicaoFerias);
        }

        [HttpGet]
        [Route("GetPrevisaoFerias")]
        public List<DistribuicaoFeriasOperModel> GetPrevisaoFerias()
        {
            return PrevisaoFeriasRepository.GetPrevisaoFerias(null);
        }

        [HttpGet]
        [Route("GetPrevisaoFerias/{matricula}/{periodoAquisitivo}")]
        public List<DistribuicaoFeriasOperModel> GetPrevisaoFerias(int matricula, string periodoAquisitivo)
        {
            return PrevisaoFeriasRepository.GetPrevisaoFerias(matricula,  periodoAquisitivo);
        }

        [HttpGet]
        [Route("GetPrevisaoFerias/{dfopId}")]
        public List<DistribuicaoFeriasOperModel> GetPrevisaoFerias(int dfopId)
        {
            return PrevisaoFeriasRepository.GetPrevisaoFerias(dfopId);
        }
    }
}