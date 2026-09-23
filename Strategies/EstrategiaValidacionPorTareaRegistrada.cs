using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.Domain.Exceptions;

namespace MesaDeAyuda.Strategies;

public class EstrategiaValidacionPorTareaRegistrada : IEstrategiaValidacionCierre
{
    private readonly ILogger<EstrategiaValidacionPorTareaRegistrada> _logger;

    public EstrategiaValidacionPorTareaRegistrada(ILogger<EstrategiaValidacionPorTareaRegistrada> logger)
    {
        _logger = logger;
    }

    public void ValidarCierre(CasoInstancia instancia)
    {
        _logger.LogInformation("Ejecutando estrategia {Estrategia}", nameof(EstrategiaValidacionPorTareaRegistrada));

        if (!instancia.Tareas.Any())
            throw new BusinessException("Debe registrar al menos una tarea antes de poder asentar el resultado de la instancia.");
    }
}