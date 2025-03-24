using FinShark.DTOs.Stock;
using FinShark.Interfaces;
using FinShark.Mappers;
using FinShark.Models;
using Newtonsoft.Json;

namespace FinShark.Services
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public FMPService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }
        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                // GET Request
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");

                if (result.IsSuccessStatusCode)
                {
                    // Read the API response as a string and deserialize it into an array of FMPStocks objects
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);

                    // Extract the first item from the tasks variable. The API expects an array response
                    // This is also why we have [] as part of FMPStock[]
                    var stock = tasks[0];

                    if (stock != null)
                    {
                        return stock.ToStockFromFMP();
                    }

                    // Didn't receive a response from API, despite success
                    return null;
                }

                // Unsuccessful status code
                return null;
            }
            // Catch any other errors that may occur
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
