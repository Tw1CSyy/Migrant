using Microsoft.EntityFrameworkCore;
using Migrant.Application.Options;
using Migrant.Data.Context;
using Migrant.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task UpdateAsync(
            IReadOnlyCollection<PassportKey> newList,
            CancellationToken ct = default)
        {
            var currentInactive = await _db.Passports
                .Where(p => p.IsInactive)
                .AsNoTracking()
                .ToListAsync(ct);

            var currentSet = currentInactive
                .Select(p => new PassportKey(p.Series, p.Number))
                .ToHashSet();

            var newSet = newList.ToHashSet();
            var updateDate = DateTime.Now;

            // ADD
            var toAdd = newSet.Except(currentSet);
            foreach (var key in toAdd)
            {
                _db.Passports.Add(new PassportEntity
                {
                    Id = Guid.NewGuid(),
                    Series = key.Series,
                    Number = key.Number,
                    IsInactive = true,
                    CreatedAt = updateDate,
                    UpdatedAt = updateDate
                });

                _db.PassportChanges.Add(new PassportChangeEntity
                {
                    Id = Guid.NewGuid(),
                    Series = key.Series,
                    Number = key.Number,
                    ChangeType = PassportChangeType.Added,
                    ChangeDate = updateDate
                });

                _db.PassportStatusHistories.Add(new PassportStatusHistoryEntity
                {
                    Id = Guid.NewGuid(),
                    Series = key.Series,
                    Number = key.Number,
                    IsInactive = true,
                    ChangedAt = updateDate
                });
            }

            // REMOVE
            var toRemove = currentSet.Except(newSet);

            var passports = await _db.Passports
                .Where(p => toRemove.Any(r => r.Series == p.Series && r.Number == p.Number))
                .ToListAsync(ct);

            foreach (var passport in passports)
            {
                passport.IsInactive = false;
                passport.UpdatedAt = updateDate;

                _db.PassportChanges.Add(new PassportChangeEntity
                {
                    Id = Guid.NewGuid(),
                    Series = passport.Series,
                    Number = passport.Number,
                    ChangeType = PassportChangeType.Removed,
                    ChangeDate = updateDate
                });

                _db.PassportStatusHistories.Add(new PassportStatusHistoryEntity
                {
                    Id = Guid.NewGuid(),
                    Series = passport.Series,
                    Number = passport.Number,
                    IsInactive = false,
                    ChangedAt = updateDate
                });
            }

            await _db.SaveChangesAsync(ct);
        }
    }
}
