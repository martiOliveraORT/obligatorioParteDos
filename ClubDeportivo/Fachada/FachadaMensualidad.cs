using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Repositorio;

namespace Fachada
{
    public class FachadaMensualidad
    {
        FachadaSocio fachadaSocio = new FachadaSocio();
        RepoMensualidad repoMensualidad = new RepoMensualidad();
        public (decimal, decimal, decimal, int) MostrarCostoPL(int ci)
        {
            Socio socio = fachadaSocio.ValidarSocio(ci);          
            var (valorCuota, porcDescuento, antig) = repoMensualidad.TraerValoresPaseLibre();
            decimal costo = CalcularCostoPL(porcDescuento, valorCuota, antig, socio.FechaIngreso);

            return (costo, porcDescuento, valorCuota, antig);
        }    

        public (decimal, decimal, decimal, int) MostrarCostoCup(int ci, int ingresosDisp)
        {
            Socio socio = fachadaSocio.ValidarSocio(ci);
            var (precioUnitario, porcDescuento, cantAct) = repoMensualidad.TraerValoresCuponera();
            decimal costo = CalcularCostoCup(porcDescuento, precioUnitario, cantAct, ingresosDisp);

            return (costo, porcDescuento, precioUnitario, cantAct);
        }

        public (bool, string) AltaMensualidadPL(int ci)
        {
            bool ok = false;
            string msj;
          
            DateTime fechaHoy = DateTime.Today;
            DateTime fechaVencimiento = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
            Socio socio = fachadaSocio.ValidarSocio(ci);            
            decimal costo;
            Mensualidad mens = repoMensualidad.BuscarPorId(ci);
            //Verifico existencia del socio
            if (socio != null)
            {
                //Verifico si ese socio ya tiene una mensualidad, si no la tiene pasa a crearla directamente
                if (mens != null)
                {
                    //Verifico que la mensualidad haya vencido para ingresar una nueva para ese socio
                    if (mens.Vencimiento < fechaHoy)
                    {   
                        var (valorCuota, porcDescuento, antig) = repoMensualidad.TraerValoresPaseLibre();
                        costo = CalcularCostoPL(porcDescuento, valorCuota, antig, socio.FechaIngreso);
                        PaseLibre ps = new PaseLibre()
                        { 
                             Costo = costo,
                             Fecha = fechaHoy,
                             Socio = socio,
                             Vencimiento = fechaVencimiento,
                             Descuento = porcDescuento
                        };
                        ok = repoMensualidad.AltaPaseLibre(ps);                         

                        if (ok)
                        {
                            msj = "Se registro el pase libre con exito";
                        }
                        else
                        {
                            msj = "No fue posible registrar el pase libre";
                        }
                    }
                    else
                    {
                        //Si el usuario tiene mensualidad sin vencer, verifico que no tenga ingresos si es cuponera
                        //Asi, si la cuponera no cumplio vencimiento pero ya uso sus cupos, puede agregar una mensualidad nueva
                        if(mens.Tipo == "c")
                        {
                            Cuponera cup = repoMensualidad.TraerCuponera(mens.CiSocio);
                            if (cup.IngresosDisponibles == 0)
                            {
                                var (valorCuota, porcDescuento, antig) = repoMensualidad.TraerValoresPaseLibre();
                                costo = CalcularCostoPL(porcDescuento, valorCuota, antig, socio.FechaIngreso);
                                PaseLibre ps = new PaseLibre()
                                {
                                    Costo = costo,
                                    Fecha = fechaHoy,
                                    Socio = socio,
                                    Vencimiento = fechaVencimiento,
                                    Descuento = porcDescuento
                                };
                                ok = repoMensualidad.AltaPaseLibre(ps);

                                if (ok)
                                {
                                    msj = "Se registro el pase libre con exito";
                                }
                                else
                                {
                                    msj = "No fue posible registrar el pase libre";
                                }
                            }
                            else 
                            {
                                msj = "Este usuario aun tiene ingresos en su cuponera";
                            }
                        }
                        else
                        {
                            msj = "Este usuario ya tiene una mensualidad activa";
                        }
                    }
                }
                else
                {
                    var (valorCuota, porcDescuento, antig) = repoMensualidad.TraerValoresPaseLibre();
                    costo = CalcularCostoPL(porcDescuento, valorCuota, antig, socio.FechaIngreso);
                    PaseLibre ps = new PaseLibre()
                    {
                        Costo = costo,
                        Fecha = fechaHoy,
                        Socio = socio,
                        Vencimiento = fechaVencimiento,
                        Descuento = porcDescuento
                    };
                    ok = repoMensualidad.AltaPaseLibre(ps);

                    if (ok)
                    {
                        msj = "Se registro el pase libre con exito";
                    }
                    else
                    {
                        msj = "No fue posible registrar el pase libre";
                    }
                }
            }
            else 
            {    
                msj = "La cedula ingresada no existe en el registro";
            }
            return (ok, msj);
        }

