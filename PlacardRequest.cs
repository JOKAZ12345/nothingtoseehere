using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SoccerDB
{
    class PlacardRequest : Placard
    {
        public string AllEvents = "https://www.jogossantacasa.pt/WebServices/SBRetailWS/FullSportsBook?apiKey=552CF226909890A044483CECF8196792&channel=7&appClient=Android.Tablet&appVersion=1.0.3";
        public string NextEvents = "https://www.jogossantacasa.pt/WebServices/SBRetailWS/NextEvents?apiKey=552CF226909890A044483CECF8196792&channel=7&appClient=Android.Tablet&appVersion=1.0.3";

        public string FUTEBOL = "FOOT";

        public async Task<IEnumerable<Event>> GetEvents(string link)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(new Uri(link));

            response.EnsureSuccessStatusCode();
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(decompressedStream))
            {
                var d = streamReader.ReadToEnd();

                var o = JObject.Parse(d);
                var events = JsonConvert.DeserializeObject<Placard>(d);

                return events.body.data.exportedProgrammeEntries;
            }
        }

        public async Task<IEnumerable<Event>> GetEvents(string link, string sport)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(new Uri(link));

            response.EnsureSuccessStatusCode();
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(decompressedStream))
            {
                var d = streamReader.ReadToEnd();

                var o = JObject.Parse(d);
                var events = JsonConvert.DeserializeObject<Placard>(d);

                return events.body.data.exportedProgrammeEntries.Where(p => p.sportCode == sport);
            }
        }
    }
}
