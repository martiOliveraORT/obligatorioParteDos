using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Repositorio
{
    public class RepoRegistroActividad : IRepositorio<RegistroActividad>
    {
        string cadena = Conexion.stringConexion;
        public bool Alta(RegistroActividad obj)
        {
            bool resultado = false;

            if (obj == null) return resultado;
            try
            {
                //Creamos la instancia del contexto asignandole la cadena de conexion
                RepoContext db = new RepoContext(cadena);
                //Agrego el obj a la base
                db.RegistroActividades.Add(obj);
                //Guardo los cambios
                db.SaveChanges();
                //Verificamos que se haya creado el obj
                RegistroActividad registro = db.RegistroActividades.Find(obj.Nombre, obj.Socio, obj.Fecha);
                if(registro != null)
                {
                    resultado = true;
                }
                db.Dispose();

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return resultado;
        }
        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public bool Modificacion(RegistroActividad obj)
        {
            throw new NotImplementedException();
        }

        public List<RegistroActividad> TraerTodo()
        {
            List<RegistroActividad> lista = null;
            try
            {
                RepoContext db = new RepoContext(cadena);
                IEnumerable<RegistroActividad> registroI = from RegistroActividad r
                                                           in db.RegistroActividades
                                                           select r;
                //Convierto a list
                lista = registroI.ToList();
                db.Dispose();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public RegistroActividad BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        //  Funcionalidad que susplanta al BuscarPorId
        public RegistroActividad BusquedaEspecifica(int socio, string act, DateTime fecha)
        {
            RegistroActividad reg = null;
            if (socio < 0 || act == null || fecha == null) return reg;
            try
            {

                RepoContext db = new RepoContext(cadena);
                //Busco el registro por la key compuesta 
                //TODO: Revisar que funcione
                reg = db.RegistroActividades.Where(r => r.Socio == socio && r.Nombre == act && r.Fecha.Year == fecha.Year && r.Fecha.Month == fecha.Month && r.Fecha.Day == fecha.Day).FirstOrDefault();
                db.Dispose();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return reg;
        }

        //Funcion que devuelve cuantos cupos disponibles hay en una actividad
        public int CuposDisponibles(string act, string fecha, int hora)
        {
            //Lo dejo en -1 por si es error
            int cupos = -1;
            if (act == null || fecha == null) return cupos;
            try
            {
                DateTime dt = DateTime.Parse(fecha);
                RepoContext db = new RepoContext(cadena);
                cupos = db.RegistroActividades.Count(r => r.Nombre == act && r.Fecha == dt && r.Hora == hora);
                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cupos;
        }
        public List<RegistroActividad> ingresoSocioPorFecha(int ci, DateTime fecha)
        {
            List<RegistroActividad> ingresos = null;
            if (ci < 0 || fecha == null) return ingresos;
            try
            {
                RepoContext db = new RepoContext(cadena);
                IEnumerable<RegistroActividad> listaI = db.RegistroActividades.Where(r => r.Socio == ci && r.Fecha == fecha);
                ingresos = listaI.ToList();
                db.Dispose();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ingresos;
        }

        public List<RegistroActividad> IngresosSocioPorActividad(int ci, string act)
        {
            List<RegistroActividad> ingresos = null;
            if (ci < 0 || act == null) return ingresos;
            try
            {
                RepoContext db = new RepoContext(cadena);
                IEnumerable<RegistroActividad> listaI = db.RegistroActividades.Where(r => r.Socio == ci && r.Nombre == act)
                                                        .OrderByDescending(r => r.Fecha);
                ingresos = listaI.ToList();
                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ingresos;
        }
    }
}
