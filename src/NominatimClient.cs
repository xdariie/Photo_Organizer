using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Photo_Organization
{
    class NominatimClient
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<(string Country, string City)> GetLocationAsync(double latitude, double longitude)
        {
            try
            {
                string url = $"htttps://nominatim.openstreetmap.org/reverse" + $"?lat={latitude}&lon={longitude}&format=json";

                client.DefaultRequestHeaders.UserAgent.ParseAdd("PhotoOrganizerProject");

                string response = await client.GetStringAsync(url);
                using JsonDocument json = JsonDocument.Parse(response);
                JsonElement address = json.RootElement.GetProperty("address");

                string country = address.TryGetProperty("country", out JsonElement c) ? c.GetString() ?? "unknown" : "unknown";

                string city = address.TryGetProperty("city", out JsonElement cityElem) ? cityElem.GetString() ?? "unknown" :
                    address.TryGetProperty("town", out JsonElement townElem) ? townElem.GetString() ?? "unknown" :
                    address.TryGetProperty("village", out JsonElement villageElem) ? villageElem.GetString() ?? "unknown" : "unknown";

                return (country, city);

            }
            catch
            {
                return ("unknown", "unknown");
            }
        }
    }
}
