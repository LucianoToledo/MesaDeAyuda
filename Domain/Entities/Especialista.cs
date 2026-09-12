namespace MesaDeAyuda.Domain.Entities;

public class Especialista
{
    public int Id { get; set; }
    public int Legajo { get; set; }
    public int Cuit { get; set; }
    public string NombreApellido { get; set; } = string.Empty;
    public DateTime? FechaHoraBaja { get; set; }

    // Relación con Sector (Cada especialista pertenece a un sector)
    public int SectorId { get; set; }

    public Sector? Sector { get; set; }
}