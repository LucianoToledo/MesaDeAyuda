namespace MesaDeAyuda.Domain.Entities;

public class CasoInstancia
{
    public int Id { get; set; }
    public int OrdenCasoInstancia { get; set; }
    public DateTime? FechaHoraInicioReal { get; set; }
    public DateTime? FechaHoraFinReal { get; set; }
    public DateTime? FechaHoraInicioPlanificada { get; set; }
    public DateTime? FechaHoraFinPlanificada { get; set; }
    public string Observaciones { get; set; } = string.Empty;

    // Relación con Caso (Padre)
    public int CasoId { get; set; }

    public Caso? Caso { get; set; }

    // Relación con Especialista (El que lo tiene asignado/tomado)
    public int? EspecialistaId { get; set; }

    public Especialista? Especialista { get; set; }

    // Relación con TipoInstancia (el Sector responsable se obtiene indirectamente a través de TipoInstancia.Sector)
    public int TipoInstanciaId { get; set; }

    public TipoInstancia? TipoInstancia { get; set; }

    // Relación con EstadoCasoInstancia
    public int EstadoId { get; set; }

    public EstadoCasoInstancia? EstadoActual { get; set; }

    public ICollection<CasoInstanciaTarea> Tareas { get; set; } = new List<CasoInstanciaTarea>();
}
