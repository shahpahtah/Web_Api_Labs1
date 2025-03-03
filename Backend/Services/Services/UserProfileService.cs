using Data.EF;
using Domain;
using Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
namespace Services
{
    public class UserProfileService : IUserProfileService

    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public Task<UserProfile> CreateUserProfileAsync(UserProfile profile)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserProfileAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfile> GetUserProfileAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfile> UpdateUserProfileAsync(UserProfile profile)
        {
            throw new NotImplementedException();
        }
    }
}
