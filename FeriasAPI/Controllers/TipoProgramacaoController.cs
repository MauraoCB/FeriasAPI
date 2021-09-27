using FeriasAPI.Models;
using FeriasAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Web.Http;

namespace FeriasAPI.Controllers
{
    [RoutePrefix("api/TipoProgramacao")]
    public class TipoProgramacaoController : ApiController
    {

        [HttpGet]
        [Route("GetTipoProgramacao")]
        public List<TipoProgramacaoModel> GetTipoProgramacao()
        {
            return TipoProgramacaoRepository.GetTipoProgramacao();
        }

        [HttpGet]
        [Route("GetTipoProgramacao/{tipoProgramacaoId}")]
        public List<TipoProgramacaoModel> GetTipoProgramacao(int tipoProgramacaoId)
        {
            return TipoProgramacaoRepository.GetTipoProgramacao(tipoProgramacaoId);
        }

        [HttpGet]
        [Route("GetTipoVisualizacao")]
        public List<TipoVisualizacaoModel> GetTipoVisualizacao()
        {
            return TipoProgramacaoRepository.GetTipoVisualizacao();
        }
        [HttpPost]
        [Route("SaveTipoProgramacao")]
        public void SaveTipoProgramacao([FromBody] TipoProgramacaoModel tipoProgramacao)
        {
            TipoProgramacaoRepository.SaveTipoProgramacao(tipoProgramacao);
        }
    }
}