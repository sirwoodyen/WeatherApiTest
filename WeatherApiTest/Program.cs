using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
//using Newtonsoft.Json;
using System.Text.Json;
//using System.Text.Json.Serialization;


namespace WeatherApiTest
{
    class Program
    {
       public static void Main(string[] args)
        {
            string location = " ";
            while (true) 
            { 
                try
                {
                while (true) 
                {
                    
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("program to check the weather API...");
                    Console.Write($"Enter a place: ");
                    location = GetLocationName();
                    Console.WriteLine("\n\n");
                    
                    Console.WriteLine($"you typed: {location}");
                    Console.WriteLine("\n");

                    GetWeather(location);
                    Console.ReadKey();
                    break;
                }
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("That place does not exist..... try again..");
                    Console.ReadKey();
                    break;
                }
            }
        }

        public static string GetLocationName()
        {
            string test = Console.ReadLine();
            return test;
        }

        private static async Task GetWeather(string location)
        {
            //string test = "https://api.weatherapi.com/v1/current.json?key=4878b60acf0e40ce9aa215511222903&q=oslo";
            string uri = "https://api.weatherapi.com/v1/current.json?key=";
            string key = "4878b60acf0e40ce9aa215511222903";
            string response_str = uri + key + "&q=" + location;

            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("starting HTTP client...");//test string
                //Http client to send and recieve data
                HttpClient client = new HttpClient();
                Console.WriteLine("checking uri...");//test string
                var httpResponsmessage = await client.GetAsync(response_str);
                //read string from the response content
                Console.WriteLine("uri sent...");//test string

                string jsonResponse = await httpResponsmessage.Content.ReadAsStringAsync();
                if (jsonResponse.Contains("error"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Youre input does not exist, and contains an error....");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Press any key to try again");
                    return;
                }
                

                Console.WriteLine("response recieved...");//test string
                Console.WriteLine("The response Length is: " + jsonResponse.Length); //test string
                Console.WriteLine("\n\n");
            
                
                WeatherInfo.root weatherForecast = JsonSerializer.Deserialize<WeatherInfo.root>(jsonResponse);
               
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"The forecast in {location} in {weatherForecast.location.country}");
                if (weatherForecast.current == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Deserialization success.....Loading Failed...\n \n");//test string
                }
                else
                {
                    Console.WriteLine($"Local Time: {weatherForecast.location.localtime} \n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Temperature: {weatherForecast.current.temp_c} Celcius");
                    Console.WriteLine($"Humidity: {weatherForecast.current.humidity}%");

                    Console.WriteLine("\nDeserialization success!! \n \n"); //test string

                }

                if (jsonResponse != null && weatherForecast.current.temp_c == null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("The Loading might have failed, but we got the json....");
                    Console.WriteLine("Whole output of Json api... \n");
                    Console.WriteLine(jsonResponse);
                }
                
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("We tried.... we failed...");
                Console.ReadKey();
            }

        }
    }
    class WeatherInfo
    {
        public class current
        {
            //[JsonProperty("temp_c")]
            public decimal temp_c { get; set; }
            //[JsonProperty("humidity")]
            public int humidity { get; set; }
        }

        public class location
        {
            public string country { get; set; }
            public string localtime { get; set; }
        }

        public class root
        {
            public current current { get; set; }
            public location location { get; set;}
        }
    }
}
