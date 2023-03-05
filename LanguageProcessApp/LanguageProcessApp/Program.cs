// Call all necassary system libraries
using Newtonsoft.Json;

namespace ChatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Store for Cities to be listed with corresponding coordinates for weather statistics
            var citycoords = new Dictionary<string, (string latitude, string longitude)>
            {
                {"New York", ("40.7128", "-74.0060")},
                {"Los Angeles", ("34.0522", "-118.2437")},
                {"Chicago", ("41.8781", "-87.6298")},
                {"Houston", ("29.7604", "-95.3698")},
                {"Toronto", ("43.6532", "-79.3832")},
                {"London", ("51.5074", "-0.1278")},
                {"Paris", ("48.8566", "2.3522")},
                {"Tokyo", ("35.6895", "139.6917")}
            };

            // Lists Cities available
            Console.WriteLine("You cna find current weather statistics for these Cities!");
            foreach (KeyValuePair<string, (string longitiude, string latitiude)> city in citycoords)
            {
                string cityName = city.Key.ToString();
                Console.WriteLine(cityName);
            }
            
            // Chat bot assks for name and repeats your name back to you
            Console.Write("Hello! I am a chat bot. What's your name? ");
            string name = Console.ReadLine();
            while (name == null || name == "")
            {
                Console.WriteLine("Hello! I am a chat bot. What's your name?");
            }
            Console.WriteLine($"Nice to meet you, {name}!");


            // Chat bot asks many queestions, how you are doing, to which you can respond in Czech or English to get different responses
            while (true)
            {
                Console.Write($"{name}, how can I help you today? ");
                string message = Console.ReadLine().ToLower();

                if (message.Contains("hello") || message.Contains("hi"))
                {
                    Console.WriteLine("Hello there!");
                }
                else if (message.Contains("how are you"))
                {
                    Console.WriteLine("I'm doing well, thank you. How about you?");
                    string msg = Console.ReadLine().ToLower();
                    if (msg.Contains("good") || msg.Contains("decent"))
                    {
                        Console.WriteLine("That's great to hear");
                    }
                    else if (msg.Contains("dobre") || msg.Contains("skvele"))
                    {
                        Console.WriteLine("WOW, you know Czech? That's amazing. Glad you are doing so good");
                    }
                }

                // End the chat bot application
                else if (message.Contains("bye"))
                {
                    Console.WriteLine("Goodbye, have a great day!");
                    break;
                }

                // keyword 'Weather' triggers the chat bot to search for your chosen city, then sends this input to the WeatherStats function with two Longitude and Latitude arguments
                else if (message.Contains("weather")) {
                    // Loops to see if a recognised city appears in user input, if not, the chat bot reiterates and asks what you would like to do
                    foreach (KeyValuePair<string, (string longitiude, string latitiude)> city in citycoords)
                    {
                        string citystrF = city.Key.ToString();
                        string citystr = citystrF.ToLower();
                        if (message.Contains(citystr))
                        {
                            Console.WriteLine(citystrF);
                            var findval = citycoords[citystrF];
                            string citylong = findval.Item1;
                            string citylat = findval.Item1;
                            WeatherStats(citylong, citylat);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("I'm sorry, I didn't understand your message.");
                }
            }
        }

        // Synchronous function waiting to be called on, using a longitude and latitude parameter
        static void WeatherStats(string latitude, string longitude)
        {
            var apiKey = "d804bfa1790d1639faaeb8781b537e28"; // API Key to access openweathermap.org for weather stats

            var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric";
            var httpClient = new HttpClient();

            try
            {
                var response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var responseBody = response.Content.ReadAsStringAsync().Result;
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);

                Console.WriteLine($"Temperature: {weatherData.Main.Temp}°C");
                Console.WriteLine($"Humidity: {weatherData.Main.Humidity}%");
                Console.WriteLine($"Wind speed: {weatherData.Wind.Speed} m/s");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Weather data classes getting data using the API, producing Wind speed, Temperature and Humidity 
        class WeatherData
        {
            public MainData Main { get; set; }
            public WindData Wind { get; set; }
        }

        class MainData
        {
            public float Temp { get; set; }
            public int Humidity { get; set; }
        }

        class WindData
        {
            public float Speed { get; set; }
        }
    }
}