namespace MesaDeAyuda.Domain.Entities;

public class Sector
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime? FechaHoraBaja { get; set; }

    public ICollection<Especialista> Especialistas { get; set; } = new List<Especialista>();
}