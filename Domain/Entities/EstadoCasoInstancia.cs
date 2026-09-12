namespace MesaDeAyuda.Domain.Entities;

public class EstadoCasoInstancia
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty; // Asignada, Resuelto, Cancelada, etc.
    public DateTime? FechaHoraBaja { get; set; }
}