using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Persistencia;
using Microsoft.Extensions.DependencyInjection;

namespace MesaDeAyuda.Adapters;

public class FactoriaAdaptadorNotificacionCliente
{
    private static readonly FactoriaAdaptadorNotificacionCliente _instancia = new();
    private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
    // Permite obtener la Indirección de Persistencia en cada llamado a ObtenerAdaptador.
    // Se carga al iniciar la aplicación para que el Singleton pueda usarla sin recibirla como parámetro.
    private IServiceScopeFactory? _scopeFactory;

    public static FactoriaAdaptadorNotificacionCliente Instancia => _instancia;

    private FactoriaAdaptadorNotificacionCliente()
    { }

    public static void Inicializar(IServiceScopeFactory scopeFactory)
    {
        _instancia._scopeFactory = scopeFactory;
    }

    public async Task<IAdaptadorNotificacionCliente> ObtenerAdaptador()
    {
        // La fábrica consulta la Indirección de Persistencia para saber qué canal
        // de notificación tiene habilitado la empresa en este momento.
        using var scope = _scopeFactory!.CreateScope();
        var persistencia = scope.ServiceProvider.GetRequiredService<IndireccionPersistencia>();

        var resultado = await persistencia.Buscar("ConfiguracionNotificacion", "FechaBaja == null");

        var configuracion = resultado.Cast<ConfiguracionNotificacion>().FirstOrDefault();

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
