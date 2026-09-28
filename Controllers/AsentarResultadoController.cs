using MesaDeAyuda.DTOs;
using MesaDeAyuda.Expertos;
using Microsoft.AspNetCore.Mvc;

namespace MesaDeAyuda.Controllers;

[ApiController]
[Route("api/v1")]
public class AsentarResultadoController : ControllerBase
{
    private readonly IExpertoAsentarResultado _experto;

    public AsentarResultadoController(IExpertoAsentarResultado experto)
    {
        _experto = experto;
    }

    /// <summary>
    /// Valida que el especialista exista y no esté dado de baja (pasos 1-4.3 del CU, C.A. N°1/N°2).
    /// </summary>
    [HttpGet("especialistas/{nroLegajoEspecialista:int}")]
    public async Task<ActionResult<DTOEspecialista>> BuscarEspecialista(int nroLegajoEspecialista)
    {
        var especialista = await _experto.BuscarEspecialista(nroLegajoEspecialista);
        return Ok(especialista);
    }

    /// <summary>
    /// Busca el caso "Tomado" y comprueba que su instancia "Asignada" pertenezca al especialista
    /// (pasos 5-6.12 del CU, C.A. N°1/N°3/N°4).
    /// </summary>
    [HttpGet("casos/{numeroCaso:int}")]
    public async Task<ActionResult<DTOCaso>> BuscarCaso(int numeroCaso, [FromQuery] int nroLegajoEspecialista)
    {
        var caso = await _experto.BuscarCaso(nroLegajoEspecialista, numeroCaso);
        return Ok(caso);
    }

    /// <summary>
    /// Asienta el resultado (Resuelto/NoResuelto) de la instancia asignada al especialista y
    /// aplica las transiciones de estado del Camino Básico y los Caminos Alternos N°5/N°6.
    /// Si la validación de cierre rechaza el intento, devuelve 400 sin pasar por el filtro global.
    /// </summary>
    [HttpPost("casos/asentar-resultado")]
    public async Task<IActionResult> IngresarRespuesta(AsentarResultadoRequestDto request)
    {
        var resultado = await _experto.IngresarRespuesta(request);

        if (!resultado.EsValido)
            return BadRequest(new { mensaje = resultado.Mensaje });

        return NoContent();
    }
}