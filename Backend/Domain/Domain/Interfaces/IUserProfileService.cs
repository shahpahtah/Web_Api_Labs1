using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetUserProfileAsync(Guid id);
        Task<UserProfile> CreateUserProfileAsync(UserProfile profile);
        Task<UserProfile> UpdateUserProfileAsync(UserProfile profile);
        Task<bool> DeleteUserProfileAsync(Guid id);
    }
}
