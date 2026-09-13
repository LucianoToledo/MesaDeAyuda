using MesaDeAyuda.DTOs;

namespace MesaDeAyuda.Adapters;

// Simulado: no hay proveedor de email real configurado todavía.
public class AdaptadorNotificacionEmail : IAdaptadorNotificacionCliente
{
    private readonly ILogger<AdaptadorNotificacionEmail> _logger;

    public AdaptadorNotificacionEmail(ILogger<AdaptadorNotificacionEmail> logger)
    {
        _logger = logger;
    }

    public bool Notificar(DTONotificacionCliente dtoNotificacion)
    {
        _logger.LogInformation("[Email] Para: {Destinatario} | {Mensaje}", dtoNotificacion.Destinatario, dtoNotificacion.Mensaje);
        return true;
    }
}
