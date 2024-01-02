using System.Text.Json;

namespace MauiApp2

{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            string apiKey = "API_KEY";
            string city = CityEntry.Text;
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        WeatherData weatherData = JsonSerializer.Deserialize<WeatherData>(data);

                        if (weatherData != null)
                        {
                            float temperature = weatherData.main.temp;//   .main.temp;
                            TemperatureLabel.Text = $"Current temperature in {city}: {temperature}°C";
                        }
                        else
                        {
                            TemperatureLabel.Text = "Failed to fetch data.";
                        }
                    }
                    else
                    {
                        TemperatureLabel.Text = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    }
                }
                catch (HttpRequestException err)
                {
                    TemperatureLabel.Text += $"Request exception: {err.Message}";
                }
            }
            /*
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "/data/user/0/com.companyname.mauiapp2/ConsoleApplication1.exe"; // Replace with your compiled C++ executable path
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                // Display the temperature in the label
                TemperatureLabel.Text = "Temperature: " + output + " Kelvin";
            }
            catch (Exception ex)
            {
                // Handle exceptions
                TemperatureLabel.Text = "Error: " + ex.Message;
            }
            */
        }
    }

}

public class WeatherData
{
    public Coord Coord { get; set; }
    public WeatherInfo[] Weather { get; set; }
    public string Base { get; set; }
    public MainInfo main { get; set; }
    public int Visibility { get; set; }
    public WindInfo Wind { get; set; }
    public CloudsInfo Clouds { get; set; }
    public long Dt { get; set; }
    public SysInfo Sys { get; set; }
    public int Timezone { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Cod { get; set; }
}

public class Coord
{
    public float Lon { get; set; }
    public float Lat { get; set; }
}

public class WeatherInfo
{
    public int Id { get; set; }
    public string Main { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}

public class MainInfo
{
    public float temp { get; set; }
    public float FeelsLike { get; set; }
    public float TempMin { get; set; }
    public float TempMax { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
}

public class WindInfo
{
    public float Speed { get; set; }
    public int Deg { get; set; }
    public float Gust { get; set; }
}

public class CloudsInfo
{
    public int All { get; set; }
}

public class SysInfo
{
    public int Type { get; set; }
    public int Id { get; set; }
    public string Country { get; set; }
    public long Sunrise { get; set; }
    public long Sunset { get; set; }
}
