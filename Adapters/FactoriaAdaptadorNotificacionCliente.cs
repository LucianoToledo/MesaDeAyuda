using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Persistencia;

namespace MesaDeAyuda.Adapters;

public class FactoriaAdaptadorNotificacionCliente
{
    private static readonly FactoriaAdaptadorNotificacionCliente _instancia = new();

    // La Factoria es un singleton clásico, fuera del contenedor de DI (estilo cátedra) —
    // por eso arma su propio ILoggerFactory en vez de recibir el de la app.
    private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

    public static FactoriaAdaptadorNotificacionCliente Instancia => _instancia;

    private FactoriaAdaptadorNotificacionCliente()
    { }

    // El canal habilitado es una configuración de la empresa (una sola fila en la base), no una
    // preferencia por cliente: no existe ninguna entidad Cliente local con datos de contacto (ver
    // documentación del proyecto, Sección 9). Por eso necesita la persistencia para resolverlo,
    // a diferencia de FactoriaEstrategiaValidacionCierre, que ya recibe todo cargado en memoria.
    public async Task<IAdaptadorNotificacionCliente> ObtenerAdaptador(IndireccionPersistencia persistencia)
    {
        var resultado = await persistencia.Buscar("ConfiguracionNotificacion", string.Empty);

        var fechaActual = DateTime.UtcNow;
        var configuracion = resultado.Cast<ConfiguracionNotificacion>()
            .FirstOrDefault(c => c.FechaAlta <= fechaActual && (c.FechaBaja == null || c.FechaBaja > fechaActual));

        // Tanto la falta de configuración como un canal que no matchee ninguno de los dos
        // conocidos son un problema de datos, no un caso de negocio.
        switch (configuracion?.CanalHabilitado?.Nombre)
        {
            case "Email":
                return new AdaptadorNotificacionEmail(_loggerFactory.CreateLogger<AdaptadorNotificacionEmail>());
            case "Sms":
                return new AdaptadorNotificacionSms(_loggerFactory.CreateLogger<AdaptadorNotificacionSms>());
            default:
                throw new InvalidOperationException(configuracion is null
                    ? "No hay ningún canal de notificación habilitado configurado."
                    : $"No existe un adaptador de notificación para el canal '{configuracion.CanalHabilitado?.Nombre}'.");
        }
    }
}