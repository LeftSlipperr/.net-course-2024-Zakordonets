using BankSystem.App.DTO;
using Newtonsoft.Json;

namespace BankSystem.App.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "edjCNTCqn4PWxEjML5zDjmpc3xXJfz";
        private const string BaseUrl = "https://www.amdoren.com/api/";

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            var url = $"{BaseUrl}currency.php?api_key={ApiKey}&from={fromCurrency}&to={toCurrency}&amount={amount}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to retrieve currency conversion data.");

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var conversionResult = JsonConvert.DeserializeObject<CurrencyResponse>(jsonResponse);

            if (conversionResult.Error != 0)
                throw new Exception($"API error: {conversionResult.ErrorMessage}");

            return conversionResult.Amount;
        }
    }
}