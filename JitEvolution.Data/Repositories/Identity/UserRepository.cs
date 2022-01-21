using JitEvolution.Core.Models.Identity;
using JitEvolution.Core.Repositories.Identity;
using Microsoft.EntityFrameworkCore;

namespace JitEvolution.Data.Repositories.Identity
{
    internal class UserRepository : BaseCrudRepository<User>, IUserRepository
    {
        public UserRepository(JitEvolutionDbContext dbContext) : base(dbContext)
        {
        }

        public Task<User?> GetByApiKeyAsync(string accessKey) =>
            Queryable.SingleOrDefaultAsync(x => x.AccessKey == accessKey);
    }
}
