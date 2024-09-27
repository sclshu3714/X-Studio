using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace XStudio.Identity
{
    public class XStudioUserManager : IdentityUserManager
    {
        private readonly IIdentityUserRepository _userRepository;
        public XStudioUserManager(IdentityUserStore store, 
            IIdentityRoleRepository roleRepository, 
            IIdentityUserRepository userRepository, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<Volo.Abp.Identity.IdentityUser> passwordHasher, 
            IEnumerable<IUserValidator<Volo.Abp.Identity.IdentityUser>> userValidators, 
            IEnumerable<IPasswordValidator<Volo.Abp.Identity.IdentityUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<IdentityUserManager> logger, 
            ICancellationTokenProvider cancellationTokenProvider, 
            IOrganizationUnitRepository organizationUnitRepository, 
            ISettingProvider settingProvider, 
            IDistributedEventBus distributedEventBus, 
            IIdentityLinkUserRepository identityLinkUserRepository, 
            IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache) 
            : base(store, roleRepository, userRepository, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger, cancellationTokenProvider, organizationUnitRepository, settingProvider, distributedEventBus, identityLinkUserRepository, dynamicClaimCache)
        {
            _userRepository = userRepository;
        }

        public override IQueryable<Volo.Abp.Identity.IdentityUser> Users
        {
            get {
                var queryableStore = Store as IQueryableUserStore<Volo.Abp.Identity.IdentityUser>;
                if (queryableStore == null)
                {
                    throw new NotSupportedException("StoreNotIQueryableUserStore)");
                }
                return queryableStore.Users;
            }
        }
    }
}
