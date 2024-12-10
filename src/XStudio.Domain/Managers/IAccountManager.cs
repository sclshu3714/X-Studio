using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace XStudio.Managers
{
    public interface IAccountManager : IDomainService
    {
        string CreateRefreshToken(Guid userId);
        Task<string> GetTokenByUserIdAsync(Guid userId,Action<IdentityUser>? getUserInfo=null);
        Task<bool> LoginValidationAsync(string userName, string password, Action<IdentityUser>? userAction = null);
        Task<bool> RegisterAsync(string userName, string password, long phone,string? nick);
        Task<bool> RestPasswordAsync(Guid userId, string password);
        Task<bool> UpdatePasswordAsync(Guid userId, string newPassword, string oldPassword);
    }
}
