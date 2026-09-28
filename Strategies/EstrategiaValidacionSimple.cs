using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.DTOs;

namespace MesaDeAyuda.Strategies;

public class EstrategiaValidacionSimple : IEstrategiaValidacionCierre
{
    private readonly ILogger<EstrategiaValidacionSimple> _logger;

    public EstrategiaValidacionSimple(ILogger<EstrategiaValidacionSimple> logger)
    {
        _logger = logger;
    }

    public DTOResultadoValidacion ValidarCierre(CasoInstancia instancia)
    {
        _logger.LogInformation("Ejecutando estrategia {Estrategia}", nameof(EstrategiaValidacionSimple));

        // No exige ninguna documentación previa.
        return new DTOResultadoValidacion(true, string.Empty);
    }
}