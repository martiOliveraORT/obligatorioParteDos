using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Repositorio;

namespace Fachada
{
    public class FachadaActividades
    {
        public List<ActividadHorario> BuscarActividadPorNombre(string texto)
        {
            List<ActividadHorario> listadoRetorno = new List<ActividadHorario>();
            RepoActividad repo = new RepoActividad();
            List<Horario> listaH = null;
            //Filtro para que tipo es la busqueda
            if (texto != "")
            {
                listaH = repo.BuscarActividadPorTexto(texto);
            }
            else
            {
                return listadoRetorno;
            }
            //Recorro la lista de actividades obtenida, para crear el nuevo 
            if (listaH.Count > 0)
            {
                foreach (Horario h in listaH)
                {
                    Actividad act = repo.BusarPorNombre(h.Actividad);
                    ActividadHorario ah = new ActividadHorario
                    {
                        Nombre = act.Nombre,
                        Id = act.Id,
                        EdadMin = act.EdadMin,
                        EdadMax = act.EdadMax,
                        Duracion = act.Duracion,
                        CuposDisponibles = act.CuposDisponibles,
                        Dia = h.Dia,
                        Hora = h.Hora
                    };
                    try
                    {
                        listadoRetorno.Add(ah);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            return listadoRetorno;
        }

        public List<ActividadHorario> BuscarActividadPorEdad(int edad)
        {
            List<ActividadHorario> listadoRetorno = new List<ActividadHorario>();
            RepoActividad repo = new RepoActividad();
            List<Horario> listaH = null;
            //Filtro para que tipo es la busqueda
            if (edad > 0)
            {
                listaH = repo.BuscarActividadPorEdad(edad);
            }
            else
            {
                return listadoRetorno;
            }
            //Recorro la lista de actividades obtenida, para crear el nuevo 
            if (listaH.Count > 0)
            {
                foreach (Horario h in listaH)
                {
                    Actividad act = repo.BusarPorNombre(h.Actividad);
                    ActividadHorario ah = new ActividadHorario
                    {
                        Nombre = act.Nombre,
                        Id = act.Id,
                        EdadMin = act.EdadMin,
                        EdadMax = act.EdadMax,
                        Duracion = act.Duracion,
                        CuposDisponibles = act.CuposDisponibles,
                        Dia = h.Dia,
                        Hora = h.Hora
                    };
                    try
                    {
                        listadoRetorno.Add(ah);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            return listadoRetorno;
        }

        public List<ActividadHorario> BuscarActividadPorDiaHora(string dia, int hora)
        {
            List<ActividadHorario> listadoRetorno = new List<ActividadHorario>();
            RepoActividad repo = new RepoActividad();
            List<Horario> listaH = null;
            //Filtro para que tipo es la busqueda
            if (dia != "")
            {
                listaH = repo.BuscarActividadPorDiaHora(dia, hora);
            }
            else
            {
                return listadoRetorno;
            }
            //Recorro la lista de actividades obtenida, para crear el nuevo 
            if (listaH.Count > 0)
            {
                foreach (Horario h in listaH)
                {
                    Actividad act = repo.BusarPorNombre(h.Actividad);
                    ActividadHorario ah = new ActividadHorario
                    {
                        Nombre = act.Nombre,
                        Id = act.Id,
                        EdadMin = act.EdadMin,
                        EdadMax = act.EdadMax,
                        Duracion = act.Duracion,
                        CuposDisponibles = act.CuposDisponibles,
                        Dia = h.Dia,
                        Hora = h.Hora
                    };
                    try
                    {
                        listadoRetorno.Add(ah);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            return listadoRetorno;
        }

        public List<string> ListaActividades()
        {
            List<string> actividades = new List<string>();

            RepoActividad repo = new RepoActividad();

            List<Actividad> lista = repo.TraerTodo();

            foreach(Actividad a in lista)
            {
                actividades.Add(a.Nombre);
            }
            return actividades;
        }
        
    }
}
