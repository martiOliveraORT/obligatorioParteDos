using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dominio;
using Fachada;

namespace ActividadApi.Controllers
{
    public class ActividadController : ApiController
    {
        //GET api/Busqueda
        public List<ActividadHorario> BusquedaActividad(string texto, int edad, string dia, int hora)
        {
            List<ActividadHorario> lista = null;

            FachadaActividades f = new FachadaActividades();

            lista = f.BuscarActividades(texto, edad, dia, hora);

            return lista;
        }
    }
}
