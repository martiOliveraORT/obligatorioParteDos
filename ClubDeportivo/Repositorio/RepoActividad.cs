using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Repositorio
{
    public class RepoActividad : IRepositorio<Actividad>
    {
        string cadena = Conexion.stringConexion;
        
        public bool Alta(Actividad obj)
        {
            bool respuesta = false;
            //Verifico que el obj no sea nulo
            if (obj == null) return respuesta;
            try
            {
                //Creo la instancia de la bd
                RepoContext db = new RepoContext(cadena);
                //Agrego el obj a la bd
                db.Actividades.Add(obj);
                //Guardo los cambios
                db.SaveChanges();

                //Verificamos que se haya creado la actividad
                Actividad act = db.Actividades.Find(obj.Id);
                if(act != null)
                {
                    //Ya que encontro la actividad, retorno true
                    respuesta = true;
                }
                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return respuesta;
        }
        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }
        public bool Modificacion(Actividad obj)
        {
            throw new NotImplementedException();
        }
        public List<Actividad> TraerTodo()
        {
            List<Actividad> act = null;
            try
            {
                //Creo el contexto
                RepoContext db = new RepoContext(cadena);
                //Obtengo la lista Ienumerable de act
                IEnumerable<Actividad> actI = from Actividad a
                                             in db.Actividades
                                                 select a;
                //La convierto a List
                act = actI.ToList();
                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return act;
        }
        public Actividad BuscarPorId(int id)
        {
            Actividad act = null;
            //Verifico que el id no llege vacio
            if (id <= 0) return act;
            try
            {
                //Creo el contexto
                RepoContext db = new RepoContext(cadena);
                //Busco la act por su id
                act = db.Actividades.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return act;
        }
        public Actividad BusarPorNombre(string n)
        {
            Actividad act = null;
            //Verifico que el nombre no llege vacio
            if (n == "") return act;
            try
            {
                //Creo el contexto
                RepoContext db = new RepoContext(cadena);
                //Busco la act por su id
                act = db.Actividades.FirstOrDefault(a => a.Nombre == n);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return act;
        }
        /*
         Se listarán todas las actividades que cumplan con el criterio de búsqueda seleccionado, ordenadas por 
        nombre de actividad, dentro de nombre por día de la semana, dentro del mismo día de la semana se 
        ordenará por hora. Todos los criterios de orden son ascendentes.

        Nuestra tabla Horarios, cuenta con tres columnas, (Actividad, dia y hora)
         */
        public List<Horario> BuscarActividadPorTexto(string text)
        {
            List<Horario> act = null;
            //Verifico que el texto no llege vacio
            if (text == "") return act;
            try
            {
                //Creo el contexto
                RepoContext db = new RepoContext(cadena);
                //Busco las actividades que contengan ese texto en el nombre y ordeno segun lo pedido
                IEnumerable<Horario> actI = db.Horarios.Where(a => a.Actividad.Contains(text)).OrderBy(a => a.Actividad).ThenBy(a => a.Dia).ThenBy(a => a.Hora).ToList();
                act = actI.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return act;
        }

        public List<Horario> BuscarActividadPorEdad(int edad)
        {
            List<Horario> act = null;
            //Verifico que la edad no llege vacio
            if (edad < 0) return act;
            try
            {
                //Creo el contexto
                RepoContext db = new RepoContext(cadena);
                //Busco por cota mínima de edad
                IEnumerable<Horario> actI = from h in db.Horarios
                                            join a in db.Actividades
                                            on h.Actividad equals a.Nombre
                                            where a.EdadMin <= edad
                                            orderby h.Actividad ascending
                                            orderby h.Dia ascending
                                            orderby h.Hora ascending
                                            select h;
                act = actI.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return act;
        }

        public List<Horario> BuscarActividadPorDiaHora(string dia, int hora)
        {
            List<Horario> act = null;
            //Verifico que el texto no llege vacio
            if (hora < 0 || dia == "") return act;
            try
            {
                //Creo el contexto
                RepoContext db = new RepoContext(cadena);
                //Busco Por día/hora de la semana
                List<Horario> horariosActividad = db.Horarios.Where(a => a.Dia == dia && a.Hora == hora).OrderBy(a => a.Actividad).ThenBy(a => a.Dia).ThenBy(a => a.Hora).ToList();
                act = horariosActividad.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return act;
        }

        // SECCION CORRESPONDE A HORARIOS
        #region Horarios
        public List<Horario> BuscarHorarios(string dia, int hora)
        {
            List<Horario> horario = null;
            try
            {
                //Creo el contexto
                RepoContext db = new RepoContext(cadena);
                //Obtengo la lista Ienumerable de horarios
                IEnumerable<Horario> horarioI = from Horario h in db.Horarios
                                            where h.Hora >= hora &&
                                            h.Dia == dia
                                            orderby h.Hora ascending
                                            select h;
                //La convierto a List
                horario = horarioI.ToList();
                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return horario;
        }

        public List<Horario> TraerTodosHorarios()
        {
            List<Horario> horarios = null;
            try
            {
                //Creo el contexto
                RepoContext db = new RepoContext(cadena);
                //Obtengo la lista Ienumerable de horarios
                IEnumerable<Horario> horariosI = from Horario h
                                                 in db.Horarios
                                                 select h;
                //La convierto a List
                horarios = horariosI.ToList();
                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return horarios;
        }
        public Horario TrarUnHorario(string act, string dia, int hora)
        {
            Horario horario = null;
            if (act == "" || dia == "" || hora < 0) return horario;
            try
            {
                //Creo el contexto
                RepoContext db = new RepoContext(cadena);
                //Obtengo un horario segun activdad, dia y hora
                horario = db.Horarios.Where(h => h.Actividad == act && h.Dia == dia && h.Hora == hora).FirstOrDefault();

                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return horario;
        }

        public bool AltaHorario(Horario obj)
        {
            bool respuesta = false;
            //Verifico que el obj no sea nulo
            if (obj == null) return respuesta;
            try
            {
                //Creo la instancia de la bd
                RepoContext db = new RepoContext(cadena);
                //Agrego el obj a la bd
                db.Horarios.Add(obj);
                //Guardo los cambios
                db.SaveChanges();

                //Verificamos que se haya creado el horario
                Horario act = db.Horarios.Find(obj.Actividad, obj.Dia, obj.Hora);
                if (act != null)
                {
                    //Ya que encontro el horario, retorno true
                    respuesta = true;
                }
                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return respuesta;
        }
    }
        #endregion
}

