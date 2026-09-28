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
        // Simulado: sin sistema externo de ventas disponible, el adaptador arma un teléfono de
        // ejemplo a partir del número de cliente en vez de recibirlo ya resuelto.
        var telefono = $"+54 9 11 {dtoNotificacion.NumeroCliente:0000}";

        _logger.LogInformation("[SMS] Para: {Destinatario} | {Mensaje}", telefono, dtoNotificacion.Mensaje);
        return true;
    }
}