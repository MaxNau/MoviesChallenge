using ApiApplication.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ApiApplication.Filters.Exception
{
    public class ValidationExceptionFilterAttribute : ExceptionFilterAttribute
    {
       public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException || context.Exception is SeatReservationUnavaliableException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                context.Result = new JsonResult(context.Exception.Message);

                return;
            }
        }
    }
}
