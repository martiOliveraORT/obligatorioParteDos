using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Repositorio;
using System.Text.RegularExpressions;


namespace Fachada
{
    public class FachadaSocio
    {
        #region CRUD SOCIO
        public string AltaSocio(Socio newSocio)
        {
            string respuesta;

            bool resCedula = ValidarCedula(newSocio.Cedula);
            bool resNombre = ValidarNombre(newSocio.Nombre);
            bool resEdad = ValidarEdad(newSocio.FechaNac);
            Socio resRegistro = ValidarSocio(newSocio.Cedula);

            if (resCedula && resNombre && resEdad && resRegistro == null)
            {
                RepoSocio repo = new RepoSocio();
                newSocio.Estado = true;
                bool retornoAlta = repo.Alta(newSocio);
                if (!retornoAlta)
                {
                    respuesta = "Error al crear en BD";
                }
                else
                {
                    respuesta = "Se registro correctamente";
                }
            }
            else
            {
                respuesta = "Error en cedula, nombre, edad o ya existe";
            }

            return respuesta;
        }


        public string ModificarSocio(int cedula, string nombre, DateTime fechaNac)
        {
            string respuesta;
            bool resCedula = ValidarCedula(cedula);
            Socio resRegistro = ValidarSocio(cedula);
            if (!resCedula)
            {
                respuesta = "Formato incorrecto de cedula";
            }
            else if (resRegistro == null)
            {
                respuesta = "No existe este socio en la BD";
            }
            else
            {
                bool resNombre = ValidarNombre(nombre);
                bool resEdad = ValidarEdad(fechaNac);
                if(resNombre && resEdad)
                {
                    resRegistro.Nombre = nombre;
                    resRegistro.FechaNac = fechaNac;
                    RepoSocio repo = new RepoSocio();
                    bool retornoModificacion = repo.Modificacion(resRegistro);
                    if (!retornoModificacion)
                    {
                        respuesta = "Error al bajar en BD";
                    }
                    else
                    {
                        respuesta = "";
                    }
                }
                else
                {
                    respuesta = "Error en nombre o edad";
                }
            }

            return respuesta;
        }

        public (Socio, string) BuscarSocio(int cedula)
        {
            Socio respuesta;
            string mensaje;
            bool resCedula = ValidarCedula(cedula);
            Socio resRegistro = ValidarSocio(cedula);
            if (!resCedula)
            {
                respuesta = null;
                mensaje = "Cedula incorrecta";
            }
            else if (resRegistro == null)
            {
                respuesta = null;
                mensaje = "Ese socio no existe";
            }
            else
            {
                respuesta = resRegistro;
                mensaje = null;
            }

            return (respuesta, mensaje);
        }

        public (List<Socio>, string) ListarSocios()
        {
            List<Socio> socios;
            string mensaje;

            RepoSocio repo = new RepoSocio();
            socios = repo.TraerTodo();

            if (socios.Count == 0)
            {
                mensaje = "No se encuentran socios";
            }
            else if (socios == null)
            {
                mensaje = "Error al traer la lista";
            }
            else
            {
                mensaje = "OK";
            }
            return (socios, mensaje);
        }

        public List<RegistroActividad> BuscarActividadesPorSocio(int cedula, DateTime fecha)
        {
            fecha.ToString("yyyy-MM-dd");
            RepoRegistroActividad repo = new RepoRegistroActividad();
            List<RegistroActividad> lista = repo.ingresoSocioPorFecha(cedula, fecha);
            return lista;
        }

        public bool DarAltaSocio(int cedula, int estado)
        {
            RepoSocio repo = new RepoSocio();
            bool respuesta = repo.CambiarEstado(cedula, estado);
            return respuesta;
        }
        #endregion
        #region VALIDACIONES
        public static bool ValidarCedula(int ciIngresada)
        {
            bool respuesta = false;

            int digitos = (int)Math.Floor(Math.Log10(ciIngresada) + 1);

            if (digitos >= 7 && digitos <= 9)
            {
                respuesta = true;
            }

            return respuesta;
        }

        public static bool ValidarNombre(string nombreIngresado)
        {
            bool respuesta = false;
            int largoNombre = nombreIngresado.Length;
            int inicio = nombreIngresado.IndexOf(" ");
            int final = nombreIngresado.IndexOf(" ", largoNombre - 1, 1);
            bool contieneNumeros = Regex.IsMatch(nombreIngresado, @"^[a-zA-Z ]*$");

            if (largoNombre >= 6 && inicio != 0 && final != largoNombre - 1 && contieneNumeros &&(inicio >= 0 || final >= 0))
            {
                int cantEspacios = Regex.Matches(nombreIngresado, " ").Count;
                if (cantEspacios > 1)
                {
                    respuesta = false;
                }
                else
                {
                    respuesta = true;
                }
            }
            return respuesta;
        }

        public static bool ValidarEdad(DateTime fechaNacimientoIngresado)
        {
            bool respuesta = false;
            DateTime hoy = DateTime.Now;
            TimeSpan ts = hoy - fechaNacimientoIngresado;
            int difDays = ts.Days;
            int difYears = difDays / 365;

            if (difYears >= 3 && difYears <= 90)
            {
                respuesta = true;
            }

            return respuesta;
        }

        public Socio ValidarSocio(int cedula)
        {
            Socio respuesta;
            RepoSocio repo = new RepoSocio();
            Socio resp = repo.BuscarPorId(cedula);
            //Si resp es null, es decir que no se encuentra en la BD, retorno null
            if (resp == null)
            {
                respuesta = null;
            }
            else
            {
                //Si la resp no es vacia, es decir que existe el registro en la BD, retorno el socio
                respuesta = resp;
            }
            return respuesta;
        }
        #endregion
    }
}
