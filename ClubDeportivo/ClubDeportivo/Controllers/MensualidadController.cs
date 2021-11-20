using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fachada;
using Dominio;

namespace ClubDeportivo.Controllers
{
    public class MensualidadController : Controller
    {
        FachadaMensualidad FchMensualidad = new FachadaMensualidad();
        FachadaSocio FchSocio = new FachadaSocio();     
      
        public ActionResult MostrarCostoPL(int ci)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            var (costo, porcDescuento, valorCuota, antig) = FchMensualidad.MostrarCostoPL(ci);
            Socio socio = FchSocio.ValidarSocio(ci);
            if (FchMensualidad.AplicaDescPL(socio.FechaIngreso, antig))
            {
                ViewBag.aplica = "Aplica descuento por antiguedad mayor a " + antig + " meses";
                ViewBag.ok = true;
            }
            else
            {
                ViewBag.ok = false;
                ViewBag.aplica = "No aplica descuento";
            }
            ViewBag.costo = "Total: $ " + costo;
            ViewBag.porcDescuento = "Descuento: %" + porcDescuento;
            ViewBag.valorCuota = "$ " + valorCuota;
            ViewBag.ci = ci;

            return View();
        }

        public ActionResult MostrarCostoCup(int ci, int ingDisp)
        {
            if(Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            var (costo, porcDescuento, precioUnitario, cantAct) = FchMensualidad.MostrarCostoCup(ci, ingDisp);
            if (FchMensualidad.AplicaDescCup(ingDisp, cantAct))
            {
                ViewBag.aplica = "Aplica descuento por tener más de " + cantAct + " actividades";
                ViewBag.ok = true;
            }
            else
            {
                ViewBag.ok = false;
                ViewBag.aplica = "No aplica descuento";
            }
            ViewBag.costo = "Total: $ " + costo;
            ViewBag.porcDescuento = "Descuento: %" + porcDescuento;
            ViewBag.precioUnitario = precioUnitario;
            ViewBag.ci = ci;
            ViewBag.ingDisp = ingDisp;

            return View();
        }

        public ActionResult AltaPaseLibre(int? ci)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            //var (socio, msj) = FchSocio.BuscarSocio(ci);
            if (ci == null)
            {
                return Redirect("/Socio/ListarSocios");
            }

            ViewBag.ci = ci;     
            return View(ci);
        }

        public ActionResult CalcularPaseLibre()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            
            return View();
        }

        public ActionResult PaseLibre()
        {
            if(Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult PaseLibre(int ci)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            var (ok, msj) = FchMensualidad.AltaMensualidadPL(ci);
            if (ok)
            {
                ViewBag.msj = msj;
            }
            else
            {
                ViewBag.msj = msj;
            }
            return View();
        }
             
        public ActionResult AltaCuponera(int? ci)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            if (ci == null)
            {
                return Redirect("/Socio/ListarSocios");
            }

            ViewBag.ci = ci;
            
            return View();
        }
        public ActionResult Cuponera()
        {
            if(Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Cuponera(int ci, int ingDisp)
        {
            if(Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            var (ok, msj) = FchMensualidad.AltaMensualidadCuponera(ci, ingDisp);

            if (ok)
            {
                ViewBag.msj = msj;
            }
            else
            {
                ViewBag.msj = msj;
            }

            return View();
        }

        public ActionResult MensualidadesFchIngresada()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MensualidadesFchIngresada(int mes, int año)
        {            
            if (mes >= 1 && mes <= 12)
            {
                if (FchMensualidad.Validar4Cifras(año))
                {
                    List<Mensualidad> aux = FchMensualidad.MensualidadesPorFecha(mes, año);
                    if (aux.Count > 0)
                    {
                        ViewBag.Lista = aux;
                    }
                    else
                    {
                        ViewBag.msj = "No hay mensualidades registradas en esa fecha.";
                    }
                }
                else
                {
                    ViewBag.msj = "El año debe ser de cuatro cifras";
                }
            }
            else
            {
                ViewBag.msj = "El mes debe ser entre 1 y 12";
            }
            return View();
        }
    }
}