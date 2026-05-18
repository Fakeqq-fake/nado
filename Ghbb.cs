using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;

namespace CurrencyConverter
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;
        private Dictionary<string, string> _currencies;

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _currencies = new Dictionary<string, string>
            {
                { "USD", "Доллар США" },
                { "EUR", "Евро" },
                { "GBP", "Британский фунт" },
                { "JPY", "Японская иена" },
                { "RUB", "Российский рубль" }
            };
            LoadCurrencies();
        }

        private void LoadCurrencies()
        {
            FromCurrencyComboBox.ItemsSource = _currencies.Keys;
            ToCurrencyComboBox.ItemsSource = _currencies.Keys;
            FromCurrencyComboBox.SelectedIndex = 0;
            ToCurrencyComboBox.SelectedIndex = 1;
        }

        private async void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!double.TryParse(AmountTextBox.Text, out double amount) || amount <= 0)
                {
                    MessageBox.Show("Введите корректную положительную сумму!");
                    return;
                }

                string fromCurrency = FromCurrencyComboBox.SelectedItem?.ToString();
                string toCurrency = ToCurrencyComboBox.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency))
                {
                    MessageBox.Show("Выберите валюты для конвертации!");
                    return;
                }

                double rate = await GetExchangeRateAsync(fromCurrency, toCurrency);
                double result = amount * rate;
                ResultLabel.Content = $"Результат: {result:F2} {toCurrency}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка конвертации: {ex.Message}");
            }
        }

        private async Task<double> GetExchangeRateAsync(string from, string to)
        {
            // Исправленный URL: протокол https, а не http://
            string url = $"https://api.exchangerate-api.com/v4/latest/{from}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var data = JsonSerializer.Deserialize<ExchangeRateResponse>(response);

                if (data?.rates != null && data.rates.ContainsKey(to))
                {
                    return data.rates[to];
                }
                throw new Exception($"Курс для валюты {to} не найден");
            }
            catch (HttpRequestException)
            {
                throw new Exception("Ошибка подключения к API. Проверьте интернет‑соединение.");
            }
            catch (JsonException)
            {
                throw new Exception("Ошибка обработки данных от API.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Общая ошибка: {ex.Message}");
            }
        }
    }

    // Класс для десериализации ответа API
    public class ExchangeRateResponse
    {
        public Dictionary<string, double> rates { get; set; }
    }
}
