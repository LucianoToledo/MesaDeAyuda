using MesaDeAyuda.Domain.Entities;
using MesaDeAyuda.DTOs;

namespace MesaDeAyuda.Strategies;

public interface IEstrategiaValidacionCierre
{
    // Devuelve el resultado como valor (EsValido/Mensaje) en vez de lanzar una excepción: el
    // rechazo llega hasta la respuesta del controller sin pasar por el manejo de excepciones.
    DTOResultadoValidacion ValidarCierre(CasoInstancia instancia);
}