using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.DTOs;

namespace MesaDeAyuda.Strategies;

public class EstrategiaValidacionPorObservaciones : IEstrategiaValidacionCierre
{
    private readonly ILogger<EstrategiaValidacionPorObservaciones> _logger;

    public EstrategiaValidacionPorObservaciones(ILogger<EstrategiaValidacionPorObservaciones> logger)
    {
        _logger = logger;
    }

    public DTOResultadoValidacion ValidarCierre(CasoInstancia instancia)
    {
        _logger.LogInformation("Ejecutando estrategia {Estrategia}", nameof(EstrategiaValidacionPorObservaciones));

        if (string.IsNullOrWhiteSpace(instancia.Observaciones))
            return new DTOResultadoValidacion(false, "Debe completar las observaciones de la instancia antes de poder asentar el resultado.");

        return new DTOResultadoValidacion(true, "Resultado asentado correctamente.");
    }
}