        public (bool, string) AltaMensualidadCuponera(int ci, int ingresosDisp)
        {
            bool ok = false;
            string msj;
            if (ingresosDisp < 8 || ingresosDisp > 60)
            {
                msj = "La cantidad de actividades debe ser mayor a 8 y menor de 60";
            }
            else
            {              
                DateTime fechaHoy = DateTime.Today;
                DateTime fechaVencimiento = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
                Socio socio = fachadaSocio.ValidarSocio(ci);
                decimal costo;
                Mensualidad mens = repoMensualidad.BuscarPorId(ci);
                //Verifico existencia del socio
                if (socio != null)
                {
                    //Verifico si ese socio ya tiene una mensualidad, si no la tiene pasa a crearla directamente
                    if (mens != null)
                    {
                        //Verifico que la mensualidad haya vencido para ingresar una nueva para ese socio
                        if (mens.Vencimiento < fechaHoy)
                        {                         
                            var (precioUnitario, porcDescuento, cantAct) = repoMensualidad.TraerValoresCuponera();
                            costo = CalcularCostoCup(porcDescuento, precioUnitario, cantAct, ingresosDisp);
                            Cuponera cup = new Cuponera()
                            {
                                Costo = costo,
                                Fecha = fechaHoy,
                                Socio = socio,
                                Vencimiento = fechaVencimiento,
                                IngresosDisponibles = ingresosDisp,
                                Descuento = porcDescuento
                            };
                            ok = repoMensualidad.AltaCuponera(cup);                
                        
                            if (ok)
                            {
                                msj = "Se registro la cuponera con exito";
                            }
                            else
                            {
                                msj = "No fue posible registrar la cuponera";
                            }
                        }
                        else
                        {
                            //Si el usuario tiene mensualidad sin vencer, verifico que no tenga ingresos si es cuponera
                            //Asi, si la cuponera no cumplio vencimiento pero ya uso sus cupos, puede agregar una mensualidad nueva
                            if (mens.Tipo == "c")
                            {
                                Cuponera aux = repoMensualidad.TraerCuponera(mens.CiSocio);
                                if (aux.IngresosDisponibles == 0)
                                {
                                    var (precioUnitario, porcDescuento, cantAct) = repoMensualidad.TraerValoresCuponera();
                                    costo = CalcularCostoCup(porcDescuento, precioUnitario, cantAct, ingresosDisp);
                                    Cuponera cup = new Cuponera()
                                    {
                                        Costo = costo,
                                        Fecha = fechaHoy,
                                        Socio = socio,
                                        Vencimiento = fechaVencimiento,
                                        IngresosDisponibles = ingresosDisp,
                                        Descuento = porcDescuento
                                    };
                                    ok = repoMensualidad.AltaCuponera(cup);

                                    if (ok)
                                    {
                                        msj = "Se registro la cuponera con exito";
                                    }
                                    else
                                    {
                                        msj = "No fue posible registrar la cuponera";
                                    }
                                }
                                else
                                {
                                    msj = "Este usuario aun tiene ingresos en su cuponera";
                                }
                            }
                            else
                            {
                                msj = "Este usuario ya tiene una mensualidad activa";
                            }
                        }
                    }
                    else
                    {
                        var (precioUnitario, porcDescuento, cantAct) = repoMensualidad.TraerValoresCuponera();
                        costo = CalcularCostoCup(porcDescuento, precioUnitario, cantAct, ingresosDisp);
                        Cuponera cup = new Cuponera()
                        {
                            Costo = costo,
                            Fecha = fechaHoy,
                            Socio = socio,
                            Vencimiento = fechaVencimiento,
                            IngresosDisponibles = ingresosDisp,
                            Descuento = porcDescuento
                        };
                        ok = repoMensualidad.AltaCuponera(cup);

                        if (ok)
                        {
                            msj = "Se registro la cuponera con exito";
                        }
                        else
                        {
                            msj = "No fue posible registrar la cuponera";
                        }
                    }
                }
                else
                {
                    msj = "La cedula ingresada no existe en el registro";
                }
            }
            return (ok, msj);
        }

