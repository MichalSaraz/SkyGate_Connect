using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Api
{
    public class ErrorController : ControllerBase
    {
        [Route("errors/{code}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}
