using MesaDeAyuda.Domain.Entities;

namespace MesaDeAyuda.Strategies;

public class EstrategiaValidacionSimple : IEstrategiaValidacionCierre
{
    private readonly ILogger<EstrategiaValidacionSimple> _logger;

    public EstrategiaValidacionSimple(ILogger<EstrategiaValidacionSimple> logger)
    {
        _logger = logger;
    }

    public void ValidarCierre(CasoInstancia instancia)
    {
        _logger.LogInformation("Ejecutando estrategia {Estrategia}", nameof(EstrategiaValidacionSimple));
        // No exige ninguna documentación previa.
    }
}