using Microsoft.Extensions.Options;
using Migrant.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Migrant.Application.Options;

namespace Migrant.Application.Services
{
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
