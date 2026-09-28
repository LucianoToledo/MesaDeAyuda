namespace MesaDeAyuda.Domain.Entities;

public class CanalNotificacion
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty; // Email, Sms
    public DateTime? FechaHoraBaja { get; set; }
}
