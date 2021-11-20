using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fachada;
using Dominio;


namespace ClubDeportivo.Controllers
{
    public class ImportarActividadesController : Controller
    {
        // GET: ImportarActividades
        ImportarActividades importarAct = new ImportarActividades();
        public ActionResult ImportarActividades()
        {
            ViewBag.res = importarAct.leerDocumentoActividad();

            return View();
        }
    }
}