using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fachada;
using Dominio;

namespace ClubDeportivo.Controllers
{
    public class SocioController : Controller
    {
        FachadaSocio fSocio = new FachadaSocio();
        FachadaMensualidad fMensualidad = new FachadaMensualidad();

        [HttpGet]
        public ActionResult AltaSocio()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            return View(new Socio());
        }

        [HttpPost]
        public ActionResult AltaSocio(Socio socio)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }

            socio.FechaIngreso = DateTime.Now;
            socio.Estado = true;

            string msj = fSocio.AltaSocio(socio);

            ViewBag.mensaje = msj;

            socio = new Socio(); //Limpia el formulario del view

            return View(socio);
        }

        public ActionResult Detalle()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }

            string valor = Request.QueryString["cedula"];
            
            if(valor != null)
            {
                int c = Convert.ToInt32(valor);
                var (socio, msj) = fSocio.BuscarSocio(c);
                //buscar mensualidad de socio
                var (mens, msjMens) = fMensualidad.BuscarMesualidad(c);

                if (mens == null)
                {
                    ViewBag.tieneMensualidad = false;
                }
                else
                {
                    if (mens.Vencimiento > DateTime.Now)
                    {
                        //si esta paga, navega a ingresar actividades y ver todos los ingresos que realizó en una fecha dada en el mes corriente
                        ViewBag.tieneMensualidad = true;

                    }
                    else
                    {
                        //si no esta paga, link al registro de pago para el socio y ver todos los ingresos que realizó en una fecha dada en el mes corriente
                        ViewBag.tieneMensualidad = false;

                    }
                }
                return View(socio);
            }


            return View(new Socio());
        }

        [HttpPost]
        public ActionResult Detalle(int? Cedula)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }            

            var (Socio, msj) = fSocio.BuscarSocio((int)Cedula);
            if(Socio == null)
            {
                ViewBag.m = msj;
                return View("BuscarSocio");
            }
            //buscar mensualidad de socio
            var (mens, msjMens) = fMensualidad.BuscarMesualidad((int)Cedula);

            if (mens == null)
            {
                ViewBag.tieneMensualidad = false;
            }
            else
            {
                if (mens.Vencimiento > DateTime.Now)
                {
                    //si esta paga, navega a ingresar actividades y ver todos los ingresos que realizó en una fecha dada en el mes corriente
                    ViewBag.tieneMensualidad = true;

                }
                else
                {
                    //si no esta paga, link al registro de pago para el socio y ver todos los ingresos que realizó en una fecha dada en el mes corriente
                    ViewBag.tieneMensualidad = false;

                }
            }

            return View(Socio);
        }

        public ActionResult ListarSocios()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }

            var (socios, msj) = fSocio.ListarSocios();
            ViewBag.msj = msj;
            ViewBag.socios = socios;
            return View();
        }

        public ActionResult ListarActividades()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ListarActividades(int cedula, DateTime ? fecha)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            List<RegistroActividad> lista = fSocio.BuscarActividadesPorSocio(cedula, (DateTime)fecha);
            if (lista == null)
            {
                ViewBag.m = "Error en la BD";
            }
            else
            {
                ViewBag.res = lista;
                ViewBag.cant = lista.Count;
            }
            return View();
        }

        public ActionResult IrAModificarSocio()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            return View(new Socio());
        }

        [HttpPost]
        public ActionResult IrAModificarSocio(int Cedula)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            var (socio, msj) = fSocio.BuscarSocio(Cedula);
            ViewBag.m = socio;
            return View(socio);
        }

        [HttpGet]
        public ActionResult ModificarSocio()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            return View(new Socio());
        }

        [HttpPost]
        public ActionResult ModificarSocio(Socio socio)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            socio.FechaIngreso = DateTime.Now;

            string msj = fSocio.ModificarSocio(socio.Cedula, socio.Nombre, socio.FechaNac);

            int c = socio.Cedula;

            socio = new Socio(); //Limpia el formulario del view


            return RedirectToAction("Detalle", new { Cedula = c});
        }

        public ActionResult BuscarSocio()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult BuscarSocio(int cedula)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            var (socio, msj) = fSocio.BuscarSocio(cedula);
            if(socio == null)
            {
                ViewBag.m = msj;
                return View("BuscarSocio");
            }
            return View();
        }


        public ActionResult DarDeAltaSocio(int cedula, int estado)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            fSocio.DarAltaSocio(cedula, estado);

            return RedirectToAction("Detalle", new { Cedula = cedula });
        }
    }
}