namespace MesaDeAyuda.DTOs;

public record DTOCaso(
    int NroCaso,
    int NroCliente,
    DateTime FechaHoraIngresoCaso,
    string NombreEstadoCaso,
    string ObservacionCaso
);