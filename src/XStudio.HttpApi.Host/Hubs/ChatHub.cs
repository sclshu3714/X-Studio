using Confluent.Kafka;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace XStudio.Hubs
{
    /// <summary>
    /// SignalR
    /// </summary>
    public class ChatHub : AbpHub
    {
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly ILookupNormalizer _lookupNormalizer;

        public ChatHub(IIdentityUserRepository identityUserRepository, ILookupNormalizer lookupNormalizer)
        {
            _identityUserRepository = identityUserRepository;
            _lookupNormalizer = lookupNormalizer;
        }

        public async Task SendMessage(string targetUserName, string message)
        {
            var targetUser = await _identityUserRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName(targetUserName));

            message = $"{CurrentUser.UserName}: {message}";

            await Clients
                .User(targetUser.Id.ToString())
                .SendAsync("ReceiveMessage", message);
        }
    }
}
