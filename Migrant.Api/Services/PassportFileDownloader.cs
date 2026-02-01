using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Api.Services
{
    public class PassportFileDownloader
    {
        private readonly HttpClient _httpClient;

        public PassportFileDownloader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Stream> DownloadAsync(string url, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync(ct);
        }
    }
}
