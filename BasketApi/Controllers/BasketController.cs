using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
          "Basket 1", "Basket 2", "Basket 3", "Basket 4", "Basket 5", "Basket 6", "Basket 7", "Basket 8", "Basket 9", "Basket 10"
    };

        private readonly ILogger<BasketController> _logger;

        public BasketController(ILogger<BasketController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetBasket")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}