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
    class ImportarUsuarios
    {
        // Lugar donde va tomar el archivo
        private static string ArchivoUsuario = AppDomain.CurrentDomain.BaseDirectory + "archivos\\usuarios.txt";
        // Constante que setea la cantidad de columnas por linea a insertar
        private const int cantColumnas = 3;
        private string delimitador = "|";

        public string leerDocumentoActividad()
        {
            List<Usuario> listaTxt = ObtenerTodos();

            int regAceptados = 0; // Cantidad de registros exitosos usuarios
            int regFallidos = 0; // Cantidad de registros fallidos usarios

            RepoUsuario repoAct = new RepoUsuario();

            foreach (Usuario u in listaTxt)
            {
                // Llamo al insert pasandole el objeto armado
                bool successAct = InsertarUsuario(u);

                if (successAct)
                {
                    regAceptados++;
                }
                else
                {
                    regFallidos++;
                }
            }

            return "Se registraron " + regAceptados + " nuevas usuarios.\n\r" +
                    "Usuarios duplicados o con errores en el archivo:" + regFallidos + "\n\r";

        }


        // Funcion que se encarga de hacer las validaciones previas
        // En caso de estar todo ok hace el insert
        public bool InsertarUsuario(Usuario user)
        {
            bool success = false;
            if (user = null && (act) && validarEdadesActividad(act))
            {
                RepoUsuario repoUser = new RepoUsuario();
                success = repoUser.Alta(user); ;
            }
            return success;
        }





        // Funcion que:
        // Obtiene la data del archivo y arma una lista de DTOArchivos para luego pasar
        private List<Usuario> ObtenerTodos()
        {
            List<Usuario> retorno = new List<Usuario>(); //Voy a retornar una lista de DTO
            using (StreamReader sr = File.OpenText(ArchivoUsuario)) // Metodo que abre el archivo
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
        private Usuario ObtenerDesdeString(string dato)
        {
            string[] datosObjeto = dato.Split(delimitador.ToCharArray());
            if (datosObjeto.Length == cantColumnas) //Verificar que la línea está ok
            {
                return new Usuario
                {
                  Email = datosObjeto[0],
                  Password = datosObjeto[1],
                  unecryptedPassword = datosObjeto[2]

                };
            }
            else
            {
                return null;
            }

        }
    }
}
