using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Repositorio;


namespace Fachada
{
    public class FachadaRegistroActividad
    {
        public List<RegistroActividad> IngresoSocioPorActividad(int ci, string act)
        {
            RepoRegistroActividad regAct = new RepoRegistroActividad();
            List<RegistroActividad> listaIngresos = null;
            if (ci < 0 || act == "") return listaIngresos;

            try
            {
                listaIngresos = regAct.IngresosSocioPorActividad(ci, act);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listaIngresos;
            
        }
    }
}
