using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.Users;

namespace XStudio.App.ViewModel.Module.Users {
    public class TokenViewModel : ViewModelBase {
        private string _token = string.Empty;
        private long _expiresIn = 0;
        private string _tokenType = string.Empty;
        private string _refreshToken = string.Empty;

        public string AccessToken {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public string RefreshToken {
            get => _refreshToken;
            set => SetProperty(ref _refreshToken, value);
        }

        public string TokenType {
            get => _tokenType;
            set => SetProperty(ref _tokenType, value);
        }

        public long ExpiresIn {
            get => _expiresIn;
            set => SetProperty(ref _expiresIn, value);
        }

        internal void SetTokenResponse(TokenResponse tokenResponse) {
            AccessToken = tokenResponse.AccessToken;
            RefreshToken = tokenResponse.RefreshToken;
            TokenType = tokenResponse.TokenType;
            ExpiresIn = tokenResponse.ExpiresIn;
        }
    }
}
