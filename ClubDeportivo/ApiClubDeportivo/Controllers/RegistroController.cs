using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fachada;
using Dominio;

namespace ApiClubDeportivo.Controllers
{
    [RoutePrefix("api/registros")]
    public class RegistroController : ApiController
    {
        [HttpGet]
        [Route("{ci:int}/{actividad}")] //Get api/registros/"{ci}/{actividad}"
        public IEnumerable<RegistroActividad> GetIngresosSocio(int ci, string actividad)
        {
            FachadaRegistroActividad fachada = new FachadaRegistroActividad();
            List<RegistroActividad> lista;
            lista = fachada.IngresoSocioPorActividad(ci, actividad);
            return lista;
        }
    }
}
