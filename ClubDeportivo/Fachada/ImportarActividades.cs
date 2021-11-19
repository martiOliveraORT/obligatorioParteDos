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
    class ImportarActividades
    {
        internal static class ManejoArchivos
        {
            // Lugar donde va tomar el archivo
            private static string ArchivoPersonas = AppDomain.CurrentDomain.BaseDirectory + "Archivos\\Actividades.txt";
            // Constante que setea la cantidad de columnas por linea a insertar
            private const int cantColumnas = 7;

            // Funcion que lee el documento para dar las altas
            private static string leerDocumentoActividad()
            {
          
                StreamReader ReaderObject = new StreamReader(ArchivoPersonas);  //Variable tipo StreamReder para leer archivo hasta el final
                int registrosAct = 0; // Cantidad de registros exitosos actividades
                int fallosAct = 0; // Cantidad de registros fallidos actividades
                int registrosHrs = 0; // Cantidad de registros exitosos horarios
                int fallosHrs = 0; // Cantidad de registros fallidos horarios
                string delimitador = "|"; // Delimitador de datos en el archivo
                string[] datosObjeto = ArchivoPersonas.Split(delimitador.ToCharArray()); // sub divide la cadena de texto en un array dependiendo el delimitador
                RepoActividad repoAct = new RepoActividad();
                // Mientras haya texto para leer
                while ((ReaderObject.ReadLine()) != null)
                {
                    if (datosObjeto.Length == cantColumnas) // Verifico que la linea que voy a moldear tenga la cantidad de datos necesarias
                    {
                        // Armo el objeto de tipo actividad
                        Actividad nuevaActividad = new Actividad
                        {
                            Nombre = datosObjeto[0],
                            EdadMin = validarInt(datosObjeto[1]),
                            EdadMax = validarInt(datosObjeto[2]),
                            Duracion = validarInt(datosObjeto[3]),
                            CuposDisponibles = validarInt(datosObjeto[4])
                        };
                        // Llamo al insert pasandole la actividad armada
                        bool successAct = InsertarActividad(nuevaActividad);

                        if (successAct)
                        {
                            registrosAct++;
                        }
                        else
                        {
                            fallosHrs++;
                        }

                        // Verifico que la actividad exista independientemente si falla el registro de actividad anterior
                        // Hago esto por si hay lineas duplicadas pero con diferente horario
                        if (repoAct.BusarPorNombre(nuevaActividad.Nombre)!=null)
                        {

                            Horario nuevoHorario = new Horario // Creo el objeto horario
                            {
                                Actividad = nuevaActividad.Nombre,
                                Dia = datosObjeto[5],
                                Hora = validarInt(datosObjeto[6])

                            };
                            // Llamo al insert pasandole el horario armado
                            bool successHorario = InsertarHorario(nuevoHorario);

                            if (successHorario)
                            {
                                registrosHrs++;
                            }
                            else
                            {
                                fallosHrs++;
                            }
                        }

                    }
                }
                
                return "Actividades registradas correctamente: " + registrosAct + " Registro de actividades fallidos:" + fallosAct
                         + " Horarios registrados correctamente:" +registrosHrs + "Registro de horarios fallidos" + fallosHrs;
            }

            // Funcion que se encarga de hacer las validaciones previas
            // En caso de estar todo ok hace el insert
            public static bool InsertarActividad(Actividad act)
            {
                bool success = false;
                if (act != null && validarNombreActividad(act) && validarEdadesActividad(act))
                {
                    RepoActividad repoAct = new RepoActividad();
                    repoAct.Alta(act);
                    success = false;
                }
                return success;
            }

            public static bool InsertarHorario(Horario hrs)
            {
                bool success = false;
                if (hrs != null && existeHorario(hrs) && validarHrComiezo(hrs))
                {
                    RepoActividad repoAct = new RepoActividad();
                    repoAct.AltaHorario(hrs);
                    success = false;
                }
                return success;
            }


            // Funcion que Valida:
            // Nombre de la actividad no este vacio
            // No haya otra actividad con ese nombre
            public static bool validarNombreActividad(Actividad act)
            {
                bool success = false;

                // Seteo variable para poder llamar al Repo
                // Valido que no haya una Actividad con el nombre ingresado en el doc
                RepoActividad RepoAct = new RepoActividad();
                Actividad nombreExistente = RepoAct.BusarPorNombre(act.Nombre);

                if (act.Nombre != "" || nombreExistente == null)
                {
                    success = true;
                }
                
                return success;
            }


            // Funcion que Valida:
            // Rango de edad Max y Min sea correcto
            // El rango min no sea mayor al max
            public static bool validarEdadesActividad(Actividad act)
            {
                bool success = false;
                int edadMinAceptada = 3;
                int edadMaxAceptada = 90;
                RepoActividad RepoAct = new RepoActividad();
                Actividad nombreExistente = RepoAct.BusarPorNombre(act.Nombre);

                if (act.Duracion == -1 || act.EdadMax == -1 || act.EdadMin == -1 || act.CuposDisponibles == -1) return success;

                if (act.EdadMax >= act.EdadMin && act.EdadMin >= edadMinAceptada && act.EdadMax <= edadMaxAceptada)
                {
                    success = true;
                }

                return success;
            }


            // Funcion que valida:
            // Si el horario para dicha actividad ya existe
            public static bool existeHorario(Horario hrs)
            {
                bool success = false;

                RepoActividad repoAct = new RepoActividad();
                Horario hrsExistente = repoAct.TraerUnHorario(hrs.Actividad, hrs.Dia, hrs.Hora);

                if (hrsExistente == null)
                {
                    success = true;
                }

                return success;
            }

            // Funcion que valida:
            // Si el horario esta denro del rango permitido y es en unpunto
            public static bool validarHrComiezo(Horario hrs)
            {
                bool success = false;

                if (hrs.Hora == -1) return success;

                if(hrs.Hora >= 7 && hrs.Hora <= 23)
                {
                    success = true;
                }

                return success;
            }

            // Funcion que se encarga de setear un valor numerico 
            // Es para que no explote si se pone algo difernte a un entero en el doc
            // Si no es un entero le asigno -1
            public static int validarInt(string n)
            {
                int valorNumerico;
                bool esEntero = int.TryParse(n, out valorNumerico);

                if (!esEntero)
                {
                    valorNumerico = -1; 
                }

                return valorNumerico;
            }
        }
    }
}

