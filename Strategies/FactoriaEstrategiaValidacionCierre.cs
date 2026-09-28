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

    // Recibe la instancia (no un dato ya resuelto) para desacoplar al Experto de las clases de
    // configuración: la búsqueda de la vigente y la lectura de su tipo de validación son
    // responsabilidad de la fábrica. Navega el objeto ya cargado (CasoInstancia → Caso → TipoCaso →
    // TiposCasoTipoInstancia) en vez de volver a consultar por IDs sueltos.
    public IEstrategiaValidacionCierre ObtenerEstrategia(CasoInstancia instancia)
    {
        var fechaActual = DateTime.UtcNow;
        var configuracion = instancia.Caso!.TipoCaso!.TiposCasoTipoInstancia
            .FirstOrDefault(t => t.Orden == instancia.OrdenCasoInstancia
                && t.FechaAlta <= fechaActual
                && (t.FechaBaja == null || t.FechaBaja > fechaActual));

        // Tanto la falta de configuración vigente para la instancia como un nombre que no matchee
        // ninguno de los tres criterios conocidos son un problema de datos, no un caso de negocio.
        switch (configuracion?.TipoValidacionCierre?.Nombre)
        {
            case "Simple":
                return new EstrategiaValidacionSimple(_loggerFactory.CreateLogger<EstrategiaValidacionSimple>());
            case "PorObservaciones":
                return new EstrategiaValidacionPorObservaciones(_loggerFactory.CreateLogger<EstrategiaValidacionPorObservaciones>());
            case "PorTareaRegistrada":
                return new EstrategiaValidacionPorTareaRegistrada(_loggerFactory.CreateLogger<EstrategiaValidacionPorTareaRegistrada>());
            default:
                throw new InvalidOperationException(configuracion is null
                    ? "No hay ninguna estrategia de validación de cierre configurada para esta instancia."
                    : $"No existe una estrategia de validación de cierre para el tipo '{configuracion.TipoValidacionCierre?.Nombre}'.");
        }
    }
}