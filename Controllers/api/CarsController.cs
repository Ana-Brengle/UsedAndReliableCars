using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace UsedAndReliableCars.Controllers.api
{
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public CarsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.marketcheck.com/v2/");
            _configuration = configuration;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? make, string? model, string? year, decimal? priceMax, string? zip, int? radius, int start = 0, int rows = 10)
        {
            var apiKey = _configuration["MarketCheck:ApiKey"];
            var url = $"search/car/active?api_key={apiKey}&start={start}&rows={rows}";

            if (!string.IsNullOrEmpty(make)) {
                url += $"&make={make}";
            }
            if (!string.IsNullOrEmpty(model)) {
                url += $"&model={model}";
            }
            if (!string.IsNullOrEmpty(year))
            {
                url += $"&year_range={year}";
            }
            if (priceMax.HasValue)
            {
                url += $"&price_range=0-{priceMax}";
            }
            if (radius.HasValue)
            {
                url += $"&radius={radius}";
            }
            if (!string.IsNullOrEmpty(zip))
            {
                url += $"&zip={zip}";
            }
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(503, new { error = "Unable to reach car" });
            }
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        

;        }
    }
}
