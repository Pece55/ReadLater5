using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadLater5.CustomExceptions;

namespace ReadLater5.Controllers
{
    [ApiController]
    [ExceptionHandler]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public class BaseController : ControllerBase
    {
    }
}
