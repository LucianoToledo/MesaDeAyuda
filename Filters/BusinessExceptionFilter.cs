using MesaDeAyuda.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MesaDeAyuda.Filters;

// Centraliza el manejo de BusinessException para que los controllers no necesiten try/catch.
public class BusinessExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not BusinessException ex)
            return;

        context.Result = new BadRequestObjectResult(new { mensaje = ex.Message });
        context.ExceptionHandled = true;
    }
}