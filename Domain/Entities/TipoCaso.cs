namespace MesaDeAyuda.Domain.Entities;

public class TipoCaso
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int NumeroMaximaIteracion { get; set; }
    public DateTime? FechaHoraBaja { get; set; }

    // Configuración de las instancias que componen este tipo de caso (con vigencia)
    public ICollection<TipoCasoTipoInstancia> TiposCasoTipoInstancia { get; set; } = new List<TipoCasoTipoInstancia>();

    // Coeficiente de reducción de tiempos por iteración (presión ERE, Regla de Negocio N°5)
    public ICollection<TipoCasoIteracion> TiposCasoIteracion { get; set; } = new List<TipoCasoIteracion>();
}