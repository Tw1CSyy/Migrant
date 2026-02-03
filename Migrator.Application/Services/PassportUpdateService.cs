using Microsoft.EntityFrameworkCore;
using Migrant.Data.Context;
using Migrant.Data.Entities;

namespace Migrant.Application.Services
{
    /// <summary>
    /// Сервис для обновления базы данных
    /// </summary>
    public class PassportUpdateService
    {
        private readonly PassportDbContext _db;

        public PassportUpdateService(PassportDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Обновляет базу данных паспартов из исходных данных
        /// </summary>
        /// <param name="newList">Список паспортов из исходных данных</param>
        /// <param name="updateDate">Дата обновления</param>
        public async Task UpdateAsync(
            IReadOnlyCollection<(string Series, string Number)> newList,
            DateTime updateDate,
            CancellationToken ct = default)
        {
            var currentInactive = await _db.Passports
                .Where(p => p.IsInactive)
                .AsNoTracking()
                .ToListAsync(ct);

            var currentSet = currentInactive
                .Select(p => (p.Series, p.Number))
                .ToHashSet();

            var newSet = newList.ToHashSet();

            // ADD
            var toAdd = newSet.Except(currentSet);
            foreach (var (series, number) in toAdd)
            {
                _db.Passports.Add(new PassportEntity
                {
                    Id = Guid.NewGuid(),
                    Series = series,
                    Number = number,
                    IsInactive = true,
                    CreatedAt = updateDate,
                    UpdatedAt = updateDate
                });

                _db.PassportChanges.Add(new PassportChangeEntity
                {
                    Id = Guid.NewGuid(),
                    Series = series,
                    Number = number,
                    ChangeType = PassportChangeType.Added,
                    ChangeDate = updateDate
                });

                _db.PassportStatusHistories.Add(new PassportStatusHistoryEntity
                {
                    Id = Guid.NewGuid(),
                    Series = series,
                    Number = number,
                    IsInactive = true,
                    ChangedAt = updateDate
                });
            }

            // REMOVE
            var toRemove = currentSet.Except(newSet);
            foreach (var (series, number) in toRemove)
            {
                var passport = await _db.Passports
                    .FirstAsync(p => p.Series == series && p.Number == number, ct);

                passport.IsInactive = false;
                passport.UpdatedAt = updateDate;

                _db.PassportChanges.Add(new PassportChangeEntity
                {
                    Id = Guid.NewGuid(),
                    Series = series,
                    Number = number,
                    ChangeType = PassportChangeType.Removed,
                    ChangeDate = updateDate
                });

                _db.PassportStatusHistories.Add(new PassportStatusHistoryEntity
                {
                    Id = Guid.NewGuid(),
                    Series = series,
                    Number = number,
                    IsInactive = false,
                    ChangedAt = updateDate
                });
            }

            await _db.SaveChangesAsync(ct);
        }
    }
}
