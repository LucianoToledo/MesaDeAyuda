namespace MesaDeAyuda.Domain.Entities;

// Configuración a nivel empresa, no por caso ni por cliente: qué canal de notificación está
// habilitado para avisarle al cliente que su caso fue resuelto. Con vigencia (FechaAlta/FechaBaja),
// igual que TipoCasoTipoInstancia: es un historial de qué canal estuvo habilitado en cada período,
// no una fila fija, por eso puede haber más de una instancia a lo largo del tiempo.
public class ConfiguracionNotificacion
{
    public int Id { get; set; }
    public DateTime FechaAlta { get; set; }
    public DateTime? FechaBaja { get; set; }

    public int CanalHabilitadoId { get; set; }
    public CanalNotificacion? CanalHabilitado { get; set; }
}