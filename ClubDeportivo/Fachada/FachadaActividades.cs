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
        public List<ActividadHorario> BuscarActividades(string textoEnNombre, int edad, string dia, int hora)
        {
            List<ActividadHorario> listadoRetorno = null;
            RepoActividad repo = new RepoActividad();
            List<Horario> listaH = null;

            if (textoEnNombre != "" || textoEnNombre != null)
            {
                listaH = repo.BuscarActividadPorTexto(textoEnNombre);
            }else if(edad > 0)
            {
                listaH = repo.BuscarActividadPorEdad(edad);
            }else if(dia != "" || dia != null)
            {
                listaH = repo.BuscarActividadPorDiaHora(dia, hora);
            }
            else
            {
                return listadoRetorno;
            }

            if(listaH.Count > 0)
            {
                foreach (Horario h in listaH)
                {
                    Actividad act = repo.BusarPorNombre(h.Actividad);
                    listadoRetorno.Add(new ActividadHorario
                    {
                        Nombre = act.Nombre,
                        Id = act.Id,
                        EdadMin = act.EdadMin,
                        EdadMax = act.EdadMax,
                        Duracion = act.Duracion,
                        CuposDisponibles = act.CuposDisponibles,
                        Dia = h.Dia,
                        Hora = h.Hora
                    });
                }
            }

            return listadoRetorno;
        }
        
    }
}
