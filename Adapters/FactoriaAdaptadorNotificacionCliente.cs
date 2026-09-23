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

    // Sin preferencia de canal por cliente todavia (no existe entidad Cliente local):
    // devuelve siempre el canal por defecto de la empresa.
    public IAdaptadorNotificacionCliente ObtenerAdaptador()
    {
        return new AdaptadorNotificacionEmail(_loggerFactory.CreateLogger<AdaptadorNotificacionEmail>());
    }
}