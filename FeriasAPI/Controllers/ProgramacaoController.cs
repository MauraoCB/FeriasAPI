
using FeriasAPI.Models;
using FeriasAPI.Repository;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace FeriasAPI.Controllers
{
    [RoutePrefix("api/Programacao")]
    public class ProgramacaoController : ApiController
    {
      
        [HttpPost]
        [Route("SaveImportedSheet")]
        public void SaveImportedSheet([FromBody] List<ProgramacaoModel> progamacaoModel)
        {
            ProgramacaoRepository.SaveImportedSheet(progamacaoModel);
        }

        [HttpPost]
        [Route("AprovarProgramacoes")]
        public void AprovarProgramacoes([FromBody] List<dynamic> progamacoesAprovar)
        {
            ProgramacaoRepository.AprovarProgramacoes(progamacoesAprovar);
        }

        [HttpPost]
        [Route("SaveProgramacao")]
        public void SaveProgramacao([FromBody] ProgramacaoModel progamacaoModel)
        {
            ProgramacaoRepository.SaveProgramacao(progamacaoModel);
        }

        [HttpGet]
        [Route("GetProgramacaoFerias")]
        public List<ProgramacaoModel> GetProgramacaoFerias()
        {
            return ProgramacaoRepository.GetProgramacaoFerias(null);
        }
    }
}