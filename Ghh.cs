private async Task<double> GetExchangeRateAsync(string from, string to)
{
    string url = $"https://api.exchangerate-api.com/v4/latest/{from}";

    try
    {
        var response = await _httpClient.GetStringAsync(url);
        var data = JsonSerializer.Deserialize<ExchangeRateResponse>(response);

        if (data.rates.ContainsKey(to))
        {
            return data.rates[to];
        }
        throw new Exception("Валюта не найдена в API");
    }
    catch (Exception ex)
    {
        throw new Exception($"Ошибка загрузки курса: {ex.Message}");
    }
}
