using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Collections.Generic;
using Fachada;
using Dominio;
using System.Linq;

namespace ClubDeportivo.Controllers
{
    public class ActividadController : Controller
    {
        HttpClient cliente = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();
        Uri actividadUri = null;

        public ActividadController()
        {
            cliente.BaseAddress = new Uri("http://localhost:58276/");
            actividadUri = new Uri("http://localhost:58276/api/actividades");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public ActionResult BuscarActividad()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            return View();
        }
        
        [HttpPost]
        public ActionResult BuscarActividadPorNombre(string textoEnNombre)
        {
                if (Session["Logueado"] == null)
                {
                    return Redirect("/usuario/Login");
                }
                response = cliente.GetAsync(actividadUri +"/"+ textoEnNombre + "/nombre").Result;
            if (response.IsSuccessStatusCode)
            {
                var act = response.Content.ReadAsAsync<IEnumerable<ActividadHorario>>().Result;

                if(act != null && act.Count() > 0)
                {
                    ViewBag.m = act.ToList();
                    return View("BuscarActividad");
                }
            }
            return View("BuscarActividad");
        }

        [HttpPost]
        public ActionResult BuscarActividadPorEdad(int edad)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            response = cliente.GetAsync(actividadUri + "/" + edad + "/edad").Result;
            if (response.IsSuccessStatusCode)
            {
                var act = response.Content.ReadAsAsync<IEnumerable<ActividadHorario>>().Result;

                if (act != null && act.Count() > 0)
                {
                    ViewBag.m = act.ToList();
                    return View("BuscarActividad");
                }
            }
            return View("BuscarActividad");
        }

        [HttpPost]
        public ActionResult BuscarActividadPorDiaHora(string dia, int hora)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            response = cliente.GetAsync(actividadUri + "/" + dia + "/"+ hora + "/dia").Result;
            if (response.IsSuccessStatusCode)
            {
                var act = response.Content.ReadAsAsync<IEnumerable<ActividadHorario>>().Result;

                if (act != null && act.Count() > 0)
                {
                    ViewBag.m = act.ToList();
                    return View("BuscarActividad");
                }
            }
            return View("BuscarActividad");
        }
    }
}