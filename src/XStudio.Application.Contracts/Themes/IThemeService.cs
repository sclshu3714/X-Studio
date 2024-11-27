using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace XStudio.Themes {
    public interface IThemeService {
        public Task<string?> GetThemeAsync();
        public Task SetThemeAsync(string themeName);
    }
}
