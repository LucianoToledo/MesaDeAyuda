using MesaDeAyuda.DTOs;
using MesaDeAyuda.Expertos;
using Microsoft.AspNetCore.Mvc;

namespace MesaDeAyuda.Controllers;

// Utilidad de desarrollo/testing, NO es el CU que se está desarrollando en este TP
// (ver documentación del proyecto, "Endpoints de desarrollo/utilidad"). "Registrar Tarea" es un
// CU propio del diagrama de casos de uso, sin especificar formalmente acá.
[ApiController]
[Route("api/v1/casos/{numeroCaso:int}/tareas")]
public class TareaController : ControllerBase
{
    private readonly IExpertoTarea _experto;

    public TareaController(IExpertoTarea experto)
    {
        _experto = experto;
    }

    /// <summary>
    /// Registra una tarea contra la instancia asignada al especialista en este caso.
    /// Utilidad de testing: "Registrar Tarea" es un CU propio, no "Asentar Resultado".
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Registrar(int numeroCaso, RegistrarTareaRequestDto request)
    {
        await _experto.RegistrarTarea(numeroCaso, request);
        return NoContent();
    }

    /// <summary>
    /// Lista las tareas registradas en las instancias de este caso.
    /// Utilidad de testing, no forma parte del CU "Asentar Resultado".
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DTOTarea>>> Buscar(int numeroCaso)
    {
        var tareas = await _experto.BuscarTareas(numeroCaso);
        return Ok(tareas);
    }
}
