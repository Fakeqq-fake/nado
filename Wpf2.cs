<Window x:Class="CurrencyConverter.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Конвертер валют" Height="350" Width="400"
        Background="#2c3e50">
    <Window.Resources>
        <Style TargetType="TextBox">
            <Setter Property="Background" Value="#34495e"/>
            <Setter Property="Foreground" Value="White"/>
            <Setter Property="BorderThickness" Value="0"/>
            <Setter Property="Padding" Value="10"/>
            <Setter Property="Margin" Value="10"/>
        </Style>
        <Style TargetType="ComboBox">
            <Setter Property="Background" Value="#34495e"/>
            <Setter Property="Foreground" Value="White"/>
            <Setter Property="BorderThickness" Value="0"/>
            <Setter Property="Padding" Value="10"/>
            <Setter Property="Margin" Value="10"/>
        </Style>
        <Style TargetType="Button">
            <Setter Property="Background" Value="#e74c3c"/>
            <Setter Property="Foreground" Value="White"/>
            <Setter Property="BorderThickness" Value="0"/>
            <Setter Property="Padding" Value="15,10"/>
            <Setter Property="Margin" Value="10"/>
            <Setter Property="FontSize" Value="14"/>
            <Style.Triggers>
                <Trigger Property="IsMouseOver" Value="True">
                    <Setter Property="Background" Value="#c0392b"/>
                </Trigger>
            </Style.Triggers>
        </Style>
        <Style TargetType="Label">
            <Setter Property="Foreground" Value="#ecf0f1"/>
            <Setter Property="FontWeight" Value="Bold"/>
            <Setter Property="Margin" Value="10,15,10,5"/>
        </Style>
    </Window.Resources>
    <Grid>
        <StackPanel VerticalAlignment="Center">
            <Label Content="Сумма:"/>
            <TextBox x:Name="AmountTextBox" Text="1" />
            <Label Content="Из валюты:"/>
            <ComboBox x:Name="FromCurrencyComboBox" />
            <Label Content="В валюту:"/>
            <ComboBox x:Name="ToCurrencyComboBox" />
            <Button Content="Конвертировать" Click="ConvertButton_Click"/>
            <Label x:Name="ResultLabel" Content="Результат: "
                   HorizontalAlignment="Center" FontSize="16" Margin="10"/>
        </StackPanel>
    </Grid>
</Window>


using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

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
                if (!double.TryParse(AmountTextBox.Text, out double amount))
                {
                    MessageBox.Show("Введите корректную сумму!");
                    return;
                }

                string fromCurrency = FromCurrencyComboBox.SelectedItem?.ToString();
                string toCurrency = ToCurrencyComboBox.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency))
                {
                    MessageBox.Show("Выберите валюты!");
                    return;
                }

                double rate = await GetExchangeRateAsync(fromCurrency, toCurrency);
                double result = amount * rate;
                ResultLabel.Content = $"Результат: {result:F2} {toCurrency}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async Task<double> GetExchangeRateAsync(string from, string to)
        {
            // Используем бесплатный API для курсов валют
            string url = $"https://api.exchangerate-api.com/v4/latest/{from}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<ExchangeRateResponse>(response);

                if (data.Rates.ContainsKey(to))
                {
                    return data.Rates[to];
                }
                throw new Exception("Валюта не найдена в API");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки курса: {ex.Message}");
            }
        }
    }

    // Класс для десериализации ответа API
    public class ExchangeRateResponse
    {
        [JsonProperty("rates")]
        public Dictionary<string, double> Rates { get; set; }
    }
}

