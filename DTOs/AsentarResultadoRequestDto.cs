namespace MesaDeAyuda.DTOs;

public record AsentarResultadoRequestDto(
    int NroLegajoEspecialista,
    int NumeroCaso,
    bool Respuesta, // true = Resuelto, false = NoResuelto
    string? Observaciones
);