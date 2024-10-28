using Abp.Auditing;
using Castle.Components.DictionaryAdapter;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.App.ViewModel.Module.Users;
using XStudio.Users;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace XStudio.App.ViewModel.Users {
    public class UserViewModel : ViewModelBase {
        private Guid? _tenantId = Guid.Empty;
        private string _userName = string.Empty;
        private string _normalizedUserName = string.Empty;
        private string _name = string.Empty;
        private string _surname = string.Empty;
        private string _email = string.Empty;
        private string _normalizedEmail = string.Empty;
        private bool _emailConfirmed = false;
        private bool _isActive = false;
        private TokenViewModel _tokenViewModel = new TokenViewModel();

        public UserViewModel() { }

        public void SetUser(UserViewModel user) {
            TenantId = user.TenantId;
            UserName = user.UserName;
            NormalizedUserName = user.NormalizedUserName;
            Name = user.Name;
            Surname = user.Surname;
            Email = user.Email;
            NormalizedEmail = user.NormalizedEmail;
            EmailConfirmed = user.EmailConfirmed;
            PasswordHash = user.PasswordHash;
            SecurityStamp = user.SecurityStamp;
            IsExternal = user.IsExternal;
            PhoneNumber = user.PhoneNumber;
            PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            IsActive = user.IsActive;
            TwoFactorEnabled = user.TwoFactorEnabled;
            LockoutEnd = user.LockoutEnd;
            LockoutEnabled = user.LockoutEnabled;
            AccessFailedCount = user.AccessFailedCount;
            ShouldChangePasswordOnNextLogin = user.ShouldChangePasswordOnNextLogin;
            EntityVersion = user.EntityVersion;
            LastPasswordChangeTime = user.LastPasswordChangeTime;
            Roles = user.Roles;
            Claims = user.Claims;
            Logins = user.Logins;
            Tokens = user.Tokens;
            OrganizationUnits = user.OrganizationUnits;
            TokenResponse = user.TokenResponse;
        }
        public virtual Guid? TenantId {
            get => _tenantId;
            protected set => SetProperty(ref _tenantId, value);
        }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public virtual string UserName {
            get => _userName;
            protected internal set => SetProperty(ref _userName, value);
        }

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        [DisableAuditing]
        public virtual string NormalizedUserName {
            get => _normalizedUserName;
            protected internal set => SetProperty(ref _normalizedUserName, value);
        }

        /// <summary>
        /// Gets or sets the Name for the user.
        /// </summary>
        [CanBeNull]
        public virtual string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets or sets the Surname for the user.
        /// </summary>
        [CanBeNull]
        public virtual string Surname {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        public virtual string Email {
            get => _email;
            protected internal set => SetProperty(ref _email, value);
        }

        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        [DisableAuditing]
        public virtual string NormalizedEmail {
            get => _normalizedEmail;
            protected internal set => SetProperty(ref _normalizedEmail, value);
        }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their email address.
        /// </summary>
        /// <value>True if the email address has been confirmed, otherwise false.</value>
        public virtual bool EmailConfirmed {
            get => _emailConfirmed;
            protected internal set => SetProperty(ref _emailConfirmed, value);
        }

        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        [DisableAuditing]
        public virtual string PasswordHash { get; protected internal set; } = string.Empty;

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed)
        /// </summary>
        [DisableAuditing]
        public virtual string SecurityStamp { get; protected internal set; } = string.Empty;

        public virtual bool IsExternal { get; set; } = false;

        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        [CanBeNull]
        public virtual string PhoneNumber { get; protected internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their telephone address.
        /// </summary>
        /// <value>True if the telephone number has been confirmed, otherwise false.</value>
        public virtual bool PhoneNumberConfirmed { get; protected internal set; } = false;

        /// <summary>
        /// Gets or sets a flag indicating if the user is active.
        /// </summary>
        public virtual bool IsActive {
            get => _isActive;
            protected internal set => SetProperty(ref _isActive, value);
        }

        /// <summary>
        /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
        /// </summary>
        /// <value>True if 2fa is enabled, otherwise false.</value>
        public virtual bool TwoFactorEnabled { get; protected internal set; } = false;

        /// <summary>
        /// Gets or sets the date and time, in UTC, when any user lockout ends.
        /// </summary>
        /// <remarks>
        /// A value in the past means the user is not locked out.
        /// </remarks>
        public virtual DateTimeOffset? LockoutEnd { get; protected internal set; } = null;

        /// <summary>
        /// Gets or sets a flag indicating if the user could be locked out.
        /// </summary>
        /// <value>True if the user could be locked out, otherwise false.</value>
        public virtual bool LockoutEnabled { get; protected internal set; } = false;

        /// <summary>
        /// Gets or sets the number of failed login attempts for the current user.
        /// </summary>
        public virtual int AccessFailedCount { get; protected internal set; } = 0;

        /// <summary>
        /// Should change password on next login.
        /// </summary>
        public virtual bool ShouldChangePasswordOnNextLogin { get; protected internal set; } = false;

        /// <summary>
        /// A version value that is increased whenever the entity is changed.
        /// </summary>
        public virtual int EntityVersion { get; protected set; } = 0;

        /// <summary>
        /// Gets or sets the last password change time for the user.
        /// </summary>
        public virtual DateTimeOffset? LastPasswordChangeTime { get; protected set; } = null;

        //TODO: Can we make collections readonly collection, which will provide encapsulation. But... can work for all ORMs?

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<object> Roles { get; protected set; } = new List<object>();

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<object> Claims { get; protected set; } = new List<object>();

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<object> Logins { get; protected set; } = new List<object>();

        /// <summary>
        /// Navigation property for this users tokens.
        /// </summary>
        public virtual ICollection<object> Tokens { get; protected set; } = new List<object>();

        /// <summary>
        /// Navigation property for this organization units.
        /// </summary>
        public virtual ICollection<object> OrganizationUnits { get; protected set; } = new List<object>();

        /// <summary>
        /// token
        /// </summary>
        public TokenViewModel TokenResponse {
            get => _tokenViewModel;
            internal set => SetProperty(ref _tokenViewModel, value);
        }
    }
}
