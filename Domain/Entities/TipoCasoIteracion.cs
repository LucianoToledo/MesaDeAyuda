namespace MesaDeAyuda.Domain.Entities;

public class TipoCasoIteracion
{
    public int Id { get; set; }
    public int NumeroDeIteracion { get; set; }
    public int CoeficienteReduccionTipo { get; set; }

    public int TipoCasoId { get; set; }
    public TipoCaso? TipoCaso { get; set; }
}