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
        string cadena = Conexion.stringConexion;

        public bool Alta(Mensualidad obj)
        {
            bool ok = false;

            if (obj.Tipo == "l")

            {
                PaseLibre ps = (PaseLibre)obj;
                ok = AltaPaseLibre(ps);
            }

            else if (obj.Tipo == "c")

            {
                Cuponera cup = (Cuponera)obj;
                ok = AltaCuponera(cup);
            }
            return ok;
        }

        public bool AltaPaseLibre(PaseLibre obj)
        {

            bool ok = false;
            if (obj == null) return ok;

            try
            {
                RepoContext db = new RepoContext(cadena);
                obj.CiSocio = obj.Socio.Cedula;
                db.Mensualidades.Add(obj);
                db.Entry(obj.Socio).State = System.Data.Entity.EntityState.Unchanged;
                db.SaveChanges();
                //Verificar que se creo la mensualidad
                Mensualidad mens = BuscarPorId(obj.Socio.Cedula);
                if (mens.Fecha == DateTime.Today)
                {
                    ok = true;
                }
                db.Dispose();
            }
            catch ( Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ok;
        }

        public bool AltaCuponera(Cuponera obj)
        {

            bool ok = false;
            if (obj == null) return ok;

            try
            {
                RepoContext db = new RepoContext(cadena);
                obj.CiSocio = obj.Socio.Cedula;                
                db.Mensualidades.Add(obj);
                db.Entry(obj.Socio).State = System.Data.Entity.EntityState.Unchanged;
                db.SaveChanges();
                //Verificar que se guardo
                Mensualidad mens = BuscarPorId(obj.Socio.Cedula);
                if (mens.Fecha == DateTime.Today)
                {
                    ok = true;
                }
                db.Dispose();
            }            
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.InnerException);
                Console.WriteLine(ex.Message);
            }
            return ok;
        }        

        public (decimal, decimal, int) TraerValoresPaseLibre()
        {
            decimal valorCuota = 0;
            decimal descAntig = 0;
            int antig = 0;
            try
            {
                RepoContext db = new RepoContext(cadena);

                var query = (from g in db.Generalidades
                            select g).SingleOrDefault();                

                if (query != null)
                {
                    valorCuota = query.ValorCuota;
                    descAntig = query.DescuentoAntig;
                    antig = query.Antiguedad;
                }
            }
            catch ( Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (valorCuota, descAntig, antig);
        }    

        public (decimal, decimal, int) TraerValoresCuponera()
        {
            decimal precioUnitario = 0;
            decimal descCup = 0;
            int cantAct = 0;
            try
            {
                RepoContext db = new RepoContext(cadena);
                var query = (from g in db.Generalidades
                             select g).SingleOrDefault();
                if (query != null)
                {
                    precioUnitario = query.PrecioUnitario;
                    descCup = query.DescuentoCantAct;
                    cantAct = query.CantActividades;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (precioUnitario, descCup, cantAct);
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

        public List<Mensualidad> TraerPorFecha(DateTime desde)
        {            
            DateTime hasta = desde.AddMonths(1).AddDays(-1);
            List<Mensualidad> aux = new List<Mensualidad>();

            RepoContext db = new RepoContext(cadena);
            try
            {
                var query = (from m in db.Mensualidades
                             where m.Vencimiento <= hasta && m.Vencimiento >= desde
                             select m).ToList();

                aux = AddSocioObjToList(query);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return aux;                    
        }

        public List<Mensualidad> AddSocioObjToList(List<Mensualidad> query)
        {
            RepoSocio rSocio = new RepoSocio();           
                       
            foreach (Mensualidad item in query)
            {
                item.Socio = rSocio.BuscarPorId(item.CiSocio);
            }
            return query;
        }

        public Mensualidad BuscarPorId(int ci)
        {
            Mensualidad mensualidad = null;
            try
            {
                RepoContext db = new RepoContext(cadena);
                List<Mensualidad> aux = db.Mensualidades.Where(m => m.Socio.Cedula == ci).ToList();
                aux = AddSocioObjToList(aux);
                mensualidad = aux.LastOrDefault();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return mensualidad;
        }
                
        public bool RestarCupo(int id)
        {
            RepoContext db = new RepoContext(cadena);
            bool ok = false;            
            Cuponera cup = null;
            try
            {
                //Trae las cuponeras que aun no vencieron de ese socio
                var query = db.Mensualidades.OfType<Cuponera>().Where(m => m.CiSocio == id && m.Vencimiento > DateTime.Today).ToList();
                //Nos quedamos con la ultima mensualidad que contrato, ya que ese mes pudo haber gastado los cupos de una y pedido otra
                //sin haber vencido aun la primera
                int max_id = query.Max(q => q.Id);
                cup = query.Where(c => c.Id == max_id).Single();
                //cup = TraerCuponera(id);

                if (cup != null)
                {                    
                    cup.IngresosDisponibles = cup.IngresosDisponibles -1 ;                   
                    db.SaveChanges();
                    ok = true;
                }
                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                
            }
            return ok;     
        }
        
        public List<PaseLibre> AllPaseLibres()
        {
            RepoContext db = new RepoContext(cadena);
            IEnumerable<PaseLibre> pases = null;
            List<PaseLibre> aux = null;
            try
            {
                pases = db.Mensualidades.OfType<PaseLibre>();
                /*pases = from PaseLibre m in db.Mensualidades
                        where m.Tipo == "l"
                        select m;*/

                aux = pases.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return aux;            
        }

        public List<Cuponera> AllCuponeras()
        {
            RepoContext db = new RepoContext(cadena);
            IEnumerable<Cuponera> cuponeras = null;
            List<Cuponera> aux = null;
            try
            {
                cuponeras = db.Mensualidades.OfType<Cuponera>();
                /*cuponeras = from Cuponera c in db.Mensualidades
                            where c.Tipo == "c"
                            select c;*/

                aux = cuponeras.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return aux;
        }        

        public List<Cuponera> CuponerasVigentes(int ci)
        {
            RepoContext db = new RepoContext(cadena);
            List<Cuponera> listCup = null;
            try
            {
                listCup = db.Mensualidades.OfType<Cuponera>().Where(m => m.CiSocio == ci && m.Vencimiento > DateTime.Today).ToList();                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listCup;
        }

        public Cuponera TraerCuponera(int ci)
        {
            RepoContext db = new RepoContext(cadena);
            Cuponera cup = null;
            try
            {
                List<Cuponera> aux = CuponerasVigentes(ci);
                int max_id = aux.Max(q => q.Id);
                cup = aux.Where(c => c.Id == max_id).Single();                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cup;
        }
    }
}


