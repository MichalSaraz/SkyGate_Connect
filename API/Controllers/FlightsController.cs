using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        [HttpGet]
        public string GetFlights()
        {
            return "this will be a list of flights";
        }

        [HttpGet("{id}")]
        public string GetFlights(int id)
        {
            return "this will be a certain flight";
        }
    }
}
