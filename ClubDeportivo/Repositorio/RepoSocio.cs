using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Repositorio
{
    public class RepoSocio : IRepositorio<Socio>
    {

        string cadena = Conexion.stringConexion;

        public bool Alta(Socio obj)
        {
            bool respuesta = false;
            if (obj == null) return respuesta;
            try
            {
                RepoContext db = new RepoContext(cadena);
                db.Socios.Add(obj);
                //Guardo los cambios
                db.SaveChanges();
                //Verificar que se haya creado el socio, buscamos en la bd
                Socio socio = db.Socios.Find(obj.Cedula);
                //Verificamos que no sea nulo, y retornamos true
                if(socio != null)
                {
                    respuesta = true;
                }
                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return respuesta;
        }
            
            public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public bool Modificacion(Socio obj)
        {
            bool respuesta = false;
            if (obj == null) return respuesta;
            try
            {
                RepoContext db = new RepoContext(cadena);
                //Buscamos el socio a modificar
                Socio socio = db.Socios.Find(obj.Cedula);
                //Verificamos que no sea nulo
                if (socio != null)
                {
                    //Cambiamos los datos
                    socio.Nombre = obj.Nombre;
                    socio.FechaNac = obj.FechaNac;
                    //Guardamos los cambios
                    db.SaveChanges();
                    db.Dispose();
                }
                //Verificamos que la modificacion se haya hecho
                Socio socioBuscado = db.Socios.Find(obj.Cedula);
                if(socioBuscado == obj)
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return respuesta;
        }

        public List<Socio> TraerTodo()
        {
            List<Socio> socios = null;
            try
            {
                //Creo el contexto
                RepoContext db = new RepoContext(cadena);
                //Obtengo la lista Ienumerable de socios
                IEnumerable<Socio>  sociosI = from Socio s 
                                              in db.Socios 
                                              select s;
                //La convierto a List
                socios = sociosI.ToList();
                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return socios;
        }

        public Socio BuscarPorId(int id)
        {
            Socio socio = null;
            //Verifico que el id no llege vacio
            if (id <= 0) return socio;
            try
            {
                //Creo el contexto
                RepoContext db = new RepoContext(cadena);
                //Busco el socio por su cedula
                socio = db.Socios.Find(id);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return socio;
        }

        public bool CambiarEstado(int cedula, int estado)
        {
            bool respuesta = false;
            if (cedula <= 0 || estado < 0) return respuesta;
            bool estadoI = false;
            if(estado == 1)
            {
                estadoI = true;
            }
            try
            {
                RepoContext db = new RepoContext(cadena);
                //Buscamos el socio a modificar
                Socio socio = db.Socios.Find(cedula);
                //Verificamos que no sea nulo
                if (socio != null)
                {
                    //Cambiamos el estado
                    socio.Estado = estadoI;
                    //Guardamos los cambios
                    db.SaveChanges();
                    db.Dispose();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return respuesta;
        }

    }
}
