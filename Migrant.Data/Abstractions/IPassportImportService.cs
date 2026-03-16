namespace Migrant.Data.Abstractions
{
    public interface IPassportImportService
    {
        public Task ImportAsync(Stream zipStream, CancellationToken ct);
    }
}
