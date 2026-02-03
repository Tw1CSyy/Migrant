using Microsoft.EntityFrameworkCore;
using Migrant.Data.Context;

namespace Migrant.Tests.Context
{
    public static class TestDbContextFactory
    {
        public static PassportDbContext Create()
        {
            var options = new DbContextOptionsBuilder<PassportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new PassportDbContext(options);
        }
    }
}
