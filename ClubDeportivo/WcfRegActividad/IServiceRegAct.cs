using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Dominio;

namespace WcfRegActividad
{
    [ServiceContract]
    public interface IServiceRegAct
    {
        // Operaciones principales del servicio
        [OperationContract]
        IEnumerable<DtoHorario> GetHorariosDisponibles();
        [OperationContract]
        bool AltaRegistro(int ci, DtoHorario regAct);
    }


    // Creo un DTO por cada tipo de obj que voy usar (RegistroActividad, Horario, Socio)
    #region DTO
    //REGISTROACTIVIDAD
    public class DtoRegistro
    {
        // Seteo los atributos que voy a usar
        [DataMember]
        public int Socio { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }

    };

    //HORARIO
    public class DtoHorario
    {

        [DataMember]
        public string Actividad { get; set; }
        [DataMember]
        public int Hora { get; set; }
        [DataMember]
        public int Id { get; set; }

    };


    #endregion
}