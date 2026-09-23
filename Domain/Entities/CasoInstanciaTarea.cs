namespace MesaDeAyuda.Domain.Entities;

public class CasoInstanciaTarea
{
    public int Id { get; set; }
    public DateTime? FechaHoraInicio { get; set; }
    public DateTime? FechaHoraFin { get; set; }
    public string Observaciones { get; set; } = string.Empty;

    public int CasoInstanciaId { get; set; }
    public CasoInstancia? CasoInstancia { get; set; }

    public int TipoTareaId { get; set; }
    public TipoTarea? TipoTarea { get; set; }
}