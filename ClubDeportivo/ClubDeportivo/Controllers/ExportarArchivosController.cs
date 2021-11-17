using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fachada;
using Dominio;

namespace ClubDeportivo.Controllers
{
    public class ExportarArchivosController : Controller
    {
        // GET: ExportarArchivos
        ExportarArchivos expArchivos = new ExportarArchivos();

        public ActionResult ExportarArchivos()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            ViewBag.res = expArchivos.ExportarTodos();
     
            return View();
        }
    }
}