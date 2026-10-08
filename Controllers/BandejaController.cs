using MesaDeAyuda.DTOs;
using MesaDeAyuda.Expertos;
using Microsoft.AspNetCore.Mvc;

namespace MesaDeAyuda.Controllers;

// Utilidad de desarrollo/testing, NO es el CU que se está desarrollando en este TP
// (ver documentación del proyecto, "Endpoints de desarrollo/utilidad"). Permite listar los casos
// con instancia "Asignada" al especialista para facilitar la demostración del flujo completo desde la UI.
[ApiController]
[Route("api/v1/bandeja")]
public class BandejaController : ControllerBase
{
    private readonly IExpertoBandeja _experto;

    public BandejaController(IExpertoBandeja experto)
    {
        _experto = experto;
    }

    /// <summary>
    /// Devuelve los casos con instancia "Asignada" al especialista indicado.
    /// Utilidad de testing, no forma parte del CU "Asentar Resultado" ni del DC.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DTOCasoBandeja>>> ObtenerBandeja(
        [FromQuery] int nroLegajoEspecialista)
    {
        var casos = await _experto.ObtenerBandeja(nroLegajoEspecialista);
        return Ok(casos);
    }
}
