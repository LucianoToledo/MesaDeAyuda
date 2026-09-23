namespace MesaDeAyuda.Domain.Entities;

public class TipoTarea
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime? FechaHoraBaja { get; set; }
}