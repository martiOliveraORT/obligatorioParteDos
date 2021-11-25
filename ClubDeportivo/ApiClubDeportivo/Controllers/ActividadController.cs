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
    [RoutePrefix("api/actividades")]
    public class ActividadController : ApiController
    {
        [HttpGet]
        [Route("{textoEnNombre}/nombre")] //Get api/actividades/{textoEnNombre}/nombre
        public IEnumerable<ActividadHorario> GetActividadNombre(string textoEnNombre)
        {
            FachadaActividades fachada = new FachadaActividades();
            List<ActividadHorario> lista;
            lista = fachada.BuscarActividadPorNombre(textoEnNombre);
            return lista;
        }

        [HttpGet]
        [Route("{edad:int}/edad")] //Get api/actividades/{edad}/edad
        public IEnumerable<ActividadHorario> GetActividadEdad(int edad)
        {
            FachadaActividades fachada = new FachadaActividades();
            List<ActividadHorario> lista;
            lista = fachada.BuscarActividadPorEdad(edad);
            return lista;
        }

        [HttpGet]
        [Route("{dia}/{hora:int}/dia")] //Get api/actividades/{dia}/{hora}/dia
        public IEnumerable<ActividadHorario> GetActividadDiaHora(string dia, int hora)
        {
            FachadaActividades fachada = new FachadaActividades();
            List<ActividadHorario> lista;
            lista = fachada.BuscarActividadPorDiaHora(dia, hora);
            return lista;
        }
    }
}
