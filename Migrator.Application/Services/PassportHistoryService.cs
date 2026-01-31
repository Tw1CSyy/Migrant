using Microsoft.EntityFrameworkCore;
using Migrant.Data.Context;
using Migrant.Application.DTOs;

namespace Migrant.Application.Services
{
    public class PassportHistoryService
    {
        private readonly PassportDbContext _db;

        public PassportHistoryService(PassportDbContext db)
        {
            _db = db;
        }

        public async Task<List<PassportHistoryDto>> GetHistoryAsync(
            string series,
            string number)
        {
            return await _db.PassportStatusHistories
                .AsNoTracking()
                .Where(h => h.Series == series && h.Number == number)
                .OrderBy(h => h.ChangedAt)
                .Select(h => new PassportHistoryDto(
                    h.IsInactive,
                    h.ChangedAt))
                .ToListAsync();
        }
    }
}
