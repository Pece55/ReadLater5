using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ReadLater5.CustomExceptions
{
    public class ExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new JsonResult(new ErrorResponse
            {
                Status = GetStatus(context.Exception),
                Error = GetError(context.Exception),
                Message = context.Exception.Message,
                Path = context.HttpContext.Request.Path
            })
            {
                StatusCode = GetStatus(context.Exception)
            };

            context.ExceptionHandled = true;

            base.OnException(context);
        }

        #region Private
        private int GetStatus(Exception exception)
        {
            if (exception is CustomException ex)
            {
                return (int)ex.StatusCode;
            }

            return (int)HttpStatusCode.BadRequest;
        }

        private string GetError(Exception exception)
        {
            if (exception is CustomException ex)
            {
                return ex.StatusCode.ToString();
            }

            return exception.GetType().Name;
        }

        #endregion
    }
}


