using FeriasAPI.Models;
using FeriasAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FeriasAPI.Controllers
{
    [RoutePrefix("api/FeriasDelega")]
    public class FeriasDelegaController : ApiController
    {
        [HttpGet]
        [Route("GetFeriasDelega/{gestor}")]
        public List<FeriasDelegaModel> GetFeriasDelega(int gestor)
        {
            return FeriasDelegaRepository.GetFeriasDelega(gestor);
        }

        [HttpPost]
        [Route("SaveFeriasDelega")]
        public void SaveFeriasDelega([FromBody] FeriasDelegaModel feriasDelega)
        {
            FeriasDelegaRepository.SaveFeriasDelega(feriasDelega);
        }

        [HttpPost]
        [Route("SaveFeriasDelegaList")]
        public void SaveFeriasDelegaList([FromBody] List<FeriasDelegaModel> feriasDelegaList)
        {
            FeriasDelegaRepository.SaveFeriasDelegaList(feriasDelegaList);
        }
    }
}