namespace MesaDeAyuda.DTOs;

// NumeroCliente es un dato neutro (no un "destinatario" ya resuelto): cada adaptador concreto es
// quien decide cómo derivar de ahí el contacto real para su propio canal (ver documentación del
// proyecto, la resolución es simulada, no hay integración real con el sistema externo
// de ventas para obtener el email/teléfono del cliente).
public record DTONotificacionCliente(
    int NumeroCliente,
    string Mensaje
);