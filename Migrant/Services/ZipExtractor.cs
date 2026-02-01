using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrant.Services
{
    public class ZipExtractor
    {
        public async Task<Stream> ExtractCsvAsync(Stream zipStream)
        {
            using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

            var csvEntry = archive.Entries.First(e => e.Name.EndsWith(".csv"));

            var memory = new MemoryStream();
            await using var entryStream = csvEntry.Open();
            await entryStream.CopyToAsync(memory);

            memory.Position = 0;
            return memory;
        }
    }
}
