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
    public class ImportarUsuarios
    {
        // Lugar donde va tomar el archivo
        private static string ArchivoUsuario = AppDomain.CurrentDomain.BaseDirectory + "archivos\\usuarios.txt";
        // Constante que setea la cantidad de columnas por linea a insertar
        private const int cantColumnas = 2;
        private string delimitador = "|";

        public string leerDocumentoUsuario()
        {
            List<Usuario> listaTxt = ObtenerTodos();

            int regAceptados = 0; // Cantidad de registros exitosos usuarios
            int regFallidos = 0; // Cantidad de registros fallidos usarios


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

            string mensaje = "Cantidad de lineas en el archivo:" + listaTxt.Count + " |Se registraron: " + regAceptados + " nuevos usuarios. "+
                    "|Usuarios duplicados o con errores en el archivo:" + regFallidos;
            
            return mensaje;

        }


        // Funcion que se encarga de hacer las validaciones previas
        // En caso de estar todo ok hace el insert
        public bool InsertarUsuario(Usuario user)
        {
            bool success = false;

            if (user != null && FachadaUsuario.ValidarEmail(user.Email) && FachadaUsuario.ValidarPassword(user.unecryptedPassword))
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
                string encryptedPassword = FachadaUsuario.Encriptar(datosObjeto[1]);
                return new Usuario
                {
                  Email = datosObjeto[0],
                  Password = encryptedPassword,
                  unecryptedPassword = datosObjeto[1]

                };
            }
            else
            {
                return null;
            }

        }
    }
}
