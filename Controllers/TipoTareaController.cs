using MesaDeAyuda.DTOs;
using MesaDeAyuda.Expertos;
using Microsoft.AspNetCore.Mvc;

namespace MesaDeAyuda.Controllers;

// Utilidad de desarrollo/testing, NO es el CU que se está desarrollando en este TP (ver la
// documentación del proyecto, "Endpoints de desarrollo/utilidad"). Expone el nomenclador de
// TipoTarea para poder elegir un TipoTareaId válido al registrar una tarea.
[ApiController]
[Route("api/v1/tipos-tarea")]
public class TipoTareaController : ControllerBase
{
    private readonly IExpertoTarea _experto;

    public TipoTareaController(IExpertoTarea experto)
    {
        _experto = experto;
    }

    /// <summary>
    /// Lista el nomenclador de tipos de tarea (para elegir un TipoTareaId al registrar una tarea).
    /// Utilidad de testing, no forma parte del CU "Asentar Resultado".
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DTOTipoTarea>>> Buscar()
    {
        var tiposTarea = await _experto.BuscarTiposTarea();
        return Ok(tiposTarea);
    }
}
