using Microsoft.EntityFrameworkCore;
using Migrant.Application.DTOs;
using Migrant.Data.Context;

namespace Migrant.Application.Services
{
    public class PassportQueryService
    {
        private readonly PassportDbContext _db;

        public PassportQueryService(PassportDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Поиск неактивного паспорта
        /// </summary>
        /// <param name="series">Серия паспорта</param>
        /// <param name="number">Номер паспорта</param>
        public async Task<PassportStatusDto?> FindAsync(string series, string number, CancellationToken ct)
        {
            var exists = await _db.Passports
                .AsNoTracking()
                .AnyAsync(p =>
                    p.Series == series &&
                    p.Number == number,
                    ct);

            return new PassportStatusDto
            {
                Series = series,
                Number = number,
                IsInactive = exists
            };
        }

        /// <summary>
        /// Получение изменений за дату
        /// </summary>
        /// <param name="date">Дата, для которой ищем изменения</param>
        public async Task<PassportChangesDto?> GetChangesAsync(DateTime date, CancellationToken ct)
        {
            var changes = await _db.PassportChanges
                .AsNoTracking()
                .Where(c => c.ChangeDate.Date == date.Date)
                .ToListAsync(ct);

            if (!changes.Any())
                return null;

            return new PassportChangesDto
            {
                Date = date.Date,
                Added = changes
                    .Where(c => c.ChangeType == Data.Entities.PassportChangeType.Added)
                    .Select(c => c.Series + c.Number)
                    .ToList(),
                Removed = changes
                    .Where(c => c.ChangeType == Data.Entities.PassportChangeType.Removed)
                    .Select(c => c.Series + c.Number)
                    .ToList()
            };
        }

        /// <summary>
        /// Возвращает данные истории активности / неактивности паспорта
        /// </summary>
        /// <param name="series">Серия паспорта</param>
        /// <param name="number">Номер паспорта</param>
        public async Task<PassportHistoryDto?> GetHistoryAsync(string series, string number, CancellationToken ct)
        {
            var events = await _db.PassportChanges
                .AsNoTracking()
                .Where(c => c.Series == series && c.Number == number)
                .OrderBy(c => c.ChangeDate)
                .ToListAsync(ct);

            if (!events.Any())
                return null;

            return new PassportHistoryDto
            {
                Series = series,
                Number = number,
                History = events.Select(e => new PassportHistoryItemDto
                {
                    Date = e.ChangeDate,
                    IsInactive = e.ChangeType == Data.Entities.PassportChangeType.Added
                }).ToList()
            };
        }
    }
}
