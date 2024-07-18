using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Api
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("errors/{code:int}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}
