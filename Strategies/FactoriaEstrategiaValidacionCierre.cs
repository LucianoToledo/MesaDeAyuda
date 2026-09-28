using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Strategies;

public class FactoriaEstrategiaValidacionCierre
{
    private static readonly FactoriaEstrategiaValidacionCierre _instancia = new();

    // La Factoria es un singleton clásico, fuera del contenedor de DI (estilo cátedra) —
    // por eso arma su propio ILoggerFactory en vez de recibir el de la app.
    private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

    public static FactoriaEstrategiaValidacionCierre Instancia => _instancia;

    private FactoriaEstrategiaValidacionCierre()
    { }

    public IEstrategiaValidacionCierre ObtenerEstrategia(TipoValidacionCierre? tipoValidacion)
    {
        // Tanto la falta de configuración vigente para la instancia como un nombre que no matchee
        // ninguno de los tres criterios conocidos son un problema de datos, no un caso de negocio.
        switch (tipoValidacion?.Nombre)
        {
            case "Simple":
                return new EstrategiaValidacionSimple(_loggerFactory.CreateLogger<EstrategiaValidacionSimple>());
            case "PorObservaciones":
                return new EstrategiaValidacionPorObservaciones(_loggerFactory.CreateLogger<EstrategiaValidacionPorObservaciones>());
            case "PorTareaRegistrada":
                return new EstrategiaValidacionPorTareaRegistrada(_loggerFactory.CreateLogger<EstrategiaValidacionPorTareaRegistrada>());
            default:
                throw new InvalidOperationException(tipoValidacion is null
                    ? "No hay ninguna estrategia de validación de cierre configurada para esta instancia."
                    : $"No existe una estrategia de validación de cierre para el tipo '{tipoValidacion.Nombre}'.");
        }
    }
}