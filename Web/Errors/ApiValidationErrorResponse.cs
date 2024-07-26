using Microsoft.AspNetCore.Mvc;

namespace Web.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse(IEnumerable<string> errors) : base(400)
        {
            Errors = errors;
        }
  
        public IEnumerable<string> Errors { get; set; }

        public static IActionResult GenerateErrorResponse(ActionContext context)
        {
            var data = context.ModelState.Where(x => x.Value?.Errors != null && x.Value.Errors.Any());
            var errorMessages = data.SelectMany(x =>
                x.Value?.Errors.Select(error => error.ErrorMessage) ?? Enumerable.Empty<string>());
            
            var response = new ApiValidationErrorResponse(errorMessages);
            
            return new BadRequestObjectResult(response);
        }
    }
}
