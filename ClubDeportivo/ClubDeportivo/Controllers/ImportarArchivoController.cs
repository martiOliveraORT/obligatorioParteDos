using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fachada;
using Dominio;

namespace ClubDeportivo.Controllers
{
    public class ImportarArchivoController : Controller
    {
        // GET: ImportarArchivo
        public ActionResult importarArchivo()
        {
            return View("importarArchivo");
        }

       [HttpPost]
        public ActionResult importarArchivo(string opcionArchivo)
        {
  

            if (opcionArchivo == "1")
            {
                ImportarUsuarios importarUser = new ImportarUsuarios();
                ViewBag.res = importarUser.leerDocumentoUsuario();

                return View("importarArchivo");
            }
            else if (opcionArchivo == "0")
            {
                ImportarActividades importarAct = new ImportarActividades();
                ViewBag.res = importarAct.leerDocumentoActividad();

                return View("importarArchivo");
            }
            else
            {
                ViewBag.res = "Debe seleccionar una opcion de archivo a cargar";
                return View("importarArchivo");
            }
        }

    }
}