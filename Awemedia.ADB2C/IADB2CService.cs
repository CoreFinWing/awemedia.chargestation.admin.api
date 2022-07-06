using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Awemedia.ADB2C
{
    public interface IADB2CService
    {
        Task<Tuple<string, string>> CreateUserWithCustomAttribute(string serializedUser);
        Task DeleteUserById(string userId);
        Task<User> GetUserById(string userId);
        Task<List<User>> ListUsers();
        Task SetPasswordByUserId(string userId, string password);
        Task UpdateUser(string serializedUser, string userId);
    }
}
