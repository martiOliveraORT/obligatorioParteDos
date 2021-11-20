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
    public class ExportarArchivos
    {

        public string ExportarTodos()
        {
            string msg = "Algunos o todos los archivos fallaron al exportarse, verificar en:" + AppDomain.CurrentDomain.BaseDirectory + "\\Descargas";


            if(ExportarArchivoRegistroActividad() && ExportarArchivoActividades() && ExportarArchivoHorarios() && ExportarArchivoSocios() && ExportarArchivoUsuarios() && ExportarMensualidadCuponeras())
            {
                msg = "Archivos Exportados correctamente, verificar en:" + AppDomain.CurrentDomain.BaseDirectory + "\\Descargas";
            }


            return msg;
        }


        public bool ExportarArchivoRegistroActividad()
        {
            bool success = false;
            RepoRegistroActividad repo = new RepoRegistroActividad();
            List<RegistroActividad> list = new List<RegistroActividad>();
            list = repo.TraerTodo();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Descargas\\RegistroActividad.txt";
            string delimitador = "|";

            if (repo == null) return success;
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                    foreach (RegistroActividad r in list)
                    //El constructor de StreamWriter recibe el nombre del archivo y true si se desean
                    //agregar registros, false si se va a sobrescribir el archivo.
                    {
                        sw.WriteLine(r.Nombre + delimitador
                                     + r.Socio + delimitador
                                     + r.Fecha.ToString("yyyy-MM-dd") + delimitador
                                     + r.Hora);
                    }
                success = true;
            }
            catch (FileNotFoundException) { throw; }
            catch (PathTooLongException) { throw; }
            catch (InvalidDataException) { throw; }
            catch (DirectoryNotFoundException) { throw; }
            catch (DriveNotFoundException) { throw; }
            catch (Exception) { throw; }
            return success;
        }

        public bool ExportarArchivoActividades()
        {
            bool success = false;
            RepoActividad repo = new RepoActividad();
            List<Actividad> list = new List<Actividad>();
            list = repo.TraerTodo();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Descargas\\Actividades.txt";
            string delimitador = "|";

            if (repo == null) return success;
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                    foreach (Actividad r in list)
                    //El constructor de StreamWriter recibe el nombre del archivo y true si se desean
                    //agregar registros, false si se va a sobrescribir el archivo.
                    {
                        sw.WriteLine(r.Id + delimitador
                                     + r.Nombre + delimitador
                                     + r.EdadMin + delimitador
                                     + r.EdadMin + delimitador
                                     + r.EdadMax + delimitador
                                     + r.CuposDisponibles);
                    }
                success = true;
            }
            catch (FileNotFoundException) { throw; }
            catch (PathTooLongException) { throw; }
            catch (InvalidDataException) { throw; }
            catch (DirectoryNotFoundException) { throw; }
            catch (DriveNotFoundException) { throw; }
            catch (Exception) { throw; }
            return success;
        }

        public bool ExportarArchivoHorarios()
        {
            bool success = false;
            RepoActividad repo = new RepoActividad();
            List<Horario> list = new List<Horario>();
            list = repo.TraerTodosHorarios();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Descargas\\Horarios.txt";
            string delimitador = "|";

            if (repo == null) return success;
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                    foreach (Horario r in list)
                    //El constructor de StreamWriter recibe el nombre del archivo y true si se desean
                    //agregar registros, false si se va a sobrescribir el archivo.
                    {
                        sw.WriteLine(r.Actividad + delimitador
                                     + r.Dia + delimitador
                                     + r.Hora);
                    }

                success = true;
            }
            catch (FileNotFoundException) { throw; }
            catch (PathTooLongException) { throw; }
            catch (InvalidDataException) { throw; }
            catch (DirectoryNotFoundException) { throw; }
            catch (DriveNotFoundException) { throw; }
            catch (Exception) { throw; }
            return success;
        }

        public bool ExportarArchivoSocios()
        {
            bool success = false;
            RepoSocio repo = new RepoSocio();
            List<Socio> list = new List<Socio>();
            list = repo.TraerTodo();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Descargas\\Socios.txt";
            string delimitador = "|";

            if (repo == null) return success;
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                    foreach (Socio r in list)
                    //El constructor de StreamWriter recibe el nombre del archivo y true si se desean
                    //agregar registros, false si se va a sobrescribir el archivo.
                    {
                        sw.WriteLine(r.Cedula + delimitador
                                     + r.Nombre + delimitador
                                     + r.FechaIngreso.ToString("dd-MM-yyyy") + delimitador
                                     + r.FechaNac.ToString("dd-MM-yyyy") + delimitador
                                     + r.Estado);
                    }

                success = true;
            }
            catch (FileNotFoundException) { throw; }
            catch (PathTooLongException) { throw; }
            catch (InvalidDataException) { throw; }
            catch (DirectoryNotFoundException) { throw; }
            catch (DriveNotFoundException) { throw; }
            catch (Exception) { throw; }
            return success;
        }

        public bool ExportarArchivoUsuarios()
        {
            bool success = false;
            RepoUsuario repo = new RepoUsuario();
            List<Usuario> list = new List<Usuario>();
            list = repo.TraerTodo();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Descargas\\Usuarios.txt";
            string delimitador = "|";

            if (repo == null) return success;
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                    foreach (Usuario r in list)
                    //El constructor de StreamWriter recibe el nombre del archivo y true si se desean
                    //agregar registros, false si se va a sobrescribir el archivo.
                    {
                        sw.WriteLine(r.Email + delimitador
                                     + r.Password);
                    }
                success = true;
            }
            catch (FileNotFoundException) { throw; }
            catch (PathTooLongException) { throw; }
            catch (InvalidDataException) { throw; }
            catch (DirectoryNotFoundException) { throw; }
            catch (DriveNotFoundException) { throw; }
            catch (Exception) { throw; }
            return success;
        }

        public bool ExportarMensualidadCuponeras()
        {
            bool success = false;
            RepoMensualidad repo = new RepoMensualidad();
            List<Cuponera> list = new List<Cuponera>();
            list = repo.AllCuponeras();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Descargas\\Mensualidades.txt";
            string delimitador = "|";

            if (repo == null) return success;
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                    foreach (Cuponera r in list)
                    //El constructor de StreamWriter recibe el nombre del archivo y true si se desean
                    //agregar registros, false si se va a sobrescribir el archivo.
                    {
                        sw.WriteLine(r.Id + delimitador +
                                     r.Costo + delimitador +
                                     r.Fecha.ToString("dd-MM-yyyy") + delimitador +
                                     r.Socio.Cedula + delimitador +
                                     r.Descuento + delimitador +
                                     r.Tipo + delimitador +
                                     r.Vencimiento.ToString("dd-MM-yyyy") + delimitador +
                                     r.IngresosDisponibles); 
                    }
                success = ExportarMensualidadPases();
            }
            catch (FileNotFoundException) { throw; }
            catch (PathTooLongException) { throw; }
            catch (InvalidDataException) { throw; }
            catch (DirectoryNotFoundException) { throw; }
            catch (DriveNotFoundException) { throw; }
            catch (Exception) { throw; }
            return success;
        }

        public bool ExportarMensualidadPases()
        {
            bool success = false;
            RepoMensualidad repo = new RepoMensualidad();
            List<PaseLibre> list = new List<PaseLibre>();
            list = repo.AllPaseLibres();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Descargas\\Mensualidades.txt";
            string delimitador = "|";


            if (repo == null) return success;
            try
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                    foreach (PaseLibre r in list)
                    //El constructor de StreamWriter recibe el nombre del archivo y true si se desean
                    //agregar registros, false si se va a sobrescribir el archivo.
                    {
                        sw.WriteLine(r.Id + delimitador +
                                     r.Costo + delimitador +
                                     r.Fecha.ToString("dd-MM-yyyy") + delimitador +
                                     r.Socio.Cedula + delimitador +
                                     r.Descuento + delimitador +
                                     r.Tipo + delimitador +
                                     r.Vencimiento.ToString("dd-MM-yyyy")
                                     );
                    }
                success = true;
            }
            catch (FileNotFoundException) { throw; }
            catch (PathTooLongException) { throw; }
            catch (InvalidDataException) { throw; }
            catch (DirectoryNotFoundException) { throw; }
            catch (DriveNotFoundException) { throw; }
            catch (Exception) { throw; }
            return success;
        }
    }
}
