using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClubDeportivo.Controllers
{
    public class ActividadesController : Controller
    {
        // GET: Actividades
        /*
        public ActionResult Index()
        {
            HttpClient cliente = new HttpClient();
            Uri uri = new Uri("http://localhost:1234/api/actividad");
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            string json = "{nombre:\'fut'}";
            HttpContent contenido = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            solicitud.Content = contenido;
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();
            return View();

            if (respuesta.IsSuccessStatusCode)
            {
                Task<string> contenidoRespuesta = respuesta.Result.Content.ReadAsStringAsync();
                contenidoRespuesta.Wait();
            }
        }*/
    }
}