using MesaDeAyuda.DTOs;
using MesaDeAyuda.Expertos;
using Microsoft.AspNetCore.Mvc;

namespace MesaDeAyuda.Controllers;

// Utilidad de desarrollo/testing, NO es el CU que se está desarrollando en este TP
// (ver documentación del proyecto, "Endpoints de desarrollo/utilidad").
[ApiController]
[Route("api/v1/casos")]
public class TomarCasoController : ControllerBase
{
    private readonly IExpertoTomarCaso _experto;

    public TomarCasoController(IExpertoTomarCaso experto)
    {
        _experto = experto;
    }

    /// <summary>
    /// Asigna al especialista la instancia "A Asignar" del caso que corresponda a su sector.
    /// Utilidad de testing, no forma parte del CU "Asentar Resultado".
    /// </summary>
    [HttpPost("tomar")]
    public async Task<IActionResult> Tomar(TomarCasoRequestDto request)
    {
        await _experto.TomarInstancia(request);
        return NoContent();
    }
}