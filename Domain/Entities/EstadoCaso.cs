namespace MesaDeAyuda.Domain.Entities;

public class EstadoCaso
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty; // Tomado, Disponible, Cerrado, etc.
    public DateTime? FechaHoraBaja { get; set; }
}