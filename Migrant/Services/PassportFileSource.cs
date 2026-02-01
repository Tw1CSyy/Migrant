using Microsoft.Extensions.Options;
using Migrant.Options;

namespace Migrant.Services
{
    public class PassportFileSource
    {
        private readonly PassportUpdateOptions _options;
        private readonly HttpClient _httpClient;

        public PassportFileSource(
            IOptions<PassportUpdateOptions> options,
            HttpClient httpClient)
        {
            _options = options.Value;
            _httpClient = httpClient;
        }

        public async Task<Stream> GetAsync(CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(_options.SourcePath))
            {
                return File.OpenRead(_options.SourcePath);
            }

            if (!string.IsNullOrEmpty(_options.SourceUrl))
            {
                var response = await _httpClient.GetAsync(_options.SourceUrl, ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync(ct);
            }

            throw new InvalidOperationException("Источник файла не задан");
        }
    }
}
