using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Adapters;

// Simulado: no hay proveedor de email real configurado todavía.
public class AdaptadorNotificacionEmail : IAdaptadorNotificacionCliente
{
    private readonly ILogger<AdaptadorNotificacionEmail> _logger;

    public AdaptadorNotificacionEmail(ILogger<AdaptadorNotificacionEmail> logger)
    {
        _logger = logger;
    }

    public bool NotificarCliente(string mensaje, Caso caso)
    {
        _logger.LogInformation("[Email] Para: {Destinatario} | {Mensaje}", caso.MailCliente, mensaje);
        return true;
    }
}