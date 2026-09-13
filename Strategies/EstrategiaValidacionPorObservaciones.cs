using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Domain.Exceptions;

namespace MesaDeAyuda.Strategies;

public class EstrategiaValidacionPorObservaciones : IEstrategiaValidacionCierre
{
    private readonly ILogger<EstrategiaValidacionPorObservaciones> _logger;

    public EstrategiaValidacionPorObservaciones(ILogger<EstrategiaValidacionPorObservaciones> logger)
    {
        _logger = logger;
    }

    public void ValidarCierre(CasoInstancia instancia)
    {
        _logger.LogInformation("Ejecutando estrategia {Estrategia}", nameof(EstrategiaValidacionPorObservaciones));

        if (string.IsNullOrWhiteSpace(instancia.Observaciones))
            throw new BusinessException("Debe completar las observaciones de la instancia antes de poder asentar el resultado.");
    }
}