        public (Mensualidad, string) BuscarMesualidad(int cedula)
        {
            string msj;
            Mensualidad mens;

            RepoMensualidad repo = new RepoMensualidad();
            mens = repo.BuscarPorId(cedula);

            if (mens == null)
            {
                msj = "Error al buscar en BD";
            }
            else
            {
                msj = "OK";
            }
            return (mens, msj);
        }

        public bool RestarCupoCuponera(int cedula)
        {
            bool res;

            RepoMensualidad repo = new RepoMensualidad();
            res = repo.RestarCupo(cedula);

            return res;
        }

        public bool AplicaDescPL(DateTime fchIng, int antig)
        {
            bool ok = false;
            DateTime hoy = DateTime.Today;
            DateTime fchAntig = fchIng.AddMonths(antig);
            if (hoy > fchAntig)
            {
                ok = true;
            }

            return ok;
        }

        public bool AplicaDescCup(int ingDisp, int cantAct)
        {
            bool ok = false;
            if (ingDisp > cantAct)
            {
                ok = true;
            }
            return ok;
        }

        public decimal CalcularCostoPL(decimal porcDescuento, decimal valorCuota, int antig, DateTime fchIng)
        {
            decimal costo;                 
            if (AplicaDescPL(fchIng, antig))
            {
                decimal descuento = (porcDescuento * valorCuota) / 100;
                costo = valorCuota - descuento;
            }
            else
            {
                costo = valorCuota;
            }
            return costo;
        }

        public decimal CalcularCostoCup(decimal porcDescuento, decimal precioUnitario, int cantAct, int ingDisp)
        {
            decimal costo;
            decimal precioTotal = precioUnitario * ingDisp;
            if (AplicaDescCup(ingDisp, cantAct))
            {
                decimal descuento = (porcDescuento * precioTotal) / 100;
                costo = precioTotal - descuento;
            }
            else
            {
                costo = precioTotal;
            }
            return costo;
        }

        public List<Mensualidad> MensualidadesPorFecha(int mes, int año)
        {
            DateTime date = new DateTime(año, mes, 1);
            List<Mensualidad> aux = repoMensualidad.TraerPorFecha(date);

            return aux;
        }        

        public bool Validar4Cifras(int año)
        {
            bool ok = false;
            int contador = 0;
            while (año > 0)
            {
                año /= 10;
                contador ++;
            }
            if (contador == 4) ok = true;            
            
            return ok;
        }

        public Cuponera TraerUltimaCuponera(int ci)
        {
            Cuponera cup = repoMensualidad.TraerCuponera(ci);

            return cup;
        }
    }
}
