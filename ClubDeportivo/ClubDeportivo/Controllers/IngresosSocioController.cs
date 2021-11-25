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
    public class IngresosSocioController : Controller
    {
        HttpClient cliente = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();
        Uri registroUri = null;

        public IngresosSocioController()
        {
            cliente.BaseAddress = new Uri("http://localhost:58276/");
            registroUri = new Uri("http://localhost:58276/api/registros");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public ActionResult BuscarIngresos()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            FachadaActividades fachada = new FachadaActividades();
            List<string> lista = fachada.ListaActividades();
            ViewBag.lista = lista;
            return View();
        }

        [HttpPost]
        public ActionResult BuscarIngresosPorActividad(int ci, string actividad)
        {
            //Para que arme la lista de actividades
            FachadaActividades fachada = new FachadaActividades();
            List<string> lista = fachada.ListaActividades();
            ViewBag.lista = lista;

            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            response = cliente.GetAsync(registroUri + "/" + ci + "/" + actividad).Result;
            if (response.IsSuccessStatusCode)
            {
                var act = response.Content.ReadAsAsync<IEnumerable<RegistroActividad>>().Result;

                if (act != null && act.Count() > 0)
                {
                    ViewBag.m = act.ToList();
                    return View("BuscarIngresos");
                }
            }
            return View("BuscarIngresos");
        }

    }
}