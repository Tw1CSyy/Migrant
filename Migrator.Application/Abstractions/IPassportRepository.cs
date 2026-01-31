using Migrant.Data.Entities;

namespace Migrant.Application.Abstractions
{
    public interface IPassportRepository
    {
        Task<PassportEntity?> GetAsync(string series, string number);
        Task<List<PassportEntity>> GetAllInactiveAsync();

        Task AddAsync(PassportEntity passport);
        Task UpdateAsync(PassportEntity passport);
    }
}
