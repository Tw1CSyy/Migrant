using Migrant.Application.Options;
using Migrant.Application.Services;
using Migrant.Data.Entities;
using Migrant.Tests.Context;

namespace Migrant.Tests.Tests
{
    public class PassportUpdateServiceTests
    {
        /// <summary>
        /// Тест: добавление новых паспортов
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ShouldAddNewPassports()
        {
            var db = TestDbContextFactory.Create();
            var service = new PassportUpdateService(db);

            var input = new List<PassportKey>
            {
                new("1234", "567890"),
                new("4321", "098765")
            };

            await service.UpdateAsync(input, CancellationToken.None);

            Assert.Equal(2, db.Passports.Count());
        }

        /// <summary>
        /// Тест: удаление паспортов
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RunAsync_ShouldRemoveMissingPassports()
        {
            var db = TestDbContextFactory.Create();

            db.Passports.Add(new Data.Entities.PassportEntity
            {
                Series = "1111",
                Number = "222222",
                IsInactive = false
            });

            await db.SaveChangesAsync();

            var service = new PassportUpdateService(db);
            var input = new List<PassportKey>(); // пустой список

            await service.UpdateAsync(input, CancellationToken.None);

            Assert.Empty(db.Passports);
        }

        /// <summary>
        /// Тест: история изменений
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RunAsync_ShouldSaveChangeHistory()
        {
            var db = TestDbContextFactory.Create();
            var service = new PassportUpdateService(db);

            var input = new List<PassportKey>
            {
                new("1234", "567890")
            };

            await service.UpdateAsync(input, CancellationToken.None);

            var changes = db.PassportChanges.ToList();

            Assert.Single(changes);
            Assert.Equal(PassportChangeType.Added, changes[0].ChangeType);
        }
    }
}
