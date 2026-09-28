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
        // Simulado: sin sistema externo de ventas disponible, el adaptador arma un email de ejemplo
        // a partir del número de cliente en vez de recibirlo ya resuelto.
        var email = $"cliente{dtoNotificacion.NumeroCliente}@example.com";

        _logger.LogInformation("[Email] Para: {Destinatario} | {Mensaje}", email, dtoNotificacion.Mensaje);
        return true;
    }
}