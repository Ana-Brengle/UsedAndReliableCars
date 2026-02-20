using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Search(string? make, string? model, int? year, decimal? priceMax, string? zip, int? radius)
        {
            var apiKey = _configuration["MarketCheck:ApiKey"];
            var url = $"search/car/AutoGems.ai/active?api_key={apiKey}";

            if (!string.IsNullOrEmpty(make)) {
                url += $"&make={make}";
            }
            if (!string.IsNullOrEmpty(model)) {
                url += $"&model={model}";
            }
            if (year.HasValue)
            {
                url += $"&year={year}";
            }
            if (priceMax.HasValue)
            {
                url += $"&price_max={priceMax}";
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
