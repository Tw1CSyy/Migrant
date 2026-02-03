using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Services
{
    /// <summary>
    /// Сервис для загрущки исходного файла с данными
    /// </summary>
    public class PassportFileDownloader
    {
        private readonly HttpClient _httpClient;

        public PassportFileDownloader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Загружает файл из расположения и возвращает Task<Stream>
        /// </summary>
        /// <param name="url">Расположение файла</param>
        /// <returns>Task<Stream> с данными из файла</returns>
        public async Task<Stream> DownloadAsync(string url, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync(ct);
        }
    }
}
