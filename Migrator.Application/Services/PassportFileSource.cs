using Microsoft.Extensions.Options;
using Migrant.Application.Abstractions;
using Migrant.Application.Options;
using System.Net.Http.Headers;

namespace Migrant.Application.Services
{
    /// <summary>
    /// Сервис для загрузки исходного файла данных
    /// </summary>
    public class PassportFileSource : IPassportSource
    {
        private readonly PassportUpdateOptions _options;
        private readonly HttpClient _http;

        public PassportFileSource(
            IOptions<PassportUpdateOptions> options,
            HttpClient http)
        {
            _options = options.Value;
            _http = http;
        }

        /// <summary>
        /// Получает исходный файл по пути в конфигурации проекта
        /// </summary>
        public async Task<Stream> GetFileStreamAsync(
            CancellationToken ct)
        {
            Stream sourceStream;

            if (!string.IsNullOrEmpty(_options.SourcePath))
                sourceStream = File.OpenRead(_options.SourcePath);
            else
                sourceStream = await _http.GetStreamAsync(_options.SourceUrl!, ct);

            return sourceStream;
        }
    }
}
