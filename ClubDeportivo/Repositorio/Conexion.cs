using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio
{
    class Conexion
    {
        // Constante para llamar la conexion en los nuevos Repos

        public const string stringConexion = 
            "server=FEDE-PC; database=obligatorio2P3; INTEGRATED SECURITY=TRUE;";
        // HAY QUE CAMBIAR ESTA VARIABLE CON LOS DATOS DE NUESTRA BD
        private readonly string cadenaConexion =
            @"Data Source=FEDE-PC; Initial Catalog=obligatorioP3; Integrated Security=SSPI;";

        public SqlConnection CrearConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
        /*public enum EstadosConexion
		{
			abierto, cerrado,enproceso
		}*/
        public bool AbrirConexion(SqlConnection cn)
        {

            if (cn == null)
                return false;
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
                return true;
            }
            return false;
        }

        public bool CerrarConexion(SqlConnection cn)
        {
            if (cn == null)
                return false;
            if (cn.State != ConnectionState.Closed)
            {
                cn.Close();
                cn.Dispose();//liberar los recursos "tomados" por la conexión
                return true;
            }
            return false;
        }
    }
}
