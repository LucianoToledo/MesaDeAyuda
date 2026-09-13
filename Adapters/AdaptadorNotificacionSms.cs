using MesaDeAyuda.DTOs;

namespace MesaDeAyuda.Adapters;

// Simulado: no hay proveedor de SMS real configurado todavía.
public class AdaptadorNotificacionSms : IAdaptadorNotificacionCliente
{
    private readonly ILogger<AdaptadorNotificacionSms> _logger;

    public AdaptadorNotificacionSms(ILogger<AdaptadorNotificacionSms> logger)
    {
        _logger = logger;
    }

    public bool Notificar(DTONotificacionCliente dtoNotificacion)
    {
        _logger.LogInformation("[SMS] Para: {Destinatario} | {Mensaje}", dtoNotificacion.Destinatario, dtoNotificacion.Mensaje);
        return true;
    }
}
