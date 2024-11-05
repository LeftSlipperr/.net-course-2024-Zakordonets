using BankSystem.App.DTO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BankSystem.App.Services
{
    public class CurrencyService
    {
        private readonly ApiSettings _apiSettings;
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }
        
        public async Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            var url = $"{_apiSettings.BaseUrl}currency.php?api_key={_apiSettings.ApiKey}&from={fromCurrency}&to={toCurrency}&amount={amount}";

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