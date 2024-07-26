using Microsoft.AspNetCore.Mvc;

namespace Web.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
 
        public int StatusCode { get; set; }
        public string? Message { get; set; }


        /// <summary>
        /// Gets the default error message for a given status code.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns>The default error message for the specified status code,
        /// or null if the status code is not handled by the method.</returns>
        private static string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change",
                _ => null
            };
        }
    }
}
