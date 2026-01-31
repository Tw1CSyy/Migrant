using Microsoft.EntityFrameworkCore;
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

        public Task<bool> IsInactiveAsync(string series, string number)
        {
            return _db.Passports
                .AsNoTracking()
                .AnyAsync(p =>
                    p.Series == series &&
                    p.Number == number &&
                    p.IsInactive);
        }
    }
}
