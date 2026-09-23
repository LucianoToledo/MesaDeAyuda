namespace MesaDeAyuda.DTOs;

public record RegistrarTareaRequestDto(
    int NroLegajoEspecialista,
    int TipoTareaId,
    string? Observaciones
);