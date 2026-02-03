using Microsoft.Extensions.Options;
using Migrant.Application.Abstractions;
using Migrant.Application.Options;

namespace Migrant.Application.Services
{
    /// <summary>
    /// Сервис для загрузки исходного файла данных
    /// </summary>
    public class PassportFileSource : IPassportSource
    {
        private readonly PassportUpdateOptions _options;
        private readonly HttpClient _http;
        private readonly ZipExtractor _zip;
        private readonly PassportCsvReader _csv;

        public PassportFileSource(
            IOptions<PassportUpdateOptions> options,
            HttpClient http,
            ZipExtractor zip,
            PassportCsvReader csv)
        {
            _options = options.Value;
            _http = http;
            _zip = zip;
            _csv = csv;
        }

        /// <summary>
        /// Получает исходный файл по пути в конфигурации проекта
        /// </summary>
        public async Task<IReadOnlyCollection<(string, string)>> GetPassportsAsync(
            CancellationToken ct)
        {
            Stream sourceStream;

            if (!string.IsNullOrEmpty(_options.SourcePath))
                sourceStream = File.OpenRead(_options.SourcePath);
            else
                sourceStream = await _http.GetStreamAsync(_options.SourceUrl!, ct);

            using var zipStream = sourceStream;
            using var csv = await _zip.ExtractCsvAsync(zipStream);

            return await _csv.ReadAsync(csv);
        }
    }
}
