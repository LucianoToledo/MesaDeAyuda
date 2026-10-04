using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Adapters;

// Simulado: no hay proveedor de SMS real configurado todavía.
public class AdaptadorNotificacionSms : IAdaptadorNotificacionCliente
{
    private readonly ILogger<AdaptadorNotificacionSms> _logger;

    public AdaptadorNotificacionSms(ILogger<AdaptadorNotificacionSms> logger)
    {
        _logger = logger;
    }

    public bool NotificarCliente(string mensaje, Caso caso)
    {
        _logger.LogWarning("[SMS] Para: {Destinatario} | {Mensaje}", caso.NumeroTelefonoCliente, mensaje);
        return true;
    }
}