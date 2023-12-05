using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace osu_tops.Functionality {
    public class API {
        private readonly Settings settings;

        public API(Settings settings) {

            this.settings = settings;
        }

        public async Task<string?> getTopScores(string userID)
        {

            //Convert to UTF-8 format, replace non-letter and non-numerical characters
            string encodedUserID = WebUtility.UrlEncode(userID);
            string url = $"https://osu.ppy.sh/api/get_user_best?k={settings.key}&m=0&u={encodedUserID}&limit=100";
            string? response = await GetJsonResponse(url);

            return response;
        }
/*
        public async Task<string?> getBeatmap(string beatmapID) {
            return null;
        }

        public async Task<string?> getBeatmapTopScores(String beatmapID, int mods) {
            return null;
        }
*/
        private async Task<string?> GetJsonResponse(string url) {

            try {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage httpResponse = await client.GetAsync(url);
                    httpResponse.EnsureSuccessStatusCode();
                    return await httpResponse.Content.ReadAsStringAsync();
                }
            }
            catch(HttpRequestException ex) {
                Console.WriteLine($"HTTP request error. {ex.Message}");
                return null;
            }
            catch(Exception ex) {
                Console.WriteLine($"Failed to retrieve response. {ex.Message}");
                return null;
            }
        }
    }
}