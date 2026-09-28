namespace MesaDeAyuda.Domain.Entities;

// Configuración a nivel empresa (una sola fila), no por caso ni por cliente: qué canal de
// notificación está habilitado hoy para avisarle al cliente que su caso fue resuelto.
public class ConfiguracionNotificacion
{
    public int Id { get; set; }

    public int CanalHabilitadoId { get; set; }
    public CanalNotificacion? CanalHabilitado { get; set; }
}
