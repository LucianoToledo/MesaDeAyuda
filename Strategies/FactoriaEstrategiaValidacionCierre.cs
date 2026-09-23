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

    public IEstrategiaValidacionCierre ObtenerEstrategia(TipoValidacionCierre tipoValidacion)
    {
        return tipoValidacion switch
        {
            TipoValidacionCierre.PorObservaciones => new EstrategiaValidacionPorObservaciones(_loggerFactory.CreateLogger<EstrategiaValidacionPorObservaciones>()),
            TipoValidacionCierre.PorTareaRegistrada => new EstrategiaValidacionPorTareaRegistrada(_loggerFactory.CreateLogger<EstrategiaValidacionPorTareaRegistrada>()),
            _ => new EstrategiaValidacionSimple(_loggerFactory.CreateLogger<EstrategiaValidacionSimple>())
        };
    }
}