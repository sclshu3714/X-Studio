using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common
{
    public static class MyTokenExtensionGrantConsts
    {
        public const string GrantType = "MyTokenExtensionGrant";
        public static readonly ImmutableArray<string> Scopes = ImmutableArray.Create("offline_access", "audience");
    }
}
