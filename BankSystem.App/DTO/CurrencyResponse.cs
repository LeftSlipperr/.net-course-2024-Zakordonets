using Newtonsoft.Json;

namespace BankSystem.App.DTO;

public class CurrencyResponse
{
    [JsonProperty("amount")]
    public decimal Amount { get; set; }
        
    [JsonProperty("error")]
    public int Error { get; set; }
        
    [JsonProperty("error_message")]
    public string ErrorMessage { get; set; }
}