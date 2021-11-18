using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Globalization;
using Dominio;
using System.Text.RegularExpressions;
using Repositorio;

namespace WcfRegActividad
{
    public class ServiceRegAct : IServiceRegAct
    {
        // Variables para poder usar los llamados a la BD en sus respectivos REPO
        private RepoRegistroActividad RepoReg = new RepoRegistroActividad();
        private RepoActividad RepoHoras = new RepoActividad();
        private RepoSocio RepoSocios = new RepoSocio();
        private RepoMensualidad RepoMes = new RepoMensualidad();



        // Encargada de generar el alta 
        public bool AltaRegistro(int ci, DtoHorario regAct)
        {
            bool successReg = false;
            // Busco al socio
            Socio soc = RepoSocios.BuscarPorId(ci);
            Actividad act = RepoHoras.BusarPorNombre(regAct.Actividad);
            var (validacion, tipo) = VerifyCuposSocio(ci);


            // Verifico que exista la act o el user
            if (soc == null || act == null) return false;

            if (VerifyCupos(act, regAct.Hora) && VerifyEdad(soc, act) && VerifyHorario(regAct.Hora) && VerifyIngresoPrevio(ci, regAct.Actividad) && validacion)
            {

                RegistroActividad nvoRegistro = new RegistroActividad
                {
                    Nombre = regAct.Actividad,
                    Fecha = DateTime.Now,
                    Socio = ci,
                    Hora = regAct.Hora,
                };
                successReg = RepoReg.Alta(nvoRegistro);
                if (successReg && tipo == "c")
                {
                    RepoMes.RestarCupo(ci);
                }


            }


            return successReg;
        }




        // Encargada de traer todos los horarios disponibles que aun no comenzaron
        // Seguramente mas adelante refactorize esto
        public IEnumerable<DtoHorario> GetHorariosDisponibles()
        {
            string diaActual = GetDiaActual();
            int horaActual = GetHoraActual();

            // Llamo a los repos y la Query de buscar todos los horarios en base a hora y dia
            // Guardo la respuesta en una lista tipo Horario 
            IEnumerable<Horario> Horas = RepoHoras.BuscarHorarios(diaActual, horaActual);

            if (Horas == null)
            {
                return null;
            }
            else
            {
                // Creo una lista de tipo DTOHorario
                // DTOHorario seria la estrcutura de como va devolver la info el servicio
                // LLamo a la funcion Lista horario
                IEnumerable<DtoHorario> list = ObtenerListaHorarios(Horas);

                return list;
            }

        }




        // Funcion que devuelve el DTOHorarios armado para mostar, pasandole por parametro un array de horarios
        // Horas es la lista ya filtrada por la Query 
        private IEnumerable<DtoHorario> ObtenerListaHorarios(IEnumerable<Horario> Horas)
        {
            // Si la query no trajo nada, devuelvo null
            if (Horas == null) return null;

            // Creo una lista vacia de DTOHorario, donde voy a estrucuturar la data traida en 'Horas'
            // Recorro la data de Horas
            List<DtoHorario> horariosAux = new List<DtoHorario>();
            foreach (Horario h in Horas)
            {
                // Creo una variable tipo actividad para traerme una actividad por nombre (Query)
                // Luego solo me quedo con el ID para setearlo en DTOhorario a devolver en el servicio
                Actividad act = RepoHoras.BusarPorNombre(h.Actividad);
                int idAct = act.Id;
                // Verifico que haya cupos disponibles en la actividad
                if (VerifyCupos(act, h.Hora))
                {
                    horariosAux.Add(new DtoHorario
                    {
                        Actividad = h.Actividad,
                        Hora = h.Hora,
                        Id = idAct,
                    });
                }
            }
            return horariosAux;
        }


        #region funcionesGenericas
        // Funcion que devuelve dia actual en string y sin tilde ej: miercoles.
        private string GetDiaActual()
        {
            // Tomo la fecha actual, la paso astring y me traifo solo el dia en Espanol
            string dia = DateTime.Now.ToString("dddd", new CultureInfo("es-ES"));

            // Le saco los tildes
            dia = Regex.Replace(dia.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
            return dia;
        }

        //Funcion que devuelve la hora actual +1
        private int GetHoraActual()
        {
            // Tomo solo la hora sin minutos en formato string
            // HH en formato 24hrs | hh formato 12hrs AM/PM
            string horaTxt = DateTime.Now.ToString("HH", new CultureInfo("es-ES"));

            // Parse la hora en string a INT
            // Le sumo ya que las clases comienza en punto y si la hora actual es 00:58
            // Me trae 0 pero tengo que listar las que no comenzaron, por eso el +1
            int hora = Int32.Parse(horaTxt) + 1;

            return hora;
        }

        // Verifica los cupos disponibles para una actividad
        private bool VerifyCupos(Actividad act, int hora)
        {
            // Por defecto seteo el resultado en false
            bool success = false;


            // Si la actividad es null ya corto como false
            if (act == null) return false;

            // Tomo la fecha de hoy como string
            String fecha = DateTime.Now.ToString("yyyy-MM-dd");
            int cuposAct = act.CuposDisponibles;
            string nombreAct = act.Nombre;
            int cuposDis = RepoReg.CuposDisponibles(nombreAct, fecha, hora);

            // Verifico si al consulta fallo, devuelvo de una false
            if (cuposDis != -1)
            {
                // Hago la resta para verificar si hay cupos disponibles
                // Cupos de la actividad - cuantos hay anotados hasta el momento
                cuposDis = cuposAct - cuposDis;
                if (cuposDis <= 0)
                {
                    success = false;
                }
                else
                {
                    success = true;
                }
            }
            return (success);
        }

        private bool VerifyEdad(Socio soc, Actividad act)
        {
            bool success = false;
            DateTime nacimiento = soc.FechaNac; //Fecha de nacimiento
            int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
            int ActEdadMin = act.EdadMin;
            int ActEdadMax = act.EdadMax;

            if (edad >= ActEdadMin && edad <= ActEdadMax)
            {
                success = true;

            }

            return success;
        }

        private bool VerifyHorario(int horaComienzoAct)
        {
            bool success = false;
            int horaActual = GetHoraActual() - 1;

            if (horaActual < horaComienzoAct)
            {
                success = true;
            }

            return success;
        }


        private bool VerifyIngresoPrevio(int ci, string nombreAct)
        {
            bool success = false;
            String fecha = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("es-ES"));
            RegistroActividad ingresoEnElDia = RepoReg.BusquedaEspecifica(ci, nombreAct, fecha);

            if (ingresoEnElDia == null) success = true;

            return success;

        }

        private (bool, string) VerifyCuposSocio(int ci)
        {
            bool success = false;
            var mes = RepoMes.BuscarPorId(ci);
            Cuponera cuponera = null;
            if (mes == null) return (false, null);

            if (mes.TipoMetodo() == "c")
            {
                cuponera = (Cuponera)mes;

                if (cuponera.IngresosDisponibles > 0)
                {
                    success = true;
                }
            }
            else
            {
                success = true;
            }

            return (success, mes.TipoMetodo());
        }
        #endregion
    }

}


