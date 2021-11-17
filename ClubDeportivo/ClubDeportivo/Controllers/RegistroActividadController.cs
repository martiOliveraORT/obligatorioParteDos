using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WcfRegActividad;

namespace ClubDeportivo.Controllers
{
    public class RegistroActividadController : Controller
    {
        // GET: RegistroActividad
        ServiceRegAct service = new ServiceRegAct();
        public ActionResult ListaHorariosDisponibles(int cedula)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            IEnumerable<DtoHorario> horarios = service.GetHorariosDisponibles();
            ViewBag.res = horarios;
            ViewBag.socio = cedula;
            return View(cedula);
        }

        public ActionResult IngresarActividad()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult IngresarActividad(int cedula, string actividad, int hora, int id)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            DtoHorario horario = new DtoHorario
            {
                Actividad = actividad,
                Hora = hora,
                Id = id,
            };
            bool respuesta = service.AltaRegistro(cedula, horario);
            ViewBag.res = respuesta;
            return View();
        }
    }
}