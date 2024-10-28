using Abp.Auditing;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.Users;

namespace XStudio.App.Models.Users
{
    public class User
    {
        public User() { }

        public void SetUser(User user) { 
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
        public virtual Guid? TenantId { get; protected set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public virtual string UserName { get; protected internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        [DisableAuditing]
        public virtual string NormalizedUserName { get; protected internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Name for the user.
        /// </summary>
        [CanBeNull]
        public virtual string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Surname for the user.
        /// </summary>
        [CanBeNull]
        public virtual string Surname { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        public virtual string Email { get; protected internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        [DisableAuditing]
        public virtual string NormalizedEmail { get; protected internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their email address.
        /// </summary>
        /// <value>True if the email address has been confirmed, otherwise false.</value>
        public virtual bool EmailConfirmed { get; protected internal set; } = false;

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
        public virtual bool IsActive { get; protected internal set; } = false;

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
        public TokenResponse? TokenResponse { get; internal set; } = null!;

    }
}
