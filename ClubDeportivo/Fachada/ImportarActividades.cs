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
    public class ImportarActividades
    {

        // Lugar donde va tomar el archivo
        private static string ArchivoPersonas = AppDomain.CurrentDomain.BaseDirectory + "archivos\\actividades.txt";
        // Constante que setea la cantidad de columnas por linea a insertar
        private const int cantColumnas = 7;
        private string delimitador = "|";

        // Funcion que lee el documento para dar las altas
        public string leerDocumentoActividad()
        {
            List<DtoArchivo> listaTxt = ObtenerTodos();

            int registrosAct = 0; // Cantidad de registros exitosos actividades
            int fallosAct = 0; // Cantidad de registros fallidos actividades
            int registrosHrs = 0; // Cantidad de registros exitosos horarios
            int fallosHrs = 0; // Cantidad de registros fallidos horarios
            RepoActividad repoAct = new RepoActividad();

            foreach (DtoArchivo h in listaTxt)
            {
                // Armo el objeto de tipo actividad
                Actividad nuevaActividad = new Actividad
                {
                    Nombre = h.Nombre,
                    EdadMin = h.EdadMin,
                    EdadMax = h.EdadMax,
                    Duracion = h.Duracion,
                    CuposDisponibles = h.CuposDisponibles
                };
                // Llamo al insert pasandole la actividad armada
                bool successAct = InsertarActividad(nuevaActividad);

                if (successAct)
                {
                    registrosAct++;
                }
                else
                {
                    fallosAct++;
                }

                // Verifico que la actividad exista independientemente si falla el registro de actividad anterior
                // Hago esto por si hay lineas duplicadas pero con diferente horario
                if (repoAct.BusarPorNombre(nuevaActividad.Nombre) != null)
                {

                    Horario nuevoHorario = new Horario // Creo el objeto horario
                    {
                        Actividad = h.Actividad,
                        Dia = h.Dia,
                        Hora = h.Hora
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
                else
                {
                    fallosHrs++;
                }

            }

            return "Se registraron " + registrosAct + " nuevas activividades.\n\r"+
                    "Actividades duplicadas o con errores en el archivo:"+fallosAct+"\n\r"+
                    "Horarios registrados correctamente:" + registrosHrs + "\n\r"+
                    "Registro de horarios con error:" + fallosHrs;
        }

        // Funcion que se encarga de hacer las validaciones previas
        // En caso de estar todo ok hace el insert
        public bool InsertarActividad(Actividad act)
        {
            bool success = false;
            if (act != null && validarNombreActividad(act) && validarEdadesActividad(act))
            {
                RepoActividad repoAct = new RepoActividad();
                success = repoAct.Alta(act); ;
            }
            return success;
        }

        public bool InsertarHorario(Horario hrs)
        {
            bool success = false;
            if (hrs != null && existeHorario(hrs) && validarHrComiezo(hrs))
            {
                RepoActividad repoAct = new RepoActividad();
                success = repoAct.AltaHorario(hrs); ;
            }
            return success;
        }


        // Funcion que Valida:
        // Nombre de la actividad no este vacio
        // No haya otra actividad con ese nombre
        public bool validarNombreActividad(Actividad act)
        {
            bool success = false;

            // Seteo variable para poder llamar al Repo
            // Valido que no haya una Actividad con el nombre ingresado en el doc
            RepoActividad RepoAct = new RepoActividad();
            Actividad nombreExistente = RepoAct.BusarPorNombre(act.Nombre);
            string nombre = act.Nombre.Trim(' ');
            if (nombre.Length > 0 && nombreExistente == null)
            {
                success = true;
            }

            return success;
        }


        // Funcion que Valida:
        // Rango de edad Max y Min sea correcto
        // El rango min no sea mayor al max
        public bool validarEdadesActividad(Actividad act)
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
        public bool existeHorario(Horario hrs)
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
        public bool validarHrComiezo(Horario hrs)
        {
            bool success = false;

            if (hrs.Hora == -1) return success;

            if (hrs.Hora >= 7 && hrs.Hora <= 23)
            {
                success = true;
            }

            return success;
        }

        // Funcion que se encarga de setear un valor numerico 
        // Es para que no explote si se pone algo difernte a un entero en el doc
        // Si no es un entero le asigno -1
        public int validarInt(string n)
        {
            int valorNumerico;
            bool esEntero = int.TryParse(n, out valorNumerico);

            if (!esEntero)
            {
                valorNumerico = -1;
            }

            return valorNumerico;
        }


        // DTO para construir un objeto con todos los datos del archivo
        public class DtoArchivo
        {
            public string Nombre { get; set; }
            public int Id { get; set; }
            public int EdadMin { get; set; }
            public int EdadMax { get; set; }
            public int Duracion { get; set; }
            public int CuposDisponibles { get; set; }
            public string Actividad { get; set; }
            public string Dia { get; set; }
            public int Hora { get; set; }
        }

        // Funcion que:
        // Obtiene la data del archivo y arma una lista de DTOArchivos para luego pasar
        private  List<DtoArchivo> ObtenerTodos()
        {
            List<DtoArchivo> retorno = new List<DtoArchivo>(); //Voy a retornar una lista de DTO
            using (StreamReader sr = File.OpenText(ArchivoPersonas)) // Metodo que abre el archivo
            {
                // Verifico que se pueda leer la linea y que no sea NULL
                string linea = sr.ReadLine();
                while ((linea != null))
                {
                    if (linea.IndexOf(delimitador) > 0) 
                    {
                        retorno.Add(ObtenerDesdeString(linea));
                    }
                    linea = sr.ReadLine();
                }

            }
            return retorno;

        }

        // Funcion que:
        // Consruye y devuelve el DTO en base al dato levantado en el archivo
        private DtoArchivo ObtenerDesdeString(string dato)
        {
            string[] datosObjeto = dato.Split(delimitador.ToCharArray());
            if (datosObjeto.Length == cantColumnas) //Verificar que la línea está ok
            {
                return new DtoArchivo
                {
                    Nombre = datosObjeto[0],
                    EdadMin = validarInt(datosObjeto[1]),
                    EdadMax = validarInt(datosObjeto[2]),
                    Duracion = validarInt(datosObjeto[3]),
                    CuposDisponibles = validarInt(datosObjeto[4]),
                    Actividad = datosObjeto[0],
                    Dia = datosObjeto[5],
                    Hora = validarInt(datosObjeto[6])
                };
            }
            else
            {
                return null;
            }
                
        }
    }
}

