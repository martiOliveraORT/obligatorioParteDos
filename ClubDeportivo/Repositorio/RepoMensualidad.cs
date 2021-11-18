using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Repositorio
{
    public class RepoMensualidad : IRepositorio<Mensualidad>
    {
        public bool Alta(Mensualidad obj)
        {
            bool ok = false;
            if (obj.TipoMetodo() == "l")
            {
                PaseLibre ps = (PaseLibre)obj;
                ok = AltaPaseLibre(ps);
            }
            else if (obj.TipoMetodo() == "c")
            {
                Cuponera cup = (Cuponera)obj;
                ok = AltaCuponera(cup);
            }
            return ok;
        }

        public bool AltaPaseLibre(PaseLibre obj)
        {
            bool success = false;
            //INICIO LA CONEXION CON LA BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            //CREAMOS LA QUERY A EJECUTAR LUEGO
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"Insert into mensualidad (costo, fecha, socio, descuento, tipo, vencimiento) Values (@costo, @fecha, @socio, @descuento, @tipo, @vencimiento)"
            };
            //SETEAMOS LOS DATOS CON SU RESPECTIVA VARIABLE
            cmd.Parameters.AddWithValue("@costo", obj.Costo);
            cmd.Parameters.AddWithValue("@fecha", obj.Fecha);
            cmd.Parameters.AddWithValue("@socio", obj.Socio.Cedula);
            cmd.Parameters.AddWithValue("@vencimiento", obj.Vencimiento);
            cmd.Parameters.AddWithValue("@tipo", obj.TipoMetodo());
            cmd.Parameters.AddWithValue("@descuento", obj.Descuento);
            cmd.Connection = cn;

            //INTENTAMOS EJECUTAR LA QUERY CORRECTAMENTE
            //SI EL RESULTADO DE LA FILA ES 1, GUARDAMOS LA VARIABLE Y RETORNAMOS TRUE
            //SI FALLA TRAEMOS Y MSOTRAMOS EL ERROR
            try
            {
                manejadorConexion.AbrirConexion(cn);
                int rows = cmd.ExecuteNonQuery();

                if (rows == 1)
                {
                    success = true;
                }
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                // CERRAMOS LA CONEXION
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public bool AltaCuponera(Cuponera obj)
        {
            bool success = false;
            //INICIO LA CONEXION CON LA BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            //CREAMOS LA QUERY A EJECUTAR LUEGO
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"Insert into mensualidad (costo, fecha, socio, descuento, tipo, vencimiento, ingresosDisp) Values (@costo, @fecha, @socio, @descuento, @tipo, @vencimiento, @ingresosDisp)"
            };
            //SETEAMOS LOS DATOS CON SU RESPECTIVA VARIABLE
            cmd.Parameters.AddWithValue("@costo", obj.Costo);
            cmd.Parameters.AddWithValue("@fecha", obj.Fecha);
            cmd.Parameters.AddWithValue("@socio", obj.Socio.Cedula);
            cmd.Parameters.AddWithValue("@vencimiento", obj.Vencimiento);
            cmd.Parameters.AddWithValue("@tipo", obj.TipoMetodo());
            cmd.Parameters.AddWithValue("@ingresosDisp", obj.IngresosDisponibles);
            cmd.Parameters.AddWithValue("@descuento", obj.Descuento);
            cmd.Connection = cn;

            //INTENTAMOS EJECUTAR LA QUERY CORRECTAMENTE
            //SI EL RESULTADO DE LA FILA ES 1, GUARDAMOS LA VARIABLE Y RETORNAMOS TRUE
            //SI FALLA TRAEMOS Y MSOTRAMOS EL ERROR
            try
            {
                manejadorConexion.AbrirConexion(cn);
                int rows = cmd.ExecuteNonQuery();

                if (rows == 1)
                {
                    success = true;
                }
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                // CERRAMOS LA CONEXION
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public (decimal, decimal, int) TraerValoresPaseLibre()
        {
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            decimal valorCuota = 0;
            decimal descAntig = 0;
            int antig = 0;

            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"select valorCuota, descuentoAntig, antiguedad from generalidades where id = 1"
            };
            cmd.Connection = cn;
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader reader = cmd.ExecuteReader();
                
                if (reader.Read())
                {
                    valorCuota = (decimal)reader["valorCuota"];
                    descAntig = (decimal)reader["descuentoAntig"];
                    antig = (int)reader["antiguedad"];
                }
                return (valorCuota, descAntig, antig);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (valorCuota, descAntig, antig);      
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public (decimal, decimal, int) TraerValoresCuponera()
        {
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            decimal precioUnitario = 0;
            decimal descCup = 0;
            int cantAct = 0;

            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"select precioUnitario, descuentoCantAct, cantActividades from generalidades where id = 1"
            };
            cmd.Connection = cn;
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    precioUnitario = (decimal)reader["precioUnitario"];
                    descCup = (decimal)reader["descuentoCantAct"];
                    cantAct = (int)reader["cantActividades"];

                }
                return (precioUnitario, descCup, cantAct);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (precioUnitario, descCup, cantAct);
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public bool Modificacion(Mensualidad obj)
        {
            throw new NotImplementedException();
        }

        public List<Mensualidad> TraerTodo()
        {
            throw new NotImplementedException();
        }

        public Mensualidad BuscarPorId(int ci)
        {
            RepoSocio repoSocio = new RepoSocio();
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            Mensualidad mens = null;

            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"select top 1 * from mensualidad where socio = @ci order by vencimiento desc"
            };

            cmd.Parameters.AddWithValue("@ci", ci);
            cmd.Connection = cn;

            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if ((string)reader["tipo"] == "l")
                    {
                        mens = new PaseLibre
                        {
                            Id = (int)reader["id"],
                            Costo = (decimal)reader["costo"],
                            Fecha = (DateTime)reader["fecha"],
                            Socio = repoSocio.BuscarPorId((int)reader["socio"]),
                            Vencimiento = (DateTime)reader["vencimiento"],
                            Descuento = (decimal)reader["descuento"]
                        };
                    }
                    else if ((string)reader["tipo"] == "c")
                    {
                        mens = new Cuponera
                        {
                            Id = (int)reader["id"],
                            Costo = (decimal)reader["costo"],
                            Fecha = (DateTime)reader["fecha"],
                            Socio = repoSocio.BuscarPorId((int)reader["socio"]),
                            Vencimiento = (DateTime)reader["vencimiento"],
                            Descuento = (decimal)reader["descuento"],
                            IngresosDisponibles = (int)reader["ingresosDisp"]                            
                        };
                    }
                }
                return mens;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return mens;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public bool RestarCupo(int id)
        {
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            bool respuesta = false;

            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"UPDATE mensualidad SET ingresosDisp = ingresosDisp-1 WHERE vencimiento > GETDATE() and socio = @socio and tipo = 'c'"
            };

            cmd.Parameters.AddWithValue("@socio", id);
            cmd.Connection = cn;

            try
            {
                manejadorConexion.AbrirConexion(cn);
                int afectadas = cmd.ExecuteNonQuery();

                if (afectadas == 1)
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }
                return respuesta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return respuesta;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public List<PaseLibre> AllPaseLibres()
        {
            //INICIO LA CONEXION CON LA BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();
            List<PaseLibre> pases = new List<PaseLibre>();
            RepoSocio repoSocio = new RepoSocio();
            Socio soc = new Socio();



            //CREAMOS LA QUERY A EJECUTAR LUEGO
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"SELECT * FROM mensualidad WHERE tipo ='l'"
            };
            //SETEAMOS LOS DATOS CON SU RESPECTIVA VARIABLE

            cmd.Connection = cn;

            //INTENTAMOS EJECUTAR LA QUERY CORRECTAMENTE
            //SI EL RESULTADO DE LA FILA ES 1, GUARDAMOS LA VARIABLE Y RETORNAMOS TRUE
            //SI FALLA TRAEMOS Y MSOTRAMOS EL ERROR
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader filas = cmd.ExecuteReader();


                while (filas.Read())
                {
                    int ci = (int)filas["socio"];
                    soc = repoSocio.BuscarPorId(ci);

                    pases.Add(new PaseLibre
                    {
                        Id = (int)filas["id"],
                        Costo = (decimal)filas["costo"],
                        Fecha = (DateTime)filas["fecha"],
                        Socio = soc,
                        Descuento = (decimal)filas["descuento"],
                        Vencimiento = (DateTime)filas["vencimiento"],
                    });
                }

                return pases;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return pases;
            }
            finally
            {
                // CERRAMOS LA CONEXION
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public List<Cuponera> AllCuponeras()
        {
            //INICIO LA CONEXION CON LA BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();
            List<Cuponera> cuponeras = new List<Cuponera>();
            RepoSocio repoSocio = new RepoSocio();
            Socio soc = new Socio();



            //CREAMOS LA QUERY A EJECUTAR LUEGO
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"SELECT * FROM mensualidad WHERE tipo ='c'"
            };
            //SETEAMOS LOS DATOS CON SU RESPECTIVA VARIABLE

            cmd.Connection = cn;

            //INTENTAMOS EJECUTAR LA QUERY CORRECTAMENTE
            //SI EL RESULTADO DE LA FILA ES 1, GUARDAMOS LA VARIABLE Y RETORNAMOS TRUE
            //SI FALLA TRAEMOS Y MSOTRAMOS EL ERROR
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader filas = cmd.ExecuteReader();


                while (filas.Read())
                {
                    int ci = (int)filas["socio"];
                    soc = repoSocio.BuscarPorId(ci);

                    cuponeras.Add(new Cuponera
                    {
                        Id = (int)filas["id"],
                        Costo = (decimal)filas["costo"],
                        Fecha = (DateTime)filas["fecha"],
                        Socio = soc,
                        Descuento = (decimal)filas["descuento"],
                        Vencimiento = (DateTime)filas["vencimiento"],
                        IngresosDisponibles = (int)filas["ingresosDisp"]
                    }) ;
                }

                return cuponeras;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return cuponeras;
            }
            finally
            {
                // CERRAMOS LA CONEXION
                manejadorConexion.CerrarConexion(cn);
            }
        }

    }
}


