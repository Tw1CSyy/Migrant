using Microsoft.EntityFrameworkCore;
using Migrant.Application.Options;
using Migrant.Data.Context;
using Migrant.Data.Entities;
using EFCore.BulkExtensions;

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
        /// Обновляет базу данных паспортов из исходных данных
        /// </summary>
        /// <param name="newList">Список паспортов из исходных данных</param>
        public async Task UpdateAsync(IReadOnlyCollection<PassportKey> newList, CancellationToken ct = default)
        {
            var updateDate = DateTime.UtcNow;
            var newSet = newList.ToHashSet();

            var currentKeys = await _db.Passports
                .Where(p => p.IsInactive)
                .Select(p => new PassportKey(p.Series, p.Number))
                .ToListAsync(ct);

            var currentSet = currentKeys.ToHashSet();

            var toAdd = newSet.Except(currentSet).ToList();
            var toRemove = currentSet.Except(newSet).ToList();

            // ADD
            if (toAdd.Count > 0)
            {
                var passportsToAdd = toAdd.Select(key => new PassportEntity
                {
                    Id = Guid.NewGuid(),
                    Series = key.Series,
                    Number = key.Number,
                    IsInactive = true,
                    CreatedAt = updateDate,
                    UpdatedAt = updateDate
                }).ToList();

                var changesToAdd = toAdd.Select(key => new PassportChangeEntity
                {
                    Id = Guid.NewGuid(),
                    Series = key.Series,
                    Number = key.Number,
                    ChangeType = PassportChangeType.Added,
                    ChangeDate = updateDate
                }).ToList();

                var historiesToAdd = toAdd.Select(key => new PassportStatusHistoryEntity
                {
                    Id = Guid.NewGuid(),
                    Series = key.Series,
                    Number = key.Number,
                    IsInactive = true,
                    ChangedAt = updateDate
                }).ToList();

                await _db.BulkInsertAsync(passportsToAdd, cancellationToken: ct);
                await _db.BulkInsertAsync(changesToAdd, cancellationToken: ct);
                await _db.BulkInsertAsync(historiesToAdd, cancellationToken: ct);
            }

            // UPDATE
            if (toRemove.Count > 0)
            {
                var passportsToUpdate = await _db.Passports
                    .Where(p => toRemove
                        .Select(r => r.Series + r.Number)
                        .Contains(p.Series + p.Number))
                    .ToListAsync(ct);

                foreach (var passport in passportsToUpdate)
                {
                    passport.IsInactive = false;
                    passport.UpdatedAt = updateDate;
                }

                await _db.BulkUpdateAsync(passportsToUpdate, cancellationToken: ct);

                var changesToAdd = passportsToUpdate.Select(passport =>
                    new PassportChangeEntity
                    {
                        Id = Guid.NewGuid(),
                        Series = passport.Series,
                        Number = passport.Number,
                        ChangeType = PassportChangeType.Removed,
                        ChangeDate = updateDate
                    }).ToList();

                var historiesToAdd = passportsToUpdate.Select(passport =>
                    new PassportStatusHistoryEntity
                    {
                        Id = Guid.NewGuid(),
                        Series = passport.Series,
                        Number = passport.Number,
                        IsInactive = false,
                        ChangedAt = updateDate
                    }).ToList();

                await _db.BulkInsertAsync(changesToAdd, cancellationToken: ct);
                await _db.BulkInsertAsync(historiesToAdd, cancellationToken: ct);
            }
        }
    }
}
