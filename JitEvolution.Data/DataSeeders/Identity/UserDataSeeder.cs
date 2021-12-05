using JitEvolution.Core.Models.Identity;
using JitEvolution.Data.Constants.Identity;
using Microsoft.EntityFrameworkCore;

namespace JitEvolution.Data.DataSeeders.Identity
{
    internal class UserDataSeeder
    {
        public static void Add(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User
                {
                    Id = UserConstants.SuperUserId,
                    Email = "superuser@linde.ee",
                    UserName = "superuser",
                    NormalizedUserName = "SUPERUSER",
                    NormalizedEmail = "SUPERUSER@LINDE.EE",
                    PasswordHash = "AQAAAAEAACcQAAAAEHsMGzjMkGokHqRjfxWTubUykxdrCkvbl0dt148ZjbUpQjyMiCseYeXZNPoDHj5SEw==",
                    SecurityStamp = "b3af7d63-7742-4060-85ba-dd41e57f6265",
                    ConcurrencyStamp = "aaa20ff6-6bf2-4f3c-897f-6bf35568bf3b",
                    CreatedAt = new DateTime(2021, 12, 5),
                    CreatedById = UserConstants.SuperUserId,
                    ChangedAt = new DateTime(2021, 12, 5),
                    ChangedById = UserConstants.SuperUserId,
                });
        }
    }
}
