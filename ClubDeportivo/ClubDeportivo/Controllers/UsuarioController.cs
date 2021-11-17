using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fachada;
using Dominio;

namespace ClubDeportivo.Controllers
{
    public class UsuarioController : Controller
    {
        FachadaUsuario FchUsuario = new FachadaUsuario();

        // GET: Usuario
        [HttpGet]
        public ActionResult AltaUser()
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }
            return View(new Usuario());
        }

        [HttpPost]
        public ActionResult AltaUser(Usuario user)
        {
            if (Session["Logueado"] == null)
            {
                return Redirect("/usuario/Login");
            }

            string msj = FchUsuario.AltaUsuario(user.Email, user.Password);

            if (msj == "")
            {
                ViewBag.mensaje = "El usuario se registro con exito";
                user = new Usuario(); //LIMPIA EL FORMULARIO DEL VIEW
            }
            else
            {
                ViewBag.mensaje = msj;
                user = new Usuario();//LIMPIA EL FORMULARIO DEL VIEW
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            Usuario user = FchUsuario.Login(email, password);
            if (user != null)
            {
                Session["Logueado"] = true;
                Session["LogueadoEmail"] = user.Email;
                return Redirect("/socio/ListarSocios");
            }
            else
            {
                ViewBag.mensaje = "Error al ingresar";
            }

            return View();
        }

        public ActionResult Logout()
        {         
            Session.Clear();
            return Redirect("/usuario/Login");
        }
    }
}