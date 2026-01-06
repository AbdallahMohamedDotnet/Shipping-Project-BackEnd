namespace WebApi.Services
{
    /// <summary>
    /// Service implementation for weather forecast operations
    /// In a real application, this would call an external weather API or database
    /// </summary>
    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastService> _logger;

        public WeatherForecastService(ILogger<WeatherForecastService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get weather forecast for the specified number of days
        /// </summary>
        public async Task<List<WeatherForecast>> GetForecastAsync(int days = 5)
        {
            _logger.LogInformation("Generating {Days} day weather forecast", days);

            if (days < 1 || days > 30)
            {
                throw new ArgumentException("Days must be between 1 and 30", nameof(days));
            }

            // Simulate async operation (e.g., calling external API)
            await Task.Delay(50);

            var forecasts = Enumerable.Range(1, days).Select(index =>
            {
                var temperatureC = Random.Shared.Next(-20, 55);
                var condition = DetermineCondition(temperatureC);

                return new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = temperatureC,
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                    Condition = condition,
                    Humidity = Random.Shared.Next(30, 100),
                    WindSpeed = Math.Round(Random.Shared.NextDouble() * 50, 2)
                };
            }).ToList();

            return forecasts;
        }

        /// <summary>
        /// Get weather forecast for a specific date
        /// </summary>
        public async Task<WeatherForecast?> GetForecastByDateAsync(DateOnly date)
        {
            _logger.LogInformation("Getting weather forecast for date: {Date}", date);

            var today = DateOnly.FromDateTime(DateTime.Now);
            var daysDifference = date.DayNumber - today.DayNumber;

            // Only allow forecasts for the next 30 days
            if (daysDifference < 0 || daysDifference > 30)
            {
                _logger.LogWarning("Requested date {Date} is out of forecast range", date);
                return null;
            }

            await Task.Delay(30);

            var temperatureC = Random.Shared.Next(-20, 55);
            var condition = DetermineCondition(temperatureC);

            return new WeatherForecast
            {
                Date = date,
                TemperatureC = temperatureC,
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Condition = condition,
                Humidity = Random.Shared.Next(30, 100),
                WindSpeed = Math.Round(Random.Shared.NextDouble() * 50, 2)
            };
        }

        /// <summary>
        /// Get weather forecast within a date range
        /// </summary>
        public async Task<List<WeatherForecast>> GetForecastRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            _logger.LogInformation("Getting weather forecast from {StartDate} to {EndDate}", startDate, endDate);

            if (endDate < startDate)
            {
                throw new ArgumentException("End date must be after start date");
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            var daysDifference = endDate.DayNumber - startDate.DayNumber + 1;

            if (daysDifference > 30)
            {
                throw new ArgumentException("Date range cannot exceed 30 days");
            }

            if (startDate < today)
            {
                throw new ArgumentException("Start date cannot be in the past");
            }

            await Task.Delay(50);

            var forecasts = new List<WeatherForecast>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var temperatureC = Random.Shared.Next(-20, 55);
                var condition = DetermineCondition(temperatureC);

                forecasts.Add(new WeatherForecast
                {
                    Date = currentDate,
                    TemperatureC = temperatureC,
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                    Condition = condition,
                    Humidity = Random.Shared.Next(30, 100),
                    WindSpeed = Math.Round(Random.Shared.NextDouble() * 50, 2)
                });

                currentDate = currentDate.AddDays(1);
            }

            return forecasts;
        }

        /// <summary>
        /// Get current weather
        /// </summary>
        public async Task<WeatherForecast> GetCurrentWeatherAsync()
        {
            _logger.LogInformation("Getting current weather");

            await Task.Delay(30);

            var temperatureC = Random.Shared.Next(-20, 55);
            var condition = DetermineCondition(temperatureC);

            return new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = temperatureC,
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Condition = condition,
                Humidity = Random.Shared.Next(30, 100),
                WindSpeed = Math.Round(Random.Shared.NextDouble() * 50, 2)
            };
        }

        /// <summary>
        /// Determine weather condition based on temperature
        /// </summary>
        private static WeatherCondition DetermineCondition(int temperatureC)
        {
            return temperatureC switch
            {
                < 0 => WeatherCondition.Freezing,
                < 10 => WeatherCondition.Cold,
                < 15 => WeatherCondition.Cool,
                < 20 => WeatherCondition.Mild,
                < 25 => WeatherCondition.Warm,
                < 35 => WeatherCondition.Hot,
                _ => WeatherCondition.VeryHot
            };
        }
    }
}
