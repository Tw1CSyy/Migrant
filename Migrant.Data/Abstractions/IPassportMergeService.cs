namespace Migrant.Data.Abstractions
{
    public interface IPassportMergeService
    {
        public Task MergeAsync(CancellationToken ct);
    }
}
