using JitEvolution.Core.Models.Identity;
using JitEvolution.Core.Repositories.Identity;
using Microsoft.EntityFrameworkCore;

namespace JitEvolution.Data.Repositories.Identity
{
    internal class UserRepository : IUserRepository
    {
        private readonly JitEvolutionDbContext _dbContext;

        public UserRepository(JitEvolutionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<User?> GetByApiKeyAsync(string accessKey) =>
            _dbContext.Set<User>().AsQueryable().SingleOrDefaultAsync(x => x.AccessKey == accessKey);

        public async Task<User> AddAsync(User user)
        {
            await _dbContext.AddAsync(user);

            return user;
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
