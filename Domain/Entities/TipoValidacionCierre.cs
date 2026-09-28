namespace MesaDeAyuda.Domain.Entities;

public class TipoValidacionCierre
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty; // Simple, PorObservaciones, PorTareaRegistrada
    public DateTime? FechaHoraBaja { get; set; }
}
